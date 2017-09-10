CREATE TABLE [dbo].[Category] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ParentID]         INT            NULL,
    [Name]             NVARCHAR (200) NOT NULL,
    [DateLastModified] DATETIME       DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Category_ParentID] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Category] ([Id])
);

