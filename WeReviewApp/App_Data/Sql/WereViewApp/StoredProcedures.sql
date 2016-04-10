USE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF]
GO
/****** Object:  StoredProcedure [dbo].[ResetWholeSystem]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetWholeSystem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetWholeSystem]
GO
/****** Object:  StoredProcedure [dbo].[ResetReviews]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetReviews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetReviews]
GO
/****** Object:  StoredProcedure [dbo].[ResetAppsAndUsers]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppsAndUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetAppsAndUsers]
GO
/****** Object:  StoredProcedure [dbo].[ResetApps]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetApps]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetApps]
GO
/****** Object:  StoredProcedure [dbo].[ResetAppDrafts]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppDrafts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetAppDrafts]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTablesSpaceUsedInformation]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTablesSpaceUsedInformation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAllTablesSpaceUsedInformation]
GO
/****** Object:  StoredProcedure [dbo].[AppsSearch]    Script Date: 14-Mar-16 3:37:30 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppsSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AppsSearch]
GO
/****** Object:  StoredProcedure [dbo].[AppsSearch]    Script Date: 14-Mar-16 3:37:30 PM ******/
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
            SELECT  *
            FROM    SplitString(''Querying SQL Server'', '' '');
      END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllTablesSpaceUsedInformation]    Script Date: 14-Mar-16 3:37:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllTablesSpaceUsedInformation]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Md. Alim Ul Karim
-- Create date: 14 Mar 2016
-- Description:	Get all tables spaced used information.
-- =============================================
CREATE PROCEDURE [dbo].[GetAllTablesSpaceUsedInformation] 
AS
BEGIN
    EXECUTE sp_MSforeachtable ''EXECUTE SP_SPACEUSED [?];'';
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetAppDrafts]    Script Date: 14-Mar-16 3:37:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppDrafts]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetAppDrafts]
AS 
      DELETE    FROM AppDraft;
      DBCC checkident (''AppDraft'', reseed, 0);
      RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetApps]    Script Date: 14-Mar-16 3:37:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetApps]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetApps]
AS 
      DELETE    FROM ReviewLikeDislike;
      DELETE    FROM Review;

      DBCC checkident (''Review'', reseed, 0);
      DBCC checkident (''ReviewLikeDislike'', reseed, 0);

      DELETE    FROM TagAppRelation;
      DELETE    FROM FeaturedImage;
      DELETE    FROM Tag;
      DELETE    FROM TempUpload;
      DELETE    FROM Review;
      DELETE    FROM Gallery;
      DELETE    FROM AppDraft;
      DELETE    FROM App;
      DELETE    FROM [User];
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
/****** Object:  StoredProcedure [dbo].[ResetAppsAndUsers]    Script Date: 14-Mar-16 3:37:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAppsAndUsers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetAppsAndUsers]
AS 
      DELETE    FROM App;	
      DELETE    FROM [User];	
		
      DBCC checkident(''App'', reseed, 0);
      DBCC checkident(''[User]'', reseed, 0);
      RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetReviews]    Script Date: 14-Mar-16 3:37:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetReviews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetReviews]
AS 
      DELETE    FROM ReviewLikeDislike;
      DELETE    FROM Review;

      DBCC checkident (''Review'', reseed, 0);
      DBCC checkident (''ReviewLikeDislike'', reseed, 0);
      RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetWholeSystem]    Script Date: 14-Mar-16 3:37:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetWholeSystem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetWholeSystem]
AS 
      BEGIN

            TRUNCATE TABLE dbo.MessageSeen;
            TRUNCATE TABLE LatestSeenNotification;
            TRUNCATE TABLE dbo.[Message];
            TRUNCATE TABLE dbo.UserPoint;

            DBCC checkident (''[MessageSeen]'', reseed, 0);
            DBCC checkident (''[LatestSeenNotification]'', reseed, 0);
            DBCC checkident (''[Message]'', reseed, 0);
            DBCC checkident (''[UserPoint]'', reseed, 0);

            EXEC dbo.ResetApps; -- removes reviews, apps, Tag,TagAppRelation,TempUpload,Gallery,User,FeaturedImage

      END' 
END
GO
