using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Database
{
    public class Promotion
    {
        [Key]
        public Guid PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = false;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal PromotionValue { get; set; }
    }
    public class PromotionInputModel
    {
        [Required]
        public string PromotionName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public decimal PromotionValue { get; set; }
    }
}
