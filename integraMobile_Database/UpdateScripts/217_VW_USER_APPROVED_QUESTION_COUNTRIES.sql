/****** Object:  View [dbo].[VW_USER_APPROVED_QUESTION_COUNTRIES]    Script Date: 04/02/2022 10:55:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[VW_USER_APPROVED_QUESTION_COUNTRIES]
AS
SELECT			UAQC_ID, UAQC_COU_ID, UAQC_UAQ_ID, convert(varchar, UAQC_COU_ID) AS COU_STRING
FROM            dbo.USER_APPROVED_QUESTION_COUNTRIES


GO


