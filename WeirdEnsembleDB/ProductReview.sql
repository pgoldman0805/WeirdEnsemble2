﻿CREATE TABLE [dbo].[ProductReview]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[ProductID] INT NOT NULL CONSTRAINT FK_ProductReview_ProductID FOREIGN KEY REFERENCES Product(ID),
	[CustomerID] INT NOT NULL CONSTRAINT FK_ProductReview_CustomerID FOREIGN KEY REFERENCES Customer(ID),
	[Rating] INT NOT NULL,
	[Comments] NVARCHAR(3000) NULL,
	[DateCreated] DATETIME NOT NULL DEFAULT (GetUtcDate()),
	[DateLastModified] DATETIME NULL
)