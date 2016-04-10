USE [F:\WORK\GIT\WEREVIEWPROJECT\WEREVIEWAPP\APP_DATA\WEREVIEWAPP-ACCOUNTS.MDF]
GO
/****** Object:  StoredProcedure [dbo].[CleanWholeSystem]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CleanWholeSystem]
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
    
      END
GO
/****** Object:  StoredProcedure [dbo].[GetAllTablesSpaceUsedInformation]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Md. Alim Ul Karim
-- Create date: 14 Mar 2016
-- Description:	Get all tables spaced used information.
-- =============================================
CREATE PROCEDURE [dbo].[GetAllTablesSpaceUsedInformation] 
AS
BEGIN
    EXECUTE sp_MSforeachtable 'EXECUTE SP_SPACEUSED [?];';
END

GO
/****** Object:  StoredProcedure [dbo].[ResetASPNetUser]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetASPNetUser]
AS 
      DELETE    FROM RegisterCodeUserRelation;
      DELETE    FROM AspNetUserRoles;
      DELETE    FROM AspNetUsers;
	
      UPDATE    CoreSetting
      SET       IsFirstUserFound = 0
      WHERE     CoreSettingID = 1;
		
      DBCC checkident('AspNetUserRoles', reseed, 0);
      DBCC checkident('RegisterCodeUserRelation', reseed, 0);
      DBCC checkident('AspNetUsers', reseed, 0);
      RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[ResetCoreSettings]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetCoreSettings]
AS 
      DELETE    FROM CoreSetting;
      DBCC checkident ('CoreSetting', reseed, 0);
      RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[ResetFeedbacks]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetFeedbacks]
AS 
      DELETE    FROM FeedbackAppReviewRelation;
      DELETE    FROM Feedback;
      DBCC checkident ('FeedbackAppReviewRelation', reseed, 0);
      DBCC checkident ('Feedback', reseed, 0);

      RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[ResetImageResize]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetImageResize]

AS
	Delete from ImageResizeSetting;
	DBCC checkident ('ImageResizeSetting', reseed, 0);
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[ResetRoles]    Script Date: 14-Mar-16 3:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetRoles]
AS 
      DELETE    FROM AspNetUserRoles;
      DELETE    FROM AspNetRoles;
	
      DBCC checkident('AspNetUserRoles', reseed, 0);
      DBCC checkident('AspNetRoles', reseed, 0);
      RETURN 0
GO
