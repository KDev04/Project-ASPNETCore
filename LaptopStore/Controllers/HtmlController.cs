using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Humanizer;
using LaptopStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaptopStore.Controllers
{
    public class HtmlController : Controller
    {

        public IActionResult Profilepage()
        {
            return View();
        }
        public IActionResult Faqpage()
        {
            return View();
        }
        public IActionResult Contactpage()
        {
            return View();
        }
        public IActionResult Errorpage()
        {
            return View();
        }

        public IActionResult Blankpage()
        {
            return View();
        }
    }
}
