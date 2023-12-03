using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class CategoryModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

    }
}
