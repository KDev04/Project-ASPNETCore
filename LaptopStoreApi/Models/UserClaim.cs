using LaptopStoreApi.Database;
using System.Security.Policy;

namespace LaptopStoreApi.Models
{
    public class UserInfo
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public List<UserClaim>? Claims { get; set; }
/*        public List<RoleClaim>? RoleClaims { get; set; }*/
    }

    public class UserClaim
    {
        public string? Type { get; set; }
        public string? Value { get; set; }
    }
    public class UserAndClaim
    {
        public User? user { get; set; }
        public List<UserClaim>? Claims { get; set; }
    }
}
