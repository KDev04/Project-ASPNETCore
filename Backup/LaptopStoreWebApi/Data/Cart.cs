using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreWebApi.Data
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
