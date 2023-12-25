using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopStoreWebApi.Data;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LaptopStoreWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly LaptopDbContext _context;

        public CartsController(LaptopDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetCart(string userId)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };

            return Ok(JsonSerializer.Serialize(cart, options));
        }
        [HttpPost]
        public IActionResult AddToCart(string userId, int LaptopId, int Quantity)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart() { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var laptop = _context.Laptops.FirstOrDefault(l => l.LaptopId == LaptopId);
            if (laptop == null)
            {
                return BadRequest("Laptop không tồn tại");
            }

            var existingCartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.LaptopId == laptop.LaptopId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += Quantity;
                existingCartItem.Total = laptop.Price * existingCartItem.Quantity;
            }
            else
            {
                CartItem cartItem = new CartItem()
                {
                    LaptopId = laptop.LaptopId,
                    Laptop = laptop,
                    CartId = cart.Id,
                    Quantity = Quantity,
                    Total = laptop.Price * Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges();
            return Ok("Sản phẩm đã được thêm vào giỏ hàng.");
        }

    }
}
