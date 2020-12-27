using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models;
using Project.Services.Hetzner.Extensions;

namespace Project.Services.Hetzner {
	public class HetznerServerClient : IServerClient {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public HetznerServerClient(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager
		) {
			this._context = context;
			this._userManager = userManager;
			lkcode.hetznercloudapi.Core.ApiCore.ApiToken = Environment.GetEnvironmentVariable("HETZNER_TOKEN");
		}

		public async Task<Server> Find(string id) =>
			await Server.GetByIdAsync(int.Parse(id));

		public async Task<List<Server>> GetAllServers() =>
			await Server.GetAsync();

		public async Task<Image> FindImage(string id) =>
			(await Image.GetAsync())
			.FirstOrDefault(image => image.Id.ToString() == id);

		public async Task<Image> FromInput(string distro, string version) =>
			(await Image.GetAsync())
			.FirstOrDefault(image => image.OsFlavor == distro && image.OsVersion == version);

		public async Task<ServerType> GetType(byte cores, byte memory, ushort storage) =>
			(await ServerType.GetAsync())
			.FirstOrDefault(type =>
				type.Cores == cores &&
				type.Memory == memory &&
				type.Disc == storage &&
				type.CpuType == "shared"
			);

		public async Task<KeyValuePair<string, Server>> SetupServer(VPS vps, Image image) {
			Server newServer = new Server();
			newServer.Name = $"{vps.User.UserName}.{vps.Name}";
			ServerType serverType = await GetType(vps.Cores, vps.RAM, vps.SSD);
			long location = -1;
			switch (vps.Location) {
				case Enums.Location.Falkenstein_Germany: location = 1; break;
				case Enums.Location.Nuremberg_Germany: location = 2; break;
			}
			newServer.ServerType = serverType;
			ServerActionResponse action = await newServer.SaveAsync(image, true, location);
			if (action.Error != null)
				throw new Exception(action.Error.Message);
			else
				return new KeyValuePair<string, Server>(action.AdditionalActionContent.ToString(), newServer);
		}

		public async Task<ServerActionResponse> PowerOn(string id) =>
			await (new Server { Id = long.Parse(id) }).PowerOn();

		public async Task<ServerActionResponse> Reboot(string id) =>
			await (new Server { Id = long.Parse(id) }).Reboot();

		public async Task<ServerActionResponse> Reset(string id) =>
			await (new Server { Id = long.Parse(id) }).Reset();

		public async Task<ServerActionResponse> Shutdown(string id) =>
			await (new Server { Id = long.Parse(id) }).Shutdown();

		public async Task<ServerActionResponse> PowerOff(string id) =>
			await (new Server { Id = long.Parse(id) }).PowerOff();
	}
}