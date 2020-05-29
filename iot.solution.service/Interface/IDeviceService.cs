using System;
using System.Collections.Generic;
using Entity = iot.solution.entity;
using Response = iot.solution.entity.Response;

namespace iot.solution.service.Interface
{
    public interface IDeviceService
    {
        List<Entity.Device> Get();
        Entity.Device Get(Guid id);
        Entity.ActionStatus Manage(Entity.DeviceModel device);
        Entity.ActionStatus Delete(Guid id);
      
        Entity.SearchResult<List<Entity.Device>> List(Entity.SearchRequest request);
        Entity.ActionStatus UpdateStatus(Guid id, bool status);
        Response.DeviceDetailResponse GetDeviceDetail(Guid deviceId);
        List<Response.EntityWiseDeviceResponse> GetEntityWiseDevices(Guid locationId);
        List<Response.EntityWiseDeviceResponse> GetEntityChildDevices(Guid deviceId);
        Entity.BaseResponse<int> ValidateKit(string kitCode);
        Entity.BaseResponse<bool> ProvisionKit(Entity.Device request);
        Entity.BaseResponse<Entity.DeviceCounterResult> GetDeviceCounters();
        Entity.BaseResponse<List<Entity.DeviceLatestAttributeResponse>> GetDeviceLattestAttibuteValues(Guid deviceGuid);
        Entity.BaseResponse<List<Entity.WaterConsumptionResponse>> GetWaterConsumptionChartData(Guid deviceGuid, string frequency);

        Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>> GetDeviceAttributeChartData(Guid deviceGuid, string attributeName, string frequency);
        Entity.BaseResponse<List<Entity.DeviceTelemetryDataResult>> GetTelemetryData(Guid deviceId);
        Entity.BaseResponse<Entity.DeviceConnectionStatusResult> GetConnectionStatus(string uniqueId);


    }
}
