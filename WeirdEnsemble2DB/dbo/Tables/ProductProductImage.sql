CREATE TABLE [dbo].[ProductProductImage] (
    [ProductId]        INT      NOT NULL,
    [ProductImageId]   INT      NOT NULL,
    [Primary]          BIT      NOT NULL,
    [DateLastModified] DATETIME DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ProductProductImage] PRIMARY KEY CLUSTERED ([ProductId] ASC, [ProductImageId] ASC),
    CONSTRAINT [FK_ProductProductImage_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_ProductProductImage_ProductImageId] FOREIGN KEY ([ProductImageId]) REFERENCES [dbo].[ProductImage] ([Id])
);

