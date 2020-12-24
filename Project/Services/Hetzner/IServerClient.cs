using System.Collections.Generic;
using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using Project.Models;

namespace Project.Services.Hetzner {
	public interface IServerClient {
		Task<Image> FromInput(string distro, string version);
		Task<KeyValuePair<string, Server>> SetupServer(VPS vps, Image image);
	}
}