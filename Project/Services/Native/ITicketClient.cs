using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public interface ITicketClient {
		Task<Ticket> Find(string id);
		Task<List<Ticket>> GetTickets(ClaimsPrincipal user);
		Task<List<Ticket>> GetTickets(ClaimsPrincipal user, TicketsViewModel pageInfo);
		List<Ticket> GetTickets(TicketsViewModel pageInfo);
		List<Ticket> GetTickets(ApplicationUser user);
		List<Ticket> GetTickets(ApplicationUser user, TicketsViewModel pageInfo);
		IQueryable<Ticket> AnsweredTickets();
		IQueryable<Ticket> AnsweredTickets(ApplicationUser user);
		List<Ticket> AnsweredTickets(TicketsViewModel pageInfo);
		Task RegisterTicket(Ticket ticket);
		Task UpdateTicket(Ticket ticket);
		Task RemoveTicket(string id);
		Task RemoveTicket(Ticket ticket);
	}
}