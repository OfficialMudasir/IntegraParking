/* Para evitar posibles problemas de pérdida de datos, debe revisar este script detalladamente antes de ejecutarlo fuera del contexto del diseñador de base de datos.*/
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
ALTER TABLE dbo.SERVICES_TYPE SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.TARIFFS ADD
	TAR_SERTYP_ID numeric(18, 0) NULL,
	TAR_AUTOSTART int NULL,
	TAR_RESTART_TARIFF int NULL
GO
DECLARE @v sql_variant 
SET @v = N'Identficador de la tabla SERVICES_TYPE'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TARIFFS', N'COLUMN', N'TAR_SERTYP_ID'
GO
DECLARE @v sql_variant 
SET @v = N'Identificar si el inicio de la tarifa es automática'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TARIFFS', N'COLUMN', N'TAR_AUTOSTART'
GO
DECLARE @v sql_variant 
SET @v = N'Identificar si se debe mostrar nuevamente la pantalla de tarifas'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TARIFFS', N'COLUMN', N'TAR_RESTART_TARIFF'
GO
ALTER TABLE dbo.TARIFFS ADD CONSTRAINT
	FK_TARIFFS_SERVICES_TYPE FOREIGN KEY
	(
	TAR_SERTYP_ID
	) REFERENCES dbo.SERVICES_TYPE
	(
	SERTYP_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TARIFFS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT