using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost]
        public void AddToCard(int LaptopId, int quantity)
        {
            CartItem item = new CartItem() { LaptopId = LaptopId,Quantity = quantity};
        } 
    }
}
