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
        private readonly ILogger<AdminController> _logger;
        public AdminController(HttpClient httpClient, ILogger<AdminController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LaptopPage()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");
            HttpResponseMessage responselap = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptopWithAllCategory");
            if (responselap == null)
            {
                return NotFound("Danh sach laptop rong ");
            }
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var laps = await responselap.Content.ReadAsStringAsync();
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var res = JsonConvert.DeserializeObject<List<ConsolidatedCategory>>(responseData);
                if (res == null) { res = new List<ConsolidatedCategory>(); }
                var reslaps = JsonConvert.DeserializeObject<List<ConsolidatedLaptop>>(laps);
                if (reslaps == null) { reslaps = new List<ConsolidatedLaptop>(); }
                PageLaptopModel model = new PageLaptopModel()
                {
                    Categories = res,
                    Laptops = reslaps
                };
                return View(model); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View(model);
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
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
                    formData.Add(new StringContent(model.Description.ToString() ?? ""), "Description");
                    formData.Add(new StringContent(model.Type.ToString() ?? ""), "Type");
                    formData.Add(new StringContent(model.BigPrice.ToString() ?? ""), "BigPrice");
                    formData.Add(new StringContent(model.Color.ToString() ?? ""), "Color");
                    formData.Add(new StringContent(model.Brand.ToString() ?? ""), "Brand");
                    formData.Add(new StringContent(model.SeriesLaptop.ToString() ?? ""), "SeriesLaptop");
                    formData.Add(new StringContent(model.Cpu.ToString() ?? ""), "Cpu");
                    formData.Add(new StringContent(model.Chip.ToString() ?? ""), "Chip");
                    formData.Add(new StringContent(model.RAM.ToString() ?? ""), "RAM");
                    formData.Add(new StringContent(model.Memory.ToString() ?? ""), "Memory");
                    formData.Add(new StringContent(model.BlueTooth.ToString() ?? ""), "BlueTooth");
                    formData.Add(new StringContent(model.Keyboard.ToString() ?? ""), "Keyboard");
                    formData.Add(new StringContent(model.OperatingSystem.ToString() ?? ""), "OperatingSystem");
                    formData.Add(new StringContent(model.Pin.ToString() ?? ""), "Pin");
                    formData.Add(new StringContent(model.weight.ToString() ?? ""), "weight");
                    formData.Add(new StringContent(model.Accessory.ToString() ?? ""), "Accessory");
                    formData.Add(new StringContent(model.Screen.ToString() ?? ""), "Screen");
                    formData.Add(new StringContent(model.CategoryId.ToString() ?? ""), "CategoryId");
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
            }
            else
            {
                ViewBag.ErrorMessage = "Xóa thất bại";
                return Redirect("/Admin/LaptopPage");
            }
        }
        public async Task<IActionResult> UpdateLaptop(int LaptopId, Laptop model)
        {
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.Name?.ToString() ?? ""), "Name");
                    formData.Add(new StringContent(model.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
                    formData.Add(new StringContent(model.Description.ToString() ?? ""), "Description");
                    formData.Add(new StringContent(model.Type.ToString() ?? ""), "Type");
                    formData.Add(new StringContent(model.BigPrice.ToString() ?? ""), "BigPrice");
                    formData.Add(new StringContent(model.Color.ToString() ?? ""), "Color");
                    formData.Add(new StringContent(model.Brand.ToString() ?? ""), "Brand");
                    formData.Add(new StringContent(model.SeriesLaptop.ToString() ?? ""), "SeriesLaptop");
                    formData.Add(new StringContent(model.Cpu.ToString() ?? ""), "Cpu");
                    formData.Add(new StringContent(model.Chip.ToString() ?? ""), "Chip");
                    formData.Add(new StringContent(model.RAM.ToString() ?? ""), "RAM");
                    formData.Add(new StringContent(model.Memory.ToString() ?? ""), "Memory");
                    formData.Add(new StringContent(model.BlueTooth.ToString() ?? ""), "BlueTooth");
                    formData.Add(new StringContent(model.Keyboard.ToString() ?? ""), "Keyboard");
                    formData.Add(new StringContent(model.OperatingSystem.ToString() ?? ""), "OperatingSystem");
                    formData.Add(new StringContent(model.Pin.ToString() ?? ""), "Pin");
                    formData.Add(new StringContent(model.weight.ToString() ?? ""), "weight");
                    formData.Add(new StringContent(model.Accessory.ToString() ?? ""), "Accessory");
                    formData.Add(new StringContent(model.Screen.ToString() ?? ""), "Screen");
                    formData.Add(new StringContent(model.CategoryId.ToString() ?? ""), "CategoryId");
                    if (model.Image != null && model.Image.Length > 0)
                    {
                        using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                        {
                            formData.Add(streamContent, "Image", model.Image.FileName);
                        }
                    }
                    var response = await _httpClient.PutAsync("http://localhost:4000/api/Laptop/Add", formData);
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
            catch
            {
                return Redirect("/Home/Error");
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
                    formData.Add(new StringContent(model.Type.ToString() ?? ""), "Type");
                    formData.Add(new StringContent(model.BigPrice.ToString() ?? ""), "BigPrice");
                    formData.Add(new StringContent(model.Color.ToString() ?? ""), "Color");
                    formData.Add(new StringContent(model.Brand.ToString() ?? ""), "Brand");
                    formData.Add(new StringContent(model.SeriesLaptop.ToString() ?? ""), "SeriesLaptop");
                    formData.Add(new StringContent(model.Cpu.ToString() ?? ""), "Cpu");
                    formData.Add(new StringContent(model.Chip.ToString() ?? ""), "Chip");
                    formData.Add(new StringContent(model.RAM.ToString() ?? ""), "RAM");
                    formData.Add(new StringContent(model.Memory.ToString() ?? ""), "Memory");
                    formData.Add(new StringContent(model.BlueTooth.ToString() ?? ""), "BlueTooth");
                    formData.Add(new StringContent(model.Keyboard.ToString() ?? ""), "Keyboard");
                    formData.Add(new StringContent(model.OperatingSystem.ToString() ?? ""), "OperatingSystem");
                    formData.Add(new StringContent(model.Pin.ToString() ?? ""), "Pin");
                    formData.Add(new StringContent(model.weight.ToString() ?? ""), "weight");
                    formData.Add(new StringContent(model.Accessory.ToString() ?? ""), "Accessory");
                    formData.Add(new StringContent(model.Screen.ToString() ?? ""), "Screen");

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
        public async Task<IActionResult> OrderPage()
        {

            HttpResponseMessage response1 = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptops");

            if (response1.IsSuccessStatusCode)
            {
                var response1Data = await response1.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var laptopOption = JsonConvert.DeserializeObject<List<Laptop>>(response1Data);

                return View(laptopOption); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                List<Laptop> laptopOption = new List<Laptop>();
                return View(laptopOption);
            }
        }


        public async Task<IActionResult> OrderOffline(OrderOffline formOrderRequest)
        {

            Console.WriteLine("Phone: " + formOrderRequest.Phone);
            Console.WriteLine("Name: " + formOrderRequest.Name);
            Console.WriteLine("OrderDate: " + formOrderRequest.OrderDate);
            Console.WriteLine("Products: " + formOrderRequest.Products);

            return View("OrderConfirmation");

        }
    }
}