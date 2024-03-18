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
            public string? Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? BigPrice { get; set; }
        public string? Color { get; set; } = string.Empty;
        public int Quantity { get; set; } // so luong ton kho
        public string ImgPath { get; set; } = string.Empty;
    
        public ICollection<LaptopCategory>? LaptopCategories { get; set; }
        public ICollection<LikeProduct>? LikeProducts { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Brand { get; set; } // Thuong hieu 
        public string SeriesLaptop {  get; set; }
        public string Cpu {get; set; }
        public string Chip { get; set; }
        public string RAM {  get; set; }
        public string Memory { get; set; } // bo nho
        public string BlueTooth { get; set; }
        public string Keyboard { get; set; }
        public string OperatingSystem { get; set; }
        public string Pin { get; set; }
        public string weight { get; set; }
        public string Accessory { get; set; } // Phụ kiện
        public string Screen { get; set; } 
    }
}
