/****** Object:  View [dbo].[VW_ACTIVE_PERMIT_OPERATIONS]    Script Date: 12/11/2018 10:27:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VW_ACTIVE_PERMIT_OPERATIONS] AS
	SELECT o1.*, c.CUR_ISO_CODE AS OPE_AMOUNT_CUR_ISO_CODE, c.CUR_MINOR_UNIT AS OPE_AMOUNT_CUR_MINOR_UNIT, t.TAR_DESCRIPTION, p1.USRP_PLATE, p1.USRP_ID, g.GRP_DESCRIPTION, u.USR_USERNAME, i.INS_STANDARD_CITY_ID, i.INS_DESCRIPTION
	FROM   operations o1, 
		   tariffs t, 
		   user_plates p1,
		   currencies c,
		   groups g,
		   USERS u,
		   INSTALLATIONS i 
	WHERE  ( ope_utc_inidate > Getutcdate() 
			  OR ( Getutcdate() BETWEEN 
				   ope_utc_inidate AND ope_permit_expiration_utc_time 
				   AND (SELECT Count(*) 
						FROM   operations o2, 
							   user_plates p2 
						WHERE  o2.ope_tar_id = o1.ope_tar_id 
							   AND o2.ope_grp_id = o1.ope_grp_id
							   AND p2.usrp_id = o2.ope_usrp_id 
							   AND p1.usrp_plate = p2.usrp_plate 
							   AND o2.ope_date > o1.ope_date) = 0 ) ) 
		   AND o1.ope_tar_id = t.tar_id 
		   AND t.tar_type = 1 
		   AND p1.usrp_id = o1.ope_usrp_id 
		   AND c.CUR_ID = o1.OPE_AMOUNT_CUR_ID
		   AND g.GRP_ID = o1.OPE_GRP_ID
		   AND u.USR_ID = o1.OPE_USR_ID
		   AND i.INS_ID = o1.OPE_INS_ID

GO


