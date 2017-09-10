CREATE TABLE [dbo].[Product] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Brand]            NVARCHAR (200)  NULL,
    [Name]             NVARCHAR (200)  NOT NULL,
    [Description]      NTEXT           NULL,
    [ListPrice]        MONEY           NULL,
    [ProductLink]      NVARCHAR (1500) NULL,
    [BrandLink]        NVARCHAR (1500) NULL,
    [DateCreated]      DATETIME        DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME        DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
);

