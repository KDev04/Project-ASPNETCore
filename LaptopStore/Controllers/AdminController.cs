using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LaptopStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminController> _logger;
        public AdminController(HttpClient httpClient, ILogger<AdminController> logger, IWebHostEnvironment env)
        {
            _httpClient = httpClient;
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateUser(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("user");
            }
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(model.UserName?.ToString() ?? ""), "UserName");
                formData.Add(new StringContent(model.FirstName?.ToString() ?? ""), "FirstName");
                formData.Add(new StringContent(model.LastName?.ToString() ?? ""), "LastName");
                formData.Add(new StringContent(model.BirthDay.ToString() ?? ""), "BirthDay");
                formData.Add(new StringContent(model.Address?.ToString() ?? ""), "Address");
                formData.Add(new StringContent(model.Email?.ToString() ?? ""), "Email");
                formData.Add(new StringContent(model.PhoneNumber?.ToString() ?? ""), "PhoneNumber");
                formData.Add(new StringContent(model.Password?.ToString() ?? ""), "Password");
                if (model.Image != null && model.Image.Length > 0)
                {
                    var streamContent = new StreamContent(model.Image.OpenReadStream());
                    formData.Add(streamContent, "Image", model.Image.FileName);
                }
                var response = await _httpClient.PostAsync("http://localhost:4000/api/Account/CreateUser", formData);
                if (response.IsSuccessStatusCode)
                {
                    // Xử lý khi tạo laptop thành công
                    return RedirectToAction("UserPage");
                }
                else
                {
                    // Xử lý khi có lỗi từ API
                    return Redirect("Home/Error");
                }
            }
        }
        public async Task<IActionResult> LaptopPage(int page = 1, int take = 4)
        {
            // Danh sách Category 
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");

            // Danh sách laptop 
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
                int totalPages = 1;
                if (reslaps != null)
                {
                    int totalLaptops = reslaps.Count;
                    totalPages = (int)Math.Ceiling((double)totalLaptops / take);
                }
/*                if (totalPages <page)
                {
                    PageLaptopModel all = new PageLaptopModel()
                    {
                        page = page,
                        pageSize = take,
                        totalPage = totalPages,
                        Categories = res,
                        Laptops = reslaps.Skip(page - 1).Take(page * take).ToList()
                    };
                    return View(all);
                }*/
                PageLaptopModel model = new PageLaptopModel()
                {
                    page = page,
                    pageSize = take,
                    totalPage = totalPages,
                    Categories = res,
                    Laptops = reslaps.Skip(page * take -1).Take(take).ToList()
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
        // Phương thức tìm kiếm laptop trong trang LaptopPage 
        public async Task<IActionResult> SearchLaptop(string SearchKey)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");
            HttpResponseMessage responselap = await _httpClient.GetAsync($"http://localhost:4000/api/Laptop/SearchLaptopWithAllCategory?key={SearchKey}");
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
                return View("LaptopPage", model); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("LaptopPage", model);
            }
        }

         public async Task<IActionResult> OrderByName2()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");

            // Danh sách laptop 
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
                reslaps = reslaps.OrderByDescending(laptop => laptop.Laptop.Name).ToList();
                PageLaptopModel model = new PageLaptopModel()
                {

                    Categories = res,
                    Laptops = reslaps
                };
                return View("LaptopPage", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("LaptopPage", model);
            }
        }
        public async Task<IActionResult> OrderByName()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");

            // Danh sách laptop 
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
                reslaps = reslaps.OrderBy(laptop => laptop.Laptop.Name).ToList();
                PageLaptopModel model = new PageLaptopModel()
                {

                    Categories = res,
                    Laptops = reslaps
                };
                return View("LaptopPage", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("LaptopPage", model);
            }
        }
        public async Task<IActionResult> OrderByPrice()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");

            // Danh sách laptop 
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
                reslaps = reslaps.OrderBy(laptop => laptop.Laptop.Price).ToList();
                PageLaptopModel model = new PageLaptopModel()
                {

                    Categories = res,
                    Laptops = reslaps
                };
                return View("LaptopPage", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("LaptopPage", model);
            }
        }

        public async Task<IActionResult> OrderByPrice2()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");

            // Danh sách laptop 
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
                reslaps = reslaps.OrderByDescending(laptop => laptop.Laptop.Price).ToList();
                PageLaptopModel model = new PageLaptopModel()
                {

                    Categories = res,
                    Laptops = reslaps
                };
                return View("LaptopPage", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return View("LaptopPage", model);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> SaveProduct(Laptop model)
        {
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.Name?.ToString() ?? ""), "Name");
                    formData.Add(new StringContent(model.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
                    formData.Add(new StringContent(model.Description?.ToString() ?? ""), "Description");
/*                    formData.Add(new StringContent(model.Type?.ToString() ?? ""), "Type");*/
                    formData.Add(new StringContent(model.BigPrice.ToString() ?? ""), "BigPrice");
                    formData.Add(new StringContent(model.Color?.ToString() ?? ""), "Color");
                    formData.Add(new StringContent(model.Brand?.ToString() ?? ""), "Brand");
                    formData.Add(new StringContent(model.SeriesLaptop?.ToString() ?? ""), "SeriesLaptop");
                    formData.Add(new StringContent(model.Cpu?.ToString() ?? ""), "Cpu");
                    formData.Add(new StringContent(model.Chip?.ToString() ?? ""), "Chip");
                    formData.Add(new StringContent(model.RAM?.ToString() ?? ""), "RAM");
                    formData.Add(new StringContent(model.Memory?.ToString() ?? ""), "Memory");
                    formData.Add(new StringContent(model.BlueTooth?.ToString() ?? ""), "BlueTooth");
                    formData.Add(new StringContent(model.Keyboard?.ToString() ?? ""), "Keyboard");
                    formData.Add(new StringContent(model.OperatingSystem?.ToString() ?? ""), "OperatingSystem");
                    formData.Add(new StringContent(model.Pin?.ToString() ?? ""), "Pin");
                    formData.Add(new StringContent(model.weight?.ToString() ?? ""), "weight");
                    formData.Add(new StringContent(model.Accessory?.ToString() ?? ""), "Accessory");
                    formData.Add(new StringContent(model.Screen?.ToString() ?? ""), "Screen");
                    formData.Add(new StringContent(model.CategoryId.ToString() ?? ""), "CategoryId");
                    if (model.Image != null && model.Image.Length > 0)
                    {
                        var streamContent = new StreamContent(model.Image.OpenReadStream());
                        formData.Add(streamContent, "Image", model.Image.FileName);
                    }
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
                    formData.Add(new StringContent(model.Description?.ToString() ?? ""), "Description");
/*                    formData.Add(new StringContent(model.Type?.ToString() ?? ""), "Type");*/
                    formData.Add(new StringContent(model.BigPrice?.ToString() ?? ""), "BigPrice");
                    formData.Add(new StringContent(model.Color?.ToString() ?? ""), "Color");
                    formData.Add(new StringContent(model.Brand?.ToString() ?? ""), "Brand");
                    formData.Add(new StringContent(model.SeriesLaptop?.ToString() ?? ""), "SeriesLaptop");
                    formData.Add(new StringContent(model.Cpu?.ToString() ?? ""), "Cpu");
                    formData.Add(new StringContent(model.Chip?.ToString() ?? ""), "Chip");
                    formData.Add(new StringContent(model.RAM?.ToString() ?? ""), "RAM");
                    formData.Add(new StringContent(model.Memory?.ToString() ?? ""), "Memory");
                    formData.Add(new StringContent(model.BlueTooth?.ToString() ?? ""), "BlueTooth");
                    formData.Add(new StringContent(model.Keyboard?.ToString() ?? ""), "Keyboard");
                    formData.Add(new StringContent(model.OperatingSystem?.ToString() ?? ""), "OperatingSystem");
                    formData.Add(new StringContent(model.Pin?.ToString() ?? ""), "Pin");
                    formData.Add(new StringContent(model.weight?.ToString() ?? ""), "weight");
                    formData.Add(new StringContent(model.Accessory?.ToString() ?? ""), "Accessory");
                    formData.Add(new StringContent(model.Screen?.ToString() ?? ""), "Screen");
                    formData.Add(new StringContent(model.CategoryId.ToString() ?? ""), "CategoryId");
                    if (model.Image != null && model.Image.Length > 0)
                    {
                        var streamContent = new StreamContent(model.Image.OpenReadStream());
                        formData.Add(streamContent, "Image", model.Image.FileName);

                    }
                    Console.WriteLine("day hinh anh thanh congddd");
                    var response = await _httpClient.PutAsync($"http://localhost:4000/api/Laptop/UpdateLaptop/{LaptopId}", formData);
                    Console.WriteLine("goi qua api roi");
                    Console.WriteLine(response.StatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        // Xử lý khi tạo laptop thành công
                        return RedirectToAction("LaptopPage");
                    }
                    else
                    {
                        // Xử lý khi có lỗi từ API
                        return RedirectToAction("LaptopPage");
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xảy ra ngoại lệ: " + ex.Message);
                return Redirect("/Home/Error");
            }
        }
        public async Task<IActionResult> UserPage()
        {
/*            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);*/
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
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            try
            {
                HttpResponseMessage response1 = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetLaptops");

                HttpResponseMessage response2 = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetOrderOffline");

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)

                {
                    var response1Data = await response1.Content.ReadAsStringAsync();
                    var response2Data = await response2.Content.ReadAsStringAsync();

                    // Xử lý dữ liệu responseData theo nhu cầu của bạn
                    var laptopOption = JsonConvert.DeserializeObject<List<Laptop>>(response1Data);
                    var orderOfflines = JsonConvert.DeserializeObject<List<OrderOffline>>(response2Data);

                    // Tạo một đối tượng CustomModel chứa cả hai danh sách này
                    var customModel = new CustomModel
                    {
                        Laptops = laptopOption,
                        OrderOfflines = orderOfflines
                    };

                    return View(customModel); // Trả về view với custom model chứa cả danh sách laptop và orderoffline

                }
                else
                {

                    // Xử lý phản hồi không thành công
                    return BadRequest("Failed to process api");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error processing api: " + ex.Message);
            }


        }

        public async Task<IActionResult> OrderOffline(int IdOrder, int Id, [FromForm] int[] LaptopId, [FromForm] int Phone, [FromForm] string Name, [FromForm] DateTime OrderDate, [FromForm] string Note, [FromForm] string[] Products, [FromForm] int[] Quantity, [FromForm] int[] Price)
        {
            try
            {
                List<OrderOffline> AddOrderOfflines = new List<OrderOffline>();

                HttpResponseMessage responseIdOrder = await _httpClient.GetAsync("http://localhost:4000/api/Laptop/GetIdOrder");

                int maxIdOrder = await responseIdOrder.Content.ReadFromJsonAsync<int>();


                for (int i = 0; i < Products.Length; i++)
                {
                    OrderOffline AddOrderOffline = new OrderOffline();

                    // Gán ID đơn hàng
                    AddOrderOffline.IdOrder = maxIdOrder;
                    AddOrderOffline.LaptopId = LaptopId[i];
                    AddOrderOffline.Phone = Phone; // Giải mã URL để lấy số điện thoại
                    AddOrderOffline.Name = Name; // Tên người đặt hàng
                    AddOrderOffline.Note = Note; // Ghi chú
                    AddOrderOffline.Products = Products[i]; // Giải mã URL để lấy tên sản phẩm
                    AddOrderOffline.Quantity = Quantity[i];// Số lượng sản phẩm
                    AddOrderOffline.Price = Price[i];
                    AddOrderOffline.Total = Price[i] * Quantity[i];
                    AddOrderOffline.OrderDate = OrderDate;
                    AddOrderOffline.StatusOrder = 0; // Ngày đặt hàng

                    AddOrderOfflines.Add(AddOrderOffline);
                }

                var jsonContent = JsonConvert.SerializeObject(AddOrderOfflines);

                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:4000/api/Laptop/PostOrderOffline", stringContent);

                // Xử lý phản hồi từ API
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Đã thực hiện thành công!";
                    return RedirectToAction("OrderPage", "Admin");
                }
                else
                {
                    // Xử lý phản hồi không thành công
                    return BadRequest("Failed to process order");
                }




            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return BadRequest("Error processing order: " + ex.Message);
            }
        }

        public async Task<IActionResult> ChangeOrderOffline(int IdOrder, int Id, [FromForm] int[] LaptopId, [FromForm] int Phone, [FromForm] string Name, [FromForm] DateTime OrderDate, [FromForm] string Note, [FromForm] string[] Products, [FromForm] int[] Quantity, [FromForm] int[] Price)
        {
            try
            {
                List<OrderOffline> AddOrderOfflines = new List<OrderOffline>();

                for (int i = 0; i < Products.Length; i++)
                {
                    OrderOffline AddOrderOffline = new OrderOffline();

                    // Gán ID đơn hàng
                    AddOrderOffline.IdOrder = IdOrder;
                    AddOrderOffline.LaptopId = LaptopId[i];
                    AddOrderOffline.Phone = Phone; // Giải mã URL để lấy số điện thoại
                    AddOrderOffline.Name = Name; // Tên người đặt hàng
                    AddOrderOffline.Note = Note; // Ghi chú
                    AddOrderOffline.Products = Products[i]; // Giải mã URL để lấy tên sản phẩm
                    AddOrderOffline.Quantity = Quantity[i];// Số lượng sản phẩm
                    AddOrderOffline.Price = Price[i];
                    AddOrderOffline.Total = Price[i] * Quantity[i];
                    AddOrderOffline.OrderDate = OrderDate;
                    AddOrderOffline.StatusOrder = 0; // Ngày đặt hàng

                    AddOrderOfflines.Add(AddOrderOffline);
                }

                var jsonContent = JsonConvert.SerializeObject(AddOrderOfflines);

                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:4000/api/Laptop/ChangeOrderOffline", stringContent);

                // Xử lý phản hồi từ API
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Đã thực hiện thành công!";
                    return RedirectToAction("OrderPage", "Admin");
                }
                else
                {
                    // Xử lý phản hồi không thành công
                    return BadRequest("Failed to process order");
                }




            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return BadRequest("Error processing order: " + ex.Message);
            }
        }


        public async Task<IActionResult> DeleteOrderOfflines(int IdOrder)
        {
            try
            {
                // Gửi yêu cầu DELETE tới API với IdOrder được truyền vào
                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:4000/api/Laptop/DeleteOrderOfflines/{IdOrder}");

                if (response.IsSuccessStatusCode)
                {
                    // Xử lý thành công
                    return RedirectToAction("OrderPage", "Admin");
                }
                else
                {
                    // Xử lý lỗi từ API
                    return BadRequest("Failed to delete order");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      


    }
}