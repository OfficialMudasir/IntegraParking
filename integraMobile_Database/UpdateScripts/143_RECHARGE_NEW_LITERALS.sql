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
ALTER TABLE dbo.OPERATORS ADD
	OPR_AMOUNT_TO_BE_RECHARGED_LIT_ID numeric(18, 0) NULL,
	OPR_RECHARGE_BONIFICATION_LIT_ID numeric(18, 0) NULL,
	OPR_RECHARGE_BASE_CHARGE_AMOUNT_LIT_ID numeric(18, 0) NULL
GO
DECLARE @v sql_variant 
SET @v = N'Amount to be recharged literal id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATORS', N'COLUMN', N'OPR_AMOUNT_TO_BE_RECHARGED_LIT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Amount to be bonified (substracted) from amount to be recharged literal id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATORS', N'COLUMN', N'OPR_RECHARGE_BONIFICATION_LIT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Amount base amount to be charged for the recharge'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'OPERATORS', N'COLUMN', N'OPR_RECHARGE_BASE_CHARGE_AMOUNT_LIT_ID'
GO
ALTER TABLE dbo.OPERATORS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
