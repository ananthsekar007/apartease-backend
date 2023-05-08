using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Services.VendorService;

namespace apartease_backend.Controllers
{
    [Route("api/vendor")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors() { 
            IEnumerable<Vendor> vendors = await _vendorService.GetVendors();
            return Ok(vendors);
        }

        
    }
}
