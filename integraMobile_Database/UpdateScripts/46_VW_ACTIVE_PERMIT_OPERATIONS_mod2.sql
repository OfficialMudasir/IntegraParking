CREATE FUNCTION [dbo].[OperationPlatesString](@OpeId numeric)
RETURNS varchar(1000)
AS
BEGIN
	
	DECLARE @ret varchar(1000),
			@plates varchar(1000);
	
	SET @ret = '';

	SELECT @plates = P1.USRP_PLATE + '*' + ISNULL(P2.USRP_PLATE, '') + '*' + ISNULL(P3.USRP_PLATE, '') + '*' + ISNULL(P4.USRP_PLATE, '') + '*' + ISNULL(P5.USRP_PLATE, '')
					 + '*' + ISNULL(P6.USRP_PLATE, '') + '*' + ISNULL(P7.USRP_PLATE, '') + '*' + ISNULL(P8.USRP_PLATE, '') + '*' + ISNULL(P9.USRP_PLATE, '') + '*' + ISNULL(P10.USRP_PLATE, '')
	FROM OPERATIONS 
			INNER JOIN USER_PLATES P1 ON OPE_USRP_ID = P1.USRP_ID
			LEFT JOIN USER_PLATES P2 ON OPE_PLATE2_USRP_ID = P2.USRP_ID
			LEFT JOIN USER_PLATES P3 ON OPE_PLATE3_USRP_ID = P3.USRP_ID
			LEFT JOIN USER_PLATES P4 ON OPE_PLATE4_USRP_ID = P4.USRP_ID
			LEFT JOIN USER_PLATES P5 ON OPE_PLATE5_USRP_ID = P5.USRP_ID
			LEFT JOIN USER_PLATES P6 ON OPE_PLATE6_USRP_ID = P6.USRP_ID
			LEFT JOIN USER_PLATES P7 ON OPE_PLATE7_USRP_ID = P7.USRP_ID
			LEFT JOIN USER_PLATES P8 ON OPE_PLATE8_USRP_ID = P8.USRP_ID
			LEFT JOIN USER_PLATES P9 ON OPE_PLATE9_USRP_ID = P9.USRP_ID
			LEFT JOIN USER_PLATES P10 ON OPE_PLATE10_USRP_ID = P10.USRP_ID

	SELECT @ret = @ret + '*' + t.Name
	FROM dbo.SplitString(@plates, '*') t
	ORDER BY t.Name

	IF (LEN(@ret) > 0)
		BEGIN
			SET @ret = SUBSTRING(@ret, 1, LEN(@ret)-1)
		END
	

	RETURN @ret;

END

GO



ALTER VIEW [dbo].[VW_ACTIVE_PERMIT_OPERATIONS] AS
		SELECT o1.*, c.CUR_ISO_CODE AS OPE_AMOUNT_CUR_ISO_CODE, c.CUR_MINOR_UNIT AS OPE_AMOUNT_CUR_MINOR_UNIT, t.TAR_DESCRIPTION, p1.USRP_PLATE, p1.USRP_ID, g.GRP_DESCRIPTION, u.USR_USERNAME, i.INS_STANDARD_CITY_ID, i.INS_DESCRIPTION,
			p2.USRP_PLATE AS USRP_PLATE2, p3.USRP_PLATE AS USRP_PLATE3, p4.USRP_PLATE AS USRP_PLATE4, p5.USRP_PLATE AS USRP_PLATE5, p6.USRP_PLATE AS USRP_PLATE6, p7.USRP_PLATE AS USRP_PLATE7, p8.USRP_PLATE AS USRP_PLATE8, p9.USRP_PLATE AS USRP_PLATE9, p10.USRP_PLATE AS USRP_PLATE10
	FROM   operations o1
			LEFT JOIN user_plates p2 ON p2.usrp_id = o1.ope_plate2_usrp_id
			LEFT JOIN user_plates p3 ON p3.usrp_id = o1.ope_plate3_usrp_id
			LEFT JOIN user_plates p4 ON p3.usrp_id = o1.ope_plate4_usrp_id
			LEFT JOIN user_plates p5 ON p3.usrp_id = o1.ope_plate5_usrp_id
			LEFT JOIN user_plates p6 ON p3.usrp_id = o1.ope_plate6_usrp_id
			LEFT JOIN user_plates p7 ON p3.usrp_id = o1.ope_plate7_usrp_id
			LEFT JOIN user_plates p8 ON p3.usrp_id = o1.ope_plate8_usrp_id
			LEFT JOIN user_plates p9 ON p3.usrp_id = o1.ope_plate9_usrp_id
			LEFT JOIN user_plates p10 ON p3.usrp_id = o1.ope_plate10_usrp_id, 
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
						FROM   operations o2
						WHERE  o2.ope_tar_id = o1.ope_tar_id 
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


