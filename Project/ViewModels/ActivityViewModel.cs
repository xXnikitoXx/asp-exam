using System;
using Project.Enums;

namespace Project.ViewModels {
	public class ActivityViewModel {
		public string Id { get; set; }
		public string Message { get; set; }
		public NotificationType Type { get; set; }
		public string URL { get; set; }
		public DateTime Time { get; set; }
		public string UserId { get; set; }
	}
}