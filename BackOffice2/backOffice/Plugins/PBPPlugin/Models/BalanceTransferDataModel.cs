using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Globalization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using PBPPlugin.WS;

namespace backOffice.Models
{

    public enum BalanceTransferDataStatus
    {
        Idle,
        Adding,
        AddingSuccess,
        AddingFail,
        Transfering,
        TransferSuccess,
        TransferFail
    }

    [Serializable]
    public class BalanceTransferDataModel
    {

        [NonSerialized]
        private IBackOfficeRepository m_oBackOfficeRepository = null;

        private long m_dUniqueId;
        
        #region Properties

        [LocalizedDisplayNameBundle("BalanceTransferDataModel.SourceEmail", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Source Email")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public string SourceEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayNameBundle("BalanceTransferDataModel.Password", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Password")]
        public string Password { get; set; }

        [LocalizedDisplayNameBundle("BalanceTransferDataModel.Amount", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Transfer Amount")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]        
        public int Amount { get; set; }

        /*private List<EmailDataModel> recipients = new List<EmailDataModel>();        

        [LocalizedDisplayNameBundle("EmailToolDataModel_Recipients", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Recipients")]
        public EmailDataModel[] Recipients
        {
            get { return recipients.ToArray(); }
            set { recipients = value.ToList(); }
        }*/

        public string UniqueId
        {
            get { return m_dUniqueId.ToString(); }
        }

        #endregion

        #region Constructor

        public BalanceTransferDataModel()
        {
        }

        public BalanceTransferDataModel(IBackOfficeRepository oBackOfficeRepository)
        {
            m_oBackOfficeRepository = oBackOfficeRepository;
            this.m_dUniqueId = DateTime.UtcNow.Ticks;

            // Delete old data from recipients temporary data            
            m_oBackOfficeRepository.DeleteEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID <= DateTime.UtcNow.AddDays(-1).Ticks));

        }

        #endregion

        #region Public Methods

        public void AddRecipient(string sRecipient)
        {
            if (m_oBackOfficeRepository != null)
            {
                m_oBackOfficeRepository.AddEmailToolRecipient(m_dUniqueId, sRecipient);
            }
        }
        public void AddRecipients(string[] arrRecipients)
        {
            if (m_oBackOfficeRepository != null)
            {
                var oRecipients = arrRecipients.ToList();

                int iBlockSize = 3000;
                int iBlocks = oRecipients.Count / iBlockSize;
                for (int i = 0; i < iBlocks; i++)
                {
                    m_oBackOfficeRepository.AddEmailToolRecipients(m_dUniqueId, oRecipients.GetRange(i * iBlockSize, iBlockSize).ToArray());
                }
                if (oRecipients.Count > (iBlocks * iBlockSize))
                {
                    m_oBackOfficeRepository.AddEmailToolRecipients(m_dUniqueId, oRecipients.GetRange(iBlocks * iBlockSize, oRecipients.Count - (iBlocks * iBlockSize)).ToArray());
                }
                //m_oBackOfficeRepository.AddEmailToolRecipients(m_dUniqueId, arrRecipients);
            }
        }
        public void AddRecipients(DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<USER>();
            predicate = predicate.And(u => u.USR_ENABLED == 1);
            if (request.Filters != null)
            {
                decimal installationId = InstallationFilters(request.Filters);
                if (installationId > 0)
                {
                    predicate = predicate.And(u => u.HIS_OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
                }
            }
            /*IQueryable<UserDataModel>*/
            var users = UserDataModel.List(m_oBackOfficeRepository, predicate, false).ToDataSourceResult(request).Data;
            List<string> oRecipients = new List<string>();
            //for (int i = 0; i < 1000; i++)
            //{
            foreach (UserDataModel oUser in users)
            {
                oRecipients.Add(oUser.Email);
                //model.AddRecipient(oUser.Email);
            }
            //}
            this.AddRecipients(oRecipients.ToArray());
        }
        public void DeleteRecipient(string sRecipient)
        {
            if (m_oBackOfficeRepository != null)
            {
                m_oBackOfficeRepository.DeleteEmailToolRecipient(m_dUniqueId, sRecipient);
            }
        }
        public void DeleteAllRecipients()
        {
            if (m_oBackOfficeRepository != null)
            {
                m_oBackOfficeRepository.DeleteAllEmailToolRecipients(m_dUniqueId);
            }
        }

        public List<string> GetRecipients()
        {
            List<string> oRet = new List<string>();

            if (m_oBackOfficeRepository != null)
            {
                oRet = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && !e.ETR_EMAIL.StartsWith("##Status_")))
                                              .Select(e => e.ETR_EMAIL)
                                              .ToList();
            }

