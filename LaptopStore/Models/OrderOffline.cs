using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models 
{
   
    public class OrderOffline
    {
        [Key]
        public int? Phone { get; set; }
        public string? Name  { get; set; }
        public int IdOrder { get; set; }
        public int LaptopId { get; set; }
        public string Products { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public DateTime? OrderDate { get; set; }
     
    }

   
}
