using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apartease_backend.Models
{
    public class AmenityBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AmenityBookingId { get; set; }

        [ForeignKey("Amenity")]
        public int AmenityId { get; set; }
        public string GuestName { get; set;} = string.Empty;
        [EmailAddress]
        public string GuestEmail { get; set;} = string.Empty;
        public DateTime From { get; set;}
        public DateTime To { get; set;}

        [ForeignKey("Resident")]
        public int ResidentId { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual Amenity Amenity { get; set; }
        public virtual Resident Resident { get; set; }
        public virtual Manager Manager { get; set; }

    }
}
