using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Enums;
using Project.Models;

namespace Project.ViewModels
{
	public class VPSViewModel
	{
		public string Name { get; set; }

		public Enums.Plan Plan { get; set; }

		public Location Location { get; set; }

		public string IP { get; set; }

		public byte Cores { get; set; }

		public byte RAM { get; set; }

		public ushort SSD { get; set; }

		public List<Activity> Activities { get; set; }
		
		public List<State> States { get; set; }

		public string OrderId { get; set; }
	}
}
