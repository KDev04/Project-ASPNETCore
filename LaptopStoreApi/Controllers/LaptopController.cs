using LaptopStoreApi.Constants;
using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ILapRepo2 _repository;
        private readonly ApiDbContext _dbContext;

        public LaptopController(ILapRepo2 repo2, ApiDbContext dbContext)
        {
            _repository = repo2;
            _dbContext = dbContext;
        }

        /*        [Authorize]*/
        [HttpGet]
        public async Task<IActionResult> GetLaptops()
        {
            try
            {
                var laptops = await _repository.GetAll();
                return Ok(laptops);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /*        [Authorize]*/
        [HttpGet]
        public IActionResult Filter(
            string name,
            decimal? from,
            decimal? to,
            string sortBy,
            int page = 1
        )
        {
            try
            {
                var laptops = _repository.Filter(name, from, to, sortBy, page);
                return Ok(laptops);
            }
            catch
            {
                return BadRequest("khong hoat dong");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                var searchResult = await _repository.Search(keyword);
                return Ok(searchResult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = RoleNames.Moderator)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLaptop(int id)
        {
            try
            {
                var laptop = await _repository.GetById(id);
                if (laptop == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(laptop);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{categoryId}")]
        public ActionResult<IEnumerable<Laptop>> GetLaptopsByCategoryId(int categoryId)
        {
            var laptops = _dbContext.LaptopCategories
                .Where(lc => lc.CategoryId == categoryId)
                .Select(lc => lc.Laptop)
                .ToList();

            if (laptops.Count == 0)
            {
                return NotFound();
            }

            return Ok(laptops);
        }
        [HttpGet]
        public async Task<List<Laptop>> SearchByLaptopName(string name)
        {
            try
            {
                var laptops = await _dbContext.Laptops
                    .Where(l => l.Name.Contains(name))
                    .ToListAsync();

                return laptops;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ tại đây
                throw new Exception("Có lỗi xảy ra khi tìm kiếm laptop.", ex);
            }
        }
        [HttpGet]
        public async Task<List<Laptop>> SearchByLaptopPrice(decimal from , decimal to)
        {
            try
            {
                if (to >= from )
                {
                    var laptops = await _dbContext.Laptops
                    .Where(l => l.Price >= from && l.Price <= to)
                    .ToListAsync();

                    return laptops;
                }
                else return new List<Laptop> { };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ tại đây
                throw new Exception("Có lỗi xảy ra khi tìm kiếm laptop.", ex);
            }
        }
        [Authorize(Roles = RoleNames.Moderator)]
        [HttpPost]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<IActionResult> Add([FromForm] LapModel2 model)
        {
            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    var newLaptop = await _repository.Add(model);
                    return Ok(newLaptop);
                }
                else
                {
                    return BadRequest("No image file found");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLaptop(int id, [FromForm] LapModel2 Loadlaptop)
        {
            if (Loadlaptop.Image == null || Loadlaptop.Image.Length == 0)
            {
                // Xử lý khi không có tệp hình ảnh được gửi lên
                // Ví dụ: trả về lỗi hoặc thông báo không có tệp hình ảnh
                throw new Exception("Không có tệp hình ảnh được gửi lên.");
            }

            string imgFileName =
                Guid.NewGuid().ToString() + Path.GetExtension(Loadlaptop?.Image?.FileName);
            string imgFolderPath = Path.Combine("wwwroot/Image"); // Thư mục "wwwroot/Image"
            string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

            if (!Directory.Exists(imgFolderPath))
            {
                Directory.CreateDirectory(imgFolderPath);
            }

            using (var stream = new FileStream(imgFilePath, FileMode.Create))
            {
                await Loadlaptop.Image.CopyToAsync(stream);
            }
            var laptop = await _dbContext.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            laptop.Name = Loadlaptop.Name;
            laptop.Price = Loadlaptop.Price;
            laptop.Quantity = Loadlaptop.Quantity;
            laptop.Description = Loadlaptop.Description;
            laptop.ImgPath = "Image/" + imgFileName;
            laptop.Brand = Loadlaptop.Brand;
            laptop.SeriesLaptop = Loadlaptop.SeriesLaptop;
            laptop.Cpu = Loadlaptop.Cpu;
            laptop.Chip = Loadlaptop.Chip;
            laptop.RAM = Loadlaptop.RAM;
            laptop.Memory = Loadlaptop.Memory;
            laptop.BlueTooth = Loadlaptop.BlueTooth;
            laptop.Keyboard= Loadlaptop.Keyboard;
            laptop.OperatingSystem = Loadlaptop.OperatingSystem;
            laptop.Pin = Loadlaptop.Pin;
            laptop.weight = Loadlaptop.weight;
            laptop.Accessory = Loadlaptop.Accessory;
            laptop.Screen = Loadlaptop.Screen;
            laptop.LastModifiedDate = DateTime.Now;
            _dbContext.Laptops.Update(laptop);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetLinhkien()
        {
            try
            {
                // Lấy tất cả sản phẩm từ bảng Laptop có Type là "LK"
                var allLaptops = await _repository.GetAll();
                var linhkien = allLaptops.Where(laptop => laptop.Type == "LK").ToList();

                return Ok(linhkien);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLaptop()
        {
            try
            {
                // Lấy tất cả sản phẩm từ bảng Laptop có Type là "LK"
                var allLaptops = await _repository.GetAll();
                var linhkien = allLaptops.Where(laptop => laptop.Type == "Laptop").ToList();

                return Ok(linhkien);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPK()
        {
            try
            {
                // Lấy tất cả sản phẩm từ bảng Laptop có Type là "LK"
                var allLaptops = await _repository.GetAll();
                var linhkien = allLaptops.Where(laptop => laptop.Type == "PK").ToList();

                return Ok(linhkien);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCard()
        {
            try
            {
                // Lấy tất cả sản phẩm từ bảng Laptop có Type là "LK"
                var allLaptops = await _repository.GetAll();
                var linhkien = allLaptops.Where(laptop => laptop.Type == "Card").ToList();

                return Ok(linhkien);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatus(int id)
        {
            try
            {
                // Truy vấn bảng LaptopStatus dựa trên id
                var laptopStatusList = await _dbContext
                    .LaptopStatuses.Where(ls => ls.LaptopId == id)
                    .ToListAsync();

                if (laptopStatusList == null || !laptopStatusList.Any())
                {
                    return NotFound(); // Trả về HTTP 404 Not Found nếu không tìm thấy dữ liệu
                }
                else
                {
                    return Ok(laptopStatusList); // Trả về dữ liệu dưới dạng JSON
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
