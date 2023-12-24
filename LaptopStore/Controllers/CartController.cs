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
        public async Task<IActionResult> Index(string UserId = "eb991811-c293-41ab-9ead-08fd4a46b03c")
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:4000/api/Cart/GetCart/" + UserId);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Xử lý dữ liệu responseData theo nhu cầu của bạn
                    var laptops = JsonConvert.DeserializeObject<List<Cart>>(responseData);

                    return View(laptops); // Trả về view mà bạn muốn hiển thị dữ liệu
                }
                else
                {
                    // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                    return StatusCode((int)response.StatusCode);
                }
            }
        }
        //Host 8000
        /* public async Task<IActionResult> AddToCart(string userId, int laptopId, int quantity)
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
         }*/
        public async Task<IActionResult> AddToCart(CartModel model)
        {
            if (ModelState.IsValid) 
            { 
                using(var httpClient = new HttpClient())
                {
                    try
                    {
                        using (var formData = new MultipartFormDataContent())
                        {
                            formData.Add(new StringContent(model.UserId?.ToString() ?? ""), "UserId");
                            formData.Add(new StringContent(model.LaptopId.ToString() ?? ""), "LaptopId");
                            formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
                            var res = await httpClient.PostAsync("http://localhost:4000/api/Cart/AddToCart", formData);
                            if (res.IsSuccessStatusCode)
                            {
                                // Xử lý khi tạo laptop thành công
                                return Redirect("/");
                            }
                            else
                            {
                                // Xử lý khi có lỗi từ API
                                ModelState.AddModelError("", "Thêm sản phẩm vào giỏ hàng không thành công.");
                            }
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "An error occurred");
                    }
                }

            }
            return Redirect("/");

        }
    }
}
