using apartease_backend.Models;

namespace apartease_backend.Dao.ResidentDao
{
    public class ResidentAuthResponse
    {
        public Resident Resident { get; set; } = new Resident();

        public string AuthToken { get; set; } = string.Empty;
    }
}
