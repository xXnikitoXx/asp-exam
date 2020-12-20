using System;

namespace Project.ViewModels {
	public class PaymentViewModel {
		public string Id { get; set; }
		public DateTime Time { get; set; }
		public string PayPalPayment { get; set; }
		public string PayPalPayer { get; set; }
		public string OrderId { get; set; }
		public VPSViewModel AssociatedVPS { get; set; }
	}
}