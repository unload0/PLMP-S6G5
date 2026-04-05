using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View(await _context.Units.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _context.Units.FirstOrDefaultAsync(m => m.UnitId == id);

            if (unit == null) return NotFound();

            return View(unit);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Unit unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            return View(unit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Unit unit)
        {
            if (id != unit.UnitId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _context.Units.FirstOrDefaultAsync(m => m.UnitId == id);

            if (unit == null) return NotFound();

            return View(unit);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}