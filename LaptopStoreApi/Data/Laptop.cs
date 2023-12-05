using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Data
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        public Guid MaLaptop { get; set; }
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
