using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Services
{
    public class LapRepo2 : ILapRepo2
    {
        private readonly ApiDbContext _context;
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
            var laptop = new Laptop
            {
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ImgPath = "Image/" + imgFileName
            };
            _context.Laptops.Add(laptop);
            await _context.SaveChangesAsync();
            return laptop;
        }

        public async Task Delete(int id)
        {
            var laptop = await _context.Laptops.FirstOrDefaultAsync(lp => lp.LaptopId == id);
            if (laptop != null)
            {
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
    }
}
