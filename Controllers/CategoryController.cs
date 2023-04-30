using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Dao.CategoryDao;

namespace apartease_backend.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        public CategoryController(ApartEaseContext context)
        {
            _context = context;
        }

        // GET: api/category/get
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
          if (_context.Category == null)
          {
              return NotFound();
          }
            return await _context.Category.ToListAsync();
        }

        // POST: api/category/add
        [HttpPost("add")]
        public async Task<ActionResult<string>> AddCategory(AddCategoryInput category)
        {
          if (_context.Category == null)
          {
              return Problem("Entity set 'ApartEaseContext.Category'  is null.");
          }
          Category newCategory = new Category()
          {
              CategoryName = category.CategoryName,
          };
            _context.Category.Add(newCategory);
            await _context.SaveChangesAsync();

            return Ok("Category Created!");
        }
    }
}
