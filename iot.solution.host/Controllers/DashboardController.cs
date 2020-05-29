using iot.solution.entity.Structs.Routes;
using iot.solution.service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Entity = iot.solution.entity;
using Response = iot.solution.entity.Response;

namespace host.iot.solution.Controllers
{
    [Route(DashboardRoute.Route.Global)]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _service;
        private readonly IEntityService _entityService;
        private readonly IDeviceService _deviceService;

        public DashboardController(IDashboardService dashboardService, IEntityService entityService, IDeviceService deviceService)
        {
            _service = dashboardService;
            _entityService = entityService;
            _deviceService = deviceService;
        }

        [HttpGet]
        [Route(DashboardRoute.Route.GetEntity, Name = DashboardRoute.Name.GetEntity)]
        public Entity.BaseResponse<List<Entity.LookupItem>> GetEntities(Guid companyId)
        {
            Entity.BaseResponse<List<Entity.LookupItem>> response = new Entity.BaseResponse<List<Entity.LookupItem>>(true);
            try
            {
                response.Data = _service.GetEntityLookup(companyId);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Entity.LookupItem>>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(DashboardRoute.Route.GetOverview, Name = DashboardRoute.Name.GetOverview)]
        public Entity.BaseResponse<Entity.DashboardOverviewResponse> GetOverview(Guid companyId)
        {
            Entity.BaseResponse<Entity.DashboardOverviewResponse> response = new Entity.BaseResponse<Entity.DashboardOverviewResponse>(true);
            try
            {
                response.Data = _service.GetOverview();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.DashboardOverviewResponse>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(DashboardRoute.Route.GetEntityDetail, Name = DashboardRoute.Name.GetEntityDetail)]
        public Entity.BaseResponse<Response.EntityDetailResponse> GetEntityDetail(Guid entityId)
        {
            if (entityId == null || entityId == Guid.Empty)
            {
                return new Entity.BaseResponse<Response.EntityDetailResponse>(false, "Invalid Request");
            }

            Entity.BaseResponse<Response.EntityDetailResponse> response = new Entity.BaseResponse<Response.EntityDetailResponse>(true);
            try
            {
                response.Data = _entityService.GetEntityDetail(entityId);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Response.EntityDetailResponse>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(DashboardRoute.Route.GetEntityDevices, Name = DashboardRoute.Name.GetEntityDevices)]
        public Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>> GetEntityDevices(Guid entityId)
        {
            if (entityId == null || entityId == Guid.Empty)
            {
                return new Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>>(false, "Invalid Request");
            }

            Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>> response = new Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>>(true);
            try
            {
                response.Data = _deviceService.GetEntityWiseDevices(entityId);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(DashboardRoute.Route.GetEntityChildDevices, Name = DashboardRoute.Name.GetEntityChildDevices)]
        public Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>> GetEntityChildDevices(Guid deviceId)
        {
            if (deviceId == null || deviceId == Guid.Empty)
            {
                return new Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>>(false, "Invalid Request");
            }

            Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>> response = new Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>>(true);
            try
            {
                response.Data = _deviceService.GetEntityChildDevices(deviceId);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Response.EntityWiseDeviceResponse>>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(DashboardRoute.Route.GetDeviceDetail, Name = DashboardRoute.Name.GetDeviceDetail)]
        public Entity.BaseResponse<Response.DeviceDetailResponse> GetDeviceDetail(Guid deviceId)
        {
            if (deviceId == null || deviceId == Guid.Empty)
            {
                return new Entity.BaseResponse<Response.DeviceDetailResponse>(false, "Invalid Request");
            }

            Entity.BaseResponse<Response.DeviceDetailResponse> response = new Entity.BaseResponse<Response.DeviceDetailResponse>(true);
            try
            {
                response.Data = _deviceService.GetDeviceDetail(deviceId);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Response.DeviceDetailResponse>(false, ex.Message);
            }
            return response;
        }
    }
}