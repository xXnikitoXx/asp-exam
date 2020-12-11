using Project.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class Order
	{
		public Order() {
			this.VPSs = new HashSet<VPS>();
			this.PromoCodes = new HashSet<PromoCodeOrder>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		public DateTime TimeStarted { get; set; }

		public DateTime TimeFinished { get; set; }

		[Required]
		public byte Amount { get; set; }

		[Required]
		public double OriginalPrice { get; set; }

		[Required]
		public double FinalPrice { get; set; }

		public int PlanNumber { get; set; }

		public Models.Plan Plan { get; set; }

		[DefaultValue(Location.Nuremberg_Germany)]
		public Location Location { get; set; }

		[Required]
		public OrderState State { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public ICollection<VPS> VPSs { get; set; }

		public ICollection<PromoCodeOrder> PromoCodes { get; set; }
	}
}
