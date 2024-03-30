namespace LaptopStore.Models
{
    public class UserAuthority
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public List<ClaimModel>? Claims { get; set; }
    }

    public class ClaimModel
    {
        public string? Type { get; set; }
        public string? Value { get; set; }
    }
    
}
