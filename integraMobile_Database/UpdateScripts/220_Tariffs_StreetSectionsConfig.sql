BEGIN TRANSACTION
GO
ALTER TABLE dbo.TARIFFS ADD
	TAR_PERMITS_EXT_ID varchar(50) NULL
GO
ALTER TABLE dbo.TARIFFS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE dbo.GROUPS ADD
	GRP_PERMITS_EXT_ID varchar(50) NULL
GO
ALTER TABLE dbo.GROUPS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



CREATE TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS](
	[TARSTRSE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[TARSTRSE_STRSE_ID] [numeric](18, 0) NULL,
	[TARSTRSE_STRSET_ID] [numeric](18, 0) NULL,
	[TARSTRSE_TAR_ID] [numeric](18, 0) NOT NULL,
	[TARSTRSE_TIME_STEPS_VALUE] [int] NOT NULL,
	[TARSTRSE_LIT_ID] [numeric](18, 0) NOT NULL,
	[TARSTRSE_USER_SELECTABLE] [int] NOT NULL,
	[TARSTRSE_INI_APPLY_DATE] [datetime] NOT NULL,
	[TARSTRSE_END_APPLY_DATE] [datetime] NOT NULL,
	[TARSTRSE_STEP1_MIN] [int] NULL,
	[TARSTRSE_STEP1_LIT_ID] [numeric](18, 0) NULL,
	[TARSTRSE_STEP2_MIN] [int] NULL,
	[TARSTRSE_STEP2_LIT_ID] [numeric](18, 0) NULL,
	[TARSTRSE_STEP3_MIN] [int] NULL,
	[TARSTRSE_STEP3_LIT_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_TARIFFS_IN_STREETS_SECTIONS] PRIMARY KEY CLUSTERED 
(
	[TARSTRSE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS] ADD  CONSTRAINT [DF_TARIFFS_IN_STREETS_SECTIONS_TARGR_TIME_STEPS_VALUE]  DEFAULT ((5)) FOR [TARSTRSE_TIME_STEPS_VALUE]
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_LITERALS] FOREIGN KEY([TARSTRSE_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS] CHECK CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_LITERALS]
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_STREET_SECTIONS] FOREIGN KEY([TARSTRSE_STRSE_ID])
REFERENCES [dbo].[STREET_SECTIONS] ([STRSE_ID])
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS] CHECK CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_STREET_SECTIONS]
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_STREET_SECTIONS_TYPES] FOREIGN KEY([TARSTRSE_STRSET_ID])
REFERENCES [dbo].[STREET_SECTIONS_TYPES] ([STRSET_ID])
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS] CHECK CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_STREET_SECTIONS_TYPES]
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_TARIFFS] FOREIGN KEY([TARSTRSE_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO

ALTER TABLE [dbo].[TARIFFS_IN_STREETS_SECTIONS] CHECK CONSTRAINT [FK_TARIFFS_IN_STREETS_SECTIONS_TARIFFS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Type Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STRSET_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_TAR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value to pass to tariff calculator as step offset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_TIME_STEPS_VALUE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_LIT_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User is going to have the possiblity of select in front other chances' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_USER_SELECTABLE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_INI_APPLY_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_END_APPLY_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Step1 minutes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STEP1_MIN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Step1 literal id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STEP1_LIT_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Step 2 minutes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STEP2_MIN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Step 2 literal id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STEP2_LIT_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Step 3 minutes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STEP3_MIN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Step 3 literal id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_STREETS_SECTIONS', @level2type=N'COLUMN',@level2name=N'TARSTRSE_STEP3_LIT_ID'
GO




CREATE TABLE [dbo].[TARIFF_IN_STREETS_SECTIONS_COMPILED](
	[TARSTRSEC_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[TARSTRSEC_STRSE_ID] [numeric](18, 0) NOT NULL,
	[TARSTRSEC_TAR_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_TARIFF_IN_STREETS_SECTIONS_COMPILED] PRIMARY KEY CLUSTERED 
(
	[TARSTRSEC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TARIFF_IN_STREETS_SECTIONS_COMPILED]  WITH CHECK ADD  CONSTRAINT [FK_TARIFF_IN_STREETS_SECTIONS_COMPILED_STREET_SECTIONS] FOREIGN KEY([TARSTRSEC_STRSE_ID])
REFERENCES [dbo].[STREET_SECTIONS] ([STRSE_ID])
GO

ALTER TABLE [dbo].[TARIFF_IN_STREETS_SECTIONS_COMPILED] CHECK CONSTRAINT [FK_TARIFF_IN_STREETS_SECTIONS_COMPILED_STREET_SECTIONS]
GO

ALTER TABLE [dbo].[TARIFF_IN_STREETS_SECTIONS_COMPILED]  WITH CHECK ADD  CONSTRAINT [FK_TARIFF_IN_STREETS_SECTIONS_COMPILED_TARIFFS] FOREIGN KEY([TARSTRSEC_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO

ALTER TABLE [dbo].[TARIFF_IN_STREETS_SECTIONS_COMPILED] CHECK CONSTRAINT [FK_TARIFF_IN_STREETS_SECTIONS_COMPILED_TARIFFS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFF_IN_STREETS_SECTIONS_COMPILED', @level2type=N'COLUMN',@level2name=N'TARSTRSEC_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFF_IN_STREETS_SECTIONS_COMPILED', @level2type=N'COLUMN',@level2name=N'TARSTRSEC_STRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFF_IN_STREETS_SECTIONS_COMPILED', @level2type=N'COLUMN',@level2name=N'TARSTRSEC_TAR_ID'
GO


