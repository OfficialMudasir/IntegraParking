CREATE VIEW [dbo].[VW_START_STOP_OPERATIONS]
AS

SELECT
	 OPE_TYPE, OPE_PARKING_MODE_STATUS, OPE_USR_ID, OPE_MOSE_OS, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, 
	 OPE_DATE, OPE_INIDATE, OPE_ENDDATE, OPE_UTC_DATE, OPE_UTC_INIDATE, OPE_UTC_ENDDATE, OPE_DATE_UTC_OFFSET, OPE_INIDATE_UTC_OFFSET, OPE_ENDDATE_UTC_OFFSET,
     OPE_AMOUNT,
	 ISNULL(OPE_REAL_AMOUNT, OPE_AMOUNT) OPE_REAL_AMOUNT,
	 OPE_FINAL_AMOUNT,
	 ISNULL(OPE_TOTAL_AMOUNT, OPE_AMOUNT) OPE_TOTAL_AMOUNT,
	 OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
     OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, 
     OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, ISNULL(GRP_DESCRIPTION2,GRP_DESCRIPTION) GRP_DESCRIPTION, TAR_DESCRIPTION, 
     OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE, OPE_CUSPMR_ID, OPE_OPEDIS_ID, CUSPMR_DATE, CUSPMR_AMOUNT, CUSPMR_CUR_ID, 
     CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE, CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE, OPEDIS_DATE, OPEDIS_AMOUNT, OPEDIS_AMOUNT_CUR_ID, 
     CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, 
     OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, NULL CUSPMR_OP_REFERENCE, NULL CUSPMR_TRANSACTION_ID, NULL CUSPMR_AUTH_CODE, NULL CUSPMR_CARD_HASH, NULL 
     CUSPMR_CARD_REFERENCE, NULL CUSPMR_CARD_SCHEME, NULL CUSPMR_MASKED_CARD_NUMBER, NULL CUSPMR_CARD_EXPIRATION_DATE, CUSPMR_DATE_UTC_OFFSET OPE_CUSPMR_DATE_UTC_OFFSET, OPEDIS_DATE_UTC_OFFSET OPE_OPEDIS_DATE_UTC_OFFSET, USR_USERNAME, 
     CUR1.CUR_NAME OPE_AMOUNT_CUR_NAME, CUR2.CUR_NAME OPE_BALANCE_CUR_NAME, CUR3.CUR_NAME CUSPMR_CUR_NAME, CUR4.CUR_NAME OPEDIS_AMOUNT_CUR_NAME, 
     CUR5.CUR_NAME OPEDIS_BALANCE_CUR_NAME, OPE_EXTERNAL_ID1, OPE_EXTERNAL_ID2, OPE_EXTERNAL_ID3, OPE_EXTERNAL_BASE_ID1, OPE_EXTERNAL_BASE_ID2, OPE_EXTERNAL_BASE_ID3,
	 OPE_LATITUDE, OPE_LONGITUDE, OPE_ID,OPE_APP_VERSION,					 
	 USR_MAIN_TEL,	 
	 INS_STANDARD_CITY_ID

FROM OPERATIONS
		LEFT OUTER JOIN dbo.USER_PLATES ON OPE_USRP_ID = USER_PLATES.USRP_ID 
		INNER JOIN CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID 
		INNER JOIN CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID 
		LEFT OUTER JOIN dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON OPE_CUSPMR_ID = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID 
		LEFT OUTER JOIN dbo.OPERATIONS_DISCOUNTS ON OPE_OPEDIS_ID = OPERATIONS_DISCOUNTS.OPEDIS_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID 
		INNER JOIN dbo.USERS ON OPE_USR_ID = USR_ID 
		LEFT OUTER JOIN dbo.GROUPS ON OPE_GRP_ID = GRP_ID 
		LEFT OUTER JOIN dbo.TARIFFS ON OPE_TAR_ID = TAR_ID
		INNER JOIN INSTALLATIONS ON OPE_INS_ID = INS_ID
