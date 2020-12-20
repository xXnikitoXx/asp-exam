using System.Collections.Generic;

namespace Project.ViewModels {
	public class VPSsViewModel : ListViewModel {
		public VPSsViewModel() =>
			this.VPSs = new List<VPSViewModel>();

		public List<VPSViewModel> VPSs { get; set; }
	}
}
