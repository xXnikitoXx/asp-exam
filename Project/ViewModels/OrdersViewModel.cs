using System.Collections.Generic;

namespace Project.ViewModels {
	public class OrdersViewModel : ListViewModel {
		public OrdersViewModel() =>
			this.Orders = new List<OrderViewModel>();

		public List<OrderViewModel> Orders { get; set; }
		public int CreatedOrders { get; set; }
		public int AwaitingOrders { get; set; }
		public int CancelledOrders { get; set; }
		public int FinishedOrders { get; set; }
		public int FailedOrders { get; set; }
		public int ExpiredOrders { get; set; }
		public double TotalInvestments { get; set; }
	}
}
