using System;
using System.Collections.Generic;
using Entity = iot.solution.entity;
using Request = iot.solution.entity.Request;
using Response = iot.solution.entity.Response;

namespace iot.solution.service.Interface
{
    public interface IChartService
    {
        Entity.ActionStatus TelemetrySummary_DayWise();
        Entity.ActionStatus TelemetrySummary_HourWise();
        List<entity.DeviceAttributeChartResponse> GetDeviceAttributeChartData(Guid deviceGuid, string attribute, string frequency);
        List<Response.EnergyUsageResponse> GetEnergyUsage(Request.ChartRequest request);
        List<Response.DeviceUsageResponse> GetDeviceUsage(Request.ChartRequest request);
        List<Response.DeviceBatteryStatusResponse> GetDeviceBatteryStatus(Request.ChartRequest request);
    }
}
