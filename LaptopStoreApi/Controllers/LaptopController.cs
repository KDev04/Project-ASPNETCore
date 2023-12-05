using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ApplicationLaptopDbContext _logger;
        public LaptopController(ApplicationLaptopDbContext logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetLaptops")]
        public IEnumerable<Laptop> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Laptop
            {
                TenLaptop = "EFG"

            })
        .ToArray();
        }
        [HttpGet("{Name}")]
        public IActionResult GetByName(string Name)
        {
            var laptop = _logger.Laptops.ToList().FirstOrDefault(l => l.TenLaptop == Name);
            if (laptop != null)
            {
                return Ok(laptop);
            }
            else { return NotFound(); }
        }
        [HttpPost(Name = "CreateNewLaptop")]
        public async Task<IActionResult> CreateNewLaptop([FromForm]  LaptopModel model)
        {
            
            try
            {
                string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image?.FileName);
                string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Image");
                string imgFilePath = Path.Combine(imgFolderPath, imgFileName);
                if (!Directory.Exists(imgFolderPath))
                {
                    Directory.CreateDirectory(imgFolderPath);
                }
                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var stream = new FileStream(imgFilePath, FileMode.Create))
                    {
                        await model.Image?.CopyToAsync(stream);
                    }
                }
                
                var laptop = new Laptop
                {
                    TenLaptop = model.TenLaptop,
                    ImgPath = imgFilePath,

                };
                _logger.Add(laptop);
                _logger.SaveChanges();
                return Ok(laptop);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
