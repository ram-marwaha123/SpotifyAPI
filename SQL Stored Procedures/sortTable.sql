USE [myDatabase]
GO

/****** Object:  StoredProcedure [dbo].[sortTable]    Script Date: 27/07/2018 12:08:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sortTable] @playlist_name varchar(150), @column_name varchar(100), @asc_or_desc varchar(30)
AS
BEGIN
SET NOCOUNT ON;
SET @playlist_name = RTRIM(@playlist_name)
SET @column_name = RTRIM(@column_name)
SET @asc_or_desc = RTRIM(@asc_or_desc)
DECLARE @query AS nvarchar(MAX)
SET @query = N'SELECT * FROM dbo.Playlists' +
' WHERE Playlist = ''' + @playlist_name + ''' OR ''' + @playlist_name + ''' = ''''' +
' ORDER BY ' + @column_name + ' ' + @asc_or_desc
EXEC sp_executesql @query
END
GO

