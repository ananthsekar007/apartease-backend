using apartease_backend.Dao;
using apartease_backend.Dao.AmenityDao;
using apartease_backend.Data;
using apartease_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace apartease_backend.Services.AmenityService
{

    public class AmenityServiceImpl: IAmenityService
    {
        private readonly ApartEaseContext _context;
        public AmenityServiceImpl(ApartEaseContext context) { 
            _context = context;
        }

        public async Task<ServiceResponse<ICollection<Amenity>>> GetAmenitiesForApartment(int apartmentId)
        {
            ServiceResponse<ICollection<Amenity>> response = new();
            Apartment existingApartment = await _context.Apartment.FindAsync(apartmentId);


            if(existingApartment == null)
            {
                response.Error = "There is no apartment with the provided details!";
                return response;
            }

            ICollection<Amenity> amenities = await _context.Amenity.Where(a => a.ApartmentId == apartmentId).ToListAsync();

            response.Data = amenities;
            
           return response;
        }

        public async Task<ServiceResponse<Amenity>> AddAmenity(AmenityInput amenityInput)
        {
            ServiceResponse<Amenity> response = new();
            Apartment existingApartment = await _context.Apartment.FindAsync(amenityInput.ApartmentId);
            if (existingApartment == null)
            {
                response.Error = "There is no apartment with the provided details!";
                return response;
            }

            Amenity newAmenity = new Amenity()
            {
                ApartmentId = amenityInput.ApartmentId,
                AllowWeekend = amenityInput.AllowWeekend,
                AmenityAddress = amenityInput.AmenityAddress,
                AmenityContactNumber = amenityInput.AmenityContactNumber,
                AmenityDescription = amenityInput.AmenityDescription,
                AmenityName = amenityInput.AmenityName,
                MininumBookingHour = amenityInput.MininumBookingHour,
            };

            try
            {
                await _context.Amenity.AddAsync(newAmenity);
                await _context.SaveChangesAsync();
                response.Data = newAmenity;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = "Error in creating an Amenity, please try again!";
                return response;
            }
        }

        public async Task<ServiceResponse<string>> UpdateAmenity(AmenityInput amenityInput)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            Amenity existingAmenity = await _context.Amenity.FindAsync(amenityInput.AmenityId);

            if(existingAmenity == null)
            {
                response.Error = "There is no existing amenity with the provided details!";
            }

            existingAmenity.AllowWeekend = amenityInput.AllowWeekend;
            existingAmenity.AmenityDescription = amenityInput.AmenityDescription;
            existingAmenity.AmenityName = amenityInput.AmenityName;
            existingAmenity.AmenityContactNumber = amenityInput.AmenityContactNumber;
            existingAmenity.AmenityAddress = amenityInput.AmenityAddress;
            existingAmenity.MininumBookingHour = amenityInput.MininumBookingHour;
            try
            {
                await _context.SaveChangesAsync();
                response.Data = "Amenity Updated successfully!";
                return response;
            }
            catch(Exception ex)
            {
                response.Error = "Something went wrong, please try again!";
                return response;
            }
        }

    }
}
