using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopStoreWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using LaptopStoreWebApi.Models;
using NuGet.Protocol.Core.Types;
using System.Drawing.Printing;

namespace LaptopStoreWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly LaptopDbContext _context;
        public static int Page_size { set; get; } = 5;
        public LaptopController(LaptopDbContext context)
        {
            _context = context;
        }

        // GET: api/Laptops
        [HttpGet]
/*        [Authorize]*/
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
        public async Task<ActionResult<Laptop>> Add(LaptopModel model)
        {
            if (model.Image == null || model.Image.Length == 0)
            {
                // Xử lý khi không có tệp hình ảnh được gửi lên
                // Ví dụ: trả về lỗi hoặc thông báo không có tệp hình ảnh
                throw new Exception("Không có tệp hình ảnh được gửi lên.");
            }

            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model?.Image?.FileName);
            string imgFolderPath = Path.Combine("wwwroot/Image"); // Thư mục "wwwroot/Image"
            string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

            if (!Directory.Exists(imgFolderPath))
            {
                Directory.CreateDirectory(imgFolderPath);
            }

            using (var stream = new FileStream(imgFilePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            var laptop = new Laptop
            {
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ImgPath = "Image/" + imgFileName
            };
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
        [HttpGet]
        public IActionResult Filter(string name ="", string sortBy = "", int page = 1, int from = 0, int to = int.MaxValue)
        {
            try
            {
                var allLaptops = _context.Laptops.AsQueryable();

                #region Filtering
                if (!string.IsNullOrEmpty(name))
                {
                    allLaptops = allLaptops.Where(l => l.Name.Contains(name));
                }
                if (from>0)
                {
                    allLaptops = allLaptops.Where(l => l.Price >= from);
                }
                if (to > 0 && to >=from)
                {
                    allLaptops = allLaptops.Where(l => l.Price <= to);
                }
                #endregion

                #region Sorting
                // mac dinh la name
                allLaptops = allLaptops.OrderBy(l => l.Name);
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "TenLaptop_asc": allLaptops = allLaptops.OrderBy(l => l.Name); break;
                        case "TenLaptop_desc": allLaptops = allLaptops.OrderByDescending(l => l.Name); break;
                        case "Gia_asc": allLaptops = allLaptops.OrderBy(l => l.Price); break;
                        case "Gia_desc": allLaptops = allLaptops.OrderByDescending(l => l.Price); break;
                    }
                }
                #endregion

                var db = Paging<Laptop>.Create(allLaptops, page, Page_size);
                var result = db.Select(model => new Laptop
                {
                    Name = model.Name,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    CreateDate = model.CreateDate,
                    LastModifiedDate = model.LastModifiedDate,
                    ImgPath = model.ImgPath,
                });

                return Ok(result);
            }
            catch
            {
                return BadRequest("khong hoat dong");
            }
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptops.Any(e => e.LaptopId == id);
        }
    }
}
