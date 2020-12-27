using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IServerDataClient _serverDataService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public VPSController(
			IVPSClient service,
			IPlanClient planService,
			IOrderClient orderService,
			IServerClient serverService,
			IServerDataClient serverDataService,
			UserManager<ApplicationUser> userManager,
			IMapper mapper
		) {
			this._service = service;
			this._planService = planService;
			this._orderService = orderService;
			this._hetznerService = serverService;
			this._serverDataService = serverDataService;
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
				ServerData data = await this._serverDataService.Find(vps.ServerDataId);
				Plan plan = plans.FirstOrDefault(plan => plan.Number == order.PlanNumber);
				vps.Order = this._mapper.Map<OrderViewModel>(order);
				vps.ServerData = this._mapper.Map<ServerDataViewModel>(data);
				vps.Plan = this._mapper.Map<PlanViewModel>(plan);
			}
			return View(model);
		}

		public async Task<IActionResult> Manage(string Id) {
			VPS target = (await this._service.GetVPSs(User))
				.FirstOrDefault(vps => vps.Id == Id);
			if (target == null)
				return Redirect("/404");
			if (target.ServerDataId == null)
				return Redirect("/VPS/Setup?Id=" + Id);
			VPSViewModel model = this._mapper.Map<VPSViewModel>(target);
			model.Order = this._mapper.Map<OrderViewModel>(await this._orderService.Find(model.OrderId));
			model.ServerData = this._mapper.Map<ServerDataViewModel>(await this._serverDataService.Find(model.ServerDataId));
			model.Plan = this._mapper.Map<PlanViewModel>(await this._planService.Find(model.Order.PlanNumber));
			return View(model);
		}

		public async Task<IActionResult> Setup(string Id) {
			VPS target = (await this._service.GetVPSs(User))
				.FirstOrDefault(vps => vps.Id == Id);
			if (target == null)
				return Redirect("/404");
			VPSViewModel model = this._mapper.Map<VPSViewModel>(target);
			if (model.ServerDataId != null)
				model.ServerData = this._mapper.Map<ServerDataViewModel>(await this._serverDataService.Find(model.ServerDataId));
			return View(model);
		}

		[HttpPost]
		[Route("/VPS/Setup")]
		[Route("/Admin/VPS/Setup")]
		public async Task<IActionResult> Setup(VPSSetupInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			KeyValuePair<string, Server> server = new KeyValuePair<string, Server>(null, null);
			ApplicationUser user = await this._userManager.GetUserAsync(User);
			bool isAdmin = await this._userManager.IsInRoleAsync(user, "Administrator");
			VPS target =
				Request.Path.ToString().ToLower().Contains("Admin") && isAdmin ?
				await this._service.Find(model.Id) :
				(await this._service.GetVPSs(User))
					.FirstOrDefault(server => server.Id == model.Id);
			target.Name = model.Name;
			if (target == null)
				return NotFound();
			Image image = await this._hetznerService.FromInput(model.Distro, model.Version);
			try {
				server = await this._hetznerService.SetupServer(target, image);
			} catch (Exception e) {
				Console.WriteLine(e);
				return BadRequest();
			}
			if (server.Key == null || server.Value == null)
				return BadRequest();
			await this._service.UpdateVPS(target);
			await this._serverDataService.CreateForVPS(target, server.Value, image);
			return Json(new string[] { server.Key, target.ServerData.IPv4 });
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
				vps.ServerData = this._mapper.Map<ServerDataViewModel>(await this._serverDataService.Find(vps.ServerDataId));
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
			if (target.ServerDataId == null)
				return Redirect("/Admin/VPS/Setup?Id=" + Id);
			VPSViewModel model = this._mapper.Map<VPSViewModel>(target);
			ApplicationUser user = await this._userManager.FindByIdAsync(model.UserId);
			model.Username = user == null ? "{{DELETED}}" : user.UserName;
			model.Order = this._mapper.Map<OrderViewModel>(await this._orderService.Find(model.OrderId));
			model.ServerData = this._mapper.Map<ServerDataViewModel>(await this._serverDataService.Find(model.ServerDataId));
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
			if (model.ServerDataId != null)
				model.ServerData = this._mapper.Map<ServerDataViewModel>(await this._serverDataService.Find(model.ServerDataId));
			ApplicationUser user = await this._userManager.FindByIdAsync(model.UserId);
			model.Username = user == null ? "{{DELETED}}" : user.UserName;
			ViewData["Admin"] = true;
			return View("Setup", model);
		}

		[HttpPost]
		[Route("/VPS/Control")]
		[Route("/Admin/VPS/Control")]
		public async Task<IActionResult> Control(string Id, string Action) {
			ApplicationUser user = await this._userManager.GetUserAsync(User);
			bool isAdmin = await this._userManager.IsInRoleAsync(user, "Administrator");
			VPS target =
				Request.Path.ToString().ToLower().Contains("Admin") && isAdmin ?
				await this._service.Find(Id) :
				(await this._service.GetVPSs(User))
					.FirstOrDefault(server => server.Id == Id);
			if (target == null)
				return Redirect("/404");
			ServerData data = await this._serverDataService.Find(target.ServerDataId);
			switch (Action.ToLower()) {
				case "poweron":
					await this._hetznerService.PowerOn(data.ExternalId);
					break;
				case "reboot":
					await this._hetznerService.Reboot(data.ExternalId);
					break;
				case "reset":
					await this._hetznerService.Reset(data.ExternalId);
					break;
				case "shutdown":
					await this._hetznerService.Shutdown(data.ExternalId);
					break;
				case "poweroff":
					await this._hetznerService.PowerOff(data.ExternalId);
					break;
			}
			return Ok();
		}
	}
}
