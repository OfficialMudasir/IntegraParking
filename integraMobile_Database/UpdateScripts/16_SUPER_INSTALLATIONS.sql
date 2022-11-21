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
ALTER TABLE dbo.INSTALLATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.SUPER_INSTALLATIONS
	(
	SINS_ID numeric(18, 0) NOT NULL IDENTITY (1, 1),
	SINS_SUPER_INS_ID numeric(18, 0) NOT NULL,
	SINS_CHILD_INS_ID numeric(18, 0) NOT NULL,
	SINS_INI_APPLY_DATE datetime NOT NULL,
	SINS_END_APPLY_DATE datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.SUPER_INSTALLATIONS ADD CONSTRAINT
	PK_SUPER_INSTALLATIONS PRIMARY KEY CLUSTERED 
	(
	SINS_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.SUPER_INSTALLATIONS ADD CONSTRAINT
	FK_SUPER_INSTALLATIONS_INSTALLATIONS_SUPER FOREIGN KEY
	(
	SINS_SUPER_INS_ID
	) REFERENCES dbo.INSTALLATIONS
	(
	INS_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SUPER_INSTALLATIONS ADD CONSTRAINT
	FK_SUPER_INSTALLATIONS_INSTALLATIONS_CHILD FOREIGN KEY
	(
	SINS_CHILD_INS_ID
	) REFERENCES dbo.INSTALLATIONS
	(
	INS_ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SUPER_INSTALLATIONS SET (LOCK_ESCALATION = TABLE)
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
ALTER TABLE dbo.TICKET_PAYMENTS_SESSION_INFO ADD
	TPSI_INS_ID numeric(18, 0) NULL
GO
ALTER TABLE dbo.TICKET_PAYMENTS_SESSION_INFO SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/****** Object:  Trigger [dbo].[TR_SYNC_INSTALLATIONS]    Script Date: 18/09/2018 10:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_SYNC_SUPER_INSTALLATIONS]
   ON [dbo].[SUPER_INSTALLATIONS]
   AFTER INSERT,UPDATE,DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CountInserted int
	DECLARE @CountDeleted int

	SELECT @CountInserted = COUNT(*) FROM Inserted
	SELECT @CountDeleted = COUNT(*) FROM Deleted

	IF @CountInserted > 0 AND @CountDeleted > 0
	BEGIN

		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,ins.[INS_ID]
           ,ins.[INS_DESCRIPTION]
           ,ins.[INS_SHORTDESC]
           ,ins.[INS_CUR_ID]
           ,ins.[INS_COU_ID]
           ,ins.[INS_ENABLED]
		  FROM Inserted i
				INNER JOIN Deleted d ON d.[SINS_ID] = i.[SINS_ID]
				INNER JOIN (SELECT [INS_ID], [INS_DESCRIPTION], [INS_SHORTDESC], [INS_CUR_ID], [INS_COU_ID], 
								   CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = INSTALLATIONS.INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END AS INS_ENABLED
							FROM INSTALLATIONS) AS ins ON i.SINS_SUPER_INS_ID = ins.INS_ID
		  where i.SINS_SUPER_INS_ID <> d.SINS_SUPER_INS_ID AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR,INSTALLATIONS_SYNC.INS_ENABLED)
						FROM INSTALLATIONS_SYNC
						WHERE INSTALLATIONS_SYNC.INS_ID = ins.INS_ID
						ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN ('I' + CONVERT(VARCHAR, ins.INS_ENABLED), 'U' + CONVERT(VARCHAR, ins.INS_ENABLED)) ;

		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           , CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_SUPER_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END
		  FROM Deleted d
			    INNER JOIN Inserted i ON d.[SINS_ID] = i.[SINS_ID]
				INNER JOIN INSTALLATIONS ON d.SINS_SUPER_INS_ID = INS_ID
		  WHERE i.SINS_SUPER_INS_ID <> d.SINS_SUPER_INS_ID AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR, INSTALLATIONS_SYNC.INS_ENABLED)
						FROM INSTALLATIONS_SYNC
						WHERE INSTALLATIONS_SYNC.INS_ID = INSTALLATIONS.INS_ID
						ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN 
						('I' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_SUPER_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END),
						 'U' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_SUPER_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END));

		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,ins.[INS_ID]
           ,ins.[INS_DESCRIPTION]
           ,ins.[INS_SHORTDESC]
           ,ins.[INS_CUR_ID]
           ,ins.[INS_COU_ID]
           ,ins.[INS_ENABLED]
		  FROM Inserted i
				INNER JOIN Deleted d ON d.[SINS_ID] = i.[SINS_ID]
				INNER JOIN (SELECT [INS_ID], [INS_DESCRIPTION], [INS_SHORTDESC], [INS_CUR_ID], [INS_COU_ID], 
								   CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_SUPER_INS_ID = INSTALLATIONS.INS_ID), 0) WHEN 0 THEN 0 ELSE INSTALLATIONS.[INS_ENABLED] END AS INS_ENABLED
							FROM INSTALLATIONS) AS ins ON i.SINS_CHILD_INS_ID = ins.INS_ID
		  where i.SINS_CHILD_INS_ID <> d.SINS_CHILD_INS_ID AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR,INSTALLATIONS_SYNC.INS_ENABLED)
						FROM INSTALLATIONS_SYNC
						WHERE INSTALLATIONS_SYNC.INS_ID = ins.INS_ID
						ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN ('I' + CONVERT(VARCHAR, ins.INS_ENABLED), 'U' + CONVERT(VARCHAR, ins.INS_ENABLED)) ;

		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           , CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_CHILD_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END
		  FROM Deleted d
				INNER JOIN Inserted i ON d.[SINS_ID] = i.[SINS_ID]
				INNER JOIN INSTALLATIONS ON d.SINS_CHILD_INS_ID = INS_ID
		  WHERE i.SINS_CHILD_INS_ID <> d.SINS_CHILD_INS_ID AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR, INSTALLATIONS_SYNC.INS_ENABLED)
				 FROM INSTALLATIONS_SYNC
				 WHERE INSTALLATIONS_SYNC.INS_ID = INSTALLATIONS.INS_ID
				 ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN
				 ('I' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_CHILD_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END),
				  'U' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_CHILD_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END));

		   IF @@ERROR != 0
				 ROLLBACK TRAN

		
	END

	IF @CountInserted > 0
	BEGIN

		INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,ins.[INS_ID]
           ,ins.[INS_DESCRIPTION]
           ,ins.[INS_SHORTDESC]
           ,ins.[INS_CUR_ID]
           ,ins.[INS_COU_ID]
           ,ins.[INS_ENABLED]
		  FROM Inserted i
				INNER JOIN (SELECT [INS_ID], [INS_DESCRIPTION], [INS_SHORTDESC], [INS_CUR_ID], [INS_COU_ID], 
								   CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = INSTALLATIONS.INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END AS INS_ENABLED
							FROM INSTALLATIONS) AS ins ON i.SINS_SUPER_INS_ID = ins.INS_ID
		  where i.[SINS_ID] not in (select d.[SINS_ID] from Deleted d) AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR,INSTALLATIONS_SYNC.INS_ENABLED)
						FROM INSTALLATIONS_SYNC
						WHERE INSTALLATIONS_SYNC.INS_ID = ins.INS_ID
						ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN ('I' + CONVERT(VARCHAR, ins.INS_ENABLED), 'U' + CONVERT(VARCHAR, ins.INS_ENABLED)) ;


		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,ins.[INS_ID]
           ,ins.[INS_DESCRIPTION]
           ,ins.[INS_SHORTDESC]
           ,ins.[INS_CUR_ID]
           ,ins.[INS_COU_ID]
           ,ins.[INS_ENABLED]
		  FROM Inserted i
				INNER JOIN (SELECT [INS_ID], [INS_DESCRIPTION], [INS_SHORTDESC], [INS_CUR_ID], [INS_COU_ID], 
								   CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_SUPER_INS_ID = INSTALLATIONS.INS_ID), 0) WHEN 0 THEN 0 ELSE INSTALLATIONS.[INS_ENABLED] END AS INS_ENABLED
							FROM INSTALLATIONS) AS ins ON i.SINS_CHILD_INS_ID = ins.INS_ID
		  where i.[SINS_ID] not in (select d.[SINS_ID] from Deleted d) AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR,INSTALLATIONS_SYNC.INS_ENABLED)
						FROM INSTALLATIONS_SYNC
						WHERE INSTALLATIONS_SYNC.INS_ID = ins.INS_ID
						ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN ('I' + CONVERT(VARCHAR, ins.INS_ENABLED), 'U' + CONVERT(VARCHAR, ins.INS_ENABLED)) ;

		   IF @@ERROR != 0
				 ROLLBACK TRAN		
	END



	IF @CountDeleted > 0
	BEGIN

		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           , CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_SUPER_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END
		  FROM Deleted d
				INNER JOIN INSTALLATIONS ON d.SINS_SUPER_INS_ID = INS_ID
		  WHERE d.[SINS_ID] not in (select i.[SINS_ID] from Inserted i) AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR, INSTALLATIONS_SYNC.INS_ENABLED)
						FROM INSTALLATIONS_SYNC
						WHERE INSTALLATIONS_SYNC.INS_ID = INSTALLATIONS.INS_ID
						ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN 
						('I' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_SUPER_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END),
						 'U' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_SUPER_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END));

		  INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           , CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_CHILD_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END
		  FROM Deleted d
				INNER JOIN INSTALLATIONS ON d.SINS_CHILD_INS_ID = INS_ID
		  WHERE d.[SINS_ID] not in (select i.[SINS_ID] from Inserted i) AND
				ISNULL((SELECT TOP 1 INSTALLATIONS_SYNC.INS_MOV_TYPE + CONVERT(VARCHAR, INSTALLATIONS_SYNC.INS_ENABLED)
				 FROM INSTALLATIONS_SYNC
				 WHERE INSTALLATIONS_SYNC.INS_ID = INSTALLATIONS.INS_ID
				 ORDER BY INSTALLATIONS_SYNC.INS_VERSION DESC), '') NOT IN
				 ('I' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_CHILD_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END),
				  'U' + CONVERT(VARCHAR, CASE ISNULL((SELECT COUNT(*) FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE() AND SUPER_INSTALLATIONS.SINS_ID <> d.SINS_ID AND SUPER_INSTALLATIONS.SINS_CHILD_INS_ID = d.SINS_CHILD_INS_ID), 0) WHEN 0 THEN INSTALLATIONS.[INS_ENABLED] ELSE 0 END));

		   IF @@ERROR != 0
				 ROLLBACK TRAN		
	END

			
END
GO

ALTER TRIGGER [dbo].[TR_SYNC_INSTALLATIONS]
   ON [dbo].[INSTALLATIONS]
   AFTER INSERT,UPDATE,DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CountInserted int
	DECLARE @CountDeleted int

	SELECT @CountInserted = COUNT(*) FROM Inserted
	SELECT @CountDeleted = COUNT(*) FROM Deleted

	IF @CountInserted > 0 AND @CountDeleted > 0
	BEGIN


		INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'U'
           ,i.[INS_ID]
           ,i.[INS_DESCRIPTION]
           ,i.[INS_SHORTDESC]
           ,i.[INS_CUR_ID]
           ,i.[INS_COU_ID]
           ,CASE WHEN SI.SINS_CHILD_INS_ID IS NULL THEN i.INS_ENABLED ELSE 0 END
		  FROM Deleted d 
				INNER JOIN Inserted i ON d.[INS_ID] = i.[INS_ID]
				LEFT JOIN (SELECT * FROM SUPER_INSTALLATIONS WHERE SINS_INI_APPLY_DATE <= GETUTCDATE() AND SINS_END_APPLY_DATE > GETUTCDATE()) AS SI ON i.INS_ID = SI.SINS_CHILD_INS_ID
		  WHERE d.[INS_DESCRIPTION]<>i.[INS_DESCRIPTION] or
				d.[INS_SHORTDESC]<>i.[INS_SHORTDESC] or
				d.[INS_CUR_ID]<>i.[INS_CUR_ID] or
				d.[INS_COU_ID]<>i.[INS_COU_ID] or
				d.[INS_DESCRIPTION]<>i.[INS_DESCRIPTION] or
				d.[INS_ENABLED]<>i.[INS_ENABLED] or
				i.INS_ENABLED <> (CASE WHEN SI.SINS_CHILD_INS_ID IS NULL THEN i.INS_ENABLED ELSE 0 END);



		   IF @@ERROR != 0
				 ROLLBACK TRAN

		
	END

	IF @CountInserted > 0
	BEGIN

		INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'I'
           ,i.[INS_ID]
           ,i.[INS_DESCRIPTION]
           ,i.[INS_SHORTDESC]
           ,i.[INS_CUR_ID]
           ,i.[INS_COU_ID]
           ,i.[INS_ENABLED]
		  FROM Inserted i
		  where i.[INS_ID] not in (select d.[INS_ID] from Deleted d);

		   IF @@ERROR != 0
				 ROLLBACK TRAN		
	END



	IF @CountDeleted > 0
	BEGIN

		INSERT INTO INSTALLATIONS_SYNC ([INS_MOV_TYPE]
           ,[INS_ID]
           ,[INS_DESCRIPTION]
           ,[INS_SHORTDESC]
           ,[INS_CUR_ID]
           ,[INS_COU_ID]
           ,[INS_ENABLED])
		  SELECT 'D'
           ,d.[INS_ID]
           ,d.[INS_DESCRIPTION]
           ,d.[INS_SHORTDESC]
           ,d.[INS_CUR_ID]
           ,d.[INS_COU_ID]
           ,d.[INS_ENABLED]
		  FROM Deleted d
		  where d.[INS_ID] not in (select i.[INS_ID] from Inserted i);

		   IF @@ERROR != 0
				 ROLLBACK TRAN		
	END

			
END
GO