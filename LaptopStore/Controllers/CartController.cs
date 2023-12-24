using Humanizer;
using LaptopStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Xml.Linq;

namespace LaptopStore.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> AddToCart(string userId, int laptopId, int quantity)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu tới API và truyền các tham số
                    HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:8000/api/Carts/AddToCart?userId={userId}&LaptopId={laptopId}&Quantity={quantity}",null);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        // Xử lý dữ liệu responseData theo nhu cầu của bạn
                        response.EnsureSuccessStatusCode();


                        return RedirectToAction("Index", "Home"); // Chuyển hướng đến action Index trong controller Home
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                catch
                {
                    return BadRequest("khong hoat dong");
                }
            }
        }
    }
}
