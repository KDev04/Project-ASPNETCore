using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace LaptopStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminController(HttpClient httpClient) { _httpClient = httpClient; }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LaptopPage()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptops");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var laptops = JsonConvert.DeserializeObject<List<Laptop>>(responseData);

                return View(laptops); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)response.StatusCode);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> SaveProduct(Laptop model)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.Name?.ToString() ?? ""), "Name");
                    formData.Add(new StringContent(model.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity"); ;

                    if (model.Image != null && model.Image.Length > 0)
                    {
                        using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                        {
                            formData.Add(streamContent, "Image", model.Image.FileName);

                            var response = await _httpClient.PostAsync("http://localhost:4000/api/Laptop/Add", formData);

                            if (response.IsSuccessStatusCode)
                            {
                                // Xử lý khi tạo laptop thành công
                                return Redirect("/Admin/LaptopPage");
                            }
                            else
                            {
                                // Xử lý khi có lỗi từ API
                                return Redirect("Admin/Create");
                            }
                        }
                    }
                    else
                    {
                        return Redirect("Admin/Create");
                    }
                }
            }
            catch
            {
                return Redirect("/Home/Error");
            }
        }
        public async Task<IActionResult> DeleteLaptop(int LaptopId)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:4000/api/Laptop/Delete/{LaptopId}");
            if (response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Đã xóa";
                return Redirect("/Admin/LaptopPage");
            } else
            {
                ViewBag.ErrorMessage = "Xóa thất bại";
                return Redirect("/Admin/LaptopPage");
            }
        }
        public async Task<IActionResult> UserPage()
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Account/GetAllUser");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var users = JsonConvert.DeserializeObject<List<User>>(responseData);

                return View(users); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)response.StatusCode);
            }
        }
        public async Task<IActionResult>OrderPage()
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Cart/GetAllOrders");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var orders = JsonConvert.DeserializeObject<List<Order>>(responseData);

                return View(orders); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}