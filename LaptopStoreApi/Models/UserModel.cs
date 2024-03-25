namespace LaptopStoreApi.Models
{
    public class UserModel
    {
        public string?UserName { get; set; } = null!;
        public string?FullName { get; set; } = null!;
        public string?Address { get; set; } = null!;
        public string?Email { get; set; } = null!;
        public string?PhoneNumber { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
