using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native {
	public class VPSClient : IVPSClient {
		private readonly ApplicationDbContext _context;
		private readonly IOrderClient _orderService;
		private readonly UserManager<ApplicationUser> _userManager;

		public VPSClient(
			ApplicationDbContext context,
			IOrderClient orderService,
			UserManager<ApplicationUser> userManager
		) {
			this._context = context;
			this._orderService = orderService;
			this._userManager = userManager;
		}

		public async Task<VPS> Find(string id) =>
			await _context.VPSs.FirstOrDefaultAsync(vps => vps.Id == id);

		public List<VPS> GetVPSs(VPSsViewModel pageInfo) {
			IQueryable<VPS> vpss = this._context.VPSs.OrderBy(vps => vps.Name);
			pageInfo.Total = vpss.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return vpss.Skip(pageInfo.Show * (pageInfo.Page - 1))
				.Take(pageInfo.Show)
				.ToList();
		}

		public async Task<List<VPS>> GetVPSs(ClaimsPrincipal user) =>
			GetVPSs(await _userManager.GetUserAsync(user));

		public List<VPS> GetVPSs(ApplicationUser user) =>
			_context.VPSs.Where(vps => vps.UserId == user.Id)
				.OrderBy(vps => vps.Name)
				.ToList();

		public async Task<List<VPS>> GetVPSs(ClaimsPrincipal user, VPSsViewModel pageInfo) =>
			GetVPSs(await this._userManager.GetUserAsync(user), pageInfo);


		public List<VPS> GetVPSs(ApplicationUser user, VPSsViewModel pageInfo) {
			IQueryable<VPS> vpss = this._context.VPSs.Where(vps => vps.UserId == user.Id)
				.OrderBy(vps => vps.Name);
			pageInfo.Total = vpss.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return vpss.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task RegisterVPS(VPS vps) {
			this._context.VPSs.Add(vps);
			(await this._userManager.FindByIdAsync(vps.UserId)).VPSs.Add(vps);
			await _context.SaveChangesAsync();
		}

		public async Task<VPS> RegisterVPSFor(Order order, string name = "Нов VPS") {
			VPS vps = new VPS {
				Name = name,
				Location = order.Location,
				IP = "0.0.0.0",
				Cores = order.Plan.Cores,
				RAM = order.Plan.RAM,
				SSD = order.Plan.SSD,
				UserId = order.UserId,
				User = order.User,
			};
			await RegisterVPS(vps);
			await this._orderService.AddVPS(order, vps);
			return vps;
		}

		public async Task<VPS> RegisterVPSFor(Order order, int index) =>
			await RegisterVPSFor(order, $"{order.Plan.Name} {index + 1}");

		public async Task RemoveVPS(string id) =>
			await RemoveVPS(await Find(id));

		public async Task RemoveVPS(VPS vps) {
			vps.User.VPSs.Remove(vps);
			vps.Order.VPSs.Remove(vps);
			_context.States.RemoveRange(vps.States);
			_context.Activities.RemoveRange(vps.Activities);
			_context.VPSs.Remove(vps);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateVPS(VPS vps) {
			this._context.VPSs.Update(vps);
			await this._context.SaveChangesAsync();
		}

		public async Task UpdateStatus(string id, ServerStatus status, float cpu, float ram) =>
			await UpdateStatus(await Find(id), status, cpu, ram);

		public async Task UpdateStatus(VPS vps, ServerStatus status, float cpu, float ram) {
			State state = new State {
				Status = status,
				Time = DateTime.Now,
				CPU = cpu,
				RAM = ram,
			};
			_context.States.Add(state);
			vps.States.Add(state);
			await _context.SaveChangesAsync();
		}

		public async Task RegisterActivity(string id, string message, string url = null) =>
			await RegisterActivity(await Find(id), message, url);

		public async Task RegisterActivity(VPS vps, string message, string url = null) {
			Activity activity = new Activity {
				Message = message,
				URL = url,
			};
			_context.Activities.Add(activity);
			vps.Activities.Add(activity);
			vps.User.Activities.Add(activity);
			await _context.SaveChangesAsync();
		}
	}
}
