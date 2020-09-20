using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
	public class Post
	{
		public Post() {
			this.Answers = new HashSet<Post>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Subject { get; set; }

		[Required]
		public string Message { get; set; }

		public DateTime Time { get; set; }

		public string ParentId { get; set; }
		public Post ParentPost { get; set; }

		public ApplicationUser User { get; set; }

		public ICollection<Post> Answers { get; set; }
	}
}
