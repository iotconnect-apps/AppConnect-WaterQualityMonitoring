﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using host.iot.solution.Controllers;
using iot.solution.entity.Structs.Routes;
using Microsoft.AspNetCore.Mvc;
using iot.solution.entity;
using iot.solution.entity.Response;
using Microsoft.AspNetCore.Authorization;
using iot.solution.service.Interface;
using Entity = iot.solution.entity;
using Response = iot.solution.entity.Response;
using AutoMapper.Configuration;
//using component.common.model;
namespace iot.solution.host.Controllers
{

    [Route(AdminRoute.Route.Global)]
    public class AdminUserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAdminUserService _adminUserService;

        public AdminUserController(IUserService userService, IAdminUserService adminUserService)
        {
            _userService = userService;
            _adminUserService = adminUserService;
        }

        [HttpGet]
        [Route(AdminRoute.Route.GetList, Name = AdminRoute.Name.GetList)]
        public Entity.BaseResponse<Entity.SearchResult<List<Entity.UserResponse>>> GetAdminUserList(string searchText = "", int? pageNo = -1, int? pageSize = -1, string orderBy = "")
        {
            Entity.BaseResponse<Entity.SearchResult<List<Entity.UserResponse>>> response = new Entity.BaseResponse<Entity.SearchResult<List<Entity.UserResponse>>>();
            var searchRequest = new SearchRequest()
            {
                SearchText = searchText,
                //CompanyId = "895019CF-1D3E-420C-828F-8971253E5784",
                CompanyId = Convert.ToString(component.helper.SolutionConfiguration.CompanyId),
                PageNumber = pageNo.Value,
                OrderBy = orderBy,
                PageSize = pageSize.Value
            };
            try
            {
                response.Data = _adminUserService.GetAdminUserList(searchRequest);
                response.IsSuccess = true;
                response.Message = "";
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<Entity.SearchResult<List<Entity.UserResponse>>>(false, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [Route(AdminRoute.Route.Manage, Name = AdminRoute.Name.Manage)]
        public Entity.BaseResponse<Entity.UserResponse> Manage([FromBody]Entity.AddAdminUserRequest request)
        {
            Entity.BaseResponse<Entity.UserResponse> response = new Entity.BaseResponse<Entity.UserResponse>(true);
            try
            {
                var status = _adminUserService.Manage(request);
                response.IsSuccess = status.Success;
                response.Message = status.Message;
                response.Data = status.Data;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                response.IsSuccess = false;
                response.Message = ex.Message.ToString();
                return new Entity.BaseResponse<Entity.UserResponse>(false, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route(AdminRoute.Route.GetById, Name = AdminRoute.Name.GetById)]
        public Entity.BaseResponse<Entity.AdminUserResponse> Get(Guid id)
        {
            Entity.BaseResponse<Entity.AdminUserResponse> response = new BaseResponse<AdminUserResponse>(true);
            try
            {
                response.Data = _adminUserService.Get(id);
                if(response.Data == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User not found.";
                }
                else
                {
                    response.IsSuccess = true;
                }
                
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                response.IsSuccess = false;
                return new Entity.BaseResponse<Entity.AdminUserResponse>(false, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [Route(AdminRoute.Route.Delete, Name = AdminRoute.Name.Delete)]
        public Entity.BaseResponse<UserResponse> Delete(Guid id)
        {
            Entity.BaseResponse<UserResponse> response = new Entity.BaseResponse<UserResponse>(true);

            try
            {
                var status = _adminUserService.Delete(id);
                response.IsSuccess = true;
                response.Message = status.Message;
                response.Data = status.Data;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<UserResponse>(false, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [Route(AdminRoute.Route.UpdateStatus, Name = AdminRoute.Name.UpdateStatus)]
        public Entity.BaseResponse<bool> UpdateStatus(Guid id, bool status)
        {
            Entity.BaseResponse<bool> response = new Entity.BaseResponse<bool>(true);

            try
            {
                var result = _adminUserService.UpdateStatus(id, status);

                if (result.Success)
                {
                    response.IsSuccess = true;
                    response.Message = "";
                    response.Data = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong!";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<bool>(false, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [Route(AdminRoute.Route.ChangePassword, Name = AdminRoute.Name.ChangePassword)]
        public Entity.BaseResponse<AdminUserResponse> ChangePassword(ChangePasswordRequest request)
        {
            Entity.BaseResponse<AdminUserResponse> response = new Entity.BaseResponse<AdminUserResponse>(true);

            try
            {
                var result = _adminUserService.ChangePassword(request);

                response.IsSuccess = result.Success;
                response.Message = result.Message;
                response.Data = result.Data;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
                return new Entity.BaseResponse<AdminUserResponse>(false, ex.Message);
            }

            return response;
        }



    }
}
