using PLMP_S6G5.Models;

namespace PLMP_MVC.Services
{
    public class LeaseExpireService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public LeaseExpireService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PLMPS6G5>();

                    var expiredLeases = context.Leases
                        .Where(l => l.EndDate <= DateTime.Now && l.LeaseStatus != "Termination")
                        .ToList();

                    foreach (var lease in expiredLeases)
                    {
                        lease.LeaseStatus = "Termination";
                    }

                    await context.SaveChangesAsync();

                    var unitsToCheck = context.Units
                        .Where(u => u.AvailabilityStatus == "Leased")
                        .ToList();

                    foreach (var unit in unitsToCheck)
                    {
                        var hasTerminatedLease = context.Leases
                            .Any(l => l.UnitId == unit.UnitId && l.LeaseStatus == "Termination");

                        if (hasTerminatedLease)
                        {
                            unit.AvailabilityStatus = "Vacant";
                        }
                    }

                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
