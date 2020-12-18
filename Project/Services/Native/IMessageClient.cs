using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;
using Project.ViewModels;

namespace Project.Services.Native {
	public interface IMessageClient {
		Task<Message> Find(string id);
		Task<List<Message>> GetMessages(ClaimsPrincipal user);
		Task<List<Message>> GetMessages(ClaimsPrincipal user, MessagesViewModel pageInfo);
		List<Message> GetMessages(MessagesViewModel pageInfo);
		List<Message> GetMessages(ApplicationUser user);
		List<Message> GetMessages(ApplicationUser user, MessagesViewModel pageInfo);
		IQueryable<Message> Answers();
		IQueryable<Message> Answers(ApplicationUser user);
		List<Message> Answers(MessagesViewModel pageInfo);
		Task RegisterMessage(Message message);
		Task RegisterMessage(Message message, ApplicationUser target);
		Task RegisterMessage(Message message, ApplicationUser source, ApplicationUser target);
		Task UpdateMessage(Message message);
		Task RemoveMessage(string id);
		Task RemoveMessage(Message message);
	}
}