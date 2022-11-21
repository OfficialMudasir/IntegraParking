/****** Object:  Table [dbo].[STREETS]    Script Date: 20/04/2022 15:47:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STREETS](
	[STR_ID] [numeric](18, 0) NOT NULL,
	[STR_ID_EXT] [varchar](50) NOT NULL,
	[STR_DESCRIPTION] [varchar](255) NOT NULL,
	[STR_INS_ID] [numeric](18, 0) NOT NULL,
	[STR_DELETED] [int] NOT NULL,
 CONSTRAINT [PK_STREETS] PRIMARY KEY CLUSTERED 
(
	[STR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREETS] ADD  CONSTRAINT [DF_STREETS_STR_DELETED]  DEFAULT ((0)) FOR [STR_DELETED]
GO

ALTER TABLE [dbo].[STREETS]  WITH CHECK ADD  CONSTRAINT [FK_STREETS_INSTALLATIONS] FOREIGN KEY([STR_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[STREETS] CHECK CONSTRAINT [FK_STREETS_INSTALLATIONS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREETS', @level2type=N'COLUMN',@level2name=N'STR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREETS', @level2type=N'COLUMN',@level2name=N'STR_ID_EXT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREETS', @level2type=N'COLUMN',@level2name=N'STR_DESCRIPTION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREETS', @level2type=N'COLUMN',@level2name=N'STR_INS_ID'
GO






CREATE TABLE [dbo].[STREET_SECTIONS](
	[STRSE_ID] [numeric](18, 0) NOT NULL,
	[STRSE_ID_EXT] [varchar](50) NOT NULL,
	[STRSE_DESCRIPTION] [varchar](255) NOT NULL,
	[STRSE_STR_ID] [numeric](18, 0) NOT NULL,
	[STRSE_STR_ID_FROM] [numeric](18, 0) NULL,
	[STRSE_STR_ID_TO] [numeric](18, 0) NULL,
	[STRSE_FROM] [int] NULL,
	[STRSE_TO] [int] NULL,
	[STRSE_SIDE] [int] NOT NULL,
	[STRSE_GRP_ID] [numeric](18, 0) NOT NULL,
	[STRSE_DELETED] [int] NOT NULL,
	[STRSE_COLOUR] [varchar](6) NOT NULL,
	[STRSE_INS_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS] PRIMARY KEY CLUSTERED 
(
	[STRSE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS] ADD  CONSTRAINT [DF_STREET_SECTIONS_STRSE_DELETED]  DEFAULT ((0)) FOR [STRSE_DELETED]
GO

ALTER TABLE [dbo].[STREET_SECTIONS] ADD  CONSTRAINT [DF_STREET_SECTIONS_STRSE_SIDE]  DEFAULT ((0)) FOR [STRSE_SIDE]
GO

ALTER TABLE [dbo].[STREET_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_GROUPS] FOREIGN KEY([STRSE_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_GROUPS]
GO

ALTER TABLE [dbo].[STREET_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_INSTALLATIONS] FOREIGN KEY([STRSE_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_INSTALLATIONS]
GO

ALTER TABLE [dbo].[STREET_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_STREETS] FOREIGN KEY([STRSE_STR_ID])
REFERENCES [dbo].[STREETS] ([STR_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_STREETS]
GO

ALTER TABLE [dbo].[STREET_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_STREETS_FROM] FOREIGN KEY([STRSE_STR_ID_FROM])
REFERENCES [dbo].[STREETS] ([STR_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_STREETS_FROM]
GO

ALTER TABLE [dbo].[STREET_SECTIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_STREETS_TO] FOREIGN KEY([STRSE_STR_ID_TO])
REFERENCES [dbo].[STREETS] ([STR_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_STREETS_TO]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_ID_EXT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Stretch Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_DESCRIPTION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_STR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street ID From' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_STR_ID_FROM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street ID To' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_STR_ID_TO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Number From' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_FROM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Number To' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_TO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Side (0: unknown, 1: Odd, 2: Even' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_SIDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_GRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Registry is deleted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_DELETED'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Colour to be used in app' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_COLOUR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS', @level2type=N'COLUMN',@level2name=N'STRSE_INS_ID'
GO






CREATE TABLE [dbo].[STREET_SECTIONS_GEOMETRY](
	[STRSEGE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[STRSEGE_STRSE_ID] [numeric](18, 0) NOT NULL,
	[STRSEGE_POL_NUMBER] [int] NOT NULL,
	[STRSEGE_ORDER] [numeric](18, 0) NOT NULL,
	[STRSEGE_LATITUDE] [numeric](18, 12) NOT NULL,
	[STRSEGE_LONGITUDE] [numeric](18, 12) NOT NULL,
	[STRSEGE_INI_APPLY_DATE] [datetime] NOT NULL,
	[STRSEGE_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_GEOMETRY] PRIMARY KEY CLUSTERED 
(
	[STRSEGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GEOMETRY] ADD  CONSTRAINT [DF_STREET_SECTIONS_GEOMETRY_GRGE_POL_NUMBER]  DEFAULT ((1)) FOR [STRSEGE_POL_NUMBER]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GEOMETRY]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_GEOMETRY_STREET_SECTIONS] FOREIGN KEY([STRSEGE_STRSE_ID])
REFERENCES [dbo].[STREET_SECTIONS] ([STRSE_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GEOMETRY] CHECK CONSTRAINT [FK_STREET_SECTIONS_GEOMETRY_STREET_SECTIONS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_STRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Polygon Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_POL_NUMBER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order in the sequence of points' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_ORDER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Latitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_LATITUDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Longitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_LONGITUDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_INI_APPLY_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGE_END_APPLY_DATE'
GO




CREATE TABLE [dbo].[STREET_SECTIONS_GRID](
	[STRSEG_ID] [numeric](18, 0) NOT NULL,
	[STRSEG_DESCRIPTION] [varchar](50) NOT NULL,
	[STRSEG_X] [int] NOT NULL,
	[STRSEG_Y] [int] NOT NULL,
	[STRSEG_MAX_X] [int] NOT NULL,
	[STRSEG_MAX_Y] [int] NOT NULL,
	[STRSEG_INS_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_GRID] PRIMARY KEY CLUSTERED 
(
	[STRSEG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GRID]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_GRID_INSTALLATIONS] FOREIGN KEY([STRSEG_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GRID] CHECK CONSTRAINT [FK_STREET_SECTIONS_GRID_INSTALLATIONS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Stretch Grid Position' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_DESCRIPTION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'X Position' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_X'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Y Position' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_Y'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Max X Position' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_MAX_X'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Max Y Position' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_MAX_Y'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSEG_INS_ID'
GO





CREATE TABLE [dbo].[STREET_SECTIONS_GRID_GEOMETRY](
	[STRSEGG_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[STRSEGG_STRSEG_ID] [numeric](18, 0) NOT NULL,
	[STRSEGG_ORDER] [numeric](18, 0) NOT NULL,
	[STRSEGG_LATITUDE] [numeric](18, 12) NOT NULL,
	[STRSEGG_LONGITUDE] [numeric](18, 12) NOT NULL,
	[STRSEGG_INI_APPLY_DATE] [datetime] NOT NULL,
	[STRSEGG_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_GRID_GEOMETRY] PRIMARY KEY CLUSTERED 
(
	[STRSEGG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GRID_GEOMETRY]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_GRID_GEOMETRY_STREET_SECTIONS_GRID] FOREIGN KEY([STRSEGG_STRSEG_ID])
REFERENCES [dbo].[STREET_SECTIONS_GRID] ([STRSEG_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_GRID_GEOMETRY] CHECK CONSTRAINT [FK_STREET_SECTIONS_GRID_GEOMETRY_STREET_SECTIONS_GRID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Sections Grid ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_STRSEG_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order in the sequence of points' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_ORDER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Latitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_LATITUDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Longitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_LONGITUDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_INI_APPLY_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_GRID_GEOMETRY', @level2type=N'COLUMN',@level2name=N'STRSEGG_END_APPLY_DATE'
GO





CREATE TABLE [dbo].[STREET_SECTIONS_STREET_SECTIONS_GRID](
	[STRSESSG_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[STRSESSG_STRSE_ID] [numeric](18, 0) NOT NULL,
	[STRSESSG_STRSEG_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_STREET_SECTIONS_GRID] PRIMARY KEY CLUSTERED 
(
	[STRSESSG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_STREET_SECTIONS_GRID]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_STREET_SECTIONS_GRID_STREET_SECTIONS] FOREIGN KEY([STRSESSG_STRSE_ID])
REFERENCES [dbo].[STREET_SECTIONS] ([STRSE_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_STREET_SECTIONS_GRID] CHECK CONSTRAINT [FK_STREET_SECTIONS_STREET_SECTIONS_GRID_STREET_SECTIONS]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_STREET_SECTIONS_GRID]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_STREET_SECTIONS_GRID_STREET_SECTIONS_GRID] FOREIGN KEY([STRSESSG_STRSEG_ID])
REFERENCES [dbo].[STREET_SECTIONS_GRID] ([STRSEG_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_STREET_SECTIONS_GRID] CHECK CONSTRAINT [FK_STREET_SECTIONS_STREET_SECTIONS_GRID_STREET_SECTIONS_GRID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSESSG_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSESSG_STRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Grid ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_STREET_SECTIONS_GRID', @level2type=N'COLUMN',@level2name=N'STRSESSG_STRSEG_ID'
GO




CREATE TABLE [dbo].[STREET_SECTIONS_PACKAGE_VERSIONS](
	[STSEPV_ID] [numeric](18, 0) NOT NULL,
	[STSEPV_UTC_DATE] [datetime] NOT NULL,
	[STSEPV_FILE] [varbinary](max) NOT NULL,
	[STSEPV_INS_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_PACKAGE_VERSIONS] PRIMARY KEY CLUSTERED 
(
	[STSEPV_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_PACKAGE_VERSIONS] ADD  CONSTRAINT [DF_STREET_SECTIONS_PACKAGE_VERSIONS_OPE_UTC_DATE]  DEFAULT (getutcdate()) FOR [STSEPV_UTC_DATE]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_PACKAGE_VERSIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_PACKAGE_VERSIONS_INSTALLATIONS] FOREIGN KEY([STSEPV_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_PACKAGE_VERSIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_PACKAGE_VERSIONS_INSTALLATIONS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_PACKAGE_VERSIONS', @level2type=N'COLUMN',@level2name=N'STSEPV_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InsertionDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_PACKAGE_VERSIONS', @level2type=N'COLUMN',@level2name=N'STSEPV_UTC_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'File Content' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_PACKAGE_VERSIONS', @level2type=N'COLUMN',@level2name=N'STSEPV_FILE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_PACKAGE_VERSIONS', @level2type=N'COLUMN',@level2name=N'STSEPV_INS_ID'
GO





CREATE TABLE [dbo].[STREET_SECTIONS_TYPES](
	[STRSET_ID] [numeric](18, 0) NOT NULL,
	[STRSET_INS_ID] [numeric](18, 0) NOT NULL,
	[STRSET_DESCRIPTION] [varchar](50) NULL,
	[STRSET_ID_EXT] [varchar](50) NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_TYPES] PRIMARY KEY CLUSTERED 
(
	[STRSET_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_TYPES]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_TYPES_INSTALLATIONS] FOREIGN KEY([STRSET_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_TYPES] CHECK CONSTRAINT [FK_STREET_SECTIONS_TYPES_INSTALLATIONS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES', @level2type=N'COLUMN',@level2name=N'STRSET_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES', @level2type=N'COLUMN',@level2name=N'STRSET_INS_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Type Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES', @level2type=N'COLUMN',@level2name=N'STRSET_DESCRIPTION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Type external id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES', @level2type=N'COLUMN',@level2name=N'STRSET_ID_EXT'
GO






CREATE TABLE [dbo].[STREET_SECTIONS_TYPES_ASSIGNATIONS](
	[STRSETA_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[STRSETA_DESCRIPTION] [varchar](50) NOT NULL,
	[STRSETA_STRSE_ID] [numeric](18, 0) NOT NULL,
	[STRSETA_STRSET_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_STREET_SECTIONS_TYPES_ASSIGNATIONS] PRIMARY KEY CLUSTERED 
(
	[STRSETA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_TYPES_ASSIGNATIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_TYPES_ASSIGNATIONS_STREET_SECTIONS] FOREIGN KEY([STRSETA_STRSE_ID])
REFERENCES [dbo].[STREET_SECTIONS] ([STRSE_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_TYPES_ASSIGNATIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_TYPES_ASSIGNATIONS_STREET_SECTIONS]
GO

ALTER TABLE [dbo].[STREET_SECTIONS_TYPES_ASSIGNATIONS]  WITH CHECK ADD  CONSTRAINT [FK_STREET_SECTIONS_TYPES_ASSIGNATIONS_STREET_SECTIONS_TYPES] FOREIGN KEY([STRSETA_STRSET_ID])
REFERENCES [dbo].[STREET_SECTIONS_TYPES] ([STRSET_ID])
GO

ALTER TABLE [dbo].[STREET_SECTIONS_TYPES_ASSIGNATIONS] CHECK CONSTRAINT [FK_STREET_SECTIONS_TYPES_ASSIGNATIONS_STREET_SECTIONS_TYPES]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'STRSETA_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'STRSETA_DESCRIPTION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section  Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'STRSETA_STRSE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street Section Type Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STREET_SECTIONS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'STRSETA_STRSET_ID'
GO


