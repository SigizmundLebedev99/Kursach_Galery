CREATE PROCEDURE [dbo].[GetTopPicturesInfo]
	@skip int = 0,
	@take int
AS
	SELECT u.Avatar, u.UserName, Pics.* FROM (SELECT count(pl.UserId) as Likes, p.* 
	FROM [Picture] p
	LEFT JOIN [PictureLikes] pl on pl.PictureId = p.Id
	group by p.[Id], p.[DateOfCreation], p.[Description], p.[ImagePath], p.[Name], p.[UserId]
	order by Likes desc
		offset @skip rows
		fetch next @take rows only) AS Pics JOIN [User] u on u.Id = Pics.UserId
