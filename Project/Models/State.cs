using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Project.Enums;

namespace Project.Models
{
	public class State
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public ServerStatus Status { get; set; }

		public DateTime Time { get; set; }

		public float CPU { get; set; }

		public float RAM { get; set; }

		public VPS VPS { get; set; }
	}
}
