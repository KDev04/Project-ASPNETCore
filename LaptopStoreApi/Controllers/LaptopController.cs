using LaptopStoreApi.Constants;
using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ILapRepo2 _repository;
        public LaptopController(ILapRepo2 repo2)
        {
            _repository = repo2;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetLaptops()
        {
            try
            {
                var laptops = await _repository.GetAll();
                return Ok(laptops);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Filter(string name, decimal? from, decimal? to, string sortBy, int page = 1)
        {
            try
            {
                var laptops = _repository.Filter(name, from, to, sortBy, page);
                return Ok(laptops);
            }
            catch
            {
                return BadRequest("khong hoat dong");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                var searchResult = await _repository.Search(keyword);
                return Ok(searchResult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLaptop(int id)
        {
            try
            {
                var laptop = await _repository.GetById(id);
                if (laptop == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(laptop);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /*[Authorize]*/
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<IActionResult> Add([FromForm] LapModel2 model)
        {
            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    var newLaptop = await _repository.Add(model);
                    return Ok(newLaptop);
                }
                else
                {
                    return BadRequest("No image file found");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
