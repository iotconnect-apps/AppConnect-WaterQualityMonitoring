﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity = iot.solution.entity;

namespace iot.solution.service.Interface
{
    public interface ILookupService
    {
        List<Entity.LookupItem> Get(string type, string param);
        List<Entity.LookupItem> GetTemplate(bool isGateway);
     //   List<Entity.LookupItem> GetAllTemplate();
        List<Entity.TagLookup> GetTagLookup(Guid templateId);
        List<Entity.LookupItem> GetSensors(Guid deviceId);
        List<Entity.LookupItem> GetTemplateAttribute(Guid templateId);
        List<Entity.LookupItem> GetTemplateCommands(Guid templateId);
        string GetIotTemplateGuidByName(string templateName);
        List<Entity.LookupItem> GetAllTemplateFromIoT();
        List<Entity.KitTypeAttribute> GetAllAttributesFromIoT(string templateGuid);
        List<Entity.LookupItem> GetAllCommandsFromIoT(string templateGuid);

        List<Entity.LookupItemWithStatus> EntityLookup(Guid companyId);

        List<Entity.LookupItemWithImage> SubEntityLookup(Guid entityId);

        List<Entity.LookupItemWithStatus> DeviceLookup(Guid zoneId);
    }
}