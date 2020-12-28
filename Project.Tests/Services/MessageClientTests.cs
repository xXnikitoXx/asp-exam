using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.Tests.FakeClasses;
using Project.ViewModels;
using Xunit;

namespace Project.Tests.Services {
	public class MessageClientTests {
		private ApplicationUser sampleUser =>
			new ApplicationUser {
				UserName = "testUser",
				Email = "test@user.com",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				PhoneNumber = "111-222-3344",
			};

		[Fact]
		public async Task ReturnsSpecifiedMessage() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			Message message = new Message { Content = "test test test" };
			context.Messages.Add(message);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(message, await service.Find(message.Id));
		}

		[Fact]
		public async Task ReturnsMessagesToUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Message message = new Message { Content = "test test test" };
			Message messageToUser =new Message {
				Content = "test test test",
				UserId = user.Id,
				User = user
			};
			context.Messages.Add(message);
			context.Messages.Add(messageToUser);
			user.Messages.Add(messageToUser);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Message> { messageToUser }, service.GetMessages(user));
		}

		[Fact]
		public async Task ReturnsPageWithMessagesToUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Message message = new Message { Content = "test test test" };
			Message messageToUser = new Message {
				Content = "test test test",
				UserId = user.Id,
				User = user
			};

			context.Messages.AddRange(new List<Message> { message, messageToUser, message, messageToUser });
			user.Messages.Add(messageToUser);
			await context.SaveChangesAsync();

			// Assert
			Assert.Equal(new List<Message> { messageToUser }, service.GetMessages(user, new MessagesViewModel { Page = 1, Show = 1 }));
		}

		[Fact]
		public async Task RegistersNewMessage() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			Message message = new Message { Content = "test test test" };
			await service.RegisterMessage(message);

			// Assert
			Assert.Equal(message, await context.Messages.FirstAsync());
		}

		[Fact]
		public async Task RegistersNewMessageForSpecificUser() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			ApplicationUser user = sampleUser;
			context.Users.Add(user);
			await context.SaveChangesAsync();
			Message message = new Message { Content = "test test test" };
			await service.RegisterMessage(message, user);

			// Assert
			Assert.Equal(message, await context.Messages.FirstAsync());
			Assert.Equal(message, (await context.Users.FirstAsync()).Messages.First());
		}

		[Fact]
		public async Task RegistersNewMessageWithSenderAndReceiver() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			ApplicationUser sender = sampleUser,
				receiver = sampleUser;
			context.Users.AddRange(sender, receiver);
			await context.SaveChangesAsync();
			Message message = new Message { Content = "test test test" };
			await service.RegisterMessage(message, sender, receiver);

			// Assert
			Message target = await context.Messages.FirstAsync();
			Assert.Equal(message, target);
			Assert.Equal(sender, target.Sender);
			Assert.Equal(receiver, target.User);
		}

		[Fact]
		public async Task UpdatesMessage() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			string content = "test test test",
				newContent = "changed content";
			Message message = new Message { Content = content };
			context.Messages.Add(message);
			await context.SaveChangesAsync();
			message.Content = newContent;
			await service.UpdateMessage(message);

			// Assert
			Assert.NotEqual(content, (await context.Messages.FirstAsync()).Content);
			Assert.Equal(newContent, (await context.Messages.FirstAsync()).Content);
		}

		[Fact]
		public async Task RemovesSpecifiedMessage() {
			// Arrange
			ApplicationDbContext context = new ApplicationDbContext(DbContextOptions.Options);
			IMessageClient service = new MessageClient(context, new FakeUserManager(context), null);

			// Act
			Message message = new Message { Content = "test test test" };
			context.Messages.Add(message);
			await context.SaveChangesAsync();
			await service.RemoveMessage(message);

			// Assert
			Assert.Empty(context.Messages);
		}
	}
}