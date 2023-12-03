using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationLaptopDbContext _context;
        public CategoryController(ApplicationLaptopDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (category != null)
            {
                return Ok(category);
            }
            else { return NotFound(); }
        }
        [HttpPost] 
        public IActionResult CreateNew(CategoryModel model)
        {
            try
            {
                var category = new Category
                {
                    Name = model.Name
                };
                _context.Add(category);
                _context.SaveChanges();
                return Ok(category);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateById(int id, CategoryModel model)
        {

            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.Name = model.Name;
                _context.SaveChanges();
                return NoContent();
            }
            else { return NotFound(); }
        }
    }
}
