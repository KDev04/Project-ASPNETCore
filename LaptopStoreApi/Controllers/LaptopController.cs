using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaptopStoreApi.Models;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ILogger<ApplicationLaptopDbContext> _logger;
        public LaptopController(ILogger<ApplicationLaptopDbContext> logger)
        {
            _logger = logger;
        }
        //[HttpGet(Name = "GetLaptops")]
        //public IEnumerable<Laptop> Get()
        //{
            
        //}
    }
}
