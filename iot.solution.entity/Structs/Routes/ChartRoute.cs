using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity.Structs.Routes
{
    public class ChartRoute
    {
        public struct Name
        {
            public const string DeviceUsage = "chart.deviceusage";
            public const string EnergyUsage = "chart.energyusage";
            public const string DeviceAttributeChartData = "chart.deviceattributechartdata";
            public const string WaterConsumptionChartData = "chart.waterconsumptionchartdata";
        }

        public struct Route
        {
            public const string Global = "api/chart";
            public const string DeviceUsage = "getdeviceusage";
            public const string EnergyUsage = "getenergyusage";
            public const string DeviceAttributeChartData = "deviceattributechartdata/{deviceGuid}/{attributeName}/{frequency}";
            public const string WaterConsumptionChartData = "getwaterconsumptionchartdata/{deviceGuid}/{frequency}";
        }
    }
}
