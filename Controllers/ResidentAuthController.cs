using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Dao.ResidentDao;
using apartease_backend.Helpers;
using apartease_backend.Dao.ManagerDao;
using apartease_backend.Dao;

namespace apartease_backend.Controllers
{
    [Route("api/auth/resident")]
    [ApiController]
    public class ResidentAuthController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        private readonly IConfiguration _configuration;

        public ResidentAuthController(IConfiguration configuration, ApartEaseContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<ResidentAuthResponse>> ResidentSignIn([FromBody] ResidentAuthInput residentAuthInput)
        {
            if (_context.Resident == null) return NotFound("Resident Context not found!");

            Resident existingResident =
                await _context.Resident.FirstOrDefaultAsync(m => m.Email == residentAuthInput.Email);

            if (existingResident != null) return BadRequest("Resident with same email already exists!");

            bool isApartmentExists = await _context.Apartment.AnyAsync(a => a.ApartmentId ==  residentAuthInput.ApartmentId);

            if (!isApartmentExists) return BadRequest("There is no such apartment!");

            PasswordHelper passwordHelper = new PasswordHelper();

            passwordHelper.CreatePasswordHash(residentAuthInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Resident newResident = new Resident()
            {
                FirstName = residentAuthInput.FirstName,
                LastName = residentAuthInput.LastName,
                City = residentAuthInput.City,
                Email = residentAuthInput.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                State = residentAuthInput.State,
                Street = residentAuthInput.Street,
                ApartmentId = residentAuthInput.ApartmentId,
                PhoneNumber = residentAuthInput.PhoneNumber,
                Zip = residentAuthInput.Zip,
            };

            _context.Resident.Add(newResident);
            await _context.SaveChangesAsync();

            AppUser appUser = new AppUser()
            {
                Email = newResident.Email,
                Role = "Manager"
            };

            JwtHelper jwtHelper = new JwtHelper(_configuration);

            string jwtToken = jwtHelper.CreateToken(appUser);

            ResidentAuthResponse response = new ResidentAuthResponse()
            {
                Resident = newResident,
                AuthToken = jwtToken
            };

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<ResidentAuthResponse>> ResidentLogin([FromBody] UserLoginInput userLoginInput)
        {
            if (_context.Resident == null) return NotFound("Resident Context not found!");
            Resident existingResident =
                await _context.Resident.FirstOrDefaultAsync(m => m.Email == userLoginInput.Email);

            if (existingResident == null) return BadRequest("User does not Exist!");

            PasswordHelper passwordHelper = new PasswordHelper();

            if (!passwordHelper.VerifyPasswordHash(userLoginInput.Password, existingResident.PasswordHash, existingResident.PasswordSalt))
            {
                return BadRequest("Please check the login credentials and try again!");
            }

            AppUser appUser = new AppUser()
            {
                Email = existingResident.Email,
                Role = "Resident"
            };

            JwtHelper jwtHelper = new JwtHelper(_configuration);

            string jwtToken = jwtHelper.CreateToken(appUser);

            ResidentAuthResponse response = new ResidentAuthResponse()
            {
                Resident = existingResident,
                AuthToken = jwtToken
            };

            return Ok(response);

        }
    }
 }
