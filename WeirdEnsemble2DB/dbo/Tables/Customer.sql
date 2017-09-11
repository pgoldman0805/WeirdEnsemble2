CREATE TABLE [dbo].[Customer] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (8)   NULL,
    [FirstName]        NVARCHAR (100) NOT NULL,
    [MiddleName]       NVARCHAR (100) NULL,
    [LastName]         NVARCHAR (100) NOT NULL,
    [DateOfBirth]      DATETIME       NULL,
    [PhoneNumber]      NVARCHAR (100) NULL,
    [DateCreated]      DATETIME       DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME       DEFAULT (getUtcDate()) NULL,
    [AspNetUserID] NVARCHAR(128) NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Customer_AspNetUsers] FOREIGN KEY (AspNetUserID) REFERENCES AspNetUsers(Id)
);

