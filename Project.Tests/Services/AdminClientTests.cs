using System;
using System.Threading.Tasks;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Xunit;

namespace Project.Tests.Services {
	public class AdminClientTests {
		[Fact]
		public async Task ReturnsCorrectAnnouncementCount() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAdminClient service = new AdminClient(context);

			// Act
			context.Announcements.Add(new Announcement {
				Title = "test test",
				Content = "test test test",
				Type = NotificationType.Info,
			});
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(1, service.AnnouncementsCount());

			context.Dispose();
		}

		[Fact]
		public async Task ReturnsCorrectVPSCount() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAdminClient service = new AdminClient(context);

			// Act
			context.VPSs.Add(new VPS {
				Name = "test server",
				Location = Location.Falkenstein_Germany,
				Cores = 1,
				RAM = 1,
				SSD = 1,
			});
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(1, service.VPSsCount());

			context.Dispose();
		}

		[Fact]
		public async Task ReturnsCorrectOrderCount() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAdminClient service = new AdminClient(context);

			// Act
			context.Orders.Add(new Order {
				Amount = 1,
				OriginalPrice = 1,
				FinalPrice = 1,
				State = OrderState.Created,
			});
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(1, service.OrdersCount());

			context.Dispose();
		}

		[Fact]
		public async Task ReturnsCorrectPromoCodeCount() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAdminClient service = new AdminClient(context);

			// Act
			context.PromoCodes.Add(new PromoCode {
				Code = "ASD123",
				Value = 1,
				Type = PromoCodeType.Free,
			});
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(1, service.PromoCodesCount());

			context.Dispose();
		}

		[Fact]
		public async Task ReturnsCorrectTicketCount() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAdminClient service = new AdminClient(context);

			// Act
			context.Tickets.Add(new Ticket {
				Subject = "test test",
				Message = "test test test",
				Priority = Priority.High,
			});
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(1, service.TicketsCount());

			context.Dispose();
		}

		[Fact]
		public async Task ReturnsCorrectPlanCount() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAdminClient service = new AdminClient(context);

			// Act
			context.Plans.Add(new Models.Plan {
				Number = 1,
				Name = "VPS_01",
				Cores = 1,
				RAM = 1,
				SSD = 1,
				Price = 1,
			});
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(1, service.PlansCount());

			context.Dispose();
		}
	}
}