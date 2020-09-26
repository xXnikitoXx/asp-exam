using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class ApplicationUser : IdentityUser
	{
		public ApplicationUser() {
			this.VPSs = new HashSet<VPS>();
			this.Posts = new HashSet<Post>();
			this.Tickets = new HashSet<Ticket>();
			this.Messages = new HashSet<Message>();
			this.Activities = new HashSet<Activity>();
			this.Orders = new HashSet<Order>();
			this.PromoCodes = new HashSet<UserPromoCode>();
		}

		public ICollection<VPS> VPSs { get; set; }
		public ICollection<Post> Posts { get; set; }	
		public ICollection<Ticket> Tickets { get; set; }
		public ICollection<Message> Messages { get; set; }
		public ICollection<Activity> Activities { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<UserPromoCode> PromoCodes { get; set; }
	}
}
