using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LaptopStoreApi.Services
{
    public class LapRepo2 : ILapRepo2
    {
        private readonly ApiDbContext _context;
        public static int Page_size { set; get; } = 5;
        public LapRepo2(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<Laptop> Add(LapModel2 model)
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
                await model.Image.CopyToAsync(stream);
            }
            Laptop laptop = new Laptop
            {
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
                Description = model.Description,
                BigPrice = model.BigPrice,
                Color = model.Color,
                Type = model.Type,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ImgPath = "Image/" + imgFileName,
                Brand = model.Brand,
                SeriesLaptop = model.SeriesLaptop,
                Cpu = model.Cpu,
                Chip = model.Chip,
                RAM = model.RAM,
                Memory = model.Memory,
                BlueTooth = model.BlueTooth,
                Keyboard = model.Keyboard,
                OperatingSystem = model.OperatingSystem,
                Pin = model.Pin,
                weight = model.weight,
                Accessory = model.Accessory,
                Screen = model.Screen,
            };
            _context.Laptops.Add(laptop);
            await _context.SaveChangesAsync();
            var category = await _context.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                // Không tìm thấy danh mục
                throw new Exception("Không tìm thấy danh mục.");
            }
            LaptopCategory lap_cate = new LaptopCategory()
            {
                CategoryId = model.CategoryId,
                Category = category,
                LaptopId = laptop.LaptopId,
                Laptop = laptop
            };
            _context.LaptopCategories.Add(lap_cate);
            await _context.SaveChangesAsync();
            laptop.LaptopCategories.Add(lap_cate);
            _context.Laptops.Update(laptop);
            category.LaptopCategories.Add(lap_cate);
            _context.Categories.Update(category);

            await _context.SaveChangesAsync();
            return laptop;
        }

        public async Task Delete(int LaptopId)
        {
            var laptop = await _context.Laptops.FirstOrDefaultAsync(lp => lp.LaptopId == LaptopId);
            var laptopCategories = await _context.LaptopCategories
            .Where(lc => lc.LaptopId == LaptopId)
            .ToListAsync();
            _context.LaptopCategories.RemoveRange(laptopCategories);
            if (laptop != null)
            {
                _context.LaptopCategories.RemoveRange(laptopCategories);
                await _context.SaveChangesAsync();
                _context.Laptops.Remove(laptop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Laptop>> GetAll()
        {

            var laptops = await _context.Laptops.ToListAsync();
            return laptops;
        }

        public async Task<Laptop> GetById(int id)
        {
            var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.LaptopId == id);
            if (laptop != null)
            {
                return laptop;
            }
            return new Laptop();
        }

        public async Task<List<Laptop>> Search(string keyword)
        {
            var searchResult = await _context.Laptops.Where(l => l.Name.Contains(keyword)).ToListAsync();

            return searchResult;
        }

        public Task Update(LapModel2 model)
        {
            throw new NotImplementedException();
        }
        public List<Laptop> Filter(string name, decimal? from, decimal? to, string sortBy, int page = 1)
        {
            var allLaptops = _context.Laptops.AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(name))
            {
                allLaptops = allLaptops.Where(l => l.Name.Contains(name));
            }
            if (from.HasValue)
            {
                allLaptops = allLaptops.Where(l => l.Price >= from);
            }
            if (to.HasValue)
            {
                allLaptops = allLaptops.Where(l => l.Price <= to);
            }
            #endregion

            #region Sorting
            // mac dinh la name
            allLaptops = allLaptops.OrderBy(l => l.Name);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "TenLaptop_asc": allLaptops = allLaptops.OrderBy(l => l.Name); break;
                    case "TenLaptop_desc": allLaptops = allLaptops.OrderByDescending(l => l.Name); break;
                    case "Gia_asc": allLaptops = allLaptops.OrderBy(l => l.Price); break;
                    case "Gia_desc": allLaptops = allLaptops.OrderByDescending(l => l.Price); break;
                }
            }
            #endregion

            var db = Paging<Laptop>.Create(allLaptops, page, Page_size);
            var result = db.Select(model => new Laptop
            {
                LaptopId = model.LaptopId,
                Name = model.Name, 
                Price = model.Price,
                Quantity = model.Quantity,
                Description = model.Description,
                CreateDate = model.CreateDate,
                LastModifiedDate = model.LastModifiedDate,
                ImgPath = model.ImgPath,

            });

            return result.ToList();
        }
    }
}
