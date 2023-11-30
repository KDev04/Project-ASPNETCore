using Microsoft.EntityFrameworkCore;
namespace LaptopStoreApi.Models
{
    public class SeedData
    {
        public static void CreateData(IApplicationBuilder app)
        {
            ApplicationLaptopDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<ApplicationLaptopDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Laptops.Any())
            {
                context.Laptops.AddRange
                    (
                    new Laptop
                    {
                        Name = "Gigabyte G5",
                        Price = 18000000,
                        Type = 15.6,
                        Color = "Black",
                        Year = 2016,
                        Description = "A vipro Laptop",
                        Status = "ok",
                        Category = "Gigabyte"

                    },
                    new Laptop
                    {
                        Name = "Gigabyte MD7",
                        Price = 18000000,
                        Type = 17,
                        Color = "Black",
                        Year = 2019,
                        Description = "A vipro Laptop",
                        Status = "hot",
                        Category = "Gigabyte"

                    }


                    );
            }
        }
    }
}
