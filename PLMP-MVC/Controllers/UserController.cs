using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

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
                .ToListAsync();

            var myLeases = await _context.Leases
                .Where(l => l.TenantId == tenantId)
                .ToListAsync();

            ViewBag.AvailableUnits = availableUnits;
            ViewBag.MyRequests = myRequests;
            ViewBag.MyLeases = myLeases;

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
            ModelState.Remove("Tenant");
            ModelState.Remove("Staff");
            ModelState.Remove("TenantId");
            ModelState.Remove("StaffId");
            ModelState.Remove("Status");
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

                TempData["Error"] = "Please complete all required fields.";
                return View(request);
            }

            request.TenantId = tenantId;
            request.StaffId = null;
            request.Status = "Submitted";

            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Maintenance request submitted successfully.";
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyConfirmed(int id)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            int tenantId = int.Parse(tenantIdClaim);

            var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null)
                return NotFound();

            if (unit.AvailabilityStatus != "Vacant")
            {
                TempData["Error"] = "This unit is no longer available.";
                return RedirectToAction("Dashboard");
            }

            var existingLease = await _context.Leases
                .FirstOrDefaultAsync(l => l.UnitId == id && l.TenantId == tenantId);

            if (existingLease != null)
            {
                TempData["Error"] = "You already applied for this unit.";
                return RedirectToAction("Dashboard");
            }

            var newLease = new Lease
            {
                UnitId = unit.UnitId,
                TenantId = tenantId,
                ManagerId = 1,
                ApplicationStatus = "Pending",
                LeaseStatus = "Pending",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1)
            };

            _context.Leases.Add(newLease);

            unit.AvailabilityStatus = "Pending";
            _context.Units.Update(unit);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Your application has been submitted successfully.";
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Contract(int id)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return RedirectToAction("Login", "Auth");

            int tenantId = int.Parse(tenantIdClaim);

            var lease = await _context.Leases
                .FirstOrDefaultAsync(l => l.LeaseId == id && l.TenantId == tenantId);

            if (lease == null)
                return NotFound();

            return View(lease);
        }
    }
}