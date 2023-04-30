using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Dao.CompanyDao;

namespace apartease_backend.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        public CompanyController(ApartEaseContext context)
        {
            _context = context;
        }

        // GET: api/company/get
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        {
          if (_context.Company == null)
          {
              return NotFound();
          }
            return await _context.Company.ToListAsync();
        }

        // GET: api/company/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
          if (_context.Company == null)
          {
              return NotFound();
          }
            var company = await _context.Company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/company/edit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit")]
        public async Task<IActionResult> PutCompany(CompanyDao company)
        {

            Company existingCompany = await _context.Company.FirstOrDefaultAsync(x => x.CompanyId == (int)company.CompanyId);
            if (existingCompany == null) return BadRequest("Company does not exist!");
            
            existingCompany.CompanyZip = company.CompanyZip;
            existingCompany.CompanyName = company.CompanyName;
            existingCompany.CategoryId = company.CategoryId;

            await _context.SaveChangesAsync();

            return Ok("Company Details Modified!");

        }

        // POST: api/company/create
        [HttpPost("create")]
        public async Task<ActionResult<Company>> PostCompany(CompanyDao company)
        {
          if (_context.Company == null)
          {
              return Problem("Entity set 'ApartEaseContext.Company'  is null.");
          }

          Company existingCompany = 
                await _context.Company.
                            FirstOrDefaultAsync(x => x.CompanyName == company.CompanyName && x.CompanyZip == company.CompanyZip);

        if (existingCompany != null) return BadRequest("Company with the same details already Exists!");

            Company newCompany = new Company()
            {
                CompanyName = company.CompanyName,
                CategoryId = company.CategoryId,
                CompanyZip = company.CompanyZip
            };

            _context.Company.Add(newCompany);
            await _context.SaveChangesAsync();

            return newCompany;
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (_context.Company == null)
            {
                return NotFound();
            }
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
