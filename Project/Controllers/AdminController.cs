using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize(Roles = "Administrator")]
	public class AdminController : Controller {
		private readonly IAdminClient _service;
		private readonly IRoleClient _roleService;
		private readonly IOrderClient _orderService;

		public AdminController(
			IAdminClient service,
			IRoleClient roleService,
			IOrderClient orderService
		) {
			this._service = service;
			this._roleService = roleService;
			this._orderService = orderService;
		}

		public IActionResult Index() {
			AdminViewModel model = new AdminViewModel {
				UsersCount = this._service.UsersCount(),
				AnnouncementsCount = this._service.AnnouncementsCount(),
				VPSsCount = this._service.VPSsCount(),
				OrdersCount = this._service.OrdersCount(),
				PromoCodesCount = this._service.PromoCodesCount(),
				TicketsCount = this._service.TicketsCount(),
				PlansCount = this._service.PlansCount(),
				DailyIncome = this._orderService.DailyIncome(),
				MonthlyIncome = this._orderService.MonthlyIncome(),
				YearlyIncome = this._orderService.YearlyIncome(),
				TotalIncome = this._orderService.TotalIncome(),
			};
			return View(model);
		}
	}
}