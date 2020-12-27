using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Services.Hetzner;

namespace Project.Services.Native {
	public class ServerDataClient : IServerDataClient {
		private readonly IServerClient _hetznerService;
		private readonly ApplicationDbContext _context;

		public ServerDataClient(
			IServerClient serverService,
			ApplicationDbContext context
		) {
			this._hetznerService = serverService;
			this._context = context;
		}

		public async Task<ServerData> Find(string id) =>
			await this._context.ServersData.FirstOrDefaultAsync(data => data.Id == id);
		
		public async Task<ServerData> FindByExternalId(string externalId) =>
			await this._context.ServersData.FirstOrDefaultAsync(data => data.ExternalId == externalId);

		public async Task<ServerData> FindByVPSId(string vpsId) =>
			await this._context.ServersData.FirstOrDefaultAsync(data => data.VPSId == vpsId);

		public async Task<ServerData> FindByVPS(VPS vps) =>
			await FindByVPSId(vps.Id);

		public async Task CreateForVPS(VPS vps, Server server, Image image) {
			ServerData data = new ServerData {
				ExternalId = server.Id.ToString(),
				Status = server.Status,
				IPv4 = server.Network.Ipv4.Ip,
				IPv4DNSPointer = server.Network.Ipv4.DnsPointer == null ? null : server.Network.Ipv4.DnsPointer,
				IPv4Blocked = server.Network.Ipv4.Blocked,
				IPv6 = server.Network.Ipv6.Ip,
				IPv6DNSPointer = server.Network.Ipv6.DnsPointer == null ? null : string.Join("; ", server.Network.Ipv6.DnsPointer),
				IPv6Blocked = server.Network.Ipv6.Blocked,
				Distro = image.OsFlavor,
				DistroVersion = image.OsVersion,
			};
			this._context.ServersData.Add(data);
			await this._context.SaveChangesAsync();
			data.VPSId = vps.Id;
			vps.ServerDataId = data.Id;
			await this._context.SaveChangesAsync();
		}

		public async Task CreateForVPS(VPS vps, string externalId, Image image) =>
			await CreateForVPS(vps, await this._hetznerService.Find(externalId), image);

		public async Task UpdateData(ServerData data) {
			this._context.Update(data);
			await this._context.SaveChangesAsync();
		}

		public async Task Remove(string id) {
			this._context.ServersData.Remove(await Find(id));
			await this._context.SaveChangesAsync();
		}

		public async Task Remove(ServerData data) =>
			await Remove(data.Id);

		public async Task RemoveByExternalId(string externalId) =>
			await Remove(await FindByExternalId(externalId));

		public async Task RemoveByVPSId(string vpsId) =>
			await Remove(await FindByVPSId(vpsId));

		public async Task RemoveByVPS(VPS vps) =>
			await Remove(await FindByVPS(vps));
	}
}