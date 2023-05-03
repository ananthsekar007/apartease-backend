using System.ComponentModel.DataAnnotations.Schema;

namespace apartease_backend.Dao.AmenityDao
{
    public class AmenityInput
    {
        public int? AmenityId { get; set; }
        public string AmenityName { get; set; } = string.Empty;
        public string AmenityDescription { get; set; } = string.Empty;

        public string AmenityContactNumber { get; set; } = string.Empty;

        public string AmenityAddress { get; set; } = string.Empty;

        public bool AllowWeekend { get; set; } = true;

        public int MininumBookingHour { get; set; }

        public int ApartmentId { get; set; }
    }
}
