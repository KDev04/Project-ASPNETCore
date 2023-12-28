using LaptopStoreApi.Constants;
using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Filter(string name, decimal? from, decimal? to, string sortBy, int page = 1)
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

            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(Loadlaptop?.Image?.FileName);
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
            laptop.LastModifiedDate = DateTime.Now;
            _dbContext.Laptops.Update(laptop);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
