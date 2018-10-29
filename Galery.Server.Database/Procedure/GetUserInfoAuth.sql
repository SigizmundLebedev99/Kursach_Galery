CREATE PROCEDURE [dbo].[GetUserInfoAuth]
	@userId int,
	@fromUserId int
AS
	SELECT count(s.FromUserId) as SubscribersCount, u.Id, u.Avatar, u.UserName,
		iif(@userId = any (select [ToUserId] from [Subscribe] where [FromUserId] = @fromUserId),1,0) as IsInSubscribes
	FROM [User] u left join Subscribe s on s.ToUserId = u.Id
	group by u.Id, u.Avatar, u.UserName
	having u.Id = @userId
