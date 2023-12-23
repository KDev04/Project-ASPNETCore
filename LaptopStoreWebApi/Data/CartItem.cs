using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreWebApi.Data
{

    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public int LaptopId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public Laptop? Laptop { get; set; }
    }
    public class ItemModel
    {
        public int LaptopId { get; set; }
        public int Quantity { get; set; }
    }
}
