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
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD
	TARGR_INCLUDE_ZERO_STEP int NOT NULL CONSTRAINT DF_TARIFFS_IN_GROUPS_TARGR_INCLUDE_ZERO_STEP DEFAULT 0,
	TARGR_TIME_DEF_STEP int NULL
GO
DECLARE @v sql_variant 
SET @v = N'Include non seleccionable zero time step'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_INCLUDE_ZERO_STEP'
GO
DECLARE @v sql_variant 
SET @v = N'Time in minutes of step selected by default'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_TIME_DEF_STEP'
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
