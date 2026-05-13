using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_S6G5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicMaintenanceLookupController : ControllerBase
    {
        private readonly PLMPS6G5 _context;

        public PublicMaintenanceLookupController(PLMPS6G5 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int requestId, string phoneNumber)
        {
            var request = await _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Staff)
                .FirstOrDefaultAsync(r =>
                    r.RequestId == requestId &&
                    r.Tenant.PhoneNumber == phoneNumber);

            if (request == null)
                return NotFound();

            return Ok(new
            {
                request.RequestId,
                request.CategoryType,
                request.Priority,
                request.Description,
                request.Status,
                TenantName = request.Tenant.Name,
                TenantPhone = request.Tenant.PhoneNumber,
                StaffName = request.Staff != null ? request.Staff.Name : "Not Assigned"
            });
        }
    }
}