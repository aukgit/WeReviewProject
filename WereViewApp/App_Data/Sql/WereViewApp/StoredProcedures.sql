USE [D:\WORKING\GITHUB\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF]
GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SplitString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[SplitString]
GO
/****** Object:  StoredProcedure [dbo].[ResetWholeSystem]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetWholeSystem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetWholeSystem]
GO
/****** Object:  StoredProcedure [dbo].[ResetReviews]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetReviews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetReviews]
GO
/****** Object:  StoredProcedure [dbo].[ResetAppsAndUsers]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppsAndUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetAppsAndUsers]
GO
/****** Object:  StoredProcedure [dbo].[ResetApps]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetApps]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetApps]
GO
/****** Object:  StoredProcedure [dbo].[ResetAppDrafts]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppDrafts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetAppDrafts]
GO
/****** Object:  StoredProcedure [dbo].[AppsSearch]    Script Date: 23-Feb-16 2:22:47 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppsSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AppsSearch]
GO
/****** Object:  StoredProcedure [dbo].[AppsSearch]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppsSearch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Alim Ul Karim
-- Create date: 
-- Description:	Search for apps
-- =============================================
CREATE PROCEDURE [dbo].[AppsSearch] 
	-- Add the parameters for the stored procedure here
	@SearchText VARCHAR(200) = 0
AS
BEGIN
	
	SET NOCOUNT ON;
	select *from SplitString(''Querying SQL Server'','' '') ;
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetAppDrafts]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppDrafts]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetAppDrafts]

AS
	Delete from AppDraft;
	DBCC checkident (''AppDraft'', reseed, 0);
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetApps]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetApps]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetApps]

AS
	Delete from ReviewLikeDislike;
	Delete from Review;

	DBCC checkident (''Review'', reseed, 0);
	DBCC checkident (''ReviewLikeDislike'', reseed, 0);

	Delete from TagAppRelation;
	Delete from FeaturedImage;
	Delete from Tag;
	Delete from TempUpload;
	Delete from Review;
	Delete from Gallery;
	Delete from AppDraft;
	Delete from App;
	Delete from [User];
	DBCC checkident (''App'', reseed, 0);
	DBCC checkident (''AppDraft'', reseed, 0);
	DBCC checkident (''Tag'', reseed, 0);
	DBCC checkident (''TagAppRelation'', reseed, 0);
	DBCC checkident (''TempUpload'', reseed, 0);
	DBCC checkident (''Review'', reseed, 0);
	DBCC checkident (''Gallery'', reseed, 0);
	DBCC checkident (''[User]'', reseed, 0);
	DBCC checkident (''FeaturedImage'', reseed, 0);

	
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetAppsAndUsers]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppsAndUsers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetAppsAndUsers]

AS
	Delete from App;	
	Delete from [User];	
		
	DBCC checkident(''App'', reseed, 0);
	DBCC checkident(''[User]'', reseed, 0);
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetReviews]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetReviews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetReviews]

AS
	Delete from ReviewLikeDislike;
	Delete from Review;

	DBCC checkident (''Review'', reseed, 0);
	DBCC checkident (''ReviewLikeDislike'', reseed, 0);
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetWholeSystem]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetWholeSystem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetWholeSystem]
AS begin

	truncate table dbo.MessageSeen;
    truncate table LatestSeenNotification;
	truncate table dbo.[Message];
	truncate table dbo.UserPoint;

	DBCC checkident (''[MessageSeen]'', reseed, 0);
	DBCC checkident (''[LatestSeenNotification]'', reseed, 0);
	DBCC checkident (''[Message]'', reseed, 0);
	DBCC checkident (''[UserPoint]'', reseed, 0);

	Exec dbo.ResetApps; -- removes reviews, apps, Tag,TagAppRelation,TempUpload,Gallery,User,FeaturedImage

End' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 23-Feb-16 2:22:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SplitString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Alim Ul Karim
-- Create date: 20 Sep 2014
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[SplitString]( @stringToSplit VARCHAR(200),@splitChar VARCHAR(1) )
RETURNS
 @returnList TABLE (ID int,[Name] [nvarchar] (500))
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT
 DECLARE @i INT
 SELECT  @i = 0
 WHILE CHARINDEX(@splitChar, @stringToSplit) > 0
 BEGIN
 
  SELECT @pos  = CHARINDEX(@splitChar, @stringToSplit)  ;
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1);
  
  

  INSERT INTO @returnList 
  SELECT  @i,@name;
  
  SELECT @i =@i + 1;

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @i,@stringToSplit

 RETURN
END
' 
END

GO
