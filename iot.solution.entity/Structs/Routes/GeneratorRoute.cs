using System;
using System.Collections.Generic;
using System.Text;

namespace iot.solution.entity.Structs.Routes
{
    public class GeneratorRoute
    {
        public struct Name
        {
            public const string Add = "generator.add";
            public const string GetList = "generator.list";
            public const string GetById = "generator.getbyid";
            public const string Delete = "generator.delete";
            public const string DeleteMediaFile = "generator.deletemediafile";
            public const string BySearch = "generator.search";
            public const string UpdateStatus = "generator.updatestatus";
            public const string AcquireDevice = "device.acquire";
            public const string FileUpload = "generator.fileupload";
            public const string ValidateKit = "generator.validatekit";
            public const string ProvisionKit = "generator.provisionkit";
        }

        public struct Route
        {
            public const string Global = "api/generator";
            public const string Manage = "manage";
            public const string GetList = "";
            public const string GetById = "{id}";
            public const string Delete = "delete/{id}";
            public const string DeleteMediaFile = "deletemediafile/{generatorId}/{fileId?}";
            public const string UpdateStatus = "updatestatus/{id}/{status}";
            public const string AcquireDevice = "acquire/{id}";
            public const string BySearch = "search";
            public const string FileUpload = "fileupload/{generatorId}";
            public const string ValidateKit = "validatekit/{kitCode}";
            public const string ProvisionKit = "provisionkit";
        }
    }
}
