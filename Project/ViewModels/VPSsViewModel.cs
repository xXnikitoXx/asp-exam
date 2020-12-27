using System.Collections.Generic;

namespace Project.ViewModels {
	public class VPSsViewModel : ListViewModel {
		public VPSsViewModel() =>
			this.VPSs = new List<VPSViewModel>();

		public List<VPSViewModel> VPSs { get; set; }
		public int OnlineCount { get; set; }
		public int OfflineCount { get; set; }
		public int ErrorCount { get; set; }
		public int MaintenanceCount { get; set; }
	}
}
