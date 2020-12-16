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
	public class TicketClient : ITicketClient {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public TicketClient (
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager
		) {
			this._context = context;
			this._userManager = userManager;
		}

		public async Task<Ticket> Find(string id) =>
			await this._context.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == id);

		public IQueryable<Ticket> AnsweredTickets() =>
			this._context.Tickets.Where(ticket => ticket.AnswerId != null);

		public IQueryable<Ticket> AnsweredTickets(ApplicationUser user) =>
			this._context.Tickets.Where(ticket => ticket.AnswerId != null && ticket.UserId == user.Id);

		public List<Ticket> AnsweredTickets(TicketsViewModel pageInfo) {
			IQueryable<Ticket> tickets = AnsweredTickets();
			pageInfo.Total = tickets.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.LowPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.Low);
			pageInfo.MediumPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.Medium);
			pageInfo.HighPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.High);
			pageInfo.AnsweredCount = tickets.Count(ticket => ticket.AnswerId != null);
			return tickets.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public List<Ticket> GetTickets(ApplicationUser user) =>
			this._context.Tickets.Where(ticket => ticket.UserId == user.Id)
			.ToList();

		public async Task<List<Ticket>> GetTickets(ClaimsPrincipal user) =>
			GetTickets(await this._userManager.GetUserAsync(user));

		public List<Ticket> GetTickets(ApplicationUser user, TicketsViewModel pageInfo) {
			IQueryable<Ticket> tickets = this._context.Tickets.Where(ticket => ticket.UserId == user.Id);
			pageInfo.Total = tickets.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.LowPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.Low);
			pageInfo.MediumPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.Medium);
			pageInfo.HighPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.High);
			pageInfo.AnsweredCount = tickets.Count(ticket => ticket.AnswerId != null);
			return tickets.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task<List<Ticket>> GetTickets(ClaimsPrincipal user, TicketsViewModel pageInfo) =>
			GetTickets(await this._userManager.GetUserAsync(user), pageInfo);

		public List<Ticket> GetTickets(TicketsViewModel pageInfo) {
			IQueryable<Ticket> tickets = this._context.Tickets.AsQueryable();
			pageInfo.Total = tickets.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			pageInfo.LowPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.Low);
			pageInfo.MediumPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.Medium);
			pageInfo.HighPriorityCount = tickets.Count(ticket => ticket.Priority == Priority.High);
			pageInfo.AnsweredCount = tickets.Count(ticket => ticket.AnswerId != null);
			return tickets.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public async Task RegisterTicket(Ticket ticket) {
			this._context.Tickets.Add(ticket);
			await this._context.SaveChangesAsync();
		}

		public async Task UpdateTicket(Ticket ticket) {
			Ticket target = await Find(ticket.Id);
			target = ticket;
			await this._context.SaveChangesAsync();
		}

		public async Task RemoveTicket(Ticket ticket) {
			ApplicationUser user = this._context.Users.FirstOrDefault(user => user.Id == ticket.UserId);
			if (user == null)
				throw new Exception();
			if (ticket.AnswerId != null)
				user.Messages.Remove(ticket.Answer);
			user.Tickets.Remove(ticket);
			this._context.Messages.Remove(ticket.Answer);
			this._context.Tickets.Remove(ticket);
			await this._context.SaveChangesAsync();
		}

		public async Task RemoveTicket(string id) =>
			await RemoveTicket(await Find(id));
	}
}