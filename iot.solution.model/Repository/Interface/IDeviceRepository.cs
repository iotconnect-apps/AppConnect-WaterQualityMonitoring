using iot.solution.entity;
using System;
using System.Collections.Generic;
using Entity = iot.solution.entity;
using Model = iot.solution.model.Models;
using Response = iot.solution.entity.Response;

namespace iot.solution.model.Repository.Interface
{
    public interface IDeviceRepository : IGenericRepository<Model.Device>
    {
        Model.Device Get(string device);
        Entity.ActionStatus Manage(Model.Device request);
        Entity.ActionStatus Delete(Guid id);
        Entity.SearchResult<List<Entity.DeviceDetail>> List(Entity.SearchRequest request);
        List<Response.EntityWiseDeviceResponse> GetEntityWiseDevices(Guid? locationId, Guid? deviceId);
        Entity.BaseResponse<int> ValidateKit(string kitCode);
        Entity.BaseResponse<List<Entity.HardwareKit>> ProvisionKit(Entity.ProvisionKitRequest request);
        Entity.BaseResponse<List<Entity.DeviceLatestAttributeResponse>> GetLatestAttributesValue(Guid deviceGuid);
        Entity.BaseResponse<List<Entity.WaterConsumptionResponse>> GetWaterConsumptionChartData(Guid deviceGuid, string frequency);
        Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>> GetDeviceAttributeChartData(Guid deviceGuid, string attributeName, string frequency);


        List<LookupItem> GetDeviceLookup();
    }
}
