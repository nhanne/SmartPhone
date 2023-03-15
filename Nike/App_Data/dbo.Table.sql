CREATE TABLE [dbo].[Users]
(
	[IdUser] INT IDENTITY (1,1) NOT NULL ,
	[FirstName] Nvarchar(50) null,
	[LastName] Nvarchar(50) null, 
    [Email] NVARCHAR(50) NULL, 
    [Password] NVARCHAR(50) NULL,
	PRIMARY KEY CLUSTERED ([IdUser] ASC)
)
