/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.OPERATIONS_SESSION_INFO ADD
	OSI_CAMPAIGN_AMOUNT int NULL
GO
DECLARE @v sql_variant 
SET @v = N'Campaing amount calculated'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_CAMPAIGN_AMOUNT'
GO
ALTER TABLE dbo.OPERATIONS_SESSION_INFO SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CAMPAING ADD
	CAMP_FREE_MINUTES int NULL
GO
DECLARE @v sql_variant 
SET @v = N'Free minutes given amount (Initially campaing schema 8)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CAMPAING', N'COLUMN', N'CAMP_FREE_MINUTES'
GO
ALTER TABLE dbo.CAMPAING SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
