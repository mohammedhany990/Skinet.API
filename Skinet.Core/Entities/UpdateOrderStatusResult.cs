﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Entities
{
    public enum UpdateOrderStatusResult
    {
        Success,
        OrderNotFound,
        InvalidStatusTransition,
        InvalidStatus,
        SaveFailed
    }
}
