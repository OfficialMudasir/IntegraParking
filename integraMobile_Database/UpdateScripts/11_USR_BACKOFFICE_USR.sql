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
ALTER TABLE dbo.USERS ADD
	USR_BACKOFFICE_USR varchar(256) NULL
GO
DECLARE @v sql_variant 
SET @v = N'Backoffice user in case of user insertion from BackOffice'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'USERS', N'COLUMN', N'USR_BACKOFFICE_USR'
GO
ALTER TABLE dbo.USERS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
