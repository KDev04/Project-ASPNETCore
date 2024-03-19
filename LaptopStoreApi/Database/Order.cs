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
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }
        public bool? IsExport { get; set; } = true;
        public string? PromotionCode { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public StatusOrder StatusOrder { get; set; }

    }
    public enum StatusOrder
    {
        New = 0, Shipping = 1, Complete = 2, Cancel = -1
    }
}
