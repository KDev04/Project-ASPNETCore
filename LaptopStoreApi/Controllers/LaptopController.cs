using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using LaptopStoreApi.Services;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ILaptopRepository _repository;

        public LaptopController(ILaptopRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
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
        [HttpGet("Filter")]
        public IActionResult Filter(string name, decimal? from, decimal? to, string sortBy, int page =1) 
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

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
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




        [HttpGet("Search/{keyword}")]
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

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] LaptopModel model)
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

        [HttpDelete("Delete/{id}")]
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

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] LaptopModel model)
        {
            if (id != model.MaLaptop)
            {
                return BadRequest();
            }

            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    await _repository.Update(model);
                    return Ok(model);
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
