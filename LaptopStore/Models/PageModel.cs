namespace LaptopStore.Models
{
    public class PageCategoryModel
    {
        public List<ConsolidatedCategory> Categories { get; set; }
        public List<Laptop> Laptops { get; set; }
    }
    public class PageLaptopModel
    {
        public List<ConsolidatedLaptop> Laptops { get; set;}
        public List<ConsolidatedCategory> Categories { get; set; }
    }
}
