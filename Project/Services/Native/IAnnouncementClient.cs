using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Services.Native {
	public interface IAnnouncementClient {
		Announcement GetAnnouncement(string id);
		List<Announcement> GetAnnouncements();
		Task CreateAnnouncement(Announcement announcement);
		Task RemoveAnnouncement(Announcement announcement);
		Task RemoveAnnouncement(string id);
	}
}