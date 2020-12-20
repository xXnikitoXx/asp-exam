using System.Collections.Generic;

namespace Project.ViewModels {
	public class UsersViewModel : ListViewModel {
		public UsersViewModel() =>
			this.Users = new List<UserViewModel>();

		public List<UserViewModel> Users { get; set; }
		public int UsersCount { get; set; }
		public int AdminsCount { get; set; }
	}
}