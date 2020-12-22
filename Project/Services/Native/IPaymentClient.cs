using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public interface IPaymentClient {
		Task<Payment> Find(string id);
		Payment GetPayment(string orderId);
		Payment GetPayment(Order order);
		List<Payment> GetPayments(ApplicationUser user);
		Task<List<Payment>> GetPayments(ClaimsPrincipal user);
		Task<List<VPS>> CreatePayment(Payment payment);
	}
}