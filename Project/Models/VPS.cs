using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Enums;

namespace Project.Models {
	public class VPS {
		public VPS() {
			this.Activities = new HashSet<Activity>();
			this.States = new HashSet<State>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 3)]
		public string Name { get; set; }

		[Required]
		public Location Location { get; set; }

		[Required]
		public byte Cores { get; set; }

		[Required]
		public byte RAM { get; set; }

		[Required]
		public ushort SSD { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public ICollection<Activity> Activities { get; set; }
		public ICollection<State> States { get; set; }
		public string OrderId { get; set; }
		public Order Order { get; set; }
		public string ServerDataId { get; set; }
		public ServerData ServerData { get; set; }
	}
}
