CREATE TABLE [dbo].[OrderItem] (
    [OrderId]          INT        NOT NULL,
    [ProductId]        INT        NOT NULL,
    [Quantity]         INT        DEFAULT ((1)) NOT NULL,
    [PurchasePrice]    MONEY NOT NULL DEFAULT 0,
    [DateCreated]      DATETIME   DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME   DEFAULT (getUtcDate()) NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([OrderId] ASC,[ProductId] ASC),
    CONSTRAINT [FK_OrderItem_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]),
    CONSTRAINT [FK_OrderItem_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

