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
	OSI_PLATE2 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE3 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE4 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE5 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE6 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE7 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE8 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE9 varchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	OSI_PLATE10 varchar(50) COLLATE Modern_Spanish_CI_AS NULL
GO
DECLARE @v sql_variant 
SET @v = N'Plate 2'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE2'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 3'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE3'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 4'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE4'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 5'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE5'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 6'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE6'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 7'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE7'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 8'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE8'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 9'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE9'
GO
DECLARE @v sql_variant 
SET @v = N'Plate 10'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS_SESSION_INFO', N'COLUMN', N'OSI_PLATE10'
GO
ALTER TABLE dbo.OPERATIONS_SESSION_INFO SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
