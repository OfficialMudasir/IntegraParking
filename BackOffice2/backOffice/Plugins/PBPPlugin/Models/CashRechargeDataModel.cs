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
using backOffice.Infrastructure.Security;
//using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using PBPPlugin.WS;

namespace backOffice.Models
{

    [Serializable]
    public class CashRechargeDataModel
    {

        [NonSerialized]
        private IGeograficAndTariffsRepository m_oGeograficAndTariffsRepository = null;
        [NonSerialized]
        private IBackOfficeRepository m_oBackOfficeRepository = null;
        [NonSerialized]
        private ICustomersRepository m_oCustomersRepository = null;

        private long m_dUniqueId;
        
        #region Properties
        
        [LocalizedDisplayNameBundle("CashRechargeDataModel.Installation", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "City")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public decimal InstallationId { get; set; }
        
        [LocalizedDisplayNameBundle("CashRechargeDataModel.Email", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Email")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public string Email { get; set; }

        [LocalizedDisplayNameBundle("CashRechargeDataModel.EmailRepeat", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Email Repeat")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public string EmailRepeat { get; set; }

        [LocalizedDisplayNameBundle("CashRechargeDataModel.TotalAmount", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Recharge Amount")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]        
        public int TotalAmount { get; set; }

        public string UniqueId
        {
            get { return m_dUniqueId.ToString(); }
        }

        public decimal? OperatorId
        {
            get
            {
                decimal? dRet = null;
                var oUser = FormAuthMemberShip.HelperService.GetCurrentUser();
                if (oUser != null)
                {
                    dRet = oUser.FdoId;
                }
                return dRet;
            }
        }
        #endregion

        #region Constructor

        public CashRechargeDataModel()
        {
        }

        public CashRechargeDataModel(IGeograficAndTariffsRepository oGeograficAndTariffsRepository, IBackOfficeRepository oBackOfficeRepository, ICustomersRepository oCustomersRepository)
        {
            m_oGeograficAndTariffsRepository = oGeograficAndTariffsRepository;
            m_oBackOfficeRepository = oBackOfficeRepository;
            m_oCustomersRepository = oCustomersRepository;

            this.m_dUniqueId = DateTime.UtcNow.Ticks;

            InstallationDataModel oInstallation = this.AllowedInstallations().FirstOrDefault();
            if (oInstallation != null)
            {
                this.InstallationId = oInstallation.InstallationId;
            }
            
        }

        #endregion

        #region Public Methods

        public IQueryable<InstallationDataModel> AllowedInstallations()
        {                        
            /*var predicate = PredicateBuilder.True<INSTALLATION>();
            var oAllowedInstallations = FormAuthMemberShip.HelperService.InstallationsFeatureAllowed("CashRechargeTool", AccessLevel.Read);
            predicate = predicate.And(i => oAllowedInstallations.Contains(Convert.ToInt32(i.INS_ID)));
            return InstallationDataModel.List(m_oBackOfficeRepository, predicate);*/
            //return Json(InstallationDataModel.List(backOfficeRepository, predicate), JsonRequestBehavior.AllowGet);

            var predicate = PredicateBuilder.True<INSTALLATION>();
            var oInsAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed("CASHRECHARGETOOL_READ");
            var oInsOperator = new List<int>();
            FINAN_DIST_OPERATOR oOperator = null;
            var oCurUser = FormAuthMemberShip.HelperService.GetCurrentUser();
            if (oCurUser != null && oCurUser.FdoId.HasValue)
            {
                if (m_oGeograficAndTariffsRepository.GetFinanDistOperator(oCurUser.FdoId.Value, ref oOperator))
                {
                    oInsOperator = oOperator.FINAN_DIST_OPERATORS_INSTALLATIONs.Select(i => Convert.ToInt32(i.FDOI_INS_ID)).ToList();
                }
            }
            predicate = predicate.And(i => oInsAllowed.Contains(Convert.ToInt32(i.INS_ID)) && i.INS_ENABLED == 1 &&
                                           oInsOperator.Contains(Convert.ToInt32(i.INS_ID)));
            return InstallationDataModel.List(m_oBackOfficeRepository, predicate);
        }

        public USER GetUser()
        {
            USER oUser = null;
            if (!m_oCustomersRepository.GetUserDataByEmail(ref oUser, this.Email))
                oUser = null;
            return oUser;
        }

        public bool RechargeSummary(USER oUser, out int iAmount, out int iFEE, out int iVAT)
        {
            bool bRet = false;
            iAmount = 0;
            iFEE = 0;
            iVAT = 0;

            if (oUser == null)
                m_oCustomersRepository.GetUserDataByEmail(ref oUser, this.Email);
            

            if (oUser != null)
            {

                decimal dPercVAT1;
                decimal dPercVAT2;
                decimal dPercFEE;
                int iPercFEETopped;
                int iFixedFEE;

                if (m_oCustomersRepository.GetFinantialParams(oUser, "", (int)PaymentMeanType.pmtCash, null, ChargeOperationsType.BalanceRecharge,
                                                              out dPercVAT1, out dPercVAT2, out dPercFEE, out iPercFEETopped, out iFixedFEE))
                {

                    int iPartialVAT1;
                    int iPartialPercFEE;
                    int iPartialFixedFEE;
                    int iPartialBonusFEE;
                    int iPartialPercFEEVAT = 0;
                    int iPartialFixedFEEVAT = 0;
                    int iPartialBonusFEEVAT = 0;
                    
                    iAmount = m_oCustomersRepository.CalculateFEEReverse(TotalAmount, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, 0,
                                                                         out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                      out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                    iFEE = (iPartialPercFEE - iPartialPercFEEVAT) + (iPartialFixedFEE - iPartialFixedFEEVAT);
                    //int iQC = iPartialBonusFEE - iPartialBonusFEEVAT;
                    iVAT = iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
                    

                    //NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    //numberFormatProvider.NumberDecimalSeparator = ".";
                    //decimal dQuantity = Convert.ToDecimal(iQuantity, numberFormatProvider) / 100;
                    //decimal dQuantityToCharge = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / 100;

                    bRet = true;
                }
            }

            return bRet;
        }

        public ResultType Recharge(USER oUser, out int iAmount, out int iFEE, out int iVAT)
        {
            ResultType oRet = ResultType.Result_Error_Generic;
            iAmount = 0;
            iFEE = 0;
            iVAT = 0;

            if (this.OperatorId.HasValue)
            {

                if (oUser == null)
                    m_oCustomersRepository.GetUserDataByEmail(ref oUser, this.Email);

                if (oUser != null)
                {
                    if (RechargeSummary(oUser, out iAmount, out iFEE, out iVAT))
                    {
                        string sUserIdent = oUser.USR_USERNAME + "_" + DateTime.UtcNow.Ticks.ToString();
                        string strCellModel = "";
                        string strOSVersion = "";
                        string strPhoneSerialNumber = "";
                        string strCulture = "en-US";
                        string strAppVersion = "1.5";
                        bool bSessionKeepAlive = true;
                        string sSessionID = "";

                        if (m_oCustomersRepository.StartSession(ref oUser, this.InstallationId, 5, sUserIdent, sUserIdent, sUserIdent, strCellModel,
                                                            strOSVersion, strPhoneSerialNumber, strCulture, strAppVersion, bSessionKeepAlive, out sSessionID))
                        {

                            WSIntegraMobile oWS = new WSIntegraMobile();
                            SortedList oParametersOut = new SortedList();


                            IUser oBackofficeUser = FormAuthMemberShip.HelperService.GetCurrentUser();

                            string strEmail = string.IsNullOrEmpty(oBackofficeUser.Email) ? "" : oBackofficeUser.Email;

                            oRet = oWS.ConfirmRecharge(oUser.USR_USERNAME, sSessionID, this.TotalAmount, this.InstallationId, this.OperatorId.Value, FormAuthMemberShip.HelperService.GetCurrentUsername(), strEmail, ref oParametersOut);
                        }
                        else
                            oRet = ResultType.Result_Error_Invalid_User;
                    }
                }
                else
                    oRet = ResultType.Result_Error_Invalid_User;

            }
            else
                oRet = ResultType.Result_Error_Generic;

            return oRet;
        }

        #endregion

        #region Private Methods


        #endregion

    }
}