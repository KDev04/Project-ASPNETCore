using LaptopStoreWebApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LaptopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<LaptopDbContext>()
    .AddApiEndpoints();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapIdentityApi<User>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/test", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}").RequireAuthorization();
SeedData.CreateData(app);
app.Run();

