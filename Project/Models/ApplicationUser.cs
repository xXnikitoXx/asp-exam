using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Project.Models {
	public class ApplicationUser : IdentityUser {
		public ApplicationUser() {
			this.VPSs = new HashSet<VPS>();
			this.Posts = new HashSet<Post>();
			this.Tickets = new HashSet<Ticket>();
			this.Messages = new HashSet<Message>();
			this.SentMessages = new HashSet<Message>();
			this.Activities = new HashSet<Activity>();
			this.Orders = new HashSet<Order>();
			this.PromoCodes = new HashSet<UserPromoCode>();
			this.Payments = new HashSet<Payment>();
		}

		public DateTime JoinDate { get; set; }
		public DateTime LastLoginDate { get; set; }
		public ICollection<VPS> VPSs { get; set; }
		public ICollection<Post> Posts { get; set; }	
		public ICollection<Ticket> Tickets { get; set; }
		public ICollection<Message> Messages { get; set; }
		public ICollection<Message> SentMessages { get; set; }
		public ICollection<Activity> Activities { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<UserPromoCode> PromoCodes { get; set; }
		public ICollection<Payment> Payments { get; set; }
	}
}
