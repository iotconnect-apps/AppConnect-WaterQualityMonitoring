using iot.solution.model.Repository.Interface;
using iot.solution.service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Request = iot.solution.entity.Request;
using Response = iot.solution.entity.Response;
using Entity = iot.solution.entity;
using LogHandler = component.services.loghandler;
using iot.solution.data;
using component.logger;

namespace iot.solution.service.Implementation
{
    public class ChartService : IChartService
    {
        private readonly IEntityRepository _entityRepository;
        public string ConnectionString = component.helper.SolutionConfiguration.Configuration.ConnectionString;
        //private readonly LogHandler.Logger _logger;
        private readonly ILogger _logger;
        public ChartService(IEntityRepository entityRepository, ILogger logger)//, LogHandler.Logger logger)
        {
            _entityRepository = entityRepository;
            _logger = logger;
        }
        public Entity.ActionStatus TelemetrySummary_DayWise()
        {
            Entity.ActionStatus actionStatus = new Entity.ActionStatus(true);
            try
            {
                _logger.InfoLog(LogHandler.Constants.ACTION_ENTRY, null, "", "", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<DbParameter> parameters = new List<DbParameter>();
                    sqlDataAccess.ExecuteNonQuery(sqlDataAccess.CreateCommand("[TelemetrySummary_DayWise_Add]", CommandType.StoredProcedure, null), parameters.ToArray());
                }
                _logger.InfoLog(LogHandler.Constants.ACTION_EXIT, null, "", "", this.GetType().Name, MethodBase.GetCurrentMethod().Name);

            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                actionStatus.Success = false;
                actionStatus.Message = ex.Message;
            }
            return actionStatus;
        }
        public Entity.ActionStatus TelemetrySummary_HourWise()
        {
            Entity.ActionStatus actionStatus = new Entity.ActionStatus(true);
            try
            {
                _logger.InfoLog(LogHandler.Constants.ACTION_ENTRY, null, "", "", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<DbParameter> parameters = new List<DbParameter>();
                    sqlDataAccess.ExecuteNonQuery(sqlDataAccess.CreateCommand("[TelemetrySummary_HourWise_Add]", CommandType.StoredProcedure, null), parameters.ToArray());
                }
                _logger.InfoLog(LogHandler.Constants.ACTION_EXIT, null, "", "", this.GetType().Name, MethodBase.GetCurrentMethod().Name);

            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                actionStatus.Success = false;
                actionStatus.Message = ex.Message;
            }
            return actionStatus;
        }
        public List<Response.DeviceUsageResponse> GetDeviceUsage(Request.ChartRequest request)
        {
           
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("companyguid", request.CompanyGuid.ToString());
            parameters.Add("entityguid", request.Entityguid.ToString());
            parameters.Add("hardwarekitguid", request.HardwareKitGuid.ToString());
            return _entityRepository.ExecuteStoredProcedure<Response.DeviceUsageResponse>("[DeviceUsage_Get]", parameters);
        }
        public List<Response.EnergyUsageResponse> GetEnergyUsage(Request.ChartRequest request)
        {
            Dictionary<string,string> parameters = new Dictionary<string, string>();
            parameters.Add("companyguid", request.CompanyGuid.ToString());
            parameters.Add("entityguid", request.Entityguid.ToString());
            parameters.Add("hardwarekitguid", request.HardwareKitGuid.ToString());
            return _entityRepository.ExecuteStoredProcedure<Response.EnergyUsageResponse>("[ChartDate]", parameters);
        }

        public List<Response.DeviceBatteryStatusResponse> GetDeviceBatteryStatus(Request.ChartRequest request)
        {
        
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("companyguid", request.CompanyGuid.ToString());
            parameters.Add("entityguid", request.Entityguid.ToString());
            parameters.Add("hardwarekitguid", request.HardwareKitGuid.ToString());
            return _entityRepository.ExecuteStoredProcedure<Response.DeviceBatteryStatusResponse>("[DeviceUsage_Get]", parameters);
        }

        public List<entity.DeviceAttributeChartResponse> GetDeviceAttributeChartData(Guid deviceGuid,string attribute,string frequency)
        {
            var result = new List<entity.DeviceAttributeChartResponse>();
           

            return result;
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("companyguid", request.CompanyGuid.ToString());
            //parameters.Add("entityguid", request.Entityguid.ToString());
            //parameters.Add("hardwarekitguid", request.HardwareKitGuid.ToString());
            //return _entityRepository.ExecuteStoredProcedure<Response.FuelUsageResponse>("[ChartDate]", parameters);
        }
    }
}
