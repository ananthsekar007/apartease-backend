﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Services.AmenityBookingService;
using apartease_backend.Dao.ApartmentBookingDao;
using apartease_backend.Dao;
using Microsoft.AspNetCore.Authorization;

namespace apartease_backend.Controllers
{
    [Route("api/amenitybooking")]
    [ApiController]
    [Authorize(Roles = "Manager, Resident")]
    public class AmenityBookingController : ControllerBase
    {
        private readonly ApartEaseContext _context;
        private readonly IAmenityBookingService _amenityBookingService;

        public AmenityBookingController(ApartEaseContext context, IAmenityBookingService amenityBookingService)
        {
            _context = context;
            _amenityBookingService = amenityBookingService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<string>> BookAmenity(AmenityBookingInput amenityBookingInput)
        {
            ServiceResponse<string> response = await _amenityBookingService.BookAmenity(amenityBookingInput);

            if(response.Error != null) return BadRequest(response.Error);

            return Ok(response.Data);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<string>> UpdateAmenityBooking(AmenityBookingInput amenityBookingInput)
        {
            ServiceResponse<string> response = await _amenityBookingService.UpdateBooking(amenityBookingInput);

            if (response.Error != null) return BadRequest(response.Error);

            return Ok(response.Data);
        }

        [HttpGet("get-for-resident/{residentId}")]
        public async Task<ActionResult<AmenityBooking>> GetAmenitiesForResident(int residentId)
        {
            IEnumerable<AmenityBooking> response = await _amenityBookingService.GetAmenitiesForResident(residentId);

            return Ok(response);
        }

        [HttpGet("get-for-manager/{managerId}")]
        public async Task<ActionResult<AmenityBooking>> GetAmenitiesForManager(int managerId)
        {
            IEnumerable<AmenityBooking> response = await _amenityBookingService.GetBookingsForManager(managerId);

            return Ok(response);
        }

    }
}
