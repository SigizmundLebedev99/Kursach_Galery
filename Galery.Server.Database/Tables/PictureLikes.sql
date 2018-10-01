CREATE TABLE [dbo].[PictureLikes]
(
	[UserId] INT NOT NULL , 
    [PictureId] INT NOT NULL, 
    PRIMARY KEY ([UserId], [PictureId]),
    CONSTRAINT [FK_PictureLikes_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_PictureLikes_Picture] FOREIGN KEY ([PictureId]) REFERENCES [Picture]([Id])
)
