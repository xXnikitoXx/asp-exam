using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native
{
	public class OrderClient : IOrderClient
	{
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public OrderClient(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			this._context = context;
			this._userManager = userManager;
		}

		async Task<Order> Find(string id) => await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);

		public async Task<List<Order>> GetOrders(ClaimsPrincipal user) => GetOrders(await _userManager.GetUserAsync(user));

		public List<Order> GetOrders(ApplicationUser user) => _context.Orders
			.Where(order => order.UserId == user.Id)
			.ToList();

		public async Task RegisterOrder(Order order)
		{
			_context.Orders.Add(order);
			_context.Users.Find(order.User)
				.Orders.Add(order);
			await _context.SaveChangesAsync();
		}
		public async Task RemoveOrder(string id) => await RemoveOrder(await Find(id));

		public async Task RemoveOrder(Order order)
		{
			order.User.Orders.Remove(order);
			foreach (VPS vps in order.VPSs)
				vps.Order = null;
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateState(string id, OrderState state) => await UpdateState(await Find(id), state);

		public async Task UpdateState(Order order, OrderState state)
		{
			order.State = state;
			await _context.SaveChangesAsync();
		}

		public async Task AddVPS(string id, VPS vps) => await AddVPS(await Find(id), vps);

		public async Task AddVPS(Order order, VPS vps)
		{
			order.VPSs.Add(vps);
			await _context.SaveChangesAsync();
		}

		public async Task RemoveVPS(string id, VPS vps) => await RemoveVPS(await Find(id), vps);

		public async Task RemoveVPS(Order order, VPS vps)
		{
			order.VPSs.Remove(vps);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateVPSs(string id, List<VPS> vpss) => await UpdateVPSs(await Find(id), vpss);

		public async Task UpdateVPSs(Order order, List<VPS> vpss)
		{
			order.VPSs = vpss;
			await _context.SaveChangesAsync();
		}

		public static Order OrderFromPlan(Models.Plan plan) =>
			new Order() {
				TimeStarted = DateTime.Now,
				Amount = 1,
				OriginalPrice = plan.Price,
				FinalPrice = plan.Price,
				PlanNumber = plan.Number,
				Plan = plan,
				State = OrderState.Created,
			};
	}
}
