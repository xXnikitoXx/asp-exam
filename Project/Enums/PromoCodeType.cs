using System.ComponentModel;

namespace Project.Enums
{
    public enum PromoCodeType
    {
        [Description("Определена стойност")]
        FixedAmount = 0,
        [Description("Процент")]
        Percentage = 1,
        [Description("Презаписване на стойност")]
        PriceOverride = 2,
        [Description("Безплатно")]
        Free = 3,
    }
}
