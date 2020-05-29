﻿using component.logger;
using iot.solution.data;
using iot.solution.entity;
using iot.solution.model.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Entity = iot.solution.entity;
using Model = iot.solution.model.Models;
using Response = iot.solution.entity.Response;

namespace iot.solution.model.Repository.Implementation
{
    public class DeviceRepository : GenericRepository<Model.Device>, IDeviceRepository
    {
        private readonly ILogger logger;
        private readonly IWebHostEnvironment _env;
        public DeviceRepository(IUnitOfWork unitOfWork, ILogger logManager,IWebHostEnvironment env) : base(unitOfWork, logManager)
        {
            logger = logManager;
            _uow = unitOfWork;
            _env = env;
        }

        public Model.Device Get(string device)
        {
            var result = new Model.Device();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.Get");
                _uow.DbContext.Device.Where(u => u.Name.Equals(device, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.Get");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        private string GetFileSize(string FilePath) 
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            string fileSize = string.Empty;
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;
            FileInfo fileInfo = new FileInfo(webRootPath + "/" + FilePath);
            if (fileInfo.Exists)
            {
                try
                {
                    double len = fileInfo.Length;
                    int order = 0;
                    while (len >= 1024 && order < sizes.Length - 1)
                    {
                        order++;
                        len = len / 1024;
                    }

                    // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                    // show a single decimal place, and no space.
                    fileSize = String.Format("{0:0.##} {1}", len, sizes[order]);
                }
                catch (Exception ex) 
                { 
                
                }
            }
            return fileSize;
        }
       
        public Entity.ActionStatus Manage(Model.Device request)
        {
            ActionStatus result = new ActionStatus(true);
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.Manage");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {

                     List<DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CurrentUserId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("guid", request.Guid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("companyGuid", request.CompanyGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("entityGuid", request.EntityGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("templateGuid", request.TemplateGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("parentDeviceGuid", request.ParentDeviceGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("typeGuid", request.TypeGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("uniqueId", request.UniqueId, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("name", request.Name, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("note", request.Note, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("tag", request.Tag, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("description", request.Description, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("specification", request.Specification, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("image", request.Image, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("isProvisioned", request.IsProvisioned, DbType.Boolean, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("newid", request.Guid, DbType.Guid, ParameterDirection.Output));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    int intResult = sqlDataAccess.ExecuteNonQuery(sqlDataAccess.CreateCommand("[Device_AddUpdate]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Data = Guid.Parse(parameters.Where(p => p.ParameterName.Equals("newid")).FirstOrDefault().Value.ToString());                    
                }
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.Manage");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        public Entity.ActionStatus Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        public Entity.SearchResult<List<Entity.DeviceDetail>> List(Entity.SearchRequest request)
        {
            Entity.SearchResult<List<Entity.DeviceDetail>> result = new Entity.SearchResult<List<Entity.DeviceDetail>>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.List");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CurrentUserId, request.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("companyguid", component.helper.SolutionConfiguration.CompanyId, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("isParent", true, DbType.Boolean, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("search", request.SearchText, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("pagesize", request.PageSize, DbType.Int32, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("pagenumber", request.PageNumber, DbType.Int32, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("orderby", request.OrderBy, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("count", DbType.Int32, ParameterDirection.Output, 16));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Device_List]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Items = DataUtils.DataReaderToList<Entity.DeviceDetail>(dbDataReader, null);
                    result.Count = int.Parse(parameters.Where(p => p.ParameterName.Equals("count")).FirstOrDefault().Value.ToString());
                }
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.List");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        

      
      
        public List<Response.EntityWiseDeviceResponse> GetEntityWiseDevices(Guid? locationId, Guid? deviceId)
        {
            List<Response.EntityWiseDeviceResponse> result = new List<Response.EntityWiseDeviceResponse>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "GetLocationDevices.Get");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CompanyId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("companyGuid", component.helper.SolutionConfiguration.CompanyId, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("entityGuid", locationId, DbType.Guid, ParameterDirection.Input));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Device_Lookup]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result = DataUtils.DataReaderToList<Response.EntityWiseDeviceResponse>(dbDataReader, null);
                }
                logger.Information(Constants.ACTION_EXIT, "GetLocationDevices.Get");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        public Entity.BaseResponse<int> ValidateKit(string kitCode)
        {
            Entity.BaseResponse<int> result = new Entity.BaseResponse<int>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "ValidateKit.Get");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CompanyId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("kitCode", kitCode, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    sqlDataAccess.ExecuteScalar(sqlDataAccess.CreateCommand("[Validate_KitCode]", CommandType.StoredProcedure, null), parameters.ToArray());
                    int outPut = int.Parse(parameters.Where(p => p.ParameterName.Equals("output")).FirstOrDefault().Value.ToString());
                    if (outPut > 0)
                    {
                        result.IsSuccess = true;
                    }
                    else {
                        result.IsSuccess = false;
                    }
                    result.Message = parameters.Where(p => p.ParameterName.Equals("fieldname")).FirstOrDefault().Value.ToString();
                }
                logger.Information(Constants.ACTION_EXIT, "ValidateKit.Get");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        public Entity.BaseResponse<List<Entity.HardwareKit>> ProvisionKit(Entity.ProvisionKitRequest request)
        {
            Entity.BaseResponse<List<Entity.HardwareKit>> result = new Entity.BaseResponse<List<Entity.HardwareKit>>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.ProvisionKit");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {                   

                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CompanyId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("kitCode", request.KitCode, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("uniqueId", request.UniqueId, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[HardwareKit_GetStatus]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Data = DataUtils.DataReaderToList<Entity.HardwareKit>(dbDataReader, null);
                }
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.ProvisionKit");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }

        public Entity.BaseResponse<List<Entity.DeviceLatestAttributeResponse>> GetLatestAttributesValue(Guid deviceGuid)
        {
            Entity.BaseResponse<List<Entity.DeviceLatestAttributeResponse>> result = new Entity.BaseResponse<List<Entity.DeviceLatestAttributeResponse>>(true);
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.GetLatestAttributesValue");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {

                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CompanyId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("guid", deviceGuid, DbType.Guid, ParameterDirection.Input));
                   // parameters.Add(sqlDataAccess.CreateParameter("uniqueId", request.UniqueId, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Device_LatestAttributeValue_Get]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Data = DataUtils.DataReaderToList<Entity.DeviceLatestAttributeResponse>(dbDataReader, null);
                }
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.GetLatestAttributesValue");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }

        public Entity.BaseResponse<List<Entity.WaterConsumptionResponse>> GetWaterConsumptionChartData(Guid deviceGuid,string frequency)
        {
            Entity.BaseResponse<List<Entity.WaterConsumptionResponse>> result = new Entity.BaseResponse<List<Entity.WaterConsumptionResponse>>(true);
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.GetWaterConsumptionChartData");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {

                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CompanyId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("guid", deviceGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("frequency", frequency, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("syncDate", DateTime.UtcNow, DbType.DateTime, ParameterDirection.Output));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Chart_WaterConsumption]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Data = DataUtils.DataReaderToList<Entity.WaterConsumptionResponse>(dbDataReader, null);
                    if (parameters.Where(p => p.ParameterName.Equals("syncDate")).FirstOrDefault() != null)
                    {
                        result.LastSyncDate = Convert.ToString(parameters.Where(p => p.ParameterName.Equals("syncDate")).FirstOrDefault().Value);
                    }
                }
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.GetWaterConsumptionChartData");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }

        public Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>> GetDeviceAttributeChartData(Guid deviceGuid,string attributeName, string frequency)
        {
            Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>> result = new Entity.BaseResponse<List<Entity.DeviceAttributeChartResponse>>(true);
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "DeviceRepository.GetDeviceAttributeChartData");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {

                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CompanyId, component.helper.SolutionConfiguration.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("guid", deviceGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("attribute", attributeName, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("frequency", frequency, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("syncDate", DateTime.UtcNow, DbType.DateTime, ParameterDirection.Output));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Chart_QualityParameter]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Data = DataUtils.DataReaderToList<Entity.DeviceAttributeChartResponse>(dbDataReader, null);
                    if (parameters.Where(p => p.ParameterName.Equals("syncDate")).FirstOrDefault() != null)
                    {
                        result.LastSyncDate = Convert.ToString(parameters.Where(p => p.ParameterName.Equals("syncDate")).FirstOrDefault().Value);
                    }
                }
                logger.Information(Constants.ACTION_EXIT, "DeviceRepository.GetDeviceAttributeChartData");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }


        #region Lookup       

        public List<Entity.LookupItem> GetDeviceLookup()
        {
            using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
            {
                return sqlDataAccess.QueryList<Entity.LookupItem>("SELECT CONVERT(NVARCHAR(50),[Guid]) AS [Value], [name] AS [Text] FROM [DeviceType] WHERE [isActive] = 1 AND [isDeleted] = 0");
            }
        }
        #endregion
    }
}
