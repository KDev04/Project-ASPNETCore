using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Database
{

    public class InventoryTicket
    {
        [Key]
        public int? Id { get; set; }
        public int IdTicket { get; set; }
        public int? Phone { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }
        public StatusOrder? StatusOrder { get; set; }

        [ForeignKey("LaptopId")]
        public Laptop? Laptop { get; set; } // Đây là thuộc tính để tham chiếu đến đối tượng Laptop
        public int LaptopId { get; set; }
        public string? Products { get; set; }
        public decimal? Price { get; set; }

    }

}
