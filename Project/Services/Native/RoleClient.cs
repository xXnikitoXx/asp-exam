using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public class RoleClient : IRoleClient {
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
				try {
					if (!await this._roleManager.RoleExistsAsync("Administrator"))
						await this._roleManager.CreateAsync(new IdentityRole("Administrator"));
					if (!await HasAdmin()) {
						ApplicationUser user = First();
						if (user != null)
							await PromoteToAdmin(First());
					}
				} catch {}
			})
			.GetAwaiter().GetResult();
		}

		public ApplicationUser First() =>
			this._context.Users.OrderBy(user => user.JoinDate).FirstOrDefault();
		public ApplicationUser Find(string id) =>
			this._context.Users.FirstOrDefault(user => user.Id == id || user.UserName == id);

		public List<ApplicationUser> GetUsers() =>
			this._context.Users.ToList();
		public List<ApplicationUser> GetUsers(UsersViewModel pageInfo) {
			IQueryable<ApplicationUser> users = this._context.Users.AsQueryable();
			pageInfo.Total = users.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			string targetRoleId = this._context.Roles.FirstOrDefault(role => role.Name == "Administrator").Id;
			pageInfo.AdminsCount = this._context.UserRoles.Count(userRole => userRole.RoleId == targetRoleId);
			pageInfo.UsersCount = pageInfo.Total - pageInfo.AdminsCount;
			return users.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task<bool> HasAdmin() =>
			(await this._userManager.GetUsersInRoleAsync("Administrator")).Count != 0;
		public async Task<bool> IsAdmin(ApplicationUser user) =>
			await this._userManager.IsInRoleAsync(user, "Administrator");
		public async Task<bool> IsAdmin(ClaimsPrincipal user) =>
			await IsAdmin(await this._userManager.GetUserAsync(user));
		public async Task<bool> IsAdmin(string id) =>
			await this._userManager.IsInRoleAsync(Find(id), "Administrator");

		public async Task PromoteToAdmin(ApplicationUser user) {
			await this._userManager.AddToRoleAsync(user, "Administrator");
			await this._userManager.UpdateSecurityStampAsync(user);
		}

		public async Task PromoteToAdmin(ClaimsPrincipal user) =>
			await PromoteToAdmin(await this._userManager.GetUserAsync(user));

		public async Task PromoteToAdmin(string id) => await PromoteToAdmin(Find(id));

		public async Task DemoteToUser(ApplicationUser user) {
			await this._userManager.RemoveFromRoleAsync(user, "Administrator");
			await this._userManager.UpdateSecurityStampAsync(user);
		}

		public async Task DemoteToUser(ClaimsPrincipal user) =>
			await DemoteToUser(await this._userManager.GetUserAsync(user));

		public async Task DemoteToUser(string id) =>
			await DemoteToUser(Find(id));
	}
}