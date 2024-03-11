using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

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
        public IActionResult Setting()
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
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
                    formData.Add(new StringContent(model.Description.ToString() ?? ""), "Description");

                    if (model.Image != null && model.Image.Length > 0)
                    {
                        using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                        {
                            formData.Add(streamContent, "Image", model.Image.FileName);

                            var response = await _httpClient.PostAsync("http://localhost:4000/api/Laptop/Add", formData);
                            Console.WriteLine("goi qua api roi");
                            Console.WriteLine(response.StatusCode);

                            if (response.IsSuccessStatusCode)
                            {
                                // Xử lý khi tạo laptop thành công
                                return Redirect("/Admin/LaptopPage");
                            }
                            else
                            {
                                // Xử lý khi có lỗi từ API
                                return Redirect("/Admin/LaptopPage");
                            }
                        }
                    }
                    else
                    {
                        return Redirect("/");
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
        public async Task<IActionResult> UpdateLaptop(int LaptopId)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptop/" + LaptopId);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Tiếp theo, bạn có thể xử lý dữ liệu JSON nhận được ở đây
                // Ví dụ: var laptops = JsonConvert.DeserializeObject<List<Laptop>>(content);

                var laptop = JsonConvert.DeserializeObject<Laptop>(content);
                return View(laptop);
            }
            catch (Exception)
            {
                // Xử lý lỗi khi gặp vấn đề khi gọi API
                // Ví dụ:
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> SaveUpdate(Laptop model)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.Name?.ToString() ?? ""), "Name");
                    formData.Add(new StringContent(model.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
                    formData.Add(new StringContent(model.Description.ToString() ?? ""), "Description");
                    if (model.Image != null && model.Image.Length > 0)
                    {
                        using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                        {
                            formData.Add(streamContent, "Image", model.Image.FileName);
                            Console.WriteLine(model.LaptopId);

                            var response = await _httpClient.PutAsync($"http://localhost:4000/api/Laptop/UpdateLaptop/{model.LaptopId}", formData);
                            Console.WriteLine("goi qua api roi");

                            Console.WriteLine(response.StatusCode);
                            
                            if (response.IsSuccessStatusCode)
                            {
                                // Xử lý khi tạo laptop thành công
                                Console.WriteLine("Cap nhat thanh cong");
                                return Redirect("/Admin/LaptopPage");
                            }
                            else
                            {
                                // Xử lý khi có lỗi từ API
                                Console.WriteLine("Cap nhat that bai");
                                return Redirect("/Admin/Index");
                            }
                        }
                    }
                    else
                    {
                        return Redirect("/Admin/LaptopPage");
                    }
                }
            }
            catch
            {
                return Redirect("/Home/Error");
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
        public async Task<IActionResult> AddRole(string UserId)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:4000/api/Account/AddRoleModerator/{UserId}", null);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Trả về lỗi 401 - Unauthorized
                Console.WriteLine("Chưa đăng nhập");
                ViewBag.ErrorMessage = "Lỗi xác thực";
                return Redirect("/Admin/UserPage");

            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                // Trả về lỗi 403 - Forbidden
                Console.WriteLine("Không đủ quyền");
                ViewBag.ErrorMessage = "Không đủ quyền hạn";
                return Redirect("/Admin/UserPage");
            }
            else if (response.IsSuccessStatusCode)
            {
                // Trả về thành công với kết quả Result từ response
                var resultString = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeAnonymousType(resultString, new { Result = "" });

                Console.WriteLine(resultObject.Result);
                ViewBag.SuccessMessage = resultObject.Result;
                TempData["SuccessMessage"] = "Đã thêm quyền.";
                return Redirect("/Admin/UserPage");

            }
            else
            {
                // Xử lý các mã lỗi khác (nếu cần)
                ViewBag.ErrorMessage = "Lỗi không xác định";
                return Redirect("/Admin/UserPage");

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
                List<Order> orders = new List<Order>();
                return View(orders);
            }
        }
    }
}