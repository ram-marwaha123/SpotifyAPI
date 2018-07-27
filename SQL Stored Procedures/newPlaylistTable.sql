USE [myDatabase]
GO

/****** Object:  StoredProcedure [dbo].[newPlaylistTable]    Script Date: 27/07/2018 12:08:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[newPlaylistTable] @name varchar(MAX)
AS
BEGIN
SET NOCOUNT ON
SET @name = RTRIM(@name)
DECLARE @cmd AS nvarchar(MAX)
SET @cmd = N'CREATE TABLE ' + @name +
' (Title varchar(150) NOT NULL,' +
' Artist varchar(200) NOT NULL,' +
' Album varchar(150) NOT NULL,' +
' [Explicit] bit NOT NULL,' +
' Duration time(2) NOT NULL,' +
' Popularity tinyint NOT NULL,' +
' ReleaseDate date NOT NULL,' +
' ID varchar(200) NOT NULL)'
EXEC sp_executesql @cmd
END
GO

