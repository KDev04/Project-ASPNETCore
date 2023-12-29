using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LaptopStoreApi.Database;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Database
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        public int LaptopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? BigPrice { get; set; }
        public string? Color { get; set; } = string.Empty;
        public int Quantity { get; set; } // so luong ton kho
        public string ImgPath { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
