using AutoMapper;
using FRN.Application._1._3_ViewModels;
using FRN.Domain._2._2_Entity;

namespace FRN.Application._1._4_SeedWork
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Product, ProductViewModel>();
        }
    }
}
