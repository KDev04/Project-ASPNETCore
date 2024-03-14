using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LikeProductController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public LikeProductController(ApiDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Laptop>> GetLikes()
        {
            var laptops = _dbContext.LikeProducts.Select(lp => lp.Laptop).ToList();
              
            if (laptops.Count == 0)
            {
                return NotFound();
            }

            return Ok(laptops);
        }
        [HttpGet("{UserId}")]
        public ActionResult<IEnumerable<Laptop>> GetLaptopsByUserId(string UserId)
        {
            var laptops = _dbContext.LikeProducts
                .Where(lp=> lp.UserId == UserId)
                .Select(lp=>lp.Laptop)
                .ToList();

            if (laptops.Count == 0)
            {
                return NotFound();
            }

            return Ok(laptops);
        }

        [HttpPost]
        public async Task<ActionResult<LikeProduct>> AddToLikeList([FromForm] int LaptopId, [FromForm] string UserId)
        {
            if (!_dbContext.Users.Any(u => u.Id == UserId))
            {
                // Không tìm thấy danh mục
                var testbase = await _userManager.FindByNameAsync("Base");
                UserId = testbase.Id;
            }

            if (!_dbContext.Laptops.Any(l => l.LaptopId == LaptopId))
            {
                // Không tìm thấy Laptop
                return BadRequest("Không tìm thấy Laptop này!");
            }

            if (_dbContext.LikeProducts.Any(lp => lp.LaptopId == LaptopId && lp.UserId == UserId))
            {
                // LaptopCategory đã tồn tại
                return BadRequest("Sản phẩm này đã có sẵn trong danh sách yêu thích!");
            }
            var User = await _dbContext.Users.FindAsync(UserId);
            var Laptop = await _dbContext.Laptops.FindAsync(LaptopId);
            Console.WriteLine("OK con dee");
            LikeProduct like_product = new LikeProduct
            {
                LaptopId = LaptopId,
                Laptop = Laptop,
                UserId = UserId,
                User = User
            };
            _dbContext.LikeProducts.Add(like_product);
            await _dbContext.SaveChangesAsync();

            return Ok("Đã thêm vào danh sách yêu thích!");
        }
        
        [HttpDelete("{UserId}/{LaptopId}")]
        public async Task<IActionResult> DeleteLaptopLike(string UserId, int LaptopId)
        {
            var User = await _dbContext.Users.FindAsync(UserId);
            if (User == null)
            {
                var testbase = await _userManager.FindByNameAsync("Base");
                UserId = testbase.Id;
            }
            else
            {
                UserId = User.Id;
            }
            var Liked = await _dbContext.LikeProducts.FindAsync(LaptopId, UserId);

            if (Liked == null)
            {
                return NotFound();
            }

            _dbContext.LikeProducts.Remove(Liked);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
