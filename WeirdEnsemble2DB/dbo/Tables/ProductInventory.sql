CREATE TABLE [dbo].[ProductInventory] (
    [ProductId]        INT      NOT NULL,
    [Quantity]         SMALLINT DEFAULT ((1)) NOT NULL,
    [DateCreated]      DATETIME DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME DEFAULT (getUtcDate()) NULL,
    CONSTRAINT [PK_ProductInventory] PRIMARY KEY CLUSTERED ([ProductId] ASC),
    CONSTRAINT [FK_ProductInventory_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

