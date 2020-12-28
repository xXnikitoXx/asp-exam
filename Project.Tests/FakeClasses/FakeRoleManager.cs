using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Project.Tests.FakeClasses {
	public class FakeRoleManager : RoleManager<IdentityRole> {
		public FakeRoleManager() : base(
			new Mock<FakeRoleStore>().Object,
			new Mock<List<IRoleValidator<IdentityRole>>>().Object.AsEnumerable(),
			new Mock<ILookupNormalizer>().Object,
			new Mock<IdentityErrorDescriber>().Object,
			new Mock<ILogger<RoleManager<IdentityRole>>>().Object
		) {}
	}
}