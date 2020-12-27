using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using Project.Models;

namespace Project.Services.Native {
	public interface IServerDataClient {
		Task<ServerData> Find(string id);
		Task<ServerData> FindByExternalId(string externalId);
		Task<ServerData> FindByVPSId(string vpsId);
		Task<ServerData> FindByVPS(VPS vps);
		Task CreateForVPS(VPS vps, Server server, Image image);
		Task CreateForVPS(VPS vps, string externalId, Image image);
		Task UpdateData(ServerData data);
		Task Remove(string id);
		Task Remove(ServerData data);
		Task RemoveByExternalId(string externalId);
		Task RemoveByVPSId(string vpsId);
		Task RemoveByVPS(VPS vps);
	}
}