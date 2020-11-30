using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
	public class OrdersViewModel
	{
		public List<OrderViewModel> Orders { get; set; }

		public int Page { get; set; }

		public int Pages { get; set; }

		public int Total { get; set; }

		public int Show { get; set; }

		public double TotalInvestments { get; set; }
	}
}
