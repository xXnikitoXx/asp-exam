using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Enums;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public class MessageClient : IMessageClient {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITicketClient _ticketService;

		public MessageClient (
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			ITicketClient ticketService
		) {
			this._context = context;
			this._userManager = userManager;
			this._ticketService = ticketService;
		}

		public async Task<Message> Find(string id) =>
			await this._context.Messages.FirstOrDefaultAsync(message => message.Id == id);

		public IQueryable<Message> Answers() =>
			this._context.Messages.Where(message => message.TicketId != null);

		public IQueryable<Message> Answers(ApplicationUser user) =>
			this._context.Messages.Where(message => message.UserId == user.Id);

		public List<Message> Answers(MessagesViewModel pageInfo) {
			IQueryable<Message> messages = Answers();
			pageInfo.Total = messages.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.NoneOfATypeCount = messages.Where(message => message.Status == MessageStatus.None).Count();
			pageInfo.InfoCount = messages.Where(message => message.Status == MessageStatus.Info).Count();
			pageInfo.SuccessCount = messages.Where(message => message.Status == MessageStatus.Success).Count();
			pageInfo.WarningCount = messages.Where(message => message.Status == MessageStatus.Warning).Count();
			pageInfo.ErrorCount = messages.Where(message => message.Status == MessageStatus.Error).Count();
			return messages.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<Message> GetMessages(MessagesViewModel pageInfo) {
			IQueryable<Message> messages = this._context.Messages.AsQueryable();
			pageInfo.Total = messages.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.NoneOfATypeCount = messages.Where(message => message.Status == MessageStatus.None).Count();
			pageInfo.InfoCount = messages.Where(message => message.Status == MessageStatus.Info).Count();
			pageInfo.SuccessCount = messages.Where(message => message.Status == MessageStatus.Success).Count();
			pageInfo.WarningCount = messages.Where(message => message.Status == MessageStatus.Warning).Count();
			pageInfo.ErrorCount = messages.Where(message => message.Status == MessageStatus.Error).Count();
			return messages.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<Message> GetMessages(ApplicationUser user) =>
			this._context.Messages.Where(message => message.UserId == user.Id).ToList();

		public async Task<List<Message>> GetMessages(ClaimsPrincipal user) =>
			GetMessages(await this._userManager.GetUserAsync(user));

		public List<Message> GetMessages(ApplicationUser user, MessagesViewModel pageInfo) {
			IQueryable<Message> messages = this._context.Messages.Where(message => message.UserId == user.Id);
			pageInfo.Total = messages.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.NoneOfATypeCount = messages.Where(message => message.Status == MessageStatus.None).Count();
			pageInfo.InfoCount = messages.Where(message => message.Status == MessageStatus.Info).Count();
			pageInfo.SuccessCount = messages.Where(message => message.Status == MessageStatus.Success).Count();
			pageInfo.WarningCount = messages.Where(message => message.Status == MessageStatus.Warning).Count();
			pageInfo.ErrorCount = messages.Where(message => message.Status == MessageStatus.Error).Count();
			return messages.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task<List<Message>> GetMessages(ClaimsPrincipal user, MessagesViewModel pageInfo) =>
			GetMessages(await this._userManager.GetUserAsync(user), pageInfo);

		public async Task RegisterMessage(Message message) {
			this._context.Messages.Add(message);
			await this._context.SaveChangesAsync();
		}

		public async Task RegisterMessage(Message message, ApplicationUser target) {
			message.User = target;
			target.Messages.Add(message);
			this._context.Messages.Add(message);
			await this._context.SaveChangesAsync();
		}

		public async Task RegisterMessage(Message message, ApplicationUser source, ApplicationUser target) {
			target.Messages.Add(message);
			source.SentMessages.Add(message);
			message.User = target;
			message.Sender = source;
			this._context.Messages.Add(message);
			await this._context.SaveChangesAsync();
		}

		public async Task UpdateMessage(Message message) {
			Message target = this._context.Messages.FirstOrDefault(msg => msg.Id == message.Id);
			if (target == null)
				throw new Exception();
			target = message;
			await this._context.SaveChangesAsync();
		}

		public async Task RemoveMessage(string id) =>
			await RemoveMessage(await Find(id));

		public async Task RemoveMessage(Message message) {
			this._context.Messages.Remove(message);
			await this._context.SaveChangesAsync();
		}
	}
}