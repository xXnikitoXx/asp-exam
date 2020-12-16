using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
	public class AccountViewModel
	{
		public AccountViewModel() {
			this.Orders = new List<OrderViewModel>();
		}

		public int VPSCount { get; set; }
		public int OrdersCount { get; set; }
		public int TicketsCount { get; set; }
		public int PromoCodesCount { get; set; }
		public int CreatedOrders { get; set; }
		public int AwaitingOrders { get; set; }
		public int CancelledOrders { get; set; }
		public int FinishedOrders { get; set; }
		public int FailedOrders { get; set; }
		public int ExpiredOrders { get; set; }
		public List<OrderViewModel> Orders { get; set; }
		public double TotalInvestments { get; set; }
		public double MonthlyBill { get; set; }
	}
}
