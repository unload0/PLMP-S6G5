using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_S6G5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "PropertyManager")]
    public class ApiReportsController : ControllerBase
    {
        private readonly PLMPS6G5 _context;

        public ApiReportsController(PLMPS6G5 context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            var report = new
            {
                TotalBuildings = await _context.Buildings.CountAsync(),
                TotalUnits = await _context.Units.CountAsync(),
                TotalTenants = await _context.Tenants.CountAsync(),
                TotalLeases = await _context.Leases.CountAsync(),

                VacantUnits = await _context.Units
                    .CountAsync(u => u.AvailabilityStatus == "Vacant"),

                LeasedUnits = await _context.Units
                    .CountAsync(u => u.AvailabilityStatus == "Leased"),

                ActiveLeases = await _context.Leases
                    .CountAsync(l => l.LeaseStatus == "Active"),

                TerminatedLeases = await _context.Leases
                    .CountAsync(l => l.LeaseStatus == "Termination"),

                OverduePayments = await _context.Payments
                    .CountAsync(p => p.PaymentStatus == "Overdue"),

                OpenMaintenanceRequests = await _context.MaintenanceRequests
                    .CountAsync(r => r.Status != "Resolved" && r.Status != "Closed")
            };

            return Ok(report);
        }
    }
}