using apartease_backend.Dao.WorkOrderDao;
using apartease_backend.Dao;
using apartease_backend.Models;
using apartease_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace apartease_backend.Services.WorkOrderService
{
    public class WorkOrderServiceImpl: IWorkOrderService
    {
        private readonly ApartEaseContext _context;

        public WorkOrderServiceImpl(ApartEaseContext context) {
            _context = context;
        }

        public async Task<ServiceResponse<WorkOrder>> AddWorkOrder(WorkOrderInput workOrderInput)
        {

            ServiceResponse<WorkOrder> response = new ServiceResponse<WorkOrder>();

            Resident existingResident = await _context.Resident.FindAsync(workOrderInput.ResidentId);

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