WHERE OPE_PARKING_MODE = 1 AND OPE_PARKING_MODE_STATUS = 2

UNION ALL

SELECT 
	 opStop.OPE_TYPE, opStop.OPE_PARKING_MODE_STATUS, opStop.OPE_USR_ID, opStop.OPE_MOSE_OS, opStop.OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, 
	 opStop.OPE_DATE, opStop.OPE_INIDATE, opStop.OPE_ENDDATE, opStop.OPE_UTC_DATE, opStop.OPE_UTC_INIDATE, opStop.OPE_UTC_ENDDATE, opStop.OPE_DATE_UTC_OFFSET, opStop.OPE_INIDATE_UTC_OFFSET, opStop.OPE_ENDDATE_UTC_OFFSET,
	 opStart.OPE_AMOUNT - opStop.OPE_AMOUNT OPE_AMOUNT,
	 ISNULL(opStart.OPE_REAL_AMOUNT, opStart.OPE_AMOUNT) - ISNULL(opStop.OPE_REAL_AMOUNT, opStop.OPE_AMOUNT) OPE_REAL_AMOUNT,
	 opStart.OPE_FINAL_AMOUNT - opStop.OPE_FINAL_AMOUNT OPE_FINAL_AMOUNT,
	 ISNULL(opStart.OPE_TOTAL_AMOUNT, opStart.OPE_AMOUNT) - ISNULL(opStop.OPE_TOTAL_AMOUNT, opStop.OPE_AMOUNT) OPE_TOTAL_AMOUNT,
	 opStart.OPE_TIME - opStop.OPE_TIME OPE_TIME, 
	 opStop.OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
     opStop.OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, 
     opStop.OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, ISNULL(GRP_DESCRIPTION2,GRP_DESCRIPTION) GRP_DESCRIPTION, TAR_DESCRIPTION, 
     opStop.OPE_SUSCRIPTION_TYPE, opStop.OPE_BALANCE_BEFORE, opStop.OPE_INSERTION_UTC_DATE, opStop.OPE_CUSPMR_ID, opStop.OPE_OPEDIS_ID, CUSPMR_DATE, CUSPMR_AMOUNT, CUSPMR_CUR_ID, 
     CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE, CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE, OPEDIS_DATE, OPEDIS_AMOUNT, OPEDIS_AMOUNT_CUR_ID, 
     CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, 
     OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, NULL CUSPMR_OP_REFERENCE, NULL CUSPMR_TRANSACTION_ID, NULL CUSPMR_AUTH_CODE, NULL CUSPMR_CARD_HASH, NULL 
     CUSPMR_CARD_REFERENCE, NULL CUSPMR_CARD_SCHEME, NULL CUSPMR_MASKED_CARD_NUMBER, NULL CUSPMR_CARD_EXPIRATION_DATE, CUSPMR_DATE_UTC_OFFSET OPE_CUSPMR_DATE_UTC_OFFSET, OPEDIS_DATE_UTC_OFFSET OPE_OPEDIS_DATE_UTC_OFFSET, USR_USERNAME, 
     CUR1.CUR_NAME OPE_AMOUNT_CUR_NAME, CUR2.CUR_NAME OPE_BALANCE_CUR_NAME, CUR3.CUR_NAME CUSPMR_CUR_NAME, CUR4.CUR_NAME OPEDIS_AMOUNT_CUR_NAME, 
     CUR5.CUR_NAME OPEDIS_BALANCE_CUR_NAME, opStop.OPE_EXTERNAL_ID1, opStop.OPE_EXTERNAL_ID2, opStop.OPE_EXTERNAL_ID3, opStop.OPE_EXTERNAL_BASE_ID1, opStop.OPE_EXTERNAL_BASE_ID2, opStop.OPE_EXTERNAL_BASE_ID3,
	 opStop.OPE_LATITUDE, opStop.OPE_LONGITUDE, opStop.OPE_ID, opStop.OPE_APP_VERSION,					 
	 USR_MAIN_TEL,	 
	 INS_STANDARD_CITY_ID

 FROM OPERATIONS opStart
		INNER JOIN OPERATIONS opStop ON opStart.OPE_EXTERNAL_BASE_ID1 = opStop.OPE_EXTERNAL_BASE_ID1
		LEFT OUTER JOIN dbo.USER_PLATES ON opStop.OPE_USRP_ID = USER_PLATES.USRP_ID 
		INNER JOIN CURRENCIES CUR1 ON opStop.OPE_AMOUNT_CUR_ID = CUR1.CUR_ID 
		INNER JOIN CURRENCIES CUR2 ON opStop.OPE_BALANCE_CUR_ID = CUR2.CUR_ID 
		LEFT OUTER JOIN dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON opStop.OPE_CUSPMR_ID = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID 
		LEFT OUTER JOIN dbo.OPERATIONS_DISCOUNTS ON opStop.OPE_OPEDIS_ID = OPERATIONS_DISCOUNTS.OPEDIS_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID 
		INNER JOIN dbo.USERS ON opStop.OPE_USR_ID = USR_ID 
		LEFT OUTER JOIN dbo.GROUPS ON opStop.OPE_GRP_ID = GRP_ID 
		LEFT OUTER JOIN dbo.TARIFFS ON opStop.OPE_TAR_ID = TAR_ID
		INNER JOIN INSTALLATIONS ON opStop.OPE_INS_ID = INS_ID
