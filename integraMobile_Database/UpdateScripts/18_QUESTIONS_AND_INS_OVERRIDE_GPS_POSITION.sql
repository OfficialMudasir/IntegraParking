CREATE TABLE [dbo].[USER_APPROVAL_QUESTIONS](
	[UAQ_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UAQ_DESCRIPTION] [varchar](500) NULL,
	[UAQ_UAQV_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_USER_APPROVAL_QUESTION] PRIMARY KEY CLUSTERED 
(
	[UAQ_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO




CREATE TABLE [dbo].[USER_APPROVAL_QUESTION_URLS](
	[UAQU_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UAQU_UAQV_ID] [numeric](18, 0) NOT NULL,
	[UAQU_DESCRIPTION] [varchar](250) NULL,
 CONSTRAINT [PK_USER_APPROVAL_QUESTION_URL] PRIMARY KEY CLUSTERED 
(
	[UAQU_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO





CREATE TABLE [dbo].[USER_APPROVAL_QUESTION_VERSIONS](
	[UAQV_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UAQV_UAQ_ID] [numeric](18, 0) NOT NULL,
	[UAQV_VERSION] [varchar](50) NULL,
	[UAQV_DESCRIPTION] [varchar](250) NULL,
	[UAQV_MANDATORY] [int] NULL,
	[UAQV_LIT_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_USER_APPROVAL_QUESTION_VERSION] PRIMARY KEY CLUSTERED 
(
	[UAQV_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO





CREATE TABLE [dbo].[USER_APPROVED_QUESTIONS](
	[UAPQ_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UAPQ_USR_ID] [numeric](18, 0) NOT NULL,
	[UAPQ_UAQV_ID] [numeric](18, 0) NOT NULL,
	[UAPQ_APPROVED] [int] NOT NULL,
 CONSTRAINT [PK_USER_APPROVED_QUESTION] PRIMARY KEY CLUSTERED 
(
	[UAPQ_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO






CREATE TABLE [dbo].[USER_APPROVED_QUESTION_COUNTRIES](
	[UAQC_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UAQC_COU_ID] [numeric](18, 0) NOT NULL,
	[UAQC_UAQ_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_USER_APPROVED_QUESTION_COUNTRY] PRIMARY KEY CLUSTERED 
(
	[UAQC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO






CREATE TABLE [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES](
	[UAQUL_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UAQUL_UAQU_ID] [numeric](18, 0) NOT NULL,
	[UAQUL_LAN_ID] [numeric](18, 0) NOT NULL,
	[UAQUL_URL] [varchar](500) NULL,
 CONSTRAINT [PK_USER_APPROVED_QUESTION_URL_LANGUAGE] PRIMARY KEY CLUSTERED 
(
	[UAQUL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO





--RELACIONES

------------------------------------
--USER_APPROVAL_QUESTIONS
------------------------------------
ALTER TABLE [dbo].[USER_APPROVAL_QUESTIONS]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVAL_QUESTION_USER_APPROVAL_QUESTION_VERSION] FOREIGN KEY([UAQ_UAQV_ID])
REFERENCES [dbo].[USER_APPROVAL_QUESTION_VERSIONS] ([UAQV_ID])
GO

ALTER TABLE [dbo].[USER_APPROVAL_QUESTIONS] CHECK CONSTRAINT [FK_USER_APPROVAL_QUESTION_USER_APPROVAL_QUESTION_VERSION]
GO

------------------------------------
--USER_APPROVAL_QUESTION_VERSIONS
------------------------------------
ALTER TABLE [dbo].[USER_APPROVAL_QUESTION_VERSIONS]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVAL_QUESTION_VERSION_LITERALS] FOREIGN KEY([UAQV_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO

ALTER TABLE [dbo].[USER_APPROVAL_QUESTION_VERSIONS] CHECK CONSTRAINT [FK_USER_APPROVAL_QUESTION_VERSION_LITERALS]
GO

ALTER TABLE [dbo].[USER_APPROVAL_QUESTION_VERSIONS]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVAL_QUESTION_VERSION_USER_APPROVAL_QUESTION] FOREIGN KEY([UAQV_UAQ_ID])
REFERENCES [dbo].[USER_APPROVAL_QUESTIONS] ([UAQ_ID])
GO

ALTER TABLE [dbo].[USER_APPROVAL_QUESTION_VERSIONS] CHECK CONSTRAINT [FK_USER_APPROVAL_QUESTION_VERSION_USER_APPROVAL_QUESTION]
GO




------------------------------------
--USER_APPROVED_QUESTIONS
------------------------------------
ALTER TABLE [dbo].[USER_APPROVED_QUESTIONS]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVED_QUESTION_USER_APPROVAL_QUESTION_VERSION] FOREIGN KEY([UAPQ_UAQV_ID])
REFERENCES [dbo].[USER_APPROVAL_QUESTION_VERSIONS] ([UAQV_ID])
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTIONS] CHECK CONSTRAINT [FK_USER_APPROVED_QUESTION_USER_APPROVAL_QUESTION_VERSION]
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTIONS]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVED_QUESTION_USER_APPROVED_QUESTION] FOREIGN KEY([UAPQ_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTIONS] CHECK CONSTRAINT [FK_USER_APPROVED_QUESTION_USER_APPROVED_QUESTION]
GO


------------------------------------
--USER_APPROVED_QUESTION_COUNTRIES
------------------------------------


ALTER TABLE [dbo].[USER_APPROVED_QUESTION_COUNTRIES]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVED_QUESTION_COUNTRY_COUNTRIES] FOREIGN KEY([UAQC_COU_ID])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_COUNTRIES] CHECK CONSTRAINT [FK_USER_APPROVED_QUESTION_COUNTRY_COUNTRIES]
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_COUNTRIES]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVED_QUESTION_COUNTRY_USER_APPROVAL_QUESTION] FOREIGN KEY([UAQC_UAQ_ID])
REFERENCES [dbo].[USER_APPROVAL_QUESTIONS] ([UAQ_ID])
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_COUNTRIES] CHECK CONSTRAINT [FK_USER_APPROVED_QUESTION_COUNTRY_USER_APPROVAL_QUESTION]
GO

------------------------------------
--USER_APPROVED_QUESTION_URL_LANGUAGES
------------------------------------

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVED_QUESTION_URL_LANGUAGE_LANGUAGES] FOREIGN KEY([UAQUL_LAN_ID])
REFERENCES [dbo].[LANGUAGES] ([LAN_ID])
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES] CHECK CONSTRAINT [FK_USER_APPROVED_QUESTION_URL_LANGUAGE_LANGUAGES]
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]  WITH CHECK ADD  CONSTRAINT [FK_USER_APPROVED_QUESTION_URL_LANGUAGE_USER_APPROVED_QUESTION_URL_LANGUAGE] FOREIGN KEY([UAQUL_UAQU_ID])
REFERENCES [dbo].[USER_APPROVAL_QUESTION_URLS] ([UAQU_ID])
GO

ALTER TABLE [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES] CHECK CONSTRAINT [FK_USER_APPROVED_QUESTION_URL_LANGUAGE_USER_APPROVED_QUESTION_URL_LANGUAGE]
GO




---------------------
create view [dbo].[ALL_USER_APPROVAL_QUESTION_VERSIONS]
as 
SELECT *
FROM
	(SELECT *,ROW_NUMBER() over ( PARTITION BY UAQV_UAQ_ID ORDER BY UAQV_VERSION desc) as Orden 
	FROM [dbo].[USER_APPROVAL_QUESTION_VERSIONS]) 
	SubConsulta1 
where Orden=1
GO








/*****************************************************************************/
/*                       [USER_APPROVAL_QUESTIONS]                            */
/*****************************************************************************/
INSERT INTO [dbo].[USER_APPROVAL_QUESTIONS]([UAQ_DESCRIPTION],[UAQ_UAQV_ID])
VALUES('He leído y aceptado las Condiciones de uso y la Política de privacidad',NULL)
GO

INSERT INTO [dbo].[USER_APPROVAL_QUESTIONS]([UAQ_DESCRIPTION],[UAQ_UAQV_ID])
VALUES('Acepto recibir información sobre las novedades de servicios y/ó productos de integr@.',NULL)
GO


/*****************************************************************************/
/*                  [USER_APPROVED_QUESTION_COUNTRIES]                         */
/*****************************************************************************/

--Spain
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(198,1)
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(198,2)

--Canada
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(39,1)

--Aruba
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(12,1)

--Costa Rica
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(51,1)

--Mexico
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(135,1)

--Ireland
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(99,1)

--Panama
INSERT INTO [dbo].[USER_APPROVED_QUESTION_COUNTRIES]([UAQC_COU_ID],[UAQC_UAQ_ID])
VALUES(162,1)



go


/*****************************************************************************/
/*                          [LITERALS]                                       */
/*****************************************************************************/
INSERT INTO [dbo].[LITERALS]([LIT_ID],[LIT_DESCRIPTION],[LIT_KEY])
VALUES(50000001,'He leído y aceptado las Condiciones de uso y la P',NULL)
GO


INSERT INTO [dbo].[LITERALS]([LIT_ID],[LIT_DESCRIPTION],[LIT_KEY])
VALUES(50000002,'Acepto recibir información sobre las novedades',NULL)
GO


/*****************************************************************************/
/*                          [LITERAL_LANGUAGES]                              */
/*****************************************************************************/
/*****************************************************************************/
/*                          [LITERAL_LANGUAGES]                              */
/*****************************************************************************/
INSERT INTO [dbo].[LITERAL_LANGUAGES]([LITL_LIT_ID],[LITL_LAN_ID],[LITL_LITERAL])
VALUES(50000001,1,'He leído y aceptado las $Condiciones de uso$ y la $Política de privacidad$')
GO
INSERT INTO [dbo].[LITERAL_LANGUAGES]([LITL_LIT_ID],[LITL_LAN_ID],[LITL_LITERAL])
VALUES(50000001,2,'I have read and agreed to the $Condicions of use$ and the $Privacy Policy$')
GO

INSERT INTO [dbo].[LITERAL_LANGUAGES]([LITL_LIT_ID],[LITL_LAN_ID],[LITL_LITERAL])
VALUES(50000002,1,'Acepto recibir información sobre las novedades de servicios y/ó productos de integr@.')
GO




/*****************************************************************************/
/*                [USER_APPROVAL_QUESTION_VERSIONS]                           */
/*****************************************************************************/
INSERT INTO [dbo].[USER_APPROVAL_QUESTION_VERSIONS]([UAQV_UAQ_ID],[UAQV_VERSION],[UAQV_DESCRIPTION],[UAQV_MANDATORY],[UAQV_LIT_ID])
VALUES(1 ,'1.0.','Version 1.0. He leído y aceptado las Condiciones de uso y la Política de privacidad',1,50000001)
GO
INSERT INTO [dbo].[USER_APPROVAL_QUESTION_VERSIONS]([UAQV_UAQ_ID],[UAQV_VERSION],[UAQV_DESCRIPTION],[UAQV_MANDATORY],[UAQV_LIT_ID])
VALUES(1 ,'2.0.','Version 2.0. He leído y aceptado las Condiciones de uso y la Política de privacidad',1,50000001)
GO
INSERT INTO [dbo].[USER_APPROVAL_QUESTION_VERSIONS]([UAQV_UAQ_ID],[UAQV_VERSION],[UAQV_DESCRIPTION],[UAQV_MANDATORY],[UAQV_LIT_ID])
VALUES(1 ,'3.0.','Version 3.0. He leído y aceptado las Condiciones de uso y la Política de privacidad',1,50000001)
GO
INSERT INTO [dbo].[USER_APPROVAL_QUESTION_VERSIONS]([UAQV_UAQ_ID],[UAQV_VERSION],[UAQV_DESCRIPTION],[UAQV_MANDATORY],[UAQV_LIT_ID])
VALUES(2 ,'1.0.','Version 1.0. Acepto recibir información sobre las novedades de servicios y/ó productos de integr@.',0,50000002)
GO



/*****************************************************************************/
/*                       [USER_APPROVAL_QUESTIONS]                            */
/*****************************************************************************/
UPDATE USER_APPROVAL_QUESTIONS SET UAQ_UAQV_ID=3 where UAQ_ID=1
UPDATE USER_APPROVAL_QUESTIONS SET UAQ_UAQV_ID=4 where UAQ_ID=2


/*****************************************************************************/
/*                       [USER_APPROVAL_QUESTION_URLS]                        */
/*****************************************************************************/
INSERT INTO [dbo].[USER_APPROVAL_QUESTION_URLS]([UAQU_UAQV_ID],[UAQU_DESCRIPTION])
VALUES(1,'Version 1.0. He leído y aceptado las Condiciones de uso y la Política de privacidad')
GO

INSERT INTO [dbo].[USER_APPROVAL_QUESTION_URLS]([UAQU_UAQV_ID],[UAQU_DESCRIPTION])
VALUES(2,'Version 2.0. He leído y aceptado las Condiciones de uso y la Política de privacidad')
GO

INSERT INTO [dbo].[USER_APPROVAL_QUESTION_URLS]([UAQU_UAQV_ID],[UAQU_DESCRIPTION])
VALUES(3,'Version 3.0. He leído y aceptado las Condiciones de uso y la Política de privacidad')
GO

INSERT INTO [dbo].[USER_APPROVAL_QUESTION_URLS]([UAQU_UAQV_ID],[UAQU_DESCRIPTION])
VALUES(4,'Version 1.0. Acepto recibir información sobre las novedades de servicios y/ó productos de integr@.')
GO



/*****************************************************************************/
/*                 [USER_APPROVED_QUESTION_URL_LANGUAGES]                     */
/*****************************************************************************/
--sELECT  * FROM LICENSE_TERMS_PARAMS  WHERE LTP_LAN_ID=1
--vERSION 1
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,1,'http://www.id-a2.com/integra/es/terminos.html')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,1,'http://www.id-a2.com/integra/en/politica.html')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,2,'http://www.id-a2.com/integra/en/terminos.html')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,2,'http://www.id-a2.com/integra/es/politica.html')
GO


--vERSION 2
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,1,'http://ips.integraparking.com/LicenseTerms/LegalTerms.htm')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,1,'http://ips.integraparking.com/LicenseTerms/Policy.htm')
GO

INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,2,'http://ips.integraparking.com/LicenseTerms/LegalTerms.htm')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,2,'http://ips.integraparking.com/LicenseTerms/Policy.htm')
GO

--vERSION 3
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,1,'https://www.iparkme.com/iparkme/es/condiciones-legales/')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,1,'https://www.iparkme.com/iparkme/es/aviso-de-privacidad/')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,2,'https://www.iparkme.com/iparkme/en/legal-terms/')
GO
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,2,'https://www.iparkme.com/iparkme/en/privacy-notice-2/')
GO


/*****************************************************************************/
/*                 [USER_APPROVED_QUESTIONS]                                 */
/*****************************************************************************/


insert into [USER_APPROVED_QUESTIONS]([UAPQ_USR_ID],[UAPQ_UAQV_ID],[UAPQ_APPROVED])
select USR_ID, 3,1
from Users
where USR_ENABLED=1 and USR_COU_ID in (198,39,12,51,135,99,162) and USR_LCT_ID=3



insert into [USER_APPROVED_QUESTIONS]([UAPQ_USR_ID],[UAPQ_UAQV_ID],[UAPQ_APPROVED])
select USR_ID, 2,1
from Users
where USR_ENABLED=1 and USR_COU_ID in (198,39,12,51,135,99,162) and USR_LCT_ID=2




