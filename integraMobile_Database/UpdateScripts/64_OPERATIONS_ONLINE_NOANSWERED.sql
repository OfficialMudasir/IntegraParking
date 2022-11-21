
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED](
	[OPE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[OPE_USR_ID] [numeric](18, 0) NOT NULL,
	[OPE_MOSE_OS] [int] NOT NULL,
	[OPE_USRP_ID] [numeric](18, 0) NULL,
	[OPE_INS_ID] [numeric](18, 0) NOT NULL,
	[OPE_TYPE] [int] NOT NULL,
	[OPE_GRP_ID] [numeric](18, 0) NULL,
	[OPE_TAR_ID] [numeric](18, 0) NULL,
	[OPE_DATE] [datetime] NOT NULL,
	[OPE_INIDATE] [datetime] NOT NULL,
	[OPE_ENDDATE] [datetime] NOT NULL,
	[OPE_UTC_DATE] [datetime] NOT NULL,
	[OPE_UTC_INIDATE] [datetime] NOT NULL,
	[OPE_UTC_ENDDATE] [datetime] NOT NULL,
	[OPE_DATE_UTC_OFFSET] [int] NOT NULL,
	[OPE_INIDATE_UTC_OFFSET] [int] NOT NULL,
	[OPE_ENDDATE_UTC_OFFSET] [int] NOT NULL,
	[OPE_AMOUNT] [int] NOT NULL,
	[OPE_TIME] [int] NOT NULL,
	[OPE_AMOUNT_CUR_ID] [numeric](18, 0) NOT NULL,
	[OPE_BALANCE_CUR_ID] [numeric](18, 0) NOT NULL,
	[OPE_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
	[OPE_CHANGE_FEE_APPLIED] [numeric](18, 6) NOT NULL,
	[OPE_FINAL_AMOUNT] [int] NOT NULL,
	[OPE_EXTERNAL_ID1] [varchar](50) NULL,
	[OPE_EXTERNAL_ID2] [varchar](50) NULL,
	[OPE_EXTERNAL_ID3] [varchar](50) NULL,
	[OPE_INSERTION_UTC_DATE] [datetime] NULL,
	[OPE_CUSPMR_ID] [numeric](18, 0) NULL,
	[OPE_OPEDIS_ID] [numeric](18, 0) NULL,
	[OPE_BALANCE_BEFORE] [int] NOT NULL,
	[OPE_SUSCRIPTION_TYPE] [int] NOT NULL,
	[OPE_CONFIRMED_IN_WS1] [int] NOT NULL,
	[OPE_CONFIRMED_IN_WS2] [int] NOT NULL,
	[OPE_CONFIRMED_IN_WS3] [int] NOT NULL,
	[OPE_CONFIRM_IN_WS1_RETRIES_NUM] [int] NULL,
	[OPE_CONFIRM_IN_WS2_RETRIES_NUM] [int] NULL,
	[OPE_CONFIRM_IN_WS3_RETRIES_NUM] [int] NULL,
	[OPE_CONFIRM_IN_WS1_DATE] [datetime] NULL,
	[OPE_CONFIRM_IN_WS2_DATE] [datetime] NULL,
	[OPE_CONFIRM_IN_WS3_DATE] [datetime] NULL,	
	[OPE_LATITUDE] [numeric](18, 12) NULL,
	[OPE_LONGITUDE] [numeric](18, 12) NULL,
	[OPE_APP_VERSION] [varchar](20) NULL,
	[OPE_CONFIRMATION_TIME_IN_WS1] [numeric](18, 0) NULL,
	[OPE_CONFIRMATION_TIME_IN_WS2] [numeric](18, 0) NULL,
	[OPE_CONFIRMATION_TIME_IN_WS3] [numeric](18, 0) NULL,
	[OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1] [int] NULL,
	[OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2] [int] NULL,
	[OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3] [int] NULL,
	[OPE_PERC_VAT1] [numeric](18, 5) NULL,
	[OPE_PERC_VAT2] [numeric](18, 5) NULL,
	[OPE_PARTIAL_VAT1] [numeric](18, 0) NULL,
	[OPE_PERC_FEE] [numeric](18, 5) NULL,
	[OPE_PERC_FEE_TOPPED] [numeric](18, 0) NULL,
	[OPE_PARTIAL_PERC_FEE] [numeric](18, 0) NULL,
	[OPE_FIXED_FEE] [numeric](18, 0) NULL,
	[OPE_PARTIAL_FIXED_FEE] [numeric](18, 0) NULL,
	[OPE_PERC_BONUS] [numeric](18, 5) NULL,
	[OPE_PARTIAL_BONUS_FEE] [numeric](18, 0) NULL,
	[OPE_TOTAL_AMOUNT] [int] NULL,
	[OPE_CUSINV_ID] [numeric](18, 0) NULL,
	[OPE_BONUS_ID] [varchar](50) NULL,
	[OPE_BONUS_MARCA] [varchar](50) NULL,
	[OPE_BONUS_TYPE] [int] NULL,
	[OPE_SPACE_STRING] [nvarchar](50) NULL,
	[OPE_TIME_BALANCE_USED] [int] NULL,
	[OPE_TIME_BALANCE_BEFORE] [int] NULL,
	[OPE_REAL_AMOUNT] [int] NULL,
	[OPE_POSTPAY] [int] NULL,
	[OPE_REFUND_PREVIOUS_ENDDATE] [datetime] NULL,
	[OPE_SHOPKEEPER_OP] [int] NULL,
	[OPE_SHOPKEEPER_PROFIT] [int] NULL,
	[OPE_SHOPKEEPER_AMOUNT] [int] NULL,
	[OPE_AUTH_ID] [numeric](18, 0) NULL,
	[OPE_AMOUNT_WITHOUT_BON] [int] NULL,
	[OPE_BON_MLT] [numeric](18, 5) NULL,
	[OPE_VEHICLE_TYPE] [varchar](50) NULL,
	[OPE_BACKOFFICE_USR] [varchar](256) NULL,
	[OPE_STOP_DATE] [datetime] NULL,
	[OPE_UTC_STOP_DATE] [datetime] NULL,
	[OPE_PARKING_MODE] [int] NOT NULL,
	[OPE_PARKING_MODE_STATUS] [int] NOT NULL,
	[OPE_EXTERNAL_BASE_ID1] [varchar](50) NULL,
	[OPE_EXTERNAL_BASE_ID2] [varchar](50) NULL,
	[OPE_EXTERNAL_BASE_ID3] [varchar](50) NULL,
	[OPE_PERMIT_AUTO_RENEW] [int] NULL,
	[OPE_PERMIT_EXPIRATION_TIME] [datetime] NULL,
	[OPE_PERMIT_LAST_AUTORENEWAL_DATE] [datetime] NULL,
	[OPE_PERMIT_LAST_AUTORENEWAL_NUM_RETRIES] [int] NULL,
	[OPE_PERMIT_LAST_AUTORENEWAL_STATUS] [int] NULL,
	[OPE_PERMIT_EXPIRATION_UTC_TIME] [datetime] NULL,
	[OPE_PLATE2_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE3_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE4_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE5_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE6_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE7_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE8_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE9_USRP_ID] [numeric](18, 0) NULL,
	[OPE_PLATE10_USRP_ID] [numeric](18, 0) NULL,
	[OPE_REL_OPE_ID] [numeric](18, 0) NULL,
	[OPE_CONFIRM_MODE] [int] NULL,
	[OPE_PERMIT_LAST_AUTORENEWAL_RESULT] [int] NULL,
 CONSTRAINT [PK_OPERATIONS_ONLINE_NOANSWERED] PRIMARY KEY CLUSTERED 
(
	[OPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_MOSE_OS]  DEFAULT ((1)) FOR [OPE_MOSE_OS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_TYPE]  DEFAULT ((1)) FOR [OPE_TYPE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_UTC_DATE]  DEFAULT (getutcdate()) FOR [OPE_UTC_DATE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_UTC_INIDATE]  DEFAULT (getutcdate()) FOR [OPE_UTC_INIDATE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_UTC_ENDDATE]  DEFAULT (getutcdate()) FOR [OPE_UTC_ENDDATE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_DATE_UTC_OFFSET]  DEFAULT ((0)) FOR [OPE_DATE_UTC_OFFSET]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_INIDATE_UTC_OFFSET]  DEFAULT ((0)) FOR [OPE_INIDATE_UTC_OFFSET]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_ENDDATE_UTC_OFFSET]  DEFAULT ((0)) FOR [OPE_ENDDATE_UTC_OFFSET]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_AMOUNT_CUR_ID]  DEFAULT ((50)) FOR [OPE_AMOUNT_CUR_ID]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_BALANCE_CUR_ID]  DEFAULT ((50)) FOR [OPE_BALANCE_CUR_ID]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CHANGE_APPLIED]  DEFAULT ((1)) FOR [OPE_CHANGE_APPLIED]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_BALANCE_BEFORE]  DEFAULT ((0)) FOR [OPE_BALANCE_BEFORE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_SUSCRIPTION_TYPE]  DEFAULT ((1)) FOR [OPE_SUSCRIPTION_TYPE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CONFIRMED_IN_WS21_1]  DEFAULT ((1)) FOR [OPE_CONFIRMED_IN_WS1]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CONFIRMED_IN_WS2]  DEFAULT ((1)) FOR [OPE_CONFIRMED_IN_WS2]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CONFIRMED_IN_WS21]  DEFAULT ((1)) FOR [OPE_CONFIRMED_IN_WS3]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CONFIRM_IN_WS1_RETRIES_NUM]  DEFAULT ((0)) FOR [OPE_CONFIRM_IN_WS1_RETRIES_NUM]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CONFIRM_IN_WS1_RETRIES_NUM1]  DEFAULT ((0)) FOR [OPE_CONFIRM_IN_WS2_RETRIES_NUM]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_CONFIRM_IN_WS1_RETRIES_NUM2]  DEFAULT ((0)) FOR [OPE_CONFIRM_IN_WS3_RETRIES_NUM]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_SHOPKEEPER_OP]  DEFAULT ((0)) FOR [OPE_SHOPKEEPER_OP]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_SHOPKEEPER_PROFIT]  DEFAULT ((0)) FOR [OPE_SHOPKEEPER_PROFIT]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_SHOPKEEPER_AMOUNT]  DEFAULT ((0)) FOR [OPE_SHOPKEEPER_AMOUNT]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_PARKING_MODE]  DEFAULT ((1)) FOR [OPE_PARKING_MODE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] ADD  CONSTRAINT [DF_OPERATIONS_ONLINE_NOANSWERED_OPE_PARKING_MODE_STATUS]  DEFAULT ((2)) FOR [OPE_PARKING_MODE_STATUS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CURRENCIES_AMOUNT] FOREIGN KEY([OPE_AMOUNT_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CURRENCIES_AMOUNT]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CURRENCIES_BALANCE] FOREIGN KEY([OPE_BALANCE_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CURRENCIES_BALANCE]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CUSTOMER_INVOICES] FOREIGN KEY([OPE_CUSINV_ID])
