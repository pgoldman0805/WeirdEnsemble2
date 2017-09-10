CREATE TABLE [dbo].[OrderItem] (
    [Id]               INT        IDENTITY (1, 1) NOT NULL,
    [OrderId]          INT        NOT NULL,
    [ProductId]        INT        NOT NULL,
    [Quantity]         INT        DEFAULT ((1)) NOT NULL,
    [PurchasePrice]    SMALLMONEY NOT NULL,
    [DateCreated]      DATETIME   DEFAULT (getutcdate()) NOT NULL,
    [DateLastModified] DATETIME   DEFAULT (getUtcDate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrderItem_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]),
    CONSTRAINT [FK_OrderItem_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

