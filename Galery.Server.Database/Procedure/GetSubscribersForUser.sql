CREATE PROCEDURE [dbo].[GetSubscribersForUser]
	@userId int
AS
	SELECT u.Avatar, u.UserName, u.Id
	FROM Subscribe s join [User] u on s.FromUserId = u.Id
	WHERE s.ToUserId = @userId
