using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels {
	public class OrderEditInputModel {
		public OrderEditInputModel() =>
			this.Codes = new List<string>();
		
		[DefaultValue(1)]
		[Range(1, 5)]
		public byte Amount { get; set; }

		[DefaultValue("Nuremberg_Germany")]
		public string Location { get; set; }
		public List<string> Codes { get; set; }
		public int ProductId { get; set; }
		public bool New { get; set; }
		public string OrderId{ get; set; }
	}
}