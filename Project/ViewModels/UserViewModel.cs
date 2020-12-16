using System;

namespace Project.ViewModels {
	public class UserViewModel {
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsAdmin { get; set; }
		public DateTime JoinDate { get; set; }
		public DateTime LastLoginDate { get; set; }
		public AccountViewModel Account { get ;set; }
	}
}