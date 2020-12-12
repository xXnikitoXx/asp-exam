using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public interface IAnnouncementClient {
		Announcement GetAnnouncement(string id);
		List<Announcement> GetAnnouncements();
		List<Announcement> GetAnnouncements(AnnouncementsViewModel pageInfo);
		Task CreateAnnouncement(Announcement announcement);
		Task RemoveAnnouncement(Announcement announcement);
		Task RemoveAnnouncement(string id);
	}
}