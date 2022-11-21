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
ALTER TABLE dbo.SOURCE_APPS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.USERS_PUSH_ID ADD
	UPID_LAST_SOAPP_ID numeric(18, 0) NULL
GO
DECLARE @v sql_variant 
SET @v = N'Source App Identifier'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'USERS_PUSH_ID', N'COLUMN', N'UPID_LAST_SOAPP_ID'
GO
ALTER TABLE dbo.USERS_PUSH_ID ADD CONSTRAINT
	DF_USERS_PUSH_ID_MOSE_SOAPP_ID DEFAULT ((1)) FOR UPID_LAST_SOAPP_ID
GO
ALTER TABLE dbo.USERS_PUSH_ID ADD CONSTRAINT
	FK_USERS_PUSH_ID_SOURCE_APPS FOREIGN KEY
	(
	UPID_LAST_SOAPP_ID
	) REFERENCES dbo.SOURCE_APPS
	(
	SOAPP_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.USERS_PUSH_ID SET (LOCK_ESCALATION = TABLE)
GO

UPDATE USERS_PUSH_ID SET UPID_LAST_SOAPP_ID=1
GO

COMMIT