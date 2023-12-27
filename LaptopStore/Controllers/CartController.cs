using Humanizer;
using LaptopStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace LaptopStore.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:4000/api/");

        }
        // userId HieuLord bên máy thịnh eb991811-c293-41ab-9ead-08fd4a46b03c
        public async Task<IActionResult> Index() 
        {
            var userId = await GetUserId();
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return Redirect("/Auth/Login");
            }
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Cart/GetCart/" + userId);

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
        public async Task<IActionResult> Order()
        {
            var userId = await GetUserId();
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return Redirect("/Auth/Login");
            }
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Cart/GetOrders/" + userId);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var laptops = JsonConvert.DeserializeObject<List<Order>>(responseData);

                return View(laptops); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)response.StatusCode);
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
        public async Task<string> GetUserId()
        {
            var token = HttpContext.Session.GetString("Token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Account/GetUserId");

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseData);
                return responseData;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Chuyển hướng đến trang "/Auth/Login"
                HttpContext.Response.Redirect("/Auth/Login");
                return null;
            }
            else
            {
                // Xử lý các mã lỗi khác (nếu cần)
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
        public async Task<IActionResult> AddToCart(CartModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        var userId = await GetUserId(); // Gọi phương thức GetUserId() để lấy giá trị UserId
                        if (userId == null) {
                            TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                            return Redirect("/Auth/Login");
                        }
                        formData.Add(new StringContent(userId.ToString() ?? ""), "UserId");
                        formData.Add(new StringContent(model.LaptopId.ToString() ?? ""), "LaptopId");
                        formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");

                        /*var token = HttpContext.Session.GetString("Token");*/

                        var res = await _httpClient.PostAsync("http://localhost:4000/api/Cart/AddToCart", formData);
                        if (res.IsSuccessStatusCode)
                        {
                            // Xử lý khi thêm sản phẩm vào giỏ hàng thành công
                            return Redirect("/Cart");
                        }
                        else if (res.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            // Mã lỗi 401: Unauthorized (Chưa xác thực)
                            // Hiển thị thông báo và chuyển hướng đến trang đăng nhập
                            TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                            return Redirect("/Auth/Login");
                        }
                        else
                        {
                            // Xử lý các mã lỗi khác (nếu cần)
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred");
                }
            }
            return Redirect("/");
        }
        public async Task<string> DeleteCart(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"Cart/DeleteCart/{id}");

            if (response.IsSuccessStatusCode)
            {
                return "Xóa thành công";
            }
            else
            {
                return "Xóa không thành công";
            }
        }
        public async Task<string> OrderCartById(int id)
        {
            /* var token = HttpContext.Session.GetString("Token");*/
            HttpResponseMessage response = await _httpClient.PostAsync($"Cart/OrderCartById/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return "Đặt hàng thành công";
            }
            else
            {
                return "Đặt hàng không thành công";
            }
        }
    }
}
