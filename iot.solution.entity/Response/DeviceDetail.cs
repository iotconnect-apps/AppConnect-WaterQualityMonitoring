using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity
{
    public partial class DeviceDetail : Device
    {
        public string EntityName { get; set; }
        public string SubEntityName { get; set; }
    }
}
