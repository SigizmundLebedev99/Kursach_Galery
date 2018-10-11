CREATE PROCEDURE [dbo].[GetSubscribesForUser]
	@userId int
AS
	SELECT u.Avatar, u.UserName, u.Id
	FROM Subscribe s join [User] u on s.ToUserId = u.Id
	WHERE s.FromUserId = @userId
