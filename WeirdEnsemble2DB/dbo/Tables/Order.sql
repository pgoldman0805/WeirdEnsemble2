CREATE TABLE [dbo].[Order] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT      NOT NULL,
    [BillToAddressID]  INT      NOT NULL,
    [ShipToAddressID]  INT      NOT NULL,
    [DatePlaced]       DATETIME DEFAULT (getutcdate()) NULL,
    [DateShipped]      DATETIME NULL,
    [DateLastModified] DATETIME DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_BillToAddressID] FOREIGN KEY ([BillToAddressID]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_Order_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([Id]),
    CONSTRAINT [FK_Order_ShipToAddressID] FOREIGN KEY ([ShipToAddressID]) REFERENCES [dbo].[Address] ([Id])
);

