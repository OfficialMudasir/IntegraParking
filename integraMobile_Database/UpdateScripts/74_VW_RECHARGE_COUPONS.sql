SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[VW_RECHARGE_COUPONS]
AS
	SELECT RECHARGE_COUPONS.*, RTL_NAME, RTL_EMAIL, RTL_ADDRESS, RTL_DOC_ID, u.RCOUPU_USR_ID, u.RCOUPU_DATE
	FROM RECHARGE_COUPONS 
	LEFT JOIN RETAILER_PAYMENTS ON RCOUP_RTLPY_ID = RTLPY_ID
	LEFT JOIN RETAILERS ON RTLPY_RTL_ID = RTL_ID
	LEFT JOIN RECHARGE_COUPONS_USES u ON RCOUP_ID = u.RCOUPU_RCOUP_ID
GO