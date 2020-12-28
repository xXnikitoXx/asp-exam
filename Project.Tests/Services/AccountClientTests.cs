using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.Tests.FakeClasses;
using Xunit;

namespace Project.Tests.Services {
	public class AccountClientTests {
		private ApplicationUser sampleUser =>
			new ApplicationUser {
				UserName = "testUser",
				Email = "test@user.com",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				PhoneNumber = "111-222-3344",
			};

		[Fact]
		public async Task ThrowsExceptionWhenUserIsNotFound() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IAccountClient service = new AccountClient(context, null, null, null, null);

			// Assert
			await Assert.ThrowsAsync<Exception>(async () => await service.RemoveUser(""));

			context.Dispose();
		}

		[Fact]
		public async Task RemovesTargetUserIfExists() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			FakeUserManager userManager = new FakeUserManager(context);
			FakeSignInManager signInManager = new FakeSignInManager(context);
			Mock<FakeRoleManager> roleManager = new Mock<FakeRoleManager>();
			RoleClient roleService = new RoleClient(context, roleManager.Object, userManager);
			OrderClient orderService = new OrderClient(null, null, context, userManager);
			IAccountClient service = new AccountClient(context, userManager, signInManager, roleService, orderService);

			// Act
			ApplicationUser user = sampleUser;
			user.SecurityStamp = Guid.NewGuid().ToString();
			context.Users.Add(user);
			await context.SaveChangesAsync();
			string id = context.Users.First().Id;
			await service.RemoveUser(id);

			// Assert
			Assert.Empty(context.Users);

			context.Dispose();
		}
	}
}