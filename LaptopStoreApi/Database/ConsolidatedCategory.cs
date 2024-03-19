namespace LaptopStoreApi.Database
{
    public class ConsolidatedCategory
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<Laptop>? Laptops { get; set; }
    }
}
