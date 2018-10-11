CREATE PROCEDURE [dbo].[GetUserInfo]
	@userId int
AS
	SELECT count(s.FromUserId) as SubscribersCount, u.Id, u.Avatar, u.UserName
	FROM [User] u left join Subscribe s on s.ToUserId = u.Id
	group by u.Id, u.Avatar, u.UserName
	having u.Id = @userId
