using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class UserMessage
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public string MessageId { get; set; }
		public Message Message { get; set; }
	}
}
