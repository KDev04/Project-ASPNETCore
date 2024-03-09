using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LaptopCategoryController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public LaptopCategoryController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaptopCategory>>> GetLaptopCategories()
        {
            var laptopCategories = await _dbContext.LaptopCategories.ToListAsync();
            return Ok(laptopCategories);
        }

        [HttpGet("{laptopId}/{categoryId}")]
        public async Task<ActionResult<LaptopCategory>> GetLaptopCategory(int laptopId, int categoryId)
        {
            var laptopCategory = await _dbContext.LaptopCategories.FindAsync(laptopId, categoryId);

            if (laptopCategory == null)
            {
                return NotFound();
            }

            return Ok(laptopCategory);
        }
      
        [HttpPost]
        public async Task<ActionResult<LaptopCategory>> CreateLaptopCategory([FromForm] int LaptopId, [FromForm] int CategoryId)
        {
            if (!_dbContext.Categories.Any(c => c.CategoryId == CategoryId))
            {
                // Không tìm thấy danh mục
                return BadRequest("Không tìm thấy danh mục này!");
            }

            if (!_dbContext.Laptops.Any(l => l.LaptopId == LaptopId))
            {
                // Không tìm thấy Laptop
                return BadRequest("Không tìm thấy Laptop này!");
            }

            if (_dbContext.LaptopCategories.Any(lc => lc.LaptopId == LaptopId && lc.CategoryId == CategoryId))
            {
                // LaptopCategory đã tồn tại
                return BadRequest("LaptopCategory đã tồn tại!");
            }

            var Category = await _dbContext.Categories.FindAsync(CategoryId);
            var Laptop = await _dbContext.Laptops.FindAsync(LaptopId);
            Console.WriteLine("OK con dee");
            LaptopCategory lap_cate = new LaptopCategory
            {
                CategoryId = CategoryId,
                Category = Category,
                LaptopId = LaptopId,
                Laptop = Laptop
            };
            _dbContext.LaptopCategories.Add(lap_cate);
            await _dbContext.SaveChangesAsync();

            return Ok("Đã thêm vào danh mục");
        }

        [HttpDelete("{laptopId}/{categoryId}")]
        public async Task<IActionResult> DeleteLaptopCategory(int laptopId, int categoryId)
        {
            var laptopCategory = await _dbContext.LaptopCategories.FindAsync(laptopId, categoryId);

            if (laptopCategory == null)
            {
                return NotFound();
            }

            _dbContext.LaptopCategories.Remove(laptopCategory);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

