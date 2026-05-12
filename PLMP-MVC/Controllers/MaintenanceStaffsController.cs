using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PLMP_MVC.Controllers
{
    [Authorize(Roles = "Staff")]
    public class MaintenanceStaffsController : Controller
    {
        private readonly PLMPS6G5 _context;

        public MaintenanceStaffsController(PLMPS6G5 context)
        {
            _context = context;
        }

        // GET: MaintenanceStaffs
        public async Task<IActionResult> Index()
        {
            ViewBag.PaymentsCount = await _context.Payments.CountAsync();
            ViewBag.LeasesCount = await _context.Leases.CountAsync();
            ViewBag.RequestsCount = await _context.MaintenanceRequests.CountAsync();

            var maintenanceRequests = await _context.MaintenanceRequests
                .OrderByDescending(r => r.RequestId)
                .ToListAsync();

            ViewBag.MaintenanceRequests = maintenanceRequests;

            var maintenanceAssignedRequests = await _context.MaintenanceRequests
                .Where(r => r.StaffId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                .OrderByDescending(r => r.RequestId)
                .ToListAsync();

            ViewBag.MaintenanceAssignedRequests = maintenanceAssignedRequests;

            return View();
        }

        // GET: MaintenanceStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceStaff = await _context.MaintenanceStaffs
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (maintenanceStaff == null)
            {
                return NotFound();
            }

            return View(maintenanceStaff);
        }

        // GET: MaintenanceStaffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,Name,PhoneNumber,Email,SkillProfile,Available")] MaintenanceStaff maintenanceStaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenanceStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(maintenanceStaff);
        }

        // GET: MaintenanceStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceStaff = await _context.MaintenanceStaffs.FindAsync(id);
            if (maintenanceStaff == null)
            {
                return NotFound();
            }
            return View(maintenanceStaff);
        }

        // POST: MaintenanceStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,Name,PhoneNumber,Email,SkillProfile,Available")] MaintenanceStaff maintenanceStaff)
        {
            if (id != maintenanceStaff.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceStaffExists(maintenanceStaff.StaffId))
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
            return View(maintenanceStaff);
        }

        // GET: MaintenanceStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceStaff = await _context.MaintenanceStaffs
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (maintenanceStaff == null)
            {
                return NotFound();
            }

            return View(maintenanceStaff);
        }

        // POST: MaintenanceStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceStaff = await _context.MaintenanceStaffs.FindAsync(id);
            if (maintenanceStaff != null)
            {
                _context.MaintenanceStaffs.Remove(maintenanceStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceStaffExists(int id)
        {
            return _context.MaintenanceStaffs.Any(e => e.StaffId == id);
        }
    }
}
