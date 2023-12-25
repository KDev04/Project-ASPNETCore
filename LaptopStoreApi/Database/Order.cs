using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LaptopStoreApi.Database
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int LaptopId { get; set; }
        public Laptop? Laptop { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

    }
}
