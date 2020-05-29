﻿using System;
using System.Collections.Generic;
using Entity = iot.solution.entity;

namespace iot.solution.service.Interface
{
    public interface IDashboardService
    {
        List<Entity.LookupItem> GetEntityLookup(Guid companyId);
        Entity.DashboardOverviewResponse GetOverview();
        
    }
}
