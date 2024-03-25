using Microsoft.AspNetCore.Identity;

namespace LaptopStoreApi.Database
{
    public class User: IdentityUser
    {
        public string? FullName { get; set; } = null!;
        public string? AvatarUrl { get; set;} = null!;
        public string? Address { get; set; } = null!;
        public ICollection<LikeProduct> LikeProducts { get; set; }
    }
     
}
