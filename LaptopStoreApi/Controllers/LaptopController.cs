using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using LaptopStoreApi.Services;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        /*private readonly ApplicationLaptopDbContext _logger;*/
        private readonly ILaptopRepository _repository;
        public LaptopController(ILaptopRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_repository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("Get/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var data = _repository.GetById(id);
                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(data);

                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("Add")]
        public IActionResult Add([FromForm] LaptopModel model)
        {
            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    return Ok(_repository.Add(model));
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
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromForm] LaptopModel model)
        {
            if (id != model.MaLaptop)
            {
                return BadRequest();
            }
            try
            {   
                if (model.Image != null && model.Image.Length > 0)
                {
                    return Ok(_repository.Add(model));
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



        /*public LaptopController(ApplicationLaptopDbContext logger)
        {
            _logger = logger;
        }*/
        /*[HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var laptops = _logger.Laptops.ToList();
            return Ok(laptops);
        }
*/
        /*[HttpGet("Hello", Name = "GetString")]
        public IActionResult GetString()
        {
            return Ok("Hello");
        }*/
        /*[HttpGet("Get/{Name}")]
        public IActionResult GetByName(string Name)
        {
            var laptop = _logger.Laptops.ToList().FirstOrDefault(l => l.TenLaptop == Name);
            if (laptop != null)
            {
                return Ok(laptop);
            }
            else { return NotFound(); }
        }
        [HttpPost("New")]
        public async Task<IActionResult> CreateNewLaptop([FromForm] LaptopModel model)
        {
            try
            {
                if (model.Image != null && model.Image.Length > 0)
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

                    _logger.Add(laptop);
                    _logger.SaveChanges();

                    return Ok(laptop);
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
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteById (Guid id)
        {
            var laptop = _logger.Laptops.FirstOrDefault(l => l.MaLaptop == id);
            if (laptop is not null)
            {
                _logger.Laptops.Remove(laptop);
                _logger.SaveChanges();
                return Ok("Đã xóa");
            }
            else
            {
                return NotFound();
            }
        } */
    }
}
