using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreWebApi.Data
{
    [Table("Laptops")]
    public class Laptop
    {
        [Key]
        public int LaptopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; } // so luong ton kho
        public string ImgPath { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
