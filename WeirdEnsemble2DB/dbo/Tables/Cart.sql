CREATE TABLE [dbo].[Cart] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]       INT           NULL,
    [Name]             NVARCHAR (50) NOT NULL,
    [DateCreated]      DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [DateLastModified] DATETIME      DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Cart_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id])
);

