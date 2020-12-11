using Project.Enums;

namespace Project.ViewModels {
	public class PromoCodeViewModel {
		public string Id { get; set; }

		public string Code { get; set; }

		public PromoCodeType Type { get; set; }

		public float Value { get; set; }

		public bool IsValid { get; set; }

		public int Usage { get; set; }
	}
}