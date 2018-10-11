CREATE PROCEDURE [dbo].[GetPicsFromSubscribes]
	@userId int,
	@skip int,
	@take int
AS
	SELECT u.Avatar, u.UserName, p.Id, p.ImagePath, p.[Name], p.UserId, p.DateOfCreation
	FROM [User] u 
	JOIN Picture p on p.UserId = u.Id 
	JOIN Subscribe s on s.ToUserId = u.Id
	WHERE s.FromUserId = @userId
	order by p.DateOfCreation desc
		offset @skip rows
		fetch next @take rows only
	
	
