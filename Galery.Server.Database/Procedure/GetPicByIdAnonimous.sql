CREATE PROCEDURE [dbo].[GetPicByIdAnonimous]
	@id int
AS
	SELECT u.Avatar, u.UserName, Pics.* 
	FROM (
		SELECT count(pl.UserId) as Likes,count(c.Id) as Comments, p.*, 0 as IsLiked
		FROM [Picture] p 
		LEFT JOIN [Comment] c on c.PictureId = p.Id
		LEFT JOIN [PictureLikes] pl on pl.PictureId = p.Id
		group by p.[Id], p.[DateOfCreation], p.[Description], p.[ImagePath], p.[Name], p.[UserId]
		having p.Id = 1) AS Pics 
	JOIN [User] u on u.Id = Pics.UserId
