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

        public async Task<IActionResult> AvailableUnits()
        {
            var availableUnits = await _context.Units
                .Where(u => u.AvailabilityStatus == "Vacant")
                .ToListAsync();

            return View(availableUnits);
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        public async Task<IActionResult> ApplyForUnit(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);

            if (unit == null || unit.AvailabilityStatus != "Vacant")
            {
                return RedirectToAction("Index", "Home");
            }

            var newLease = new Lease
            {
                UnitId = unitId,
                TenantId = 1,
                ManagerId = 1,
                ApplicationStatus = "Pending",
                LeaseStatus = "Pending",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1)
            };

            _context.Leases.Add(newLease);
            await _context.SaveChangesAsync();

            return RedirectToAction("ConfirmApplication", "Units", new { id = newLease.LeaseId });
        }

        public async Task<IActionResult> ConfirmApplication(int id)
        {
            var lease = await _context.Leases
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(l => l.LeaseId == id);

            if (lease == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(lease);
        }

        public async Task<IActionResult> PayForLease(int leaseId)
        {
            var lease = await _context.Leases
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(l => l.LeaseId == leaseId);

            if (lease == null || lease.LeaseStatus != "Pending")
            {
                return RedirectToAction("Index", "Home");
            }

            var payment = new Payment
            {
                LeaseId = leaseId,
                InstallmentAmount = lease.Unit.RentAmount,
                DateOfIssue = DateTime.Now,
                Balance = 0,
                PaymentStatus = "Paid"
            };

            _context.Payments.Add(payment);
            lease.LeaseStatus = "Active";

            await _context.SaveChangesAsync();

            return RedirectToAction("ContractSigned", "Units", new { leaseId = leaseId });
        }

        public async Task<IActionResult> ContractSigned(int leaseId)
        {
            var lease = await _context.Leases
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(l => l.LeaseId == leaseId);

            if (lease == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(lease);
        }
    }
}