CREATE TABLE [dbo].[Comment]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[DateOfCreation] date not null,
	[PictureId] int not null,
    [UserId] int not null,
    [Text] nvarchar(max) not null,
	CONSTRAINT [FK_Comment_Picture] FOREIGN KEY ([PictureId]) REFERENCES [Picture]([Id]),
    CONSTRAINT [FK_Comment_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
