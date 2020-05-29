﻿using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity.Structs.Routes
{
    public class DeviceRoute
    {
        public struct Name
        {
            public const string Add = "device.add";
            public const string GetList = "device.list";
            public const string GetById = "device.getbyid";
            public const string Delete = "device.delete";
            public const string DeleteMediaFile = "device.deletemediafile";
            public const string BySearch = "device.search";
            public const string UpdateStatus = "device.updatestatus";
            public const string FileUpload = "device.fileupload";
            public const string ValidateKit = "device.validatekit";
            public const string ProvisionKit = "device.provisionkit";
            public const string DeviceCounters = "device.counters";
            public const string DeviceLatestAttributeValues = "device.devicelatestattributevalues";
            public const string WQIIndexValue = "device.wqiindexvalue";
            public const string TelemetryData = "device.telemetry";
            public const string ConnectionStatus = "device.connectionstatus";
        }

        public struct Route
        {
            public const string Global = "api/device";
            public const string Manage = "manage";
            public const string GetList = "";
            public const string GetById = "{id}";
            public const string Delete = "delete/{id}";
            public const string DeleteMediaFile = "deletemediafile/{generatorId}/{fileId?}";
            public const string UpdateStatus = "updatestatus/{id}/{status}";
            public const string BySearch = "search";
            public const string FileUpload = "fileupload/{generatorId}";
            public const string ValidateKit = "validatekit/{kitCode}";
            public const string ProvisionKit = "provisionkit";
            public const string DeviceCounters = "counters";
            public const string DeviceLatestAttributeValues = "getdeviceattributevalues/{deviceGuid}";
            public const string WQIIndexValue = "getwqiindexvalue/{deviceGuid}";
            public const string TelemetryData = "telemetry/{deviceId}";
            public const string ConnectionStatus = "connectionstatus/{uniqueId}";
        }
    }
}
