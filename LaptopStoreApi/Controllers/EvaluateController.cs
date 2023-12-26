using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Authorization;
namespace LaptopStoreApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EvaluateController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public EvaluateController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("{LaptopId}")] 
        public ActionResult GetEvaluatesByLaptopId(int LaptopId) 
        { 
            var Evals = _context.Evaluates.Include(x => x.User).Include(e => e.Laptop).Where(e=>e.LaptopId == LaptopId).ToList();
            return Ok(Evals);
        }
        [HttpGet("{UserId}")]
        public ActionResult GetEvaluatesByUserId(string UserId)
        {
            var Evals = _context.Evaluates.Include(e => e.Laptop).Include(x => x.User).Where(e => e.UserId == UserId).ToList();
            return Ok(Evals);
        }

        [HttpPost]
        public IActionResult AddEvaluate([FromForm] EvalModel evaluate)
        {
            var user = _context.Users.FirstOrDefault(u=>u.Id == evaluate.UserId);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng");
            }
            var laptop = _context.Laptops.FirstOrDefault(l => l.LaptopId == evaluate.LaptopId);
            if (laptop == null)
            {
                return BadRequest("Không tìm thấy laptop");
            }
            Evaluate eval = new Evaluate()
            {
                UserId = evaluate.UserId,
                User = user,
                LaptopId = evaluate.LaptopId,
                Laptop = laptop,
                Cmt = evaluate.Cmt,
                Rate = evaluate.Rate,
            };
            _context.Evaluates.Add(eval);
            _context.SaveChanges();
            return Ok(eval);

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteEval (int id)
        {
            var eval = _context.Evaluates.Where(e => e.Id == id).FirstOrDefault();
            if (eval == null)
            {
                return BadRequest("Xóa không thành công");
            }
            else
            {
                _context.Evaluates.Remove(eval);
                _context.SaveChanges();
                return Ok("Cucsess");
            }
        }
    }
}
