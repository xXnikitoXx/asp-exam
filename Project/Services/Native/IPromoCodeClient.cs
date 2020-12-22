using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public interface IPromoCodeClient {
		IQueryable<PromoCode> Codes { get; }
		double GetFinalPrice(double price, List<PromoCode> codes);
		double GetFinalPrice(double price, List<PromoCodeOrder> codes);
		double GetDiscount(double price, List<PromoCode> codes);
		double GetDiscount(double price, List<PromoCodeOrder> codes);
		PromoCode GetCodeById(string id);
		PromoCode GetCodeByText(string text);
		PromoCode GetCode(string search);
		List<PromoCode> GetPromoCodes();
		List<PromoCode> GetPromoCodes(PromoCodesViewModel pageInfo);
		List<PromoCode> GetPromoCodes(Order order);
		List<PromoCode> GetPromoCodes(ApplicationUser user);
		List<PromoCode> GetPromoCodes(ApplicationUser user, PromoCodesViewModel pageInfo);
		Task<List<PromoCode>> GetPromoCodes(ClaimsPrincipal user);
		Task<List<PromoCode>> GetPromoCodes(ClaimsPrincipal user, PromoCodesViewModel pageInfo);
		Task SetCodes(List<PromoCode> codes, Order order);
		Task SetCodes(List<string> codes, Order order);
		Task<PromoCode> CreateCode();
		Task CreateCode(PromoCode code);
		Task<bool> SwitchCode(string id);
		Task UpdateCode(PromoCode code);
		Task RemoveCode(PromoCode code);
	}
}