using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Data;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public class AnnouncementClient : IAnnouncementClient {
		private readonly ApplicationDbContext _context;

		public AnnouncementClient(ApplicationDbContext context) =>
			this._context = context;

		public Announcement GetAnnouncement(string id) =>
			this._context.Announcements.FirstOrDefault(announcement => announcement.Id == id);
		public List<Announcement> GetAnnouncements() =>
			this._context.Announcements.ToList();
		public List<Announcement> GetAnnouncements(AnnouncementsViewModel pageInfo) {
			pageInfo.Total = this._context.Announcements.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return this._context.Announcements.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task CreateAnnouncement(Announcement announcement) {
			this._context.Announcements.Add(announcement);
			await this._context.SaveChangesAsync();
		}

		public async Task RemoveAnnouncement(Announcement announcement) {
			if (announcement == null)
				throw new Exception();
			this._context.Remove(announcement);
			await this._context.SaveChangesAsync();
		}

		public async Task RemoveAnnouncement(string id) =>
			await RemoveAnnouncement(GetAnnouncement(id));
	}
}