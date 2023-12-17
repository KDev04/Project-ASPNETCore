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
           if (!context.Laptops.Any())
            {
                // Thêm dữ liệu mẫu cho bảng Laptops
                var laptops = new[]
                {
                    new Laptop
                    {
                        TenLaptop = "Laptop 1",
                        Gia = 1000,
                        GiamGia = 10,
                        LoaiManHinh = 15.6,
                        Hangsx = "Dell",
                        Mau = "Đen",
                        NamSanXuat = 2022,
                        Mota = "Laptop mô tả 1",
                        ImgPath = "/images/laptop1.jpg",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        CategoryId = 1 // Thay 1 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 2",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 2",
                        ImgPath = "/images/laptop2.jpg",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        CategoryId = 2 // Thay 2 bằng Id thực tế của Category
                    },
                    // Thêm các dòng dữ liệu khác tương tự
                };

                context.Laptops.AddRange(laptops);
                context.SaveChanges();
            }
        }

       
    }
}
