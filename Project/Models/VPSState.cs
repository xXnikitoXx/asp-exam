using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class VPSState
	{
		public string VPSId { get; set; }
		public VPS VPS { get; set; }
		public string StateId { get; set; }
		public State State { get; set; }
	}
}
