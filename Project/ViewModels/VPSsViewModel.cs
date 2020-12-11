using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
	public class VPSsViewModel
	{
		public VPSsViewModel() {
			this.VPSs = new List<VPSViewModel>();
		}

		public List<VPSViewModel> VPSs { get; set; }

		public int Page { get; set; }

		public int Pages { get; set; }

		public int Total { get; set; }

		public int Show { get; set; }
	}
}
