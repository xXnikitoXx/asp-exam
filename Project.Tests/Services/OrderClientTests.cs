using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Project.Tests.FakeClasses;
using Project.ViewModels;
using Xunit;

namespace Project.Tests.Services {
	public class OrderClientTests {
		private ApplicationUser sampleUser =>
			new ApplicationUser {
				UserName = "testUser",
				Email = "test@user.com",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				PhoneNumber = "111-222-3344",
			};

		private Order sampleOrder =>
			new Order {
				TimeStarted = DateTime.Now,
				Amount = 1,
				OriginalPrice = 1,
				FinalPrice = 1,
				State = OrderState.Created,
			};

		[Fact]
		public async Task ReturnsSpecificOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order = sampleOrder;
			context.Orders.Add(order);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(order, await service.Find(order.Id));
		}

		[Fact]
		public async Task ReturnsPageWithOrders() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order = sampleOrder;
			context.Orders.AddRange(order, order);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order }, service.GetOrders(new OrdersViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task ReturnsOrdersOfUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			context.Orders.Add(order);
			order.UserId = user.Id;
			user.Orders.Add(order);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order }, service.GetOrders(user));
		}

		[Fact]
		public async Task ReturnsPageWithOrdersOfUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order1 = sampleOrder,
				order2 = sampleOrder;
			context.Orders.AddRange(order1, order2);
			order1.UserId = user.Id;
			order2.UserId = user.Id;
			user.Orders.Add(order1);
			user.Orders.Add(order2);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2 }, service.GetOrders(user, new OrdersViewModel { Page = 1 ,Show = 1 }));
		}

		[Fact]
		public async Task ReturnsFinishedOrders() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder;
			order2.State = OrderState.Finished;
			context.Orders.AddRange(order1, order2);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2 }, service.FinishedOrders().ToList());
		}

		[Fact]
		public async Task ReturnsFinishedOrdersOfUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order2.UserId = user.Id;
			order3.UserId = user.Id;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			user.Orders.Add(order1);
			user.Orders.Add(order2);
			user.Orders.Add(order3);
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2, order1 }, service.FinishedOrders(user).ToList());
		}

		[Fact]
		public async Task ReturnsPageWithFinishedOrders() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order1 }, service.FinishedOrders(new OrdersViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task ReturnsFinishedOrdersFromToday() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddDays(-1);
			order2.TimeFinished = DateTime.Today;
			context.Orders.AddRange(order1, order2);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2 }, service.FromToday());
		}

		[Fact]
		public async Task ReturnsPageWithFinishedOrdersFromToday() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddDays(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Today;
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2 }, service.FromToday(new OrdersViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task ReturnsFinishedOrdersFromThisMonth() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddMonths(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Today.AddDays(-1);
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();
			List<Order> result = new List<Order> { order2 };
			if (DateTime.Today.Day > 1)
				result.Add(order3);

			// Assert
			Assert.Equal(result, service.FromThisMonth());
		}

		[Fact]
		public async Task ReturnsPageWithFinishedOrdersFromThisMonth() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder,
				order4 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order4.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddMonths(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Today;
			order4.TimeFinished = DateTime.Today.AddDays(-1);
			context.Orders.AddRange(order1, order2, order3, order4);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2 }, service.FromThisMonth(new OrdersViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task ReturnsFinishedOrdersFromThisYear() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddYears(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Today.AddMonths(-1);
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();
			List<Order> result = new List<Order> { order2 };
			if (DateTime.Today.Month > 1)
				result.Add(order3);

			// Assert
			Assert.Equal(result, service.FromThisYear());
		}

		[Fact]
		public async Task ReturnsPageWithFinishedOrdersFromThisYear() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder,
				order4 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order4.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddYears(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Today;
			order4.TimeFinished = DateTime.Today.AddMonths(-1);
			context.Orders.AddRange(order1, order2, order3, order4);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Order> { order2 }, service.FromThisYear(new OrdersViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task CalculatesDailyIncome() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddDays(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Now;
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(2, service.DailyIncome());
		}

		[Fact]
		public async Task CalculatesMonthlyIncome() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order2.FinalPrice = 2;
			order3.FinalPrice = 2;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddMonths(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Now;
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(4, service.MonthlyIncome());
		}

		[Fact]
		public async Task CalculatesYearlyIncome() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order2.FinalPrice = 3;
			order3.FinalPrice = 3;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddYears(-1);
			order2.TimeFinished = DateTime.Today;
			order3.TimeFinished = DateTime.Now;
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(6, service.YearlyIncome());
		}

		[Fact]
		public async Task CalculatesTotalIncome() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder,
				order3 = sampleOrder;
			order2.FinalPrice = 3;
			order3.FinalPrice = 3;
			order1.State = OrderState.Finished;
			order2.State = OrderState.Finished;
			order3.State = OrderState.Finished;
			order1.TimeFinished = DateTime.Today.AddYears(-1);
			order2.TimeFinished = DateTime.Today.AddMonths(-1);
			order3.TimeFinished = DateTime.Now;
			context.Orders.AddRange(order1, order2, order3);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(7, service.TotalIncome());
		}

		[Fact]
		public async Task RegistersOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.UserId = user.Id;
			await service.RegisterOrder(order);

			// Assert
			Assert.Equal(order, await context.Orders.FirstAsync());
		}

		[Fact]
		public async Task RemovesSavedAndCancelledOrders() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order1 = sampleOrder,
				order2 = sampleOrder;
			order1.State = OrderState.Created;
			order2.State = OrderState.Cancelled;
			order1.UserId = user.Id;
			order2.UserId = user.Id;
			context.Orders.AddRange(order1, order2);
			user.Orders.Add(order1);
			user.Orders.Add(order2);
			await context.SaveChangesAsync();
			await service.RemoveOrder(order1);
			await service.RemoveOrder(order2);

			// Assert
			Assert.Empty(context.Orders);
		}

		[Fact]
		public async Task ThrowsExceptionForOrdersWithDisallowedRemoveState() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IOrderClient service = new OrderClient(null, null, context, new FakeUserManager(context));

			// Act
			Order order1 = sampleOrder,
				order2 = sampleOrder;
			order1.State = OrderState.Finished;
			context.Orders.AddRange(order1);
			await context.SaveChangesAsync();

			// Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await service.RemoveOrder(order1));
		}

		[Fact]
		public async Task UpdatesOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPromoCodeClient codeService = new PromoCodeClient(context, new FakeUserManager(context));
			IOrderClient service = new OrderClient(planService, codeService, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.PlanNumber = 1;
			order.UserId = user.Id;
			context.Orders.Add(order);
			user.Orders.Add(order);
			await context.SaveChangesAsync();
			order.Amount = 2;
			await service.UpdateOrder(order);

			// Assert
			Assert.NotEqual(planService.GetPlans()[0].Price, (await context.Orders.FirstAsync()).FinalPrice);
			Assert.Equal(planService.GetPlans()[0].Price * 2, (await context.Orders.FirstAsync()).FinalPrice);
		}

		[Fact]
		public async Task UpdatesOrderState() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPromoCodeClient codeService = new PromoCodeClient(context, new FakeUserManager(context));
			IOrderClient service = new OrderClient(planService, codeService, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.PlanNumber = 1;
			context.Orders.AddRange(order);
			await context.SaveChangesAsync();
			await service.UpdateState(order, OrderState.Finished);

			// Assert
			Assert.Equal(OrderState.Finished, (await context.Orders.FirstAsync()).State);
		}

		[Fact]
		public async Task UpdatesTimeWhenOrderIsFinished() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPromoCodeClient codeService = new PromoCodeClient(context, new FakeUserManager(context));
			IOrderClient service = new OrderClient(planService, codeService, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.PlanNumber = 1;
			context.Orders.AddRange(order);
			await context.SaveChangesAsync();
			await service.UpdateState(order, OrderState.Finished);

			// Assert
			Assert.NotNull((await context.Orders.FirstAsync()).TimeFinished);
		}

		[Fact]
		public async Task AddsVPSToOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPromoCodeClient codeService = new PromoCodeClient(context, new FakeUserManager(context));
			IOrderClient service = new OrderClient(planService, codeService, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.PlanNumber = 1;
			context.Orders.AddRange(order);
			VPS vps = new VPS {
				Name = "test",
				Location = Location.Falkenstein_Germany,
				Cores = 1,
				RAM = 1,
				SSD = 1,
			};
			context.VPSs.Add(vps);
			await context.SaveChangesAsync();
			await service.AddVPS(order, vps);

			// Assert
			Assert.Equal(new List<VPS> { vps }, (await context.Orders.FirstAsync()).VPSs);
			Assert.Equal(order, (await context.VPSs.FirstAsync()).Order);
		}

		[Fact]
		public async Task RemovesVPSFromOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPromoCodeClient codeService = new PromoCodeClient(context, new FakeUserManager(context));
			IOrderClient service = new OrderClient(planService, codeService, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.PlanNumber = 1;
			context.Orders.AddRange(order);
			VPS vps = new VPS {
				Name = "test",
				Location = Location.Falkenstein_Germany,
				Cores = 1,
				RAM = 1,
				SSD = 1,
			};
			context.VPSs.Add(vps);
			vps.UserId = user.Id;
			vps.OrderId = order.Id;
			order.VPSs.Add(vps);
			user.VPSs.Add(vps);
			await context.SaveChangesAsync();
			await service.RemoveVPS(order, vps);

			// Assert
			Assert.Empty((await context.Orders.FirstAsync()).VPSs);
		}

		[Fact]
		public async Task SetsVPSsToOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPromoCodeClient codeService = new PromoCodeClient(context, new FakeUserManager(context));
			IOrderClient service = new OrderClient(planService, codeService, context, new FakeUserManager(context));

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Order order = sampleOrder;
			order.PlanNumber = 1;
			context.Orders.AddRange(order);
			VPS vps1 = new VPS { Name = "test1", Location = Location.Falkenstein_Germany, Cores = 1, RAM = 1, SSD = 1, },
				vps2 = new VPS { Name = "test2", Location = Location.Falkenstein_Germany, Cores = 1, RAM = 1, SSD = 1, };
			context.VPSs.AddRange(vps1, vps2);
			await context.SaveChangesAsync();
			await service.UpdateVPSs(order, new List<VPS> { vps1, vps2 });

			// Assert
			Assert.Equal(new List<VPS> { vps1, vps2 }, (await context.Orders.FirstAsync()).VPSs);
		}

		[Fact]
		public void ContructsCorrectOrderFromPlan() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);

			// Act
			Models.Plan plan = planService.GetPlans()[0];
			Order order = OrderClient.OrderFromPlan(plan);

			// Assert
			Assert.Equal(plan.Price, order.OriginalPrice);
			Assert.Equal(plan.Price, order.FinalPrice);
			Assert.Equal(plan.Number, order.PlanNumber);
			Assert.Equal(OrderState.Created, order.State);
			Assert.Equal(1, order.Amount);
		}
	}
}