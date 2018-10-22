CREATE PROCEDURE [dbo].[UserSearch]
	@search nvarchar
AS
	SELECT u.Avatar, u.UserName, u.Id
	FROM [User] u
	WHERE u.NormalizedUserName like @search + '%' and u.EmailConfirmed = 1 