insert into [USER_APPROVED_QUESTIONS]([UAPQ_USR_ID],[UAPQ_UAQV_ID],[UAPQ_APPROVED])
select USR_ID, 1,1
from Users
where USR_ENABLED=1 and USR_COU_ID in (198,39,12,51,135,99,162) and USR_LCT_ID=1



/*****************************************************************************/
/*                 [INSTALLATIONS]                                           */
/*****************************************************************************/


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.INSTALLATIONS ADD
	INS_OVERRIDE_GPS_POSITION int NULL
GO
ALTER TABLE dbo.INSTALLATIONS ADD CONSTRAINT
	DF_INSTALLATIONS_INS_OVERRIDE_GPS_POSITION DEFAULT ((1)) FOR INS_OVERRIDE_GPS_POSITION
GO
ALTER TABLE dbo.INSTALLATIONS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT 

UPDATE [INSTALLATIONS] SET INS_OVERRIDE_GPS_POSITION=1

UPDATE [INSTALLATIONS] SET INS_OVERRIDE_GPS_POSITION=0 where INS_ID between 10000 and 10999




--INSERT DE REGISTROS QUE FALTANBAN
--INGLES
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000002,2,'I agree to receive information about the news of services and/or products of integr@')
  
--FRANCES
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000001,3,'J''ai lu et accepté les &Conditions d''utilisation& et la &Politique de confidentialité$')
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000002,3,'J''accepte de recevoir des informations sur l''actualité des services et/ou des produits de integr @.')

