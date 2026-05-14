using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PLMP_Reports.Controllers
{
    public class ReportAuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportAuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient();

            var loginData = new
            {
                Username = username,
                Password = password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
    "https://localhost:7111/api/ApiAuth/login",
    content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login.";
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<LoginResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            HttpContext.Session.SetString("JWToken", result!.Token);

            return RedirectToAction("Dashboard", "Reports");
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";
    }
}