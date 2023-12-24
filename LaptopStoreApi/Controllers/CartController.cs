using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        public CartController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult AddToCart([FromForm] CartModel cart)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == cart.UserId);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng");
            }
            var laptop = _dbContext.Laptops.FirstOrDefault(l => l.LaptopId == cart.LaptopId);
            if (laptop == null)
            {
                return BadRequest("Không tìm thấy laptop");
            }
            var cartOld = _dbContext.Carts.FirstOrDefault(c => c.UserId == cart.UserId && c.LaptopId == cart.LaptopId);
            if (cartOld != null)
            {
                cartOld.Quantity += cart.Quantity;
                _dbContext.SaveChanges();
                return Ok(cartOld);
            } else
            {
                Cart NewCart = new Cart()
                {
                    User = user,
                    UserId = cart.UserId,
                    Laptop = laptop,
                    LaptopId = cart.LaptopId,
                    Name = laptop.Name,
                    Price = laptop.Price,
                    Quantity = cart.Quantity,
                    ImgPath = laptop.ImgPath,

                };
                _dbContext.Carts.Add(NewCart);
                _dbContext.SaveChanges();
                return Ok(NewCart);
            }


        }
        [HttpGet("{UserId}")]
        public IActionResult GetCart(string UserId)
        {
            var carts = _dbContext.Carts.Where(c=>c.UserId == UserId).ToList();
            return Ok(carts);
        }
       
    }
}
