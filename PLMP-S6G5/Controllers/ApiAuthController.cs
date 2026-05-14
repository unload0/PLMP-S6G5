using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PLMP_S6G5.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PLMP_S6G5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthController : ControllerBase
    {
        private readonly PLMPS6G5 _context;

        public ApiAuthController(PLMPS6G5 context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApiLoginRequest request)
        {
            var manager = await _context.PropertyManagers
                .FirstOrDefaultAsync(m => m.Email == request.Username);

            if (manager == null || request.Password != "123")
            {
                return Unauthorized("Invalid username or password.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, manager.Name),
                new Claim(ClaimTypes.NameIdentifier, manager.ManagerId.ToString()),
                new Claim(ClaimTypes.Role, "PropertyManager")
            };

            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes("PLMP_SUPER_SECRET_KEY_123456789_ABCDE"));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "PLMP",
                audience: "PLMPReports",
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                name = manager.Name,
                role = "PropertyManager"
            });
        }
    }

    public class ApiLoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}