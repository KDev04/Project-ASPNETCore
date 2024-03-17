namespace LaptopStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<LaptopCategory> LaptopCategories { get; set; }
    }
    public class ConsolidatedCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Laptop> Laptops { get; set; }
    }
    public class LaptopCategory
    {
        public int LaptopId { get; set; }

        public Laptop Laptop { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
