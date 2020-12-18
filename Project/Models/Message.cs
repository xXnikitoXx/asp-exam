using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

		public DateTime Time { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public string SenderId { get; set; }
		public ApplicationUser Sender { get; set; }

		public string TicketId { get; set; }
		public Ticket Ticket { get; set; }

		public bool Final { get; set; }

		public string PreviousMessageId { get; set; }
		public Message PreviousMessage { get; set; }
		
		public string ReplyId { get; set; }
		public Message Reply { get; set; }
	}
}
