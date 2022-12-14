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
CREATE TABLE dbo.PAYPAL_CONFIGURATION
	(
	PPCON_ID numeric(18, 0) NOT NULL IDENTITY (1, 1),
	PPCON_GUID varchar(100) NOT NULL,
	PPCON_RESTAPI_URL_PREFIX varchar(100) NULL,
	PPCON_RESTAPI_CLIENT_ID varchar(200) NULL,
	PPCON_RESTAPI_CLIENT_SECRET varchar(200) NULL,
	PPCON_RESTAPI_ENVIRONMENT int NULL,
	PPCON_SERVICE_TIMEOUT int NOT NULL,
	PPCON_CHECK_DATE_AND_HASH int NOT NULL,
	PPCON_CONFIRMATION_TIME int NOT NULL,
	PPCON_HASH_SEED varchar(100) NOT NULL
	)  ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'Identifier'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Identifier GUID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_GUID'
GO
DECLARE @v sql_variant 
SET @v = N'Paypal URL Prefix'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_RESTAPI_URL_PREFIX'
GO
DECLARE @v sql_variant 
SET @v = N'Paypal Client ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_RESTAPI_CLIENT_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Paypal Client Secret'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_RESTAPI_CLIENT_SECRET'
GO
DECLARE @v sql_variant 
SET @v = N'Paypal Rest API Environment: 0: sandbox; 1:live'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_RESTAPI_ENVIRONMENT'
GO
DECLARE @v sql_variant 
SET @v = N'Wait Milliseconds'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_SERVICE_TIMEOUT'
GO
DECLARE @v sql_variant 
SET @v = N'Check date and hash or not?'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_CHECK_DATE_AND_HASH'
GO
DECLARE @v sql_variant 
SET @v = N'Max Number of seconds to confirm transaction'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_CONFIRMATION_TIME'
GO
DECLARE @v sql_variant 
SET @v = N'Hash Seed'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PAYPAL_CONFIGURATION', N'COLUMN', N'PPCON_HASH_SEED'
GO
ALTER TABLE dbo.PAYPAL_CONFIGURATION ADD CONSTRAINT
	DF_PAYPAL_CONFIGURATION_MONCON_SERVICE_TIMEOUT DEFAULT ((5000)) FOR PPCON_SERVICE_TIMEOUT
GO
ALTER TABLE dbo.PAYPAL_CONFIGURATION ADD CONSTRAINT
	DF_PAYPAL_CONFIGURATION_MONCON_CHECK_DATE_AND_HASH DEFAULT ((1)) FOR PPCON_CHECK_DATE_AND_HASH
GO
ALTER TABLE dbo.PAYPAL_CONFIGURATION ADD CONSTRAINT
	DF_PAYPAL_CONFIGURATION_MONCON_CONFIRMATION_TIME DEFAULT ((300)) FOR PPCON_CONFIRMATION_TIME
GO
ALTER TABLE dbo.PAYPAL_CONFIGURATION ADD CONSTRAINT
	DF_PAYPAL_CONFIGURATION_MONCON_HASH_SEED DEFAULT ((1)) FOR PPCON_HASH_SEED
GO
ALTER TABLE dbo.PAYPAL_CONFIGURATION ADD CONSTRAINT
	PK_PAYPAL_CONFIGURATION PRIMARY KEY CLUSTERED 
	(
	PPCON_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.PAYPAL_CONFIGURATION SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG ADD
	CPTGC_PPCON_ID numeric(18, 0) NULL
GO
DECLARE @v sql_variant 
SET @v = N'Paypal Configuration Identifier'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG', N'COLUMN', N'CPTGC_PPCON_ID'
GO
ALTER TABLE dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG ADD CONSTRAINT
	FK_CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG_PAYPAL_CONFIGURATION FOREIGN KEY
	(
	CPTGC_PPCON_ID
	) REFERENCES dbo.PAYPAL_CONFIGURATION
	(
	PPCON_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
