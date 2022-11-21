/****** Object:  StoredProcedure [dbo].[Report_LIQUIDATIONDETAIL_EXTENDED]    Script Date: 03/07/2020 9:47:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Report_LIQUIDATIONDETAIL_EXTENDED]
	@DateIniUTC AS DATETIME,
	@DateEndUTC AS DATETIME,
	@Culture	AS VARCHAR(5)
AS	
BEGIN

IF @Culture IS NULL
BEGIN
	SET @Culture = 'en-US'
END

SELECT  D.OPE_INS_ID, 
		INS_DESCRIPTION,
		GROUPS.GRP_SHOW_ID,		
		GROUPS.GRP_ID, 
		GROUPS.GRP_DESCRIPTION,
		SUM(PARKINGS_COUNT) Parkings,
		SUM(EXTENSIONS_COUNT) Extensions,
		SUM(REFUNDS_COUNT) Refunds,
		TPC.TICKETPAYMENTS_COUNT /*0*/ Payments,
		SUM(dbo.AmountChangedIso(PARKINGS_TOTAL_AMOUNT, OPE_AMOUNT_CUR_ISO_CODE)) Parkings_Amount,
		SUM(dbo.AmountChangedIso(EXTENSIONS_TOTAL_AMOUNT, OPE_AMOUNT_CUR_ISO_CODE)) Extensions_Amount,
		SUM(dbo.AmountChangedIso(REFUNDS_TOTAL_AMOUNT*-1, OPE_AMOUNT_CUR_ISO_CODE)) Refunds_Amount,
		SUB.Payments_Amount,
		SUM(dbo.AmountChangedIso(PARKINGS_TOTAL_AMOUNT+EXTENSIONS_TOTAL_AMOUNT+(REFUNDS_TOTAL_AMOUNT*-1), OPE_AMOUNT_CUR_ISO_CODE)) Total_Amount,
		1+INS_PERC_VAT2 VAT,
		CUR_ISO_CODE,
		POWER(10, CUR_MINOR_UNIT) DECIMAL_PART,
		CAST(ROUND(INS_PERC_VAT2*100,2) AS NUMERIC(18,2)) VAT_AMOUNT,
		CAST(ROUND(INS_PERC_VAT1*100,2) AS NUMERIC(18,2)) VAT1_AMOUNT,
		L1.LITL_LITERAL,
		L2.LITL_LITERAL FEE_LITERAL,
		LA1.LAN_DESCRIPTION,
		INS_CULTURE_LANG,
		INSF1.INSF_IS_TAX,
		INSF1.INSF_PERC_FEE_APPLY,
		INSF1.INSF_PERC_FEE,
		INSF1.INSF_FIXED_FEE_APPLY,
		INSF1.INSF_FIXED_FEE,
		INS_PERC_VAT1,
		INS_PERC_VAT2,
		INSF4.INSF_IS_TAX INSF_IS_TAX4,
		INSF4.INSF_PERC_FEE_APPLY INSF_PERC_FEE_APPLY4,
		INSF4.INSF_PERC_FEE INSF_PERC_FEE4,
		INSF4.INSF_FIXED_FEE_APPLY INSF_FIXED_FEE_APPLY4,
		INSF4.INSF_FIXED_FEE INSF_FIXED_FEE4
