CREATE TABLE [dbo].[Picture]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[DateOfCreation] date not null,
    [Name] nvarchar(265) not null,
    [ImagePath] nvarchar(265) not null,
    [UserId] int null references [User](Id), 
    [Description] NVARCHAR(MAX) NULL
)

go
CREATE INDEX [IX_Picture_Id] ON [dbo].[Picture] ([Id])