WHERE  opStart.OPE_PARKING_MODE = 1 AND opStart.OPE_PARKING_MODE_STATUS = 1 AND opStart.OPE_TYPE = 1 AND
       opStop.OPE_PARKING_MODE = 1 AND opStop.OPE_PARKING_MODE_STATUS = 1 AND opStop.OPE_TYPE = 3
       
UNION ALL

SELECT

	 OPE_TYPE, OPE_PARKING_MODE_STATUS, OPE_USR_ID, OPE_MOSE_OS, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, 
	 OPE_DATE, OPE_INIDATE, NULL OPE_ENDDATE, OPE_UTC_DATE, OPE_UTC_INIDATE, NULL OPE_UTC_ENDDATE, OPE_DATE_UTC_OFFSET, OPE_INIDATE_UTC_OFFSET, NULL OPE_ENDDATE_UTC_OFFSET,
     OPE_AMOUNT,
	 ISNULL(OPE_REAL_AMOUNT, OPE_AMOUNT) OPE_REAL_AMOUNT,
	 OPE_FINAL_AMOUNT,
	 ISNULL(OPE_TOTAL_AMOUNT, OPE_AMOUNT) OPE_TOTAL_AMOUNT,
	 OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
     OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, 
     OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, ISNULL(GRP_DESCRIPTION2,GRP_DESCRIPTION) GRP_DESCRIPTION, TAR_DESCRIPTION, 
     OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE, OPE_CUSPMR_ID, OPE_OPEDIS_ID, CUSPMR_DATE, CUSPMR_AMOUNT, CUSPMR_CUR_ID, 
     CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE, CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE, OPEDIS_DATE, OPEDIS_AMOUNT, OPEDIS_AMOUNT_CUR_ID, 
     CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, 
     OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, NULL CUSPMR_OP_REFERENCE, NULL CUSPMR_TRANSACTION_ID, NULL CUSPMR_AUTH_CODE, NULL CUSPMR_CARD_HASH, NULL 
     CUSPMR_CARD_REFERENCE, NULL CUSPMR_CARD_SCHEME, NULL CUSPMR_MASKED_CARD_NUMBER, NULL CUSPMR_CARD_EXPIRATION_DATE, CUSPMR_DATE_UTC_OFFSET OPE_CUSPMR_DATE_UTC_OFFSET, OPEDIS_DATE_UTC_OFFSET OPE_OPEDIS_DATE_UTC_OFFSET, USR_USERNAME, 
     CUR1.CUR_NAME OPE_AMOUNT_CUR_NAME, CUR2.CUR_NAME OPE_BALANCE_CUR_NAME, CUR3.CUR_NAME CUSPMR_CUR_NAME, CUR4.CUR_NAME OPEDIS_AMOUNT_CUR_NAME, 
     CUR5.CUR_NAME OPEDIS_BALANCE_CUR_NAME, OPE_EXTERNAL_ID1, OPE_EXTERNAL_ID2, OPE_EXTERNAL_ID3, OPE_EXTERNAL_BASE_ID1, OPE_EXTERNAL_BASE_ID2, OPE_EXTERNAL_BASE_ID3,	 
	 OPE_LATITUDE, OPE_LONGITUDE, OPE_ID,OPE_APP_VERSION,					 
	 USR_MAIN_TEL,	 
	 INS_STANDARD_CITY_ID

