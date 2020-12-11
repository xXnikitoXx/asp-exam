using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Enums;

namespace Project.Models
{
	public class Announcement
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Title { get; set; }

		[Required]
		public MessageType Type { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
