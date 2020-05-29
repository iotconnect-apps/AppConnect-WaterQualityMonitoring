﻿using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity
{
    public class IOTAlertMessage
    {
        public string message { get; set; }
        public string companyGuid { get; set; }
        public string condition { get; set; }
        public string deviceGuid { get; set; }
        public string entityGuid { get; set; }
        public DateTime eventDate { get; set; }
        public string uniqueId { get; set; }
        public string audience { get; set; }
        public int eventId { get; set; }
        public string refGuid { get; set; }
        public string severity { get; set; }
        public string ruleName { get; set; }
        public string data { get; set; }
    }
    public class AlertRequest
    {
        public string EntityGuid { get; set; }
        public string DeviceGuid { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "";
    }

    public class AlertResponse
    {
        public Guid Guid { get; set; }
        public string Message { get; set; }
        public DateTime EventDate { get; set; }
        public string UniqueId { get; set; }
        public string Severity { get; set; }
        public string EntityName { get; set; }
        public string ParentEntityName{get;set;}
        public string DeviceName { get; set; }
    }
}