using apartease_backend.Dao.WorkOrderDao;
using apartease_backend.Dao;
using apartease_backend.Models;
using apartease_backend.Data;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Services.EmailService;
using apartease_backend.Dao.ApartmentBookingDao;

namespace apartease_backend.Services.WorkOrderService
{
    public class WorkOrderServiceImpl: IWorkOrderService
    {
        private readonly ApartEaseContext _context;
        private readonly IEmailService _emailService;

        public WorkOrderServiceImpl(ApartEaseContext context, IEmailService emailService) {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<WorkOrder>> AddWorkOrder(WorkOrderInput workOrderInput)
        {

            ServiceResponse<WorkOrder> response = new ServiceResponse<WorkOrder>();

            Resident existingResident = await _context.Resident.FindAsync(workOrderInput.ResidentId);
            Vendor existingVendor = await _context.Vendor.FindAsync(workOrderInput.VendorId);

            if (existingResident == null) {
                response.Error = "No resident found with the given information!";
                return response;
            }
            
            if(!existingResident.ApartmentId.HasValue) {
                response.Error = "The resident is not linked with any apartments!";
                return response;
            }

            WorkOrder newWorkOrder = new WorkOrder()
            {
                ApartmentId = existingResident.ApartmentId.Value,
                ResidentId = workOrderInput.ResidentId,
                VendorId = workOrderInput.VendorId,
                WorkOrderDescription = workOrderInput.WorkOrderDescription,
                WorkOrderTitle = workOrderInput.WorkOrderTitle,
                VendorStatus = workOrderInput.VendorStatus,
                ResidentStatus = workOrderInput.ResidentStatus
            };

            try
            {
                await _context.WorkOrder.AddAsync(newWorkOrder);
                await _context.SaveChangesAsync();

                await _emailService.SendAppEmailAsync(existingResident.Email, "Work Order", "Your new work order creation is successful!");
                await _emailService.SendAppEmailAsync(existingVendor.Email, "Work Order Assigned", "A resident has assigned a work order to you!");

                response.Data = newWorkOrder;
                return response;
            }
            catch (Exception ex)
            {
                response.Error = "Something went wrong, please try again!";
                return response;
            }

        }

        public async Task<ServiceResponse<string>> UpdateWorkOrder(WorkOrderInput workOrderInput)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            WorkOrder existingWorkOrder = await _context.WorkOrder.FindAsync(workOrderInput.WorkOrderId);
            Resident existingResident = await _context.Resident.FindAsync(workOrderInput.ResidentId);
            Vendor existingVendor = await _context.Vendor.FindAsync(workOrderInput.VendorId);


            if (existingWorkOrder == null)
            {
                response.Error = "There is no work order with the given details!";
                return response;
            }

            Console.WriteLine("kdshfklashflkashd>>>>>>>>><<<<<<<<<" + workOrderInput.CancelledByVendor);

            existingWorkOrder.WorkOrderDescription = workOrderInput.WorkOrderDescription;
            existingWorkOrder.WorkOrderTitle = workOrderInput.WorkOrderTitle;
            existingWorkOrder.VendorStatus = workOrderInput.VendorStatus;
            existingWorkOrder.ResidentStatus = workOrderInput.ResidentStatus;
            existingWorkOrder.AcceptedByVendor = workOrderInput.AcceptedByVendor;
            existingWorkOrder.CancelledByVendor = workOrderInput.CancelledByVendor;

            try
            {
                await _context.SaveChangesAsync();

                await _emailService.SendAppEmailAsync(existingResident.Email, "Work Order Update", "An existing work order status is updated!");
                await _emailService.SendAppEmailAsync(existingVendor.Email, "Work Order Update", "An existing work order status is updated!");


                response.Data = "Work order is edited!";
                return response;
            }
            catch (Exception ex)
            {
                response.Error = "Something went wrong, please try again!";
                return response;
            }
        }

        public async Task<IEnumerable<WorkOrder>> GetAllWaitingWorkOrdersForResidents(int residentId)
        {
          return await _context.WorkOrder.Include(w => w.Vendor).ThenInclude(v => v.Company).ThenInclude(c => c.Category).Where(w => w.ResidentId == residentId && w.AcceptedByVendor == false && w.CancelledByVendor == false).ToListAsync();   
        }

        public async Task<IEnumerable<WorkOrder>> GetAllWorkOrdersForResidents(int residentId)
        {
            return await _context.WorkOrder.Include(w => w.Vendor).ThenInclude(v => v.Company).ThenInclude(c => c.Category).Where(w => w.ResidentId == residentId && w.AcceptedByVendor == true && w.CancelledByVendor == false).ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetAllWorkOrdersForVendors(int vendorId)
        {
            return await _context.WorkOrder.Include(w => w.Resident).Where(w => w.VendorId == vendorId && w.AcceptedByVendor == false && w.CancelledByVendor == false).ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetAllOnGoingWorkOrdersForVendors(int vendorId)
        {
            return await _context.WorkOrder.Include(w => w.Resident).Where(w => w.VendorId == vendorId && w.AcceptedByVendor == true && w.CancelledByVendor == false).ToListAsync();
        }
    }
}
