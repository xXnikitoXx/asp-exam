using System;
using Project.Enums;
using Project.Models;

namespace Project.ViewModels {
	public class MessageViewModel {
		public string Id { get; set; }

		public MessageStatus Status { get; set; }

		public string Content { get; set; }

		public string URL { get; set; }

		public DateTime Time { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public string TicketId { get; set; }
		public Ticket Ticket { get; set; }
	}
}