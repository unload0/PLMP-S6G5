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

        // GET: MaintenanceRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaintenanceRequests.ToListAsync());
        }

        // GET: MaintenanceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var maintenanceRequest = await _context.MaintenanceRequests
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (maintenanceRequest == null) return NotFound();

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceRequest maintenanceRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenanceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest == null) return NotFound();

            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaintenanceRequest maintenanceRequest)
        {
            if (id != maintenanceRequest.RequestId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MaintenanceRequests.Any(e => e.RequestId == maintenanceRequest.RequestId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var maintenanceRequest = await _context.MaintenanceRequests
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (maintenanceRequest == null) return NotFound();

            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}