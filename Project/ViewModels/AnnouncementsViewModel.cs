using System.Collections.Generic;
using Project.Models;

namespace Project.ViewModels {
	public class AnnouncementsViewModel : ListViewModel {
		public AnnouncementsViewModel() =>
			this.Announcements = new List<Announcement>();

		public List<Announcement> Announcements { get; set; }
	}
}