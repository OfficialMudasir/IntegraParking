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
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_TARIFFS
GO
ALTER TABLE dbo.TARIFFS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_GROUPS
GO
ALTER TABLE dbo.GROUPS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_LITERALS
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_LITERALS1
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_LITERALS2
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_LITERALS3
GO
ALTER TABLE dbo.LITERALS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT FK_TARIFFS_IN_GROUPS_GROUPS_TYPES
GO
ALTER TABLE dbo.GROUPS_TYPES SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS
	DROP CONSTRAINT DF_TARIFFS_IN_GROUPS_TARGR_TIME_OFFSET
GO
CREATE TABLE dbo.Tmp_TARIFFS_IN_GROUPS
	(
	TARGR_ID numeric(18, 0) NOT NULL IDENTITY (1, 1),
	TARGR_GRP_ID numeric(18, 0) NULL,
	TARGR_GRPT_ID numeric(18, 0) NULL,
	TARGR_TAR_ID numeric(18, 0) NOT NULL,
	TARGR_TIME_STEPS_VALUE int NOT NULL,
	TARGR_LIT_ID numeric(18, 0) NOT NULL,
	TARGR_USER_SELECTABLE int NOT NULL,
	TARGR_INI_APPLY_DATE datetime NOT NULL,
	TARGR_END_APPLY_DATE datetime NOT NULL,
	TARGR_STEP1_MIN int NULL,
	TARGR_STEP1_LIT_ID numeric(18, 0) NULL,
	TARGR_STEP1_ENABLED int NULL,
	TARGR_STEP2_MIN int NULL,
	TARGR_STEP2_LIT_ID numeric(18, 0) NULL,
	TARGR_STEP2_ENABLED int NULL,
	TARGR_STEP3_MIN int NULL,
	TARGR_STEP3_LIT_ID numeric(18, 0) NULL,
	TARGR_STEP3_ENABLED int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TARIFFS_IN_GROUPS SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'Table to store assignations of tariffs to groups or group types'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', NULL, NULL
GO
DECLARE @v sql_variant 
SET @v = N'Identificator'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Group Identificator'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_GRP_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Group Type Identificator'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_GRPT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Tariff Identificator'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_TAR_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Value to pass to tariff calculator as step offset'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_TIME_STEPS_VALUE'
GO
DECLARE @v sql_variant 
SET @v = N'Literal Identificator'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_LIT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'User is going to have the possiblity of select in front other chances'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_USER_SELECTABLE'
GO
DECLARE @v sql_variant 
SET @v = N'Ini Date of application Range'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_INI_APPLY_DATE'
GO
DECLARE @v sql_variant 
SET @v = N'End Date of application Range'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_END_APPLY_DATE'
GO
DECLARE @v sql_variant 
SET @v = N'Step1 minutes'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP1_MIN'
GO
DECLARE @v sql_variant 
SET @v = N'Step1 literal id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP1_LIT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Step1 minutes'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP1_ENABLED'
GO
DECLARE @v sql_variant 
SET @v = N'Step 2 minutes'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP2_MIN'
GO
DECLARE @v sql_variant 
SET @v = N'Step 2 literal id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP2_LIT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Step1 minutes'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP2_ENABLED'
GO
DECLARE @v sql_variant 
SET @v = N'Step 3 minutes'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP3_MIN'
GO
DECLARE @v sql_variant 
SET @v = N'Step 3 literal id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP3_LIT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Step1 minutes'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_STEP3_ENABLED'
GO
ALTER TABLE dbo.Tmp_TARIFFS_IN_GROUPS ADD CONSTRAINT
	DF_TARIFFS_IN_GROUPS_TARGR_TIME_OFFSET DEFAULT ((5)) FOR TARGR_TIME_STEPS_VALUE
GO
ALTER TABLE dbo.Tmp_TARIFFS_IN_GROUPS ADD CONSTRAINT
	DF_TARIFFS_IN_GROUPS_TARGR_STEP1_ENABLED DEFAULT 1 FOR TARGR_STEP1_ENABLED
GO
ALTER TABLE dbo.Tmp_TARIFFS_IN_GROUPS ADD CONSTRAINT
	DF_TARIFFS_IN_GROUPS_TARGR_STEP1_ENABLED1 DEFAULT 1 FOR TARGR_STEP2_ENABLED
GO
ALTER TABLE dbo.Tmp_TARIFFS_IN_GROUPS ADD CONSTRAINT
	DF_TARIFFS_IN_GROUPS_TARGR_STEP1_ENABLED1_1 DEFAULT 1 FOR TARGR_STEP3_ENABLED
GO
SET IDENTITY_INSERT dbo.Tmp_TARIFFS_IN_GROUPS ON
GO
IF EXISTS(SELECT * FROM dbo.TARIFFS_IN_GROUPS)
	 EXEC('INSERT INTO dbo.Tmp_TARIFFS_IN_GROUPS (TARGR_ID, TARGR_GRP_ID, TARGR_GRPT_ID, TARGR_TAR_ID, TARGR_TIME_STEPS_VALUE, TARGR_LIT_ID, TARGR_USER_SELECTABLE, TARGR_INI_APPLY_DATE, TARGR_END_APPLY_DATE, TARGR_STEP1_MIN, TARGR_STEP1_LIT_ID, TARGR_STEP2_MIN, TARGR_STEP2_LIT_ID, TARGR_STEP3_MIN, TARGR_STEP3_LIT_ID)
		SELECT TARGR_ID, TARGR_GRP_ID, TARGR_GRPT_ID, TARGR_TAR_ID, TARGR_TIME_STEPS_VALUE, TARGR_LIT_ID, TARGR_USER_SELECTABLE, TARGR_INI_APPLY_DATE, TARGR_END_APPLY_DATE, TARGR_STEP1_MIN, TARGR_STEP1_LIT_ID, TARGR_STEP2_MIN, TARGR_STEP2_LIT_ID, TARGR_STEP3_MIN, TARGR_STEP3_LIT_ID FROM dbo.TARIFFS_IN_GROUPS WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_TARIFFS_IN_GROUPS OFF
GO
DROP TABLE dbo.TARIFFS_IN_GROUPS
GO
EXECUTE sp_rename N'dbo.Tmp_TARIFFS_IN_GROUPS', N'TARIFFS_IN_GROUPS', 'OBJECT' 
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	PK_TARIFFS_IN_GROUPS PRIMARY KEY CLUSTERED 
	(
	TARGR_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_GROUPS_TYPES FOREIGN KEY
	(
	TARGR_GRPT_ID
	) REFERENCES dbo.GROUPS_TYPES
	(
	GRPT_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_LITERALS FOREIGN KEY
	(
	TARGR_LIT_ID
	) REFERENCES dbo.LITERALS
	(
	LIT_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_GROUPS FOREIGN KEY
	(
	TARGR_GRP_ID
	) REFERENCES dbo.GROUPS
	(
	GRP_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_TARIFFS FOREIGN KEY
	(
	TARGR_TAR_ID
	) REFERENCES dbo.TARIFFS
	(
	TAR_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_LITERALS1 FOREIGN KEY
	(
	TARGR_STEP1_LIT_ID
	) REFERENCES dbo.LITERALS
	(
	LIT_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_LITERALS2 FOREIGN KEY
	(
	TARGR_STEP2_LIT_ID
	) REFERENCES dbo.LITERALS
	(
	LIT_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS ADD CONSTRAINT
	FK_TARIFFS_IN_GROUPS_LITERALS3 FOREIGN KEY
	(
	TARGR_STEP3_LIT_ID
	) REFERENCES dbo.LITERALS
	(
	LIT_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
