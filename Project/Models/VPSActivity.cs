using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class VPSActivity
	{
		public string VPSId { get; set; }
		public VPS VPS { get; set; }
		public string ActivityId { get; set; }
		public Activity Activity { get; set; }
	}
}
