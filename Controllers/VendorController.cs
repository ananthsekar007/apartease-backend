using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Dao.VendorDao;
using apartease_backend.Helpers;
using apartease_backend.Dao.ManagerDao;
using apartease_backend.Dao;

namespace apartease_backend.Controllers
{
    [Route("api/auth/vendor")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly ApartEaseContext _context;
        private readonly IConfiguration _configuration;

        public VendorController(IConfiguration configuration, ApartEaseContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<VendorAuthResponse>> VendorSignIn([FromBody] VendorAuthInput vendorAuthInput)
        {
            if (_context.Vendor == null) return NotFound();

            Vendor vendorWithEmail = await _context.Vendor.FirstOrDefaultAsync(x => x.Email == vendorAuthInput.Email);

            if(vendorWithEmail != null) return BadRequest("Vendor with same email already exists!");

            Vendor existingVendor = await _context.Vendor.FirstOrDefaultAsync(x => x.CompanyId == vendorAuthInput.CompanyId);

            if (existingVendor != null) return BadRequest("A vendor is already registered with the company!");

            PasswordHelper passwordHelper = new PasswordHelper();

            passwordHelper.CreatePasswordHash(vendorAuthInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Vendor newVendor = new Vendor()
            {
                CompanyId = vendorAuthInput.CompanyId,
                City = vendorAuthInput.City,
                Email = vendorAuthInput.Email,
                FirstName = vendorAuthInput.FirstName,
                LastName = vendorAuthInput.LastName,
                IsActive = false,
                PhoneNumber = vendorAuthInput.PhoneNumber,
                State = vendorAuthInput.State,
                Street = vendorAuthInput.Street,
                Zip = vendorAuthInput.Zip,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Vendor.Add(newVendor);
            await _context.SaveChangesAsync();


            AppUser appUser = new AppUser()
            {
                Email = newVendor.Email,
                Role = "Vendor"
            };

            JwtHelper jwtHelper = new JwtHelper(_configuration);

            string jwtToken = jwtHelper.CreateToken(appUser);

            VendorAuthResponse response = new VendorAuthResponse()
            {
                Vendor = newVendor,
                AuthToken = jwtToken
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<VendorAuthResponse>> VendorLogin([FromBody] UserLoginInput loginInput)
        {
            if (_context.Vendor == null) return NotFound("Vendor Context not found!");
            Vendor existingVendor =
                await _context.Vendor.FirstOrDefaultAsync(v => v.Email == loginInput.Email);

            if (existingVendor == null) return BadRequest("User does not Exist!");

            PasswordHelper passwordHelper = new PasswordHelper();

            if (!passwordHelper.VerifyPasswordHash(loginInput.Password, existingVendor.PasswordHash, existingVendor.PasswordSalt))
            {
                return BadRequest("Please check the login credentials and try again!");
            }

            AppUser appUser = new AppUser()
            {
                Email = existingVendor.Email,
                Role = "Vendor"
            };

            JwtHelper jwtHelper = new JwtHelper(_configuration);

            string jwtToken = jwtHelper.CreateToken(appUser);

            VendorAuthResponse response = new VendorAuthResponse()
            {
                Vendor = existingVendor,
                AuthToken = jwtToken
            };

            return Ok(response);
        }
    }
}
