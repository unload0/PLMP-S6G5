using Microsoft.AspNetCore.SignalR;

namespace PLMP_MVC.Hubs
{
    public class MaintenanceHub : Hub
    {
        public override async Task OnConnectedAsync()
        {   
            if (Context.User != null)
            {
                if (Context.User.IsInRole("Admin"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                }
                if (Context.User.IsInRole("Staff"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Staff");
                }
            }
            await base.OnConnectedAsync();
        }
    }
}