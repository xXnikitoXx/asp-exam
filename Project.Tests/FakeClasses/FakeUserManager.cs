using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Project.Data;
using Project.Models;

namespace Project.Tests.FakeClasses {
	public class FakeUserManager : UserManager<ApplicationUser> {
		private readonly ApplicationDbContext _context;

		public FakeUserManager(ApplicationDbContext context) : base(
			new Mock<IUserSecurityStampStore<ApplicationUser>>().Object,
			new Mock<IOptions<IdentityOptions>>().Object,
			new Mock<IPasswordHasher<ApplicationUser>>().Object,
			new IUserValidator<ApplicationUser>[0],
			new IPasswordValidator<ApplicationUser>[0],
			new Mock<ILookupNormalizer>().Object,
			new Mock<IdentityErrorDescriber>().Object,
			new Mock<IServiceProvider>().Object,
			new Mock<ILogger<UserManager<ApplicationUser>>>().Object
		) {
			this._context = context;
		}

		public override async Task<IdentityResult> DeleteAsync(ApplicationUser user) {
			this._context.Users.Remove(user);
			await this._context.SaveChangesAsync();
			return new IdentityResult();
		}

		public Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType) {
			ClaimsIdentity identity = new ClaimsIdentity();
			identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
			return Task.FromResult(identity);
		}

		public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password) =>
			Task.FromResult(IdentityResult.Success);

		public override Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) =>
			Task.FromResult(IdentityResult.Success);

		public override Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) =>
			Task.FromResult(Guid.NewGuid().ToString());

		public override Task<string> GetSecurityStampAsync(ApplicationUser user) =>
			Task.FromResult(Guid.NewGuid().ToString());
	}
}