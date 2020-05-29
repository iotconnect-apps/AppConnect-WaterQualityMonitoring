namespace iot.solution.entity.Structs.Routes
{
    public struct LookupRoute
    {
        public struct Name
        {
            public const string Get = "lookup.get";
            public const string GetTemplate = "lookup.template";
            public const string GetAllTemplate = "lookup.alltemplate";
            public const string GetTagLookup = "lookup.attributes";
            public const string GetAllTemplateIoT = "lookup.alltemplateiot";
            public const string GetAttributesIoT = "lookup.allattributesiot";            
            public const string GetSensorsLookup = "lookup.sensors";
            public const string GetTemplateCommands = "lookup.GetTemplateCommands";
            public const string GetCommandsIoT = "allcommandsiot/{templateGuid}";
            public const string GetEntityLookup = "lookup.getentitylookup";
            public const string GetSubEntityLookup = "lookup.getsubentitylookup";
            public const string GetDeviceLookup = "lookup.getdevicelookup";
        }

        public struct Route
        {
            public const string Global = "api/lookup";
            public const string Get = "{type}/{param?}";
            public const string GetTemplate = "template/{isGateway?}";
            public const string GetAllTemplate = "alltemplate";
            public const string GetTagLookup = "attributes";
            public const string GetAllTemplateIoT = "alltemplateiot";
            public const string GetAttributesIoT = "allattributesiot/{templateGuid}";
            public const string GetCommandsIoT = "allcommandsiot/{templateGuid}";            
            public const string GetSensorsLookup = "sensors/{deviceId}";
            public const string GetTemplateCommands = "commands/{templateId}";
            public const string GetEntityLookup = "entitylookup/{companyId}";
            public const string GetSubEntityLookup = "subentitylookup/{entityId}";
            public const string GetDeviceLookup = "devicelookup/{subentityId}";
        }
    }
}
