using LaptopStoreApi.Models;
using System.Threading.Tasks;
namespace LaptopStoreApi.Services
{
    public interface ICategoryRepository
    {
        List<CategoryModel> GetAll();
        CategoryModel GetById(int id);
        CategoryModel Add(CategoryModel category);
        void Delete(int id);
        void Update(CategoryModel category);
    }
}
