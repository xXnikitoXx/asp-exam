using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Enums
{
    public enum PromoCodeType
    {
        [Description("Определена стойност")]
        FixedAmount = 0,
        [Description("Процент")]
        Precentage = 1,
        [Description("Презаписване на стойност")]
        PriceOverride = 2,
        [Description("Безплатно")]
        Free = 3,
    }
}
