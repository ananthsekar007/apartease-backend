using apartease_backend.Dao;
using apartease_backend.Data;
using apartease_backend.Models;

namespace apartease_backend.Services.ResidentService
{
    public class ResidentServiceImpl: IResidentService
    {
        private readonly ApartEaseContext _context;
        public ResidentServiceImpl(ApartEaseContext context) {
            _context = context;
        }

        public async Task<ServiceResponse<bool>> GetActivityStatus(int residentId)
        {
            ServiceResponse<bool> response = new();

            Resident existingResident = await _context.Resident.FindAsync(residentId);

            if(existingResident == null)
            {
                response.Error = "There is no resident with the provided information";
                return response;
            }

            response.Data = existingResident.IsActive;

            return response;
        }
    }
}
