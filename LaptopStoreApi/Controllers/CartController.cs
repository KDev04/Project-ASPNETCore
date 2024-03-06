using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using LaptopStoreApi.Constants;

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
        [Authorize]
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
            var carts = _dbContext.Carts.Include(l=>l.Laptop).Include(c=>c.User).Where(c => c.UserId == UserId).ToList();
            return Ok(carts);
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id) 
        { 
            var cart = _dbContext.Carts.Where(cart=>cart.Id == id).FirstOrDefault();
            if (cart != null)
            {
                _dbContext.Carts.Remove(cart);
                _dbContext.SaveChanges();
                return Ok("Xoa thanh cong");
            }
            else
            {
                return BadRequest("Xoa khong thanh cong");
            }
        }
        [HttpPost("{id}")]
        public IActionResult OrderCartById(int id)
        {
            var cart = _dbContext.Carts.Where(c => c.Id == id).FirstOrDefault();

            if (cart != null)
            {
                Order order = new Order()
                {
                    LaptopId = cart.LaptopId,
                    Laptop = cart.Laptop,
                    UserId = cart.UserId,
                    User = cart.User,
                    Price = cart.Price,
                    Quantity = cart.Quantity,
                    Total = cart.Price * cart.Quantity,
                    StatusOrder = 0
                };

                _dbContext.Orders.Add(order);

                var laptop = _dbContext.Laptops.Find(cart.LaptopId);
                if (laptop != null)
                {
                    laptop.Quantity -= cart.Quantity; // Giảm Quantity của Laptop theo giá trị Quantity của cart
                    _dbContext.SaveChanges(); // Cập nhật giá trị Quantity của Laptop trong cơ sở dữ liệu
                }

                _dbContext.Carts.Remove(cart);
                _dbContext.SaveChanges();

                return Ok("Đặt hàng thành công");
            }
            else
            {
                return BadRequest("Đặt hàng không thành công");
            }
        }
        [HttpGet("{UserId}")]
        public IActionResult GetOrders(string UserId) 
        { 
            var orders = _dbContext.Orders.Include(l => l.Laptop).Include(c => c.User).Where(c => c.UserId == UserId).ToList();
            return Ok(orders);
        }
/*        [Authorize(Roles =RoleNames.Moderator)]*/
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _dbContext.Orders.Include(l => l.Laptop).Include(c => c.User).ToList();
            return Ok(orders);
        }
    }
}
