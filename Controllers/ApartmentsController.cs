using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;

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
    }
}
