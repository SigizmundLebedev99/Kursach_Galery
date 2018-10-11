CREATE TABLE [dbo].[Subscribe]
(
	[FromUserId] INT NOT NULL , 
    [ToUserId] INT NOT NULL, 
    PRIMARY KEY ([FromUserId], [ToUserId]),
    CONSTRAINT [FK_Subscribe_From] FOREIGN KEY ([FromUserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Subscribr_To] FOREIGN KEY ([ToUserId]) REFERENCES [User]([Id])
)
