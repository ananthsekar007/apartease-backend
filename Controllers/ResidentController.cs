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
using apartease_backend.Services.ResidentService;
using apartease_backend.Dao;
using Microsoft.AspNetCore.Authorization;

namespace apartease_backend.Controllers
{
    [Route("api/resident")]
    [ApiController]
    [Authorize]
    public class ResidentController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        private readonly IResidentService _residentService;

        public ResidentController(ApartEaseContext context, IResidentService residentService)
        {
            _context = context;
            _residentService = residentService;
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

        [HttpGet("get/status/{residentId}")]
        public async Task<ActionResult<bool>> GetResidentStatus(int residentId)
        {
            ServiceResponse<bool> response = await _residentService.GetActivityStatus(residentId);

            if(response.Error != null) return BadRequest(response.Error);

            return Ok(response.Data);
        }

    }
}
