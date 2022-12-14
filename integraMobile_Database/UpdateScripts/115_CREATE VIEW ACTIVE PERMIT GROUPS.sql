CREATE VIEW VW_ACTIVE_PERMIT_GROUPS
AS 
SELECT GRP_ID, GRP_DESCRIPTION, GRP_INS_ID
FROM GROUPS WHERE GRP_ID IN (
	SELECT tg.TARGR_GRP_ID FROM TARIFFS t, TARIFFS_IN_GROUPS tg
	WHERE tar_type = 1 
	AND t.TAR_ID = tg.TARGR_TAR_ID 
	AND tg.TARGR_GRP_ID IS NOT NULL
	
	UNION ALL
	
	SELECT gta.GTA_GRP_ID 
	FROM TARIFFS t, TARIFFS_IN_GROUPS tg, GROUPS_TYPES gt, GROUPS_TYPES_ASSIGNATIONS gta
	WHERE tar_type = 1 
	AND t.TAR_ID = tg.TARGR_TAR_ID 
	AND tg.TARGR_GRPT_ID IS NOT NULL
	AND tg.TARGR_GRPT_ID = gt.GRPT_ID 
	AND gt.GRPT_ID=gta.GTA_GRPT_ID
)
