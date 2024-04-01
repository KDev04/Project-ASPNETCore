using LaptopStoreApi.Database;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CategoryController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }
        [HttpGet]
        public async Task<ActionResult<List<ConsolidatedCategory>>> GetAllCategoriesWithLaptopCategories()
        {
            var categories = await _dbContext.Categories.Include(c => c.LaptopCategories).ToListAsync();

            var consolidatedCategories = categories.Select(c => new ConsolidatedCategory
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Laptops = GetLaptopsByCategoryId(c.CategoryId)
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 32 // Nếu cần thiết, bạn có thể tăng giới hạn độ sâu của đối tượng lên
            };

            return new JsonResult(consolidatedCategories, options);
        }
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<List<ConsolidatedCategory>>> GetAllCategoriesWithCategoryId(int categoryId)
        {
            var categories = await _dbContext.Categories.Where(c=> c.CategoryId == categoryId).Include(c => c.LaptopCategories).ToListAsync();

            var consolidatedCategories = categories.Select(c => new ConsolidatedCategory
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Laptops = GetLaptopsByCategoryId(c.CategoryId)
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 32 // Nếu cần thiết, bạn có thể tăng giới hạn độ sâu của đối tượng lên
            };

            return new JsonResult(consolidatedCategories, options);
        }
        [HttpGet("{categoryId}")]
        public List<Laptop> GetLaptopsByCategoryId(int categoryId)
        {
            var laptops = _dbContext.LaptopCategories
                .Where(lc => lc.CategoryId == categoryId)
                .Select(lc => lc.Laptop)
                .ToList();

            return laptops;
        }
        [HttpGet]
        public List<Laptop> GetLaptopsByCategoryIds(List<Category> categories)
        {
            // Lấy danh sách LaptopId từ các LaptopCategories trong các Category
            var laptopIds = categories.SelectMany(c => c.LaptopCategories.Select(lc => lc.LaptopId)).ToList();

            // Lấy danh sách Laptop tương ứng với danh sách LaptopId
            var laptops = _dbContext.Laptops.Where(l => laptopIds.Contains(l.LaptopId)).ToList();

            return laptops;
        }
        [HttpGet]
        public async Task<ActionResult<List<ConsolidatedCategory>>> SearchCategory(string categoryName)
        {
            try
            {
                var categories = await _dbContext.Categories
                    .Where(c => c.CategoryName.Contains(categoryName))
                    .Include(c => c.LaptopCategories)
                    .ToListAsync();

                var consolidatedCategories = categories.Select(c => new ConsolidatedCategory
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Laptops = GetLaptopsByCategoryId(c.CategoryId)
                }).ToList();

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    MaxDepth = 32 // Nếu cần thiết, bạn có thể tăng giới hạn độ sâu của đối tượng lên
                };

                return new JsonResult(consolidatedCategories, options);
            }
            catch (Exception ex)
            {
                // Ghi nhật ký lỗi bằng Console.WriteLine
                Console.WriteLine($"An error occurred while searching for categories: {ex}");

                // Trả về phản hồi lỗi chứa thông báo lỗi
                return StatusCode(500, "An error occurred while searching for categories.");
            }
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
        [Authorize(Roles = "Thêm danh mục")]
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromForm] string CategoryName)
        {
            // Kiểm tra xem danh mục có tồn tại trước đó hay không
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == CategoryName);
            if (existingCategory != null)
            {
                return BadRequest("Trùng tên danh mục");
            }

            // Tạo danh mục mới
            Category category = new Category { CategoryName = CategoryName };
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return Ok("Thêm danh mục thành công");
        }

        [Authorize(Roles ="Cập nhật danh mục")]
        [HttpPut("{CategoryId}/{CategoryName}")]
        public async Task<IActionResult> UpdateCategoryName(int CategoryId, string CategoryName)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == CategoryId);
            var existingCategoryName = await _dbContext.Categories.Where(c=> c.CategoryName == CategoryName).FirstOrDefaultAsync();
            Console.WriteLine("1dd");
            if (existingCategoryName != null)
            {
                Console.WriteLine("ddd");
                return BadRequest("Tên danh mục đã tồn tại.");
            }
            if (existingCategory != null)
            {
                
                existingCategory.CategoryName = CategoryName;
                await _dbContext.SaveChangesAsync();
                return Ok("Đã thay đổi tên danh mục");
            }

            return NotFound();
        }


        [Authorize("Xóa danh mục")]
        [HttpDelete("{CategoryId}")]
        public async Task<IActionResult> DeleteCategory(int CategoryId)
        {
            var category = await _dbContext.Categories.FindAsync(CategoryId);

            if (category == null)
            {
                return NotFound("Không có danh mục này");
            }
            var laptopCategories = await _dbContext.LaptopCategories
            .Where(lc => lc.CategoryId == CategoryId)
            .ToListAsync();
            _dbContext.LaptopCategories.RemoveRange(laptopCategories);
            var affectedRows = await _dbContext.SaveChangesAsync();
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return Ok("Đã xóa danh mục");
        }
    }
}