FROM OPERATIONS
		LEFT OUTER JOIN dbo.USER_PLATES ON OPE_USRP_ID = USER_PLATES.USRP_ID 
		INNER JOIN CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID 
		INNER JOIN CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID 
		LEFT OUTER JOIN dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON OPE_CUSPMR_ID = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID 
		LEFT OUTER JOIN dbo.OPERATIONS_DISCOUNTS ON OPE_OPEDIS_ID = OPERATIONS_DISCOUNTS.OPEDIS_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID 
		INNER JOIN dbo.USERS ON OPE_USR_ID = USR_ID 
		LEFT OUTER JOIN dbo.GROUPS ON OPE_GRP_ID = GRP_ID 
		LEFT OUTER JOIN dbo.TARIFFS ON OPE_TAR_ID = TAR_ID
		INNER JOIN INSTALLATIONS ON OPE_INS_ID = INS_ID
WHERE OPE_PARKING_MODE = 1 AND OPE_PARKING_MODE_STATUS = 0




GO


CREATE VIEW [dbo].[VW_START_STOP_HIS_OPERATIONS]
AS

SELECT
	 OPE_TYPE, OPE_PARKING_MODE_STATUS, OPE_USR_ID, OPE_MOSE_OS, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, 
	 OPE_DATE, OPE_INIDATE, OPE_ENDDATE, OPE_UTC_DATE, OPE_UTC_INIDATE, OPE_UTC_ENDDATE, OPE_DATE_UTC_OFFSET, OPE_INIDATE_UTC_OFFSET, OPE_ENDDATE_UTC_OFFSET,
     OPE_AMOUNT,
	 ISNULL(OPE_REAL_AMOUNT, OPE_AMOUNT) OPE_REAL_AMOUNT,
	 OPE_FINAL_AMOUNT,
	 ISNULL(OPE_TOTAL_AMOUNT, OPE_AMOUNT) OPE_TOTAL_AMOUNT,
	 OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
     OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, 
     OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, ISNULL(GRP_DESCRIPTION2,GRP_DESCRIPTION) GRP_DESCRIPTION, TAR_DESCRIPTION, 
     OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE, OPE_CUSPMR_ID, OPE_OPEDIS_ID, CUSPMR_DATE, CUSPMR_AMOUNT, CUSPMR_CUR_ID, 
     CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE, CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE, OPEDIS_DATE, OPEDIS_AMOUNT, OPEDIS_AMOUNT_CUR_ID, 
     CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, 
     OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, NULL CUSPMR_OP_REFERENCE, NULL CUSPMR_TRANSACTION_ID, NULL CUSPMR_AUTH_CODE, NULL CUSPMR_CARD_HASH, NULL 
     CUSPMR_CARD_REFERENCE, NULL CUSPMR_CARD_SCHEME, NULL CUSPMR_MASKED_CARD_NUMBER, NULL CUSPMR_CARD_EXPIRATION_DATE, CUSPMR_DATE_UTC_OFFSET OPE_CUSPMR_DATE_UTC_OFFSET, OPEDIS_DATE_UTC_OFFSET OPE_OPEDIS_DATE_UTC_OFFSET, USR_USERNAME, 
     CUR1.CUR_NAME OPE_AMOUNT_CUR_NAME, CUR2.CUR_NAME OPE_BALANCE_CUR_NAME, CUR3.CUR_NAME CUSPMR_CUR_NAME, CUR4.CUR_NAME OPEDIS_AMOUNT_CUR_NAME, 
     CUR5.CUR_NAME OPEDIS_BALANCE_CUR_NAME, OPE_EXTERNAL_ID1, OPE_EXTERNAL_ID2, OPE_EXTERNAL_ID3, OPE_EXTERNAL_BASE_ID1, OPE_EXTERNAL_BASE_ID2, OPE_EXTERNAL_BASE_ID3,
	 OPE_LATITUDE, OPE_LONGITUDE, OPE_ID,OPE_APP_VERSION,					 
	 USR_MAIN_TEL,	 
	 INS_STANDARD_CITY_ID

