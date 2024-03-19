namespace LaptopStoreApi.Database
{
    public class OrderDetail
    {
        public int? LaptopId { get; set; }
        public Laptop? Laptop { get; set; }
        public int? Order2Id { get; set; }
        public Order2? Order2 { get; set; }
        public int? Quantity {  get; set; }
        public decimal? Price { get; set; }

    }
}
