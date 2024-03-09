using LaptopStoreApi.Database;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public CategoryController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        [HttpGet("{laptopId}")]
        public ActionResult<IEnumerable<Category>> GetCategoriesByLaptopId(int laptopId)
        {
            var categories = _dbContext.LaptopCategories
                .Where(lc => lc.LaptopId == laptopId)
                .Select(lc => lc.Category)
                .ToList();

            if (categories.Count == 0)
            {
                return NotFound();
            }

            return Ok(categories);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromForm] string CategoryName)
        {
            Category category = new Category { CategoryName = CategoryName };
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
