using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Database
{
   
    public class OrderOffline
    {
        [Key]
        public int IdOrder { get; set; }
        public int LaptopId { get; set; }
        public int LaptopName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public DateTime? OrderDate { get; set; }
     
    }

   
}
