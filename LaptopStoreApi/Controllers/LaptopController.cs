using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaptopStoreApi.Data;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ApplicationLaptopDbContext _logger;
        public LaptopController(ApplicationLaptopDbContext logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetLaptops")]
        public IEnumerable<Laptop> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Laptop
            {
                TenLaptop = "EFG"

            })
        .ToArray();
        }
    }
}
