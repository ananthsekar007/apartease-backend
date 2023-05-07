using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace apartease_backend.Dao.ApartmentBookingDao
{
    public class AmenityBookingInput
    {
        public int AmenityId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        [EmailAddress]
        public string GuestEmail { get; set; } = string.Empty;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int ResidentId { get; set; }

        public Nullable<int> AmenityBookingId { get; set; }

    }
}
