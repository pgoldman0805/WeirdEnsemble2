CREATE TABLE [dbo].[ProductReview] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [ProductID]        INT             NOT NULL,
    [CustomerID]       INT             NOT NULL,
    [Rating]           INT             NOT NULL,
    [Comments]         NVARCHAR (3000) NULL,
    [DateCreated]      DATETIME        DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME        DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductReview_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([Id]),
    CONSTRAINT [FK_ProductReview_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([Id])
);

