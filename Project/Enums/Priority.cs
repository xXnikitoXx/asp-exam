using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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
