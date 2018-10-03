CREATE PROCEDURE [dbo].[GetTagsWithPicCount]
AS
	SELECT count(p.PictureId) as PictureCount, t.*
	FROM [Tag] t JOIN [PictureTag] p on t.Id = p.TagId
