﻿CREATE TABLE [dbo].[Contact] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]        NVARCHAR (200) NOT NULL,
    [LastName]         NVARCHAR (200) NOT NULL,
    [Email]            NVARCHAR (256) NOT NULL,
    [Comment]          NTEXT          NOT NULL,
    [DateCreated]      DATETIME       DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME       DEFAULT (getUtcDate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

