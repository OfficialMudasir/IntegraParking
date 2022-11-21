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
	TARGR_PARKING_MODE int NOT NULL CONSTRAINT DF_TARIFFS_IN_GROUPS_TARGR_PARKING_MODE DEFAULT 1
GO
DECLARE @v sql_variant 
SET @v = N'1: normal mode, 2: start/stop mode'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TARIFFS_IN_GROUPS', N'COLUMN', N'TARGR_PARKING_MODE'
GO
ALTER TABLE dbo.TARIFFS_IN_GROUPS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





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
ALTER TABLE dbo.OPERATIONS ADD
	OPE_STOP_DATE datetime NULL,
	OPE_UTC_STOP_DATE datetime NULL,
	OPE_PARKING_MODE int NOT NULL CONSTRAINT DF_OPERATIONS_OPE_PARKING_MODE DEFAULT 1,
	OPE_PARKING_MODE_STATUS int NOT NULL CONSTRAINT DF_OPERATIONS_OPE_PARKING_MODE_STATUS DEFAULT 2,
	OPE_EXTERNAL_BASE_ID1 varchar(50) NULL,
	OPE_EXTERNAL_BASE_ID2 varchar(50) NULL,
	OPE_EXTERNAL_BASE_ID3 varchar(50) NULL
GO
DECLARE @v sql_variant 
SET @v = N'1: normal mode, 2: start/stop mode'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS', N'COLUMN', N'OPE_PARKING_MODE'
GO
DECLARE @v sql_variant 
SET @v = N'1: opened, 2: closed'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS', N'COLUMN', N'OPE_PARKING_MODE_STATUS'
GO
DECLARE @v sql_variant 
SET @v = N'Operation External WS1 returned base Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS', N'COLUMN', N'OPE_EXTERNAL_BASE_ID1'
GO
DECLARE @v sql_variant 
SET @v = N'Operation External WS2 returned base Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS', N'COLUMN', N'OPE_EXTERNAL_BASE_ID2'
GO
DECLARE @v sql_variant 
SET @v = N'Operation External WS3 returned base Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATIONS', N'COLUMN', N'OPE_EXTERNAL_BASE_ID3'
GO
ALTER TABLE dbo.OPERATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





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
ALTER TABLE dbo.HIS_OPERATIONS ADD
	OPE_STOP_DATE datetime NULL,
	OPE_UTC_STOP_DATE datetime NULL,
	OPE_PARKING_MODE int NOT NULL CONSTRAINT DF_HIS_OPERATIONS_OPE_PARKING_MODE DEFAULT ((1)),
	OPE_PARKING_MODE_STATUS int NOT NULL CONSTRAINT DF_HIS_OPERATIONS_OPE_PARKING_MODE_STATUS DEFAULT ((2)),
	OPE_EXTERNAL_BASE_ID1 varchar(50) NULL,
	OPE_EXTERNAL_BASE_ID2 varchar(50) NULL,
	OPE_EXTERNAL_BASE_ID3 varchar(50) NULL
GO
DECLARE @v sql_variant 
SET @v = N'1: normal mode, 2: start/stop mode'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'HIS_OPERATIONS', N'COLUMN', N'OPE_PARKING_MODE'
GO
DECLARE @v sql_variant 
SET @v = N'1: opened, 2: closed'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'HIS_OPERATIONS', N'COLUMN', N'OPE_PARKING_MODE_STATUS'
GO
DECLARE @v sql_variant 
SET @v = N'Operation External WS1 returned base Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'HIS_OPERATIONS', N'COLUMN', N'OPE_EXTERNAL_BASE_ID1'
GO
DECLARE @v sql_variant 
SET @v = N'Operation External WS2 returned base Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'HIS_OPERATIONS', N'COLUMN', N'OPE_EXTERNAL_BASE_ID2'
GO
DECLARE @v sql_variant 
SET @v = N'Operation External WS3 returned base Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'HIS_OPERATIONS', N'COLUMN', N'OPE_EXTERNAL_BASE_ID3'
GO
ALTER TABLE dbo.HIS_OPERATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT




