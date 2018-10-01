CREATE PROCEDURE [dbo].[GetLikedByUser]
	@userId int,
	@skip int, 
	@take int
AS
	SELECT u.Avatar, u.UserName, Pics.* 
	FROM(SELECT p.Id, p.ImagePath, p.[Name], p.[UserId]
	FROM PictureLikes as pl join Picture as p on pl.PictureId = p.Id
	WHERE pl.UserId = @userId
	order by pl.UserId
		offset @skip rows
		fetch next @take rows only) as Pics JOIN [User] as u on Pics.UserId = u.Id
RETURN 0
