USE [master]
GO
/****** Object:  Database [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF]    Script Date: 18-Mar-16 11:58:12 AM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF')
BEGIN
CREATE DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WereViewApp', FILENAME = N'F:\Work\Git\WereViewProject\WereViewApp\App_Data\WereViewApp.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'WereViewApp_log', FILENAME = N'F:\Work\Git\WereViewProject\WereViewApp\App_Data\WereViewApp_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END

GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET ARITHABORT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET  DISABLE_BROKER 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET RECOVERY FULL 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET  MULTI_USER 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF]
GO
/****** Object:  StoredProcedure [dbo].[AppsSearch]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetAllTablesSpaceUsedInformation]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetAppDrafts]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetApps]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetAppsAndUsers]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetReviews]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetWholeSystem]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 18-Mar-16 11:58:12 AM ******/
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
/****** Object:  Table [dbo].[App]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[App]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[App](
	[AppID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppName] [varchar](60) NOT NULL,
	[PlatformID] [tinyint] NOT NULL,
	[PlatformVersion] [float] NOT NULL,
	[CategoryID] [smallint] NOT NULL,
	[Url] [varchar](70) NOT NULL,
	[Description] [nvarchar](2000) NOT NULL,
	[PostedByUserID] [bigint] NOT NULL,
	[ReviewsCount] [smallint] NOT NULL,
	[IsVideoExist] [bit] NOT NULL,
	[YoutubeEmbedLink] [varchar](255) NULL,
	[WebsiteUrl] [nvarchar](255) NULL,
	[StoreUrl] [nvarchar](255) NULL,
	[IsBlocked] [bit] NOT NULL,
	[IsPublished] [bit] NOT NULL,
	[UploadGuid] [uniqueidentifier] NOT NULL,
	[TotalViewed] [bigint] NOT NULL,
	[WebsiteClicked] [bigint] NOT NULL,
	[StoreClicked] [bigint] NOT NULL,
	[AvgRating] [float] NOT NULL,
	[ReleaseDate] [date] NOT NULL,
	[CreatedDate] [date] NOT NULL,
	[LastModifiedDate] [date] NULL,
	[UrlWithoutEscapseSequence] [varchar](70) NOT NULL,
	[IsMultipleVersion] [bit] NOT NULL,
	[TagsDisplay] [varchar](450) NOT NULL,
	[SupportedOSVersions] [varchar](120) NULL,
	[AppOfferTypeID] [tinyint] NULL,
	[Price] [float] NULL,
 CONSTRAINT [PK_App] PRIMARY KEY CLUSTERED 
(
	[AppID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [URLUnique] UNIQUE NONCLUSTERED 
(
	[PlatformID] ASC,
	[PlatformVersion] ASC,
	[Url] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AppDraft]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppDraft]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AppDraft](
	[AppDraftID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppName] [varchar](60) NULL,
	[PlatformID] [tinyint] NOT NULL,
	[CategoryID] [smallint] NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[PostedByUserID] [bigint] NOT NULL,
	[ReviewsCount] [smallint] NULL,
	[IsVideoExist] [bit] NULL,
	[YoutubeEmbedLink] [varchar](255) NULL,
	[WebsiteUrl] [nvarchar](255) NULL,
	[StoreUrl] [nvarchar](255) NULL,
	[IsBlocked] [bit] NULL,
	[IsPublished] [bit] NULL,
	[PlatformVersion] [float] NULL,
	[UploadGuid] [uniqueidentifier] NOT NULL,
	[TotalViewed] [bigint] NULL,
	[Url] [varchar](65) NULL,
	[ReleaseDate] [date] NULL,
	[IsMultipleVersion] [bit] NOT NULL,
	[TagsDisplay] [varchar](450) NOT NULL,
	[SupportedOSVersions] [varchar](80) NULL,
 CONSTRAINT [PK_AppDraft] PRIMARY KEY CLUSTERED 
(
	[AppDraftID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AppOfferType]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppOfferType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AppOfferType](
	[AppOfferTypeID] [tinyint] IDENTITY(1,1) NOT NULL,
	[OfferType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_AppOfferType] PRIMARY KEY CLUSTERED 
(
	[AppOfferTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Category]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[CategoryID] [smallint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](40) NOT NULL,
	[Slug] [varchar](40) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CellPhone]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CellPhone]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CellPhone](
	[CellPhoneID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[PlatformID] [tinyint] NOT NULL,
	[PlatformVersion] [float] NOT NULL,
 CONSTRAINT [PK_CellPhone] PRIMARY KEY CLUSTERED 
(
	[CellPhoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[FeaturedImage]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeaturedImage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FeaturedImage](
	[FeaturedImageID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppID] [bigint] NOT NULL,
	[IsFeatured] [bit] NOT NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK_FeaturedImage] PRIMARY KEY CLUSTERED 
(
	[FeaturedImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Gallery]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Gallery]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Gallery](
	[GalleryID] [uniqueidentifier] NOT NULL,
	[UploadGuid] [uniqueidentifier] NOT NULL,
	[GalleryCategoryID] [int] NOT NULL,
	[Sequence] [tinyint] NOT NULL,
	[Title] [varchar](50) NULL,
	[Subtitle] [varchar](150) NULL,
	[Extension] [varchar](5) NOT NULL,
 CONSTRAINT [PK_Gallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GalleryCategory]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GalleryCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GalleryCategory](
	[GalleryCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NOT NULL,
	[Width] [float] NOT NULL,
	[Height] [float] NOT NULL,
	[IsAdvertise] [bit] NOT NULL,
 CONSTRAINT [PK_GalleryCategory] PRIMARY KEY CLUSTERED 
(
	[GalleryCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Message]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Message]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Message](
	[MessageID] [bigint] IDENTITY(1,1) NOT NULL,
	[Msg1] [varchar](300) NOT NULL,
	[Msg2] [varchar](300) NULL,
	[Msg3] [varchar](300) NULL,
	[Msg4] [varchar](300) NULL,
	[MessageDisplay]  AS (concat([Msg1],[Msg2],[Msg3],[Msg4])),
	[SenderUserID] [bigint] NOT NULL,
	[ReceiverUserID] [bigint] NOT NULL,
	[IsDraft] [bit] NOT NULL,
	[LastModified] [smalldatetime] NOT NULL,
	[SentDate] [smalldatetime] NOT NULL,
	[ReceivedDate] [smalldatetime] NULL,
	[IsReceived] [bit] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MessageSeen]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessageSeen]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MessageSeen](
	[MessageSeenID] [bigint] IDENTITY(1,1) NOT NULL,
	[Msg1] [varchar](300) NOT NULL,
	[Msg2] [varchar](300) NULL,
	[Msg3] [varchar](300) NULL,
	[Msg4] [varchar](300) NULL,
	[MessageDisplay]  AS (concat([Msg1],[Msg2],[Msg3],[Msg4])),
	[SenderUserID] [bigint] NOT NULL,
	[ReceiverUserID] [bigint] NOT NULL,
	[IsDraft] [bit] NOT NULL,
	[LastModified] [smalldatetime] NOT NULL,
	[SentDate] [smalldatetime] NOT NULL,
	[ReceivedDate] [smalldatetime] NOT NULL,
	[IsReceived] [bit] NOT NULL,
 CONSTRAINT [PK_MessageSeen] PRIMARY KEY CLUSTERED 
(
	[MessageSeenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notification]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Notification](
	[NotificationID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[NotificationTypeID] [tinyint] NOT NULL,
	[Message] [varchar](250) NULL,
	[Dated] [smalldatetime] NOT NULL,
	[IsSeen] [bit] NOT NULL,
	[SeenDate] [smalldatetime] NULL,
	[IsUseDefaultMessage] [bit] NOT NULL,
	[HasClicked] [bit] NOT NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NotificationSeen]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationSeen]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[NotificationSeen](
	[NotificationSeenID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[NotificationTypeID] [tinyint] NOT NULL,
	[Message] [varchar](250) NULL,
	[Dated] [smalldatetime] NOT NULL,
	[IsSeen] [bit] NOT NULL,
	[SeenDate] [smalldatetime] NULL,
	[IsUseDefaultMessage] [bit] NOT NULL,
	[HasClicked] [bit] NOT NULL,
 CONSTRAINT [PK_LatestSeenNotification] PRIMARY KEY CLUSTERED 
(
	[NotificationSeenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[NotificationType](
	[NotificationTypeID] [tinyint] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NOT NULL,
	[IsGood] [bit] NOT NULL,
	[DefaultMessage] [varchar](250) NULL,
	[MessageIconName] [varchar](40) NOT NULL,
 CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED 
(
	[NotificationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Platform]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Platform]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Platform](
	[PlatformID] [tinyint] IDENTITY(1,1) NOT NULL,
	[PlatformName] [varchar](40) NOT NULL,
	[Icon] [varchar](50) NULL,
 CONSTRAINT [PK_Platform] PRIMARY KEY CLUSTERED 
(
	[PlatformID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Review]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Review]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Review](
	[ReviewID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](30) NOT NULL,
	[Pros] [varchar](300) NULL,
	[Cons] [varchar](300) NULL,
	[IsSuggest] [bit] NOT NULL,
	[Comment1] [varchar](100) NOT NULL,
	[Comment2] [varchar](500) NULL,
	[AppID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Comments]  AS (concat([Comment1],[Comment2])),
	[LikedCount] [int] NOT NULL,
	[DisLikeCount] [int] NOT NULL,
	[Rating] [tinyint] NOT NULL,
	[CreatedDate] [date] NOT NULL,
 CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReviewLikeDislike]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReviewLikeDislike]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ReviewLikeDislike](
	[ReviewLikeDislikeID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReviewID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[IsLiked] [bit] NOT NULL,
	[IsDisliked] [bit] NOT NULL,
	[IsNone] [bit] NOT NULL,
 CONSTRAINT [PK_ReviewLikeDislike] PRIMARY KEY CLUSTERED 
(
	[ReviewLikeDislikeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Subscribe]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subscribe]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Subscribe](
	[SubscribeID] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[IsSubscriberMemeber] [bit] NOT NULL,
	[SubscribeCategoryID] [smallint] NOT NULL,
	[SubcribeToUserID] [bigint] NULL,
	[LastNotifiedOnDate] [smalldatetime] NULL,
	[SubscribedByUserID] [bigint] NULL,
 CONSTRAINT [PK_Subscribe] PRIMARY KEY CLUSTERED 
(
	[SubscribeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SubscribeCategory]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubscribeCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SubscribeCategory](
	[SubscribeCategoryID] [smallint] IDENTITY(1,1) NOT NULL,
	[Category] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SubscribeCategory] PRIMARY KEY CLUSTERED 
(
	[SubscribeCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubscribeNotifiedHistory]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubscribeNotifiedHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SubscribeNotifiedHistory](
	[SubscribeNotifiedHistoryID] [bigint] IDENTITY(1,1) NOT NULL,
	[SubscribeID] [bigint] NOT NULL,
	[NotifiedOnDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SubscribeNotifiedHistory] PRIMARY KEY CLUSTERED 
(
	[SubscribeNotifiedHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tag](
	[TagID] [bigint] IDENTITY(1,1) NOT NULL,
	[TagDisplay] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TagAppRelation]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TagAppRelation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TagAppRelation](
	[TagAppRelationID] [bigint] IDENTITY(1,1) NOT NULL,
	[TagID] [bigint] NOT NULL,
	[AppID] [bigint] NOT NULL,
 CONSTRAINT [PK_TagAppRelation] PRIMARY KEY CLUSTERED 
(
	[TagAppRelationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TempUpload]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempUpload]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TempUpload](
	[TempUploadID] [uniqueidentifier] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[AppID] [bigint] NULL,
	[GalleryID] [uniqueidentifier] NOT NULL,
	[RelatingUploadGuidForDelete] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[TempUploadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[User]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[UserID] [bigint] NOT NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NOT NULL,
	[Phone] [varchar](18) NULL,
	[UserName] [varchar](30) NOT NULL,
	[TotalEarnedPoints] [bigint] NOT NULL,
	[DefaultCellPhoneID] [bigint] NULL,
	[SubscriberCount] [bigint] NOT NULL,
	[UploadGuid] [uniqueidentifier] NOT NULL,
	[HasPicture] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserPoint]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPoint]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserPoint](
	[UserPointID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Point] [int] NOT NULL,
	[UserPointSettingID] [tinyint] NOT NULL,
	[Dated] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_UserPoint] PRIMARY KEY CLUSTERED 
(
	[UserPointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[UserPointSetting]    Script Date: 18-Mar-16 11:58:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPointSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserPointSetting](
	[UserPointSettingID] [tinyint] IDENTITY(1,1) NOT NULL,
	[TaskName] [varchar](50) NOT NULL,
	[Point] [int] NOT NULL,
 CONSTRAINT [PK_UserPointSetting] PRIMARY KEY CLUSTERED 
(
	[UserPointSettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_App_1]    Script Date: 18-Mar-16 11:58:12 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[App]') AND name = N'IX_App_1')
CREATE UNIQUE NONCLUSTERED INDEX [IX_App_1] ON [dbo].[App]
(
	[UploadGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_App_TotalViewCount]    Script Date: 18-Mar-16 11:58:12 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[App]') AND name = N'IX_App_TotalViewCount')
CREATE NONCLUSTERED INDEX [IX_App_TotalViewCount] ON [dbo].[App]
(
	[TotalViewed] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Gallery]    Script Date: 18-Mar-16 11:58:12 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Gallery]') AND name = N'IX_Gallery')
CREATE NONCLUSTERED INDEX [IX_Gallery] ON [dbo].[Gallery]
(
	[UploadGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_GalleryCategory]    Script Date: 18-Mar-16 11:58:12 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[GalleryCategory]') AND name = N'IX_GalleryCategory')
CREATE NONCLUSTERED INDEX [IX_GalleryCategory] ON [dbo].[GalleryCategory]
(
	[GalleryCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_App_IsBlocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF_App_IsBlocked]  DEFAULT ((0)) FOR [IsBlocked]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_App_IsPublished]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF_App_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Websi__420DC656]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF__tmp_ms_xx__Websi__420DC656]  DEFAULT ((0)) FOR [WebsiteClicked]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Store__4301EA8F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF__tmp_ms_xx__Store__4301EA8F]  DEFAULT ((0)) FOR [StoreClicked]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__AvgRa__43F60EC8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF__tmp_ms_xx__AvgRa__43F60EC8]  DEFAULT ((0)) FOR [AvgRating]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_App_IsMultipleVersion]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF_App_IsMultipleVersion]  DEFAULT ((0)) FOR [IsMultipleVersion]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_App_TagsDisplay]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF_App_TagsDisplay]  DEFAULT ('a') FOR [TagsDisplay]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_App_SupportedOSVersions]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[App] ADD  CONSTRAINT [DF_App_SupportedOSVersions]  DEFAULT ('a') FOR [SupportedOSVersions]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__IsBlo__7DEDA633]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AppDraft] ADD  DEFAULT ((0)) FOR [IsBlocked]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__IsPub__7EE1CA6C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AppDraft] ADD  DEFAULT ((0)) FOR [IsPublished]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_AppDraft_IsMultipleVersion]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AppDraft] ADD  CONSTRAINT [DF_AppDraft_IsMultipleVersion]  DEFAULT ((0)) FOR [IsMultipleVersion]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_AppDraft_TagsDisplay]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AppDraft] ADD  CONSTRAINT [DF_AppDraft_TagsDisplay]  DEFAULT ('a') FOR [TagsDisplay]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_AppDraft_SupportedOSVersions]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AppDraft] ADD  CONSTRAINT [DF_AppDraft_SupportedOSVersions]  DEFAULT ('a') FOR [SupportedOSVersions]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__Category__Slug__222B06A9]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] ADD  DEFAULT ('None') FOR [Slug]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__IsAdv__269AB60B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GalleryCategory] ADD  DEFAULT ((0)) FOR [IsAdvertise]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Message_LastModified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_LastModified]  DEFAULT (getdate()) FOR [LastModified]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Message_SentDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_SentDate]  DEFAULT (getdate()) FOR [SentDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_MessageSeen_LastModified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[MessageSeen] ADD  CONSTRAINT [DF_MessageSeen_LastModified]  DEFAULT (getdate()) FOR [LastModified]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_MessageSeen_SentDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[MessageSeen] ADD  CONSTRAINT [DF_MessageSeen_SentDate]  DEFAULT (getdate()) FOR [SentDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Notification_IsUseDefaultMessage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_IsUseDefaultMessage]  DEFAULT ((0)) FOR [IsUseDefaultMessage]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Notification_HasClicked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_HasClicked]  DEFAULT ((0)) FOR [HasClicked]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_NotificationSeen_IsUseDefaultMessage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NotificationSeen] ADD  CONSTRAINT [DF_NotificationSeen_IsUseDefaultMessage]  DEFAULT ((0)) FOR [IsUseDefaultMessage]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_NotificationSeen_HasClicked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NotificationSeen] ADD  CONSTRAINT [DF_NotificationSeen_HasClicked]  DEFAULT ((0)) FOR [HasClicked]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__Notificat__IsGoo__2FBA0BF1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NotificationType] ADD  CONSTRAINT [DF__Notificat__IsGoo__2FBA0BF1]  DEFAULT ((0)) FOR [IsGood]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_NotificationType_MessageIconName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NotificationType] ADD  CONSTRAINT [DF_NotificationType_MessageIconName]  DEFAULT ('a') FOR [MessageIconName]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Liked__74CE504D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Review] ADD  DEFAULT ((0)) FOR [LikedCount]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__DisLi__75C27486]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Review] ADD  DEFAULT ((0)) FOR [DisLikeCount]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__Review__CreatedD__42CCE065]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Review] ADD  DEFAULT (getdate()) FOR [CreatedDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Total__01BE3717]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  DEFAULT ((0)) FOR [TotalEarnedPoints]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_User_SubscriberCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_SubscriberCount]  DEFAULT ((0)) FOR [SubscriberCount]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_User_UploadGuid]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_UploadGuid]  DEFAULT (newid()) FOR [UploadGuid]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_User_HasPicture]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_HasPicture]  DEFAULT ((0)) FOR [HasPicture]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserPoint_Point]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[UserPoint] ADD  CONSTRAINT [DF_UserPoint_Point]  DEFAULT ((0)) FOR [Point]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_AppOfferType]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App]  WITH NOCHECK ADD  CONSTRAINT [FK_App_AppOfferType] FOREIGN KEY([AppOfferTypeID])
REFERENCES [dbo].[AppOfferType] ([AppOfferTypeID])
NOT FOR REPLICATION 
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_AppOfferType]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App] NOCHECK CONSTRAINT [FK_App_AppOfferType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App]  WITH CHECK ADD  CONSTRAINT [FK_App_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App] CHECK CONSTRAINT [FK_App_Category]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_Platform]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App]  WITH CHECK ADD  CONSTRAINT [FK_App_Platform] FOREIGN KEY([PlatformID])
REFERENCES [dbo].[Platform] ([PlatformID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_Platform]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App] CHECK CONSTRAINT [FK_App_Platform]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App]  WITH CHECK ADD  CONSTRAINT [FK_App_User] FOREIGN KEY([PostedByUserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_App_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[App]'))
ALTER TABLE [dbo].[App] CHECK CONSTRAINT [FK_App_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CellPhone_Platform]') AND parent_object_id = OBJECT_ID(N'[dbo].[CellPhone]'))
ALTER TABLE [dbo].[CellPhone]  WITH CHECK ADD  CONSTRAINT [FK_CellPhone_Platform] FOREIGN KEY([PlatformID])
REFERENCES [dbo].[Platform] ([PlatformID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CellPhone_Platform]') AND parent_object_id = OBJECT_ID(N'[dbo].[CellPhone]'))
ALTER TABLE [dbo].[CellPhone] CHECK CONSTRAINT [FK_CellPhone_Platform]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CellPhone_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[CellPhone]'))
ALTER TABLE [dbo].[CellPhone]  WITH CHECK ADD  CONSTRAINT [FK_CellPhone_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CellPhone_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[CellPhone]'))
ALTER TABLE [dbo].[CellPhone] CHECK CONSTRAINT [FK_CellPhone_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FeaturedImage_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[FeaturedImage]'))
ALTER TABLE [dbo].[FeaturedImage]  WITH CHECK ADD  CONSTRAINT [FK_FeaturedImage_App] FOREIGN KEY([AppID])
REFERENCES [dbo].[App] ([AppID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FeaturedImage_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[FeaturedImage]'))
ALTER TABLE [dbo].[FeaturedImage] CHECK CONSTRAINT [FK_FeaturedImage_App]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FeaturedImage_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[FeaturedImage]'))
ALTER TABLE [dbo].[FeaturedImage]  WITH CHECK ADD  CONSTRAINT [FK_FeaturedImage_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FeaturedImage_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[FeaturedImage]'))
ALTER TABLE [dbo].[FeaturedImage] CHECK CONSTRAINT [FK_FeaturedImage_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Gallery_GalleryCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Gallery]'))
ALTER TABLE [dbo].[Gallery]  WITH CHECK ADD  CONSTRAINT [FK_Gallery_GalleryCategory] FOREIGN KEY([GalleryCategoryID])
REFERENCES [dbo].[GalleryCategory] ([GalleryCategoryID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Gallery_GalleryCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Gallery]'))
ALTER TABLE [dbo].[Gallery] CHECK CONSTRAINT [FK_Gallery_GalleryCategory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_User] FOREIGN KEY([SenderUserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_User1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_User1] FOREIGN KEY([ReceiverUserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_User1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_User1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageSeen_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageSeen]'))
ALTER TABLE [dbo].[MessageSeen]  WITH CHECK ADD  CONSTRAINT [FK_MessageSeen_User] FOREIGN KEY([SenderUserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageSeen_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageSeen]'))
ALTER TABLE [dbo].[MessageSeen] CHECK CONSTRAINT [FK_MessageSeen_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageSeen_User1]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageSeen]'))
ALTER TABLE [dbo].[MessageSeen]  WITH CHECK ADD  CONSTRAINT [FK_MessageSeen_User1] FOREIGN KEY([ReceiverUserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageSeen_User1]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageSeen]'))
ALTER TABLE [dbo].[MessageSeen] CHECK CONSTRAINT [FK_MessageSeen_User1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notification_NotificationType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notification]'))
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_NotificationType] FOREIGN KEY([NotificationTypeID])
REFERENCES [dbo].[NotificationType] ([NotificationTypeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notification_NotificationType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notification]'))
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_NotificationType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notification_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notification]'))
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notification_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notification]'))
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LatestSeenNotification_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[NotificationSeen]'))
ALTER TABLE [dbo].[NotificationSeen]  WITH CHECK ADD  CONSTRAINT [FK_LatestSeenNotification_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LatestSeenNotification_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[NotificationSeen]'))
ALTER TABLE [dbo].[NotificationSeen] CHECK CONSTRAINT [FK_LatestSeenNotification_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LatestUnSeenNotification_NotificationType]') AND parent_object_id = OBJECT_ID(N'[dbo].[NotificationSeen]'))
ALTER TABLE [dbo].[NotificationSeen]  WITH CHECK ADD  CONSTRAINT [FK_LatestUnSeenNotification_NotificationType] FOREIGN KEY([NotificationTypeID])
REFERENCES [dbo].[NotificationType] ([NotificationTypeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LatestUnSeenNotification_NotificationType]') AND parent_object_id = OBJECT_ID(N'[dbo].[NotificationSeen]'))
ALTER TABLE [dbo].[NotificationSeen] CHECK CONSTRAINT [FK_LatestUnSeenNotification_NotificationType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Review_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[Review]'))
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_App] FOREIGN KEY([AppID])
REFERENCES [dbo].[App] ([AppID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Review_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[Review]'))
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_App]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Review_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Review]'))
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Review_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Review]'))
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReviewLikeDislike_Review]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReviewLikeDislike]'))
ALTER TABLE [dbo].[ReviewLikeDislike]  WITH CHECK ADD  CONSTRAINT [FK_ReviewLikeDislike_Review] FOREIGN KEY([ReviewID])
REFERENCES [dbo].[Review] ([ReviewID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReviewLikeDislike_Review]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReviewLikeDislike]'))
ALTER TABLE [dbo].[ReviewLikeDislike] CHECK CONSTRAINT [FK_ReviewLikeDislike_Review]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReviewLikeDislike_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReviewLikeDislike]'))
ALTER TABLE [dbo].[ReviewLikeDislike]  WITH CHECK ADD  CONSTRAINT [FK_ReviewLikeDislike_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReviewLikeDislike_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReviewLikeDislike]'))
ALTER TABLE [dbo].[ReviewLikeDislike] CHECK CONSTRAINT [FK_ReviewLikeDislike_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscribe_SubscribeCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscribe]'))
ALTER TABLE [dbo].[Subscribe]  WITH CHECK ADD  CONSTRAINT [FK_Subscribe_SubscribeCategory] FOREIGN KEY([SubscribeCategoryID])
REFERENCES [dbo].[SubscribeCategory] ([SubscribeCategoryID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscribe_SubscribeCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscribe]'))
ALTER TABLE [dbo].[Subscribe] CHECK CONSTRAINT [FK_Subscribe_SubscribeCategory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscribe_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscribe]'))
ALTER TABLE [dbo].[Subscribe]  WITH NOCHECK ADD  CONSTRAINT [FK_Subscribe_User] FOREIGN KEY([SubcribeToUserID])
REFERENCES [dbo].[User] ([UserID])
NOT FOR REPLICATION 
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscribe_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscribe]'))
ALTER TABLE [dbo].[Subscribe] NOCHECK CONSTRAINT [FK_Subscribe_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscribe_User1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscribe]'))
ALTER TABLE [dbo].[Subscribe]  WITH CHECK ADD  CONSTRAINT [FK_Subscribe_User1] FOREIGN KEY([SubscribedByUserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscribe_User1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscribe]'))
ALTER TABLE [dbo].[Subscribe] CHECK CONSTRAINT [FK_Subscribe_User1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubscribeNotifiedHistory_Subscribe]') AND parent_object_id = OBJECT_ID(N'[dbo].[SubscribeNotifiedHistory]'))
ALTER TABLE [dbo].[SubscribeNotifiedHistory]  WITH CHECK ADD  CONSTRAINT [FK_SubscribeNotifiedHistory_Subscribe] FOREIGN KEY([SubscribeID])
REFERENCES [dbo].[Subscribe] ([SubscribeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubscribeNotifiedHistory_Subscribe]') AND parent_object_id = OBJECT_ID(N'[dbo].[SubscribeNotifiedHistory]'))
ALTER TABLE [dbo].[SubscribeNotifiedHistory] CHECK CONSTRAINT [FK_SubscribeNotifiedHistory_Subscribe]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TagAppRelation_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[TagAppRelation]'))
ALTER TABLE [dbo].[TagAppRelation]  WITH CHECK ADD  CONSTRAINT [FK_TagAppRelation_App] FOREIGN KEY([AppID])
REFERENCES [dbo].[App] ([AppID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TagAppRelation_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[TagAppRelation]'))
ALTER TABLE [dbo].[TagAppRelation] CHECK CONSTRAINT [FK_TagAppRelation_App]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TagAppRelation_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[TagAppRelation]'))
ALTER TABLE [dbo].[TagAppRelation]  WITH CHECK ADD  CONSTRAINT [FK_TagAppRelation_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TagAppRelation_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[TagAppRelation]'))
ALTER TABLE [dbo].[TagAppRelation] CHECK CONSTRAINT [FK_TagAppRelation_Tag]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_User_CellPhone]') AND parent_object_id = OBJECT_ID(N'[dbo].[User]'))
ALTER TABLE [dbo].[User]  WITH NOCHECK ADD  CONSTRAINT [FK_User_CellPhone] FOREIGN KEY([DefaultCellPhoneID])
REFERENCES [dbo].[CellPhone] ([CellPhoneID])
NOT FOR REPLICATION 
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_User_CellPhone]') AND parent_object_id = OBJECT_ID(N'[dbo].[User]'))
ALTER TABLE [dbo].[User] NOCHECK CONSTRAINT [FK_User_CellPhone]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPoint_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPoint]'))
ALTER TABLE [dbo].[UserPoint]  WITH CHECK ADD  CONSTRAINT [FK_UserPoint_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPoint_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPoint]'))
ALTER TABLE [dbo].[UserPoint] CHECK CONSTRAINT [FK_UserPoint_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPoint_UserPointSetting]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPoint]'))
ALTER TABLE [dbo].[UserPoint]  WITH CHECK ADD  CONSTRAINT [FK_UserPoint_UserPointSetting] FOREIGN KEY([UserPointSettingID])
REFERENCES [dbo].[UserPointSetting] ([UserPointSettingID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPoint_UserPointSetting]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPoint]'))
ALTER TABLE [dbo].[UserPoint] CHECK CONSTRAINT [FK_UserPoint_UserPointSetting]
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'App', N'COLUMN',N'Url'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'It contains "Hello World 2" => "hello-world-2"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App', @level2type=N'COLUMN',@level2name=N'Url'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'App', N'COLUMN',N'UrlWithoutEscapseSequence'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'It contains "Hello World 2" => "hello-world" escapse number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App', @level2type=N'COLUMN',@level2name=N'UrlWithoutEscapseSequence'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'App', N'COLUMN',N'SupportedOSVersions'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'It will contain coma seperated values of OS platform values.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'App', @level2type=N'COLUMN',@level2name=N'SupportedOSVersions'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AppDraft', N'COLUMN',N'SupportedOSVersions'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'It will contain coma seperated values of OS platform values.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppDraft', @level2type=N'COLUMN',@level2name=N'SupportedOSVersions'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Gallery', N'COLUMN',N'UploadGuid'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UploadGuid can be duplicated, it defines same gallery images.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gallery', @level2type=N'COLUMN',@level2name=N'UploadGuid'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Gallery', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Galleries are disjoint because all types of gallery images information will be stored there.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gallery'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Notification', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Only contains notifications are not (clicked or seens) and 7 days have passed after clicked and seen.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'NotificationSeen', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Keeps old notifications history' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NotificationSeen'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ReviewLikeDislike', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Review like dislike database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReviewLikeDislike'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Subscribe', N'COLUMN',N'IsSubscriberMemeber'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Means if the subscriber is a registered user.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Subscribe', @level2type=N'COLUMN',@level2name=N'IsSubscriberMemeber'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Subscribe', N'COLUMN',N'SubscribeCategoryID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subscribe category : Is is app subscribe or web subscribe' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Subscribe', @level2type=N'COLUMN',@level2name=N'SubscribeCategoryID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Subscribe', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Website or user or app subscriber' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Subscribe'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubscribeCategory', N'COLUMN',N'SubscribeCategoryID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subscribe category : Is is app subscribe or web subscribe' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubscribeCategory', @level2type=N'COLUMN',@level2name=N'SubscribeCategoryID'
GO
USE [master]
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP.MDF] SET  READ_WRITE 
GO
