CREATE PROCEDURE [dbo].[GetCommentsForPicture]
	@pictureId int
AS
	SELECT u.UserName, u.Avatar, c.*
	FROM [Comment] as c join [User] as u on c.UserId = u.Id
	WHERE c.PictureId = @pictureId
	order by c.DateOfCreation desc 

