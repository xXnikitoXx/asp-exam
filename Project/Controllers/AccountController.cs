using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize]
	public class AccountController : Controller {
		private readonly IAccountClient _service;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IRoleClient _roleService;
		private readonly IOrderClient _orderService;
		private readonly IPlanClient _planService;
		private readonly IVPSClient _vpsService;
		private readonly IMapper _mapper;

		public AccountController(
			IAccountClient service,
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			IRoleClient roleService,
			IOrderClient orderService,
			IPlanClient planService,
			IVPSClient vpsService,
			IMapper mapper
		) {
			this._service = service;
			this._signInManager = signInManager;
			this._userManager = userManager;
			this._roleService = roleService;
			this._orderService = orderService;
			this._planService = planService;
			this._vpsService = vpsService;
			this._mapper = mapper;
		}

		[HttpGet("/Account")]
		public async Task<IActionResult> Index() {
			List<VPS> vpss = await _vpsService.GetVPSs(User);
			List<Order> orders = await _orderService.GetOrders(User);
			List<Models.Plan> plans = this._planService.GetPlans();

			AccountViewModel model = new AccountViewModel {
				VPSCount = vpss.Count,
				OrdersCount = orders.Count,
				Orders = orders.OrderByDescending(order => order.TimeFinished)
					.ThenByDescending(orders => orders.TimeStarted)
					.Take(10)
					.Select(_mapper.Map<OrderViewModel>)
					.ToList(),
				CreatedOrders = orders.Count(order => order.State == OrderState.Created),
				AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting),
				CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled),
				FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished),
				FailedOrders = orders.Count(orders => orders.State == OrderState.Failed),
				ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired),
				TotalInvestments = orders.Where(orders => orders.State == OrderState.Finished)
					.Sum(order => order.FinalPrice),
				MonthlyBill = orders.Where(order => order.State == OrderState.Finished)
					.Sum(order => order.OriginalPrice),
			};

			foreach (OrderViewModel order in model.Orders)
				order.Plan = _mapper.Map<PlanViewModel>(plans.FirstOrDefault(plan => plan.Number == order.PlanNumber));
			return this.View(model);
		}

		[HttpGet("/Profile")]
		public IActionResult Profile() => this.Redirect("/Account");

		[HttpGet("/Panel")]
		public IActionResult Panel() {
			return this.View();
		}

		[AllowAnonymous]
		public IActionResult Settings() => this.Redirect("/Identity/Account");

		[AllowAnonymous]
		[HttpGet("/Login")]
		public IActionResult Login() => this.Redirect("/Identity/Account/Login");

		[AllowAnonymous]
		[HttpGet("/Register")]
		public IActionResult Register() => this.Redirect("/Identity/Account/Register");

		[HttpGet("/Logout")]
		public async Task<IActionResult> Logout() {
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet("/Admin/Users")]
		[Authorize(Roles = "Administrator")]
		public IActionResult AdminList(int Page = 1, int Show = 20) {
			UsersViewModel model = new UsersViewModel {
				Page = Page,
				Show = Show,
			};
			model.Users = this._roleService.GetUsers(model)
				.Select(this._mapper.Map<UserViewModel>)
				.Select(user => {
					user.IsAdmin = Task.Run<bool>(async () => await this._roleService.IsAdmin(user.Id))
						.GetAwaiter()
						.GetResult();
					return user;
				})
				.ToList();
			return View(model);
		}

		[HttpPost("/Admin/Users/Switch")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Switch(string Id) {
			bool isAdmin;
			try {
				isAdmin = await this._roleService.IsAdmin(Id);
				if (isAdmin)
					await this._roleService.DemoteToUser(Id);
				else
					await this._roleService.PromoteToAdmin(Id);
			} catch (Exception) {
				return NotFound();
			}
			return Json(!isAdmin);
		}

		[HttpGet("/Admin/User/Remove")]
		[Authorize(Roles = "Administrator")]
		public IActionResult RemovePage(string Id) {
			ApplicationUser user = this._roleService.Find(Id);
			if (user == null)
				return Redirect("/404");
			return View("Remove", this._mapper.Map<UserViewModel>(user));
		}

		[HttpDelete("/Admin/User/Remove")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Remove(string Id) {
			try {
				await this._service.RemoveUser(Id);
			} catch (Exception) {
				return BadRequest();
			}
			return Ok();
		}
	}
}