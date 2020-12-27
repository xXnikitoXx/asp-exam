using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;

namespace Project.Services.Native {
	public class PaymentClient : IPaymentClient {
		private readonly ApplicationDbContext _context;
		private readonly IVPSClient _vpsService;
		private readonly IPlanClient _planService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IOrderClient _orderService;

		public PaymentClient(
			ApplicationDbContext context,
			IVPSClient vpsService,
			IPlanClient planService,
			UserManager<ApplicationUser> userManager,
			IOrderClient orderService
		) {
			this._context = context;
			this._vpsService = vpsService;
			this._planService = planService;
			this._userManager = userManager;
			this._orderService = orderService;
		}

		public async Task<Payment> Find(string id) =>
			await this._context.Payments.FirstOrDefaultAsync();

		public Payment GetPayment(string orderId) =>
			this._context.Payments.FirstOrDefault(payment => payment.OrderId == orderId);

		public Payment GetPayment(Order order) =>
			GetPayment(order.Id);

		public List<Payment> GetPayments(ApplicationUser user) =>
			this._context.Payments.Where(payment => payment.UserId == user.Id).ToList();
		public async Task<List<Payment>> GetPayments(ClaimsPrincipal user) =>
			GetPayments(await this._userManager.GetUserAsync(user));

		public async Task<List<VPS>> CreatePayment(Payment payment) {
			List<Models.Plan> plans = this._planService.GetPlans();
			this._context.Payments.Add(payment);
			ApplicationUser user = await this._context.Users
				.FirstOrDefaultAsync(user => user.Id == payment.UserId);
			Order order = await this._context.Orders
				.FirstOrDefaultAsync(order => order.Id == payment.OrderId);
			user.Payments.Add(payment);
			order.Payment = payment;
			order.Plan = plans.FirstOrDefault(plan => plan.Number == order.PlanNumber);
			List<PromoCodeOrder> otherOrdersCodes = this._context.PromoCodeOrders
				.Where(pco => order.PromoCodes.Select(pc => pc.PromoCodeId).Contains(pco.PromoCodeId))
				.ToList();
			this._context.PromoCodeOrders.RemoveRange(otherOrdersCodes);
			foreach (PromoCodeOrder pco in order.PromoCodes)
				this._context.UserPromoCodes.Add(new UserPromoCode {
					User = order.User,
					PromoCode = pco.PromoCode,
				});
			await this._context.SaveChangesAsync();
			await this._orderService.UpdateState(order, OrderState.Finished);
			List<VPS> vpss = new List<VPS>();
			for (int i = 0; i < order.Amount; i++)
				vpss.Add(await this._vpsService.RegisterVPSFor(order, i));
			return vpss;
		}
	}
}