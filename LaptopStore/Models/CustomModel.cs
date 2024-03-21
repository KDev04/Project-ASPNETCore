using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models
{
    public class CustomModel
    {
        public List<Laptop> Laptops { get; set; }
        public List<OrderOffline> OrderOfflines { get; set; }

    }
}
