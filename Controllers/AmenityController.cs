using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Services.AmenityService;
using apartease_backend.Dao;
using System.Collections.ObjectModel;
using apartease_backend.Dao.AmenityDao;

namespace apartease_backend.Controllers
{
    [Route("api/amenity")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        //API - api/amenity/get/{apartmentId}

        [HttpGet("get/{apartmentId}")]
        public async Task<ActionResult<ICollection<Amenity>>> GetAmenitiesForApartment(int apartmentId)
        {
            ServiceResponse<ICollection<Amenity>> amenitiesResponse = await _amenityService.GetAmenitiesForApartment(apartmentId);

            if (amenitiesResponse.Error != null) return BadRequest(amenitiesResponse.Error);
            return Ok(amenitiesResponse.Data);
        }

        //API - api/amenity/add

        [HttpPost("add")]
        public async Task<ActionResult<Amenity>> AddNewAmenity(AmenityInput amenityInput)
        {
            ServiceResponse<Amenity> addAmenityResponse = await _amenityService.AddAmenity(amenityInput);

            if(addAmenityResponse.Error != null) return BadRequest(addAmenityResponse.Error);
            return Ok(addAmenityResponse.Data);

        }

        //API- api/amenity/update

        [HttpPut("update")]
        public async Task<ActionResult<string>> UpdateExistingAmenity(AmenityInput amenityInput)
        {
            ServiceResponse<string> addAmenityResponse = await _amenityService.UpdateAmenity(amenityInput);

            if (addAmenityResponse.Error != null) return BadRequest(addAmenityResponse.Error);
            return Ok(addAmenityResponse.Data);
        }
    }
}
