using System.Collections.Generic;
using Project.Enums;
using Project.Models;

namespace Project.ViewModels {
	public class VPSViewModel {
		public VPSViewModel() {
			this.Activities = new List<ActivityViewModel>();
			this.States = new List<StateViewModel>();
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public PlanViewModel Plan { get; set; }
		public Location Location { get; set; }
		public byte Cores { get; set; }
		public byte RAM { get; set; }
		public ushort SSD { get; set; }
		public List<ActivityViewModel> Activities { get; set; }
		public List<StateViewModel> States { get; set; }
		public string OrderId { get; set; }
		public OrderViewModel Order { get; set; }
		public string ServerDataId { get; set; }
		public ServerDataViewModel ServerData { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
	}
}
