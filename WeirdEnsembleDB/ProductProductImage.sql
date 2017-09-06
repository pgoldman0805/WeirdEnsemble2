CREATE TABLE [dbo].[ProductProductImage]
(
	[ProductId] INT NOT NULL CONSTRAINT FK_ProductProductImage_ProductId FOREIGN KEY REFERENCES Product(Id),
	[ProductImageId] INT NOT NULL CONSTRAINT FK_ProductProductImage_ProductImageId FOREIGN KEY REFERENCES ProductImage(Id),
	[Primary] BIT NOT NULL, -- 0 = NOT Primary image, 1 = Primary Image
	[DateLastModified] DATETIME NOT NULL DEFAULT (GetUtcDate()),
	CONSTRAINT PK_ProductProductImage PRIMARY KEY (ProductId, ProductImageId)
)
