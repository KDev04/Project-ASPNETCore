using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
