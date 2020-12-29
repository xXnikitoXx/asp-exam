using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;
using Xunit;

namespace Project.Tests.Services {
	public class AnnouncementClientTests {
		[Fact]
		public async Task ReturnsSpecifiedAnnouncement() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAnnouncementClient service = new AnnouncementClient(context);

			// Act
			Announcement announcement = new Announcement {
				Title = "test test",
				Content = "test test test",
				Type = NotificationType.Info,
			};
			context.Announcements.Add(announcement);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(announcement, service.GetAnnouncement(announcement.Id));
		}

		[Fact]
		public async Task ReturnsAllAnnouncements() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAnnouncementClient service = new AnnouncementClient(context);

			// Act
			Announcement[] announcements = new Announcement[] { 
				new Announcement {
					Title = "test test 1",
					Content = "test test test",
					Type = NotificationType.Info,
				},
				new Announcement {
					Title = "test test 2",
					Content = "test test test",
					Type = NotificationType.Info,
				},
			};
			context.Announcements.AddRange(announcements);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(announcements.ToList(), service.GetAnnouncements());
		}

		[Fact]
		public async Task ReturnsPageWithAnnouncements() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAnnouncementClient service = new AnnouncementClient(context);

			// Act
			Announcement[] announcements = new Announcement[] { 
				new Announcement {
					Title = "test test 1",
					Content = "test test test",
					Type = NotificationType.Info,
				},
				new Announcement {
					Title = "test test 2",
					Content = "test test test",
					Type = NotificationType.Info,
				},
			};
			context.Announcements.AddRange(announcements);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(announcements.Take(1).ToList(), service.GetAnnouncements(new AnnouncementsViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task CreatesAnnouncement() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAnnouncementClient service = new AnnouncementClient(context);

			// Act
			Announcement announcement = new Announcement {
				Title = "test test",
				Content = "test test test",
				Type = NotificationType.Info,
			};
			await service.CreateAnnouncement(announcement);

			// Assert
			Assert.Equal(announcement, await context.Announcements.FirstAsync());
		}

		[Fact]
		public async Task RemovesAnnnouncement() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAnnouncementClient service = new AnnouncementClient(context);

			// Act
			Announcement announcement = new Announcement {
				Title = "test test",
				Content = "test test test",
				Type = NotificationType.Info,
			};
			context.Announcements.Add(announcement);
			await context.SaveChangesAsync();
			await service.RemoveAnnouncement(announcement);

			// Assert
			Assert.Empty(context.Announcements);
		}
	}
}