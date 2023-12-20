using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Database
{
    [Table("LaptopStatus")]
    public class LaptopStatus
    {
        [Key]
        public int LaptopStatusId { get; set; }
        public string Categoty { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Size { get; set; }
        public ICollection<Image>? Images { get; set; }
        [ForeignKey("LaptopId")]
        public int LaptopId {  get; set; }

    }
}
