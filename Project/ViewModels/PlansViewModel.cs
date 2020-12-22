using System.Collections.Generic;

namespace Project.ViewModels {
	public class PlansViewModel : ListViewModel {
		public PlansViewModel() =>
			this.Plans = new List<PlanViewModel>();

		public List<PlanViewModel> Plans { get; set; }
	}
}