using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LaptopStoreApi.Database
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        public int IdLaptop { get; set; }
        public string Name { get; set; } = string.Empty;    
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImgPath { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
