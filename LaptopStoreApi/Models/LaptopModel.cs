using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class LaptopModel
    {
        [Required]
        [MaxLength(100)]
        public string TenLaptop { get; set; } = string.Empty;
        [Range(100000, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Gia { get; set; }
        public byte GiamGia { get; set; }

        public double LoaiManHinh { get; set; }

        public string Mau { get; set; } = string.Empty;

        public int NamSanXuat { get; set; }

        public string Mota { get; set; } = string.Empty;
        public int? CategoryId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
