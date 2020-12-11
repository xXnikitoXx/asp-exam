using System.Collections.Generic;

namespace Project.ViewModels {
	public class PromoCodesViewModel : ListViewModel
	{
		public PromoCodesViewModel() {
			this.Codes = new List<PromoCodeViewModel>();
		}

		public List<PromoCodeViewModel> Codes { get; set; }
		public int Active { get; set; }
		public int Inactive { get; set; }
		public int FixedAmount { get; set; }
		public int Percentage { get; set; }
		public int PriceOverride { get; set; }
		public int Free { get; set; }
	}
}