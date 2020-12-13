using Project.Enums;
using Project.Models;
using Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native
{
	public interface IOrderClient
	{
		Task<Order> Find(string id);
		Task<List<Order>> GetOrders(ClaimsPrincipal user);
		Task<List<Order>> GetOrders(ClaimsPrincipal user, OrdersViewModel pageInfo);
		List<Order> GetOrders(OrdersViewModel pageInfo);
		List<Order> GetOrders(ApplicationUser user);
		List<Order> GetOrders(ApplicationUser user, OrdersViewModel pageInfo);
		IQueryable<Order> FinishedOrders();
		List<Order> FinishedOrders(OrdersViewModel pageInfo);
		double DailyIncome();
		double MonthlyIncome();
		double YearlyIncome();
		double TotalIncome();
		List<Order> FromToday();
		List<Order> FromToday(OrdersViewModel pageInfo);
		List<Order> FromThisMonth();
		List<Order> FromThisMonth(OrdersViewModel pageInfo);
		List<Order> FromThisYear();
		List<Order> FromThisYear(OrdersViewModel pageInfo);
		Task RegisterOrder(Order order);
		Task UpdateOrder(Order order);
		Task AddVPS(string id, VPS vps);
		Task AddVPS(Order order, VPS vps);
		Task RemoveVPS(string id, VPS vps);
		Task RemoveVPS(Order order, VPS vps);
		Task UpdateVPSs(string id, List<VPS> vpss);
		Task UpdateVPSs(Order order, List<VPS> vpss);
		Task UpdateState(string id, OrderState state);
		Task UpdateState(Order order, OrderState state);
		Task RemoveOrder(string id);
		Task RemoveOrder(Order order);
	}
}
