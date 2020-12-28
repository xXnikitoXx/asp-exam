using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Tests {
	public static class DbContextOptions {
		private static int dbIndex = 0;

		public static DbContextOptions<ApplicationDbContext> Options =>
			new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("TestDb" + dbIndex++)
				.Options;
	}
}