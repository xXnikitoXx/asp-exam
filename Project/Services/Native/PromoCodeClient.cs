using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Data;
using Project.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System;
using Project.Enums;
using Project.ViewModels;

namespace Project.Services.Native {
	public class PromoCodeClient : IPromoCodeClient {
		
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public PromoCodeClient(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager
		) {
			this._context = context;
			this._userManager = userManager;
		}

		public IQueryable<PromoCode> Codes => this._context.PromoCodes.AsQueryable();

		public double GetFinalPrice(double price, List<PromoCode> codes) {
			double totalFixedAmount = 0;
			double percentage = 0;
			foreach (PromoCode code in codes) {
				if (!code.IsValid)
					continue;
				switch(code.Type) {
					case PromoCodeType.Free: return 0;
					case PromoCodeType.PriceOverride: return code.Value;
					case PromoCodeType.Percentage: percentage += code.Value * .01; break;
					case PromoCodeType.FixedAmount: totalFixedAmount += code.Value; break;
				}
			}
			price = (price - totalFixedAmount) * (1 - percentage);
			return price < 0 ? 0 : price;
		}

		public double GetFinalPrice(double price, List<PromoCodeOrder> codes) =>
			GetFinalPrice(price, codes.Select(code => code.PromoCode).ToList());

		public double GetDiscount(double price, List<PromoCode> codes) =>
			price - GetFinalPrice(price, codes);

		public double GetDiscount(double price, List<PromoCodeOrder> codes) =>
			price - GetFinalPrice(price, codes);

		public PromoCode GetCodeById(string id) => this._context.PromoCodes.FirstOrDefault(code => code.Id == id);
		public PromoCode GetCodeByText(string text) => this._context.PromoCodes.FirstOrDefault(code => code.Code == text);

		public PromoCode GetCode(string search) {
			PromoCode target = GetCodeById(search);
			if (target == null)
				return GetCodeByText(search);
			return target;
		}

