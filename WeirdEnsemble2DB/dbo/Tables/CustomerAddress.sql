CREATE TABLE [dbo].[CustomerAddress] (
    [CustomerID]       INT      NOT NULL,
    [AddressID]        INT      NOT NULL,
    [DateLastModified] DATETIME DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_CustomerAddress] PRIMARY KEY CLUSTERED ([CustomerID] ASC, [AddressID] ASC),
    CONSTRAINT [FK_CustomerAddress_AddressID] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_CustomerAddress_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([Id])
);

