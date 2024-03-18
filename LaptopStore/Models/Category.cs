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
   
}
