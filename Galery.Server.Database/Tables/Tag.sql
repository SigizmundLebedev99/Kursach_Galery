CREATE TABLE [dbo].[Tag]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [Name] NVARCHAR(50) NOT NULL, 
    [DateOfCreation] DATE NOT NULL
)
