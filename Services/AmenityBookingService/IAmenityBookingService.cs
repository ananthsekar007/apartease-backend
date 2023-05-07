using apartease_backend.Dao;
using apartease_backend.Dao.ApartmentBookingDao;
using apartease_backend.Models;

namespace apartease_backend.Services.AmenityBookingService
{
    public interface IAmenityBookingService
    {
        Task<ServiceResponse<string>> BookAmenity(AmenityBookingInput amenityBookingInput);
        Task<ServiceResponse<string>> UpdateBooking(AmenityBookingInput amenityBookingInput);
        Task<string> CheckIfBookingIsValid(DateTime from, DateTime to, int minimumBookingHour);
        Task<IEnumerable<AmenityBooking>> GetAmenitiesForResident(int residentId);
    }
}
