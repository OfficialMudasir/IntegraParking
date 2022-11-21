CREATE TABLE [dbo].[INSTALLATIONS_SERVICES](
	[INSSER_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[INSSER_INS_ID] [numeric](18, 0) NOT NULL,
	[INSSER_SERTYP_ID] [numeric](18, 0) NOT NULL,
	[INSSER_ENABLED] [int] NOT NULL,
	[INSSER_DATE_INI] [datetime] NULL,
	[INSSER_DATE_END] [datetime] NULL,
 CONSTRAINT [PK_INSTALLATIONS_SERVICES] PRIMARY KEY CLUSTERED 
(
	[INSSER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[INSTALLATIONS_SERVICES] ADD  CONSTRAINT [DF_INSTALLATIONS_SERVICES_INSSER_ENABLED]  DEFAULT ((1)) FOR [INSSER_ENABLED]
GO

ALTER TABLE [dbo].[INSTALLATIONS_SERVICES]  WITH CHECK ADD  CONSTRAINT [FK_INSTALLATIONS_SERVICES_INSTALLATIONS] FOREIGN KEY([INSSER_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[INSTALLATIONS_SERVICES] CHECK CONSTRAINT [FK_INSTALLATIONS_SERVICES_INSTALLATIONS]
GO

ALTER TABLE [dbo].[INSTALLATIONS_SERVICES]  WITH CHECK ADD  CONSTRAINT [FK_INSTALLATIONS_SERVICES_SERVICES_TYPE] FOREIGN KEY([INSSER_SERTYP_ID])
REFERENCES [dbo].[SERVICES_TYPE] ([SERTYP_ID])
GO

ALTER TABLE [dbo].[INSTALLATIONS_SERVICES] CHECK CONSTRAINT [FK_INSTALLATIONS_SERVICES_SERVICES_TYPE]
GO