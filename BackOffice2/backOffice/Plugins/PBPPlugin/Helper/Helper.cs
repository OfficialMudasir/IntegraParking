using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Models;

namespace backOffice.Helper
{
    public class Helper
    {
        /*
        public static void PopulateChargeOperationTypes(ViewDataDictionary ViewData, bool bNoRecharge = false)
        {
            IQueryable<ChargeOperationTypeDataModel> types;
            if (!bNoRecharge)            
                types = ChargeOperationTypeDataModel.List();            
            else
                types = ChargeOperationTypeDataModel.ListNoRecharge();
            ViewData["chargeOperationTypes" + (bNoRecharge?"NoRecharge":"")] = types;
            if (types.Count() > 0) ViewData["defaultChargeOperationTypes"] = types.First();
        }
        */
        public static void PopulatePaymentSuscryptionTypes(ViewDataDictionary ViewData, bool bAddNotDefinedOption = false)
        {
            var types = PaymentSuscryptionTypeDataModel.List();
            var list = types.ToList();
            if (bAddNotDefinedOption) list.Insert(0, new PaymentSuscryptionTypeDataModel() { PaymentSuscryptionTypeId = -1, Description = "-" });
            ViewData["paymentSuscryptionTypes"] = list;
            if (types.Count() > 0) ViewData["defaultPaymentSuscryptionTypes"] = list.First();
        }
        /*
        public static void PopulateUsers(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var users = UserDataModel.List(backOfficeRepository, false);
            ViewData["users"] = users;
            if (users.Count() > 0) ViewData["defaultUser"] = users.First();
        }

        public static void PopulateGroups(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var groups = GroupDataModel.List(backOfficeRepository);
            string sInstallation = ConfigurationManager.AppSettings["InstallationShortDesc"];
            if (!string.IsNullOrEmpty(sInstallation))
            {
                groups = groups.Where(t => t.InstallationShortDesc == sInstallation).OrderBy(t => t.Description).AsQueryable();
                ViewData["groupsDescriptionField"] = "Description";
            }
            else
            {
                groups = groups.OrderBy(t => t.InstallationShortDesc).ThenBy(t => t.Description).AsQueryable();
                ViewData["groupsDescriptionField"] = "DescriptionWithInst";
            }
            ViewData["groups"] = groups;
            if (groups.Count() > 0) ViewData["defaultGroup"] = groups.First();
        }

        public static void PopulateTariffs(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var tariffs = TariffDataModel.List(backOfficeRepository);
            string sInstallation = ConfigurationManager.AppSettings["InstallationShortDesc"];
            if (!string.IsNullOrEmpty(sInstallation))
            {
                tariffs = tariffs.Where(t => t.InstallationShortDesc == sInstallation).OrderBy(t => t.Description).AsQueryable();
                ViewData["tariffsDescriptionField"] = "Description";
            }
            else
            {
                tariffs = tariffs.OrderBy(t => t.InstallationShortDesc).ThenBy(t => t.Description).AsQueryable();
                ViewData["tariffsDescriptionField"] = "DescriptionWithInst";
            }
            ViewData["tariffs"] = tariffs;
            if (tariffs.Count() > 0) ViewData["defaultTariff"] = tariffs.First();
        }

        public static void PopulateServiceChargeTypes(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var types = ServiceChargeTypeDataModel.List(backOfficeRepository);
            ViewData["serviceChargeTypes"] = types;
            if (types.Count() > 0) ViewData["defaultServiceChargeType"] = types.First();
        }
        */
        public static void PopulateCurrencies(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var currencies = CurrencyDataModel.List(backOfficeRepository);
            ViewData["currencies"] = currencies;
            if (currencies.Count() > 0) ViewData["defaultCurrency"] = currencies.First();
        }
        
        public static void PopulateCountries(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var countries = CountryDataModel.List(backOfficeRepository);
            ViewData["countries"] = countries;
            if (countries.Count() > 0) ViewData["defaultCountry"] = countries.First();
        }
        
        public static void PopulatePaymentMeanTypes(ViewDataDictionary ViewData)
        {
            var types = PaymentMeanTypeDataModel.List();
            var list = types.ToList();
            list.Insert(0, new PaymentMeanTypeDataModel() { PaymentMeanTypeId = -1, Description = "-" });
            ViewData["paymentMeanTypes"] = list;
            if (types.Count() > 0) ViewData["defaultPaymentMeanTypes"] = list.First();
        }

        public static void PopulatePaymentMeanSubTypes(ViewDataDictionary ViewData)
        {
            var types = PaymentMeanSubTypeDataModel.List();
            ViewData["paymentMeanSubTypes"] = types;
            if (types.Count() > 0) ViewData["defaultPaymentMeanSubTypes"] = types.First();
        }
        
        public static void PopulateBooleans(ViewDataDictionary ViewData)
        {
            var types = BooleanDataModel.List();
            ViewData["booleans"] = types;
            if (types.Count() > 0) ViewData["defaultBooleans"] = types.First();
        }
        /*
        public static void PopulateOperationSourceTypes(ViewDataDictionary ViewData)
        {
            IQueryable<OperationSourceTypeDataModel> types;            
            types = OperationSourceTypeDataModel.List();
            ViewData["operationSourceTypes"] = types;
            if (types.Count() > 0) ViewData["defaultOperationSourceTypes"] = types.First();
        }

        public static void PopulateExternalProviders(ViewDataDictionary ViewData, IBackOfficeRepository backOfficeRepository)
        {
            var externalProviders = ExternalProviderDataModel.List(backOfficeRepository);
            ViewData["externalProviders"] = externalProviders;
            if (externalProviders.Count() > 0) ViewData["defaultExternalProvider"] = externalProviders.First();
        }

        public static bool MenuOptionEnabled(string sMenuOption)
        {
            bool bEnabled = true;
            string sOptions = System.Configuration.ConfigurationManager.AppSettings["MenuOptionsEnabled"];
            if (!string.IsNullOrEmpty(sOptions))
            {
                bEnabled = sOptions.Split(';').Contains(sMenuOption);
            }
            return bEnabled;
        }

        public static void PopulateMobileOSs(ViewDataDictionary ViewData)
        {
            IQueryable<MobileOSDataModel> types;            
            types = MobileOSDataModel.List();
            ViewData["mobileOSs"] = types;
            if (types.Count() > 0) ViewData["defaultMobileOSs"] = types.First();
        }

        public static void PopulateSecurityOperationType(ViewDataDictionary ViewData)
        {
            var types = SecurityOperationTypeDataModel.List();
            ViewData["securityOperationTypes"] = types;
            if (types.Count() > 0) ViewData["defaultSecurityOperationType"] = types.First();
        }

        public static void PopulateSecurityOperationStatus(ViewDataDictionary ViewData)
        {
            var types = SecurityOperationStatusDataModel.List();
            ViewData["securityOperationStatus"] = types;
            if (types.Count() > 0) ViewData["defaultSecurityOperationStatus"] = types.First();
        }
        */
    }
}