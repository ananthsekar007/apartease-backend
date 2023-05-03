using apartease_backend.Dao.AmenityDao;
using apartease_backend.Dao;
using apartease_backend.Models;

namespace apartease_backend.Services.AmenityService
{
    public interface IAmenityService
    {
        Task<ServiceResponse<ICollection<Amenity>>> GetAmenitiesForApartment(int apartmentId);
        Task<ServiceResponse<Amenity>> AddAmenity(AmenityInput amenityInput);

        Task<ServiceResponse<string>> UpdateAmenity(AmenityInput amenityInput);
    }
}
