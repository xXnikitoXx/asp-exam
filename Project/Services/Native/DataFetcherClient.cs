using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using lkcode.hetznercloudapi.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Enums;
using Project.Models;
using Project.Services.Hetzner;

namespace Project.Services.Native {
	public class DataFetcherClient : BackgroundService {
		private IServiceProvider _services;
		private IVPSClient _vpsService;
		private IServerClient _hetznerService;
		private IServerDataClient _serverDataService;

		private readonly Dictionary<string, ServerStatus> _filter;
		private Timer _timer;

		public DataFetcherClient(IServiceProvider services) {
			this._services = services;
			this._filter = new Dictionary<string, ServerStatus> {
				{ "running", ServerStatus.Online },
				{ "initializing", ServerStatus.Online },
				{ "starting", ServerStatus.Online },
				{ "stopping", ServerStatus.Offline },
				{ "off", ServerStatus.Offline },
				{ "deleting", ServerStatus.Maintenance },
				{ "migrating", ServerStatus.Maintenance },
				{ "rebuilding", ServerStatus.Maintenance },
				{ "unknown", ServerStatus.Error },
			};
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken) {
			this._timer = new Timer(
				DoWork,
				null,
				TimeSpan.Zero,
				TimeSpan.FromSeconds(30)
			);
			return Task.CompletedTask;
		}

		public void DoWork(object state) {
			using (IServiceScope scope = this._services.CreateScope()) {
				this._vpsService = scope.ServiceProvider.GetRequiredService<IVPSClient>();
				this._hetznerService = scope.ServiceProvider.GetRequiredService<IServerClient>();
				this._serverDataService = scope.ServiceProvider.GetRequiredService<IServerDataClient>();

				List<Server> servers = this._hetznerService.GetAllServers().GetAwaiter().GetResult();
				SyncStatus(servers).GetAwaiter().GetResult();
			}
		}

		public async Task SyncStatus(List<Server> servers) {
			foreach (Server server in servers) {
				ServerData data = await this._serverDataService.FindByExternalId(server.Id.ToString());
				data.Status = server.Status;
				await this._serverDataService.UpdateData(data);
				DateTime end = DateTime.UtcNow;
				DateTime start = end.AddSeconds(-1);
				ServerMetric cpuMetric = await server.GetMetrics("cpu", start.ToString("o"), end.ToString("o"));
				ServerMetric diskMetric = await server.GetMetrics("disk", start.ToString("o"), end.ToString("o"));
				ServerMetric networkMetric = await server.GetMetrics("network", start.ToString("o"), end.ToString("o"));
				await UpdateVPS(data, cpuMetric, diskMetric, networkMetric);
			}
		}

		public async Task UpdateVPS(ServerData data, ServerMetric cpuMetric, ServerMetric diskMetric, ServerMetric networkMetric) {
			ServerMetricTimeSeries srs = cpuMetric.TimeSeries;
			double cpu = srs.CpuValues[0].Value;
			srs = diskMetric.TimeSeries;
			List<double> disk = new List<double> {
				srs.DiskValues[0].BandwithRead[0].Value,
				srs.DiskValues[0].BandwithWrite[0].Value,
				srs.DiskValues[0].IOPSRead[0].Value,
				srs.DiskValues[0].IOPSWrite[0].Value,
			};
			srs = networkMetric.TimeSeries;
			List<double> network = new List<double> {
				srs.NetworkValues[0].BandwithIn[0].Value,
				srs.NetworkValues[0].BandwithOut[0].Value,
				srs.NetworkValues[0].PPSIn[0].Value,
				srs.NetworkValues[0].PPSOut[0].Value,
			};
			await this._vpsService.UpdateStatus(data.VPSId, _filter[data.Status], cpu, disk, network);
		}
	}
}