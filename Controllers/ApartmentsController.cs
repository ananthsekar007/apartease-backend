using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Dao.ApartmentDao;

namespace apartease_backend.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        public ApartmentsController(ApartEaseContext context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Apartment>>> GetApartments()
        {
            if (_context.Apartment == null)
            {
                return NotFound();
            }
            return await _context.Apartment.ToListAsync();
        }

        [HttpGet("get/{managerId}")]
        public async Task<ActionResult<Apartment>> GetApartmentWithManager(int managerId)
        {
            Apartment existingApartment = await _context.Apartment.Include(a => a.Manager).FirstOrDefaultAsync(a => a.ManagerId == managerId);
            if (existingApartment == null) return BadRequest("Apartment does not exist!");
            return Ok(existingApartment);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Apartment>> AddApartment([FromBody] AddApartmentInput addApartmentInput)
        {

            Apartment existingApartmentWithManager = await _context.Apartment.FirstOrDefaultAsync(a => a.ManagerId == addApartmentInput.ManagerId); ;

            if (existingApartmentWithManager != null) return BadRequest("Manager is already assoicated with an apartment!");

            Apartment existingApartmentWithDetails = await _context.Apartment.FirstOrDefaultAsync(a => a.Name == addApartmentInput.Name && a.Zip == addApartmentInput.Zip);

            if (existingApartmentWithDetails != null) return BadRequest("Apartment with same details already exist!");

            Apartment newApartment = new Apartment()
            {
                ManagerId = addApartmentInput.ManagerId,
                City = addApartmentInput.City,
                State = addApartmentInput.State,
                Name = addApartmentInput.Name,
                Street = addApartmentInput.Street,
                Zip = addApartmentInput.Zip,
            };

            try
            {
                await _context.Apartment.AddAsync(newApartment);
                await _context.SaveChangesAsync();

                return Ok(newApartment);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again!");
            }
        }
    }
}
