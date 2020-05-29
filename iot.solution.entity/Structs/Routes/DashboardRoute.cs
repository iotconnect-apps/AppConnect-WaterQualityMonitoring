using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity.Structs.Routes
{
    public class DashboardRoute
    {
        public struct Name
        {
            public const string GetEntity = "dashboard.getcompanylocation";
            public const string GetOverview = "dashboard.getoverview";
            public const string GetEntityDetail = "dashboard.getlocationdetail";
            public const string GetDeviceDetail = "dashboard.getdevicedetail";
            public const string GetEntityZone = "dashboard.getentityzone";
            public const string GetEntityDevices = "dashboard.getentitydevices";
            public const string GetEntityChildDevices = "dashboard.getentitychilddevices";
        }
        public struct Route
        {
            public const string Global = "api/dashboard";
            public const string GetEntity = "getcompanyentity/{companyId}";
            public const string GetOverview = "overview/{companyId}";
            public const string GetEntityDetail = "getentitydetail/{entityId}";
            public const string GetDeviceDetail = "getdevicedetail/{deviceId}";
            public const string GetEntityZone = "getentitysubentities/{entityId}";
            public const string GetEntityDevices = "getentitydevices/{entityId}";
            public const string GetEntityChildDevices = "getentitychilddevices/{deviceId}";
        }
    }
}
