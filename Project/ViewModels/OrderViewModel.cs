using Project.Enums;
using Project.Models;
using System;
using System.Collections.Generic;

namespace Project.ViewModels
{
	public class OrderViewModel
	{
		public OrderViewModel() {
			this.PromoCodes = new List<PromoCodeViewModel>();
		}

		public string Id { get; set; }

		public DateTime TimeStarted { get; set; }

		public DateTime TimeFinished { get; set; }

		public byte Amount { get; set; }

		public double OriginalPrice { get; set; }

		public List<PromoCodeViewModel> PromoCodes { get; set; }

		public double FinalPrice { get; set; }

		public int PlanNumber { get; set; }

		public Location Location { get; set; }

		public string UserId { get; set; }

		public string Username { get; set; }

		public PlanViewModel Plan { get; set; }

		public OrderState State { get; set; }
	}
}
