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
        public AuthController()
        {
            _httpClient = new HttpClient();
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
        public async Task<IActionResult> SignUp(RegisterModel model)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (ModelState.IsValid)
            {
                // Chuyển đổi RegisterModel thành chuỗi JSON
                var json = JsonConvert.SerializeObject(model);

                // Gửi yêu cầu HTTP POST đến http://localhost:8000/register
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("http://localhost:8000/register"),
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };

                    // Thiết lập tiêu đề yêu cầu
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                    var response = await httpClient.SendAsync(request);

                    // Kiểm tra mã trạng thái của phản hồi
                    if (response.IsSuccessStatusCode)
                    {
                        // Chuyển hướng về trang Home/Index khi SignUp thành công
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Chuyển hướng về trang Home/Error khi SignUp thất bại
                        return RedirectToAction("Error", "Home");
                    }
                }
            }

            // Xử lý khi dữ liệu không hợp lệ
            return View(model);
        }
        public async Task<IActionResult> SignIn(RegisterModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:8000/login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                // Lưu token trong session hoặc cookie
                HttpContext.Session.SetString("Token", token);

                // Chuyển hướng về trang chính sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập thất bại, xử lý tương ứng
                return View("LoginFailed");
            }
        }
    }
}
