using apartease_backend.Models;

namespace apartease_backend.Services.VendorService
{
    public interface IVendorService
    {
        Task<IEnumerable<Vendor>> GetVendors();
    }
}
