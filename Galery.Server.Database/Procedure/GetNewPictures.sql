CREATE PROCEDURE [dbo].[GetNewPictures]
	@skip int,
	@take int
AS
	SELECT p.Id, p.ImagePath, p.[Name], p.[UserId], p.DateOfCreation, u.UserName, u.Avatar
	FROM Picture as p join [User] u on p.UserId = u.Id
	order by p.DateOfCreation desc
		offset @skip rows
		fetch next @take rows only
