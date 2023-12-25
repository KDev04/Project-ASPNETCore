using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
