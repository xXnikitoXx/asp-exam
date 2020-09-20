using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Project.Enums;

namespace Project.Models
{
	public class Message
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public MessageStatus Status { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 5)]
		public string Content { get; set; }

		public string URL { get; set; }

		public ApplicationUser User { get; set; }
	}
}
