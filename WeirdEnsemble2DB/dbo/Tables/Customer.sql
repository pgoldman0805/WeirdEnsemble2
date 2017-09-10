CREATE TABLE [dbo].[Customer] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (8)   NULL,
    [FirstName]        NVARCHAR (100) NOT NULL,
    [MiddleName]       NVARCHAR (100) NULL,
    [LastName]         NVARCHAR (100) NOT NULL,
    [Suffix]           NVARCHAR (8)   NULL,
    [DateOfBirth]      DATETIME       NULL,
    [PhoneNumber]      NVARCHAR (100) NULL,
    [EmailAddress]     NVARCHAR (256) NOT NULL,
    [EmailPromotion]   BIT            NULL,
    [PasswordHash]     VARCHAR (200)  NOT NULL,
    [DateCreated]      DATETIME       DEFAULT (getutcdate()) NULL,
    [DateLastModified] DATETIME       DEFAULT (getUtcDate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

