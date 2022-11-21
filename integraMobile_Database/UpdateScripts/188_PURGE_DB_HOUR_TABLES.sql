INSERT INTO [dbo].[DB_RECHARGES_HOUR_HIS]
SELECT [HOUR]
      ,[OPE_USR_ID]
      ,[OPE_AMOUNT_CUR_ISO_CODE]
      ,[RECHARGES_COUNT]
      ,[PREVRECHARGES_COUNT]
      ,[RECHARGES_AMOUNT]
      ,[HOUR_UTC]
      ,[RECHARGES_USERCREATION_COUNT]
      ,[RECHARGES_USERCREATION_AMOUNT]
  FROM [dbo].[DB_RECHARGES_HOUR]
  WHERE HOUR < '2020-01-01'

GO


DELETE 
FROM [dbo].[DB_RECHARGES_HOUR]
  WHERE HOUR < '2020-01-01'
GO


INSERT INTO [dbo].[DB_OPERATIONS_USERS_HOUR_HIS]
SELECT [HOUR]
      ,[OPE_INS_ID]
      ,[GRP_ID]
      ,[USERS_COUNT]
      ,[USERS_ALL_COUNT]
      ,[HOUR_UTC]
  FROM [dbo].[DB_OPERATIONS_USERS_HOUR]
  WHERE HOUR < '2020-01-01'

GO

DELETE 
  FROM [dbo].[DB_OPERATIONS_USERS_HOUR]
  WHERE HOUR < '2020-01-01'
GO


INSERT INTO [dbo].[DB_OPERATIONS_MINUTE_HIS]
SELECT [MINUTE]
      ,[OPE_INS_ID]
      ,[GRP_ID]
      ,[OPE_AMOUNT_CUR_ISO_CODE]
      ,[PARKINGS_COUNT]
      ,[EXTENSIONS_COUNT]
      ,[REFUNDS_COUNT]
      ,[TICKETPAYMENTS_COUNT]
      ,[RECHARGES_COUNT]
      ,[PARKINGS_AMOUNT]
      ,[EXTENSIONS_AMOUNT]
      ,[REFUNDS_AMOUNT]
      ,[RECHARGES_AMOUNT]
      ,[PARKINGS_TIME]
      ,[EXTENSIONS_TIME]
      ,[REFUNDS_TIME]
      ,[MINUTE_UTC]
      ,[TICKETPAYMENTS_NON_USERS_COUNT]
  FROM [dbo].[DB_OPERATIONS_MINUTE]
 WHERE MINUTE < '2020-01-01'
GO

DELETE 
   FROM [dbo].[DB_OPERATIONS_MINUTE]
 WHERE MINUTE < '2020-01-01'
GO


