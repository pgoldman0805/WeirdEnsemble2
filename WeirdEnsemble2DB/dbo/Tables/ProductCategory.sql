CREATE TABLE [dbo].[ProductCategory] (
    [ProductId]        INT      NOT NULL,
    [CategoryId]       INT      NOT NULL,
    [DateCreated]      DATETIME DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED ([ProductId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_ProductCategory_CategoryID] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_ProductCategory_ProductID] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

