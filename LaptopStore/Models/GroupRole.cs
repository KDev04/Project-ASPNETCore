using Microsoft.AspNetCore.Identity;

namespace LaptopStore.Models
{
    public class GroupRole
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<IdentityRole>? Roles { get; set; }
        public List<UserGroupRole>? UserGroups { get; set; }
    }
    public class UserGroupRole
    {
        public int? GroupRoleId { get; set; }
        public GroupRole? GroupRole { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
    public class GroupRoleModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Roles { get; set; }
    }

    public class UserRoles
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public List<GroupRole>? Roles { get; set; }
    }

    public class UserRolePage
    {
        public List<User>? Users { get; set; }
        public List<UserRoles>? Roles { get; set; }
    }
    public class UserGroupRolePage
    {
        public List<User>? Users { get; set; }
        public List<GroupRole>? GroupRoles { get; set; }
    }
}
