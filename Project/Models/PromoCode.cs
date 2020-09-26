using Project.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class PromoCode
    {
        public PromoCode()
        {
            this.Plans = new HashSet<PromoCodePlan>();
            this.Users = new HashSet<UserPromoCode>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Code { get; set; }

        [Required]
        public PromoCodeType Type { get; set; }

        [Required]
        public float Value { get; set; }

        [DefaultValue(true)]
        public bool IsValid { get; set; }

        public ICollection<PromoCodePlan> Plans { get; set; }

        public ICollection<UserPromoCode> Users { get; set; }
    }
}
