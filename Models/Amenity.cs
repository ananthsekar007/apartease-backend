using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apartease_backend.Models
{
    public class Amenity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AmenityId { get; set; }

        public string AmenityName { get; set; } = string.Empty;
        public string AmenityDescription { get; set; } = string.Empty;

        public string AmenityContactNumber { get; set; } = string.Empty;

        public string AmenityAddress { get; set; } = string.Empty;

        public bool AllowWeekend { get; set; } = true;

        public int MininumBookingHour { get; set; }

        [ForeignKey("Apartment")]
        public int ApartmentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual Apartment Apartment { get; set; }


    }
}
