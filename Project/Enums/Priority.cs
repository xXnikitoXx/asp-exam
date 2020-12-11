using System.ComponentModel;

namespace Project.Enums
{
	public enum Priority
	{
		[Description("Нисък")]
		Low = 0,
		[Description("Среден")]
		Medium = 1,
		[Description("Висок")]
		High = 2,
	}
}
