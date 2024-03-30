namespace LaptopStore.Models
{
    public class PageCategoryModel
    {
        public List<ConsolidatedCategory>? Categories { get; set; }
        public List<Laptop>? Laptops { get; set; }
    }
    public class PageLaptopModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public List<ConsolidatedLaptop>? Laptops { get; set;}
        public List<ConsolidatedCategory>? Categories { get; set; }
    }
}
