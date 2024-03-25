﻿using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using LaptopStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                        var content = new FormUrlEncodedContent(
                            new[]
                            {
                                new KeyValuePair<string, string>("UserName", model.UserName),
                                new KeyValuePair<string, string>("Email", model.Email),
                                new KeyValuePair<string, string>("Password", model.Password)
                            }
                        );

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
                            Console.WriteLine(
                                $"API request failed with status code: {response.StatusCode}"
                            );
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
                    // Địa chỉ API đăng ký tài khoản
                    var apiUrl = "http://localhost:4000/api/Account/Login";

                    // Chuyển đối tượng RegisterModel thành chuỗi JSON
                    var jsonModel = JsonConvert.SerializeObject(model);

                    // Tạo nội dung yêu cầu từ chuỗi JSON
                    var content = new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("UserName", model.UserName),
                            new KeyValuePair<string, string>("Password", model.Password)
                        }
                    );

                    var response = await _httpClient.PostAsync(apiUrl, content);

                    // Kiểm tra xem cuộc gọi API có thành công hay không
                    if (response.IsSuccessStatusCode)
                    {
                        // Xử lý khi thành công
                        Console.WriteLine("Success");
                        var token = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(token);
                        // Lưu token trong session hoặc cookie
                        HttpContext.Session.SetString("Token", token);
                        Response.Cookies.Append("Token", token);
                        ViewBag.SuccessMessage = $"User '{model.UserName}' has been logged.";
                        Response.Cookies.Append("CheckLogin", "Inlogged");
                        Console.WriteLine(model.UserName);
                        Response.Cookies.Append("Username", model.UserName);
                        var role = await GetUserRoles();

                        if (role != null)
                        {
                            Response.Cookies.Append("Role", role.ToString());
                            if(role == "Moderator" || role == "Administrator")
                            {
                                return Redirect("/Admin/Index");
                            }
                           
                        }
                        Console.WriteLine(role);
                        return RedirectToAction("Index", "Laptop"); // Đổi thành action hoặc view mong muốn
                    }
                    else
                    {
                        // Xử lý khi cuộc gọi API không thành công
                        Console.WriteLine(
                            $"API request failed with status code: {response.StatusCode}"
                        );
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
        public async Task<IActionResult> AddToLikeList(int LaptopId)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserId"
            );
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                ViewBag.IsLoggedIn = true;
                Console.WriteLine(ViewBag.IsLoggedIn);
                Console.WriteLine(responseData);
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var content = new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("LaptopId", LaptopId.ToString()),
                            new KeyValuePair<string, string>("UserId", responseData)
                        }
                    );

                HttpResponseMessage resLap = await _httpClient.PostAsync(
                    "http://localhost:4000/api/LikeProduct/AddToLikeList",content
                );

                if (resLap.IsSuccessStatusCode)
                {
                    // Xử lý dữ liệu responseData theo nhu cầu của bạn


                    return Redirect("/Laptop");
                }
                else
                {
                    // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                    return StatusCode((int)resLap.StatusCode);
                }
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ APIs
                var content = new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("LaptopId", LaptopId.ToString()),
                            new KeyValuePair<string, string>("UserId", "test")
                        }
                    );

                HttpResponseMessage resLap = await _httpClient.PostAsync(
                    "http://localhost:4000/api/LikeProduct/AddToLikeList", content
                );

                if (resLap.IsSuccessStatusCode)
                {
                    // Xử lý dữ liệu responseData theo nhu cầu của bạn


                    return Redirect("/Laptop");
                }
                else
                {
                    // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                    return StatusCode((int)resLap.StatusCode);
                }
            }
        }
        public async Task<IActionResult> LikeList()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserId"
            );
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                ViewBag.IsLoggedIn = true;
                Console.WriteLine(ViewBag.IsLoggedIn);
                Console.WriteLine(responseData);
                // Xử lý dữ liệu responseData theo nhu cầu của bạn

                HttpResponseMessage resLap= await _httpClient.GetAsync(
                   $"http://localhost:4000/api/LikeProduct/GetLaptopsByUserId/{responseData}"
                );

                if (resLap.IsSuccessStatusCode)
                {
                    var Laptops = await resLap.Content.ReadAsStringAsync();
                    Console.WriteLine(Laptops);
                    // Xử lý dữ liệu responseData theo nhu cầu của bạn
                    var laps = JsonConvert.DeserializeObject<List<Laptop>>(Laptops);

                    return View(laps); // Trả về view mà bạn muốn hiển thị dữ liệu
                }
                else
                {
                    // Xử lý lỗi khi không nhận được phản hồi thành công từ AP
                    List<Laptop> list = new List<Laptop>() {};
                    return View(list);
                }
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ APIs
                HttpResponseMessage resLap = await _httpClient.GetAsync(
                   "http://localhost:4000/api/LikeProduct/GetLikes"
                );

                var Laptops = await resLap.Content.ReadAsStringAsync();
                Console.WriteLine(Laptops);
                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var laps = JsonConvert.DeserializeObject<List<Laptop>>(Laptops);

                return View(laps);
            }
        }
        public async Task<IActionResult> DeleteLikeProduct(int LaptopId)
        {
            var token = Request.Cookies["Token"];
            if (token == null)
            {
                HttpResponseMessage res = await _httpClient.DeleteAsync(
                $"http://localhost:4000/api/LikeProduct/DeleteLaptopLike/ABC/{LaptopId}"
            );

                if (res.IsSuccessStatusCode)
                {
                    // Xử lý dữ liệu responseData theo nhu cầu của bạn


                    return Redirect("/Auth/LikeList");
                }
                else
                {
                    // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                    return StatusCode((int)res.StatusCode);
                }
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserId"
            );
            Console.WriteLine(response);
            var responseData = await response.Content.ReadAsStringAsync();
            HttpResponseMessage resLap = await _httpClient.DeleteAsync(
                $"http://localhost:4000/api/LikeProduct/DeleteLaptopLike/{responseData}/{LaptopId}"
            );
            
            if (resLap.IsSuccessStatusCode)
            {
                // Xử lý dữ liệu responseData theo nhu cầu của bạn


                return Redirect("/Auth/LikeList");
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)resLap.StatusCode);
            }
            // Xử lý dữ liệu responseData theo nhu cầu của bạn

            
        }
        public async Task<IActionResult> UserInfo()
        {
            /*var token = HttpContext.Session.GetString("Token");*/

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserInfo"
            );
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                ViewBag.IsLoggedIn = true;
                Console.WriteLine(ViewBag.IsLoggedIn);

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

        public IActionResult Logout()
        {
            // Xóa Token khỏi Session
            HttpContext.Session.Remove("Token");

            // Xóa Cookie "Token"
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("CheckLogin");
            Response.Cookies.Delete("Role");
            Response.Cookies.Delete("Username");
            ViewBag.IsLoggedIn = false;
            Console.WriteLine(ViewBag.IsLoggedIn);
            Console.WriteLine("No Login");

            // Chuyển hướng đến trang Logout thành công hoặc trang khác
            return Redirect("/Auth/Login");
            /*return View("Login");*/
        }
        public async Task<string> GetUserRoles()
        {
            var token = HttpContext.Session.GetString("Token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Account/GetUserRoles");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return null!;
                }

                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();

                // Chuyển đổi chuỗi JSON thành một danh sách (List) hoặc một đối tượng (object)
                var roles = JsonConvert.DeserializeObject<List<string>>(responseData);
                // Hoặc: var role = JsonConvert.DeserializeObject<string>(responseData);

                if (roles != null && roles.Count > 0)
                {
                    // Trả về giá trị cụ thể (ví dụ: roles[0]) thay vì chuỗi JSON gốc
                    return roles[0];
                    // Hoặc: return role;
                }
                else
                {
                    // Xử lý trường hợp không có vai trò nào được trả về
                    return null!;
                }
            }


            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Chuyển hướng đến trang "/Auth/Login"
                HttpContext.Response.Redirect("/Auth/Login");
                return null!;
            }
            else
            {
                // Xử lý các mã lỗi khác (nếu cần)
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
        public  async Task<IActionResult> UpdateUser ()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserInfo"
            );
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                ViewBag.IsLoggedIn = true;
                Console.WriteLine(ViewBag.IsLoggedIn);

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
        public async Task<IActionResult> SaveUpdateUser(User model)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.UserName ?? ""), "UserName");
                    formData.Add(new StringContent(model.FullName ?? ""), "FullName");
                    formData.Add(new StringContent(model.Address ?? ""), "Address");
                    formData.Add(new StringContent(model.Email ?? ""), "Email");
                    formData.Add(new StringContent(model.PhoneNumber ?? ""), "PhoneNumber");

                    if (model.Image != null && model.Image.Length > 0)
                    {
                        using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                        {
                            formData.Add(streamContent, "Image", model.Image.FileName);

                            var response = await _httpClient.PutAsync($"http://localhost:4000/api/Account/UpdateUser/{model.Id}", formData);
                            Console.WriteLine("Gọi qua API rồi");
                            Console.WriteLine(response.StatusCode);

                            if (response.IsSuccessStatusCode)
                            {
                                // Xử lý khi cập nhật người dùng thành công
                                Console.WriteLine("Cap nhat thanh cong");

                                return Redirect("/Auth/UserInfo");
                            }
                            else
                            {
                                Console.WriteLine("Cap nhat that bai");
                                // Xử lý khi có lỗi từ API
                                return Redirect("/Admin/Index");
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
        public async Task<IActionResult> ChangePassword()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            HttpResponseMessage response = await _httpClient.GetAsync(
                "http://localhost:4000/api/Account/GetUserInfo"
            );
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                ViewBag.IsLoggedIn = true;
                Console.WriteLine(ViewBag.IsLoggedIn);

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
        public async Task<IActionResult> SavePassword(string userId, string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:4000/api/Account/ChangePassword?userId={userId}&currentPassword={currentPassword}&newPassword={newPassword}&confirmPassword={confirmPassword}", null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Đổi mật khẩu thành công!");
                    return Redirect("/Auth/UserInfo");

                }
                else 
                { 
                    Console.WriteLine("Đỏi mật khẩu thất bại!");
                    return Redirect("/Home/Error");
                }
            }
            catch
            {
                return Redirect("/Home/Error");

            }
        }
        public IActionResult ForgetPasswordView()
        {
            return View();
        }
        public async Task<IActionResult> ForgetPassword(string username, string newPassword, string confirmPassword)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:4000/api/Account/ForgetPassword?username={username}&newPassword={newPassword}&confirmPassword={confirmPassword}", null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Đổi mật khẩu thành công!");
                    return Redirect("/Auth/Login");

                }
                else
                {
                    Console.WriteLine("Đỏi mật khẩu thất bại!");
                    return Redirect("/Home/Error");
                }
            }
            catch
            {
                return Redirect("/Home/Error");

            }
        }
    }
}
