using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class PromoCodePlan
    {
        public string PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; }
        public int PlanNumber { get; set; }
    }
}
