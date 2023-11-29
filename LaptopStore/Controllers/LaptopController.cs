using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
namespace LaptopStore.Controllers
{
    public class LaptopController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Detail() => View();
    }
}
