using component.logger;
using iot.solution.data;
using iot.solution.model.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Entity = iot.solution.entity;

namespace iot.solution.model.Repository.Implementation
{
    public class NotificationsRepository : GenericRepository<Models.AdminRule>, INotificationsRepository
    {
        private readonly ILogger logger;
        public NotificationsRepository(IUnitOfWork unitOfWork, ILogger logManager) : base(unitOfWork, logManager)
        {
            logger = logManager;
            _uow = unitOfWork;
        }

        public Entity.SearchResult<List<Entity.AlertResponse>> GetAlertList(Entity.AlertRequest request)
        {
            Entity.SearchResult<List<Entity.AlertResponse>> result = new Entity.SearchResult<List<Entity.AlertResponse>>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY,  MethodBase.GetCurrentMethod().Name);
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<System.Data.Common.DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CurrentUserId, "v1");
                    parameters.Add(sqlDataAccess.CreateParameter("companyGuid", component.helper.SolutionConfiguration.CompanyId, DbType.Guid, ParameterDirection.Input));
                    if(!string.IsNullOrEmpty(request.EntityGuid))
                        parameters.Add(sqlDataAccess.CreateParameter("entityGuid", new Guid(request.EntityGuid), DbType.Guid, ParameterDirection.Input));
                    if (!string.IsNullOrEmpty(request.DeviceGuid))
                        parameters.Add(sqlDataAccess.CreateParameter("deviceGuid", new Guid(request.DeviceGuid), DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("pagesize", request.PageSize, DbType.Int32, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("pagenumber", request.PageNumber, DbType.Int32, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("orderby", request.OrderBy, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("count", DbType.Int32, ParameterDirection.Output, 16));
                    System.Data.Common.DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Alert_List]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Items = DataUtils.DataReaderToList<Entity.AlertResponse>(dbDataReader, null);
                    result.Count = int.Parse(parameters.Where(p => p.ParameterName.Equals("count")).FirstOrDefault().Value.ToString());
                }
                logger.Information(Constants.ACTION_EXIT,MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
    }
}
