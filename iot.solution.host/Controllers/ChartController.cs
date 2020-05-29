using iot.solution.entity.Structs.Routes;
using iot.solution.service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Entity = iot.solution.entity;
using Response = iot.solution.entity.Response;
using Request = iot.solution.entity.Request;

namespace host.iot.solution.Controllers
{
    [Route(ChartRoute.Route.Global)]
    [ApiController]
    public class ChartController : BaseController
    {
        private readonly IChartService _chartService;
        private readonly IDeviceService _deviceService;
        
        public ChartController(IChartService chartService,IDeviceService deviceService)
        {
            _chartService = chartService;
            _deviceService = deviceService;
        }
        //[HttpPost]
        //[Route(ChartRoute.Route.DeviceUsage, Name = ChartRoute.Name.DeviceUsage)]
        //public Entity.BaseResponse<List<Response.DeviceUsageResponse>> DeviceUsage(Request.ChartRequest request)
        //{
        //    Entity.BaseResponse<List<Response.DeviceUsageResponse>> response = new Entity.BaseResponse<List<Response.DeviceUsageResponse>>(true);
        //    response.Data = _chartService.GetDeviceUsage(request);
        //    return response;
        //}
        //[HttpPost]
        //[Route(ChartRoute.Route.EnergyUsage, Name = ChartRoute.Name.EnergyUsage)]
        //public Entity.BaseResponse<List<Response.EnergyUsageResponse>> EnergyUsage(Request.ChartRequest request)
        //{
        //    Entity.BaseResponse<List<Response.EnergyUsageResponse>> response = new Entity.BaseResponse<List<Response.EnergyUsageResponse>>(true);
        //    response.Data = _chartService.GetEnergyUsage(request);
        //    return response;
        //}

        [HttpGet]
        [Route(ChartRoute.Route.DeviceAttributeChartData, Name = ChartRoute.Name.DeviceAttributeChartData)]
        public Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>> GetDeviceAttributeChartData(Guid deviceGuid,string attributeName,string frequency)
        {
            try
            {
                return _deviceService.GetDeviceAttributeChartData(deviceGuid, attributeName, frequency);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>>(false, ex.Message);
            }
            
        }

        [HttpGet]
        [Route(ChartRoute.Route.WaterConsumptionChartData, Name = ChartRoute.Name.WaterConsumptionChartData)]
        public Entity.BaseResponse<List<Entity.WaterConsumptionResponse>> WaterConsumptionChartData(Guid deviceGuid,string frequency)
        {
            try
            {
                return _deviceService.GetWaterConsumptionChartData(deviceGuid, frequency);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Entity.WaterConsumptionResponse>>(false, ex.Message);
            }          
           
        }
    }
}