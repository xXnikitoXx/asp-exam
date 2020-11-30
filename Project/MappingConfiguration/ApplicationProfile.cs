using AutoMapper;
using Project.Enums;
using Project.Models;
using Project.ViewModels;

namespace Project.MappingConfiguration
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Order, OrderViewModel>();
			CreateMap<VPS, VPSViewModel>();
        }
    }
}
