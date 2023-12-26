using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;

namespace LaptopStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

         public IActionResult Create()
        {
            return View();
        }

    }
}