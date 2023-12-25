namespace LaptopStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int LaptopId { get; set; }
        public Laptop? Laptop { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
