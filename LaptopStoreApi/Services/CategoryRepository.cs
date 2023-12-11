using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LaptopStoreApi.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationLaptopDbContext _context;
        public CategoryRepository(ApplicationLaptopDbContext context) 
        { 
            _context = context;
        }
        public CategoryModel Add(CategoryModel category)
        {
            var cate = new Category
            {
                Name = category.Name
            };
            _context.Categories.Add(cate);
            _context.SaveChanges();
            return new CategoryModel 
            { 
                Name = cate.Name,
                Id = cate.Id
            };
        }

        public void Delete(int id)
        {
            var cate = _context.Categories.FirstOrDefault(ct => ct.Id == id);
            if (cate != null)
            {
                _context.Categories.Remove(cate);
                _context.SaveChanges();
            }
        }

        public List<CategoryModel> GetAll()
        {
            var cates = _context.Categories.Select(ct => new CategoryModel { Id = ct.Id, Name = ct.Name });
            return cates.ToList();
        }

        public CategoryModel GetById(int id)
        {
            var cate = _context.Categories.FirstOrDefault(ct => ct.Id == id);
            if (cate != null)
            {
                return new CategoryModel
                {
                    Id = cate.Id,
                    Name = cate.Name
                };
            }
            return new CategoryModel(); // Hoặc return new CategoryModel { Id = 0, Name = "" }; tùy thuộc vào logic của ứng dụng
        }

        public void Update(CategoryModel category)
        {
            var cate = _context.Categories.FirstOrDefault(ct => ct.Id == category.Id);
            if (cate != null)
            {
                cate.Name = category.Name;
                _context.SaveChanges();
            }
        }
    }
}
