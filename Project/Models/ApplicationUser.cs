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
			this.VPSs = new HashSet<UserVPS>();
			this.Posts = new HashSet<UserPost>();
			this.Tickets = new HashSet<UserTicket>();
			this.Messages = new HashSet<UserMessage>();
			this.Activities = new HashSet<UserActivity>();
		}

		public ICollection<UserVPS> VPSs { get; set; }
		public ICollection<UserPost> Posts { get; set; }	
		public ICollection<UserTicket> Tickets { get; set; }
		public ICollection<UserMessage> Messages { get; set; }
		public ICollection<UserActivity> Activities { get; set; }
	}
}
