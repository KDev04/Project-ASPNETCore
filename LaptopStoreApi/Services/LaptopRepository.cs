using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Services
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly ApplicationLaptopDbContext _context;
        public LaptopRepository(ApplicationLaptopDbContext context)
        {
            _context = context;
        }

        public Laptop Add([FromForm] LaptopModel model)
        {
            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            string imgFolderPath = Path.Combine("wwwroot", "Image"); // Thư mục "wwwroot/Image"
            string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

            if (!Directory.Exists(imgFolderPath))
            {
                Directory.CreateDirectory(imgFolderPath);
            }

            using (var stream = new FileStream(imgFilePath, FileMode.Create))
            {
                model.Image.CopyToAsync(stream);
            }

            var laptop = new Laptop
            {
                TenLaptop = model.TenLaptop,
                Gia = model.Gia,
                GiamGia = model.GiamGia,
                Mau = model.Mau,
                LoaiManHinh = model.LoaiManHinh,
                NamSanXuat = model.NamSanXuat,
                Mota = model.Mota,
                CategoryId = model.CategoryId,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ImgPath = "Image/" + imgFileName // Đường dẫn tương đối
            };

            _context.Laptops.Add(laptop);
            _context.SaveChanges();
            return laptop;
            
        }

        public void Delete(int id)
        {
            var lap = _context.Laptops.FirstOrDefault(lp =>lp.MaLaptop == id);
            if (lap != null)
            {
                _context.Laptops.Remove(lap);
                _context.SaveChanges();
            }
        }

        public List<Laptop> GetAll()
        {
            var laps = _context.Laptops.ToList();
            return laps;
        }

        public Laptop GetById(int id)
        {
            var lap = _context.Laptops.FirstOrDefault(l => l.MaLaptop == id);
            if (lap != null)
            {
                return lap;
            }
            return new Laptop();
        }

        public void Update([FromForm] LaptopModel model)
        {
            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            string imgFolderPath = Path.Combine("wwwroot", "Image"); // Thư mục "wwwroot/Image"
            string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

            if (!Directory.Exists(imgFolderPath))
            {
                Directory.CreateDirectory(imgFolderPath);
            }

            using (var stream = new FileStream(imgFilePath, FileMode.Create))
            {
                model.Image.CopyToAsync(stream);
            }
            var lap = _context.Laptops.FirstOrDefault(l => l.MaLaptop == model.MaLaptop);
            if (lap != null)
            {
                lap.TenLaptop = model.TenLaptop;
                lap.Gia = model.Gia;
                lap.GiamGia = model.GiamGia;
                lap.Mau = model.Mau;
                lap.LoaiManHinh = model.LoaiManHinh;
                lap.NamSanXuat = model.NamSanXuat;
                lap.Mota = model.Mota;
                lap.CategoryId = model.CategoryId;
                lap.ImgPath = "Image/" + imgFileName;
                _context.SaveChanges();
            }
        }
    }
}
