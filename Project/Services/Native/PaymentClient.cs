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
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IOrderClient _orderService;

		public PaymentClient(
			ApplicationDbContext context,
			IVPSClient vpsService,
			UserManager<ApplicationUser> userManager,
			IOrderClient orderService
		) {
			this._context = context;
			this._vpsService = vpsService;
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

		public async Task<VPS> CreatePayment(Payment payment) {
			this._context.Payments.Add(payment);
			ApplicationUser user = await this._context.Users.FirstOrDefaultAsync(user => user.Id == payment.UserId);
			Order order = await this._context.Orders.FirstOrDefaultAsync(order => order.Id == payment.OrderId);
			user.Payments.Add(payment);
			order.Payment = payment;
			await this._context.SaveChangesAsync();
			await this._orderService.UpdateState(order, OrderState.Finished);
			return await this._vpsService.RegisterVPSFor(order);
		}
	}
}