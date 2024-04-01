using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace LaptopStore.Controllers
{
    public class SettingsController : Controller
    {
        private readonly HttpClient _httpClient;

        public object JsonRequestBehavior { get; private set; }

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


        public async Task<IActionResult> Authority()
        {
            HttpResponseMessage req = await _httpClient.GetAsync("http://localhost:4000/Seed/GetAllUsersWithClaims");
            if (req.IsSuccessStatusCode)
            {
                var responseData = await req.Content.ReadAsStringAsync();
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var res = JsonConvert.DeserializeObject<List<UserAuthority>>(responseData);
                if (res == null) { res = new List<UserAuthority>(); }


                return View(res); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                List<UserAuthority> res = new List<UserAuthority>();
                return View(res);
            }
        }

        public async Task<IActionResult> Authority2()
        {
            HttpResponseMessage req = await _httpClient.GetAsync("http://localhost:4000/api/UserGroup/GetUserWithGroupRole");

            if (req.IsSuccessStatusCode)
            {
                var responseData = await req.Content.ReadAsStringAsync();
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var res = JsonConvert.DeserializeObject<UserGroupRolePage>(responseData);
                if (res == null) { res = new UserGroupRolePage(); };


                return View(res); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                UserGroupRolePage res = new UserGroupRolePage();
                return View(res);
            }
        }
        public async Task<IActionResult> GetClaimsByUser(string userId)
        {
            // Lấy danh sách claim tương ứng với userId từ nguồn dữ liệu

            // Ví dụ: Lấy danh sách claim từ danh sách claims và lọc theo userId
            HttpResponseMessage req = await _httpClient.GetAsync($"http://localhost:4000/Seed/GetInfoWithUserId?userId={userId}");
            if (req.IsSuccessStatusCode)
            {
                var resdata = await req.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<UserAuthority>(resdata);
                Console.WriteLine(res);
                return Ok(res);
            }
            return NoContent();
        }


        public async Task<IActionResult> AddOrUpdateClaimModels(UserAuthority req)
        {
            Console.WriteLine("chay vo day roi");
            
            Console.WriteLine(req.UserId);
            var reqUrl = "http://localhost:4000/Seed/AddOrUpdateClaimModels";
            var jsonPayload = JsonConvert.SerializeObject(req);
            Console.WriteLine(jsonPayload);
            Console.WriteLine("Bug sieu to");
            // Tạo nội dung yêu cầu HTTP POST
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await _httpClient.PostAsync(reqUrl, content);
            if (res.IsSuccessStatusCode)
            {
                // Xử lý thành công
                return Ok(res);
            }
            else
            {
                // Xử lý khi có lỗi
                return BadRequest("Dữ liệu không hợp lệ");
            }
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
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<IActionResult> OrderCategoryByName()
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
                res = res.OrderBy(c=> c.CategoryName).ToList();
                var reslaps = JsonConvert.DeserializeObject<List<Laptop>>(laps);
                if (reslaps == null) { reslaps = new List<Laptop>(); }
                PageCategoryModel model = new PageCategoryModel()
                {
                    Categories = res,
                    Laptops = reslaps
                };
                return View("Category", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("Category", model);
            }
        }
        public async Task<IActionResult> OrderCategoryByNameDes()
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
                res = res.OrderByDescending(c => c.CategoryName).ToList();
                var reslaps = JsonConvert.DeserializeObject<List<Laptop>>(laps);
                if (reslaps == null) { reslaps = new List<Laptop>(); }
                PageCategoryModel model = new PageCategoryModel()
                {
                    Categories = res,
                    Laptops = reslaps
                };
                return View("Category", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("Category", model);
            }
        }
        public async Task<IActionResult> OrderCategoryById()
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
                res = res.OrderBy(c => c.CategoryId).ToList();
                var reslaps = JsonConvert.DeserializeObject<List<Laptop>>(laps);
                if (reslaps == null) { reslaps = new List<Laptop>(); }
                PageCategoryModel model = new PageCategoryModel()
                {
                    Categories = res,
                    Laptops = reslaps
                };
                return View("Category", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("Category", model);
            }
        }
        public async Task<IActionResult> OrderCategoryByIdDes()
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
                res = res.OrderByDescending(c => c.CategoryId).ToList();
                var reslaps = JsonConvert.DeserializeObject<List<Laptop>>(laps);
                if (reslaps == null) { reslaps = new List<Laptop>(); }
                PageCategoryModel model = new PageCategoryModel()
                {
                    Categories = res,
                    Laptops = reslaps
                };
                return View("Category", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("Category", model);
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
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            else if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["ErrorMessage"] = "Tên danh mục đã tồn tại.";
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
        public async Task<ActionResult> AddLaptopIntoCategory(int LaptopId, int CategoryId)
        {
            // Xử lý dữ liệu responseData theo nhu cầu của bạn
            var req = new FormUrlEncodedContent(
                    new[]
                    {
                            new KeyValuePair<string, string>("CategoryId",CategoryId.ToString()),
                            new KeyValuePair<string, string>("LaptopId",LaptopId.ToString())

                    }
                );

            HttpResponseMessage res = await _httpClient.PostAsync(
                "http://localhost:4000/api/LaptopCategory/CreateLaptopCategory", req
            );

            if (res.IsSuccessStatusCode)
            {
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                return NoContent();
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return RedirectToAction("Category");
            }
        }
        public async Task<IActionResult> DeleteLaptopInCategory(int LaptopId, int CategoryId)
        {
            var apiUrl = $"http://localhost:4000/api/LaptopCategory/DeleteLaptopCategory/{LaptopId}/{CategoryId}";

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
        public async Task<IActionResult> UpdateCategoryName(int CategoryId, string CategoryName)
        {
            var apiUrl = $"http://localhost:4000/api/Category/UpdateCategoryName/{CategoryId}/{CategoryName}";

            var response = await _httpClient.PutAsync(apiUrl, null);
            Console.WriteLine("Toi day roi ne");
            if (response.IsSuccessStatusCode)
            {
                // Xử lý kết quả thành công
                Console.WriteLine("da cap nhat roi");
                return RedirectToAction("Category");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Xử lý khi không tìm thấy danh mục
                return NotFound("Không có danh mục này");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["ErrorMessage"] = "Trùng tên danh mục.";
                return RedirectToAction("Category");
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
