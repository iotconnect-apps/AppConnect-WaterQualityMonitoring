﻿using System;
using System.Collections.Generic;

namespace iot.solution.entity.Request
{
    public class ChartRequest
    {
        public Guid CompanyGuid { get; set; }
        public Guid Entityguid { get; set; }
        public Guid HardwareKitGuid { get; set; }
    }
}
