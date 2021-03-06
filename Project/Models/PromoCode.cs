using Project.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models {
	public class PromoCode {
		public PromoCode() {
			this.Orders = new HashSet<PromoCodeOrder>();
			this.Users = new HashSet<UserPromoCode>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 5)]
		public string Code { get; set; }

		[Required]
		public PromoCodeType Type { get; set; }

		[Required]
		public float Value { get; set; }

		[DefaultValue(true)]
		public bool IsValid { get; set; }
		public ICollection<PromoCodeOrder> Orders { get; set; }
		public ICollection<UserPromoCode> Users { get; set; }
	}
}