		public List<PromoCode> GetPromoCodes() => this._context.PromoCodes.ToList();
		public List<PromoCode> GetPromoCodes(PromoCodesViewModel pageInfo) {
			pageInfo.Total = this._context.PromoCodes.Count();
			pageInfo.Active = this._context.PromoCodes.Count(code => code.IsValid);
			pageInfo.Inactive = this._context.PromoCodes.Count(code => !code.IsValid);
			pageInfo.FixedAmount = this._context.PromoCodes.Count(code => code.Type == PromoCodeType.FixedAmount);
			pageInfo.Percentage = this._context.PromoCodes.Count(code => code.Type == PromoCodeType.Percentage);
			pageInfo.PriceOverride = this._context.PromoCodes.Count(code => code.Type == PromoCodeType.PriceOverride);
			pageInfo.Free = this._context.PromoCodes.Count(code => code.Type == PromoCodeType.Free);
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return this._context.PromoCodes.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<PromoCode> GetPromoCodes(Order order) => this._context.PromoCodes
			.Where(promoCode =>
				promoCode.Orders.Any(promoCodeOrder => promoCodeOrder.OrderId == order.Id))
			.ToList();

		public List<PromoCode> GetPromoCodes(Order order, PromoCodesViewModel pageInfo) {
			IQueryable<PromoCode> codes = this._context.PromoCodes.Where(promoCode =>
				promoCode.Orders.Any(promoCodeOrder => promoCodeOrder.OrderId == order.Id));
			pageInfo.Total = codes.Count();
			pageInfo.Active = codes.Count(code => code.IsValid);
			pageInfo.Inactive = codes.Count(code => !code.IsValid);
			pageInfo.FixedAmount = codes.Count(code => code.Type == PromoCodeType.FixedAmount);
			pageInfo.Percentage = codes.Count(code => code.Type == PromoCodeType.Percentage);
			pageInfo.PriceOverride = codes.Count(code => code.Type == PromoCodeType.PriceOverride);
			pageInfo.Free = codes.Count(code => code.Type == PromoCodeType.Free);
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return codes.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<PromoCode> GetPromoCodes(ApplicationUser user) => this._context.PromoCodes
			.Where(promoCode =>
				promoCode.Users.Any(userPromoCode => userPromoCode.UserId == user.Id))
			.ToList();

		public List<PromoCode> GetPromoCodes(ApplicationUser user, PromoCodesViewModel pageInfo) {
			IQueryable<PromoCode> codes = this._context.PromoCodes.Where(promoCode =>
				promoCode.Users.Any(promoCodeUser => promoCodeUser.UserId == user.Id));
			pageInfo.Total = codes.Count();
			pageInfo.Active = codes.Count(code => code.IsValid);
			pageInfo.Inactive = codes.Count(code => !code.IsValid);
			pageInfo.FixedAmount = codes.Count(code => code.Type == PromoCodeType.FixedAmount);
			pageInfo.Percentage = codes.Count(code => code.Type == PromoCodeType.Percentage);
			pageInfo.PriceOverride = codes.Count(code => code.Type == PromoCodeType.PriceOverride);
			pageInfo.Free = codes.Count(code => code.Type == PromoCodeType.Free);
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return codes.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task<List<PromoCode>> GetPromoCodes(ClaimsPrincipal user) => GetPromoCodes(await _userManager.GetUserAsync(user));
		public async Task<List<PromoCode>> GetPromoCodes(ClaimsPrincipal user, PromoCodesViewModel pageInfo) => GetPromoCodes(await _userManager.GetUserAsync(user), pageInfo);

		public async Task SetCodes(List<PromoCode> codes, Order order) {
			List<PromoCodeOrder> promoCodeOrders = this._context.PromoCodeOrders
				.Where(promoCodeOrder => promoCodeOrder.OrderId == order.Id)
				.ToList();
			this._context.PromoCodeOrders.RemoveRange(promoCodeOrders);
			await this._context.SaveChangesAsync();
			promoCodeOrders = new List<PromoCodeOrder>();
			foreach (PromoCode code in codes) {
				PromoCodeOrder promoCodeOrder = new PromoCodeOrder {
					Order = order,
					PromoCode = code,
				};
				order.PromoCodes.Add(promoCodeOrder);
				code.Orders.Add(promoCodeOrder);
				promoCodeOrders.Add(promoCodeOrder);
			}
			await this._context.PromoCodeOrders.AddRangeAsync(promoCodeOrders);
			order.FinalPrice = GetDiscount(order.OriginalPrice, order.PromoCodes.ToList());
			await this._context.SaveChangesAsync();
		}

		public async Task SetCodes(List<string> codes, Order order) {
			string id = order.Id;
			List<PromoCode> promoCodes = this._context.PromoCodes.Where(code => codes.Contains(code.Code)).ToList();
			await SetCodes(promoCodes, order);
		}

		private static Random random = new Random();
		public static string RandomString(int length) => new string(
			Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
				.Select(s => s[random.Next(s.Length)]).ToArray()
		);

		public async Task<PromoCode> CreateCode() {
			PromoCode code = new PromoCode {
				Code = RandomString(5),
				Value = 1,
				Type = PromoCodeType.FixedAmount,
			};
			await CreateCode(code);
			return code;
		}

		public async Task CreateCode(PromoCode code) {
			if (this._context.PromoCodes.Any(promoCode => promoCode.Code == code.Code))
				throw new ArgumentException();
			this._context.PromoCodes.Add(code);
			await this._context.SaveChangesAsync();
		}

		public async Task<bool> SwitchCode(string id) {
			PromoCode code = this._context.PromoCodes
				.FirstOrDefault(code => code.Id == id);
			if (code == null)
				throw new Exception();
			code.IsValid = !code.IsValid;
			await this._context.SaveChangesAsync();
			return code.IsValid;
		}

		public async Task UpdateCode(PromoCode code) {
			PromoCode target = this._context.PromoCodes
				.FirstOrDefault(promoCode => promoCode.Id == code.Id);
			if (target == null)
				throw new Exception();
			target.Code = code.Code;
			target.Type = code.Type;
			target.Value = code.Value;
			target.IsValid = code.IsValid;
			await this._context.SaveChangesAsync();
		}

		public async Task RemoveCode(PromoCode code) {
			PromoCode target = this._context.PromoCodes
				.FirstOrDefault(promoCode => promoCode.Id == code.Id);
			if (target == null)
				throw new Exception();
			this._context.PromoCodeOrders.RemoveRange(code.Orders);
			this._context.UserPromoCodes.RemoveRange(code.Users);
			this._context.PromoCodes.Remove(code);
			await this._context.SaveChangesAsync();
		}
	}
}