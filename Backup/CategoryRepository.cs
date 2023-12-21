using AutoMapper;
using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Services
{
    public class CategoryRepository : ICategoryRepository
    {
/*        private readonly ApplicationLaptopDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(ApplicationLaptopDbContext context, IMapper mapper) 
        { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Add(CategoryModel category)
        {
           var cate = _mapper.Map<Category>(category);
            _context.Categories.Add(cate);
            await _context.SaveChangesAsync();
            return cate.Id;
        }

        public async Task Delete(int id)
        {
            var del = _context.Categories!.SingleOrDefault(c => c.Id == id);
            if (del != null)
            {
                _context.Categories.Remove(del);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CategoryModel>> GetAll()
        {
            var cates = await _context.Categories!.ToListAsync();
            return _mapper.Map<List<CategoryModel>>(cates);
        }

        public async Task<CategoryModel> GetById(int id)
        {
            var cate = await _context.Categories!.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CategoryModel>(cate);
        }

        public async Task Update(int id, CategoryModel category)
        {
            if (id == category.Id)
            {
                var update = _mapper.Map<Category>(category);
                _context.Categories!.Update(update);
                await _context.SaveChangesAsync();
            }
        }*/
        /*public CategoryModel Add(CategoryModel category)
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

public async Task<List<CategoryModel>> GetAll()
{
   var cates = await _context.Categories!.ToListAsync();
   return _mapper.Map<List<CategoryModel>>(cates);

}

public CategoryModel GetById(int id)
{
   c// Hoặc return new CategoryModel { Id = 0, Name = "" }; tùy thuộc vào logic của ứng dụng
}

public void Update(CategoryModel category)
{
   var cate = _context.Categories.FirstOrDefault(ct => ct.Id == category.Id);
   if (cate != null)
   {
       cate.Name = category.Name;
       _context.SaveChanges();
   }
}*/
    }
}
