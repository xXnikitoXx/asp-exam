namespace Project.ViewModels {
	public class PromoCodeEditInputModel {
		public string Id { get; set; }
		public string Code { get; set; }
		public string Type { get; set; }
		public double Value { get; set; }
		public bool IsValid { get; set; }
	}
}