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
		private readonly IMapper _mapper;

		public OrderController(
			UserManager<ApplicationUser> userManager,
			ApplicationDbContext context,
			IOrderClient service,
			IPlanClient planService,
			IPromoCodeClient codeService,
			IMapper mapper
		) {
			this._userManager = userManager;
			this._context = context;
			this._service = service;
			this._planService = planService;
			this._codeService = codeService;
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
				return this.View(this._mapper.Map<OrderViewModel>(order));
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
			return this.View(this._mapper.Map<OrderViewModel>(order));
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
			return View("Index", this._mapper.Map<OrderViewModel>(order));
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
			List<Order> orders = await this._service.GetOrders(User);
			List<Models.Plan> plans = this._planService.GetPlans();
			int Total = orders.Count;
			int Pages = (Total / Show) + (Total % Show != 0 ? 1 : 0);
			double TotalInvestments = orders
				.Where(order => order.State == OrderState.Finished)
				.Sum(order => order.FinalPrice);

			OrdersViewModel model = new OrdersViewModel {
				Orders = orders
					.Skip(Show * (Page - 1))
					.Take(Show)
					.Select(this._mapper.Map<OrderViewModel>)
					.ToList(),
				Total = Total,
				Page = Page,
				Pages = Pages,
				Show = Show,
				CreatedOrders = orders.Count(order => order.State == OrderState.Created),
				AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting),
				CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled),
				FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished),
				FailedOrders = orders.Count(orders => orders.State == OrderState.Failed),
				ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired),
				TotalInvestments = TotalInvestments
			};

			foreach (OrderViewModel order in model.Orders)
				order.Plan = _mapper.Map<PlanViewModel>(plans.FirstOrDefault(plan => plan.Number == order.PlanNumber));
			return View(model);
		}

		[HttpGet("/order/remove")]
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
	}
}
