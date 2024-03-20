using LaptopStoreApi.Constants;
using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

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
        [HttpGet]
        public async Task<ActionResult<List<ConsolidatedLaptop>>> GetLaptopWithAllCategory()
        {
            var laptops = await _dbContext.Laptops.Include(l => l.LaptopCategories).ToListAsync();

            var consolidatedLaptops = laptops.Select(c => new ConsolidatedLaptop
            {
                Laptop = c,
                Categories = GetCategoriesByLaptopId(c.LaptopId)
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 32 // Nếu cần thiết, bạn có thể tăng giới hạn độ sâu của đối tượng lên
            };

            return new JsonResult(consolidatedLaptops, options);
        }
        [HttpGet]
        public async Task<ActionResult<List<ConsolidatedLaptop>>> SearchLaptopWithAllCategory(string key)
        {
            var laptops = await _dbContext.Laptops.Where(l=> l.Name.Contains(key)).Include(l => l.LaptopCategories).ToListAsync();

            var consolidatedLaptops = laptops.Select(c => new ConsolidatedLaptop
            {
                Laptop = c,
                Categories = GetCategoriesByLaptopId(c.LaptopId)
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 32 // Nếu cần thiết, bạn có thể tăng giới hạn độ sâu của đối tượng lên
            };

            return new JsonResult(consolidatedLaptops, options);
        }
        [HttpGet("{LaptopId}")]
        public List<Category> GetCategoriesByLaptopId(int LaptopId)
        {
            var Categories = _dbContext.LaptopCategories
                .Where(lc => lc.LaptopId == LaptopId)
                .Select(lc => lc.Category)
                .ToList();

            return Categories;
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

        [HttpGet("{keyword}")]
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
        [HttpGet("{keyword}")]
        public async Task<List<Laptop>> SearchByLaptopName(string keyword)
        {
            try
            {
                var laptops = await _dbContext.Laptops
                    .Where(l => l.Name.Contains(keyword))
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
                if (to >= from || from >=0 )
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
        /*        [Authorize(Roles = RoleNames.Moderator)]*/
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

            var laptop = await _dbContext.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Name))
            {
                laptop.Name = Loadlaptop.Name;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (Loadlaptop.Price!=0 && Loadlaptop.Price != null && Loadlaptop.Price > 0)
            {
                laptop.Price = Loadlaptop.Price;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (Loadlaptop.BigPrice != 0 && Loadlaptop.BigPrice != null && Loadlaptop.BigPrice > 0)
            {
                laptop.BigPrice = Loadlaptop.BigPrice;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (Loadlaptop.Quantity != 0 && Loadlaptop.Quantity != null && Loadlaptop.Quantity > 0)
            {
                laptop.Quantity = Loadlaptop.Quantity;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Description))
            {
                laptop.Description = Loadlaptop.Description;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Color))
            {
                laptop.Color = Loadlaptop.Color;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Brand))
            {
                laptop.Brand = Loadlaptop.Brand;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.SeriesLaptop))
            {
                laptop.SeriesLaptop = Loadlaptop.SeriesLaptop;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Cpu))
            {
                laptop.Cpu = Loadlaptop.Cpu;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Chip))
            {
                laptop.Chip = Loadlaptop.Chip;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.RAM))
            {
                laptop.RAM = Loadlaptop.RAM;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Memory))
            {
                laptop.Memory = Loadlaptop.Memory;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.BlueTooth))
            {
                laptop.BlueTooth = Loadlaptop.BlueTooth;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Keyboard))
            {
                laptop.Keyboard = Loadlaptop.Keyboard;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.OperatingSystem))
            {
                laptop.OperatingSystem = Loadlaptop.OperatingSystem;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Pin))
            {
                laptop.Pin = Loadlaptop.Pin;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.weight))
            {
                laptop.weight = Loadlaptop.weight;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Accessory))
            {
                laptop.Accessory = Loadlaptop.Accessory;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (!string.IsNullOrEmpty(Loadlaptop.Screen))
            {
                laptop.Screen = Loadlaptop.Screen;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            if (Loadlaptop.Image == null || Loadlaptop.Image.Length == 0)
            {
                laptop.ImgPath = laptop.ImgPath;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
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
                laptop.ImgPath = "Image/" + imgFileName;
                laptop.LastModifiedDate = DateTime.Now;
                _dbContext.Laptops.Update(laptop);
                await _dbContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetLinhkien()
        {
            try
            {
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


        [HttpPost]
        public async Task<IActionResult> PostOrderOffline([FromBody] List<OrderOffline> AddOrderOfflines)
        {
            try
            {

                if (AddOrderOfflines == null || AddOrderOfflines.Count == 0)
                {
                    return BadRequest("No orders to process");
                }
                else
                {
                    foreach (var order in AddOrderOfflines)
                    {
                        _dbContext.OrderOfflines.Add(order);
                    }
                }
                // Save changes to database
                await _dbContext.SaveChangesAsync();

                return Ok("Orders added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetIdOrder()
        {
            try
            {
                // Lấy ra IdOrder lớn nhất trong bảng OrderOffline
                int maxIdOrder = await _dbContext.OrderOfflines
                    .Select(o => o.IdOrder)
                    .DefaultIfEmpty()
                    .MaxAsync();

                // Tăng giá trị maxIdOrder lên 1 để có IdOrder mới
                int newIdOrder = maxIdOrder + 1;

                return Ok(newIdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}



