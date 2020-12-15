using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Enums;

namespace Project.Models
{
	public class Ticket
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Subject { get; set; }

		[Required]
		public string Message { get; set; }

		[Required]
		public Priority Priority { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public string AnswerId { get; set; }
		public Message Answer { get; set; }
	}
}
