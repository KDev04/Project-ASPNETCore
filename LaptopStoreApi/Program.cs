using LaptopStoreApi.EndPoints;
using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationLaptopDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("LaptopDB"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
//SeedData.CreateData(app);
LocationEndpointsConfig.AddEndpoints(app);
app.Run();
