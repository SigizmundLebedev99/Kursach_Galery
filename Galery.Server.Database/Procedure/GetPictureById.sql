CREATE PROCEDURE [dbo].[GetPictureById]
	@id int,
	@userId int
AS
	SELECT u.Avatar, u.UserName, Pics.* 
	FROM (
		SELECT count(pl.UserId) as Likes,count(c.Id) as Comments, p.*,
		iif(@userId = any (select UserId from [PictureLikes] where PictureId = p.Id),1,0) as IsLiked
		FROM [Picture] p 
		LEFT JOIN [Comment] c on c.PictureId = p.Id
		LEFT JOIN [PictureLikes] pl on pl.PictureId = p.Id
		group by p.[Id], p.[DateOfCreation], p.[Description], p.[ImagePath], p.[Name], p.[UserId]
		having p.Id = @id) AS Pics 
	JOIN [User] u on u.Id = Pics.UserId