using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Models;

namespace Project.Tests.FakeClasses {
	public class FakeRoleStore : IRoleStore<IdentityRole>, IUserRoleStore<ApplicationUser> {
		public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken) { return null; }
		public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken) { return null; }
		public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken) { return null; }
		public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		public void Dispose() {}
		public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) { return null; }
		public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) { return null; }
		public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) { return null; }
		public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken) { return null; }
		public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) { return null; }
		public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) { return null; }
		public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken) { return null; }
		public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken) { return null; }
		public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken) { return null; }
		public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken) { return null; }
		public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken) { return null; }
		public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) { return null; }
		public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken) { return null; }
		public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) { return null; }
		Task<ApplicationUser> IUserStore<ApplicationUser>.FindByIdAsync(string userId, CancellationToken cancellationToken) { return null; }
		Task<ApplicationUser> IUserStore<ApplicationUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) { return null; }
	}
}