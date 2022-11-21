SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VW_SHOPKEEPER_USERS]
AS
SELECT        dbo.USERS.USR_ID, dbo.USERS.USR_CUS_ID, dbo.USERS.USR_INSERT_UTC_DATE, dbo.USERS.USR_COU_ID, dbo.USERS.USR_EMAIL, dbo.USERS.USR_MAIN_TEL_COUNTRY, dbo.USERS.USR_MAIN_TEL, 
                         dbo.USERS.USR_SECUND_TEL_COUNTRY, dbo.USERS.USR_SECUND_TEL, dbo.USERS.USR_USERNAME, dbo.USERS.USR_SUSCRIPTION_TYPE, dbo.USERS.USR_CUSPM_ID, dbo.USERS.USR_BALANCE, 
                         dbo.USERS.USR_CUR_ID, dbo.USERS.USR_CULTURE_LANG, dbo.USERS.USR_UTC_OFFSET, dbo.USERS.USR_ENABLED, dbo.USERS.USR_LCT_ID, dbo.USERS.USR_INSERT_MOSE_OS, 
                         dbo.USERS.USR_DISABLE_UTC_DATE, dbo.USERS.USR_OPERATIVE_UTC_DATE, dbo.USERS.USR_PAGATELIA_LAST_USER, dbo.USERS.USR_PAGATELIA_LAST_PWD, dbo.USERS.USR_PAYMETH, 
                         dbo.CUSTOMERS.CUS_ID, dbo.CUSTOMERS.CUS_TYPE, dbo.CUSTOMERS.CUS_INSERT_UTC_DATE, dbo.CUSTOMERS.CUS_USR_ID, dbo.CUSTOMERS.CUS_COU_ID, dbo.CUSTOMERS.CUS_DOC_ID, 
                         dbo.CUSTOMERS.CUS_DOC_ID_TYPE, dbo.CUSTOMERS.CUS_NAME, dbo.CUSTOMERS.CUS_SURNAME1, dbo.CUSTOMERS.CUS_SURNAME2, dbo.CUSTOMERS.CUS_STREET, 
                         dbo.CUSTOMERS.CUS_STREE_NUMBER, dbo.CUSTOMERS.CUS_LEVEL_NUM, dbo.CUSTOMERS.CUS_DOOR, dbo.CUSTOMERS.CUS_LETTER, dbo.CUSTOMERS.CUS_STAIR, dbo.CUSTOMERS.CUS_CITY, 
                         dbo.CUSTOMERS.CUS_STATE, dbo.CUSTOMERS.CUS_ZIPCODE, dbo.CUSTOMERS.CUS_ENABLED, dbo.UserPlates(dbo.USERS.USR_ID) AS Plates, dbo.USERS.USR_PAYMETH AS PaymentMethod, 
                         DATEDIFF(day, ISNULL(dbo.USERS.USR_LAST_BALANCE_USE, '2000-01-01'), GETUTCDATE()) AS DaysWithoutOps, dbo.USERS.USR_SIGNUP_OS, dbo.USERS.USR_INAA_ID, 
                         dbo.USERS.USR_FIRST_OPERATION_INS_ID, dbo.USERS.USR_LAST_OPERATION_INS_ID,dbo.USERS.USR_SHOPKEEPER_STATUS
FROM            dbo.USERS INNER JOIN
                         dbo.CUSTOMERS ON dbo.USERS.USR_CUS_ID = dbo.CUSTOMERS.CUS_ID
WHERE			USR_SHOPKEEPER_STATUS in (1,2)
AND dbo.USERS.USR_FIRST_OPERATION_INS_ID NOT IN (SELECT INS_ID FROM INSTALLATIONS INNER JOIN COUNTRIES_REDIRECTIONS ON INS_COU_ID = COURE_COU_ID)
AND dbo.USERS.USR_LAST_OPERATION_INS_ID NOT IN (SELECT INS_ID FROM INSTALLATIONS INNER JOIN COUNTRIES_REDIRECTIONS ON INS_COU_ID = COURE_COU_ID)

GO