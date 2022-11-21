BEGIN TRANSACTION
GO

UPDATE OPERATIONS SET OPE_SOAPP_ID=1
GO
UPDATE HIS_OPERATIONS SET OPE_SOAPP_ID=1
GO
UPDATE TICKET_PAYMENTS SET TIPA_SOAPP_ID=1
GO
UPDATE CUSTOMER_PAYMENT_MEANS_RECHARGES SET CUSPMR_SOAPP_ID=1
GO
UPDATE SERVICE_CHARGES SET SECH_SOAPP_ID=1
GO
UPDATE OPERATIONS_DISCOUNTS SET OPEDIS_SOAPP_ID=1
GO
UPDATE TOLL_MOVEMENTS SET TOLM_SOAPP_ID=1
GO
UPDATE OPERATIONS_OFFSTREET SET OPEOFF_SOAPP_ID=1
GO
UPDATE BALANCE_TRANSFERS SET BAT_SRC_SOAPP_ID=1, BAT_DST_SOAPP_ID=1
GO
UPDATE USERS SET USR_SIGNUP_SOAPP_ID=1
GO


COMMIT;