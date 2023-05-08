using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apartease_backend.Models
{
    public class WorkOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkOrderId { get; set; }
        public string WorkOrderTitle { get; set; }  = string.Empty;
        public string WorkOrderDescription { get; set; } = string.Empty;

        [ForeignKey("Apartment")]
        public int ApartmentId { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        [ForeignKey("Resident")]
        public int ResidentId { get; set; }
        public string VendorStatus { get; set; } = string.Empty;
        public string ResidentStatus { get; set; } = string.Empty;
        public bool AcceptedByVendor { get; set; } = false;
        public bool CancelledByVendor { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual Apartment Apartment { get; set; }
        public virtual Vendor Vendor { get; set;}
        public virtual Resident Resident { get; set; }
    }
}
