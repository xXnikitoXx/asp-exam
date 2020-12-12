using System.Linq;
using Project.Data;

namespace Project.Services.Native {
	public class AdminClient : IAdminClient {
		private readonly ApplicationDbContext _context;

		public AdminClient(ApplicationDbContext context) => this._context = context;

		public int UsersCount() => this._context.Users.Count();
		public int AnnouncementsCount() => this._context.Announcements.Count();
		public int VPSsCount() => this._context.VPSs.Count();
		public int OrdersCount() => this._context.Orders.Count();
		public int PromoCodesCount() => this._context.PromoCodes.Count();
		public int TicketsCount() => this._context.Tickets.Count();
	}
}