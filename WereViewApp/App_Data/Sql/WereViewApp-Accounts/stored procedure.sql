USE [WereViewApp-Accounts]
GO
/****** Object:  StoredProcedure [dbo].[ResetRoles]    Script Date: 23-Feb-16 2:28:11 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetRoles]
GO
/****** Object:  StoredProcedure [dbo].[ResetImageResize]    Script Date: 23-Feb-16 2:28:11 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetImageResize]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetImageResize]
GO
/****** Object:  StoredProcedure [dbo].[ResetFeedbacks]    Script Date: 23-Feb-16 2:28:11 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetFeedbacks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetFeedbacks]
GO
/****** Object:  StoredProcedure [dbo].[ResetCoreSettings]    Script Date: 23-Feb-16 2:28:11 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetCoreSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetCoreSettings]
GO
/****** Object:  StoredProcedure [dbo].[ResetASPNetUser]    Script Date: 23-Feb-16 2:28:11 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetASPNetUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetASPNetUser]
GO
/****** Object:  StoredProcedure [dbo].[CleanWholeSystem]    Script Date: 23-Feb-16 2:28:11 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CleanWholeSystem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CleanWholeSystem]
GO
/****** Object:  StoredProcedure [dbo].[CleanWholeSystem]    Script Date: 23-Feb-16 2:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CleanWholeSystem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[CleanWholeSystem]

AS begin
    truncate table FeedbackAppReviewRelation;
    truncate table Feedback;
    truncate table TempUserRoleRelation;

    Exec ResetASPNetUser;
    exec ResetFeedbacks;

    Update CoreSetting
    set CoreSetting.IsFirstUserFound = 0
    Where CoreSetting.CoreSettingID = 1;
    
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetASPNetUser]    Script Date: 23-Feb-16 2:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetASPNetUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetASPNetUser]

AS
	Delete from RegisterCodeUserRelation;
	Delete from AspNetUserRoles;
	Delete from AspNetUsers;
	
	Update CoreSetting 
	SET 
		IsFirstUserFound =0 
	WHERE 
		CoreSettingID = 1;
		
	DBCC checkident(''AspNetUserRoles'', reseed, 0);
	DBCC checkident(''RegisterCodeUserRelation'', reseed, 0);
	DBCC checkident(''AspNetUsers'', reseed, 0);
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetCoreSettings]    Script Date: 23-Feb-16 2:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetCoreSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetCoreSettings]
	
AS
	Delete from CoreSetting;
	DBCC checkident (''CoreSetting'', reseed, 0);
RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetFeedbacks]    Script Date: 23-Feb-16 2:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetFeedbacks]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetFeedbacks]

AS
	Delete from FeedbackAppReviewRelation;
	Delete from Feedback;
	DBCC checkident (''FeedbackAppReviewRelation'', reseed, 0);
	DBCC checkident (''Feedback'', reseed, 0);

RETURN 0' 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetImageResize]    Script Date: 23-Feb-16 2:28:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ResetRoles]    Script Date: 23-Feb-16 2:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetRoles]
AS
	Delete from AspNetUserRoles;
	Delete from AspNetRoles;
	
	DBCC checkident(''AspNetUserRoles'', reseed, 0);
	DBCC checkident(''AspNetRoles'', reseed, 0);
RETURN 0' 
END
GO
