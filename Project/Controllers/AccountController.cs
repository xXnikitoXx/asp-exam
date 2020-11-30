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

namespace Project.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IOrderClient _orderService;
		private readonly IVPSClient _vpsService;
		private readonly IMapper _mapper;

		public AccountController(SignInManager<ApplicationUser> signInManager,
			IOrderClient orderService,
			IVPSClient vpsService,
			IMapper mapper)
		{
			this._signInManager = signInManager;
			this._orderService = orderService;
			this._vpsService = vpsService;
			this._mapper = mapper;
		}

		[HttpGet("/Account")]
		[Authorize]
		public async Task<IActionResult> Index()
		{
			List<VPS> vpss = await _vpsService.GetVPSs(User);
			List<Order> orders = await _orderService.GetOrders(User);

			AccountViewModel model = new AccountViewModel
			{
				VPSCount = vpss.Count,
				OrdersCount = orders.Count,
				CreatedOrders = orders.Count(order => order.State == OrderState.Created),
				AwaitingOrders = orders.Count(orders => orders.State == OrderState.Awaiting),
				CancelledOrders = orders.Count(orders => orders.State == OrderState.Cancelled),
				FinishedOrders = orders.Count(orders => orders.State == OrderState.Finished),
				FailedOrders = orders.Count(orders => orders.State == OrderState.Failed),
				ExpiredOrders = orders.Count(orders => orders.State == OrderState.Expired),
				TotalInvestments = orders.Sum(order => order.FinalPrice),
				MonthlyBill = orders.Where(order => order.State == OrderState.Finished)
					.Sum(order => order.OriginalPrice),
			};
			return this.View(model);
		}

		[HttpGet("/Profile")]
		[Authorize]
		public IActionResult Profile() => this.Redirect("/Account");

		[HttpGet("/Panel")]
		[Authorize]
		public IActionResult Panel() {
			return this.View();
		}

		public IActionResult Settings() => this.Redirect("/Identity/Account");

		[HttpGet("/Login")]
		public IActionResult Login() => this.Redirect("/Identity/Account/Login");

		[HttpGet("/Register")]
		public IActionResult Register() => this.Redirect("/Identity/Account/Register");

		[HttpGet("/Logout")]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