REFERENCES [dbo].[CUSTOMER_INVOICES] ([CUSINV_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CUSTOMER_INVOICES]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CUSTOMER_PAYMENT_MEANS_RECHARGES] FOREIGN KEY([OPE_CUSPMR_ID])
REFERENCES [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ([CUSPMR_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_CUSTOMER_PAYMENT_MEANS_RECHARGES]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_GROUPS] FOREIGN KEY([OPE_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_GROUPS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_INSTALLATIONS] FOREIGN KEY([OPE_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_INSTALLATIONS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_OPERATIONS] FOREIGN KEY([OPE_REL_OPE_ID])
REFERENCES [dbo].[OPERATIONS_ONLINE_NOANSWERED] ([OPE_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_OPERATIONS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_OPERATIONS_DISCOUNTS] FOREIGN KEY([OPE_OPEDIS_ID])
REFERENCES [dbo].[OPERATIONS_DISCOUNTS] ([OPEDIS_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_OPERATIONS_DISCOUNTS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_TARIFFS] FOREIGN KEY([OPE_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_TARIFFS]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES] FOREIGN KEY([OPE_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES1] FOREIGN KEY([OPE_PLATE2_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES1]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES2] FOREIGN KEY([OPE_PLATE3_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES2]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES3] FOREIGN KEY([OPE_PLATE4_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES3]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES4] FOREIGN KEY([OPE_PLATE5_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES4]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES5] FOREIGN KEY([OPE_PLATE6_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES5]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES6] FOREIGN KEY([OPE_PLATE7_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES6]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES7] FOREIGN KEY([OPE_PLATE8_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES7]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES8] FOREIGN KEY([OPE_PLATE9_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES8]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES9] FOREIGN KEY([OPE_PLATE10_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USER_PLATES9]
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USERS] FOREIGN KEY([OPE_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO

ALTER TABLE [dbo].[OPERATIONS_ONLINE_NOANSWERED] CHECK CONSTRAINT [FK_OPERATIONS_ONLINE_NOANSWERED_USERS]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_USR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: Android; 2: Windows Phone; 3: iOS; 4:Blackberry; 5: Web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_MOSE_OS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_INS_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation type ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_GRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_TAR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Ini Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_INIDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation End' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_ENDDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_UTC_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC Ini Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_UTC_INIDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC End Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_UTC_ENDDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Utc offset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_DATE_UTC_OFFSET'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Ini Date UTC Offset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_INIDATE_UTC_OFFSET'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation End UTC offset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_ENDDATE_UTC_OFFSET'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_AMOUNT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time in minutes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_TIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Currency ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_AMOUNT_CUR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Balance Currency Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BALANCE_CUR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation change applied' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CHANGE_APPLIED'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Change Fee applied' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CHANGE_FEE_APPLIED'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Final Amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_FINAL_AMOUNT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation External WS1 returned Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_EXTERNAL_ID1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation External WS2 returned Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_EXTERNAL_ID2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation External WS3 returned Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_EXTERNAL_ID3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Insertion UTC Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_INSERTION_UTC_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Recharge ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CUSPMR_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Discount id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_OPEDIS_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Balance Before Operation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BALANCE_BEFORE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Suscription type (prepaid or pay by transaction)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_SUSCRIPTION_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirmed in WS 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRMED_IN_WS1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirmed in WS 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRMED_IN_WS2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirmed in WS 3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRMED_IN_WS3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm retries in WS1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRM_IN_WS1_RETRIES_NUM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm retries in WS2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRM_IN_WS2_RETRIES_NUM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm retries in WS3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRM_IN_WS3_RETRIES_NUM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm in WS1 Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRM_IN_WS1_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm in WS2 Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRM_IN_WS2_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm in Ws3 Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRM_IN_WS3_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Latitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_LATITUDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Longitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_LONGITUDE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'App Version' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_APP_VERSION'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirmation Time in WS1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRMATION_TIME_IN_WS1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirmation Time in WS2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRMATION_TIME_IN_WS2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirmation Time in WS3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CONFIRMATION_TIME_IN_WS3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queue length before confirmation in WS1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queue length before confirmation in WS2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queue length before confirmation in WS3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAT 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERC_VAT1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAT 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERC_VAT2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PARTIAL_VAT1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Perc Fee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERC_FEE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Perc Fee Topped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERC_FEE_TOPPED'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Percentage Fee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PARTIAL_PERC_FEE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Fixed fee charged' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_FIXED_FEE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Fixed Fee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PARTIAL_FIXED_FEE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Bonus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERC_BONUS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Bonus Fee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PARTIAL_BONUS_FEE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_TOTAL_AMOUNT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Assigned Invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_CUSINV_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bonus ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BONUS_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bonus marca' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BONUS_MARCA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bonus type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BONUS_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Space textual identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_SPACE_STRING'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time Balance Used In minutes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_TIME_BALANCE_USED'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Time Balance Before Operation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_TIME_BALANCE_BEFORE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ope_amount + value of time_balance used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_REAL_AMOUNT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Is Parking operation a postpay' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_POSTPAY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parking chain end date time before refund' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_REFUND_PREVIOUS_ENDDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Is Shopkeeper operation: 0: No ; 1:yes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_SHOPKEEPER_OP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shopkeeper Profit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_SHOPKEEPER_PROFIT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shopkeeper Total Amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_SHOPKEEPER_AMOUNT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation Bonus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BON_MLT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Backoffice user in case of cash recharge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_BACKOFFICE_USR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: normal mode, 2: start/stop mode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PARKING_MODE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: opened, 2: closed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PARKING_MODE_STATUS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation External WS1 returned base Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_EXTERNAL_BASE_ID1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation External WS2 returned base Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_EXTERNAL_BASE_ID2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation External WS3 returned base Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_EXTERNAL_BASE_ID3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permit Autorenew' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERMIT_AUTO_RENEW'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permit Expiration Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERMIT_EXPIRATION_TIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permit Last Autorenewal Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERMIT_LAST_AUTORENEWAL_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permit Last Autorenewal num Retries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERMIT_LAST_AUTORENEWAL_NUM_RETRIES'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permit Last Autorenewal Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERMIT_LAST_AUTORENEWAL_STATUS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permit Expiration utc Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PERMIT_EXPIRATION_UTC_TIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 2 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE2_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 3 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE3_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 4 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE4_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 5 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE5_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 6 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE6_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 7 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE7_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 8 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE8_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 9 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE9_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Plate 10 id optional' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED', @level2type=N'COLUMN',@level2name=N'OPE_PLATE10_USRP_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Table to store online Parking, Extension and Refund operations not answered' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATIONS_ONLINE_NOANSWERED'
GO


