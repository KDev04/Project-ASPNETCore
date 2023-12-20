﻿using LaptopStoreApi.Models;
using LaptopStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class Laptop2Controller : ControllerBase
    {
        private readonly ILapRepo2 _repository;
        public Laptop2Controller(ILapRepo2 repo2)
        {
            _repository = repo2;
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
        [HttpPost("Add")]
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
