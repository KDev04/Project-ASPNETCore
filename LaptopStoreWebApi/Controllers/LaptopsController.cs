using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopStoreWebApi.Data;

namespace LaptopStoreWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly LaptopDbContext _context;

        public LaptopsController(LaptopDbContext context)
        {
            _context = context;
        }

        // GET: api/Laptops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Laptop>>> GetLaptops()
        {
            return await _context.Laptops.ToListAsync();
        }

        // GET: api/Laptops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Laptop>> GetLaptop(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);

            if (laptop == null)
            {
                return NotFound();
            }

            return laptop;
        }

        // PUT: api/Laptops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaptop(int id, Laptop laptop)
        {
            if (id != laptop.LaptopId)
            {
                return BadRequest();
            }

            _context.Entry(laptop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaptopExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Laptops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Laptop>> PostLaptop(Laptop laptop)
        {
            _context.Laptops.Add(laptop);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLaptop", new { id = laptop.LaptopId }, laptop);
        }

        // DELETE: api/Laptops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaptop(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }

            _context.Laptops.Remove(laptop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptops.Any(e => e.LaptopId == id);
        }
    }
}
