using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Dao;
using Azure.Core;
using apartease_backend.Dao.ManagerDao;
using Microsoft.AspNetCore.Authorization;
using apartease_backend.Services.JwtService;
using apartease_backend.Services.PasswordService;

namespace apartease_backend.Controllers
{
    [Route("api/auth/manager")]
    [ApiController]
    public class ManagerAuthController : ControllerBase
    {
        private readonly ApartEaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public ManagerAuthController(IConfiguration configuration, ApartEaseContext context, IJwtService jwtService, IPasswordService passwordService)
        {
            _context = context;
            _configuration = configuration;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<ManagerAuthResponse>> ManagerSignIn([FromBody] ManagerAuthInput managerInput)
        {
            if (_context.Manager == null) return NotFound("Manager Context not found!");

            Manager existingManager = 
                await _context.Manager.FirstOrDefaultAsync(m => m.Email ==  managerInput.Email);

            if (existingManager != null) return BadRequest("User with same email already exists!");

            _passwordService.CreatePasswordHash(managerInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Manager newManager = new Manager()
            {
                Email = managerInput.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                City = managerInput.City,
                FirstName = managerInput.FirstName,
                LastName = managerInput.LastName,
                State = managerInput.State,
                Street = managerInput.Street,
                Zip = managerInput.Zip
            };

            _context.Manager.Add(newManager);
            await _context.SaveChangesAsync();


            AppUser appUser = new AppUser()
            {
                Email = newManager.Email,
                Role = "Manager"
            };

            string jwtToken = _jwtService.CreateToken(appUser);

            ManagerAuthResponse response = new ManagerAuthResponse()
            {
                Manager = newManager,
                AuthToken = jwtToken
            };

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<ManagerAuthResponse>> ManagerLogin([FromBody] UserLoginInput loginInput)
        {
            if (_context.Manager == null) return NotFound("Manager Context not found!");
            Manager existingManager =
                await _context.Manager.FirstOrDefaultAsync(m => m.Email == loginInput.Email);

            if (existingManager == null) return BadRequest("User does not Exist!");

            if (!_passwordService.VerifyPasswordHash(loginInput.Password, existingManager.PasswordHash, existingManager.PasswordSalt))
            {
                return BadRequest("Please check the login credentials and try again!");
            }

            AppUser appUser = new AppUser()
            {
                Email = existingManager.Email,
                Role = "Manager"
            };

            string jwtToken = _jwtService.CreateToken(appUser);

            ManagerAuthResponse response = new ManagerAuthResponse()
            {
                Manager = existingManager,
                AuthToken = jwtToken
            };

            return Ok(response);

        }
    }
}
