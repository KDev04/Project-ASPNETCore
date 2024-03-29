namespace LaptopStoreApi.Models
{
    public class RoleClaim
    {
        public string? RoleName { get; set; }
        public List<UserClaim>? Claims { get; set; }
    }
    public class UserRoleClaim
    {

        public string? UserName { get; set; }
        public List<RoleClaim>? RoleClaims { get; set; }
    }
}
