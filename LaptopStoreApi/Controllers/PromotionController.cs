using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public PromotionController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotions()
        {
            var categories = await _dbContext.Promotions.ToListAsync();
            return Ok(categories);
        }
        [HttpGet("{code}")]
        public async Task<ActionResult<Promotion>> GetPromotion(Guid code)
        {
            var promotion = await _dbContext.Promotions.Where(p=>p.PromotionCode == code).FirstOrDefaultAsync();

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }

        [HttpPost]
        public IActionResult CreatePromotion([FromForm] PromotionInputModel inputModel)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tạo một đối tượng Promotion từ dữ liệu nhập vào
            Promotion promotion = new Promotion
            {
                PromotionName = inputModel.PromotionName,
                Description = inputModel.Description,
                Start = inputModel.Start,
                End = inputModel.End,
                PromotionValue = inputModel.PromotionValue
            };

            _dbContext.Promotions.Add(promotion);
            _dbContext.SaveChanges();
            return Ok(promotion);
        }
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteCategory(Guid code)
        {
            var promotion = await _dbContext.Promotions.Where(p=>p.PromotionCode == code).FirstOrDefaultAsync();

            if (promotion == null)
            {
                return NotFound();
            }

            _dbContext.Promotions.Remove(promotion);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdatePromotion(Guid code, PromotionInputModel promotionInputModel)
        {
            var promotion = await _dbContext.Promotions.Where(p => p.PromotionCode == code).FirstOrDefaultAsync();

            if (promotion == null)
            {
                return NotFound();
            }
            promotion.PromotionName = promotionInputModel.PromotionName;
            promotion.PromotionValue = promotionInputModel.PromotionValue;
            promotion.Description = promotionInputModel.Description;
            promotion.Start = promotionInputModel.Start;
            promotion.End = promotionInputModel.End;
            _dbContext.Promotions.Update(promotion);
            return Ok(promotion);

        }
    }
}
