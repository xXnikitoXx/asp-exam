using System.Collections.Generic;

namespace Project.ViewModels {
	public class MessagesViewModel : ListViewModel {
		public MessagesViewModel() => this.Messages = new List<MessageViewModel>();

		public List<MessageViewModel> Messages { get; set; }

		public int NoneOfATypeCount { get; set; }
		public int InfoCount { get; set; }
		public int SuccessCount { get; set; }
		public int WarningCount { get; set; }
		public int ErrorCount { get; set; }
	}
}