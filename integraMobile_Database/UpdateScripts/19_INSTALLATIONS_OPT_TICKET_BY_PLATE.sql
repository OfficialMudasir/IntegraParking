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
ALTER TABLE dbo.INSTALLATIONS ADD
	INS_OPT_TICKET_BY_PLATE int NOT NULL CONSTRAINT DF_INSTALLATIONS_INS_OPT_TICKET_BY_PLATE DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'is pay ticket by plate enabled?'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'INSTALLATIONS', N'COLUMN', N'INS_OPT_TICKET_BY_PLATE'
GO
ALTER TABLE dbo.INSTALLATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
