using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Humanizer;
using LaptopStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaptopStore.Controllers
{
    public class LaptopController : Controller
    {
        private readonly HttpClient _httpClient;
        public int PageSize = 4;

        public LaptopController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                /*var token = HttpContext.Session.GetString("Token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);*/
                HttpResponseMessage response = await httpClient.GetAsync(
                    "http://localhost:4000/api/Laptop/GetLaptops"
                );

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
        }
        public async Task<IActionResult> GetLaptopsByCategoryId(int CategoryId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");
            HttpResponseMessage responselap = await _httpClient.GetAsync($"http://localhost:4000/api/Category/GetLaptopsByCategoryId/{CategoryId}");
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
                return View("LaptopCategoryPage", model);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                PageCategoryModel model = new PageCategoryModel();
                return RedirectToAction("LaptopCategoryPage");
            }
        }
        public async Task<IActionResult> LaptopCategoryPage()
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
        public async Task<IActionResult> Detail(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(
                        "http://localhost:4000/api/Laptop/GetLaptop/" + id
                    );
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
        }

        public async Task<IActionResult> Detail2(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(
                        "http://localhost:4000/api/Laptop/GetStatus/" + id
                    );

                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();

                    // Handle the JSON response
                    var laptopStatusList = JsonConvert.DeserializeObject<List<LaptopStatus>>(
                        content
                    );

                    // You can now use the laptopStatusList as needed, for example, pass it to the view
                    return View(laptopStatusList);
                }
                catch (Exception)
                {
                    // Handle errors when calling the API
                    // For example, you can redirect to an error page
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public async Task<List<LaptopStatus>> GetStatus (int laptopId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:4000/api/Laptop/GetStatus/{laptopId}");

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Handle the JSON response
                var laptopStatusList = JsonConvert.DeserializeObject<List<LaptopStatus>>(
                    content
                );
                Console.WriteLine(laptopStatusList);
                // You can now use the laptopStatusList as needed, for example, pass it to the view
                return laptopStatusList.ToList();
            }
            catch (HttpRequestException ex)
            {
                // Xử lý ngoại lệ HttpRequestException
                // Ví dụ: Ghi log, thông báo lỗi, hoặc trả về một giá trị mặc định
                Console.WriteLine("Lỗi khi thực hiện yêu cầu HTTP: " + ex.Message);
                return new List<LaptopStatus>(); // Trả về một danh sách đánh giá rỗng
            }
            catch (Exception ex)
            {
                // Handle errors when calling the API
                // For example, you can redirect to an error page
                Console.WriteLine("Lỗi khi thực hiện yêu cầu HTTP: " + ex.Message);
                return new List<LaptopStatus>();
            }
        }

        public async Task<IActionResult> SaveProduct(Laptop model)
        {
/*            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );*/
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.Name?.ToString() ?? ""), "Name");
                    formData.Add(new StringContent(model.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity");
/*                    formData.Add(new StringContent(model.Type.ToString() ?? ""), "Type");*/
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

                            var response = await _httpClient.PostAsync(
                                "http://localhost:4000/api/Laptop/Add",
                                formData
                            );
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
                                return Redirect("Admin/Index");
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
        public async Task<IActionResult> SearchLaptopByName(string keyword)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");
                HttpResponseMessage responselap = await _httpClient.GetAsync($"http://localhost:4000/api/Laptop/SearchByLaptopName/{keyword}");
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
                    return View("LaptopCategoryPage", model);
                }
                else
                {
                    return RedirectToAction("LaptopCategoryPage");
                }
            }
            catch
            {
                return BadRequest("khong hoat dong");
            }
        }
        public async Task<IActionResult> SearchLaptopByPrice(decimal from, decimal to)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Category/GetAllCategoriesWithLaptopCategories");
                HttpResponseMessage responselap = await _httpClient.GetAsync($"http://localhost:4000/api/Laptop/SearchByLaptopPrice?from={from}&to={to}");
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
                    return View("LaptopCategoryPage", model);
                }
                else
                {
                    return RedirectToAction("LaptopCategoryPage");
                }
            }
            catch
            {
                return BadRequest("khong hoat dong");
            }
        }
        public async Task<IActionResult> Filter(
            string name = "PREDATOR",
            string sortBy = "TenLaptop_asc",
            int page = 1,
            int from = 0,
            int to = int.MaxValue
            )
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu GET tới API Filter và truyền các tham số
                    HttpResponseMessage response = await httpClient.GetAsync(
                        $"http://localhost:4000/api/Laptop/Filter?name={name}&sortBy={sortBy}&from={from}&to={to}&page={page}"
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        // Xử lý dữ liệu responseData theo nhu cầu của bạn
                        response.EnsureSuccessStatusCode();

                        // Tiếp theo, bạn có thể xử lý dữ liệu JSON nhận được ở đây
                        // Ví dụ: var laptops = JsonConvert.DeserializeObject<List<Laptop>>(content);

                        var laptop = JsonConvert.DeserializeObject<List<Laptop>>(content);

                        return View("Index", laptop); // Trả về view mà bạn muốn hiển thị dữ liệu
                    }
                    else
                    {
                        // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                        return StatusCode((int)response.StatusCode);
                    }
                }
                catch
                {
                    return BadRequest("khong hoat dong");
                }
            }
        }

        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return View("Searchnull");
                }

                // Gửi yêu cầu GET tới API Filter và truyền các tham số
                HttpResponseMessage response = await _httpClient.GetAsync(
                    $"http://localhost:4000/api/Laptop/Search?keyword={keyword}"
                );

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    var laptops = JsonConvert.DeserializeObject<List<Laptop>>(content);

                    if (laptops != null && laptops.Any())
                    {
                        return View("Index", laptops);
                    }
                    else
                    {
                        // Chuyển hướng đến trang Searchnull khi không có sản phẩm
                        return View("Searchnull");
                    }
                }
                else
                {
                    return View("Index", null);
                }
            }
            catch
            {
                return BadRequest("khong hoat dong");
            }
        }

        public async Task<string> GetUserId()
        {
            var token = HttpContext.Session.GetString("Token");
            /* var token = Request.Cookies["Token"];*/
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserId"
            );
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseData);
            return responseData;
        }

/*        public async Task<IActionResult> Linhkien()
        {
            using (var httpClient = new HttpClient())
            {
                *//*var token = HttpContext.Session.GetString("Token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);*//*
                HttpResponseMessage response = await httpClient.GetAsync(
                    "http://localhost:4000/api/Laptop/GetLinhkien"
                );

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
        }

        public async Task<IActionResult> AllLaptop()
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(
                    "http://localhost:4000/api/Laptop/GetAllLaptop"
                );

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var laptops = JsonConvert.DeserializeObject<List<Laptop>>(responseData);

                    return View(laptops); // Truyền danh sách laptops vào view
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
        }

        public async Task<IActionResult> Phukien()
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(
                    "http://localhost:4000/api/Laptop/GetPK"
                );

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var laptops = JsonConvert.DeserializeObject<List<Laptop>>(responseData);

                    return View(laptops); // Truyền danh sách laptops vào view
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
        }

        public async Task<IActionResult> Card()
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(
                    "http://localhost:4000/api/Laptop/GetCard"
                );

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var laptops = JsonConvert.DeserializeObject<List<Laptop>>(responseData);

                    return View(laptops); // Truyền danh sách laptops vào view
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
        }*/
    }
}
