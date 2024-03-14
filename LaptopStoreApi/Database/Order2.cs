using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Database
{
    public class Order2
    {
        [Key]
        public int Id { get; set; }
/*        public string Code { get; set; }*/
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> Items { get; set; }
        public decimal Total { get; set; }
        public Guid PromotionCode { get; set; }

    }
}
