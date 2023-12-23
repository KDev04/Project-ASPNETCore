using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models;
using System.Net.Http;
using Newtonsoft.Json;
namespace LaptopStore.Controllers
{
    public class AuthController : Controller
    {
         public IActionResult Login()
        {
            return View();
        }
         public IActionResult Register()
        {
            return View();
        }
    }
}
