using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UnitsController : Controller
    {
        private readonly PLMPS6G5 _context;

        public UnitsController(PLMPS6G5 context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var units = await _context.Units.ToListAsync();
            return View(units);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Buildings = new SelectList(
                await _context.Buildings.ToListAsync(),
                "BuildingId",
                "Name"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Unit unit)
        {
            ModelState.Remove("Building");
            ModelState.Remove("Leases");

            if (!ModelState.IsValid)
            {
                ViewBag.Buildings = new SelectList(
                    await _context.Buildings.ToListAsync(),
                    "BuildingId",
                    "Name",
                    unit.BuildingId
                );

                return View(unit);
            }

            if (string.IsNullOrWhiteSpace(unit.AvailabilityStatus))
                unit.AvailabilityStatus = "Vacant";

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Unit added successfully.";
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Details(int id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);
            if (unit == null)
                return NotFound();

            return View(unit);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
                return NotFound();

            ViewBag.Buildings = new SelectList(
                await _context.Buildings.ToListAsync(),
                "BuildingId",
                "Name",
                unit.BuildingId
            );

            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Unit unit)
        {
            if (id != unit.UnitId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Buildings = new SelectList(
                    await _context.Buildings.ToListAsync(),
                    "BuildingId",
                    "Name",
                    unit.BuildingId
                );

                return View(unit);
            }

            _context.Update(unit);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Unit updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);
            if (unit == null)
                return NotFound();

            return View(unit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
                return NotFound();

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Unit deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}