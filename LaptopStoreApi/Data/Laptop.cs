using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Data
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        [Required]
        public Guid MaLaptop { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenLaptop { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Gia { get; set; }
        public byte GiamGia { get; set; }
        [Required]
        public double LoaiManHinh { get; set; }
        [Required]
        public string Mau { get; set; } = string.Empty;
        [Required]
        public int NamSanXuat { get; set; }
        [Required]
        public string Mota { get; set; } = string.Empty;
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }
        [Required]
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
