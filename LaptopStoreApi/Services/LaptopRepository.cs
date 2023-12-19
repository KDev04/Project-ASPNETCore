using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Services
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly ApplicationLaptopDbContext _context;
        public static int Page_size { set; get; } = 5;
        public LaptopRepository(ApplicationLaptopDbContext context)
        {
            _context = context;
        }

        /*public Laptop Add([FromForm] LaptopModel model)
        {
            if (model.Image == null || model.Image.Length == 0)
            {
                // Xử lý khi không có tệp hình ảnh được gửi lên
                // Ví dụ: trả về lỗi hoặc thông báo không có tệp hình ảnh
            }

            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model?.Image?.FileName);
            string imgFolderPath = Path.Combine("wwwroot/Image"); // Thư mục "wwwroot/Image"
            string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

            if (!Directory.Exists(imgFolderPath))
            {
                Directory.CreateDirectory(imgFolderPath);
            }

            using (var stream = new FileStream(imgFilePath, FileMode.Create))
            {
                model?.Image?.CopyTo(stream);
            }

            var laptop = new Laptop
            {
                TenLaptop = model?.TenLaptop ?? "",
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
        public List<Laptop> Search (string keyword)
        {
            var searchResult = _context.Laptops.Where(l => l.TenLaptop.Contains(keyword)).ToList();
            if (searchResult.Count > 0)
            {
                return searchResult;
            }
            else return null;
        }*/
        public async Task<List<Laptop>> GetAll()
        {
            var laptops = await _context.Laptops.ToListAsync();
            return laptops;
        }

        public List<Laptop> Filter(string name, decimal from, decimal to, string sortBy, int page = 1)
        {
            var allLaptops = _context.Laptops.AsQueryable();
            #region Filtering
            if (!string.IsNullOrEmpty(name))
            {
                allLaptops = allLaptops.Where(l => l.TenLaptop.Contains(name));
            }
            if (from.HasValue) 
            { 
                allLaptops = allLaptops.Where(l => l.Gia >= from);
            }
            if (to.HasValue)
            {
                allLaptops = allLaptops.Where(l => l.Gia <= to);
            }
            #endregion
            #region Sorting
            // mac dinh la name
            allLaptops = allLaptops.OrderBy(l => l.TenLaptop);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "TenLaptop_asc": allLaptops = allLaptops.OrderBy(l => l.TenLaptop); break;
                    case "TenLaptop_desc": allLaptops = allLaptops.OrderByDescending(l => l.TenLaptop); break;
                    case "Gia_asc": allLaptops = allLaptops.OrderBy(l => l.Gia); break;
                    case "Gia_desc": allLaptops = allLaptops.OrderByDescending(l => l.Gia); break;
                }
            }
            #endregion
            /*#region Paging
            allLaptops = allLaptops.Skip((page - 1) * Page_size).Take(Page_size);
            #endregion 
            */
            var db = Paging<Laptop>.Create(allLaptops, page, Page_size);
            var result = db.Select(model => new Laptop
            {
                MaLaptop = model.MaLaptop,
                TenLaptop = model.TenLaptop,
                Gia = model.Gia,
                GiamGia = model.GiamGia,
                Mau = model.Mau,
                LoaiManHinh = model.LoaiManHinh,
                NamSanXuat = model.NamSanXuat,
                Mota = model.Mota,
                CategoryId = model.CategoryId,
                CreateDate = model.CreateDate,
                LastModifiedDate = model.LastModifiedDate,
                ImgPath = model.ImgPath,
            });
            return result.ToList();
        }
        public async Task<Laptop> GetById(int id)
        {
            var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.MaLaptop == id);
            if (laptop != null)
            {
                return laptop;
            }
            return null;
        }

        public async Task<Laptop> Add(LaptopModel model)
        {
            if (model.Image == null || model.Image.Length == 0)
            {
                // Xử lý khi không có tệp hình ảnh được gửi lên
                // Ví dụ: trả về lỗi hoặc thông báo không có tệp hình ảnh
                throw new Exception("Không có tệp hình ảnh được gửi lên.");
            }

            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model?.Image?.FileName);
            string imgFolderPath = Path.Combine("wwwroot/Image"); // Thư mục "wwwroot/Image"
            string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

            if (!Directory.Exists(imgFolderPath))
            {
                Directory.CreateDirectory(imgFolderPath);
            }

            using (var stream = new FileStream(imgFilePath, FileMode.Create))
            {
                await model?.Image?.CopyToAsync(stream);
            }

            var laptop = new Laptop
            {
                TenLaptop = model?.TenLaptop ?? "",
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
            await _context.SaveChangesAsync();
            return laptop;
        }

        public async Task Update(LaptopModel model)
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
                await model.Image.CopyToAsync(stream);
            }

            var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.MaLaptop == model.MaLaptop);
            if (laptop != null)
            {
                laptop.TenLaptop = model.TenLaptop;
                laptop.Gia = model.Gia;
                laptop.GiamGia = model.GiamGia;
                laptop.Mau = model.Mau;
                laptop.LoaiManHinh = model.LoaiManHinh;
                laptop.NamSanXuat = model.NamSanXuat;
                laptop.Mota = model.Mota;
                laptop.CategoryId = model.CategoryId;
                laptop.ImgPath = "Image/" + imgFileName;
                laptop.LastModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var laptop = await _context.Laptops.FirstOrDefaultAsync(lp => lp.MaLaptop == id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Laptop>> Search(string keyword)
        {
            var searchResult = await _context.Laptops.Where(l => l.TenLaptop.Contains(keyword)).ToListAsync();
            return searchResult;
        }
    }
}
