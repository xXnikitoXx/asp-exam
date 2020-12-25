using System.Collections.Generic;
using Project.Enums;
using Project.Models;

namespace Project.ViewModels {
	public class VPSViewModel {
		public VPSViewModel() {
			this.Activities = new List<Activity>();
			this.States = new List<State>();
		}

		public string Id { get; set; }
		public string ExternalId { get; set; }
		public string Name { get; set; }
		public PlanViewModel Plan { get; set; }
		public Location Location { get; set; }
		public string IP { get; set; }
		public byte Cores { get; set; }
		public byte RAM { get; set; }
		public ushort SSD { get; set; }
		public List<Activity> Activities { get; set; }
		public List<State> States { get; set; }
		public string OrderId { get; set; }
		public OrderViewModel Order { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
	}
}
