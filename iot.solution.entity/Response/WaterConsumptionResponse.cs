using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity
{
    public class WaterConsumptionResponse
    {
        public string Name { get; set; }
        public string UniqueId { get; set; }
        public decimal WaterConsumption { get; set; }

    }
}
