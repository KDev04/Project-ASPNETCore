using LaptopStoreApi.EndPoints;
using Microsoft.EntityFrameworkCore;
// using LaptopStoreApi.Data;
using LaptopStoreApi.Services;
using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using LaptopStoreApi.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using LaptopStoreApi.Constants;
using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using LaptopStoreApi.Models;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(cfg =>
    {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]!);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
    options.AddPolicy(name: "AnyOrigin",
        cfg =>
        {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
});
builder.Services.AddControllers(options =>
{
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        (x) => $"The value '{x}' is invalid.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        (x) => $"The field {x} must be a number.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (x, y) => $"The value '{x}' is not valid for {y}.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
        () => $"A value is required.");

    options.CacheProfiles.Add("NoCache",
        new CacheProfile() { NoStore = true });
    options.CacheProfiles.Add("Any-60",
        new CacheProfile() { Location = ResponseCacheLocation.Any, Duration = 60 });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ParameterFilter<SortColumnFilter>();
    options.ParameterFilter<SortOrderFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<ApiDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("LaptopDB2"));
});

builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.Password.RequiredLength = 4;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApiDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthorization(options =>
{
    //Product
    options.AddPolicy("CreateProductPolicy", policy => policy.RequireClaim("CreateProduct", "true"));
    options.AddPolicy("UpdateProductPolicy", policy => policy.RequireClaim("UpdateProduct", "true"));
    options.AddPolicy("DeleteProductPolicy", policy => policy.RequireClaim("DeleteProduct", "true"));

    //Category
    options.AddPolicy("CreateCategoryPolicy", policy => policy.RequireClaim("CreateCategory", "true"));
    options.AddPolicy("UpdateCategoryPolicy", policy => policy.RequireClaim("UpdateCategory", "true"));
    options.AddPolicy("DeleteCategoryPolicy", policy => policy.RequireClaim("DeleteCategory", "true"));

    //User
    options.AddPolicy("ReadUserPolicy", policy => policy.RequireClaim("ReadUser", "true"));
    options.AddPolicy("CreateUserPolicy", policy => policy.RequireClaim("CreateUser", "true"));
    options.AddPolicy("UpdateUserPolicy", policy => policy.RequireClaim("UpdateUser", "true"));
    options.AddPolicy("DeleteUserPolicy", policy => policy.RequireClaim("DeleteUser", "true"));

    //Order
    options.AddPolicy("ReadOrderPolicy", policy => policy.RequireClaim("ReadOrder", "true"));
    options.AddPolicy("CreateOrderPolicy", policy => policy.RequireClaim("CreateOrder", "true"));
    options.AddPolicy("UpdateOrderPolicy", policy => policy.RequireClaim("UpdateOrder", "true"));
    options.AddPolicy("DeleteOrderPolicy", policy => policy.RequireClaim("DeleteOrder", "true"));

});
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
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
    };
});
builder.Services.AddScoped<ILapRepo2, LapRepo2>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




await SeedDatabase.CreateData(app);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.UseStaticFiles();
LocationEndpointsConfig.AddEndpoints(app);




app.MapGet("/auth/test/1",
    [Authorize]
[EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
    {
        return Results.Ok("You are authorized!");
    });
app.MapGet("/auth/test/2",
    [Authorize(Roles = RoleNames.Moderator)]
[EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
    {
        return Results.Ok("You are authorized!");
    });

app.MapGet("/auth/test/3",
    [Authorize(Roles = RoleNames.Administrator)]
[EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
    {
        return Results.Ok("You are authorized!");
    });
app.Run();
