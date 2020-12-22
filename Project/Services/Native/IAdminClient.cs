namespace Project.Services.Native {
	public interface IAdminClient {
		int UsersCount();
		int AnnouncementsCount();
		int VPSsCount();
		int OrdersCount();
		int PromoCodesCount();
		int TicketsCount();
		int PlansCount();
	}
}