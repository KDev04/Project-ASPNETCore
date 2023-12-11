using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository category)
        {
            _categoryRepository = category;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_categoryRepository.GetAll());
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
                var data = _categoryRepository.GetById(id);
                if (data == null)
                {
                    return NotFound();
                } else
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
        public IActionResult Add(CategoryModel model)
        {
            try
            {
                return Ok(_categoryRepository.Add(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, CategoryModel model)
        {

            if (id != model.Id)
            {
                return BadRequest();
            }
            try
            {
                _categoryRepository.Update(model); ;
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) 
        {
            try
            {
                _categoryRepository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
