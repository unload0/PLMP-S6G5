using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_MVC.Controllers
{
    public class PublicLookupController : Controller
    {
        private readonly PLMPS6G5 _context;

        public PublicLookupController(PLMPS6G5 context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int requestId, string phoneNumber)
        {
            var request = await _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Staff)
                .FirstOrDefaultAsync(r =>
                    r.RequestId == requestId &&
                    r.Tenant.PhoneNumber == phoneNumber);

            if (request == null)
            {
                ViewBag.Error = "No maintenance request found. Please check ticket number and phone number.";
                return View();
            }

            return View(request);
        }
    }
}