FROM HIS_OPERATIONS
		LEFT OUTER JOIN dbo.USER_PLATES ON OPE_USRP_ID = USER_PLATES.USRP_ID 
		INNER JOIN CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID 
		INNER JOIN CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID 
		LEFT OUTER JOIN dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON OPE_CUSPMR_ID = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID 
		LEFT OUTER JOIN dbo.OPERATIONS_DISCOUNTS ON OPE_OPEDIS_ID = OPERATIONS_DISCOUNTS.OPEDIS_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID 
		INNER JOIN dbo.USERS ON OPE_USR_ID = USR_ID 
		LEFT OUTER JOIN dbo.GROUPS ON OPE_GRP_ID = GRP_ID 
		LEFT OUTER JOIN dbo.TARIFFS ON OPE_TAR_ID = TAR_ID
		INNER JOIN INSTALLATIONS ON OPE_INS_ID = INS_ID
WHERE OPE_PARKING_MODE = 1 AND OPE_PARKING_MODE_STATUS = 2

UNION ALL

SELECT 
	 opStop.OPE_TYPE, opStop.OPE_PARKING_MODE_STATUS, opStop.OPE_USR_ID, opStop.OPE_MOSE_OS, opStop.OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, 
	 opStop.OPE_DATE, opStop.OPE_INIDATE, opStop.OPE_ENDDATE, opStop.OPE_UTC_DATE, opStop.OPE_UTC_INIDATE, opStop.OPE_UTC_ENDDATE, opStop.OPE_DATE_UTC_OFFSET, opStop.OPE_INIDATE_UTC_OFFSET, opStop.OPE_ENDDATE_UTC_OFFSET,
	 opStart.OPE_AMOUNT - opStop.OPE_AMOUNT OPE_AMOUNT,
	 ISNULL(opStart.OPE_REAL_AMOUNT, opStart.OPE_AMOUNT) - ISNULL(opStop.OPE_REAL_AMOUNT, opStop.OPE_AMOUNT) OPE_REAL_AMOUNT,
	 opStart.OPE_FINAL_AMOUNT - opStop.OPE_FINAL_AMOUNT OPE_FINAL_AMOUNT,
	 ISNULL(opStart.OPE_TOTAL_AMOUNT, opStart.OPE_AMOUNT) - ISNULL(opStop.OPE_TOTAL_AMOUNT, opStop.OPE_AMOUNT) OPE_TOTAL_AMOUNT,
	 opStart.OPE_TIME - opStop.OPE_TIME OPE_TIME, 
	 opStop.OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
     opStop.OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, 
     opStop.OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, ISNULL(GRP_DESCRIPTION2,GRP_DESCRIPTION) GRP_DESCRIPTION, TAR_DESCRIPTION, 
     opStop.OPE_SUSCRIPTION_TYPE, opStop.OPE_BALANCE_BEFORE, opStop.OPE_INSERTION_UTC_DATE, opStop.OPE_CUSPMR_ID, opStop.OPE_OPEDIS_ID, CUSPMR_DATE, CUSPMR_AMOUNT, CUSPMR_CUR_ID, 
     CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE, CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE, OPEDIS_DATE, OPEDIS_AMOUNT, OPEDIS_AMOUNT_CUR_ID, 
     CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, 
     OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, NULL CUSPMR_OP_REFERENCE, NULL CUSPMR_TRANSACTION_ID, NULL CUSPMR_AUTH_CODE, NULL CUSPMR_CARD_HASH, NULL 
     CUSPMR_CARD_REFERENCE, NULL CUSPMR_CARD_SCHEME, NULL CUSPMR_MASKED_CARD_NUMBER, NULL CUSPMR_CARD_EXPIRATION_DATE, CUSPMR_DATE_UTC_OFFSET OPE_CUSPMR_DATE_UTC_OFFSET, OPEDIS_DATE_UTC_OFFSET OPE_OPEDIS_DATE_UTC_OFFSET, USR_USERNAME, 
     CUR1.CUR_NAME OPE_AMOUNT_CUR_NAME, CUR2.CUR_NAME OPE_BALANCE_CUR_NAME, CUR3.CUR_NAME CUSPMR_CUR_NAME, CUR4.CUR_NAME OPEDIS_AMOUNT_CUR_NAME, 
     CUR5.CUR_NAME OPEDIS_BALANCE_CUR_NAME, opStop.OPE_EXTERNAL_ID1, opStop.OPE_EXTERNAL_ID2, opStop.OPE_EXTERNAL_ID3, opStop.OPE_EXTERNAL_BASE_ID1, opStop.OPE_EXTERNAL_BASE_ID2, opStop.OPE_EXTERNAL_BASE_ID3,
	 opStop.OPE_LATITUDE, opStop.OPE_LONGITUDE, opStop.OPE_ID, opStop.OPE_APP_VERSION,					 
	 USR_MAIN_TEL,	 
	 INS_STANDARD_CITY_ID

 FROM HIS_OPERATIONS opStart
		INNER JOIN HIS_OPERATIONS opStop ON opStart.OPE_EXTERNAL_BASE_ID1 = opStop.OPE_EXTERNAL_BASE_ID1
		LEFT OUTER JOIN dbo.USER_PLATES ON opStop.OPE_USRP_ID = USER_PLATES.USRP_ID 
		INNER JOIN CURRENCIES CUR1 ON opStop.OPE_AMOUNT_CUR_ID = CUR1.CUR_ID 
		INNER JOIN CURRENCIES CUR2 ON opStop.OPE_BALANCE_CUR_ID = CUR2.CUR_ID 
		LEFT OUTER JOIN dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON opStop.OPE_CUSPMR_ID = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID 
		LEFT OUTER JOIN dbo.OPERATIONS_DISCOUNTS ON opStop.OPE_OPEDIS_ID = OPERATIONS_DISCOUNTS.OPEDIS_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID 
		INNER JOIN dbo.USERS ON opStop.OPE_USR_ID = USR_ID 
		LEFT OUTER JOIN dbo.GROUPS ON opStop.OPE_GRP_ID = GRP_ID 
		LEFT OUTER JOIN dbo.TARIFFS ON opStop.OPE_TAR_ID = TAR_ID
		INNER JOIN INSTALLATIONS ON opStop.OPE_INS_ID = INS_ID
