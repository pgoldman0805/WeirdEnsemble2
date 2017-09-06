CREATE TABLE [dbo].[CustomerAddress]
(
	[CustomerID] INT NOT NULL CONSTRAINT FK_CustomerAddress_CustomerID FOREIGN KEY REFERENCES Customer(Id),
	[AddressID] INT NOT NULL CONSTRAINT FK_CustomerAddress_AddressID FOREIGN KEY REFERENCES Address(Id),
	[DateLastModified] DATETIME NOT NULL DEFAULT (GetUtcDate()),
	CONSTRAINT PK_CustomerAddress PRIMARY KEY (CustomerID, AddressID)
)
