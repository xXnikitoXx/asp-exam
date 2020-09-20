using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class UserPost
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public string PostId { get; set; }
		public Post Post { get; set; }
	}
}
