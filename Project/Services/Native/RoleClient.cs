using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models;

namespace Project.Services.Native {
	public class RoleClient : IRoleClient
	{
		private readonly ApplicationDbContext _context;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public RoleClient(
			ApplicationDbContext context,
			RoleManager<IdentityRole> roleManager,
			UserManager<ApplicationUser> userManager
		) {
			this._context = context;
			this._roleManager = roleManager;
			this._userManager = userManager;

			Task.Run(async () => {
				if (!await this._roleManager.RoleExistsAsync("Administrator"))
					await this._roleManager.CreateAsync(new IdentityRole("Administrator"));
				if (!await HasAdmin())
					await PromoteToAdmin(First());
			}).Wait();
		}

		public ApplicationUser First() => this._context.Users.FirstOrDefault();
		public ApplicationUser Find(string id) => this._context.Users.FirstOrDefault(user => user.Id == id);

		public List<ApplicationUser> GetUsers() => this._context.Users.ToList();

		public async Task<bool> HasAdmin() => (await this._userManager.GetUsersInRoleAsync("Administrator")).Count != 0;
		public async Task<bool> IsAdmin(ApplicationUser user) => await this._userManager.IsInRoleAsync(user, "Administrator");

		public async Task PromoteToAdmin(ApplicationUser user) =>
			await this._userManager.AddToRoleAsync(user, "Administrator");

		public async Task DemoteToUser(ApplicationUser user) =>
			await this._userManager.RemoveFromRoleAsync(user, "Administrator");
	}
}