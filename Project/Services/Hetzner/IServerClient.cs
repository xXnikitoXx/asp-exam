using System.Collections.Generic;
using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using Project.Models;

namespace Project.Services.Hetzner {
	public interface IServerClient {
		Task<Server> Find(string Id);
		Task<List<Server>> GetAllServers();
		Task<Image> FindImage(string id);
		Task<Image> FromInput(string distro, string version);
		Task<KeyValuePair<string, Server>> SetupServer(VPS vps, Image image);
		Task<ServerActionResponse> PowerOn(string id);
		Task<ServerActionResponse> Reboot(string id);
		Task<ServerActionResponse> Reset(string id);
		Task<ServerActionResponse> Shutdown(string id);
		Task<ServerActionResponse> PowerOff(string id);
	}
}