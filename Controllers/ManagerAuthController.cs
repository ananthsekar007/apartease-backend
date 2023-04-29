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
using apartease_backend.Helpers;
using Azure.Core;

namespace apartease_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerAuthController : ControllerBase
    {
        private readonly ApartEaseContext _context;
        private readonly IConfiguration _configuration;

        public ManagerAuthController(IConfiguration configuration, ApartEaseContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<ManagerAuthResponse>> ManagerSignIn([FromBody] ManagerAuthInput managerInput)
        {
            if (_context.Manager == null) return NotFound("Manager Context not found!");

            Manager existingManager = 
                await _context.Manager.FirstOrDefaultAsync(m => m.Email ==  managerInput.Email);

            if (existingManager != null) return BadRequest("User with same email already exists!");

            PasswordHelper passwordHelper = new PasswordHelper();

            passwordHelper.CreatePasswordHash(managerInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

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

            JwtHelper jwtHelper = new JwtHelper(_configuration);

            string jwtToken = jwtHelper.CreateToken(appUser);

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

            PasswordHelper passwordHelper = new PasswordHelper();

            if (!passwordHelper.VerifyPasswordHash(loginInput.Password, existingManager.PasswordHash, existingManager.PasswordSalt))
            {
                return BadRequest("Please check the login credentials and try again!");
            }

            AppUser appUser = new AppUser()
            {
                Email = existingManager.Email,
                Role = "Manager"
            };

            JwtHelper jwtHelper = new JwtHelper(_configuration);

            string jwtToken = jwtHelper.CreateToken(appUser);

            ManagerAuthResponse response = new ManagerAuthResponse()
            {
                Manager = existingManager,
                AuthToken = jwtToken
            };

            return Ok(response);

        }
    }
}
