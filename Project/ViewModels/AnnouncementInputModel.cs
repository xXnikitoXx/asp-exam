using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels {
	public class AnnouncementInputModel {
		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }
		public string Type { get; set; }
	}
}