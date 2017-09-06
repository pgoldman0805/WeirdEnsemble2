﻿CREATE TABLE [dbo].[Order]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[CustomerID] INT NOT NULL CONSTRAINT FK_Order_CustomerID FOREIGN KEY REFERENCES Customer(Id),
	[BillToAddressID] INT NOT NULL CONSTRAINT FK_Order_BillToAddressID FOREIGN KEY REFERENCES Address(Id),
	[ShipToAddressID] INT NOT NULL CONSTRAINT FK_Order_ShipToAddressID FOREIGN KEY REFERENCES Address(Id),
	[DatePlaced] DATETIME NOT NULL DEFAULT (GetUtcDate()),
	[DateShipped] DATETIME NULL,
	[DateLastModified] DATETIME NULL DEFAULT (GetUtcDate())
)