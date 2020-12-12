using Project.Enums;
using Project.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native
{
	public interface IOrderClient
	{
		Task<List<Order>> GetOrders(ClaimsPrincipal user);
		List<Order> GetOrders(ApplicationUser user);
		double DailyIncome();
		double MonthlyIncome();
		double YearlyIncome();
		double TotalIncome();
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
