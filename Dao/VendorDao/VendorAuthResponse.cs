using apartease_backend.Models;

namespace apartease_backend.Dao.VendorDao
{
    public class VendorAuthResponse
    {
        public Vendor Vendor { get; set; } = new Vendor();

        public string AuthToken { get; set; } = string.Empty;
    }
}
