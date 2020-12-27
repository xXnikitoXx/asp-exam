using System;
using AutoMapper;
using Project.Enums;
using Project.Models;
using Project.ViewModels;

namespace Project.MappingConfiguration {
	public class ApplicationProfile : Profile {
		public ApplicationProfile() {
			CreateMap<ApplicationUser, UserViewModel>();
			CreateMap<Models.Plan, PlanViewModel>();
			CreateMap<Ticket, TicketViewModel>();
			CreateMap<Payment, PaymentViewModel>();
			CreateMap<Message, MessageViewModel>();
			CreateMap<ServerData, ServerDataViewModel>();
			CreateMap<Activity, ActivityViewModel>();
			CreateMap<State, StateViewModel>();
			CreateMap<VPS, VPSViewModel>()
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
			CreateMap<Message, NotificationMessageViewModel>()
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Sender.UserName));
			CreateMap<PromoCode, PromoCodeViewModel>()
				.ForMember(dest => dest.Usage, opt => opt.MapFrom(src => src.Orders.Count));
			CreateMap<Order, OrderViewModel>()
				.ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.Plan));

			CreateMap<PromoCodeEditInputModel, PromoCode>()
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => (PromoCodeType)Enum.Parse(typeof(PromoCodeType), src.Type)));
			CreateMap<AnnouncementInputModel, Announcement>()
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => (NotificationType)Enum.Parse(typeof(NotificationType), src.Type)));
			CreateMap<TicketCreateInputModel, Ticket>()
				.ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Priority)Enum.Parse(typeof(Priority), src.Priority)));
			CreateMap<MessageCreateInputModel, Message>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (MessageStatus)Enum.Parse(typeof(MessageStatus), src.Status)));
			CreateMap<PaymentInputModel, Payment>()
				.ForMember(dest => dest.PayPalPayment, opt => opt.MapFrom(src => src.PaymentId))
				.ForMember(dest => dest.PayPalPayer, opt => opt.MapFrom(src => src.PayerId));
			CreateMap<PlanEditInputModel, Models.Plan>();
			CreateMap<VPSSetupInputModel, VPS>();
		}
	}
}
