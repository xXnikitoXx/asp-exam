using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models {
	public class Payment {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		public DateTime Time { get; set; }

		public string PayPalPayment { get; set; }
		public string PayPalPayer { get; set; }

		public string OrderId { get; set; }
		public Order Order { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}