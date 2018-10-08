CREATE TABLE [dbo].[PictureTag]
(
	[TagId] INT NOT NULL , 
    [PictureId] INT NOT NULL, 
    PRIMARY KEY ([TagId], [PictureId]),
    CONSTRAINT [FK_PictureTags_TAg] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id]) on delete cascade,
    CONSTRAINT [FK_PictureTags_Picture] FOREIGN KEY ([PictureId]) REFERENCES [Picture]([Id]) on delete cascade
)
