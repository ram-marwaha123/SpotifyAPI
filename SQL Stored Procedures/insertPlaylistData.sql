USE [myDatabase]
GO

/****** Object:  StoredProcedure [dbo].[insertPlaylistData]    Script Date: 27/07/2018 12:07:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[insertPlaylistData] (
@Playlist varchar(200),
@Title varchar(200), 
@Artist varchar(300), 
@Album varchar(200),
@Explicit bit,
@Duration time(2), 
@Popularity tinyint,
@Release_Date date,
@ID varchar(200) )
AS
BEGIN
SET NOCOUNT ON
SET @Playlist = RTRIM(@Playlist)
SET @Title = RTRIM(@Title)
SET @Artist = RTRIM(@Artist)
SET @Album = RTRIM(@Album)
SET @Explicit = RTRIM(@Explicit)
SET @Duration = RTRIM(@Duration)
SET @Popularity = RTRIM(@Popularity)
SET @Release_Date = RTRIM(@Release_Date)
SET @ID = RTRIM(@ID)
IF (SELECT COUNT(*) FROM dbo.Playlists WHERE ID =  @ID AND Playlist = @Playlist) = 0
	INSERT INTO dbo.Playlists (Playlist, Title, Artist, Album, [Explicit], Duration, Popularity, ReleaseDate, ID)
	VALUES (@Playlist, @Title, @Artist, @Album, @Explicit, @Duration, @Popularity, @Release_Date, @ID)
END
GO

