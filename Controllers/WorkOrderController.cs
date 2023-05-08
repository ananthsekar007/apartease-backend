using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Data;
using apartease_backend.Models;
using apartease_backend.Services.WorkOrderService;
using apartease_backend.Dao.WorkOrderDao;
using apartease_backend.Dao;
using Microsoft.AspNetCore.Authorization;

namespace apartease_backend.Controllers
{
    [Route("api/workorder")]
    [ApiController]
    [Authorize(Roles = "Vendor,  Resident")]
    public class WorkOrderController : ControllerBase
    {
        private readonly ApartEaseContext _context;

        private readonly IWorkOrderService _workOrderService;

        public WorkOrderController(ApartEaseContext context, IWorkOrderService workOrderService)
        {
            _context = context;
            _workOrderService = workOrderService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<WorkOrder>> AddWorkOrder([FromBody] WorkOrderInput workOrderInput) 
        {
            ServiceResponse<WorkOrder> response = await _workOrderService.AddWorkOrder(workOrderInput);

            if(response.Error != null) return BadRequest(response.Error);

            return Ok(response.Data);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<WorkOrder>> EditWorkOrder([FromBody] WorkOrderInput workOrderInput)
        {
            ServiceResponse<string> response = await _workOrderService.UpdateWorkOrder(workOrderInput);

            if (response.Error != null) return BadRequest(response.Error);

            return Ok(response.Data);
        }

        [HttpGet("resident/get/in-active/{residentId}")]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetWaitingWorkOrdersForResidents(int residentId)
        {
             IEnumerable<WorkOrder> workOrders = await _workOrderService.GetAllWaitingWorkOrdersForResidents(residentId);
            return Ok(workOrders);
        }

        [HttpGet("resident/get/active/{residentId}")]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetWorkOrdersForResidents(int residentId)
        {
            IEnumerable<WorkOrder> workOrders = await _workOrderService.GetAllWorkOrdersForResidents(residentId);
            return Ok(workOrders);
        }

        [HttpGet("vendor/get/active/{vendorId}")]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetOnGoingWorkOrdersForVendors(int vendorId)
        {
            IEnumerable<WorkOrder> workOrders = await _workOrderService.GetAllOnGoingWorkOrdersForVendors(vendorId);
            return Ok(workOrders);
        }

        [HttpGet("vendor/get/in-active/{vendorId}")]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetAllWorkOrdersForVendors(int vendorId)
        {
            IEnumerable<WorkOrder> workOrders = await _workOrderService.GetAllWorkOrdersForVendors(vendorId);
            return Ok(workOrders);
        }
    }
}
