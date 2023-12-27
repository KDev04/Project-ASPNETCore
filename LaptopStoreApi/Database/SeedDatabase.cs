using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Database
{
    public class SeedDatabase
    {

        public static async Task CreateData(IApplicationBuilder app)
        {

            ApiDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<ApiDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Laptops.Any())
            {
                context.Laptops.AddRange(
                    new Laptop
                    {
                        Name = "Laptop 1",
                        Price = 1000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-1.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 2",
                        Price = 2000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-2.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 3",
                        Price = 3000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-3.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 4",
                        Price = 4000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-4.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 5",
                        Price = 5000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-5.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 6",
                        Price = 6000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-6.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 7",
                        Price = 7000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-7.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 8",
                        Price = 8000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-8.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 9",
                        Price = 9000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-9.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 10",
                        Price = 1000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-10.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 11",
                        Price = 1100,
                        Quantity = 1000,
                        ImgPath = "/images/caro-11.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 12",
                        Price = 1200,
                        Quantity = 1000,
                        ImgPath = "/images/caro-12.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 12",
                        Price = 1200,
                        Quantity = 1000,
                        ImgPath = "/images/caro-13.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    }
                    ,
                    new Laptop
                    {
                        Name = "Laptop 12",
                        Price = 1200,
                        Quantity = 1000,
                        ImgPath = "/images/caro-14.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    }
                    
                    
                    );
                context.SaveChanges();
            }
        }
    }
}
