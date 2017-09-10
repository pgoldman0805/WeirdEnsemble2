CREATE TABLE [dbo].[Address] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Line1]            NVARCHAR (100) NOT NULL,
    [Line2]            NVARCHAR (100) NULL,
    [City]             NVARCHAR (200) NOT NULL,
    [StateProvince]    NVARCHAR (200) NOT NULL,
    [ZipCode]          NVARCHAR (25)  NOT NULL,
    [DateCreated]      DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [DateLastModified] DATETIME       DEFAULT (getUtcDate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

