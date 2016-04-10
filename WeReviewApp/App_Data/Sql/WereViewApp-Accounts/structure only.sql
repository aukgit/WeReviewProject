USE [master]
GO
/****** Object:  Database [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF]    Script Date: 14-Mar-16 3:38:41 PM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF')
BEGIN
CREATE DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WeReviewApp-Accounts', FILENAME = N'F:\Work\Git\WereViewProject\WereViewApp\App_Data\WereViewApp-Accounts.mdf' , SIZE = 9216KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'WeReviewApp-Accounts_log', FILENAME = N'F:\Work\Git\WereViewProject\WereViewApp\App_Data\WeReviewApp-Accounts_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END

GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET ARITHABORT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET  ENABLE_BROKER 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET  MULTI_USER 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF]
GO
/****** Object:  StoredProcedure [dbo].[CleanWholeSystem]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CleanWholeSystem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[CleanWholeSystem]
AS 
      BEGIN
            TRUNCATE TABLE FeedbackAppReviewRelation;
            TRUNCATE TABLE Feedback;
            TRUNCATE TABLE TempUserRoleRelation;

            EXEC ResetASPNetUser;
            EXEC ResetFeedbacks;

            UPDATE  CoreSetting
            SET     CoreSetting.IsFirstUserFound = 0
            WHERE   CoreSetting.CoreSettingID = 1;
    
      END' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllTablesSpaceUsedInformation]    Script Date: 14-Mar-16 3:38:41 PM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetASPNetUser]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetASPNetUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetASPNetUser]
AS 
      DELETE    FROM RegisterCodeUserRelation;
      DELETE    FROM AspNetUserRoles;
      DELETE    FROM AspNetUsers;
	
      UPDATE    CoreSetting
      SET       IsFirstUserFound = 0
      WHERE     CoreSettingID = 1;
		
      DBCC checkident(''AspNetUserRoles'', reseed, 0);
      DBCC checkident(''RegisterCodeUserRelation'', reseed, 0);
      DBCC checkident(''AspNetUsers'', reseed, 0);
      RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetCoreSettings]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetCoreSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetCoreSettings]
AS 
      DELETE    FROM CoreSetting;
      DBCC checkident (''CoreSetting'', reseed, 0);
      RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetFeedbacks]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetFeedbacks]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetFeedbacks]
AS 
      DELETE    FROM FeedbackAppReviewRelation;
      DELETE    FROM Feedback;
      DBCC checkident (''FeedbackAppReviewRelation'', reseed, 0);
      DBCC checkident (''Feedback'', reseed, 0);

      RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetImageResize]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetImageResize]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetImageResize]

AS
	Delete from ImageResizeSetting;
	DBCC checkident (''ImageResizeSetting'', reseed, 0);
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetRoles]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetRoles]
AS 
      DELETE    FROM AspNetUserRoles;
      DELETE    FROM AspNetRoles;
	
      DBCC checkident(''AspNetUserRoles'', reseed, 0);
      DBCC checkident(''AspNetRoles'', reseed, 0);
      RETURN 0' 
END
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[PriorityLevel] [tinyint] NOT NULL,
	[CanBeAcheivedByPoint] [bit] NOT NULL,
	[PointsRequired] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](20) NULL,
	[ClaimValue] [nvarchar](80) NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUsers](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](15) NOT NULL,
	[LastName] [varchar](15) NOT NULL,
	[UserTimeZoneID] [int] NULL,
	[Email] [varchar](256) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [varchar](70) NULL,
	[SecurityStamp] [varchar](38) NULL,
	[PhoneNumber] [varchar](30) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [date] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [varchar](30) NOT NULL,
	[CreatedDate] [smalldatetime] NOT NULL,
	[IsRegistrationComplete] [bit] NOT NULL,
	[CountryID] [int] NULL,
	[CountryLanguageID] [int] NULL,
	[GeneratedGuid] [uniqueidentifier] NULL,
	[IsBlocked] [bit] NOT NULL,
	[BlockingReason] [varchar](20) NULL,
	[BlockedbyUserId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CoreSetting]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CoreSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CoreSetting](
	[CoreSettingID] [tinyint] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](100) NOT NULL,
	[ApplicationSubtitle] [nvarchar](200) NOT NULL,
	[ApplicationDescription] [nvarchar](300) NOT NULL,
	[CompanyName] [nvarchar](120) NOT NULL,
	[Language] [varchar](35) NOT NULL,
	[LiveUrl] [nvarchar](500) NOT NULL,
	[AdminLocation] [nvarchar](20) NOT NULL,
	[TestingUrl] [nvarchar](500) NOT NULL,
	[AdminEmail] [nvarchar](256) NOT NULL,
	[DeveloperEmail] [nvarchar](256) NOT NULL,
	[IsAuthenticationEnabled] [bit] NOT NULL,
	[IsInTestingEnvironment] [bit] NOT NULL,
	[DoesRegisterCodeNeverExpires] [bit] NOT NULL,
	[IsRegisterCodeRequiredToRegister] [bit] NOT NULL,
	[ShouldRegistrationCodeBeLinkedWithUser] [bit] NOT NULL,
	[Address] [nvarchar](400) NOT NULL,
	[OfficeEmail] [varchar](256) NOT NULL,
	[GoogleMetaTag] [varchar](45) NOT NULL,
	[SmtpHost] [varchar](25) NULL,
	[SmtpMailPort] [int] NOT NULL,
	[IsSMTPSSL] [bit] NOT NULL,
	[PageItems] [bigint] NOT NULL,
	[SenderEmail] [nvarchar](256) NOT NULL,
	[SenderEmailPassword] [varchar](50) NOT NULL,
	[SenderDisplay] [nvarchar](256) NOT NULL,
	[IsConfirmMailRequired] [bit] NOT NULL,
	[NotifyDeveloperOnError] [bit] NOT NULL,
	[FacebookClientID] [bigint] NOT NULL,
	[FacebookSecret] [varchar](40) NULL,
	[IsFacebookAuthentication] [bit] NOT NULL,
	[AdminName] [varchar](60) NULL,
	[OfficePhone] [bigint] NOT NULL,
	[Fax] [bigint] NOT NULL,
	[SupportEmail] [varchar](256) NOT NULL,
	[SupportPhone] [bigint] NOT NULL,
	[MarketingEmail] [varchar](256) NOT NULL,
	[MarketingPhone] [bigint] NOT NULL,
	[IsFirstUserFound] [bit] NOT NULL,
	[GalleryMaxPictures] [tinyint] NOT NULL,
	[MaxDraftPostByUsers] [tinyint] NOT NULL,
	[ServicesControllerUrl] [nvarchar](500) NOT NULL,
	[ApiControllerUrl] [nvarchar](500) NOT NULL,
	[DeveloperName] [nvarchar](120) NOT NULL,
 CONSTRAINT [PK_dbo.CoreSetting] PRIMARY KEY CLUSTERED 
(
	[CoreSettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Country]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Country]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Country](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
	[Capital] [nvarchar](25) NULL,
	[Alpha2Code] [char](2) NOT NULL,
	[Alpha3Code] [char](3) NOT NULL,
	[CallingCode] [int] NOT NULL,
	[NationalityName] [varchar](50) NULL,
	[Region] [varchar](20) NULL,
	[SubRegion] [varchar](25) NULL,
	[Population] [bigint] NOT NULL,
	[Area] [real] NOT NULL,
	[Culture] [nvarchar](12) NULL,
	[GiniCoefficient] [real] NOT NULL,
	[Relevance] [real] NOT NULL,
	[LatitudeStartingPoint] [real] NOT NULL,
	[LatitudeEndingPoint] [real] NOT NULL,
	[RelatedTimeZoneID] [int] NULL,
	[IsSingleTimeZone] [bit] NOT NULL,
	[DisplayCountryName] [varchar](60) NOT NULL,
 CONSTRAINT [PK_dbo.Country] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryAlternativeName]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryAlternativeName]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryAlternativeName](
	[CountryAlternativeNameID] [int] IDENTITY(1,1) NOT NULL,
	[AlternativeName] [nvarchar](80) NOT NULL,
	[CountryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryAlternativeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CountryBorder]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryBorder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryBorder](
	[CountryBorderID] [int] IDENTITY(1,1) NOT NULL,
	[BorderCountryID] [int] NOT NULL,
	[CountryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryBorderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CountryCurrency]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryCurrency]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryCurrency](
	[CountryCurrencyID] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyName] [varchar](7) NOT NULL,
	[CountryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryCurrencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryDetectByIP]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryDetectByIP]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryDetectByIP](
	[CountryDetectByIPID] [int] IDENTITY(1,1) NOT NULL,
	[BeginingIP] [bigint] NOT NULL,
	[EndingIP] [bigint] NOT NULL,
	[CountryID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CountryDetectByIP] PRIMARY KEY CLUSTERED 
(
	[CountryDetectByIPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CountryDomain]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryDomain]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryDomain](
	[CountryDomainID] [int] IDENTITY(1,1) NOT NULL,
	[Domain] [varchar](6) NOT NULL,
	[CountryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryDomainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryLanguage]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryLanguage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryLanguage](
	[CountryLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[Language] [varchar](50) NULL,
	[Code] [varchar](3) NULL,
	[NativeName] [nvarchar](60) NULL,
 CONSTRAINT [PK_dbo.CountryLanguage] PRIMARY KEY CLUSTERED 
(
	[CountryLanguageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryLanguageRelation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryLanguageRelation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryLanguageRelation](
	[CountryLanguageRelationID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[CountryLanguageID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryLanguageRelationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CountryTimezoneRelation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryTimezoneRelation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryTimezoneRelation](
	[CountryTimezoneRelationID] [int] IDENTITY(1,1) NOT NULL,
	[UserTimeZoneID] [int] NOT NULL,
	[CountryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryTimezoneRelationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CountryTranslation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryTranslation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CountryTranslation](
	[CountryTranslationID] [int] IDENTITY(1,1) NOT NULL,
	[CountryLanguageID] [int] NOT NULL,
	[Translation] [nvarchar](50) NOT NULL,
	[CountryID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CountryTranslation] PRIMARY KEY CLUSTERED 
(
	[CountryTranslationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Feedback](
	[FeedbackID] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](30) NULL,
	[Name] [varchar](30) NULL,
	[Subject] [varchar](150) NOT NULL,
	[Message] [varchar](800) NOT NULL,
	[Email] [varchar](256) NOT NULL,
	[RateUrgency] [real] NOT NULL,
	[Response] [varchar](256) NULL,
	[IsViewed] [bit] NOT NULL,
	[IsInProcess] [bit] NOT NULL,
	[IsSolved] [bit] NOT NULL,
	[IsUnSolved] [bit] NOT NULL,
	[HasMarkedToFollowUpDate] [bit] NOT NULL,
	[PostedDate] [datetime2](7) NOT NULL,
	[FollowUpdateDate] [date] NULL,
	[FeedbackCategoryID] [tinyint] NOT NULL,
	[HasAppOrReviewReport] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Feedback] PRIMARY KEY CLUSTERED 
(
	[FeedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FeedbackAppReviewRelation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeedbackAppReviewRelation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FeedbackAppReviewRelation](
	[FeedbackAppReviewRelationID] [bigint] IDENTITY(1,1) NOT NULL,
	[FeedbackID] [bigint] NOT NULL,
	[AppID] [bigint] NOT NULL,
	[ReviewID] [bigint] NOT NULL,
	[HasAppId] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.FeedbackAppReviewRelation] PRIMARY KEY CLUSTERED 
(
	[FeedbackAppReviewRelationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[FeedbackCategory]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeedbackCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FeedbackCategory](
	[FeedbackCategoryID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Category] [varchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.FeedbackCategory] PRIMARY KEY CLUSTERED 
(
	[FeedbackCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ImageResizeSetting]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImageResizeSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ImageResizeSetting](
	[ImageResizeSettingID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](256) NOT NULL,
	[Height] [real] NOT NULL,
	[Width] [real] NOT NULL,
	[Extension] [varchar](5) NOT NULL,
 CONSTRAINT [PK_dbo.ImageResizeSetting] PRIMARY KEY CLUSTERED 
(
	[ImageResizeSettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Navigation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Navigation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Navigation](
	[NavigationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NULL,
	[ElementClasses] [varchar](200) NULL,
	[ElementID] [varchar](200) NULL,
 CONSTRAINT [PK_dbo.Navigation] PRIMARY KEY CLUSTERED 
(
	[NavigationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NavigationItem]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NavigationItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[NavigationItem](
	[NavigationItemID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](250) NOT NULL,
	[RelativeURL] [nvarchar](600) NOT NULL,
	[NavigationID] [int] NOT NULL,
	[HasDropDown] [bit] NOT NULL,
	[ParentNavigationID] [int] NULL,
	[ElementClasses] [varchar](250) NULL,
	[ElementID] [varchar](200) NULL,
	[Ordering] [int] NOT NULL,
 CONSTRAINT [PK_dbo.NavigationItem] PRIMARY KEY CLUSTERED 
(
	[NavigationItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegisterCode]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegisterCode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RegisterCode](
	[RegisterCodeID] [uniqueidentifier] NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[IsExpired] [bit] NOT NULL,
	[GeneratedDate] [date] NOT NULL,
	[ValidityTill] [date] NOT NULL,
	[RoleID] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.RegisterCode] PRIMARY KEY CLUSTERED 
(
	[RegisterCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RegisterCodeUserRelation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegisterCodeUserRelation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RegisterCodeUserRelation](
	[RegisterCodeUserRelationID] [uniqueidentifier] NOT NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.RegisterCodeUserRelation] PRIMARY KEY CLUSTERED 
(
	[RegisterCodeUserRelationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TempUserRoleRelation]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempUserRoleRelation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TempUserRoleRelation](
	[TempUserRoleRelationID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[UserRoleID] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.TempUserRoleRelation] PRIMARY KEY CLUSTERED 
(
	[TempUserRoleRelationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[UserTimeZone]    Script Date: 14-Mar-16 3:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserTimeZone]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserTimeZone](
	[UserTimeZoneID] [int] IDENTITY(1,1) NOT NULL,
	[InfoID] [varchar](50) NOT NULL,
	[Display] [varchar](70) NOT NULL,
	[UTCName] [varchar](10) NOT NULL,
	[UTCValue] [real] NOT NULL,
	[TimePartOnly] [varchar](10) NOT NULL,
 CONSTRAINT [PK_dbo.UserTimeZone] PRIMARY KEY CLUSTERED 
(
	[UserTimeZoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF] SET  READ_WRITE 
GO
