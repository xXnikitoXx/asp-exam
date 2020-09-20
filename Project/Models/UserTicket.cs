﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class UserTicket
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public string TicketId { get; set; }
		public Ticket Ticket { get; set; }
	}
}
