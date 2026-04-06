using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;
using System.Security.Claims;

namespace PLMP_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicationsController : Controller
    {
        private readonly PLMPS6G5 _context;

        public ApplicationsController(PLMPS6G5 context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _context.Applications
                .Include(a => a.Tenant)
                .Include(a => a.Unit)
                .OrderByDescending(a => a.ApplicationId)
                .ToListAsync();

            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var application = await _context.Applications
                    .Include(a => a.Unit)
                    .FirstOrDefaultAsync(a => a.ApplicationId == id);

                if (application == null)
                    return NotFound();

                if (application.ApplicationStatus != "Submitted" && application.ApplicationStatus != "Pending")
                    return RedirectToAction("Index");

                application.ApplicationStatus = "Approved";

                if (application.Unit != null)
                    application.Unit.AvailabilityStatus = "Leased";

                int managerId = 0;

                var managerIdClaim = User.FindFirst("ManagerId")?.Value;
                if (!string.IsNullOrEmpty(managerIdClaim) && int.TryParse(managerIdClaim, out int parsedManagerId))
                {
                    managerId = parsedManagerId;
                }
                else
                {
                    managerId = await _context.Leases
                        .Where(l => l.ManagerId > 0)
                        .Select(l => l.ManagerId)
                        .FirstOrDefaultAsync();
                }

                if (managerId == 0)
                {
                    TempData["Error"] = "No valid Manager ID was found. Please make sure there is a manager record in the database.";
                    return RedirectToAction("Index");
                }

                var lease = new Lease
                {
                    UnitId = application.UnitId,
                    TenantId = application.TenantId,
                    ManagerId = managerId,
                    ApplicationStatus = "Approved",
                    LeaseStatus = "Active",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1)
                };

                _context.Leases.Add(lease);
                await _context.SaveChangesAsync();

                var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == application.UnitId);

                if (unit != null)
                {
                    var payment = new Payment
                    {
                        LeaseId = lease.LeaseId,
                        InstallmentAmount = unit.RentAmount,
                        DateOfIssue = DateTime.Now,
                        Balance = unit.RentAmount,
                        PaymentStatus = "Pending"
                    };

                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Application approved successfully.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "A database error occurred while approving the application. Please check ManagerId and foreign key data.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "An unexpected error occurred while approving the application.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Unit)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                return NotFound();

            application.ApplicationStatus = "Rejected";

            if (application.Unit != null)
                application.Unit.AvailabilityStatus = "Vacant";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Application rejected.";
            return RedirectToAction("Index");
        }
    }
}