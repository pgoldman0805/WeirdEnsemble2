CREATE TABLE [dbo].[ProductImage] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]        INT            NOT NULL,
    [ImagePath]        NVARCHAR (256) NOT NULL,
    [AlternateText]    NTEXT          NULL,
	[DateCreated] DATETIME NULL DEFAULT (GetUtcDate()),
    [DateLastModified] DATETIME       DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductImage_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([Id]) ON DELETE CASCADE
);