INSERT INTO [dbo].[DB_OPERATIONS_HOUR_HIS]
SELECT [DBOH_ID]
      ,[HOUR]
      ,[OPE_INS_ID]
      ,[GRP_ID]
      ,[OPE_AMOUNT_CUR_ISO_CODE]
      ,[PARKINGS_COUNT]
      ,[EXTENSIONS_COUNT]
      ,[REFUNDS_COUNT]
      ,[TICKETPAYMENTS_COUNT]
      ,[RECHARGES_COUNT]
      ,[RECHARGEREFUNDS_COUNT]
      ,[COUPONS_COUNT]
      ,[COUPONREFUNDS_COUNT]
      ,[SERVICES_COUNT]
      ,[PARKINGS_AMOUNT]
      ,[PARKINGS_VAT]
      ,[PARKINGS_PERC_FEE]
      ,[PARKINGS_PERC_FEE_VAT]
      ,[PARKINGS_FIXED_FEE]
      ,[PARKINGS_FIXED_FEE_VAT]
      ,[PARKINGS_TOTAL_AMOUNT]
      ,[EXTENSIONS_AMOUNT]
      ,[EXTENSIONS_VAT]
      ,[EXTENSIONS_PERC_FEE]
      ,[EXTENSIONS_PERC_FEE_VAT]
      ,[EXTENSIONS_FIXED_FEE]
      ,[EXTENSIONS_FIXED_FEE_VAT]
      ,[EXTENSIONS_TOTAL_AMOUNT]
      ,[REFUNDS_AMOUNT]
      ,[REFUNDS_VAT]
      ,[REFUNDS_PERC_FEE]
      ,[REFUNDS_PERC_FEE_VAT]
      ,[REFUNDS_FIXED_FEE]
      ,[REFUNDS_FIXED_FEE_VAT]
      ,[REFUNDS_TOTAL_AMOUNT]
      ,[TICKETPAYMENTS_AMOUNT]
      ,[TICKETPAYMENTS_VAT]
      ,[TICKETPAYMENTS_PERC_FEE]
      ,[TICKETPAYMENTS_PERC_FEE_VAT]
      ,[TICKETPAYMENTS_FIXED_FEE]
      ,[TICKETPAYMENTS_FIXED_FEE_VAT]
      ,[TICKETPAYMENTS_TOTAL_AMOUNT]
      ,[RECHARGES_AMOUNT]
      ,[RECHARGES_VAT]
      ,[RECHARGES_PERC_FEE]
      ,[RECHARGES_PERC_FEE_VAT]
      ,[RECHARGES_FIXED_FEE]
      ,[RECHARGES_FIXED_FEE_VAT]
      ,[RECHARGES_TOTAL_AMOUNT]
      ,[RECHARGEREFUNDS_TOTAL_AMOUNT]
      ,[COUPONS_AMOUNT]
      ,[COUPONS_VAT]
      ,[COUPONS_PERC_FEE]
      ,[COUPONS_PERC_FEE_VAT]
      ,[COUPONS_FIXED_FEE]
      ,[COUPONS_FIXED_FEE_VAT]
      ,[COUPONS_TOTAL_AMOUNT]
      ,[COUPONREFUNDS_TOTAL_AMOUNT]
      ,[SERVICES_AMOUNT]
      ,[SERVICES_VAT]
      ,[SERVICES_PERC_FEE]
      ,[SERVICES_PERC_FEE_VAT]
      ,[SERVICES_FIXED_FEE]
      ,[SERVICES_FIXED_FEE_VAT]
      ,[SERVICES_TOTAL_AMOUNT]
      ,[PARKINGS_TIME]
      ,[EXTENSIONS_TIME]
      ,[REFUNDS_TIME]
      ,[HOUR_UTC]
      ,[PAGATELIA_COUNT]
      ,[PAGATELIA_AMOUNT]
      ,[PAGATELIA_VAT]
      ,[PAGATELIA_PERC_FEE]
      ,[PAGATELIA_PERC_FEE_VAT]
      ,[PAGATELIA_FIXED_FEE]
      ,[PAGATELIA_FIXED_FEE_VAT]
      ,[PAGATELIA_TOTAL_AMOUNT]
      ,[RECHARGES_REGULAR_COUNT]
      ,[RECHARGES_AUTOMATIC_COUNT]
      ,[RECHARGES_USERCREATION_COUNT]
      ,[RECHARGES_CHANGEPM_COUNT]
      ,[RECHARGES_REGULAR_AMOUNT]
      ,[RECHARGES_REGULAR_VAT]
      ,[RECHARGES_REGULAR_PERC_FEE]
      ,[RECHARGES_REGULAR_PERC_FEE_VAT]
      ,[RECHARGES_REGULAR_FIXED_FEE]
      ,[RECHARGES_REGULAR_FIXED_FEE_VAT]
      ,[RECHARGES_REGULAR_TOTAL_AMOUNT]
      ,[RECHARGES_AUTOMATIC_AMOUNT]
      ,[RECHARGES_AUTOMATIC_VAT]
      ,[RECHARGES_AUTOMATIC_PERC_FEE]
      ,[RECHARGES_AUTOMATIC_PERC_FEE_VAT]
      ,[RECHARGES_AUTOMATIC_FIXED_FEE]
      ,[RECHARGES_AUTOMATIC_FIXED_FEE_VAT]
      ,[RECHARGES_AUTOMATIC_TOTAL_AMOUNT]
      ,[RECHARGES_USERCREATION_AMOUNT]
      ,[RECHARGES_USERCREATION_VAT]
      ,[RECHARGES_USERCREATION_PERC_FEE]
      ,[RECHARGES_USERCREATION_PERC_FEE_VAT]
      ,[RECHARGES_USERCREATION_FIXED_FEE]
      ,[RECHARGES_USERCREATION_FIXED_FEE_VAT]
      ,[RECHARGES_USERCREATION_TOTAL_AMOUNT]
      ,[RECHARGES_CHANGEPM_AMOUNT]
      ,[RECHARGES_CHANGEPM_VAT]
      ,[RECHARGES_CHANGEPM_PERC_FEE]
      ,[RECHARGES_CHANGEPM_PERC_FEE_VAT]
      ,[RECHARGES_CHANGEPM_FIXED_FEE]
      ,[RECHARGES_CHANGEPM_FIXED_FEE_VAT]
      ,[RECHARGES_CHANGEPM_TOTAL_AMOUNT]
      ,[TICKETPAYMENTS_NON_USERS_COUNT]
      ,[TICKETPAYMENTS_NON_USERS_AMOUNT]
      ,[TICKETPAYMENTS_NON_USERS_VAT]
      ,[TICKETPAYMENTS_NON_USERS_PERC_FEE]
      ,[TICKETPAYMENTS_NON_USERS_PERC_FEE_VAT]
      ,[TICKETPAYMENTS_NON_USERS_FIXED_FEE]
      ,[TICKETPAYMENTS_NON_USERS_FIXED_FEE_VAT]
      ,[TICKETPAYMENTS_NON_USERS_TOTAL_AMOUNT]
  FROM [dbo].[DB_OPERATIONS_HOUR]
