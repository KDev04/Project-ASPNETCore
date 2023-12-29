using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models
{
    public class LaptopStatus
    {
        public int LaptopStatusId { get; set; }
        public ICollection<Image>? Images { get; set; }
        public string Information { get; set; }
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; } // Navigation Property
    }

    public class LaptopDetailViewModel
    {
        public Laptop Laptop { get; set; }
        public List<LaptopStatus> LaptopStatusList { get; set; }
    }
}
