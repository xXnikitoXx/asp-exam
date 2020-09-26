using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Plan
    {
		[Required]
		public int Number { get; set; }

        [Required]
        public string Name { get; set; }

		[Required]
		[StringLength(15, MinimumLength = 7)]
		public string IP { get; set; }

		[Required]
		public byte Cores { get; set; }

		[Required]
		public byte RAM { get; set; }

		[Required]
		public ushort SSD { get; set; }
	}
}
