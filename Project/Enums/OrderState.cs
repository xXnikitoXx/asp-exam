using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Enums
{
    public enum OrderState
    {
        Created = 0,
        Awaiting = 1,
        Cancelled = 2,
        Finished = 3,
        Failed = 4,
    }
}
