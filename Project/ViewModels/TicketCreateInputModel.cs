using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels {
	public class TicketCreateInputModel {
		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Subject { get; set; }

		[Required]
		public string Priority { get; set; }

		[Required]
		public string Message { get; set; }
	}
}