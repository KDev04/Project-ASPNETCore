using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http.Headers;
using System.Net.Http;

namespace LaptopStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminController(HttpClient httpClient) { _httpClient = httpClient; }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> SaveProduct(Laptop model)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(model.Name?.ToString() ?? ""), "Name");
                    formData.Add(new StringContent(model.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(model.Quantity.ToString() ?? ""), "Quantity"); ;

                    if (model.Image != null && model.Image.Length > 0)
                    {
                        using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                        {
                            formData.Add(streamContent, "Image", model.Image.FileName);

                            var response = await _httpClient.PostAsync("http://localhost:4000/api/Laptop/Add", formData);

                            if (response.IsSuccessStatusCode)
                            {
                                // Xử lý khi tạo laptop thành công
                                return Redirect("/Laptop/Index");
                            }
                            else
                            {
                                // Xử lý khi có lỗi từ API
                                return Redirect("Admin/Create");
                            }
                        }
                    }
                    else
                    {
                        return Redirect("Admin/Create");
                    }
                }
            }
            catch
            {
                return Redirect("/Home/Error");
            }
        }
    }
}