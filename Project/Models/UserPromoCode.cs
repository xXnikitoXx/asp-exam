using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserPromoCode
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; }
    }
}
