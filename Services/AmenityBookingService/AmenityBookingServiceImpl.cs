using apartease_backend.Dao;
using apartease_backend.Dao.ApartmentBookingDao;
using apartease_backend.Data;
using apartease_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace apartease_backend.Services.AmenityBookingService
{
    public class AmenityBookingServiceImpl : IAmenityBookingService 
    {
        private readonly ApartEaseContext _context;
        public AmenityBookingServiceImpl(ApartEaseContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<string>> BookAmenity(AmenityBookingInput amenityBookingInput)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            Amenity existingAmenity = await _context.Amenity
                                            .Include(a => a.Apartment)
                                            .FirstOrDefaultAsync(x => x.AmenityId == amenityBookingInput.AmenityId);

            if (existingAmenity == null)
            {
                response.Error = "There is no Amenity with the given text";
                return response;
            }

            if(amenityBookingInput.To < DateTime.Now || amenityBookingInput.From < DateTime.Now)
            {
                response.Error = "Cannot allow to book in past dates!.";
                return response;
            }

            TimeSpan timeDifference = amenityBookingInput.To - amenityBookingInput.From;

            if(timeDifference.TotalMinutes > 240) {
                response.Error = "The maximum booking time must not exceed 4 hrs!";
                return response;
            }

            if(timeDifference.TotalMinutes < (existingAmenity.MininumBookingHour* 60))
            {
                response.Error = $"The minimum booking hours should be greater than or equal to {existingAmenity.MininumBookingHour}";
                return response;
            }

            bool isBookingConflicting = await _context.AmenityBooking.AnyAsync(b => b.To > amenityBookingInput.From && b.From < amenityBookingInput.To);

            if (isBookingConflicting)
            {
                response.Error = "The time slots are un available, please choose different time slots!";
                return response;
            }

            AmenityBooking newBooking = new AmenityBooking()
            {
                From = amenityBookingInput.From,
                AmenityId = amenityBookingInput.AmenityId,
                GuestEmail = amenityBookingInput.GuestEmail,
                GuestName = amenityBookingInput.GuestName,
                To = amenityBookingInput.To,
                ResidentId = amenityBookingInput.ResidentId,
                ManagerId = existingAmenity.Apartment.ManagerId
            };

            await _context.AmenityBooking.AddAsync(newBooking);
            await _context.SaveChangesAsync();

            response.Data = "Booking made successfully!";

            return response;

        }
    }
}
