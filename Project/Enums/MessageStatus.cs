using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Enums
{
	public enum MessageStatus
	{
		[Description("")]
		None = 0,
		[Description("Информация")]
		Info = 1,
		[Description("Успешна операция")]
		Success = 2,
		[Description("Внимание")]
		Warning = 3,
		[Description("Грешка")]
		Error = 4,
	}
}
