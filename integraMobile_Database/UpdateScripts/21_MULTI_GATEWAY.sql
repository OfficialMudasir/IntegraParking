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
CREATE TABLE dbo.COMMON_TOKEN_GROUPS
	(
	CTG_ID numeric(18, 0) NOT NULL,
	CTG_DESCRIPTION varchar(100) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.COMMON_TOKEN_GROUPS ADD CONSTRAINT
	PK_COMMON_TOKEN_GROUPS PRIMARY KEY CLUSTERED 
	(
	CTG_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.COMMON_TOKEN_GROUPS SET (LOCK_ESCALATION = TABLE)
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
ALTER TABLE dbo.COMMON_TOKEN_GROUPS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG ADD
	CPTGC_CTG_ID numeric(18, 0) NULL
GO
ALTER TABLE dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG ADD CONSTRAINT
	FK_CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG_COMMON_TOKEN_GROUPS FOREIGN KEY
	(
	CPTGC_CTG_ID
	) REFERENCES dbo.COMMON_TOKEN_GROUPS
	(
	CTG_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.INSTALLATIONS ADD
	INS_CPTGC_ID numeric(18, 0) NULL
GO
ALTER TABLE dbo.INSTALLATIONS ADD CONSTRAINT
	FK_INSTALLATIONS_CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG FOREIGN KEY
	(
	INS_CPTGC_ID
	) REFERENCES dbo.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG
	(
	CPTGC_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.INSTALLATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
