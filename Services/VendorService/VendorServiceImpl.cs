using apartease_backend.Data;
using apartease_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace apartease_backend.Services.VendorService
{
    public class VendorServiceImpl: IVendorService
    {
        private readonly ApartEaseContext _context;

        public VendorServiceImpl(ApartEaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vendor>> GetVendors()
        {
            return await _context.Vendor.Include(v => v.Company).ThenInclude(c => c.Category).ToListAsync();
        }

    }
}
