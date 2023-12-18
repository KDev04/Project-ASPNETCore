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
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:4000/api/Laptop/GetAll");

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
        public async Task<IActionResult> Detail(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync("http://localhost:4000/api/Laptop/Get/" + id);
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

        public async Task<IActionResult> SaveProduct(Laptop model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        using (var formData = new MultipartFormDataContent())
                        {
                            formData.Add(new StringContent(model.TenLaptop?.ToString() ?? ""), "TenLaptop");
                            formData.Add(new StringContent(model.Gia?.ToString() ?? ""), "Gia");
                            formData.Add(new StringContent(model.GiamGia?.ToString() ?? ""), "GiamGia");
                            formData.Add(new StringContent(model.LoaiManHinh?.ToString() ?? ""), "LoaiManHinh");
                            formData.Add(new StringContent(model.Mau ?? ""), "Mau");
                            formData.Add(new StringContent(model.NamSanXuat?.ToString() ?? ""), "NamSanXuat");
                            formData.Add(new StringContent(model.Mota ?? ""), "Mota");
                            formData.Add(new StringContent(model.CategoryId?.ToString() ?? ""), "CategoryId");

                            if (model.Image != null && model.Image.Length > 0)
                            {
                                using (var streamContent = new StreamContent(model.Image.OpenReadStream()))
                                {
                                    formData.Add(streamContent, "Image", model.Image.FileName);

                                    var response = await httpClient.PostAsync("http://localhost:4000/api/Laptop/Add", formData);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        // Xử lý khi tạo laptop thành công
                                        return Redirect("/Laptop/Index");
                                    }
                                    else
                                    {
                                        // Xử lý khi có lỗi từ API
                                        ModelState.AddModelError("", "An error occurred while creating the laptop.");
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "No image file found");
                            }
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "An error occurred");
                    }
                }
            }

            return Redirect("/Laptop/Index");
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
