using LaptopStoreApi.Data;
using LaptopStoreApi.Models;
namespace LaptopStoreApi.Services
{
    public interface ILaptopRepository
    {
        List<Laptop> GetAll();
        Laptop GetById(int id);
        Laptop Add(LaptopModel model);
        void Update(LaptopModel model);
        void Delete(int id);
    }
}
