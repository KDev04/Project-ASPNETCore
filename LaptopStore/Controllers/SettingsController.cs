using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

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
    }
}
