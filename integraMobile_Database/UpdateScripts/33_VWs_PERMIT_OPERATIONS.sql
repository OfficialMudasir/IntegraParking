create view VW_ACTIVE_PERMIT_OPERATIONS AS
SELECT o1.* 
FROM   operations o1, 
       tariffs t, 
       user_plates p1 
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
GO

create view VW_PERMIT_AUTORENEWABLE_OPERATIONS AS
SELECT o1.* 
FROM   operations o1, 
       tariffs t, 
       user_plates p1 
WHERE  Getutcdate() BETWEEN 
               ope_utc_inidate AND ope_permit_expiration_utc_time 
               AND (SELECT Count(*) 
                    FROM   operations o2, 
                           user_plates p2 
                    WHERE  o2.ope_tar_id = o1.ope_tar_id 
						   AND o2.ope_grp_id = o1.ope_grp_id
                           AND p2.usrp_id = o2.ope_usrp_id 
                           AND p1.usrp_plate = p2.usrp_plate 
                           AND o2.ope_date > o1.ope_date) = 0 
	   AND (o1.ope_permit_last_autorenewal_status is null OR o1.ope_permit_last_autorenewal_status<4)
       AND o1.OPE_PERMIT_AUTO_RENEW=1
       AND o1.ope_tar_id = t.tar_id 
       AND t.tar_type = 1 
       AND p1.usrp_id = o1.ope_usrp_id 
GO
