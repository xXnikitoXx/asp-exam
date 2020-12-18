using System.Collections.Generic;
using System.Linq;
using Project.Models;

namespace Project.Services.Native {
	public interface IUserStatusClient {
		bool IsOnline(string user);
		bool IsOnline(ApplicationUser user);
		List<string> ConnectionsOf(string id);
		List<string> ConnectionsOf(ApplicationUser user);
		int TotalConnectionsOf(string id);
		int TotalConnectionsOf(ApplicationUser user);
		void AddConnection(string userId, string connectionId);
		void AddConnection(ApplicationUser user, string connectionId);
		void RemoveConnection(string connectionId);
		void RemoveConnections(string userId);
		void RemoveConnections(ApplicationUser user);
	}
}