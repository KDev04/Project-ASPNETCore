using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

    }
}
