using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using LaptopStore.Models;

namespace LaptopStore.Controllers
{
    public class EvaluateController : Controller
    {
        private readonly HttpClient _httpClient;
        public EvaluateController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userId = await GetUserId();
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Evaluate/GetEvaluatesByUserId/" + userId);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var laptops = JsonConvert.DeserializeObject<List<Evaluate>>(responseData);

                return View(laptops); // Trả về view mà bạn muốn hiển thị dữ liệu
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)response.StatusCode);
            }
        }
        public async Task<string> GetUserId()
        {

            var token = HttpContext.Session.GetString("Token");
            /* var token = Request.Cookies["Token"];*/
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Account/GetUserId");
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseData);
            return responseData;

        }
        public async Task<IActionResult> GetEvaluateByLaptopId(int laptopId)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userId = await GetUserId();
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Evaluate/GetEvaluatesByLaptopId/" + laptopId);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Xử lý dữ liệu responseData theo nhu cầu của bạn
                var evaluations = JsonConvert.DeserializeObject<List<Evaluate>>(responseData);

                return View(evaluations);
            }
            else
            {
                // Xử lý lỗi khi không nhận được phản hồi thành công từ API
                return StatusCode((int)response.StatusCode);
            }

        }
        public async Task<List<Evaluate>> Gd(int laptopId)
        {
            /*var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userId = await GetUserId();*/
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:4000/api/Evaluate/GetEvaluatesByLaptopId/" + laptopId);
            var responseData = await response.Content.ReadAsStringAsync();

            // Xử lý dữ liệu responseData theo nhu cầu của bạn
            var evaluations = JsonConvert.DeserializeObject<List<Evaluate>>(responseData);

            return evaluations.ToList();

        }
    }
}
