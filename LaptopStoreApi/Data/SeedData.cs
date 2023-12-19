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
                /*var laptops = new[]
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
                };*/

                context.Laptops.AddRange(
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
                        Loai="Dell" // Thay 1 bằng Id thực tế của Category
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
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 3",
                        Gia = 1300,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "Leveno",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 3",
                        ImgPath = "/images/laptop3.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "Leveno"
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 4",
                        Gia = 1400,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "Acer",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 2",
                        ImgPath = "/images/laptop4.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "Acer" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 5",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 5",
                        ImgPath = "/images/laptop5.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP"// Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 6",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 6",
                        ImgPath = "/images/laptop6.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 7",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 7",
                        ImgPath = "/images/laptop7.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 8.png",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 8",
                        ImgPath = "/images/laptop8.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 9",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 2",
                        ImgPath = "/images/laptop9.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 10",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 10",
                        ImgPath = "/images/laptop10.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 11",
                        Gia = 1200,
                        GiamGia = 5,
                        LoaiManHinh = 14,
                        Hangsx = "HP",
                        Mau = "Bạc",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 11",
                        ImgPath = "/images/laptop11.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP"  // Thay 2 bằng Id thực tế của Category
                    },
                    new Laptop
                    {
                        TenLaptop = "Laptop 12",
                        Gia = 1900,
                        GiamGia = 5,
                        LoaiManHinh = 17,
                        Hangsx = "GIGABYTE",
                        Mau = "Đen",
                        NamSanXuat = 2021,
                        Mota = "Laptop mô tả 12",
                        ImgPath = "/images/laptop12.png",
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Loai = "HP" // Thay 2 bằng Id thực tế của Category
                    });
                context.SaveChanges();
            }
        }

       
    }
}
