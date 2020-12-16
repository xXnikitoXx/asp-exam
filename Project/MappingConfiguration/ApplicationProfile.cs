using System;
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
			CreateMap<ApplicationUser, UserViewModel>();
			CreateMap<VPS, VPSViewModel>();
			CreateMap<Models.Plan, PlanViewModel>();
			CreateMap<Ticket, TicketViewModel>();
			CreateMap<PromoCode, PromoCodeViewModel>()
				.ForMember(dest => dest.Usage, opt => opt.MapFrom(src => src.Orders.Count));
			CreateMap<Order, OrderViewModel>()
				.ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.Plan));

			CreateMap<PromoCodeEditInputModel, PromoCode>()
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => (PromoCodeType)Enum.Parse(typeof(PromoCodeType), src.Type)));
			CreateMap<AnnouncementInputModel, Announcement>()
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => (MessageType)Enum.Parse(typeof(MessageType), src.Type)));
			CreateMap<TicketCreateInputModel, Ticket>()
				.ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Priority)Enum.Parse(typeof(Priority), src.Priority)));
		}
	}
}
