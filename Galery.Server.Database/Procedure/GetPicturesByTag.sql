CREATE PROCEDURE [dbo].[GetPicturesByTag]
	@tagId int,
	@skip int, 
	@take int
AS
	SELECT u.Avatar, u.UserName, Pics.* 
	FROM(SELECT p.Id, p.ImagePath, p.[Name], p.[UserId], p.DateOfCreation
	FROM PictureTag as pt join Picture as p on pt.PictureId = p.Id
	WHERE pt.TagId = @tagId
	order by pt.PictureId
		offset @skip rows
		fetch next @take rows only) as Pics JOIN [User] as u on Pics.UserId = u.Id