CREATE TABLE [dbo].[Order] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT      NOT NULL,
    [DatePlaced]       DATETIME DEFAULT (getutcdate()) NOT NULL,
    [DateShipped]      DATETIME NULL,
    [DateLastModified] DATETIME DEFAULT (getutcdate()) NULL,
	[ShippingAddressLine1] NVARCHAR(200) NULL,
    [TransactionID] NVARCHAR(100) NULL, 
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([Id]),
);

