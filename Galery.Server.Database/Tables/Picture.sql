CREATE TABLE [dbo].[Picture]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[DateOfCreation] date not null,
    [Name] nvarchar(265) not null,
    [ImagePath] nvarchar(265) not null,
    [UserId] int not null references [User](Id), 
    [Description] NVARCHAR(MAX) NULL
)
