using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models {
	public class Plan {
		public Plan() =>
			this.Orders = new HashSet<Order>();

		[Required]
		public int Number { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public byte Cores { get; set; }

		[Required]
		public byte RAM { get; set; }

		[Required]
		public ushort SSD { get; set; }

		[Required]
		public double Price { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
