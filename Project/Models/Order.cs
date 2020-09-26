using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Order
    {
        public Order()
        {
            this.VPSs = new HashSet<VPS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public DateTime TimeStarted { get; set; }

        public DateTime TimeFinished { get; set; }

        [Required]
        public byte Amount { get; set; }

        [Required]
        public double OriginalPrice { get; set; }

        [Required]
        public double FinalPrice { get; set; }

        [Required]
        public Enums.Plan Plan { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<VPS> VPSs { get; set; }
    }
}
