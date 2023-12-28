namespace LaptopStoreApi.Models
{
    public class UserModel
    {
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDay { get; set; } = DateTime.MinValue;
        public string Address { get; set; } = null!;
        public int? Age { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