WHERE  opStart.OPE_PARKING_MODE = 1 AND opStart.OPE_PARKING_MODE_STATUS = 1 AND opStart.OPE_TYPE = 1 AND
       opStop.OPE_PARKING_MODE = 1 AND opStop.OPE_PARKING_MODE_STATUS = 1 AND opStop.OPE_TYPE = 3
       
UNION ALL

SELECT

	 OPE_TYPE, OPE_PARKING_MODE_STATUS, OPE_USR_ID, OPE_MOSE_OS, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, 
	 OPE_DATE, OPE_INIDATE, NULL OPE_ENDDATE, OPE_UTC_DATE, OPE_UTC_INIDATE, NULL OPE_UTC_ENDDATE, OPE_DATE_UTC_OFFSET, OPE_INIDATE_UTC_OFFSET, NULL OPE_ENDDATE_UTC_OFFSET,
     OPE_AMOUNT,
	 ISNULL(OPE_REAL_AMOUNT, OPE_AMOUNT) OPE_REAL_AMOUNT,
	 OPE_FINAL_AMOUNT,
	 ISNULL(OPE_TOTAL_AMOUNT, OPE_AMOUNT) OPE_TOTAL_AMOUNT,
	 OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, 
     OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, 
     OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, ISNULL(GRP_DESCRIPTION2,GRP_DESCRIPTION) GRP_DESCRIPTION, TAR_DESCRIPTION, 
     OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE, OPE_CUSPMR_ID, OPE_OPEDIS_ID, CUSPMR_DATE, CUSPMR_AMOUNT, CUSPMR_CUR_ID, 
     CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE, CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE, OPEDIS_DATE, OPEDIS_AMOUNT, OPEDIS_AMOUNT_CUR_ID, 
     CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, 
     OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE, NULL CUSPMR_OP_REFERENCE, NULL CUSPMR_TRANSACTION_ID, NULL CUSPMR_AUTH_CODE, NULL CUSPMR_CARD_HASH, NULL 
     CUSPMR_CARD_REFERENCE, NULL CUSPMR_CARD_SCHEME, NULL CUSPMR_MASKED_CARD_NUMBER, NULL CUSPMR_CARD_EXPIRATION_DATE, CUSPMR_DATE_UTC_OFFSET OPE_CUSPMR_DATE_UTC_OFFSET, OPEDIS_DATE_UTC_OFFSET OPE_OPEDIS_DATE_UTC_OFFSET, USR_USERNAME, 
     CUR1.CUR_NAME OPE_AMOUNT_CUR_NAME, CUR2.CUR_NAME OPE_BALANCE_CUR_NAME, CUR3.CUR_NAME CUSPMR_CUR_NAME, CUR4.CUR_NAME OPEDIS_AMOUNT_CUR_NAME, 
     CUR5.CUR_NAME OPEDIS_BALANCE_CUR_NAME, OPE_EXTERNAL_ID1, OPE_EXTERNAL_ID2, OPE_EXTERNAL_ID3, OPE_EXTERNAL_BASE_ID1, OPE_EXTERNAL_BASE_ID2, OPE_EXTERNAL_BASE_ID3,
	 OPE_LATITUDE, OPE_LONGITUDE, OPE_ID,OPE_APP_VERSION,					 
	 USR_MAIN_TEL,	 
	 INS_STANDARD_CITY_ID

FROM HIS_OPERATIONS
		LEFT OUTER JOIN dbo.USER_PLATES ON OPE_USRP_ID = USER_PLATES.USRP_ID 
		INNER JOIN CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID 
		INNER JOIN CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID 
		LEFT OUTER JOIN dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON OPE_CUSPMR_ID = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID 
		LEFT OUTER JOIN dbo.OPERATIONS_DISCOUNTS ON OPE_OPEDIS_ID = OPERATIONS_DISCOUNTS.OPEDIS_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID 
		LEFT OUTER JOIN dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID 
		INNER JOIN dbo.USERS ON OPE_USR_ID = USR_ID 
		LEFT OUTER JOIN dbo.GROUPS ON OPE_GRP_ID = GRP_ID 
		LEFT OUTER JOIN dbo.TARIFFS ON OPE_TAR_ID = TAR_ID
		INNER JOIN INSTALLATIONS ON OPE_INS_ID = INS_ID
WHERE OPE_PARKING_MODE = 1 AND OPE_PARKING_MODE_STATUS = 0





GO


