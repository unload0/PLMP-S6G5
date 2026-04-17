using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace PLMP_MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly PLMPS6G5 _context;

        public UserController(PLMPS6G5 context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            int tenantId = int.Parse(tenantIdClaim);

            var availableUnits = await _context.Units
                .Where(u => u.AvailabilityStatus == "Vacant")
                .ToListAsync();

            var myRequests = await _context.MaintenanceRequests
                .Where(r => r.TenantId == tenantId)
                .OrderByDescending(r => r.RequestId)
                .ToListAsync();

            //var myApplications = await _context.Applications
            //    .Where(a => a.TenantId == tenantId)
            //    .OrderByDescending(a => a.ApplicationId)
            //    .ToListAsync();

            var myLeases = await _context.Leases
                .Where(l => l.TenantId == tenantId)
                .OrderByDescending(l => l.LeaseId)
                .ToListAsync();

            var myPayments = await _context.Payments
                .Join(_context.Leases,
                    p => p.LeaseId,
                    l => l.LeaseId,
                    (p, l) => new { Payment = p, Lease = l })
                .Where(x => x.Lease.TenantId == tenantId)
                .Select(x => x.Payment)
                .ToListAsync();

            ViewBag.AvailableUnits = availableUnits;
            ViewBag.MyRequests = myRequests;
            //ViewBag.MyApplications = myApplications;
            ViewBag.MyLeases = myLeases;
            ViewBag.MyPayments = myPayments;

            return View();
        }

        [HttpGet]
        public IActionResult CreateMaintenanceRequest()
        {
            ViewBag.CategoryOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Plumbing", Text = "Plumbing" },
                new SelectListItem { Value = "Electricity", Text = "Electricity" },
                new SelectListItem { Value = "AC", Text = "AC" },
                new SelectListItem { Value = "Cleaning", Text = "Cleaning" },
                new SelectListItem { Value = "General Maintenance", Text = "General Maintenance" }
            };

            ViewBag.PriorityOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Low", Text = "Low" },
                new SelectListItem { Value = "Medium", Text = "Medium" },
                new SelectListItem { Value = "High", Text = "High" }
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMaintenanceRequest(MaintenanceRequest request)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;
            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            int tenantId = int.Parse(tenantIdClaim);

            if (string.IsNullOrWhiteSpace(request.CategoryType))
                ModelState.AddModelError("CategoryType", "Please select a category.");

            if (string.IsNullOrWhiteSpace(request.Priority))
                ModelState.AddModelError("Priority", "Please select a priority.");

            if (string.IsNullOrWhiteSpace(request.Description))
                ModelState.AddModelError("Description", "Please enter a description.");

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Plumbing", Text = "Plumbing" },
                    new SelectListItem { Value = "Electricity", Text = "Electricity" },
                    new SelectListItem { Value = "AC", Text = "AC" },
                    new SelectListItem { Value = "Cleaning", Text = "Cleaning" },
                    new SelectListItem { Value = "General Maintenance", Text = "General Maintenance" }
                };

                ViewBag.PriorityOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Low", Text = "Low" },
                    new SelectListItem { Value = "Medium", Text = "Medium" },
                    new SelectListItem { Value = "High", Text = "High" }
                };

                return View(request);
            }

            request.TenantId = tenantId;
            request.Status = "Submitted";

            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Maintenance request submitted successfully.";
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            var unit = await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId == id && u.AvailabilityStatus == "Vacant");

            if (unit == null)
            {
                TempData["Error"] = "This unit is not available.";
                return RedirectToAction("Dashboard");
            }

            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyConfirmed(int id)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            int tenantId = int.Parse(tenantIdClaim);

            var unit = await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null)
                return NotFound();

            if (unit.AvailabilityStatus != "Vacant")
            {
                TempData["Error"] = "This unit is no longer available.";
                return RedirectToAction("Dashboard");
            }

            //var existingApplication = await _context.Applications
            //    .FirstOrDefaultAsync(a => a.UnitId == id && a.TenantId == tenantId &&
            //        (a.ApplicationStatus == "Submitted" || a.ApplicationStatus == "Pending"));
            var existingApplication = await _context.Leases
                .FirstOrDefaultAsync(a => a.UnitId == id && a.TenantId == tenantId &&
                    (a.ApplicationStatus == "Screening"));

            if (existingApplication != null)
            {
                TempData["Error"] = "You already applied for this unit.";
                return RedirectToAction("Dashboard");
            }

            //var application = new Application
            //{
            //    UnitId = unit.UnitId,
            //    TenantId = tenantId,
            //    ApplicationDate = DateTime.Now,
            //    ApplicationStatus = "Submitted"
            //};
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
                UnitId = unit.UnitId,
                TenantId = tenantId,
                ManagerId = managerId,
                ApplicationStatus = "Screening",
                LeaseStatus = "Pending",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1)
            };

            unit.AvailabilityStatus = "Pending";

            //_context.Applications.Add(application);
            _context.Leases.Add(lease);
            _context.Units.Update(unit);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Application submitted successfully.";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            int tenantId = int.Parse(tenantIdClaim);

            var payment = await _context.Payments
                .Join(_context.Leases,
                    p => p.LeaseId,
                    l => l.LeaseId,
                    (p, l) => new { Payment = p, Lease = l })
                .Where(x => x.Payment.PaymentId == id && x.Lease.TenantId == tenantId)
                .Select(x => x.Payment)
                .FirstOrDefaultAsync();

            if (payment == null)
                return NotFound();

            payment.PaymentStatus = "Paid";
            payment.Balance = 0;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment completed successfully.";
            return RedirectToAction("Dashboard");
        }
    }
}