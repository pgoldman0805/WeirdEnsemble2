CREATE TABLE [dbo].[CartItem] (
    [CartId]           INT      NOT NULL,
    [ProductId]        INT      NOT NULL,
    [Quantity]         INT      DEFAULT ((1)) NOT NULL,
    [DateCreated]      DATETIME DEFAULT (getutcdate()) NOT NULL,
    [DateLastModified] DATETIME DEFAULT (getUtcDate()) NOT NULL,
    CONSTRAINT [PK_CartItem] PRIMARY KEY CLUSTERED ([CartId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_CartItem_CartId] FOREIGN KEY ([CartId]) REFERENCES [dbo].[Cart] ([Id]),
    CONSTRAINT [FK_CartItem_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

