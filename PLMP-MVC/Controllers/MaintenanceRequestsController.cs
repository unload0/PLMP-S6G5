using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;
using System.Security.Claims;

namespace PLMP_MVC.Controllers
{
    [Authorize]
    public class MaintenanceRequestsController : Controller
    {
        private readonly PLMPS6G5 _context;

        public MaintenanceRequestsController(PLMPS6G5 context)
        {
            _context = context;
        }

        // Admin only: view all maintenance requests
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var requests = await _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Staff)
                .ToListAsync();

            var staffList = await _context.MaintenanceStaffs
                .Where(s => s.Available == true)
                .ToListAsync();

            ViewBag.StaffList = staffList;

            return View(requests);
        }

        // Admin only: assign technician
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignTechnician(int requestId, int staffId)
        {
            var request = await _context.MaintenanceRequests
                .FirstOrDefaultAsync(r => r.RequestId == requestId);

            if (request == null)
                return NotFound();

            var staff = await _context.MaintenanceStaffs
                .FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
                return NotFound();

            request.StaffId = staffId;
            request.Status = "Assigned";

            _context.MaintenanceRequests.Update(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Technician assigned successfully.";
            return RedirectToAction("Index");
        }

        // User/Admin: open create request page
        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View("~/Views/User/CreateMaintenanceRequest.cshtml");
        }

        // User/Admin: submit maintenance request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceRequest request)
        {
            request.Status = "Pending";

            var tenantIdClaim = User.FindFirst("TenantId")?.Value
                                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(tenantIdClaim, out int tenantId))
            {
                request.TenantId = tenantId;
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                TempData["Error"] = "Please fill in all required fields correctly.";
                return View("~/Views/User/CreateMaintenanceRequest.cshtml", request);
            }

            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your maintenance request has been submitted successfully.";
            return RedirectToAction("Create");
        }
        

        private void LoadDropdowns()
        {
            ViewBag.CategoryOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Electrical", Text = "Electrical" },
                new SelectListItem { Value = "Plumbing", Text = "Plumbing" },
                new SelectListItem { Value = "HVAC", Text = "HVAC" },
                new SelectListItem { Value = "General", Text = "General" }
            };

            ViewBag.PriorityOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Low", Text = "Low" },
                new SelectListItem { Value = "Medium", Text = "Medium" },
                new SelectListItem { Value = "High", Text = "High" }
            };
        }
    }
}