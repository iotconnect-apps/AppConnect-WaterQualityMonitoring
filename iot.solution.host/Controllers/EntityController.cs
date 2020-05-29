using iot.solution.entity.Structs.Routes;
using iot.solution.service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Entity = iot.solution.entity;

namespace host.iot.solution.Controllers
{
    [Route(EntityRoute.Route.Global)]
    [ApiController]
    public class EntityController : BaseController
    {
        private readonly IEntityService _service;
        private readonly IDeviceService _deviceService;
        private readonly ILookupService _lookupService;

        public EntityController(IEntityService locationService, IDeviceService deviceService, ILookupService lookupService)
        {
            _service = locationService;
            _deviceService = deviceService;
            _lookupService = lookupService;
        }

        [HttpGet]
        [Route(EntityRoute.Route.GetList, Name = EntityRoute.Name.GetList)]
        public Entity.BaseResponse<List<Entity.EntityWithCounts>> Get()
        {
            Entity.BaseResponse<List<Entity.EntityWithCounts>> response = new Entity.BaseResponse<List<Entity.EntityWithCounts>>(true);
            try
            {
                response.Data = _service.Get();

                //foreach (var item in response.Data)
                //{
                //    item.TotalDevices = 11;
                //    //item.TotalUsers = 15;
                //}
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Entity.EntityWithCounts>>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(EntityRoute.Route.GetById, Name = EntityRoute.Name.GetById)]
        public Entity.BaseResponse<Entity.Entity> Get(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return new Entity.BaseResponse<Entity.Entity>(false, "Invalid Request");
            }

            Entity.BaseResponse<Entity.Entity> response = new Entity.BaseResponse<Entity.Entity>(true);
            try
            {
                response.Data = _service.Get(id);


               // response.Data.EntityDetails = null;// _service.GetEntityDetail(id);
                
                response.Data.Devices = _deviceService.GetEntityWiseDevices(id);
                
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.Entity>(false, ex.Message);
            }
            return response;
        }

        [HttpPost]
        [Route(EntityRoute.Route.Manage, Name = EntityRoute.Name.Add)]
        public Entity.BaseResponse<Entity.Entity> Manage([FromForm]Entity.EntityModel request)
        {

            Entity.BaseResponse<Entity.Entity> response = new Entity.BaseResponse<Entity.Entity>(false);
            try
            {

                var status = _service.Manage(request);
                if(status.Success)
                {
                    response.IsSuccess = status.Success;
                    response.Message = status.Message;
                    response.Data = status.Data;
                }
                else
                {
                    response.IsSuccess = status.Success;
                    response.Message = status.Message;
                }
               
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.Entity>(false, ex.Message);
            }
            return response;
        }

        [HttpPut]
        [Route(EntityRoute.Route.Delete, Name = EntityRoute.Name.Delete)]
        public Entity.BaseResponse<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return new Entity.BaseResponse<bool>(false, "Invalid Request");
            }

            Entity.BaseResponse<bool> response = new Entity.BaseResponse<bool>(true);
            try
            {
                var status = _service.Delete(id);
                response.IsSuccess = status.Success;
                response.Message = status.Message;
                response.Data = status.Success;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<bool>(false, ex.Message);
            }
            return response;
        }
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [HttpPut]
        [Route(EntityRoute.Route.DeleteImage, Name = EntityRoute.Name.DeleteImage)]
        public Entity.BaseResponse<bool> DeleteImage(Guid id)
        {
            Entity.BaseResponse<bool> response = new Entity.BaseResponse<bool>(true);
            try
            {
                var status = _service.DeleteImage(id);
                response.IsSuccess = status.Success;
                response.Message = status.Message;
                response.Data = status.Success;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<bool>(false, ex.Message);
            }
            return response;
        }
        [HttpGet]
        [Route(EntityRoute.Route.BySearch, Name = EntityRoute.Name.BySearch)]
        public Entity.BaseResponse<Entity.SearchResult<List<Entity.EntityDetail>>> GetBySearch(string parentEntityGuid = "", string searchText = "", int? pageNo = 1, int? pageSize = 10, string orderBy = "")
        {
            Entity.BaseResponse<Entity.SearchResult<List<Entity.EntityDetail>>> response = new Entity.BaseResponse<Entity.SearchResult<List<Entity.EntityDetail>>>(true);
            try
            {
                response.Data = _service.List(new Entity.SearchRequest()
                {
                    EntityId = string.IsNullOrEmpty(parentEntityGuid) ? Guid.Empty : new Guid(parentEntityGuid),
                    SearchText = searchText,
                    PageNumber = -1,//pageNo.Value,
                    PageSize = -1,//pageSize.Value,
                    OrderBy = orderBy
                });
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.SearchResult<List<Entity.EntityDetail>>>(false, ex.Message);
            }
            return response;
        }

        [HttpPost]
        [Route(EntityRoute.Route.UpdateStatus, Name = EntityRoute.Name.UpdateStatus)]
        public Entity.BaseResponse<bool> UpdateStatus(Guid id, bool status)
        {
            Entity.BaseResponse<bool> response = new Entity.BaseResponse<bool>(true);
            try
            {
                Entity.ActionStatus result = _service.UpdateStatus(id, status);
                response.IsSuccess = result.Success;
                response.Message = result.Message;
                response.Data = result.Success;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<bool>(false, ex.Message);
            }
            return response;
        }

      

    }
}