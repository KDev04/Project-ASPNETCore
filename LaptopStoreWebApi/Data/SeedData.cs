using Microsoft.EntityFrameworkCore;
namespace LaptopStoreWebApi.Data
{
    public class SeedData
    {
        public static void CreateData(IApplicationBuilder app)
        {
            LaptopDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<LaptopDbContext>();

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
                        ImgPath = "/images/laptop1.jpg",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 2",
                        Price = 2000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop2.jpg",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 3",
                        Price = 3000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop3.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 4",
                        Price = 4000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop4.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 5",
                        Price = 5000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop5.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 6",
                        Price = 6000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop6.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 7",
                        Price = 7000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop7.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 8",
                        Price = 8000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop8.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 9",
                        Price = 9000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop9.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 10",
                        Price = 1000,
                        Quantity = 1000,
                        ImgPath = "/images/laptop10.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 11",
                        Price = 1100,
                        Quantity = 1000,
                        ImgPath = "/images/laptop11.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Laptop 12",
                        Price = 1200,
                        Quantity = 1000,
                        ImgPath = "/images/laptop12.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    });
                context.SaveChanges();
            }
        }
    }
}
