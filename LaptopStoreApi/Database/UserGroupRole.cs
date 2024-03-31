using Microsoft.AspNetCore.Identity;

namespace LaptopStoreApi.Database
{
    public class UserGroupRole
    {
        public int?GroupRoleId { get; set; }
        public GroupRole?GroupRole { get; set; }

        public string? UserId { get; set; }
        public User?User { get; set; }
    }

}
