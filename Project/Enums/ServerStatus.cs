using System.ComponentModel;

namespace Project.Enums
{
	public enum ServerStatus
	{
		[Description("На линия")]
		Online = 0,
		[Description("Извън линия")]
		Offline = 1,
		[Description("Неочакван срив")]
		Error = 2,
		[Description("В ремонт")]
		Maintenance = 3,
	}
}
