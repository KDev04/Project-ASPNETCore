using Microsoft.EntityFrameworkCore;
namespace LaptopStoreApi.Data
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
            if(!context.Laptops.Any())
            {

            }
        }

       
    }
}
