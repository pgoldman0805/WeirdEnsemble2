CREATE TABLE [dbo].[ProductCategory]
(
	[ProductId] INT NOT NULL CONSTRAINT FK_ProductCategory_ProductID FOREIGN KEY REFERENCES Product(Id),
	[CategoryId] INT NOT NULL CONSTRAINT FK_ProductCategory_CategoryID FOREIGN KEY REFERENCES Category(Id),
	[DateCreated] DATETIME NOT NULL DEFAULT (GetUtcDate()),
	[DateLastModified] DATETIME NULL,
	CONSTRAINT PK_ProductCategory PRIMARY KEY(ProductId, CategoryId)
)
