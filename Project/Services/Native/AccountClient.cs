using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models;

namespace Project.Services.Native {
	public class AccountClient : IAccountClient
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IRoleClient _roleService;
		private readonly IOrderClient _orderService;
		public AccountClient(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IRoleClient roleService,
			IOrderClient orderService
		) {
			this._context = context;
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._roleService = roleService;
			this._orderService = orderService;
		}

		public async Task SignOut(ApplicationUser user) => await this._userManager.UpdateSecurityStampAsync(user);

		public async Task RemoveUser(string id) {
			ApplicationUser user = this._context.Users.FirstOrDefault(user => user.Id == id);
			if (user == null)
				throw new Exception();
			await SignOut(user);
			List<Order> finishedOrders = this._orderService.FinishedOrders(user)
				.Select(order => new Order {
					TimeStarted = order.TimeStarted,
					TimeFinished = order.TimeFinished,
					Amount = order.Amount,
					OriginalPrice = order.OriginalPrice,
					FinalPrice = order.FinalPrice,
					PlanNumber = order.PlanNumber,
					Location = order.Location,
					State = order.State,
				})
				.ToList();
			this._context.UserPromoCodes.RemoveRange(user.PromoCodes);
			this._context.Activities.RemoveRange(user.Activities);
			this._context.Messages.RemoveRange(user.Messages);
			if (await this._roleService.IsAdmin(user))
				await this._roleService.DemoteToUser(user);
			await this._userManager.DeleteAsync(user);
			await this._context.Orders.AddRangeAsync(finishedOrders);
			await this._context.SaveChangesAsync();
		}
	}
}