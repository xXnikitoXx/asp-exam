using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Services.Native {
	public interface IAccountClient {
		Task SignOut(ApplicationUser user);
		Task SignOut(ClaimsPrincipal user);
		Task SignOut(string id);
		Task RemoveUser(string id);
	}
}