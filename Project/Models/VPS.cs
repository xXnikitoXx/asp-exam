using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Project.Enums;

namespace Project.Models
{
	public class VPS
	{
		public VPS() {
			this.Activities = new HashSet<VPSActivity>();
			this.States = new HashSet<VPSState>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 3)]
		public string Name { get; set; }

		[Required]
		public Plan Plan { get; set; }

		[Required]
		public Location Location { get; set; }
		
		[Required]
		[StringLength(15, MinimumLength = 7)]
		public string IP { get; set; }

		[Required]
		public byte Cores { get; set; }

		[Required]
		public byte RAM { get; set; }

		[Required]
		public byte SSD { get; set; }

		public ApplicationUser User { get; set; }

		public ICollection<VPSActivity> Activities { get; set; }
		
		public ICollection<VPSState> States { get; set; }
	}
}