WHERE HOUR < '2020-01-01'
GO

INSERT INTO [dbo].[DB_OPERATIONS_HOUR_DIST_HIS]
SELECT  [OHD_ID]
      ,[OHD_DBOH_ID]
      ,[OHD_FDO_ID]
      ,[OHD_PARKINGS_AMOUNT]
      ,[OHD_PARKINGS_VAT]
      ,[OHD_PARKINGS_PERC_FEE]
      ,[OHD_PARKINGS_PERC_FEE_VAT]
      ,[OHD_PARKINGS_FIXED_FEE]
      ,[OHD_PARKINGS_FIXED_FEE_VAT]
      ,[OHD_PARKINGS_TOTAL_AMOUNT]
      ,[OHD_EXTENSIONS_AMOUNT]
      ,[OHD_EXTENSIONS_VAT]
      ,[OHD_EXTENSIONS_PERC_FEE]
      ,[OHD_EXTENSIONS_PERC_FEE_VAT]
      ,[OHD_EXTENSIONS_FIXED_FEE]
      ,[OHD_EXTENSIONS_FIXED_FEE_VAT]
      ,[OHD_EXTENSIONS_TOTAL_AMOUNT]
      ,[OHD_REFUNDS_AMOUNT]
      ,[OHD_REFUNDS_VAT]
      ,[OHD_REFUNDS_PERC_FEE]
      ,[OHD_REFUNDS_PERC_FEE_VAT]
      ,[OHD_REFUNDS_FIXED_FEE]
      ,[OHD_REFUNDS_FIXED_FEE_VAT]
      ,[OHD_REFUNDS_TOTAL_AMOUNT]
      ,[OHD_TICKETPAYMENTS_AMOUNT]
      ,[OHD_TICKETPAYMENTS_VAT]
      ,[OHD_TICKETPAYMENTS_PERC_FEE]
      ,[OHD_TICKETPAYMENTS_PERC_FEE_VAT]
      ,[OHD_TICKETPAYMENTS_FIXED_FEE]
      ,[OHD_TICKETPAYMENTS_FIXED_FEE_VAT]
      ,[OHD_TICKETPAYMENTS_TOTAL_AMOUNT]
      ,[OHD_RECHARGES_AMOUNT]
      ,[OHD_RECHARGES_VAT]
      ,[OHD_RECHARGES_PERC_FEE]
      ,[OHD_RECHARGES_PERC_FEE_VAT]
      ,[OHD_RECHARGES_FIXED_FEE]
      ,[OHD_RECHARGES_FIXED_FEE_VAT]
      ,[OHD_RECHARGES_TOTAL_AMOUNT]
      ,[OHD_COUPONS_AMOUNT]
      ,[OHD_COUPONS_VAT]
      ,[OHD_COUPONS_PERC_FEE]
      ,[OHD_COUPONS_PERC_FEE_VAT]
      ,[OHD_COUPONS_FIXED_FEE]
      ,[OHD_COUPONS_FIXED_FEE_VAT]
      ,[OHD_COUPONS_TOTAL_AMOUNT]
      ,[OHD_SERVICES_AMOUNT]
      ,[OHD_SERVICES_VAT]
      ,[OHD_SERVICES_PERC_FEE]
      ,[OHD_SERVICES_PERC_FEE_VAT]
      ,[OHD_SERVICES_FIXED_FEE]
      ,[OHD_SERVICES_FIXED_FEE_VAT]
      ,[OHD_SERVICES_TOTAL_AMOUNT]
  FROM [dbo].[DB_OPERATIONS_HOUR_DIST]
  WHERE [OHD_DBOH_ID] IN (SELECT [DBOH_ID]
					FROM [dbo].[DB_OPERATIONS_HOUR]
					WHERE HOUR < '2020-01-01')
GO


DELETE
  FROM [dbo].[DB_OPERATIONS_HOUR_DIST]
  WHERE [OHD_DBOH_ID] IN (SELECT [DBOH_ID]
					FROM [dbo].[DB_OPERATIONS_HOUR]
					WHERE HOUR < '2020-01-01')
GO

DELETE
  FROM [dbo].[DB_OPERATIONS_HOUR]
WHERE HOUR < '2020-01-01'
GO