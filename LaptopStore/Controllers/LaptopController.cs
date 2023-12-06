using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http;
using Newtonsoft.Json;
namespace LaptopStore.Controllers
{
    public class LaptopController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync("http://localhost:4000/api/Laptop");
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();

                    // Tiếp theo, bạn có thể xử lý dữ liệu JSON nhận được ở đây
                    // Ví dụ: var laptops = JsonConvert.DeserializeObject<List<Laptop>>(content);

                   var laptops = JsonConvert.DeserializeObject<List<Laptop>>(content);
                    return View(laptops);
                }
                catch (Exception)
                {
                    // Xử lý lỗi khi gặp vấn đề khi gọi API
                    // Ví dụ:
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public IActionResult Detail() => View();
    }
}
