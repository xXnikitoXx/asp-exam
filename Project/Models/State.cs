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
		public double CPU { get; set; }
		public double DiskRead { get; set; }
		public double DiskWrite { get; set; }
		public double OperationsRead { get; set; }
		public double OperationsWrite { get; set; }
		public double NetworkIn { get; set; }
		public double NetworkOut { get; set; }
		public double PacketsIn { get; set; }
		public double PacketsOut { get; set; }
		public string VPSId { get; set; }
		public VPS VPS { get; set; }
	}
}
