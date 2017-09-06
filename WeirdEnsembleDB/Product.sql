CREATE TABLE [dbo].[Product]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Brand] NVARCHAR(200) NULL,
	[Name] NVARCHAR(200) NOT NULL CHECK (Len(Name) > 3),
	[Description] NTEXT NULL,
	[ListPrice] MONEY NULL,
	[Rating] DECIMAL (10,2) NULL,
	[ProductLink] NVARCHAR(1500) NULL,
	[BrandLink] NVARCHAR(1500) NULL,
	[DateCreated] DATETIME NOT NULL DEFAULT(GetUtcDate()),
	[DateLastModified] DATETIME NULL
)
