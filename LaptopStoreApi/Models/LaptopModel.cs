using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class LaptopModel
    {
        public int MaLaptop { get; set; }
        public string TenLaptop { get; set; } = string.Empty;

        public decimal Gia { get; set; }
        public byte GiamGia { get; set; }

        public double LoaiManHinh { get; set; }

        public string Mau { get; set; } = string.Empty;

        public int NamSanXuat { get; set; }

        public string Mota { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string ImgPath { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
