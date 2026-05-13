using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PLMP_MVC.Controllers
{
    public class PublicLookupController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PublicLookupController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int requestId, string phoneNumber)
        {
            var client = _httpClientFactory.CreateClient();

            var apiUrl =
                $"https://localhost:7111/api/PublicMaintenanceLookup?requestId={requestId}&phoneNumber={phoneNumber}";

            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "No maintenance request found. Please check ticket number and phone number.";
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<MaintenanceLookupResult>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(result);
        }
    }

    public class MaintenanceLookupResult
    {
        public int RequestId { get; set; }
        public string? CategoryType { get; set; }
        public string? Priority { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TenantName { get; set; }
        public string? TenantPhone { get; set; }
        public string? StaffName { get; set; }
    }
}