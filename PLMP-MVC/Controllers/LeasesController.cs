using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_MVC.Controllers
{
    public class LeasesController : Controller
    {
        private readonly PLMPS6G5 _context;

        public LeasesController(PLMPS6G5 context)
        {
            _context = context;
        }

        // GET: Leases
        public async Task<IActionResult> Index()
        {
            var pLMPS6G5 = _context.Leases.Include(l => l.Manager).Include(l => l.Tenant).Include(l => l.Unit);
            return View(await pLMPS6G5.ToListAsync());
        }

        // GET: Leases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Manager)
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(m => m.LeaseId == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Leases/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Email");
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Email");
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "AvailabilityStatus");
            return View();
        }

        // POST: Leases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaseId,UnitId,TenantId,ManagerId,ApplicationStatus,LeaseStatus,StartDate,EndDate")] Lease lease)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lease);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Email", lease.ManagerId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Email", lease.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "AvailabilityStatus", lease.UnitId);
            return View(lease);
        }

        // GET: Leases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Email", lease.ManagerId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Email", lease.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "AvailabilityStatus", lease.UnitId);
            return View(lease);
        }

        // POST: Leases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaseId,UnitId,TenantId,ManagerId,ApplicationStatus,LeaseStatus,StartDate,EndDate")] Lease lease)
        {
            if (id != lease.LeaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseExists(lease.LeaseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Email", lease.ManagerId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Email", lease.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "AvailabilityStatus", lease.UnitId);
            return View(lease);
        }

        // GET: Leases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Manager)
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(m => m.LeaseId == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // POST: Leases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lease = await _context.Leases.FindAsync(id);
            if (lease != null)
            {
                _context.Leases.Remove(lease);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseExists(int id)
        {
            return _context.Leases.Any(e => e.LeaseId == id);
        }
    }
}
