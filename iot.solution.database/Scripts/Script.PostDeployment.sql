/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.[configuration] WHERE [configKey] = 'db-version')
BEGIN
	INSERT [dbo].[Configuration] ([guid], [configKey], [value], [isDeleted], [createdDate], [createdBy], [updatedDate], [updatedBy]) 
	VALUES (N'cf45da4c-1b49-49f5-a5c3-8bc29c1999ea', N'db-version', N'0', 0, GETUTCDATE(), NULL, GETUTCDATE(), NULL)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.[configuration] WHERE [configKey] = 'telemetry-last-exectime')
BEGIN
	INSERT [dbo].[Configuration] ([guid], [configKey], [value], [isDeleted], [createdDate], [createdBy], [updatedDate], [updatedBy]) 
	VALUES (N'465970b2-8bc3-435f-af97-8ca26f2bf383', N'telemetry-last-exectime', N'2020-01-01 12:08:02.380', 0, GETUTCDATE(), NULL, GETUTCDATE(), NULL)
END

IF NOT EXISTS(SELECT 1 FROM dbo.[configuration] WHERE [configKey] = 'db-version') 
	OR ((SELECT CONVERT(FLOAT,[value]) FROM dbo.[configuration] WHERE [configKey] = 'db-version') < 1 )
BEGIN

INSERT [dbo].[KitType] ([guid], [companyGuid], [name], [code], [tag], [isActive], [isDeleted], [createdDate], [createdBy], [updatedDate], [updatedBy]) VALUES (N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'b2a02818-9fee-4c69-9e54-da9d4b95f313', N'WQDefault', N'WQDefault', N'', 1, 0, CAST(N'2020-03-27T06:30:19.550' AS DateTime), NULL, CAST(N'2020-03-27T06:30:19.550' AS DateTime), NULL)

INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'605e2072-12c4-49d0-ad56-01c4b0ea5247', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'nh4', N'nh4', NULL, N'nh4')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'ae4f7ec0-24bc-4e33-a904-07926cf20b3e', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'turbidity', N'turbidity', NULL, N'turbidity')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'a604c1f8-faaa-4c87-a81d-1fc8dc6d7029', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'no2', N'no2', NULL, N'no2')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'a804359c-09b0-4ec4-8d9c-2a6d70f360af', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'orp', N'orp', NULL, N'orp')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'b35fb6d2-7476-45e1-91bd-36bda68cbea3', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'do', N'do', NULL, N'do')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'6a1dd501-7b64-4c85-9f2f-4bda237d0d65', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'n03', N'n03', NULL, N'n03')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'70afb835-2660-4ba7-ae3c-5e85a4a7c495', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'li', N'li', NULL, N'li')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'499a3001-78ca-433b-a0b5-663a98e29ea0', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'clo4', N'clo4', NULL, N'clo4')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'6a828f47-2e82-4428-9f7c-66c6bdc1adec', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'f', N'f', NULL, N'f')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'ab97cb42-b42a-46ba-862c-89de58e89f2f', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'br', N'br', NULL, N'br')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'7f0020f8-e643-4501-bf8a-8b499519dedc', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'consumption', N'cons', NULL, N'consumption')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'77d9a66a-334a-4e09-abd3-a1897ebb1af0', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'mg2', N'mg2', NULL, N'mg2')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'43d457dc-284a-4d98-8ed8-a4a735098463', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'ag', N'ag', NULL, N'ag')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'2360c4fc-c360-480c-bc61-ab52d7a3e90b', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'tds', N'tds', NULL, N'nh4')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'd6a7338f-cf98-4edb-b032-bb8b7536f8fc', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'na', N'na', NULL, N'na')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'ff6b622d-cb61-4a3f-96a1-c1242bf0a13b', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'cu2', N'cu2', NULL, N'cu2')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'b5020913-d490-4912-ac25-c8fe1aa70920', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'ph', N'ph', NULL, N'ph')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'764eb0bd-030d-4fed-8e7f-d76cf0c09d73', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'cl', N'cl', NULL, N'cl')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'b92f2e0b-1e93-49dd-bb6b-dccdd6f17c07', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'temp', N'temp', NULL, N'temp')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'127caa87-52eb-48f4-9540-e1cbe04537ac', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'lod', N'lod', NULL, N'lod')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'95542489-bffb-4aba-ae11-ef7326190b74', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'bf4', N'bf4', NULL, N'bf4')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'1ce57f09-49a0-4850-85ad-f7df9ddfb438', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'conv', N'conv', NULL, N'conv')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'c2d84ffd-2aa1-4857-a7c2-f9c0f14957fb', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'ca2', N'ca2', NULL, N'ca2')
INSERT [dbo].[KitTypeAttribute] ([guid], [parentTemplateAttributeGuid], [templateGuid], [localName], [code], [tag], [description]) VALUES (N'd713e130-d97a-4248-9600-feb5ce005539', NULL, N'bd59cf12-9cf9-4e6c-933d-c53296cfcda2', N'k', N'k', NULL, N'k')

INSERT INTO [dbo].[AdminUser] ([guid],[email],[companyGuid],[firstName],[lastName],[password],[isActive],[isDeleted],[createdDate]) VALUES (NEWID(),'admin@waterquality.com','B2A02818-9FEE-4C69-9E54-DA9D4B95F313','WaterQuality','admin','Softweb#123',1,0,GETUTCDATE())

UPDATE [dbo].[Configuration]
SET [value]  = '1'
WHERE [configKey] = 'db-version'

END