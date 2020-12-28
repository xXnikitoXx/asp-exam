using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native {
	public class OrderClient : IOrderClient {
		private readonly IPlanClient _planService;
		private readonly IPromoCodeClient _codeService;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrderClient(
			IPlanClient planService,
			IPromoCodeClient codeService,
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager
		) {
			this._planService = planService;
			this._codeService = codeService;
			this._context = context;
			this._userManager = userManager;
		}

		public async Task<Order> Find(string id) =>
			await this._context.Orders.FirstOrDefaultAsync(order => order.Id == id);

		public async Task<List<Order>> GetOrders(ClaimsPrincipal user) =>
			GetOrders(await _userManager.GetUserAsync(user));
		public async Task<List<Order>> GetOrders(ClaimsPrincipal user, OrdersViewModel pageInfo) =>
			GetOrders(await _userManager.GetUserAsync(user), pageInfo);

		public List<Order> GetOrders(OrdersViewModel pageInfo) {
			IQueryable<Order> orders = this._context.Orders
				.OrderByDescending(order => order.TimeStarted)
				.AsQueryable();
			pageInfo.Total = orders.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.CreatedOrders = orders.Count(order => order.State == OrderState.Created);
			pageInfo.AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting);
			pageInfo.CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled);
			pageInfo.FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished);
			pageInfo.FailedOrders = orders.Count(orders => orders.State == OrderState.Failed);
			pageInfo.ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired);
			pageInfo.TotalInvestments = orders.Where(order => order.State == OrderState.Finished).Sum(order => order.FinalPrice);
			return orders.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}
		
		public List<Order> GetOrders(ApplicationUser user) =>
			_context.Orders.Where(order => order.UserId == user.Id)
			.OrderByDescending(order => order.TimeStarted)
			.ToList();

		public List<Order> GetOrders(ApplicationUser user, OrdersViewModel pageInfo) {
			IQueryable<Order> orders = this._context.Orders
				.Where(order => order.UserId == user.Id)
				.OrderByDescending(order => order.TimeStarted);
			pageInfo.Total = orders.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.CreatedOrders = orders.Count(order => order.State == OrderState.Created);
			pageInfo.AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting);
			pageInfo.CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled);
			pageInfo.FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished);
			pageInfo.FailedOrders = orders.Count(orders => orders.State == OrderState.Failed);
			pageInfo.ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired);
			pageInfo.TotalInvestments = orders.Where(order => order.State == OrderState.Finished).Sum(order => order.FinalPrice);
			return orders.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public IQueryable<Order> FinishedOrders() =>
			this._context.Orders.Where(order => order.State == OrderState.Finished)
				.OrderBy(order => order.TimeStarted);
		public IQueryable<Order> FinishedOrders(ApplicationUser user) =>
			this._context.Orders.Where(order => order.State == OrderState.Finished && order.UserId == user.Id)
				.OrderByDescending(order => order.TimeStarted);
		public List<Order> FinishedOrders(OrdersViewModel pageInfo) {
			IQueryable<Order> orders = FinishedOrders();
			pageInfo.Total = orders.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.CreatedOrders = orders.Count(order => order.State == OrderState.Created);
			pageInfo.AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting);
			pageInfo.CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled);
			pageInfo.FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished);
			pageInfo.FailedOrders = orders.Count(orders => orders.State == OrderState.Failed);
			pageInfo.ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired);
			pageInfo.TotalInvestments = orders.Where(order => order.State == OrderState.Finished).Sum(order => order.FinalPrice);
			return orders.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		private int Day => DateTime.Today.Day;
		private int Month => DateTime.Today.Month;
		private int Year => DateTime.Today.Year;

		public double DailyIncome() =>
			FinishedOrders().Where(order => order.TimeFinished.Year == Year && order.TimeFinished.Month == Month && order.TimeFinished.Day == Day)
				.Sum(order => order.FinalPrice);

		public double MonthlyIncome() =>
			FinishedOrders().Where(order => order.TimeFinished.Year == Year && order.TimeFinished.Month == Month)
				.Sum(order => order.FinalPrice);

		public double YearlyIncome() =>
			FinishedOrders().Where(order => order.TimeFinished.Year == Year)
				.Sum(order => order.FinalPrice);

		public double TotalIncome() =>
			FinishedOrders().Sum(order => order.FinalPrice);

		public List<Order> FromToday() =>
			FinishedOrders().Where(order => order.TimeFinished.Year == Year && order.TimeFinished.Month == Month && order.TimeFinished.Day == Day)
			.ToList();

		public List<Order> FromToday(OrdersViewModel pageInfo) {
			IQueryable<Order> orders = FinishedOrders()
				.Where(order => order.TimeFinished.Year == Year && order.TimeFinished.Month == Month && order.TimeFinished.Day == Day);
			pageInfo.Total = orders.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.CreatedOrders = orders.Count(order => order.State == OrderState.Created);
			pageInfo.AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting);
			pageInfo.CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled);
			pageInfo.FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished);
			pageInfo.FailedOrders = orders.Count(orders => orders.State == OrderState.Failed);
			pageInfo.ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired);
			pageInfo.TotalInvestments = orders.Where(order => order.State == OrderState.Finished).Sum(order => order.FinalPrice);
			return orders.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<Order> FromThisMonth() =>
			FinishedOrders().Where(order => order.TimeFinished.Year == Year && order.TimeFinished.Month == Month)
			.ToList();

		public List<Order> FromThisMonth(OrdersViewModel pageInfo) {
			IQueryable<Order> orders = FinishedOrders()
				.Where(order => order.TimeFinished.Year == Year && order.TimeFinished.Month == Month);
			pageInfo.Total = orders.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.CreatedOrders = orders.Count(order => order.State == OrderState.Created);
			pageInfo.AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting);
			pageInfo.CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled);
			pageInfo.FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished);
			pageInfo.FailedOrders = orders.Count(orders => orders.State == OrderState.Failed);
			pageInfo.ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired);
			pageInfo.TotalInvestments = orders.Where(order => order.State == OrderState.Finished).Sum(order => order.FinalPrice);
			return orders.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<Order> FromThisYear() =>
			FinishedOrders().Where(order => order.TimeFinished.Year == Year)
			.ToList();

		public List<Order> FromThisYear(OrdersViewModel pageInfo) {
			IQueryable<Order> orders = FinishedOrders()
				.Where(order => order.TimeFinished.Year == Year);
			pageInfo.Total = orders.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.CreatedOrders = orders.Count(order => order.State == OrderState.Created);
			pageInfo.AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting);
			pageInfo.CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled);
			pageInfo.FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished);
			pageInfo.FailedOrders = orders.Count(orders => orders.State == OrderState.Failed);
			pageInfo.ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired);
			pageInfo.TotalInvestments = orders.Where(order => order.State == OrderState.Finished).Sum(order => order.FinalPrice);
			return orders.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task RegisterOrder(Order order) {
			_context.Orders.Add(order);
			_context.Users.FirstOrDefault(user => user.Id == order.User.Id)
				.Orders.Add(order);
			await _context.SaveChangesAsync();
		}
		public async Task RemoveOrder(string id) =>
			await RemoveOrder(await Find(id));

		public async Task RemoveOrder(Order order) {
			OrderState[] allowedStates = new OrderState[] {
				OrderState.Cancelled,
				OrderState.Created,
			};
			if (!allowedStates.Contains(order.State))
				throw new ArgumentException();
			_context.PromoCodeOrders.RemoveRange(order.PromoCodes);
			order.User.Orders.Remove(order);
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateOrder(Order order) {
			Order target = await Find(order.Id);
			target.Plan = this._planService.GetPlans()
				.FirstOrDefault(plan => plan.Number == target.PlanNumber);
			order.OriginalPrice = target.Plan.Price * order.Amount;
			order.FinalPrice = this._codeService.GetFinalPrice(order.OriginalPrice, order.PromoCodes.ToList());
			target = order;
			await this._context.SaveChangesAsync();
		}

		public async Task UpdateState(string id, OrderState state) =>
			await UpdateState(await Find(id), state);

		public async Task UpdateState(Order order, OrderState state) {
			order.State = state;
			if (state == OrderState.Cancelled || state == OrderState.Finished || state == OrderState.Expired)
				order.TimeFinished = DateTime.Now;
			await _context.SaveChangesAsync();
		}

		public async Task AddVPS(string id, VPS vps) =>
			await AddVPS(await Find(id), vps);

		public async Task AddVPS(Order order, VPS vps) {
			order.VPSs.Add(vps);
			vps.Order = order;
			await _context.SaveChangesAsync();
		}

		public async Task RemoveVPS(string id, VPS vps) =>
			await RemoveVPS(await Find(id), vps);

		public async Task RemoveVPS(Order order, VPS vps) {
			order.VPSs.Remove(vps);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateVPSs(string id, List<VPS> vpss) =>
			await UpdateVPSs(await Find(id), vpss);

		public async Task UpdateVPSs(Order order, List<VPS> vpss) {
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