            return oRet;
        }

        public List<EmailDataModel> GetRecipientsModel()
        {
            List<EmailDataModel> oRet = new List<EmailDataModel>();

            if (m_oBackOfficeRepository != null)
            {
                oRet = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && !e.ETR_EMAIL.StartsWith("##Status_")))
                                              .Select(e => new EmailDataModel() { Email = e.ETR_EMAIL })
                                              .ToList();
            }

            return oRet;
        }

        public bool CheckBalanceTransferAmount(out USER oSourceUser)
        {
            bool bRet = false;

          
            oSourceUser = m_oBackOfficeRepository.GetUsers(PredicateBuilder.True<USER>().And(t => t.USR_EMAIL.ToUpper() == this.SourceEmail.ToUpper())).FirstOrDefault();

            if (oSourceUser != null)
            {
                bRet = (oSourceUser.USR_BALANCE >= (this.Amount * this.GetRecipients().Count));
            }

            return bRet;
        }

        public ResultType Transfer(out List<string> oTransfersSuccess, out List<string> oTransfersFail)
        {
            ResultType oRet = ResultType.Result_Error_Generic;

            oTransfersSuccess = new List<string>();
            oTransfersFail = new List<string>();

            var oRecipients = this.GetRecipients();

            if (oRecipients.Count > 0)
            {

                USER oSourceUser = null;
                if (this.CheckBalanceTransferAmount(out oSourceUser))
                {

                    WSIntegraMobile oWS = new WSIntegraMobile();
                    SortedList oParametersOut = new SortedList();
                    string sSessionID = "";

                    string sLanguage = "en-US"; // ((CultureInfo)HttpContext.Current.Session["Culture"]).Name;

                    var dInstallationId = m_oBackOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>().And(t => t.INS_ENABLED == 1)).Select(t => t.INS_ID).FirstOrDefault();

                    oRet = oWS.QueryLogin(this.SourceEmail, this.Password, sLanguage, true, Convert.ToInt32(dInstallationId), null, null, out sSessionID, ref oParametersOut);
                    if (oRet == ResultType.Result_OK)
                    {

                        oTransfersFail.AddRange(oRecipients);

                        foreach (string sRecipientEmail in oRecipients)
                        {
                            oParametersOut = new SortedList();
                            oRet = oWS.TransferBalance(this.SourceEmail, sSessionID, this.Password, this.Amount, sRecipientEmail, ref oParametersOut);
                            if (oRet == ResultType.Result_OK)
                            {
                                oTransfersSuccess.Add(sRecipientEmail);
                                oTransfersFail.Remove(sRecipientEmail);
                            }
                            else
                                break;
                        }

                        foreach (string sEmail in oTransfersSuccess)
                        {
                            this.DeleteRecipient(sEmail);
                        }
                    }

                }
                else if (oSourceUser == null)
                    oRet = ResultType.Result_Error_Invalid_User;
                else
                    oRet = ResultType.Result_Error_Not_Enough_Balance;
            }
            else
                oRet = ResultType.Result_Error_Invalid_Input_Parameter;

            return oRet;
        }

        public void SetRepository(IBackOfficeRepository oBackOfficeRepository)
        {
            m_oBackOfficeRepository = oBackOfficeRepository;
        }

        public bool SetStatus(BalanceTransferDataStatus oStatus, ResultType? oWSStatus = null)
        {
            bool bRet = false;
            bRet = m_oBackOfficeRepository.DeleteEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && e.ETR_EMAIL.StartsWith("##Status_")));
            if (bRet && oStatus != BalanceTransferDataStatus.Idle)
            {
                if (!oWSStatus.HasValue)
                    bRet = m_oBackOfficeRepository.AddEmailToolRecipient(m_dUniqueId, "##Status_" + oStatus.ToString());
                else
                    bRet = m_oBackOfficeRepository.AddEmailToolRecipient(m_dUniqueId, ("##Status_" + oStatus.ToString() + "_" + oWSStatus.Value.ToString().Replace("_", "~")));
            }
            return bRet;
        }

        public BalanceTransferDataStatus GetStatus(out int iRecipientsCount)
        {
            ResultType oWSStatus;
            return GetStatus(out iRecipientsCount, out oWSStatus);
        }
        public BalanceTransferDataStatus GetStatus(out int iRecipientsCount, out ResultType oWSStatus)
        {
            BalanceTransferDataStatus oRet = BalanceTransferDataStatus.Idle;
            iRecipientsCount = 0;
            oWSStatus = ResultType.Result_OK;

            var oItem = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && e.ETR_EMAIL.StartsWith("##Status_"))).FirstOrDefault();
            if (oItem != null)
            {
                if (!System.Enum.TryParse<BalanceTransferDataStatus>(oItem.ETR_EMAIL.Split('_')[1], out oRet))
                    oRet = BalanceTransferDataStatus.Idle;
                if (oItem.ETR_EMAIL.Split('_').Length > 2)
                {
                    if (!System.Enum.TryParse<ResultType>(oItem.ETR_EMAIL.Split('_')[2].Replace("~", "_"), out oWSStatus))
                        oWSStatus = ResultType.Result_OK;
                }
            }
            iRecipientsCount = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && !e.ETR_EMAIL.StartsWith("##Status_"))).Count();

            return oRet;
        }

        #endregion

        #region Private Methods

        private decimal InstallationFilters(IList<Kendo.Mvc.IFilterDescriptor> oFilters)
        {
            decimal installationId = 0;
            if (oFilters != null)
            {
                List<Kendo.Mvc.FilterDescriptor> delFilters = new List<Kendo.Mvc.FilterDescriptor>();
                List<Kendo.Mvc.CompositeFilterDescriptor> delCompositeFilters = new List<Kendo.Mvc.CompositeFilterDescriptor>();

                for (int iFilter = 0; iFilter < oFilters.Count; iFilter++)
                {
                    if (oFilters[iFilter].GetType() == typeof(Kendo.Mvc.FilterDescriptor))
                    {
                        Kendo.Mvc.FilterDescriptor oFilter = (Kendo.Mvc.FilterDescriptor)oFilters[iFilter];
                        if (oFilter.Member == "undefined" || oFilter.Member == "InstallationId")
                        {
                            installationId = Convert.ToDecimal(oFilter.Value);
                            delFilters.Add(oFilter);
                        }
                    }
                    else if (oFilters[iFilter].GetType() == typeof(Kendo.Mvc.CompositeFilterDescriptor))
                    {
                        Kendo.Mvc.CompositeFilterDescriptor oCompositeFilter = (Kendo.Mvc.CompositeFilterDescriptor)oFilters[iFilter];
                        installationId = InstallationFilters(oCompositeFilter.FilterDescriptors);
                        if (oCompositeFilter.FilterDescriptors.Count == 0)
                            delCompositeFilters.Add(oCompositeFilter);
                    }
                }
                foreach (var oDelFilter in delFilters)
                {
                    oFilters.Remove(oDelFilter);
                }
                foreach (var oDelCompositeFilter in delCompositeFilters)
                {
                    oFilters.Remove(oDelCompositeFilter);
                }

            }
            return installationId;
        }

        #endregion

    }
}