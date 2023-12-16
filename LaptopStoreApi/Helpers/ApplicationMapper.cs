using AutoMapper;
using LaptopStoreApi.Data;
using LaptopStoreApi.Models;

namespace LaptopStoreApi.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        { 
            CreateMap<Category, CategoryModel>().ReverseMap();
        }
    }
}
