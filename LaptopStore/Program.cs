using LaptopStore.Controllers;
using LaptopStore.PublicHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using QuestPDF;

var builder = WebApplication.CreateBuilder(args);






// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSession();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();

app.MapFallbackToPage("/admin/{*catchall}", "/Admin/index");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Home}/{action=Auth}");
LocationEndPointsConfig.AddEndpoints(app);
app.Run();
