using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class EvalModel
    {
        [Range(0, 5)]
        public int Rate { get; set; }
        public string Cmt { get; set; } = null!;
        public int LaptopId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
