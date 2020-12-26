using System.Collections.Generic;
using System.Linq;
using Project.Models;

namespace Project.Services.Native {
	public class UserStatusClient : IUserStatusClient {
		private readonly Dictionary<string, HashSet<string>> statusRegistrar;

		public UserStatusClient() =>
			statusRegistrar = new Dictionary<string, HashSet<string>>();

		public bool IsOnline(string user) =>
			this.statusRegistrar.ContainsKey(user);
		public bool IsOnline(ApplicationUser user) =>
			IsOnline(user.Id);

		public List<string> ConnectionsOf(string id) =>
			this.statusRegistrar.ContainsKey(id) ? this.statusRegistrar[id].ToList() : new List<string>();
		public List<string> ConnectionsOf(ApplicationUser user) =>
			ConnectionsOf(user.Id);

		public int TotalConnectionsOf(string id) =>
			this.statusRegistrar.ContainsKey(id) ? this.statusRegistrar[id].Count : 0;
		public int TotalConnectionsOf(ApplicationUser user) =>
			TotalConnectionsOf(user.Id);

		public void AddConnection(string userId, string connectionId) {
			if (this.statusRegistrar.ContainsKey(userId))
				this.statusRegistrar[userId].Add(connectionId);
			else
				this.statusRegistrar.Add(userId, new HashSet<string> { connectionId });
		}
		public void AddConnection(ApplicationUser user, string connectionId) =>
			AddConnection(user.Id, connectionId);

		public void RemoveConnection(string connectionId) {
			foreach (HashSet<string> connections in this.statusRegistrar.Values)
				connections.Remove(connectionId);
		}

		public void RemoveConnections(string userId) =>
			this.statusRegistrar.Remove(userId);
		public void RemoveConnections(ApplicationUser user) =>
			RemoveConnections(user.Id);
	}
}