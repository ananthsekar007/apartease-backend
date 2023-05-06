using apartease_backend.Dao;

namespace apartease_backend.Services.ResidentService
{
    public interface IResidentService
    {
        Task<ServiceResponse<bool>> GetActivityStatus(int residentId);
    }
}
