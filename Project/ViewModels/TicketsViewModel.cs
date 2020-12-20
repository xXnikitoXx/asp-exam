using System.Collections.Generic;

namespace Project.ViewModels {
	public class TicketsViewModel : ListViewModel {
		public TicketsViewModel() =>
			this.Tickets = new List<TicketViewModel>();

		public List<TicketViewModel> Tickets { get; set; }
		public int LowPriorityCount { get; set; }
		public int MediumPriorityCount { get; set; }
		public int HighPriorityCount { get; set; }
		public int AnsweredCount { get; set; }
	}
}