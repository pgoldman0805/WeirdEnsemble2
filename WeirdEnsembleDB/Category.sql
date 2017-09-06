﻿CREATE TABLE [dbo].[Category]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[ParentID] INT NULL CONSTRAINT CK_Category_ParentID FOREIGN KEY REFERENCES Category(Id),
	[Name] NVARCHAR(200) NOT NULL,
	[DateLastModified] DATETIME NOT NULL DEFAULT (GetUtcDate())
)