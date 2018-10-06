CREATE PROCEDURE [dbo].[GetTagsWithPicCount]
AS
	SELECT count(p.PictureId) as PictureCount, t.*
	FROM [Tag] t LEFT JOIN [PictureTag] p on t.Id = p.TagId
	GROUP BY t.Id, t.DateOfCreation, t.[Name]
