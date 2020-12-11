using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Services.Native {
	public interface IPromoCodeClient {
		PromoCode GetCodeById(string id);
		PromoCode GetCodeByText(string text);
		PromoCode GetCode(string search);
		List<PromoCode> GetPromoCodes();
		List<PromoCode> GetPromoCodes(Order order);
		List<PromoCode> GetPromoCodes(ApplicationUser user);
		Task<List<PromoCode>> GetPromoCodes(ClaimsPrincipal user);
		Task SetCodes(List<string> codes, Order order);
		Task<PromoCode> CreateCode();
		Task CreateCode(PromoCode code);
		Task<bool> SwitchCode(string id);
		Task UpdateCode(PromoCode code);
		Task RemoveCode(PromoCode code);
	}
}