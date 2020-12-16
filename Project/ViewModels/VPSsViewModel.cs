using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
	public class VPSsViewModel : ListViewModel
	{
		public VPSsViewModel()  => this.VPSs = new List<VPSViewModel>();

		public List<VPSViewModel> VPSs { get; set; }
	}
}
