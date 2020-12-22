using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize]
	public class VPSController : Controller {
		private readonly IVPSClient _service;
		private readonly IPlanClient _planService;
		private readonly IOrderClient _orderService;
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public VPSController(
			IVPSClient service,
			IPlanClient planService,
			IOrderClient orderService,
			ApplicationDbContext context,
			IMapper mapper
		) {
			this._service = service;
			this._planService = planService;
			this._orderService = orderService;
			this._context = context;
			this._mapper = mapper;
		}


		public async Task<IActionResult> Index(int Page = 1, int Show = 10) {
			List<Plan> plans = this._planService.GetPlans();
			VPSsViewModel model = new VPSsViewModel {
				Page = Page,
				Show = Show,
			};
			model.VPSs = (await this._service.GetVPSs(User, model))
				.Select(this._mapper.Map<VPSViewModel>)
				.ToList();
			foreach (VPSViewModel vps in model.VPSs) {
				Order order = await this._orderService.Find(vps.OrderId);
				Plan plan = plans.FirstOrDefault(plan => plan.Number == order.PlanNumber);
				vps.Order = this._mapper.Map<OrderViewModel>(order);
				vps.Plan = this._mapper.Map<PlanViewModel>(plan);
			}
			return View(model);
		}

		public IActionResult Manage(string id) {
			return View();
		}
	}
}
