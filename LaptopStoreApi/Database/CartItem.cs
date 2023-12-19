using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LaptopStoreApi.Database
{
    public class CartItem
    {
        public int IdCartItem {  get; set; }
        public Laptop Laptop { get; set; } = new();
        public int Quantity { get; set; }
    }
}
