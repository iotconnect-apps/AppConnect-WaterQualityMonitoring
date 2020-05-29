using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace iot.solution.entity
{
    public class DeviceLatestAttributeResponse
    {
        public string UniqueId { get; set; }
        public string Key { get; set; }
        public decimal Value { get; set; }
    }
}
