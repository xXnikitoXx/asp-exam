using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Services.Native {
	public interface IRoleClient {
		ApplicationUser First();
		ApplicationUser Find(string id);
		List<ApplicationUser> GetUsers();
		Task<bool> HasAdmin();
		Task<bool> IsAdmin(ApplicationUser user);
		Task PromoteToAdmin(ApplicationUser user);
		Task DemoteToUser(ApplicationUser user);
	}
}