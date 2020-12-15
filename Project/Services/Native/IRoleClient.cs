using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public interface IRoleClient {
		ApplicationUser First();
		ApplicationUser Find(string id);
		List<ApplicationUser> GetUsers();
		List<ApplicationUser> GetUsers(UsersViewModel pageInfo);
		Task<bool> HasAdmin();
		Task<bool> IsAdmin(string id);
		Task<bool> IsAdmin(ApplicationUser user);
		Task<bool> IsAdmin(ClaimsPrincipal user);
		Task PromoteToAdmin(ApplicationUser user);
		Task PromoteToAdmin(ClaimsPrincipal user);
		Task PromoteToAdmin(string id);
		Task DemoteToUser(ApplicationUser user);
		Task DemoteToUser(ClaimsPrincipal user);
		Task DemoteToUser(string id);
	}
}