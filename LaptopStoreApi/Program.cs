using LaptopStoreApi.EndPoints;
using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Data;
using LaptopStoreApi.Services;
using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
/*builder.Services.AddDbContext<ApplicationLaptopDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("LaptopDB"));
});*/
builder.Services.AddDbContext<ApiDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("LaptopDB2"));
});
// add authentication 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    };
});
builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.Password.RequiredLength = 4;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApiDbContext>();
/*builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILaptopRepository, LaptopRepository>();*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
SeedData.CreateData(app);
LocationEndpointsConfig.AddEndpoints(app);
app.Run();
