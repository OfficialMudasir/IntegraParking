using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Properties;

namespace backOffice.Models
{
    public class CustomerInscriptionDataModel
    {

        public decimal Id { get; set; }
        
        [LocalizedDisplayName("CustomerInscriptionDataModel_Name", NameResourceType = typeof(Resources))]
        public string Name { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_Surname1", NameResourceType = typeof(Resources))]
        public string Surname1 { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_Surname2", NameResourceType = typeof(Resources))]
        public string Surname2 { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_DocId", NameResourceType = typeof(Resources))]
        public string DocId { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_MainPhoneCountryId", NameResourceType = typeof(Resources))]
        public decimal MainPhoneCountryId { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_MainPhoneNumber", NameResourceType = typeof(Resources))]
        public string MainPhoneNumber { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_AlternativePhoneCountryID", NameResourceType = typeof(Resources))]
        public decimal? AlternativePhoneCountryID { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_AlternativePhoneNumber", NameResourceType = typeof(Resources))]
        public string AlternativePhoneNumber { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_ActivationCode", NameResourceType = typeof(Resources))]
        public string ActivationCode { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_Url", NameResourceType = typeof(Resources))]
        public string Url { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_ActivationRetries", NameResourceType = typeof(Resources))]
        public decimal? ActivationRetries { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_LastSentTime", NameResourceType = typeof(Resources))]
        public DateTime? LastSentTime { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_Culture", NameResourceType = typeof(Resources))]
        public string Culture { get; set; }

        [LocalizedDisplayName("CustomerInscriptionDataModel_CustomerId", NameResourceType = typeof(Resources))]
        public decimal? CustomerId { get; set; }


        public string MainPhoneCountryId_FK { get; set; }
        public string AlternativePhoneCountryID_FK { get; set; }

        public static IQueryable<CustomerInscriptionDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<CUSTOMER_INSCRIPTION>();
            return List(backOfficeRepository, predicate, true);
        }

        public static IQueryable<CustomerInscriptionDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<CUSTOMER_INSCRIPTION>();
            return List(backOfficeRepository, predicate, loadFKs);
        }

        public static IQueryable<CustomerInscriptionDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<CUSTOMER_INSCRIPTION, bool>> predicate, bool loadFKs)
        {

            /*var dbUsers = backOfficeRepository.GetUsers(predicate);
            List<UserDataModel> users = new List<UserDataModel>();
            foreach (USER dom in dbUsers)
            {
                users.Add(new UserDataModel(dom, backOfficeRepository, loadFKs));
            }*/

            string sUrl = ConfigurationManager.AppSettings["CustomerInscriptions_Url"] ?? "";

            IQueryable<CustomerInscriptionDataModel> customerInscriptions = null;
            if (loadFKs)
            {
                customerInscriptions = (from dom in backOfficeRepository.GetCustomerInscriptions(predicate)
                                         select new CustomerInscriptionDataModel()
                                         {
                                             Id = dom.CUSINS_ID,
                                             Name  = dom.CUSINS_NAME,
                                             Surname1 = dom.CUSINS_SURNAME1,
                                             Surname2 = dom.CUSINS_SURNAME2,
                                             DocId = dom.CUSINS_DOC_ID,
                                             MainPhoneCountryId = dom.CUSINS_MAIN_TEL_COUNTRY,
                                             MainPhoneNumber = dom.CUSINS_MAIN_TEL,
                                             AlternativePhoneCountryID = dom.CUSINS_SECUND_TEL_COUNTRY,
                                             AlternativePhoneNumber = dom.CUSINS_SECUND_TEL,
                                             Email = dom.CUSINS_EMAIL,
                                             ActivationCode = dom.CUSINS_ACTIVATION_CODE,
                                             Url = sUrl + dom.CUSINS_URL_PARAMETER, //string.Format(sUrl, dom.CUSINS_URL_PARAMETER),
                                             ActivationRetries = dom.CUSINS_ACTIVATION_RETRIES,
                                             LastSentTime = dom.CUSINS_LAST_SENT_DATE,
                                             Culture = dom.CUISINS_CULTURE,
                                             CustomerId = dom.CUISINS_CUS_ID,
                                             MainPhoneCountryId_FK = dom.COUNTRy.COU_DESCRIPTION,
                                             AlternativePhoneCountryID_FK = (dom.COUNTRy1 != null) ? dom.COUNTRy1.COU_DESCRIPTION : ""
                                         })
                                         .OrderByDescending(c => c.LastSentTime)
                                         .AsQueryable();
            }
            else
            {
                customerInscriptions = (from dom in backOfficeRepository.GetCustomerInscriptions(predicate)
                                         select new CustomerInscriptionDataModel()
                                         {
                                             Id = dom.CUSINS_ID,
                                             Name = dom.CUSINS_NAME,
                                             Surname1 = dom.CUSINS_SURNAME1,
                                             Surname2 = dom.CUSINS_SURNAME2,
                                             DocId = dom.CUSINS_DOC_ID,
                                             MainPhoneCountryId = dom.CUSINS_MAIN_TEL_COUNTRY,
                                             MainPhoneNumber = dom.CUSINS_MAIN_TEL,
                                             AlternativePhoneCountryID = dom.CUSINS_SECUND_TEL_COUNTRY,
                                             AlternativePhoneNumber = dom.CUSINS_SECUND_TEL,
                                             Email = dom.CUSINS_EMAIL,
                                             ActivationCode = dom.CUSINS_ACTIVATION_CODE,
                                             Url = sUrl + dom.CUSINS_URL_PARAMETER,
                                             ActivationRetries = dom.CUSINS_ACTIVATION_RETRIES,
                                             LastSentTime = dom.CUSINS_LAST_SENT_DATE,
                                             Culture = dom.CUISINS_CULTURE,
                                             CustomerId = dom.CUISINS_CUS_ID,
                                         }).OrderByDescending(c => c.LastSentTime).AsQueryable();
            }

            return customerInscriptions;
        }

    }
}