FROM DB_OPERATIONS_HOUR D WITH (NOLOCK)
		INNER JOIN INSTALLATIONS WITH (NOLOCK) ON OPE_INS_ID = INS_ID
		INNER JOIN CURRENCIES WITH (NOLOCK)ON INS_CUR_ID = CUR_ID
		LEFT JOIN GROUPS WITH (NOLOCK) ON D.GRP_ID = GROUPS.GRP_ID
		INNER JOIN LITERAL_LANGUAGES L1 WITH (NOLOCK) ON INS_SERVICE_VAT_LIT_ID = L1.LITL_LIT_ID
		INNER JOIN LITERAL_LANGUAGES L2 WITH (NOLOCK) ON INS_SERVICE_FEE_LIT_ID = L2.LITL_LIT_ID
		INNER JOIN LANGUAGES LA1 WITH (NOLOCK) ON L1.LITL_LAN_ID = LA1.LAN_ID
		INNER JOIN LANGUAGES LA2 WITH (NOLOCK) ON L2.LITL_LAN_ID = LA1.LAN_ID
		LEFT JOIN (
			SELECT  SUM(dbo.AmountChangedIso(TICKETPAYMENTS_TOTAL_AMOUNT, OPE_AMOUNT_CUR_ISO_CODE)) Payments_Amount, OPE_INS_ID
			FROM DB_OPERATIONS_HOUR WITH (NOLOCK)
			WHERE HOUR_UTC >= @DateIniUTC
				AND HOUR_UTC <= @DateEndUTC
			GROUP BY OPE_INS_ID
		) SUB ON D.OPE_INS_ID = SUB.OPE_INS_ID
		INNER JOIN INSTALLATIONS_FINAN_PARS_OPE_TYPE INSF1 WITH (NOLOCK) ON INS_ID = INSF1.INSF_INS_ID AND INSF1.INSF_OPE_TYPE = 1 AND INSF1.INSF_SUSCRIPTION_TYPE = 1 AND COALESCE(INSF1.INSF_TAR_TYPE, 0) = 0 /* INSF_TAR_TYPE = 0 DUE TO IPS-315 */
		INNER JOIN INSTALLATIONS_FINAN_PARS_OPE_TYPE INSF4 WITH (NOLOCK) ON INS_ID = INSF4.INSF_INS_ID AND INSF4.INSF_OPE_TYPE = 4 AND INSF4.INSF_SUSCRIPTION_TYPE = 1 AND COALESCE(INSF4.INSF_TAR_TYPE, 0) = 0 /* INSF_TAR_TYPE = 0 DUE TO IPS-315 */
		INNER JOIN (
			SELECT OPE_INS_ID, SUM(TICKETPAYMENTS_COUNT) TICKETPAYMENTS_COUNT
			FROM DB_OPERATIONS_HOUR WITH (NOLOCK)
			WHERE HOUR_UTC >= @DateIniUTC
			AND HOUR_UTC <= @DateEndUTC
			GROUP BY OPE_INS_ID
		) TPC ON D.OPE_INS_ID = TPC.OPE_INS_ID
WHERE HOUR_UTC >= @DateIniUTC
	AND HOUR_UTC <= @DateEndUTC
	AND LA1.LAN_CULTURE = @Culture
	AND LA2.LAN_CULTURE = @Culture
	AND GROUPS.GRP_DESCRIPTION IS NOT NULL
	AND (DBO.UDF_ConvertUtcToLocalByTimezoneIdentifier(INS_TIMEZONE_ID,@DateIniUTC) BETWEEN INSF1.INSF_INI_APPLY_DATE AND INSF1.INSF_END_APPLY_DATE OR  
		DBO.UDF_ConvertUtcToLocalByTimezoneIdentifier(INS_TIMEZONE_ID,@DateEndUTC) BETWEEN INSF1.INSF_INI_APPLY_DATE AND INSF1.INSF_END_APPLY_DATE)
	AND (DBO.UDF_ConvertUtcToLocalByTimezoneIdentifier(INS_TIMEZONE_ID,@DateIniUTC) BETWEEN INSF4.INSF_INI_APPLY_DATE AND INSF4.INSF_END_APPLY_DATE OR  
		DBO.UDF_ConvertUtcToLocalByTimezoneIdentifier(INS_TIMEZONE_ID,@DateEndUTC) BETWEEN INSF4.INSF_INI_APPLY_DATE AND INSF4.INSF_END_APPLY_DATE)
GROUP BY D.OPE_INS_ID, INS_DESCRIPTION, GROUPS.GRP_SHOW_ID, GROUPS.GRP_ID, GROUPS.GRP_DESCRIPTION, CUR_MINOR_UNIT,CUR_ISO_CODE,INS_PERC_VAT2,L1.LITL_LITERAL,L2.LITL_LITERAL,LA1.LAN_DESCRIPTION,INS_CULTURE_LANG,SUB.Payments_Amount,INSF1.INSF_IS_TAX,INSF1.INSF_PERC_FEE_APPLY,INSF1.INSF_PERC_FEE,INSF1.INSF_FIXED_FEE_APPLY,INSF1.INSF_FIXED_FEE,INSF4.INSF_IS_TAX,INSF4.INSF_PERC_FEE_APPLY,INSF4.INSF_PERC_FEE,INSF4.INSF_FIXED_FEE_APPLY,INSF4.INSF_FIXED_FEE,INS_PERC_VAT1,INS_PERC_VAT2,TPC.TICKETPAYMENTS_COUNT
ORDER BY D.OPE_INS_ID,GRP_SHOW_ID ASC
END
