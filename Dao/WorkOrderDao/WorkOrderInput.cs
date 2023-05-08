using System.ComponentModel.DataAnnotations.Schema;

namespace apartease_backend.Dao.WorkOrderDao
{
    public class WorkOrderInput
    {
        public Nullable<int> WorkOrderId { get; set; }
        public string WorkOrderTitle { get; set; } = string.Empty;
        public string WorkOrderDescription { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public int ResidentId { get; set; }
        public string VendorStatus { get; set; } = string.Empty;
        public string ResidentStatus { get; set; } = string.Empty;
        public bool AcceptedByVendor { get; set; } = false;

        public bool CancelledByVendor { get; set; } = false;
    }
}
