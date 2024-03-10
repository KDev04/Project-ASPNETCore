namespace LaptopStoreApi.Database
{
    public class LikeProduct
    {
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
