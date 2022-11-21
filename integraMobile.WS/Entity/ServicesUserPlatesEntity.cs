using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.Domain;
using integraMobile.WS;
using System.Collections;
using Newtonsoft.Json;
using integraMobile.Domain.Abstract;
using System.Xml;
using integraMobile.WS.Tools;

namespace integraMobile.WS.Entity
{
    public class ServicesUserPlatesEntity: SERVICES_USER_PLATE
    {
        #region Constructor
        public ServicesUserPlatesEntity()
        { 
        }

        public ServicesUserPlatesEntity(string xmlIn, ref ServicesPlateParameterInEntity addServicesPlateParameterInEntity, USER user)
        {

            if (xmlIn.Contains("<Photo>"))
            {
                xmlIn = xmlIn.Replace("<ipark_in>", @"<ipark_in xmlns:json='http://james.newtonking.com/projects/json'>");
                xmlIn = xmlIn.Replace("<Photo>", @"<Photo json:Array='true'>");
            }

            ServicesPlateParameterInEntity oAddServicesPlateParameterInEntity = Helpers.StrinXmlToObject<ServicesPlateParameterInEntity>(xmlIn,true);
        
            TypeTypeServicesEnum oTypeTypeServicesEnum = (TypeTypeServicesEnum)Convert.ToInt32(oAddServicesPlateParameterInEntity.TypeOfServiceType);
            switch (oTypeTypeServicesEnum)
            {
                //Carga y descarga
                case TypeTypeServicesEnum.DUM:
                    if (!String.IsNullOrEmpty(oAddServicesPlateParameterInEntity.CompanyName) &&
                        !String.IsNullOrEmpty(oAddServicesPlateParameterInEntity.CifNifCompany))
                    {
                        SERUP_COMPANY_NAME = oAddServicesPlateParameterInEntity.CompanyName;
                        SERUP_CIF_NIF_COMPANY = oAddServicesPlateParameterInEntity.CifNifCompany;
                    }
                    break;
                //Minusválido
                case TypeTypeServicesEnum.PMR:
                    if (!String.IsNullOrEmpty(oAddServicesPlateParameterInEntity.FirstName) &&
                        !String.IsNullOrEmpty(oAddServicesPlateParameterInEntity.FirstName) &&
                        !String.IsNullOrEmpty(oAddServicesPlateParameterInEntity.CardReducedMovility))
                    {
                        SERUP_FIRST_NAME = oAddServicesPlateParameterInEntity.FirstName;
                        SERUP_LAST_NAME = oAddServicesPlateParameterInEntity.FirstName;
                        SERUP_CARD_REDUCED_MOVILITY = oAddServicesPlateParameterInEntity.CardReducedMovility;
                    }
                    break;

            }
            addServicesPlateParameterInEntity = oAddServicesPlateParameterInEntity;
        }
        #endregion

        public static TypeTypeServicesEnum GetTypeTypeServicesEnum(SERVICES_USER_PLATE serviceplate )
        {
            TypeTypeServicesEnum oTypeTypeServicesEnum= TypeTypeServicesEnum.None;
            if (serviceplate != null)
            {
                if (!string.IsNullOrEmpty(serviceplate.SERUP_CIF_NIF_COMPANY) &&
                    !string.IsNullOrEmpty(serviceplate.SERUP_COMPANY_NAME))
                {
                    oTypeTypeServicesEnum = TypeTypeServicesEnum.DUM;
                }
                else if (!string.IsNullOrEmpty(serviceplate.SERUP_FIRST_NAME) &&
                    !string.IsNullOrEmpty(serviceplate.SERUP_LAST_NAME) &&
                    !string.IsNullOrEmpty(serviceplate.SERUP_CARD_REDUCED_MOVILITY))
                {
                    oTypeTypeServicesEnum = TypeTypeServicesEnum.PMR;
                }
            }
            return oTypeTypeServicesEnum;
        }
        
    }
}