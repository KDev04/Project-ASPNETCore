using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Data
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        public int MaLaptop { get; set; }
        [MaxLength(100)]
        public string TenLaptop { get; set; } = string.Empty;

        public decimal Gia { get; set; }
        public byte GiamGia { get; set; }

        public double LoaiManHinh { get; set; }
         public string Hangsx { get; set; } = string.Empty;

        public string Mau { get; set; } = string.Empty;

        public int NamSanXuat { get; set; }

        public string Mota { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        
        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public ICollection<DonHangChiTiet>? DonHangChiTiets { get; set; }
        public Laptop()
        {
            DonHangChiTiets = new HashSet<DonHangChiTiet>();

        }

    }

    [Table("Homepage")]
    public class Homepage
    {
        [Key]
        public int? HomePageId { get; set; }

        [Required]
        public string? VideoUrl { get; set; }
        [Required]
        public string? SlideImageUrl { get; set; }
        public int? MaLaptop { get; set; }
        [ForeignKey("MaLaptop")]
        public Laptop? Laptop { get; set; }
    }


    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<Laptop>? Laptops { get; set; }
    }
}
