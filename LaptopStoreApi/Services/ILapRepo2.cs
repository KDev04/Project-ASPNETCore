using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
namespace LaptopStoreApi.Services
{
    public interface ILapRepo2
    {
        Task<List<Laptop>> GetAll();
        Task<Laptop> GetById(int id);
        Task<Laptop> Add (LapModel2 model);
        Task Update(LapModel2 model);
        Task Delete(int id);
        Task<List<Laptop>> Search(string keyword);
    }
}
