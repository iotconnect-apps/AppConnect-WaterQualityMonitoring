﻿CREATE TABLE [dbo].[Entity] (
    [guid]             UNIQUEIDENTIFIER NOT NULL,
    [companyGuid]      UNIQUEIDENTIFIER NOT NULL,
    [parentEntityGuid] UNIQUEIDENTIFIER NULL,
    [name]             NVARCHAR (500)   NOT NULL,
    [type]             NVARCHAR (100)   NULL,
    [description]      NVARCHAR (1000)  NULL,
    [address]          NVARCHAR (500)   NULL,
    [address2]         NVARCHAR (500)   NULL,
    [city]             NVARCHAR (50)    NULL,
    [zipcode]          NVARCHAR (10)    NULL,
    [stateGuid]        UNIQUEIDENTIFIER NULL,
    [countryGuid]      UNIQUEIDENTIFIER NULL,
    [image]            NVARCHAR (250)   NULL,
    [latitude]         NVARCHAR (50)    NULL,
    [longitude]        NVARCHAR (50)    NULL,
    [isActive]         BIT              CONSTRAINT [DF__GreenHous__isact__59063A47] DEFAULT ((1)) NOT NULL,
    [isDeleted]        BIT              CONSTRAINT [DF__GreenHous__isdel__59FA5E80] DEFAULT ((0)) NOT NULL,
    [createdDate]      DATETIME         NULL,
    [createdBy]        UNIQUEIDENTIFIER NULL,
    [updatedDate]      DATETIME         NULL,
    [updatedBy]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK__GreenHou__497F6CB475C7652E] PRIMARY KEY CLUSTERED ([guid] ASC)
);



