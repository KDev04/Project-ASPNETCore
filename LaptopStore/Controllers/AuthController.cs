using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
namespace LaptopStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Sử dụng HTTPClient để gọi API
                    using (var httpClient = new HttpClient())
                    {
                        // Địa chỉ API đăng ký tài khoản
                        var apiUrl = "http://localhost:4000/api/Account/Register";

                        // Chuyển đối tượng RegisterModel thành chuỗi JSON
                        var jsonModel = JsonConvert.SerializeObject(model);

                        // Tạo nội dung yêu cầu từ chuỗi JSON
                        var content = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("UserName", model.UserName),
                                new KeyValuePair<string, string>("Email", model.Email),
                                new KeyValuePair<string, string>("Password", model.Password)
                            });

                        var response = await httpClient.PostAsync(apiUrl, content);


                        // Kiểm tra xem cuộc gọi API có thành công hay không
                        if (response.IsSuccessStatusCode)
                        {
                            // Xử lý khi đăng ký thành công
                            Console.WriteLine("Success");
                            ViewBag.SuccessMessage = $"User '{model.UserName}' has been created.";
                            return View("Register"); // Đổi thành action hoặc view mong muốn
                        }
                        else
                        {
                            // Xử lý khi cuộc gọi API không thành công
                            Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"API response content: {responseContent}");

                            // Log lỗi từ ModelState
                            foreach (var entry in ModelState)
                            {
                                foreach (var error in entry.Value.Errors)
                                {
                                    Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                                }
                            }

                            Console.WriteLine("Fail");
                            // Log dữ liệu đầu vào
                            Console.WriteLine($"JSON data being sent: {jsonModel}");

                            Console.WriteLine($"Received data: {model}");
                            ViewBag.SuccessMessage = $"User '{model.UserName}' has been created.";
                            return View("Register"); // Đổi thành action hoặc view mong muốn
                        }
                    }
                }
                else
                {
                    // Log lỗi từ ModelState
                    foreach (var entry in ModelState)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                        }
                    }

                    Console.WriteLine("Validation failed");

                    return View("Register"); // Đổi thành action hoặc view mong muốn
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View("Register");
            }
        }



        public async Task<IActionResult> SignIn([FromForm] LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Sử dụng HTTPClient để gọi API
                    using (var httpClient = new HttpClient())
                    {
                        // Địa chỉ API đăng ký tài khoản
                        var apiUrl = "http://localhost:4000/api/Account/Login";

                        // Chuyển đối tượng RegisterModel thành chuỗi JSON
                        var jsonModel = JsonConvert.SerializeObject(model);

                        // Tạo nội dung yêu cầu từ chuỗi JSON
                        var content = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("UserName", model.UserName),
                                new KeyValuePair<string, string>("Password", model.Password)
                            });

                        var response = await httpClient.PostAsync(apiUrl, content);


                        // Kiểm tra xem cuộc gọi API có thành công hay không
                        if (response.IsSuccessStatusCode)
                        {
                            // Xử lý khi thành công
                            Console.WriteLine("Success");
                            var token = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(token);
                            // Lưu token trong session hoặc cookie
                            HttpContext.Session.SetString("Token", token);
                            ViewBag.SuccessMessage = $"User '{model.UserName}' has been logged.";
                            return RedirectToAction("UserInfo", "Auth"); // Đổi thành action hoặc view mong muốn
                        }
                        else
                        {
                            // Xử lý khi cuộc gọi API không thành công
                            Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"API response content: {responseContent}");

                            // Log lỗi từ ModelState
                            foreach (var entry in ModelState)
                            {
                                foreach (var error in entry.Value.Errors)
                                {
                                    Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                                }
                            }

                            Console.WriteLine("Fail");
                            // Log dữ liệu đầu vào
                            Console.WriteLine($"JSON data being sent: {jsonModel}");

                            Console.WriteLine($"Received data: {model}");
/*                            ViewBag.SuccessMessage = $"User '{model.UserName}' has been created.";*/
                            return View("Register"); // Đổi thành action hoặc view mong muốn
                        }
                    }
                }
                else
                {
                    // Log lỗi từ ModelState
                    foreach (var entry in ModelState)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                        }
                    }

                    Console.WriteLine("Validation failed");

                    return View("Register"); // Đổi thành action hoặc view mong muốn
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View("Register");
            }
        }


        public async Task<IActionResult> UserInfo()
        {
            using (var httpClient  = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:4000/api/Account/GetUserInfo");
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Xử lý dữ liệu responseData theo nhu cầu của bạn
                    var user = JsonConvert.DeserializeObject<User>(responseData);

                    return View(user); // Trả về view mà bạn muốn hiển thị dữ liệu
                }
                else
                {
                    // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                    return StatusCode((int)response.StatusCode);
                }
            }
        }



        
    }
}
