using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels {
	public class VPSSetupInputModel {
		[Required]
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Distro { get; set; }

		[Required]
		public string Version { get; set; }
	}
}