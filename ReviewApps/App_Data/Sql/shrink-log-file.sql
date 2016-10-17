USE [WeReviewApp];
--DBCC SHRINKFILE(N'D:\Working (SSD)\GitHub\WeReviewProject\WereViewApp\App_Data\WeReviewApp-Accounts_log.ldf', 1);
SELECT file_id, name
FROM sys.database_files
DBCC SHRINKFILE (2, TRUNCATEONLY);