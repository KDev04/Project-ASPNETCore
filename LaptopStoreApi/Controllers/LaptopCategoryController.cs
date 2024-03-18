using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;


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
        public async Task<ActionResult<List<ConsolidatedCategory>>> GetAllData()
        {
            var consolidatedData = await _dbContext.LaptopCategories
                .Include(lc => lc.Category)
                .Include(lc => lc.Laptop)
                .GroupBy(lc => lc.CategoryId)
                .Select(g => new ConsolidatedCategory
                {
                    CategoryId = g.Key,
                    CategoryName = g.FirstOrDefault().Category.CategoryName,
                    Laptops = g.Select(lc => lc.Laptop).ToList()
                })
                .ToListAsync();

            return Ok(consolidatedData);
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
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var laptopCategory = await _dbContext.LaptopCategories
                .Include(lc => lc.Category)
                .Include(lc => lc.Laptop)
                .FirstOrDefaultAsync(lc => lc.LaptopId == laptopId && lc.CategoryId == categoryId);

            if (laptopCategory == null)
            {
                return NotFound();
            }

            var serializedLaptopCategory = JsonSerializer.Serialize(laptopCategory, options);

            return Ok(serializedLaptopCategory);
        }
        [HttpPost]
        public async Task<ActionResult> CreateLaptopCategory([FromForm] int LaptopId, [FromForm] int CategoryId)
        {
            var category = await _dbContext.Categories.FindAsync(CategoryId);
            var laptop = await _dbContext.Laptops.FindAsync(LaptopId);

            if (category == null)
            {
                // Không tìm thấy danh mục
                return BadRequest("Không tìm thấy danh mục này!");
            }

            if (laptop == null)
            {
                // Không tìm thấy Laptop
                return BadRequest("Không tìm thấy Laptop này!");
            }

            if (_dbContext.LaptopCategories.Any(lc => lc.LaptopId == LaptopId && lc.CategoryId == CategoryId))
            {
                // LaptopCategory đã tồn tại
                return BadRequest("LaptopCategory đã tồn tại!");
            }

            LaptopCategory lap_cate = new LaptopCategory
            {
                CategoryId = CategoryId,
                Category = category,
                LaptopId = LaptopId,
                Laptop = laptop
            };

            _dbContext.LaptopCategories.Add(lap_cate);
            category.LaptopCategories.Add(lap_cate);
            _dbContext.Categories.Update(category);
            laptop.LaptopCategories.Add(lap_cate);
            _dbContext.Laptops.Update(laptop);

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

            return Ok();
        }
    }
}

