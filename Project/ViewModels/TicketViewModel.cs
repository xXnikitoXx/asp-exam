using Project.Enums;
using Project.Models;

namespace Project.ViewModels {
	public class TicketViewModel {
		public string Id { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public Priority Priority { get; set; }
		public string AnswerId { get; set; }
		public Message Answer { get; set; }
	}
}