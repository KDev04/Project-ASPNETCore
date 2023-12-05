using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class LaptopModel
    {
        [Required]
        [MaxLength(100)]
        public string TenLaptop { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
