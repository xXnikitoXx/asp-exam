using System.ComponentModel;

namespace Project.Enums
{
	public enum OrderState
	{
		[Description("Записана")]
		Created = 0,
		[Description("В процес на обработка")]
		Awaiting = 1,
		[Description("Отказана")]
		Cancelled = 2,
		[Description("Завършена")]
		Finished = 3,
		[Description("Провалена")]
		Failed = 4,
		[Description("Изтекла")]
		Expired = 5,
	}
}
