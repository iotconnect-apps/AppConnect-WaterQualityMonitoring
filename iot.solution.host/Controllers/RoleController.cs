﻿using iot.solution.entity.Structs.Routes;
using iot.solution.service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = iot.solution.entity;

namespace host.iot.solution.Controllers
{
    [Route(RoleRoute.Route.Global)]
    public class RoleController : BaseController
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService roleEngine)
        {
            _service = roleEngine;
        }

        [HttpGet]
        [Route(RoleRoute.Route.GetList, Name = RoleRoute.Name.GetList)]
        public Entity.BaseResponse<List<Entity.Role>> Get()
        {
            Entity.BaseResponse<List<Entity.Role>> response = new Entity.BaseResponse<List<Entity.Role>>(true);
            try
            {
                response.Data = _service.Get();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Entity.Role>>(false, ex.Message);
            }
            return response;
        }
        [HttpGet]
        [Route(RoleRoute.Route.GetByCompany, Name = RoleRoute.Name.GetByCompany)]
        public Entity.BaseResponse<List<Entity.Role>> GetByCompany()
        {
            Entity.BaseResponse<List<Entity.Role>> response = new Entity.BaseResponse<List<Entity.Role>>(true);
            try
            {
                response.Data = _service.Get().Where(t => t.CompanyGuid == component.helper.SolutionConfiguration.CompanyId && t.IsActive).ToList();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<List<Entity.Role>>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(RoleRoute.Route.GetById, Name = RoleRoute.Name.GetById)]
        public Entity.BaseResponse<Entity.Role> Get(Guid id)
        {
            Entity.BaseResponse<Entity.Role> response = new Entity.BaseResponse<Entity.Role>(true);
            try
            {
                response.Data = _service.Get(id);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.Role>(false, ex.Message);
            }
            return response;
        }

        [HttpPost]
        [Route(RoleRoute.Route.Manage, Name = RoleRoute.Name.Add)]
        public Entity.BaseResponse<Entity.Role> Manage([FromBody]Entity.Role request)
        {
            Entity.BaseResponse<Entity.Role> response = new Entity.BaseResponse<Entity.Role>(true);
            try
            {
                var status = _service.Manage(request);
                response.IsSuccess = status.Success;
                response.Message = status.Message;
                response.Data = status.Data;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.Role>(false, ex.Message);
            }
            return response;
        }

        [HttpPut]
        [Route(RoleRoute.Route.Delete, Name = RoleRoute.Name.Delete)]
        public Entity.BaseResponse<bool> Delete(Guid id)
        {
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

        [HttpGet]
        [Route(RoleRoute.Route.BySearch, Name = RoleRoute.Name.BySearch)]
        public Entity.BaseResponse<Entity.SearchResult<List<Entity.Role>>> GetBySearch(string searchText = "", int? pageNo = 1, int? pageSize = 10, string orderBy = "")
        {
            Entity.BaseResponse<Entity.SearchResult<List<Entity.Role>>> response = new Entity.BaseResponse<Entity.SearchResult<List<Entity.Role>>>(true);
            try
            {
                response.Data = _service.List(new Entity.SearchRequest()
                {
                    SearchText = searchText,
                    PageNumber = pageNo.Value,
                    PageSize = pageSize.Value,
                    OrderBy = orderBy
                });
                if (response.Data.Count < 0)
                {
                    response.Data.Count = 0;
                    response.IsSuccess = false;
                    response.Message = "User not authorized!";
                }
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.SearchResult<List<Entity.Role>>>(false, ex.Message);
            }
            return response;
        }

        [HttpPost]
        [Route(RoleRoute.Route.UpdateStatus, Name = RoleRoute.Name.UpdateStatus)]
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
