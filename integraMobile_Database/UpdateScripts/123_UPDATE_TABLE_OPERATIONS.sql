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
	OPE_CAMP_ID numeric(18, 0) NULL,
	OPE_CAMP_OPE_NUM_BON_PARK numeric(18, 0) NULL
GO
ALTER TABLE dbo.OPERATIONS ADD CONSTRAINT
	FK_OPERATIONS_CAMPAING FOREIGN KEY
	(
	OPE_CAMP_ID
	) REFERENCES dbo.CAMPAING
	(
	CAMP_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.OPERATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

