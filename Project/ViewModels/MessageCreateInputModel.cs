using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project.ViewModels
{
	public class MessageCreateInputModel {
		public string Status { get; set; }
		[Required]
		[StringLength(200, MinimumLength = 5)]
		public string Content { get; set; }
		public string URL { get; set; }
		public string UserId { get; set; }
		public string TicketId { get; set; }
		public bool Final { get; set; }
		public string PreviousMessageId { get; set; }
	}
}
