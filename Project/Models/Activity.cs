using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
	public class Activity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Message { get; set; }

		public string URL { get; set; }

		public DateTime Time { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public string VPSId { get; set; }
		public VPS VPS { get; set; }
	}
}
