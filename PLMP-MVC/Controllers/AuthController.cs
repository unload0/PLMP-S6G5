using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMP_S6G5.Models;

namespace PLMP_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly PLMPS6G5 _context;

        public AuthController(PLMPS6G5 context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            //users login

            var manager = await _context.PropertyManagers
             .FirstOrDefaultAsync(t => t.Email == username);

            if (manager != null && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, manager.Name),
                    new Claim(ClaimTypes.NameIdentifier, manager.ManagerId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction("Index", "Home");
            }

            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Email == username);

            if (tenant != null && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, tenant.Name ?? "User"),
                    new Claim(ClaimTypes.NameIdentifier, tenant.TenantId.ToString()),
                    new Claim(ClaimTypes.Role, "User"),
                    //new Claim("TenantId", tenant.TenantId.ToString())
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction("Dashboard", "User");
            }

            var staff = await _context.MaintenanceStaffs
                .FirstOrDefaultAsync(t => t.Email == username);

            if (staff != null && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, staff.Name ?? "Staff"),
                    new Claim(ClaimTypes.NameIdentifier, staff.StaffId.ToString()),
                    new Claim(ClaimTypes.Role, "Staff"),
                    //new Claim("StaffId", staff.StaffId.ToString())
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction("Index", "MaintenanceStaffs");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }
    }
}