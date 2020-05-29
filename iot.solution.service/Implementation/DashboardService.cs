using component.logger;
using iot.solution.model.Repository.Interface;
using iot.solution.service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = iot.solution.entity;

namespace iot.solution.service.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardrepository;
        private readonly IEntityRepository _entityRepository;
        private readonly ILogger _logger;
        public DashboardService(IEntityRepository entityRepository, IDashboardRepository dashboardrepository, ILogger logManager)
        {
            _entityRepository = entityRepository;
            _dashboardrepository = dashboardrepository;
            _logger = logManager;
        }

        public List<Entity.LookupItem> GetEntityLookup(Guid companyId)
        {
            List<Entity.LookupItem> lstResult = new List<Entity.LookupItem>();
            lstResult = (from g in _entityRepository.FindBy(r => r.CompanyGuid == companyId)
                         select new Entity.LookupItem()
                         {
                             Text = g.Name,
                             Value = g.Guid.ToString().ToUpper()
                         }).ToList();
            return lstResult;
        }

        public Entity.DashboardOverviewResponse GetOverview()
        {
        
            List<Entity.DashboardOverviewResponse> listResult = new List<Entity.DashboardOverviewResponse>();            
            try
            {               
                listResult = _dashboardrepository.GetStatistics();
                if (listResult.Count > 0)
                {
                    return listResult[0];
                }
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return new Entity.DashboardOverviewResponse()
            {
                TotalAlerts = 10,
                TotalSubEntities = 11,
                TotalEntities = 07
                
            }; 
        }
    }
}
