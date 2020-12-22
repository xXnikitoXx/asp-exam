using System;
using System.Collections.Generic;

namespace Project.ViewModels {
	public class PaymentViewModel {
		public PaymentViewModel() =>
			this.AssociatedVPSs = new List<VPSViewModel>();

		public string Id { get; set; }
		public DateTime Time { get; set; }
		public string PayPalPayment { get; set; }
		public string PayPalPayer { get; set; }
		public string OrderId { get; set; }
		public List<VPSViewModel> AssociatedVPSs { get; set; }
	}
}