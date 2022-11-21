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
ALTER TABLE dbo.CUSTOMER_PAYMENT_MEANS ADD
	CUSPM_CREATION_UTC_DATE datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'Creation UTC Date'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CUSTOMER_PAYMENT_MEANS', N'COLUMN', N'CUSPM_CREATION_UTC_DATE'
GO
ALTER TABLE dbo.CUSTOMER_PAYMENT_MEANS ADD CONSTRAINT
	DF_CUSTOMER_PAYMENT_MEANS_CUSPM_CREATION_UTC_DATE DEFAULT getutcdate() FOR CUSPM_CREATION_UTC_DATE
GO
ALTER TABLE dbo.CUSTOMER_PAYMENT_MEANS SET (LOCK_ESCALATION = TABLE)
GO
update CUSTOMER_PAYMENT_MEANS set CUSPM_CREATION_UTC_DATE=CUSPM_LAST_TIME_USERD where CUSPM_VALID=1 and CUSPM_ENABLED=1 AND CUSPM_LAST_TIME_USERD IS NOT NULL
GO
update CUSTOMER_PAYMENT_MEANS set CUSPM_CREATION_UTC_DATE=GETUTCDATE() where CUSPM_VALID=1 and CUSPM_ENABLED=1 AND CUSPM_LAST_TIME_USERD IS NULL
GO
COMMIT