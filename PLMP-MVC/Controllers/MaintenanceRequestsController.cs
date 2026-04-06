using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MaintenanceRequestsController : Controller
    {
        private readonly PLMPS6G5 _context;

        public MaintenanceRequestsController(PLMPS6G5 context)
        {
            _context = context;
        }

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
    }
}