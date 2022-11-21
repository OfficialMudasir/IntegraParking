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
ALTER TABLE dbo.CAMPAING ADD
	CAMP_MIN_PARKING_MINUTES int NULL
GO
DECLARE @v sql_variant 
SET @v = N'Min number of minutes to have right to the campaing (Initially campaing schema 8)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CAMPAING', N'COLUMN', N'CAMP_MIN_PARKING_MINUTES'
GO
ALTER TABLE dbo.CAMPAING SET (LOCK_ESCALATION = TABLE)
GO
COMMIT