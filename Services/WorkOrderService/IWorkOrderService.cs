using apartease_backend.Dao;
using apartease_backend.Dao.WorkOrderDao;
using apartease_backend.Models;

namespace apartease_backend.Services.WorkOrderService
{
    public interface IWorkOrderService
    {
        Task<ServiceResponse<WorkOrder>> AddWorkOrder(WorkOrderInput workOrderInput);

        Task<ServiceResponse<string>> UpdateWorkOrder(WorkOrderInput workOrderInput);

        Task<IEnumerable<WorkOrder>> GetAllWaitingWorkOrdersForResidents(int residentId);

        Task<IEnumerable<WorkOrder>> GetAllWorkOrdersForResidents(int residentId);

        Task<IEnumerable<WorkOrder>> GetAllWorkOrdersForVendors(int vendorId);

        Task<IEnumerable<WorkOrder>> GetAllOnGoingWorkOrdersForVendors(int vendorId);
    }
}
