using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.Services.Hetzner;
using Project.ViewModels;
using System;
using lkcode.hetznercloudapi.Api;
using Microsoft.AspNetCore.Identity;

namespace Project.Controllers {
	[Authorize]
	public class VPSController : Controller {
		private readonly IVPSClient _service;
		private readonly IPlanClient _planService;
		private readonly IOrderClient _orderService;
		private readonly IServerClient _hetznerService;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public VPSController(
			IVPSClient service,
			IPlanClient planService,
			IOrderClient orderService,
			IServerClient hetznerService,
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			IMapper mapper
		) {
			this._service = service;
			this._planService = planService;
			this._orderService = orderService;
			this._hetznerService = hetznerService;
			this._context = context;
			this._userManager = userManager;
			this._mapper = mapper;
		}

		[HttpGet("/VPS")]
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

		public async Task<IActionResult> Manage(string Id) {
			VPS target = (await this._service.GetVPSs(User))
				.FirstOrDefault(vps => vps.Id == Id);
			if (target == null)
				return Redirect("/404");
			if (target.ExternalId == null)
				return Redirect("/VPS/Setup?Id=" + Id);
			VPSViewModel model = this._mapper.Map<VPSViewModel>(target);
			model.Order = this._mapper.Map<OrderViewModel>(await this._orderService.Find(model.OrderId));
			model.Plan = this._mapper.Map<PlanViewModel>(await this._planService.Find(model.Order.PlanNumber));
			return View(model);
		}

		public async Task<IActionResult> Setup(string Id) {
			VPS target = (await this._service.GetVPSs(User))
				.FirstOrDefault(vps => vps.Id == Id);
			if (target == null)
				return Redirect("/404");
			return View(this._mapper.Map<VPSViewModel>(target));
		}

		[HttpPost]
		[Route("/VPS/Setup")]
		[Route("/Admin/VPS/Setup")]
		public async Task<IActionResult> Setup(VPSSetupInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			KeyValuePair<string, Server> server = new KeyValuePair<string, Server>(null, null);
			VPS target = (await this._service.GetVPSs(User))
				.FirstOrDefault(server => server.Id == model.Id);
			target.Name = model.Name;
			if (target == null)
				return NotFound();
			try {
				Image image = await this._hetznerService.FromInput(model.Distro, model.Version);
				server = await this._hetznerService.SetupServer(target, image);
			} catch (Exception e) {
				Console.WriteLine(e);
			}
			if (server.Key == null || server.Value == null)
				return BadRequest();
			target.IP = server.Value.Network.Ipv4.ToString();
			target.ExternalId = server.Value.Id.ToString();
			await this._service.UpdateVPS(target);
			return Json(new string[] { server.Key, target.IP });
		}

		[HttpGet("/Admin/VPSs")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> AdminList(int Page = 1, int Show = 10) {
			VPSsViewModel model = new VPSsViewModel {
				Page = Page,
				Show = Show
			};
			model.VPSs = this._service.GetVPSs(model)
				.Select(this._mapper.Map<VPSViewModel>)
				.ToList();
			foreach (VPSViewModel vps in model.VPSs) {
				ApplicationUser user = await this._userManager.FindByIdAsync(vps.UserId);
				vps.Username = user == null ? "{{DELETED}}" : user.UserName;
				vps.Order = this._mapper.Map<OrderViewModel>(await this._orderService.Find(vps.OrderId));
				vps.Plan = this._mapper.Map<PlanViewModel>(await this._planService.Find(vps.Order.PlanNumber));
			}
			return View(model);
		}

		[HttpGet("/Admin/VPS/Manage")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> AdminManage(string Id) {
			VPS target = await this._service.Find(Id);
			if (target == null)
				return Redirect("/404");
			if (target.ExternalId == null)
				return Redirect("/Admin/VPS/Setup?Id=" + Id);
			VPSViewModel model = this._mapper.Map<VPSViewModel>(target);
			ApplicationUser user = await this._userManager.FindByIdAsync(model.UserId);
			model.Username = user == null ? "{{DELETED}}" : user.UserName;
			model.Order = this._mapper.Map<OrderViewModel>(await this._orderService.Find(model.OrderId));
			model.Plan = this._mapper.Map<PlanViewModel>(await this._planService.Find(model.Order.PlanNumber));
			ViewData["Admin"] = true;
			return View("Manage", model);
		}

		[HttpGet("/Admin/VPS/Setup")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> AdminSetup(string Id) {
			VPS target = await this._service.Find(Id);
			if (target == null)
				return Redirect("/404");
			VPSViewModel model = this._mapper.Map<VPSViewModel>(target);
			ApplicationUser user = await this._userManager.FindByIdAsync(model.UserId);
			model.Username = user == null ? "{{DELETED}}" : user.UserName;
			ViewData["Admin"] = true;
			return View("Setup", model);
		}
	}
}
