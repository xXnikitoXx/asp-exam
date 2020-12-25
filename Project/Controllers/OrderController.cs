using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize]
	public class OrderController : Controller {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IOrderClient _service;
		private readonly IPlanClient _planService;
		private readonly IPromoCodeClient _codeService;
		private readonly IPaymentClient _paymentService;
		private readonly IVPSClient _vpsService;
		private readonly IMapper _mapper;

		public OrderController(
			UserManager<ApplicationUser> userManager,
			ApplicationDbContext context,
			IOrderClient service,
			IPlanClient planService,
			IPromoCodeClient codeService,
			IPaymentClient paymentService,
			IVPSClient vpsService,
			IMapper mapper
		) {
			this._userManager = userManager;
			this._context = context;
			this._service = service;
			this._planService = planService;
			this._codeService = codeService;
			this._paymentService = paymentService;
			this._vpsService = vpsService;
			this._mapper = mapper;
		}

		public async Task<IActionResult> Index(int ProductId = 0, bool New = false) {
			Order order = (await _service.GetOrders(User))
				.FirstOrDefault(order => order.State == OrderState.Created && ProductId == order.PlanNumber);
			if (order != null && !New) {
				ViewData.Add("Message", $"Заредена е ваша запазена поръчка за този продукт. Натиснете <a href=\"/order?productId={ProductId}&new=true\">ТУК</a> ако искате да започнете на чист шаблон.");
				order.Plan = this._planService
					.GetPlans()
					.FirstOrDefault(plan => plan.Number == order.PlanNumber);
			} else {
				Models.Plan target = this._planService.GetPlans().FirstOrDefault(plan => plan.Number == ProductId);
				if (order != null) 
					target.Orders.Remove(order);
				if (target == null)
					return Redirect("/404");
				order = OrderClient.OrderFromPlan(target);
				order.User = await this._userManager.GetUserAsync(User);
				target.Orders.Add(order);
				await this._context.SaveChangesAsync();
				if (New)
					ViewData.Add("Message", $"Поръчката Ви беше върната по подразбиране.");
			}
			OrderViewModel model = this._mapper.Map<OrderViewModel>(order);
			model.PromoCodes = this._codeService.GetPromoCodes(order)
				.Select(this._mapper.Map<PromoCodeViewModel>)
				.ToList();
			return View(model);
		}

		public async Task<IActionResult> Details(string OrderId, bool New = false) {
			Order order = (await _service.GetOrders(User))
				.FirstOrDefault(order => order.Id == OrderId);
			if (order == null)
				return Redirect("/404");
			if (New) {
				await this._service.RemoveOrder(order);
				Models.Plan plan = this._planService.GetPlans()
					.FirstOrDefault(plan => plan.Number == order.PlanNumber);
				order = OrderClient.OrderFromPlan(plan);
				order.Plan = plan;
				order.User = await this._userManager.GetUserAsync(User);
				plan.Orders.Add(order);
				await this._service.RegisterOrder(order);
				ViewData.Add("Message", $"Поръчката Ви беше върната по подразбиране.");
			}
			order.Plan = this._planService.GetPlans()
				.FirstOrDefault(plan => plan.Number == order.PlanNumber);
			if (order.State == OrderState.Created && !New)
				ViewData.Add("Message", $"Заредена е Ваша запазена поръчка за този продукт. Натиснете <a href=\"/order/details?orderId={order.Id}&new=true\">ТУК</a> ако искате да започнете на чист шаблон.");
			OrderViewModel model = this._mapper.Map<OrderViewModel>(order);
			model.PromoCodes = this._codeService.GetPromoCodes(order)
				.Select(this._mapper.Map<PromoCodeViewModel>)
				.ToList();
			if (model.State == OrderState.Finished) {
				model.Payment = this._mapper.Map<PaymentViewModel>(this._paymentService.GetPayment(order));
				model.Payment.AssociatedVPSs = (await this._vpsService.GetVPSs(User))
					.Where(vps => vps.OrderId == order.Id)
					.OrderBy(vps => vps.Name)
					.Select(this._mapper.Map<VPSViewModel>)
					.ToList();
			}
			return View("Index", model);
		}

		[HttpPatch]
		[Route("/Order")]
		[Route("/Order/Details")]
		public async Task<IActionResult> Update(OrderEditInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			Order order = null;
			if (model.OrderId == null) {
				order = (await _service.GetOrders(User))
					.FirstOrDefault(order => order.State == OrderState.Created && order.PlanNumber == model.ProductId);
				if (order == null || model.New) {
					Models.Plan target = this._planService.GetPlans().FirstOrDefault(plan => plan.Number == model.ProductId);
					order = OrderClient.OrderFromPlan(target);
					order.User = await this._userManager.GetUserAsync(User);
					order.User.Orders.Add(order);
					target.Orders.Add(order);
				}
			} else {
				order = (await this._service.GetOrders(User))
					.FirstOrDefault(order => order.Id == model.OrderId);
				if (order == null)
					return NotFound();
			}
			order.Amount = model.Amount;
			order.Location = (Location)Enum.Parse(typeof(Location), model.Location);
			await this._codeService.SetCodes(model.Codes, order);
			await this._service.UpdateOrder(order);
			return Ok();
		}

		public async Task<IActionResult> List(int Page = 1, int Show = 5) {
			List<Models.Plan> plans = this._planService.GetPlans();
			OrdersViewModel model = new OrdersViewModel {
				Page = Page,
				Show = Show,
			};
			model.Orders = (await this._service.GetOrders(User, model))
				.Select(this._mapper.Map<OrderViewModel>)
				.ToList();
			foreach (OrderViewModel order in model.Orders)
				order.Plan = _mapper.Map<PlanViewModel>(plans.FirstOrDefault(plan => plan.Number == order.PlanNumber));
			return View(model);
		}

		[HttpGet("/Order/Remove")]
		public async Task<IActionResult> RemovePage(string Id) {
			Order target = (await this._service.GetOrders(User))
				.FirstOrDefault(order => order.Id == Id);
			if (target == null)
				return Redirect("/404");
			target.Plan = this._planService.GetPlans()
				.FirstOrDefault(plan => plan.Number == target.PlanNumber);
			return View("Remove", this._mapper.Map<OrderViewModel>(target));
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(string Id) {
			Order target = (await this._service.GetOrders(User))
				.FirstOrDefault(order => order.Id == Id);
			if (target == null)
				return NotFound();
			await this._service.RemoveOrder(target);
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> SetCodes(string OrderId, List<string> codes) {
			ApplicationUser user = await this._userManager.GetUserAsync(User);
			Order target = this._service.GetOrders(user)
				.FirstOrDefault(order => order.Id == OrderId && order.State != OrderState.Finished);
			if (target == null)
				return NotFound();
			List<PromoCode> promoCodes = this._codeService.Codes.Where(code => codes.Contains(code.Code)).ToList();
			promoCodes.RemoveAll(code => target.PromoCodes.Any(orderCode => orderCode.PromoCode.Code == code.Code));
			promoCodes.RemoveAll(code => user.PromoCodes.Any(userCode => userCode.PromoCode.Code == code.Code));
			await this._codeService.SetCodes(promoCodes, target);
			return Json(this._codeService.GetDiscount(target.OriginalPrice, target.PromoCodes.ToList()));
		}

		[HttpGet("/Admin/Order")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> AdminIndex(string OrderId) {
			Order order = await this._service.Find(OrderId);
			OrderViewModel model = this._mapper.Map<OrderViewModel>(order);
			if (model == null)
				return Redirect("/404");
			model.Plan = this._mapper.Map<PlanViewModel>(this._planService.GetPlans()
				.FirstOrDefault(plan => plan.Number == model.PlanNumber));
			model.PromoCodes = this._codeService.GetPromoCodes(order)
				.Select(this._mapper.Map<PromoCodeViewModel>)
				.ToList();
			if (model.State == OrderState.Finished) {
				model.Payment = this._mapper.Map<PaymentViewModel>(this._paymentService.GetPayment(order));
				model.Payment.AssociatedVPSs = (await this._vpsService.GetVPSs(User))
					.Where(vps => vps.OrderId == order.Id)
					.OrderBy(vps => vps.Name)
					.Select(this._mapper.Map<VPSViewModel>)
					.ToList();
			}
			return View(model);
		}

		[HttpGet("/Admin/Orders")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> AdminList(int Page = 1, int show = 20, string From = "") {
			List<Models.Plan> plans = this._planService.GetPlans();
			OrdersViewModel model = new OrdersViewModel {
				Page = Page,
				Show = show,
			};
			switch (From.ToLower()) {
				case "today":
					model.Orders = this._service.FromToday(model)
						.Select(this._mapper.Map<OrderViewModel>)
						.ToList();
					break;
				case "thismonth":
					model.Orders = this._service.FromThisMonth(model)
						.Select(this._mapper.Map<OrderViewModel>)
						.ToList();
					break;
				case "thisyear":
					model.Orders = this._service.FromThisYear(model)
						.Select(this._mapper.Map<OrderViewModel>)
						.ToList();
					break;
				case "alltime":
					model.Orders = this._service.FinishedOrders(model)
						.Select(this._mapper.Map<OrderViewModel>)
						.ToList();
					break;
				default:
					model.Orders = this._service.GetOrders(model)
						.Select(this._mapper.Map<OrderViewModel>)
						.ToList();
					break;
			}
			
			foreach (OrderViewModel order in model.Orders) {
				ApplicationUser user = await this._userManager.FindByIdAsync(order.UserId);
				order.Username = user == null ? "{{DELETED}}" : user.UserName;
				order.Plan = _mapper.Map<PlanViewModel>(plans.FirstOrDefault(plan => plan.Number == order.PlanNumber));
			}
			return View(model);
		}
	}
}
