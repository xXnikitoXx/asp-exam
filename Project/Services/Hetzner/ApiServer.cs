using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using lkcode.hetznercloudapi.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Project.Services.Hetzner.Extensions {
	public static class ApiServer {
		private static Server GetServerFromResponseData(lkcode.hetznercloudapi.Objects.Server.Universal.Server responseData) =>
			new Server {
				Id = responseData.id,
				Name = responseData.name,
				Status = responseData.status,
				Network = new Network() {
					Ipv4 = new AddressIpv4() {
						Ip = responseData.public_net.ipv4.ip,
						Blocked = responseData.public_net.ipv4.blocked,
					},
					Ipv6 = new AddressIpv6() {
						Ip = responseData.public_net.ipv6.ip,
						Blocked = responseData.public_net.ipv6.blocked,
					},
					FloatingIpIds = responseData.public_net.floating_ips,
				},
			};

		private static ServerActionResponse GetServerActionFromResponseData(lkcode.hetznercloudapi.Objects.Server.Universal.ServerAction responseData) =>
			new ServerActionResponse {
				Id = responseData.id,
				Command = responseData.command,
				Progress = responseData.progress,
				Started = responseData.started,
				Status = responseData.status,
			};

		private static Error GetErrorFromResponseData(lkcode.hetznercloudapi.Objects.Server.Universal.ErrorResponse errorResponse) =>
			new Error {
				Message = errorResponse.error.message,
				Code = errorResponse.error.code,
			};

		public static async Task<ServerActionResponse> SaveAsync(this Server server, Image image, bool startAfterCreate = true, long locationId = -1) {
			Dictionary<string, object> arguments = new Dictionary<string, object>();
			arguments.Add("name", server.Name);
			arguments.Add("server_type", server.ServerType.Id.ToString());
			arguments.Add("image", image.Id);
			arguments.Add("start_after_create", startAfterCreate.ToString());
			if (locationId > 0)
				arguments.Add("location", locationId.ToString());
			string responseContent = await ApiCore.SendPostRequest(string.Format("/servers"), arguments);
			JObject responseObject = JObject.Parse(responseContent);
			if (responseObject["error"] != null) {
				lkcode.hetznercloudapi.Objects.Server.Universal.ErrorResponse error = JsonConvert.DeserializeObject<lkcode.hetznercloudapi.Objects.Server.Universal.ErrorResponse>(responseContent);
				ServerActionResponse response = new ServerActionResponse();
				response.Error = GetErrorFromResponseData(error);
				throw new Exception(response.Error.Message);
			} else {
				lkcode.hetznercloudapi.Objects.Server.Create.Response response =
					JsonConvert.DeserializeObject<lkcode.hetznercloudapi.Objects.Server.Create.Response>(responseContent, new JsonSerializerSettings {
						NullValueHandling = NullValueHandling.Ignore
					});
				Server srv = GetServerFromResponseData(response.server);
				server.Id = srv.Id;
				server.Name = srv.Name;
				server.Network = srv.Network;
				server.ServerImage = srv.ServerImage;
				server.ServerType = srv.ServerType;
				server.Status = srv.Status;
				ServerActionResponse actionResponse = GetServerActionFromResponseData(response.action);
				actionResponse.AdditionalActionContent = response.root_password;
				return actionResponse;
			}
		}
	}
}