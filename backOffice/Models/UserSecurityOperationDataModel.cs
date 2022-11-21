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
    public class UserSecurityOperationDataModel
    {
        public decimal Id { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_OpType", NameResourceType = typeof(Resources))]
        public int OpType { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_Status", NameResourceType = typeof(Resources))]
        public int Status { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_UtcDateTime", NameResourceType = typeof(Resources))]
        public DateTime UtcDateTime { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_Username", NameResourceType = typeof(Resources))]
        public string Username { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_NewMainPhoneCountryId", NameResourceType = typeof(Resources))]
        public decimal? NewMainPhoneCountryId { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_NewMainPhoneNumber", NameResourceType = typeof(Resources))]
        public string NewMainPhoneNumber { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_NewEmail", NameResourceType = typeof(Resources))]
        public string NewEmail { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_UrlParameter", NameResourceType = typeof(Resources))]
        public string UrlParameter { get; set; }
        public string UrlPrefix { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_ActivationCode", NameResourceType = typeof(Resources))]
        public string ActivationCode { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_ActivationRetries", NameResourceType = typeof(Resources))]
        public int ActivationRetries { get; set; }

        [LocalizedDisplayName("UserSecurityOperationDataModel_LastSendDate", NameResourceType = typeof(Resources))]
        public DateTime? LastSendDate { get; set; }

        public string OpType_FK { get; set; }
        public string Status_FK { get; set; }
        public string NewMainPhoneCountryId_FK { get; set; }

        public static IQueryable<UserSecurityOperationDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<USERS_SECURITY_OPERATION>();
            return List(backOfficeRepository, predicate, true);
        }

        public static IQueryable<UserSecurityOperationDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<USERS_SECURITY_OPERATION>();
            return List(backOfficeRepository, predicate, loadFKs);
        }

        public static IQueryable<UserSecurityOperationDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<USERS_SECURITY_OPERATION, bool>> predicate, bool loadFKs)
        {

            string[] urlPrefix = { ConfigurationManager.AppSettings["UserSecurityOperations_ChangeEmail_Telephone_Url"] ?? "{0}",
                                   ConfigurationManager.AppSettings["UserSecurityOperations_RecoverPassword_Url"] ?? "{0}" };

            IQueryable<UserSecurityOperationDataModel> securityOperations = null;
            if (loadFKs)
            {
                securityOperations = (from dom in backOfficeRepository.GetUsersSecurityOperations(predicate)
                                      select new UserSecurityOperationDataModel()
                                         {
                                             Id = dom.USOP_ID,
                                             OpType = dom.USOP_OP_TYPE,
                                             Status = dom.USOP_STATUS,
                                             UtcDateTime = dom.USOP_UTCDATETIME,
                                             Username = dom.USER.USR_USERNAME,
                                             NewMainPhoneCountryId = dom.USOP_NEW_MAIN_TEL_COUNTRY,
                                             NewMainPhoneNumber = dom.USOP_NEW_MAIN_TEL,
                                             NewEmail = dom.USOP_NEW_EMAIL,
                                             UrlParameter = dom.USOP_URL_PARAMETER,
                                             UrlPrefix = (dom.USOP_OP_TYPE == 1 ? urlPrefix[0] : urlPrefix[1]),
                                             ActivationCode = dom.USOP_ACTIVATION_CODE,
                                             LastSendDate = dom.USOP_LAST_SENT_DATE,

                                             OpType_FK = SecurityOperationTypeDataModel.GetTypeIdString(dom.USOP_OP_TYPE),
                                             Status_FK = SecurityOperationStatusDataModel.GetTypeIdString(dom.USOP_STATUS),
                                             NewMainPhoneCountryId_FK = dom.COUNTRy.COU_DESCRIPTION
                                         });
            }
            else
            {
                securityOperations = (from dom in backOfficeRepository.GetUsersSecurityOperations(predicate)
                                      select new UserSecurityOperationDataModel()
                                         {
                                             Id = dom.USOP_ID,
                                             OpType = dom.USOP_OP_TYPE,
                                             Status = dom.USOP_STATUS,
                                             UtcDateTime = dom.USOP_UTCDATETIME,
                                             Username = dom.USER.USR_USERNAME,
                                             UrlPrefix = (dom.USOP_OP_TYPE == 1 ? urlPrefix[0] : urlPrefix[1]),
                                             NewMainPhoneCountryId = dom.USOP_NEW_MAIN_TEL_COUNTRY,
                                             NewMainPhoneNumber = dom.USOP_NEW_MAIN_TEL,
                                             NewEmail = dom.USOP_NEW_EMAIL,
                                             UrlParameter = dom.USOP_URL_PARAMETER,
                                             ActivationCode = dom.USOP_ACTIVATION_CODE,
                                             LastSendDate = dom.USOP_LAST_SENT_DATE
                                         });
            }
            securityOperations = securityOperations.OrderBy(e => e.LastSendDate);

            return securityOperations;
        }

    }
}