using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Database
{
    public class SeedDatabase
    {
        public static async Task CreateData(IApplicationBuilder app)
        {
            ApiDbContext context = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<ApiDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Laptops.Any())
            {
                context.Laptops.AddRange(
                    new Laptop
                    {
                        Name = "PREDATOR TRITON 1666",
                        Description = "NVIDIA® GEFORCE RTX™ 40 SERIES LAPTOPS\nIntel® Core™ i9 Processor1\nWINDOWS 11 HOME".Trim(),
                        Type = "Laptop",
                        Price = 1800,
                        Quantity = 72,
                        ImgPath = "/images/caro-1.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "PREDATOR TRITON 17 X",
                        Description ="GEFORCE RTX™ 4090\r\nIntel® Core™ i9-13900HX\r\nWINDOWS 11 HOME\r\n32GB DDR5 / 4TB",
                         Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-2.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "PREDATOR HELIOS NEO 16",
                        Description ="GEFORCE RTX™ 4070\r\nIntel® Core™ i7-13700HX\r\nWINDOWS 11 HOME\r\n32GB DDR5 / 2TB",
                           Type = "Laptop"  ,
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-3.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "PREDATOR TRITON 14",
                        Description ="NVIDIA® GEFORCE RTX™ 40 SERIES LAPTOPS\r\nIntel® Core™ i7 Processor1\r\nWINDOWS 11 HOME\r\n32GB LPDDR5",
                         Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-4.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Swift 14",
                        Description ="13th Gen Intel® Core™ H-Series\r\nTwinAir cooling\r\nAntimicrobial Corning® Gorilla® Glass\r\nWINDOWS 11 HOME\r\n",
                        Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-5.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name ="Nitro 16 AMD",
                        Description ="12th Gen Intel® Core™ processors\r\nTwinAir cooling\r\nAntimicrobial Corning® Gorilla® Glass\r\nWINDOWS 11 HOME\r\n",
                         Type = "Laptop",
                        Price = 6000,
                        Quantity = 1000,
                        ImgPath = "/images/caro-6.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Swift 5",
                        Description ="12th Gen Intel® Core™ processors\r\nTwinAir cooling\r\nAntimicrobial Corning® Gorilla® Glass\r\nWINDOWS 11 HOME\r\n",
                         Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-7.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Swift Edge 16",
                        Description ="AMD Ryzen™ 7040 Series\r\n16-inch, 16:10, 3.2K OLED\r\nAntimicrobial Corning® Gorilla® Glass\r\nWINDOWS 11 HOME\r\n",
                         Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-8.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Swift X AMD",
                        Description = "AMD Ryzen™ 5000 Series1\r\nGeForce RTX™ 3050 Ti1\r\n16GB RAM \r\nWINDOWS 11 HOME\r\n",
                         Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-9.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Nitro 5 Intel",
                        Description ="Intel® Core™ i7 Processors1\r\nGeForce RTX™ 30 Series\r\n32GB, 3200MHZ\r\nWINDOWS 11 HOME\r\n",
                         Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-10.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Nitro 7",
                        Description ="Intel® Core™ i7 Processors1\r\nGeForce RTX™\r\n32GB, 3200MHZ\r\nWINDOWS 11 HOME\r\n",
                          Type = "Laptop",
                        Price = 1599,
                        Quantity = 72,
                        ImgPath = "/images/caro-11.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Nitro 7 AMD",
                        Description ="AMD Ryzen™ 7 7840HS\r\nNVIDIA® GeForce RTX™ 4050\r\n16 GB, DDR5 SDRAM\r\n1 TB SSD\r\n",
                           Type = "Laptop",
                        Price = 1200,
                        Quantity = 1000,
                        ImgPath = "/images/caro-12.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Nitro 17 AMD",
                        Description ="AMD Ryzen™ 7 7840HS\r\nNVIDIA® GeForce RTX™ 4050\r\n16 GB, DDR5 SDRAM\r\n1 TB SSD\r\n",
                          Type = "Laptop",
                        Price = 1199,
                        Quantity = 72,
                        ImgPath = "/images/caro-13.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    },
                    new Laptop
                    {
                        Name = "Nitro 16 AMD",
                        Description = "AMD Ryzen™ 5 7640HS processor\r\nNVIDIA® GeForce RTX™ 4050\r\n8 GB, DDR5 SDRAM\r\n512 GB SSD\r\n",
                          Type = "Laptop",
                        Price = 1099,
                        Quantity = 72,
                        ImgPath = "/images/caro-14.webp",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    }
                    new Laptop
                    {
                        Name = "PREDATOR CG48",
                        Description = "AMD Ryzen™ 5 7640HS processor\r\nNVIDIA® GeForce RTX™ 4050\r\n8 GB, DDR5 SDRAM\r\n512 GB SSD\r\n",
                          Type = "Laptop",
                        Price = 1099,
                        Quantity = 72,
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
