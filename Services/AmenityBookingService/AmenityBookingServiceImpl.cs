using apartease_backend.Dao;
using apartease_backend.Dao.ApartmentBookingDao;
using apartease_backend.Data;
using apartease_backend.Models;
using Azure;
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
                response.Error = "There is no Amenity with the given information";
                return response;
            }

            if(!existingAmenity.AllowWeekend && CheckIfDateLiesOnWeekend(amenityBookingInput.From, amenityBookingInput.To))
            {
                response.Error = $"{existingAmenity.AmenityName} is closed on weekends!";
                return response;
            }

            string checkIfAnyErrorInBooking = await CheckIfBookingIsValid(amenityBookingInput.From, amenityBookingInput.To, existingAmenity.MininumBookingHour);

            if(checkIfAnyErrorInBooking != "OK")
            {
                response.Error = checkIfAnyErrorInBooking;
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

        public async Task<ServiceResponse<string>> UpdateBooking(AmenityBookingInput amenityBookingInput)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            AmenityBooking existingAmenityBooking = await _context.AmenityBooking.FindAsync(amenityBookingInput.AmenityBookingId);
            if(existingAmenityBooking == null)
            {
                response.Error = "No Booking details available for the given information!";
                return response;
            }

            Amenity existingAmenity = await _context.Amenity.FindAsync(amenityBookingInput.AmenityId);

            if (existingAmenity == null)
            {
                response.Error = "There is no Amenity with the given information";
                return response;
            }

            if (!existingAmenity.AllowWeekend && CheckIfDateLiesOnWeekend(amenityBookingInput.From, amenityBookingInput.To))
            {
                response.Error = $"{existingAmenity.AmenityName} is closed on weekends!";
                return response;
            }


            string checkIfAnyErrorInBooking = await CheckIfBookingIsValid(amenityBookingInput.From, amenityBookingInput.To, existingAmenity.MininumBookingHour);

            if (checkIfAnyErrorInBooking != "OK")
            {
                response.Error = checkIfAnyErrorInBooking;
                return response;
            }

            existingAmenityBooking.From = amenityBookingInput.From;
            existingAmenityBooking.To = amenityBookingInput.To;
            existingAmenityBooking.GuestEmail = amenityBookingInput.GuestEmail;
            existingAmenityBooking.GuestName = amenityBookingInput.GuestName;

            await _context.SaveChangesAsync();

            response.Data = "Booking updated successfully!";

            return response;

        }

        public async Task<string> CheckIfBookingIsValid(DateTime fromDate, DateTime toDate, int minimumBookingHour)
        {
            if (toDate < DateTime.Now || fromDate < DateTime.Now)
            {
                return "Cannot allow to book in past dates!.";
            }

            TimeSpan timeDifference = toDate - fromDate;

            if (timeDifference.TotalMinutes > 240)
            {
                return "The maximum booking time must not exceed 4 hrs!";
            }

            if (timeDifference.TotalMinutes < (minimumBookingHour * 60))
            {
                return $"The minimum booking hours should be greater than or equal to {minimumBookingHour}";
            }

            bool isBookingConflicting = await _context.AmenityBooking.AnyAsync(b => b.To > fromDate && b.From < toDate);

            if (isBookingConflicting)
            {
                return "The time slots are un available, please choose different time slots!";
            }

            return  "OK";
        }

        public async Task<IEnumerable<AmenityBooking>> GetAmenitiesForResident(int residentId)
        {
            IEnumerable<AmenityBooking> amenities = await _context.AmenityBooking
                .Include(ab => ab.Amenity).Where(x => x.ResidentId == residentId).ToListAsync();

            return amenities;
        }

        public bool CheckIfDateLiesOnWeekend(DateTime fromDate, DateTime toDate)
        {
            return fromDate.DayOfWeek == DayOfWeek.Saturday || fromDate.DayOfWeek == DayOfWeek.Sunday || toDate.DayOfWeek == DayOfWeek.Saturday || toDate.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
