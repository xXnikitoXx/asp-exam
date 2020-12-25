using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Project.Services.PayPal;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize]
	public class PaymentController : Controller {
		private readonly PayPalPaymentClient _service;
		private readonly IOrderClient _orderService;
		private readonly IPlanClient _planService;
		private readonly IPaymentClient _paymentService;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly string host;

		public PaymentController(
			PayPalPaymentClient service,
			IOrderClient orderService,
			IPlanClient planService,
			IPaymentClient paymentService,
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			IMapper mapper
		) {
			this._service = service;
			this._orderService = orderService;
			this._planService = planService;
			this._paymentService = paymentService;
			this._context = context;
			this._userManager = userManager;
			this._mapper = mapper;
			this.host = Environment.GetEnvironmentVariable("PROXY_HOST");
		}

		[HttpGet("/Payment/Create")]
		public async Task<IActionResult> CreatePayment(string OrderId) {
			Models.Order target = (await this._orderService.GetOrders(User))
				.FirstOrDefault(order => order.Id == OrderId && order.State != OrderState.Finished);
			if (target == null)
				return Redirect("/404");
			target.Plan = this._planService.GetPlans()
				.FirstOrDefault(plan => plan.Number == target.PlanNumber);
			PayPal.Api.Payment payment = this._service.CreatePayment(target, this.host, "sale");
			return Redirect(payment.GetApprovalUrl());
		}

		[HttpGet("/Payment/Successful")]
		public async Task<IActionResult> Successful(PaymentInputModel inputModel) {
			Models.Order order = (await this._orderService.GetOrders(User))
				.FirstOrDefault(order => order.Id == inputModel.OrderId && order.State != OrderState.Finished);
			if (order == null)
				return Redirect("/404");
			order.Plan = this._planService.GetPlans()
				.FirstOrDefault(plan => plan.Number == order.PlanNumber);
			ApplicationUser user = await this._userManager.GetUserAsync(User);
			PayPal.Api.Payment payPalPayment = this._service.ExecutePayment(inputModel.PaymentId, inputModel.PayerId);
			Models.Payment payment = this._mapper.Map<Models.Payment>(inputModel);
			payment.UserId = user.Id;
			payment.OrderId = order.Id;
			List<VPSViewModel> vpss = (await this._paymentService.CreatePayment(payment))
				.Select(this._mapper.Map<VPSViewModel>)
				.OrderBy(vps => vps.Name)
				.ToList(); 
			PaymentViewModel viewModel = this._mapper.Map<PaymentViewModel>(payment);
			viewModel.AssociatedVPSs = vpss;
			return View(viewModel);
		}
	}
}