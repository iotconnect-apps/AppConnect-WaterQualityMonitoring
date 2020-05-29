using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity.Structs.Routes
{
    public struct LocationRoute
    {
        public struct Name
        {
            public const string Add = "location.add";
            public const string GetList = "location.list";
            public const string GetById = "location.getdevicebyid";
            public const string Delete = "location.deletedevice";
            public const string BySearch = "location.search";
            public const string UpdateStatus = "location.updatestatus";
            public const string ChildDevice = "location.childdevicelist";
            public const string ValidateKit = "location.validatekit";
            public const string ProvisionKit = "location.provisionkit";
        }

        public struct Route
        {
            public const string Global = "api/location";
            public const string Manage = "manage";
            public const string GetList = "";
            public const string GetById = "{id}";
            public const string Delete = "delete/{id}";
            public const string UpdateStatus = "updatestatus/{id}/{status}";
            public const string BySearch = "search";
            public const string ChildDevice = "childdevicelist";
          
        }
    }
}
