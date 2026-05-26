var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("Reporting", client =>
{
    client.BaseAddress = new Uri("https://api-plmp-s6g5-01-d0hdbzbng3b0a2h0.westeurope-01.azurewebsites.net");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ReportAuth}/{action=Login}/{id?}");

app.Run();