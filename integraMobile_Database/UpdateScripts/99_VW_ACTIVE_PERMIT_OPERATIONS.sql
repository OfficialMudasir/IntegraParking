/****** Object:  View [dbo].[VW_ACTIVE_PERMIT_OPERATIONS]    Script Date: 5/3/2019 3:51:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VW_ACTIVE_PERMIT_OPERATIONS] AS
		SELECT o1.*, c.CUR_ISO_CODE AS OPE_AMOUNT_CUR_ISO_CODE, c.CUR_MINOR_UNIT AS OPE_AMOUNT_CUR_MINOR_UNIT, t.TAR_DESCRIPTION, p1.USRP_PLATE, p1.USRP_ID, g.GRP_DESCRIPTION, u.USR_USERNAME, i.INS_STANDARD_CITY_ID, i.INS_DESCRIPTION,
			p2.USRP_PLATE AS USRP_PLATE2, p3.USRP_PLATE AS USRP_PLATE3, p4.USRP_PLATE AS USRP_PLATE4, p5.USRP_PLATE AS USRP_PLATE5, p6.USRP_PLATE AS USRP_PLATE6, p7.USRP_PLATE AS USRP_PLATE7, p8.USRP_PLATE AS USRP_PLATE8, p9.USRP_PLATE AS USRP_PLATE9, p10.USRP_PLATE AS USRP_PLATE10, g.GRP_PERMIT_MAX_MONTHS
	FROM   operations o1
			LEFT JOIN user_plates p2 ON p2.usrp_id = o1.ope_plate2_usrp_id
			LEFT JOIN user_plates p3 ON p3.usrp_id = o1.ope_plate3_usrp_id
			LEFT JOIN user_plates p4 ON p4.usrp_id = o1.ope_plate4_usrp_id
			LEFT JOIN user_plates p5 ON p5.usrp_id = o1.ope_plate5_usrp_id
			LEFT JOIN user_plates p6 ON p6.usrp_id = o1.ope_plate6_usrp_id
			LEFT JOIN user_plates p7 ON p7.usrp_id = o1.ope_plate7_usrp_id
			LEFT JOIN user_plates p8 ON p8.usrp_id = o1.ope_plate8_usrp_id
			LEFT JOIN user_plates p9 ON p9.usrp_id = o1.ope_plate9_usrp_id
			LEFT JOIN user_plates p10 ON p10.usrp_id = o1.ope_plate10_usrp_id, 
		   tariffs t, 
		   user_plates p1,
		   currencies c,
		   groups g,
		   USERS u,
		   INSTALLATIONS i
	WHERE  ( OPE_UTC_ENDDATE >= GETUTCDATE()
			 OR
			 OPE_PERMIT_EXPIRATION_UTC_TIME >= GETUTCDATE() AND 
					   (SELECT Count(*) 
						FROM   operations o2, TARIFFS t2
						WHERE  o2.ope_tar_id = t2.tar_id
							   AND o2.ope_tar_id = o1.ope_tar_id
							   AND t2.tar_type = 1 
							   AND o2.OPE_INS_ID = o1.ope_ins_id
							   AND o2.ope_grp_id = o1.ope_grp_id							   
							   AND o2.ope_inidate between o1.ope_enddate and o2.ope_permit_expiration_time
							   AND dbo.OperationPlatesString(o2.OPE_ID) = dbo.OperationPlatesString(o1.OPE_ID)) = 0)
		   AND o1.ope_tar_id = t.tar_id 
		   AND t.tar_type = 1 
		   AND p1.usrp_id = o1.ope_usrp_id 
		   AND c.CUR_ID = o1.OPE_AMOUNT_CUR_ID
		   AND g.GRP_ID = o1.OPE_GRP_ID
		   AND u.USR_ID = o1.OPE_USR_ID
		   AND i.INS_ID = o1.OPE_INS_ID






GO


