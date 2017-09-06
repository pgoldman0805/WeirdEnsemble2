CREATE TABLE [dbo].[Customer]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Title] NVARCHAR(8) NULL,
	[FirstName] NVARCHAR(100) NOT NULL,
	[MiddleName] NVARCHAR(100) NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[Suffix] NVARCHAR(8) NULL,
	[DateOfBirth] DATETIME NULL,
	[PhoneNumber] NVARCHAR(100) NULL,
	[EmailAddress] NVARCHAR(256) NOT NULL,
	[EmailPromotion] BIT NULL,
	[PasswordHash] VARCHAR(200) NOT NULL,
	[DateCreated] DATETIME DEFAULT (GetUtcDate()) NOT NULL,
	[DateLastModified] DATETIME NULL
)
