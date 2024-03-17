using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

namespace LaptopStore.Controllers
{
    public class SettingsController : Controller
    {
        private readonly HttpClient _httpClient;
        public SettingsController(HttpClient httpClient) { _httpClient = httpClient; }
        public async Task<IActionResult> Dashboard()
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
        public IActionResult Authority()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }
        public IActionResult Inventory()
        {
            return View();
        }
        public IActionResult Promotion()
        {
            return View();
        }
        //Category
        public async Task<IActionResult> Category()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");
            HttpResponseMessage responselap = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptops");
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
                var reslaps = JsonConvert.DeserializeObject<List<Laptop>>(laps);
                if (reslaps == null) { reslaps = new List<Laptop>(); }
                PageCategoryModel model = new PageCategoryModel()
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
        public async Task<IActionResult> SearchCategory(string SearchKey)
        {
            HttpResponseMessage laps = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptops");
            if (laps== null)
            {
                return NotFound("Danh sach laptop rong ");
            }
            HttpResponseMessage req = await _httpClient.GetAsync(
               $"http://localhost:4000/api/Category/SearchCategory?categoryName={SearchKey}"
            );

            if (req.IsSuccessStatusCode)
            {
                var responseData = await req.Content.ReadAsStringAsync();
                var reqlaps = await laps.Content.ReadAsStringAsync();
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var res = JsonConvert.DeserializeObject<List<ConsolidatedCategory>>(responseData);
                if (res == null) { res = new List<ConsolidatedCategory>(); }
                var reslaps = JsonConvert.DeserializeObject<List<Laptop>>(reqlaps);
                if (reslaps == null) { reslaps = new List<Laptop>(); }
                PageCategoryModel model = new PageCategoryModel()
                {
                    Categories = res,
                    Laptops = reslaps
                };
                Console.WriteLine("ok con de");
                return View("Category", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                Console.WriteLine("ok con khi");

                return RedirectToAction("Category");
            }
        }
        public async Task<ActionResult> CreateCategory(string CategoryName)
        {
            // Xử lý dữ liệu responseData theo nhu cầu của bạn
            var req = new FormUrlEncodedContent(
                    new[]
                    {
                            new KeyValuePair<string, string>("CategoryName", CategoryName)
                    }
                );

            HttpResponseMessage res = await _httpClient.PostAsync(
                "http://localhost:4000/api/Category/CreateCategory", req
            );

            if (res.IsSuccessStatusCode)
            {
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                return RedirectToAction("Category");
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return RedirectToAction("Category");
            }
        }

        public async Task<IActionResult> DeleteCategory(int CategoryId)
        {
            var apiUrl = $"http://localhost:4000/api/Category/DeleteCategory/{CategoryId}";

            var response = await _httpClient.DeleteAsync(apiUrl);
            Console.WriteLine("Toi day roi ne");
            if (response.IsSuccessStatusCode)
            {
                // Xử lý kết quả thành công
                Console.WriteLine("da vo day roi");
                return RedirectToAction("Category");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Xử lý khi không tìm thấy danh mục
                return NotFound("Không có danh mục này");
            }
            else
            {
                // Xử lý lỗi
                return StatusCode((int)response.StatusCode);
            }
        }
        public IActionResult Ticket()
        {
            return View();
        }

    }
}
