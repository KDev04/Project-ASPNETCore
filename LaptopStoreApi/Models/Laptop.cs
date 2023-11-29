using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Models
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]  
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        [Required]
        public double Type { get; set; }
        [Required]
        public string Color { get; set; } = string.Empty;
        [Required]
        public int Year { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
