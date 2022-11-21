/****** Object:  View [dbo].[ALL_OPERATIONS]    Script Date: 15/01/2019 16:23:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[ALL_OPERATIONS]
AS
SELECT        CASE ISNULL(TAR_TYPE, 0) WHEN 1 THEN 19 ELSE OPE_TYPE END AS OPE_TYPE, OPE_USR_ID, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, OPE_DATE, OPE_UTC_DATE, OPE_INIDATE, OPE_ENDDATE, 
                         OPE_REFUND_PREVIOUS_ENDDATE, CAST(CASE WHEN OPE_TYPE = 3 THEN OPE_AMOUNT ELSE - OPE_AMOUNT END AS int) OPE_AMOUNT, CAST(CASE WHEN OPE_TYPE = 3 THEN ISNULL(OPE_REAL_AMOUNT, 
                         OPE_AMOUNT) ELSE - ISNULL(OPE_REAL_AMOUNT, OPE_AMOUNT) END AS int) OPE_REAL_AMOUNT, OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
                         cast(CUR1.CUR_MINOR_UNIT AS int) OPE_AMOUNT_CUR_MINOR_UNIT, CAST(CASE WHEN OPE_TYPE = 3 THEN OPE_FINAL_AMOUNT ELSE - OPE_FINAL_AMOUNT END AS int) OPE_FINAL_AMOUNT, OPE_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, cast(CUR2.CUR_MINOR_UNIT AS int) OPE_BALANCE_CUR_MINOR_UNIT, OPE_CHANGE_APPLIED, USER_PLATES.USRP_ID, USER_PLATES.USRP_PLATE, NULL 
                         TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, GRP_DESCRIPTION, TAR_DESCRIPTION, OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE, 
                         OPE_DATE_UTC_OFFSET, OPE_INIDATE_UTC_OFFSET, OPE_ENDDATE_UTC_OFFSET, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, OPE_PERC_VAT1, OPE_PERC_VAT2, 
                         CAST(CASE WHEN OPE_TYPE = 3 THEN OPE_PARTIAL_VAT1 ELSE - OPE_PARTIAL_VAT1 END AS int) OPE_PARTIAL_VAT1, OPE_PERC_FEE, OPE_PERC_FEE_TOPPED, 
                         CAST(CASE WHEN OPE_TYPE = 3 THEN OPE_PARTIAL_PERC_FEE ELSE - OPE_PARTIAL_PERC_FEE END AS int) OPE_PARTIAL_PERC_FEE, OPE_FIXED_FEE, 
                         CAST(CASE WHEN OPE_TYPE = 3 THEN OPE_PARTIAL_FIXED_FEE ELSE - OPE_PARTIAL_FIXED_FEE END AS int) OPE_PARTIAL_FIXED_FEE, CAST(CASE WHEN OPE_TYPE = 3 THEN ISNULL(OPE_TOTAL_AMOUNT, 
                         OPE_AMOUNT) ELSE - ISNULL(OPE_TOTAL_AMOUNT, OPE_AMOUNT) END AS int) OPE_TOTAL_AMOUNT, CASE WHEN ISNULL(OPE_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(OPE_PERC_FEE_TOPPED, 0) 
                         < (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) THEN OPE_PERC_FEE_TOPPED ELSE (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) END + ISNULL(OPE_FIXED_FEE, 0) OPE_FEE, CASE WHEN ISNULL(OPE_PERC_BONUS, 0) 
                         > 0 THEN - OPE_PERC_BONUS * (CASE WHEN ISNULL(OPE_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(OPE_PERC_FEE_TOPPED, 0) < (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) 
                         THEN OPE_PERC_FEE_TOPPED ELSE (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) END + ISNULL(OPE_FIXED_FEE, 0)) ELSE 0 END OPE_BONUS, ISNULL(OPE_PARTIAL_VAT1, 0) 
                         + ((CASE WHEN ISNULL(OPE_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(OPE_PERC_FEE_TOPPED, 0) < (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) 
                         THEN OPE_PERC_FEE_TOPPED ELSE (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) END) * ISNULL(OPE_PERC_VAT2, 0)) + (ISNULL(OPE_FIXED_FEE, 0) * ISNULL(OPE_PERC_VAT2, 0)) 
                         - CASE WHEN ISNULL(OPE_PERC_BONUS, 0) > 0 THEN OPE_PARTIAL_BONUS_FEE - (OPE_PERC_BONUS * (CASE WHEN ISNULL(OPE_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(OPE_PERC_FEE_TOPPED, 0) 
                         < (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) THEN OPE_PERC_FEE_TOPPED ELSE (OPE_AMOUNT * ISNULL(OPE_PERC_FEE, 0)) END + ISNULL(OPE_FIXED_FEE, 0))) ELSE 0 END OPE_VAT, NULL 
                         OPE_ADDITIONAL_USR_ID, NULL OPE_ADDITIONAL_USR_USERNAME, NULL OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, 
                         CAST(CASE WHEN OPE_TYPE = 3 THEN OPE_TIME_BALANCE_USED ELSE - OPE_TIME_BALANCE_USED END AS int) OPE_TIME_BALANCE_USED, OPE_TIME_BALANCE_BEFORE, OPE_POSTPAY, OPE_SHOPKEEPER_OP, 
                         OPE_SHOPKEEPER_AMOUNT, OPE_SHOPKEEPER_PROFIT, TAR_TYPE, UP2.USRP_PLATE USRP_PLATE2, UP3.USRP_PLATE USRP_PLATE3, UP4.USRP_PLATE USRP_PLATE4, UP5.USRP_PLATE USRP_PLATE5, 
                         UP6.USRP_PLATE USRP_PLATE6, UP7.USRP_PLATE USRP_PLATE7, UP8.USRP_PLATE USRP_PLATE8, UP9.USRP_PLATE USRP_PLATE9, UP10.USRP_PLATE USRP_PLATE10, OPE_PLATE2_USRP_ID, 
						 OPE_PLATE3_USRP_ID, OPE_PLATE4_USRP_ID, OPE_PLATE5_USRP_ID, OPE_PLATE6_USRP_ID, OPE_PLATE7_USRP_ID, OPE_PLATE8_USRP_ID, OPE_PLATE9_USRP_ID, OPE_PLATE10_USRP_ID,
						 OPE_ID
FROM            dbo.HIS_OPERATIONS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.HIS_OPERATIONS.OPE_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID LEFT OUTER JOIN
                         dbo.GROUPS ON OPE_GRP_ID = GRP_ID LEFT OUTER JOIN
                         dbo.TARIFFS ON OPE_TAR_ID = TAR_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP2 ON dbo.HIS_OPERATIONS.OPE_PLATE2_USRP_ID = UP2.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP3 ON dbo.HIS_OPERATIONS.OPE_PLATE3_USRP_ID = UP3.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP4 ON dbo.HIS_OPERATIONS.OPE_PLATE4_USRP_ID = UP4.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP5 ON dbo.HIS_OPERATIONS.OPE_PLATE5_USRP_ID = UP5.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP6 ON dbo.HIS_OPERATIONS.OPE_PLATE6_USRP_ID = UP6.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP7 ON dbo.HIS_OPERATIONS.OPE_PLATE7_USRP_ID = UP7.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP8 ON dbo.HIS_OPERATIONS.OPE_PLATE8_USRP_ID = UP8.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP9 ON dbo.HIS_OPERATIONS.OPE_PLATE9_USRP_ID = UP9.USRP_ID LEFT OUTER JOIN
                         dbo.USER_PLATES UP10 ON dbo.HIS_OPERATIONS.OPE_PLATE10_USRP_ID = UP10.USRP_ID, INSTALLATIONS
WHERE        OPE_INS_ID = INS_ID
UNION ALL
SELECT        4 OPE_TYPE, TIPA_USR_ID, TIPA_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, TIPA_DATE, TIPA_UTC_DATE, NULL, NULL, NULL, - TIPA_AMOUNT TIPA_AMOUNT, - TIPA_AMOUNT, NULL, TIPA_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, cast(CUR1.CUR_MINOR_UNIT AS int) OPE_AMOUNT_CUR_MINOR_UNIT, - TIPA_FINAL_AMOUNT, TIPA_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, cast(CUR2.CUR_MINOR_UNIT AS int) OPE_BALANCE_CUR_MINOR_UNIT, TIPA_CHANGE_APPLIED, USRP_ID, TIPA_PLATE_STRING, TIPA_TICKET_NUMBER, 
                         TIPA_TICKET_DATA, NULL, NULL, NULL, NULL, NULL, TIPA_SUSCRIPTION_TYPE, TIPA_BALANCE_BEFORE, TIPA_INSERTION_UTC_DATE, TIPA_DATE_UTC_OFFSET, NULL, NULL, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, 
                         TIPA_PERC_VAT1, TIPA_PERC_VAT2, - TIPA_PARTIAL_VAT1, TIPA_PERC_FEE, TIPA_PERC_FEE_TOPPED, - TIPA_PARTIAL_PERC_FEE, TIPA_FIXED_FEE, - TIPA_PARTIAL_FIXED_FEE, - ISNULL(TIPA_TOTAL_AMOUNT, 
                         TIPA_AMOUNT), CASE WHEN ISNULL(TIPA_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(TIPA_PERC_FEE_TOPPED, 0) < (TIPA_AMOUNT * ISNULL(TIPA_PERC_FEE, 0)) 
                         THEN TIPA_PERC_FEE_TOPPED ELSE (TIPA_AMOUNT * ISNULL(TIPA_PERC_FEE, 0)) END + ISNULL(TIPA_FIXED_FEE, 0), 0, ISNULL(TIPA_PARTIAL_VAT1, 0) + ((CASE WHEN ISNULL(TIPA_PERC_FEE_TOPPED, 0) > 0 AND 
                         ISNULL(TIPA_PERC_FEE_TOPPED, 0) < (TIPA_AMOUNT * ISNULL(TIPA_PERC_FEE, 0)) THEN TIPA_PERC_FEE_TOPPED ELSE (TIPA_AMOUNT * ISNULL(TIPA_PERC_FEE, 0)) END) * ISNULL(TIPA_PERC_VAT2, 0)) 
                         + (ISNULL(TIPA_FIXED_FEE, 0) * ISNULL(TIPA_PERC_VAT2, 0)), NULL, NULL, NULL OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL 
                         OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
						 NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 TIPA_ID
FROM            dbo.TICKET_PAYMENTS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.TICKET_PAYMENTS.TIPA_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON TIPA_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON TIPA_BALANCE_CUR_ID = CUR2.CUR_ID, INSTALLATIONS
WHERE        TIPA_INS_ID = INS_ID
UNION ALL
SELECT        5 OPE_TYPE, CUSPMR_USR_ID, NULL, NULL, NULL, CUSPMR_DATE, CUSPMR_UTC_DATE, NULL, NULL, NULL, CUSPMR_AMOUNT, CUSPMR_AMOUNT, NULL, CUSPMR_CUR_ID, CUR_ISO_CODE, 
                         cast(CUR_MINOR_UNIT AS int), NULL, CUSPMR_CUR_ID, CUR_ISO_CODE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CUSPMR_SUSCRIPTION_TYPE, CUSPMR_BALANCE_BEFORE, 
                         CUSPMR_INSERTION_UTC_DATE, CUSPMR_DATE_UTC_OFFSET, NULL, NULL, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, CUSPMR_PERC_VAT1, CUSPMR_PERC_VAT2, CUSPMR_PARTIAL_VAT1, CUSPMR_PERC_FEE, 
                         CUSPMR_PERC_FEE_TOPPED, CUSPMR_PARTIAL_PERC_FEE, CUSPMR_FIXED_FEE, CUSPMR_PARTIAL_FIXED_FEE, ISNULL(CUSPMR_TOTAL_AMOUNT_CHARGED, CUSPMR_AMOUNT), 
                         CASE WHEN ISNULL(CUSPMR_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(CUSPMR_PERC_FEE_TOPPED, 0) < (CUSPMR_AMOUNT * ISNULL(CUSPMR_PERC_FEE, 0)) 
                         THEN CUSPMR_PERC_FEE_TOPPED ELSE (CUSPMR_AMOUNT * ISNULL(CUSPMR_PERC_FEE, 0)) END + ISNULL(CUSPMR_FIXED_FEE, 0), 0, ISNULL(CUSPMR_PARTIAL_VAT1, 0) 
                         + ((CASE WHEN ISNULL(CUSPMR_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(CUSPMR_PERC_FEE_TOPPED, 0) < (CUSPMR_AMOUNT * ISNULL(CUSPMR_PERC_FEE, 0)) 
                         THEN CUSPMR_PERC_FEE_TOPPED ELSE (CUSPMR_AMOUNT * ISNULL(CUSPMR_PERC_FEE, 0)) END) * ISNULL(CUSPMR_PERC_VAT2, 0)) + (ISNULL(CUSPMR_FIXED_FEE, 0) * ISNULL(CUSPMR_PERC_VAT2, 
                         0)), NULL, NULL, CUSPMR_TYPE OPE_CUSPMR_TYPE, CUSPMR_PAGATELIA_NEW_BALANCE OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL OPE_TIME_BALANCE_BEFORE, NULL 
                         OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
						 NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 CUSPMR_ID
FROM            dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES, dbo.CURRENCIES
WHERE        CUSPMR_CUR_ID = CUR_ID AND (CUSPMR_SUSCRIPTION_TYPE = 1 OR
                         CUSPMR_SUSCRIPTION_TYPE IS NULL) AND (CUSPMR_TRANS_STATUS IN (1, 2, 3, 4) OR
                         CUSPMR_RCOUP_ID IS NOT NULL OR
                         CUSPMR_TYPE IN (3, 4, 6))
UNION ALL
SELECT        6 OPE_TYPE, SECH_USR_ID, NULL, NULL, NULL, SECH_DATE, SECH_UTC_DATE, NULL, NULL, NULL, - SECH_AMOUNT, - SECH_AMOUNT, NULL, SECH_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, cast(CUR1.CUR_MINOR_UNIT AS int) OPE_AMOUNT_CUR_MINOR_UNIT, - SECH_FINAL_AMOUNT, SECH_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, cast(CUR2.CUR_MINOR_UNIT AS int) OPE_BALANCE_CUR_MINOR_UNIT, SECH_CHANGE_APPLIED, NULL, NULL, NULL, NULL, 
                         SECH_SECHT_ID, NULL, NULL, NULL, NULL, SECH_SUSCRIPTION_TYPE, SECH_BALANCE_BEFORE, SECH_INSERTION_UTC_DATE, SECH_DATE_UTC_OFFSET, NULL, NULL, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, 
                         SECH_PERC_VAT1, SECH_PERC_VAT2, - SECH_PARTIAL_VAT1, SECH_PERC_FEE, SECH_PERC_FEE_TOPPED, - SECH_PARTIAL_PERC_FEE, SECH_FIXED_FEE, - SECH_PARTIAL_FIXED_FEE, - ISNULL(SECH_TOTAL_AMOUNT, 
                         SECH_AMOUNT), CASE WHEN ISNULL(SECH_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(SECH_PERC_FEE_TOPPED, 0) < (SECH_AMOUNT * ISNULL(SECH_PERC_FEE, 0)) 
                         THEN SECH_PERC_FEE_TOPPED ELSE (SECH_AMOUNT * ISNULL(SECH_PERC_FEE, 0)) END + ISNULL(SECH_FIXED_FEE, 0), 0, ISNULL(SECH_PARTIAL_VAT1, 0) + ((CASE WHEN ISNULL(SECH_PERC_FEE_TOPPED, 0) > 0 AND
                          ISNULL(SECH_PERC_FEE_TOPPED, 0) < (SECH_AMOUNT * ISNULL(SECH_PERC_FEE, 0)) THEN SECH_PERC_FEE_TOPPED ELSE (SECH_AMOUNT * ISNULL(SECH_PERC_FEE, 0)) END) * ISNULL(SECH_PERC_VAT2, 0)) 
                         + (ISNULL(SECH_FIXED_FEE, 0) * ISNULL(SECH_PERC_VAT2, 0)), NULL, NULL, NULL OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL 
                         OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 SECH_ID
FROM            dbo.SERVICE_CHARGES INNER JOIN
                         dbo.CURRENCIES CUR1 ON SECH_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON SECH_BALANCE_CUR_ID = CUR2.CUR_ID
UNION ALL
SELECT        7 OPE_TYPE, OPEDIS_USR_ID, NULL, NULL, NULL, OPEDIS_DATE, OPEDIS_UTC_DATE, NULL, NULL, NULL, OPEDIS_AMOUNT, OPEDIS_AMOUNT, NULL, OPEDIS_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, cast(CUR1.CUR_MINOR_UNIT AS int) OPE_AMOUNT_CUR_MINOR_UNIT, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, cast(CUR2.CUR_MINOR_UNIT AS int) OPE_BALANCE_CUR_MINOR_UNIT, OPEDIS_CHANGE_APPLIED, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
                         OPEDIS_SUSCRIPTION_TYPE, OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, OPEDIS_DATE_UTC_OFFSET, NULL, NULL, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, OPEDIS_PERC_VAT1, OPEDIS_PERC_VAT2, 
                         OPEDIS_PARTIAL_VAT1, OPEDIS_PERC_FEE, OPEDIS_PERC_FEE_TOPPED, OPEDIS_PARTIAL_PERC_FEE, OPEDIS_FIXED_FEE, OPEDIS_PARTIAL_FIXED_FEE, ISNULL(OPEDIS_TOTAL_AMOUNT, OPEDIS_AMOUNT), 
                         CASE WHEN ISNULL(OPEDIS_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(OPEDIS_PERC_FEE_TOPPED, 0) < (OPEDIS_AMOUNT * ISNULL(OPEDIS_PERC_FEE, 0)) 
                         THEN OPEDIS_PERC_FEE_TOPPED ELSE (OPEDIS_AMOUNT * ISNULL(OPEDIS_PERC_FEE, 0)) END + ISNULL(OPEDIS_FIXED_FEE, 0), 0, ISNULL(OPEDIS_PARTIAL_VAT1, 0) 
                         + ((CASE WHEN ISNULL(OPEDIS_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(OPEDIS_PERC_FEE_TOPPED, 0) < (OPEDIS_AMOUNT * ISNULL(OPEDIS_PERC_FEE, 0)) 
                         THEN OPEDIS_PERC_FEE_TOPPED ELSE (OPEDIS_AMOUNT * ISNULL(OPEDIS_PERC_FEE, 0)) END) * ISNULL(OPEDIS_PERC_VAT2, 0)) + (ISNULL(OPEDIS_FIXED_FEE, 0) * ISNULL(OPEDIS_PERC_VAT2, 0)), NULL, NULL, NULL 
                         OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 
                         0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 OPEDIS_ID
FROM            dbo.OPERATIONS_DISCOUNTS INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPEDIS_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPEDIS_BALANCE_CUR_ID = CUR2.CUR_ID
UNION ALL
SELECT        EPO_TYPE, USRP_USR_ID, EPO_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, EPO_DATE, EPO_DATE_UTC, EPO_INIDATE, EPO_ENDDATE, NULL, - EPO_AMOUNT, - EPO_AMOUNT, EPO_TIME, INS_CUR_ID, CUR_ISO_CODE, 
                         cast(CUR_MINOR_UNIT AS int), - EPO_AMOUNT, NULL, NULL, NULL, 1, USRP_ID, USRP_PLATE, NULL, NULL, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, GRP_DESCRIPTION, TAR_DESCRIPTION, NULL, NULL, 
                         EPO_INSERTION_UTC_DATE, EPO_DATE_UTC_OFFSET, EPO_INIDATE_UTC_OFFSET, EPO_ENDDATE_UTC_OFFSET, EPO_SRCTYPE, EPO_SRCIDENT, 0, 0, 0, 0, 0, 0, 0, 0, - EPO_AMOUNT, 0, 0, 0, NULL, NULL, NULL 
                         OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 
                         0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 EPO_ID
FROM            dbo.EXTERNAL_PARKING_OPERATIONS LEFT JOIN
                         GROUPS ON EPO_ZONE = GRP_ID LEFT JOIN
                         TARIFFS ON EPO_TARIFF = TAR_ID, dbo.USER_PLATES, dbo.INSTALLATIONS, dbo.CURRENCIES
WHERE        EPO_PLATE = USRP_PLATE AND USRP_ENABLED = 1 AND EPO_INS_ID = INS_ID AND INS_CUR_ID = CUR_ID
UNION ALL
SELECT        OPEOFF_TYPE, OPEOFF_USR_ID, OPEOFF_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, CASE OPEOFF_TYPE WHEN 8 THEN OPEOFF_NOTIFY_ENTRY_DATE ELSE OPEOFF_PAYMENT_DATE END, 
                         CASE OPEOFF_TYPE WHEN 8 THEN OPEOFF_UTC_NOTIFY_ENTRY_DATE ELSE OPEOFF_UTC_PAYMENT_DATE END, OPEOFF_ENTRY_DATE, OPEOFF_END_DATE, NULL, - OPEOFF_AMOUNT, - OPEOFF_AMOUNT, 
                         OPEOFF_TIME, CAST(OPEOFF_AMOUNT_CUR_ID AS int), CUR1.CUR_ISO_CODE, cast(CUR1.CUR_MINOR_UNIT AS int), CAST(- OPEOFF_FINAL_AMOUNT AS int), OPEOFF_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE, 
                         cast(CUR2.CUR_MINOR_UNIT AS int) OPE_BALANCE_CUR_MINOR_UNIT, OPEOFF_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL, NULL, NULL, GRP_ID, NULL, GRP_DESCRIPTION, OPEOFF_TARIFF, 
                         OPEOFF_SUSCRIPTION_TYPE, OPEOFF_BALANCE_BEFORE, OPEOFF_INSERTION_UTC_DATE, 
                         CASE OPEOFF_TYPE WHEN 8 THEN OPEOFF_NOTIFY_ENTRY_DATE_UTC_OFFSET ELSE OPEOFF_PAYMENT_DATE_UTC_OFFSET END, OPEOFF_ENTRY_DATE_UTC_OFFSET, OPEOFF_END_DATE_UTC_OFFSET, 1, NULL, 0, 
                         0, 0, 0, 0, 0, 0, 0, - OPEOFF_AMOUNT, 0, 0, 0, NULL, NULL, NULL OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL OPE_TIME_BALANCE_BEFORE, NULL 
                         OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 OPEOFF_ID
FROM            dbo.OPERATIONS_OFFSTREET LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.OPERATIONS_OFFSTREET.OPEOFF_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPEOFF_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPEOFF_BALANCE_CUR_ID = CUR2.CUR_ID LEFT OUTER JOIN
                         dbo.GROUPS ON OPEOFF_GRP_ID = GRP_ID, INSTALLATIONS
WHERE        OPEOFF_INS_ID = INS_ID
UNION ALL
SELECT        14, BAT_SRC_USR_ID, NULL, NULL, NULL, BAT_DATE, BAT_UTC_DATE, NULL, NULL, NULL, - BAT_AMOUNT, - BAT_AMOUNT, NULL, BAT_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE, cast(CUR1.CUR_MINOR_UNIT AS int), 
                         - BAT_AMOUNT, BAT_DST_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE, cast(CUR2.CUR_MINOR_UNIT AS int), BAT_CHANGE_APPLIED, NULL, NULL, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL 
                         SECH_SECHT_ID, NULL, NULL, NULL, NULL, NULL, BAT_SRC_BALANCE_BEFORE, BAT_INSERTION_UTC_DATE, BAT_DATE_UTC_OFFSET, NULL, NULL, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, NULL, NULL, NULL 
                         OPE_PARTIAL_VAT1, NULL, NULL, NULL OPE_PARTIAL_PERC_FEE, NULL, NULL OPE_PARTIAL_FIXED_FEE, - BAT_AMOUNT, 0 OPE_FEE, 0 OPE_BONUS, 0 OPE_VAT, BAT_DST_USR_ID, USR_USERNAME, NULL 
                         OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, BAT_SHOPKEEPER_OP, 
                         BAT_SHOPKEEPER_COLLECTED_AMOUNT, BAT_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 BAT_ID
FROM            dbo.BALANCE_TRANSFERS INNER JOIN
                         dbo.CURRENCIES CUR1 ON BAT_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON BAT_DST_BALANCE_CUR_ID = CUR2.CUR_ID INNER JOIN
                         dbo.USERS ON BAT_DST_USR_ID = USR_ID
UNION ALL
SELECT        15, BAT_DST_USR_ID, NULL, NULL, NULL, BAT_DATE, BAT_UTC_DATE, NULL, NULL, NULL, BAT_DST_AMOUNT, BAT_DST_AMOUNT, NULL, BAT_DST_BALANCE_CUR_ID, CUR_ISO_CODE, 
                         cast(CUR_MINOR_UNIT AS int), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, BAT_DST_BALANCE_BEFORE, BAT_INSERTION_UTC_DATE, 
                         BAT_DATE_UTC_OFFSET, NULL, NULL, 1 EPO_SRCTYPE, NULL EPO_SRCIDENT, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, BAT_DST_AMOUNT, 0, 0, 0, BAT_SRC_USR_ID, USR_USERNAME, NULL 
                         OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, BAT_SHOPKEEPER_OP, 
                         BAT_SHOPKEEPER_COLLECTED_AMOUNT, BAT_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 BAT_ID
FROM            dbo.BALANCE_TRANSFERS, dbo.CURRENCIES, dbo.USERS
WHERE        BAT_DST_BALANCE_CUR_ID = CUR_ID AND BAT_SRC_USR_ID = USR_ID
UNION ALL
SELECT        TOLM_TYPE, TOLM_USR_ID, TOLM_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, TOLM_DATE, TOLM_UTC_DATE, NULL, NULL, NULL, 
                         CAST(CASE WHEN TOLM_TYPE = 18 THEN TOLM_AMOUNT ELSE - TOLM_AMOUNT END AS int) OPE_AMOUNT, CAST(CASE WHEN TOLM_TYPE = 2 THEN TOLM_AMOUNT ELSE - TOLM_AMOUNT END AS int) 
                         OPE_REAL_AMOUNT, /*CAST(  -TOLM_AMOUNT AS int) OPE_AMOUNT,*/ NULL, TOLM_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, cast(CUR1.CUR_MINOR_UNIT AS int) 
                         OPE_AMOUNT_CUR_MINOR_UNIT, CAST(CASE WHEN TOLM_TYPE = 18 THEN TOLM_FINAL_AMOUNT ELSE - TOLM_FINAL_AMOUNT END AS int) OPE_FINAL_AMOUNT, TOLM_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, cast(CUR2.CUR_MINOR_UNIT AS int) OPE_BALANCE_CUR_MINOR_UNIT, TOLM_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL 
                         TIPA_TICKET_DATA, NULL SECH_SECHT_ID, TOLM_TOL_ID, NULL, TOL_DESCRIPTION, TOLM_TOL_TARIFF, NULL, TOLM_BALANCE_BEFORE, TOLM_INSERTION_UTC_DATE, TOLM_DATE_UTC_OFFSET, NULL, NULL, 
                         1 EPO_SRCTYPE, NULL EPO_SRCIDENT, TOLM_PERC_VAT1, TOLM_PERC_VAT2, CAST(CASE WHEN TOLM_TYPE = 18 THEN TOLM_PARTIAL_VAT1 ELSE - TOLM_PARTIAL_VAT1 END AS int) OPE_PARTIAL_VAT1, 
                         TOLM_PERC_FEE, TOLM_PERC_FEE_TOPPED, CAST(CASE WHEN TOLM_TYPE = 18 THEN TOLM_PARTIAL_PERC_FEE ELSE - TOLM_PARTIAL_PERC_FEE END AS int) OPE_PARTIAL_PERC_FEE, TOLM_FIXED_FEE, 
                         CAST(CASE WHEN TOLM_TYPE = 18 THEN TOLM_PARTIAL_FIXED_FEE ELSE - TOLM_PARTIAL_FIXED_FEE END AS int) OPE_PARTIAL_FIXED_FEE, CAST(CASE WHEN TOLM_TYPE = 18 THEN ISNULL(TOLM_TOTAL_AMOUNT, 
                         TOLM_AMOUNT) ELSE - ISNULL(TOLM_TOTAL_AMOUNT, TOLM_AMOUNT) END AS int) OPE_TOTAL_AMOUNT, CASE WHEN ISNULL(TOLM_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(TOLM_PERC_FEE_TOPPED, 0) 
                         < (TOLM_AMOUNT * ISNULL(TOLM_PERC_FEE, 0)) THEN TOLM_PERC_FEE_TOPPED ELSE (TOLM_AMOUNT * ISNULL(TOLM_PERC_FEE, 0)) END + ISNULL(TOLM_FIXED_FEE, 0) OPE_FEE, 0 OPE_BONUS, 
                         ISNULL(TOLM_PARTIAL_VAT1, 0) + ((CASE WHEN ISNULL(TOLM_PERC_FEE_TOPPED, 0) > 0 AND ISNULL(TOLM_PERC_FEE_TOPPED, 0) < (TOLM_AMOUNT * ISNULL(TOLM_PERC_FEE, 0)) 
                         THEN TOLM_PERC_FEE_TOPPED ELSE (TOLM_AMOUNT * ISNULL(TOLM_PERC_FEE, 0)) END) * ISNULL(TOLM_PERC_VAT2, 0)) + (ISNULL(TOLM_FIXED_FEE, 0) * ISNULL(TOLM_PERC_VAT2, 0)) OPE_VAT, NULL 
                         OPE_ADDITIONAL_USR_ID, NULL OPE_ADDITIONAL_USR_USERNAME, TOLM_TYPE OPE_CUSPMR_TYPE, NULL OPE_CUSPMR_PAGATELIA_NEW_BALANCE, NULL OPE_TIME_BALANCE_USED, NULL 
                         OPE_TIME_BALANCE_BEFORE, NULL OPE_POSTPAY, 0 OPE_SHOPKEEPER_OP, 0 OPE_SHOPKEEPER_AMOUNT, 0 OPE_SHOPKEEPER_PROFIT, 0 TAR_TYPE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
						 NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
						 TOLM_ID
FROM            dbo.TOLL_MOVEMENTS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.TOLL_MOVEMENTS.TOLM_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON TOLM_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON TOLM_BALANCE_CUR_ID = CUR2.CUR_ID LEFT OUTER JOIN
                         dbo.TOLLS ON TOLM_TOL_ID = TOL_ID, INSTALLATIONS
WHERE        TOLM_INS_ID = INS_ID
GO


