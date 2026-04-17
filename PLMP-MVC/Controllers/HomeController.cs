using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly PLMPS6G5 _context;

        public HomeController(PLMPS6G5 context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Buildings = await _context.Buildings.CountAsync();
            ViewBag.Units = await _context.Units.CountAsync();
            ViewBag.Payments = await _context.Payments.CountAsync();
            ViewBag.Requests = await _context.MaintenanceRequests.CountAsync();

            var leaseApplications = await _context.Leases
                .OrderByDescending(l => l.LeaseId)
                .ToListAsync();

            var maintenanceRequests = await _context.MaintenanceRequests
                .OrderByDescending(r => r.RequestId)
                .ToListAsync();

            ViewBag.LeaseApplications = leaseApplications;
            ViewBag.MaintenanceRequests = maintenanceRequests;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveLease(int id)
        {
            var lease = await _context.Leases
                .FirstOrDefaultAsync(l => l.LeaseId == id);

            if (lease == null)
                return NotFound();

            var unit = await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId == lease.UnitId);

            if (unit == null)
                return NotFound();

            if (unit.AvailabilityStatus != "Vacant")
            {
                TempData["Error"] = "This unit is already leased.";
                return RedirectToAction("Index");
            }

            lease.ApplicationStatus = "Approved";
            lease.LeaseStatus = "Active";
            unit.AvailabilityStatus = "Leased";

            _context.Leases.Update(lease);
            _context.Units.Update(unit);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Lease application approved successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectLease(int id)
        {
            var lease = await _context.Leases
                .FirstOrDefaultAsync(l => l.LeaseId == id);

            if (lease == null)
                return NotFound();

            var unit = await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId == lease.UnitId);

            if (unit == null)
                return NotFound();

            lease.ApplicationStatus = "Rejected";
            lease.LeaseStatus = "Termination";
            unit.AvailabilityStatus = "Vacant";

            _context.Leases.Update(lease);
            _context.Units.Update(unit);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Lease application rejected.";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}