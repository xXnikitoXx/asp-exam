using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Native;
using Project.Tests.FakeClasses;
using Xunit;

namespace Project.Tests.Services {
	public class PaymentClientTests {
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

		private Payment samplePayment =>
			new Payment {
				PayPalPayer = "TEST_PAYER_ID",
				PayPalPayment = "TEST_PAYPAL_ID",
				Time = DateTime.Now,
			};

		[Fact]
		public async Task ReturnsSpecifiedPayment() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPaymentClient service = new PaymentClient(context, null, null, null, null);

			// Act
			Payment payment = samplePayment;
			context.Payments.Add(payment);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(payment, await service.Find(payment.Id));
		}

		[Fact]
		public async Task ReturnsPaymentForSpecificOrder() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPaymentClient service = new PaymentClient(context, null, null, null, null);

			// Act
			Order order = sampleOrder;
			Payment payment = samplePayment;
			context.Orders.Add(order);
			context.Payments.Add(payment);
			await context.SaveChangesAsync();
			order.PaymentId = payment.Id;
			payment.OrderId = order.Id;
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(payment, service.GetPayment(order.Id));
		}

		[Fact]
		public async Task ReturnsPaymentsOfSpecificUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPaymentClient service = new PaymentClient(context, null, null, null, null);

			// Act
			ApplicationUser user = sampleUser;
			Order order1 = sampleOrder,
				order2 = sampleOrder;
			Payment payment1 = samplePayment,
				payment2 = samplePayment;
			context.Orders.AddRange(order1, order2);
			context.Payments.AddRange(payment1, payment2);
			await context.SaveChangesAsync();
			order1.PaymentId = payment1.Id;
			order2.PaymentId = payment2.Id;
			payment1.OrderId = order1.Id;
			payment2.OrderId = order2.Id;
			order2.UserId = payment2.UserId = user.Id;
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Payment> { payment2 }, service.GetPayments(user));
		}

		[Fact]
		public async Task ThrowsExceptionWhenUserIsNull() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPaymentClient service = new PaymentClient(context, null, planService, null, null);

			// Act
			Payment payment = samplePayment;
			payment.UserId = "1234567890";

			// Assert
			await Assert.ThrowsAsync<Exception>(async () => await service.CreatePayment(payment));
		}

		[Fact]
		public async Task ThrowsExceptionWhenOrderIsNull() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			IPaymentClient service = new PaymentClient(context, null, planService, null, null);

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Payment payment = samplePayment;
			payment.UserId = user.Id;

			// Assert
			await Assert.ThrowsAsync<Exception>(async () => await service.CreatePayment(payment));
		}

		[Fact]
		public async Task CreatesPaymentWhenDataIsValid() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IPlanClient planService = new PlanClient(context);
			Mock<FakeUserManager> userManagerMock = new Mock<FakeUserManager>(context);
			userManagerMock.Setup(um => um.FindByIdAsync(null))
				.Returns(async () => await context.Users.FirstAsync());
			FakeUserManager userManager = userManagerMock.Object;
			IPromoCodeClient codeService = new PromoCodeClient(context, userManager);
			IOrderClient orderService = new OrderClient(planService, codeService, context, userManager);
			IVPSClient vpsService = new VPSClient(context, orderService, userManager);
			IPaymentClient service = new PaymentClient(context, vpsService, planService, userManager, orderService);

			// Act
			ApplicationUser user = sampleUser;
			Models.Plan plan = PlanClient.PlanFor(1);
			Order order = sampleOrder;
			Payment payment = samplePayment;
			plan.Number = 999;
			order.PlanNumber = 999;
			context.Users.Add(user);
			context.Plans.Add(plan);
			context.Orders.Add(order);
			await context.SaveChangesAsync();
			user.Orders.Add(order);
			payment.UserId = user.Id;
			payment.OrderId = order.Id;
			await context.SaveChangesAsync();
			try {
				await service.CreatePayment(payment);
			} catch (NullReferenceException) {
				/*
					Fake user manager returns null when trying
					to create VPSs for the paid order.
				*/
			} 

			// Assert
			Assert.Equal(payment, await context.Payments.FirstAsync());
		}
	}
}