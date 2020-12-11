using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native
{
	public class VPSClient : IVPSClient
	{
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public VPSClient(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			this._context = context;
			this._userManager = userManager;
		}

		async Task<VPS> Find(string id) => await _context.VPSs.FirstOrDefaultAsync(vps => vps.Id == id);

		public async Task<List<VPS>> GetVPSs(ClaimsPrincipal user) => GetVPSs(await _userManager.GetUserAsync(user));

		public List<VPS> GetVPSs(ApplicationUser user) => _context.VPSs.Where(vps => vps.UserId == user.Id).ToList();

		public async Task RegisterVPS(VPS vps)
		{
			_context.VPSs.Add(vps);
			_context.Users.Find(vps.User)
				.VPSs.Add(vps);
			await _context.SaveChangesAsync();
		}

		public async Task RemoveVPS(string id) => await RemoveVPS(await Find(id));

		public async Task RemoveVPS(VPS vps)
		{
			vps.User.VPSs.Remove(vps);
			vps.Order.VPSs.Remove(vps);
			_context.States.RemoveRange(vps.States);
			_context.Activities.RemoveRange(vps.Activities);
			_context.VPSs.Remove(vps);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateStatus(string id, ServerStatus status, float cpu, float ram) => await UpdateStatus(await Find(id), status, cpu, ram);

		public async Task UpdateStatus(VPS vps, ServerStatus status, float cpu, float ram) {
			State state = new State
			{
				Status = status,
				Time = DateTime.Now,
				CPU = cpu,
				RAM = ram,
			};
			_context.States.Add(state);
			vps.States.Add(state);
			await _context.SaveChangesAsync();
		}

		public async Task RegisterActivity(string id, string message, string url = null) => await RegisterActivity(await Find(id), message, url);

		public async Task RegisterActivity(VPS vps, string message, string url = null)
		{
			Activity activity = new Activity
			{
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
