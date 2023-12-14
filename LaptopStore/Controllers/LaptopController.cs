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
                    var response = await httpClient.GetAsync("http://localhost:4000/api/Laptop/GetAll");
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

        public async Task<IActionResult> SaveProduct(Laptop model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        if (model.Image != null && model.Image.Length > 0)
                        {
                            using (var formData = new MultipartFormDataContent())
                            {
                                formData.Add(new StringContent(model.TenLaptop), "TenLaptop");
                                formData.Add(new StringContent(model.Gia.ToString()), "Gia");
                                formData.Add(new StringContent(model.GiamGia.ToString()), "GiamGia");
                                formData.Add(new StringContent(model.LoaiManHinh.ToString()), "LoaiManHinh");
                                formData.Add(new StringContent(model.Mau), "Mau");
                                formData.Add(new StringContent(model.NamSanXuat.ToString()), "NamSanXuat");
                                formData.Add(new StringContent(model.Mota), "Mota");
                                formData.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");

                                formData.Add(new StreamContent(model.Image.OpenReadStream()), "Image", model.Image.FileName);

                                var response = await httpClient.PostAsync("http://localhost:4000/api/Laptop/Add", formData);
                                return Redirect("/Laptop/");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "No image file found");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "An error occurred");
                    }
                }
            }

            return View("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
