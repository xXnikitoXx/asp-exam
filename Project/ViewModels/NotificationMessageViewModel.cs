using System;
using Project.Enums;

namespace Project.ViewModels {
	public class NotificationMessageViewModel {
		public MessageStatus Status { get; set; }
		public string Content { get; set; }
		public string URL { get; set; }
		public DateTime Time { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
	}
}