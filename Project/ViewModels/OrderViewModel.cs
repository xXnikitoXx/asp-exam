using Project.Enums;
using System;

namespace Project.ViewModels
{
	public class OrderViewModel
	{
		public string Id { get; set; }

		public DateTime TimeStarted { get; set; }

		public DateTime TimeFinished { get; set; }

		public byte Amount { get; set; }

		public double OriginalPrice { get; set; }

		public double FinalPrice { get; set; }

		public Models.Plan Plan { get; set; }

		public OrderState State { get; set; }
	}
}
