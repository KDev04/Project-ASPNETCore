using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Database
{
    public class LaptopStatus
    {
        [Key]
        public int IdLaptopStatus { get; set; }
        public string Categoty { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Size { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        [ForeignKey("IdLaptop")]
        public int IdLaptop {  get; set; }

    }
}
