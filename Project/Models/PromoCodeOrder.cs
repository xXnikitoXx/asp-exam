namespace Project.Models {
	public class PromoCodeOrder {
		public string PromoCodeId { get; set; }
		public PromoCode PromoCode { get; set; }
		public string OrderId { get; set; }
		public Order Order { get; set; }
	}
}
