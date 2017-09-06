CREATE TABLE [dbo].[ProductInventory]
(
	[ProductId] INT NOT NULL CONSTRAINT FK_ProductInventory_ProductId FOREIGN KEY REFERENCES Product(Id),
	[Quantity] SMALLINT NOT NULL,
	[DateCreated] DATETIME NOT NULL DEFAULT (GetUtcDate()),
	[DateLastModified] DATETIME NULL
)
