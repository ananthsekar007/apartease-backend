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

namespace apartease_backend.Controllers
{
    [Route("api/resident")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        public ResidentController(ApartEaseContext context)
        {
            _context = context;
        }

        //GET: api/resident/get/active/{managerId}

        [HttpGet("get/active/{managerId}")]
        public async Task<ActionResult<IEnumerable<Resident>>> GetActiveResidents(int managerId)
        {
            Manager existingManager = await _context.Manager.FindAsync(managerId);
            if (existingManager == null) return BadRequest("There is no manager with the provided information!");

            Apartment apartmentOfManager = await _context.Apartment.FirstOrDefaultAsync(a => a.ManagerId == managerId);

            if (apartmentOfManager == null) return BadRequest("There is no apartment associated with the manager!");

            IEnumerable<Resident> activeResidents = await _context.Resident.Where(r => r.ApartmentId == apartmentOfManager.ApartmentId && r.IsActive == true).ToListAsync();

            return Ok(activeResidents);
        }

        [HttpGet("get/inactive/{managerId}")]

        public async Task<ActionResult<IEnumerable<Resident>>> GetInActiveResidents(int managerId)
        {
            Manager existingManager = await _context.Manager.FindAsync(managerId);
            if (existingManager == null) return BadRequest("There is no manager with the provided information!");

            Apartment apartmentOfManager = await _context.Apartment.FirstOrDefaultAsync(a => a.ManagerId == managerId);

            if (apartmentOfManager == null) return BadRequest("There is no apartment associated with the manager!");

            IEnumerable<Resident> activeResidents = await _context.Resident.Where(r => r.ApartmentId == apartmentOfManager.ApartmentId && r.IsActive == false).ToListAsync();

            return Ok(activeResidents);
        }

        [HttpPut("change-status")]
        public async Task<ActionResult<string>> UpdateResidentStatus([FromBody] ResidentStatusUpdate residentStatusUpdate)
        {
            Resident existingResident = await _context.Resident.FindAsync(residentStatusUpdate.ResidentId);

            if (existingResident == null) return BadRequest("No resident found with the provided information");

            existingResident.IsActive = residentStatusUpdate.Status;

            await _context.SaveChangesAsync();

            return Ok("Status Updated Successfully");
        }

    }
}
