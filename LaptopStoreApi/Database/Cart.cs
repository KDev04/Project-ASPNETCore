using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LaptopStoreApi.Database
{
    public class Cart
    {
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        [ForeignKey("Laptop")]
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; }

        public string Name { get; set; } = string.Empty;    
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImgPath { get; set; } = string.Empty;
   
    }
}
