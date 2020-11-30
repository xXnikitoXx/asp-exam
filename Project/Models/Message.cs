﻿using System;
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

		public ApplicationUser User { get; set; }
	}
}
