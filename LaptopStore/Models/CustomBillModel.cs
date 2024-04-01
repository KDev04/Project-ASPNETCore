using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models
{
    public class CustomBillModel
    {
        public List<Laptop>? Laptops { get; set; }
        public List<OrderOffline>? OrderOfflines { get; set; }
        public List<Bill>? Bills { get; set; }
        public List<InventoryTicket> InventoryTickets { get; set; }



    }
}
