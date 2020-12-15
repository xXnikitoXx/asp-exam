using System.Threading.Tasks;
using Project.Models;

namespace Project.Services.Native {
	public interface IAccountClient {
		Task SignOut(ApplicationUser user);
		Task RemoveUser(string id);
	}
}