--CATALAN
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000001,4,'He llegit i acceptat les $Condicions d''ús$ i la $Política de privacitat$')
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000002,4,'Accepto rebre informació sobre les novetats de serveis i/o productes d''integr@')


--MEXICO
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000001,5,'He leído y aceptado las $Condiciones de uso$ y la $Política de privacidad$')
INSERT INTO LITERAL_LANGUAGES(LITL_LIT_ID, LITL_LAN_ID, LITL_LITERAL)
VALUES(50000002,5,'Acepto recibir información sobre las novedades de servicios y/ó productos de integr@.')


INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,3,'http://www.id-a2.com/integra/fr/terminos.html')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,3,'http://www.id-a2.com/integra/fr/politica.html')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,3,'http://ips.integraparking.com/LicenseTerms/LegalTerms.htm')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,3,'http://ips.integraparking.com/LicenseTerms/Policy.htm')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,3,'https://www.iparkme.com/iparkme/en/legal-terms/')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,3,'https://www.iparkme.com/iparkme/en/privacy-notice-2/')

INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,4,'http://www.id-a2.com/integra/ca/terminos.html')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,4,'http://www.id-a2.com/integra/ca/politica.html')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,4,'http://ips.integraparking.com/LicenseTerms/LegalTerms.htm')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,4,'http://ips.integraparking.com/LicenseTerms/Policy.htm')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,4,'https://www.iparkme.com/iparkme/es/condiciones-legales/')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,4,'https://www.iparkme.com/iparkme/es/aviso-de-privacidad/')


INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,5,'http://www.id-a2.com/integra/en/terminos.html')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(1,5,'http://www.id-a2.com/integra/en/politica.html')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,5,'http://ips.integraparking.com/LicenseTerms/LegalTerms.htm')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(2,5,'http://ips.integraparking.com/LicenseTerms/Policy.htm')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,5,'https://www.iparkme.com/iparkme/es-mx/condiciones-legales-mx/')
INSERT INTO [dbo].[USER_APPROVED_QUESTION_URL_LANGUAGES]([UAQUL_UAQU_ID],[UAQUL_LAN_ID],[UAQUL_URL])
VALUES(3,5,'https://www.iparkme.com/iparkme/es-mx/aviso-de-privacidad-mx/')