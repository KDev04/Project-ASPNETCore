using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models
{
    public class PdfModel
    {
        public int? IdOrder { get; set; }
        public string? Name { get; set; }
        public int? Phone { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Location { get; set; }
        public StatusOrder? StatusOrder { get; set; }
        public string? Note { get; set; }
    }
}
