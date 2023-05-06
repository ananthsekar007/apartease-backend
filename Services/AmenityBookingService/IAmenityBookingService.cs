using apartease_backend.Dao;
using apartease_backend.Dao.ApartmentBookingDao;

namespace apartease_backend.Services.AmenityBookingService
{
    public interface IAmenityBookingService
    {
        Task<ServiceResponse<string>> BookAmenity(AmenityBookingInput amenityBookingInput);
    }
}
