/****** Object:  Table [dbo].[INSTALLATIONS_SYNC]    Script Date: 29/06/2020 9:11:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[USERS_SYNC](
	[USR_VERSION] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USR_MOV_UTC_DATE] [datetime] NOT NULL,
	[USR_MOV_TYPE] [varchar](1) NOT NULL,
	[USR_ID] [numeric](18, 0) NOT NULL,
	[USR_EMAIL] [varchar](50) NOT NULL,
	[USR_COU_ID] [numeric](18, 0) NOT NULL
 CONSTRAINT [PK_USERS_SYNC] PRIMARY KEY CLUSTERED 
(
	[USR_VERSION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[USERS_SYNC] ADD  CONSTRAINT [DF_USERS_SYNC_USR_MOV_UTC_DATE]  DEFAULT (getutcdate()) FOR [USR_MOV_UTC_DATE]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Movement version' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SYNC', @level2type=N'COLUMN',@level2name=N'USR_VERSION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Movement utc date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SYNC', @level2type=N'COLUMN',@level2name=N'USR_MOV_UTC_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Movement type (I-insert, U-update,D-delete)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SYNC', @level2type=N'COLUMN',@level2name=N'USR_MOV_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SYNC', @level2type=N'COLUMN',@level2name=N'USR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SYNC', @level2type=N'COLUMN',@level2name=N'USR_EMAIL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Country id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SYNC', @level2type=N'COLUMN',@level2name=N'USR_COU_ID'
GO


/****** Object:  Trigger [dbo].[TR_SYNC_INSTALLATIONS]    Script Date: 29/06/2020 9:17:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_SYNC_USERS]
   ON [dbo].[USERS]
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


		INSERT INTO USERS_SYNC ([USR_MOV_TYPE], [USR_ID], [USR_EMAIL], [USR_COU_ID])
		  SELECT CASE i.USR_ENABLED WHEN 1 THEN 'U' ELSE 'D' END
           ,i.[USR_ID]
           ,i.[USR_EMAIL]           
           ,i.[USR_COU_ID]           
		  FROM Deleted d 
				INNER JOIN Inserted i ON d.[USR_ID] = i.[USR_ID]				
		  WHERE d.[USR_EMAIL]<>i.[USR_EMAIL] or
				d.[USR_COU_ID]<>i.[USR_COU_ID] or				
				d.[USR_ENABLED]<>i.[USR_ENABLED];



		   IF @@ERROR != 0
				 ROLLBACK TRAN

		
	END

	IF @CountInserted > 0
	BEGIN

		INSERT INTO USERS_SYNC ([USR_MOV_TYPE], [USR_ID], [USR_EMAIL], [USR_COU_ID])
		  SELECT 'I'
           ,i.[USR_ID]
           ,i.[USR_EMAIL]           
           ,i.[USR_COU_ID]           
		  FROM Inserted i
		  where i.[USR_ID] not in (select d.[USR_ID] from Deleted d) AND
				i.USR_ENABLED = 1;

		   IF @@ERROR != 0
				 ROLLBACK TRAN		
	END



	IF @CountDeleted > 0
	BEGIN

		INSERT INTO USERS_SYNC ([USR_MOV_TYPE], [USR_ID], [USR_EMAIL], [USR_COU_ID])
		  SELECT 'D'
           ,d.[USR_ID]
           ,d.[USR_EMAIL]           
           ,d.[USR_COU_ID]           
		  FROM Deleted d
		  where d.[USR_ID] not in (select i.[USR_ID] from Inserted i);

		   IF @@ERROR != 0
				 ROLLBACK TRAN		
	END

			
END

GO


