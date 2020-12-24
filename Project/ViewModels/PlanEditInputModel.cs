using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels {
	public class PlanEditInputModel {
		[Required]
		public int Number { get; set; }
		
		[Required]
		public string Name { get; set; }

		[Required]
		public byte Cores { get; set; }

		[Required]
		public byte RAM { get; set; }

		[Required]
		public ushort SSD { get; set; }

		[Required]
		public double Price { get; set; }
	}
}