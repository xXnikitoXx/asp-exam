using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Enums;

namespace Project.Models {
	public class State {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }
		public ServerStatus Status { get; set; }
		public DateTime Time { get; set; }
		public float CPU { get; set; }
		public float RAM { get; set; }
		public string VPSId { get; set; }
		public VPS VPS { get; set; }
	}
}
