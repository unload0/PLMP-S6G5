using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PLMP_Reports.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Dashboard()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "ReportAuth");
            }

            var client = _httpClientFactory.CreateClient("Reporting");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/ApiReports/summary");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "ReportAuth");
            }

            var json = await response.Content.ReadAsStringAsync();

            var report = JsonSerializer.Deserialize<ReportSummary>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(report);
        }
    }

    public class ReportSummary
    {
        public int TotalBuildings { get; set; }
        public int TotalUnits { get; set; }
        public int TotalTenants { get; set; }
        public int TotalLeases { get; set; }

        public int VacantUnits { get; set; }
        public int LeasedUnits { get; set; }

        public int ActiveLeases { get; set; }
        public int TerminatedLeases { get; set; }

        public int OverduePayments { get; set; }

        public int OpenMaintenanceRequests { get; set; }
    }
}