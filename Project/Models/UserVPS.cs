using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class UserVPS
	{
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public string VPSId { get; set; }
		public VPS VPS { get; set; }
	}
}
