using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class UserActivity
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public string ActivityId { get; set; }
		public Activity Activity { get; set; }
	}
}
