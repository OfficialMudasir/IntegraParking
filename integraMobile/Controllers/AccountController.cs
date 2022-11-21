using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Threading;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
//OBSOLETE
//using com.paypal.sdk.services;
//using com.paypal.soap.api;
//using com.paypal.sdk.profiles;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using System.Reflection;
using NPOI.HSSF.UserModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Stripe;
using integraMobile.ExternalWS;
using integraMobile.Domain.Concrete;
using integraMobile.Infrastructure.RedsysAPI;
using integraMobile.Helper;
using integraMobile.Request;
using System.Xml;
using Newtonsoft.Json;
using integraMobile.Response;


namespace integraMobile.Controllers
{

    [HandleError]
    [NoCache]
    public class AccountController : Controller
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(AccountController));

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;
        private IBackOfficeRepository backOfficeRepository;
        private SQLGeograficAndTariffsRepository geograficRepository;
        private const int DEFAULT_MAX_GRID_NUM_OPS = 20;
        private const string DEFAULT_STRIPE_IMAGE_URL = "https://stripe.com/img/documentation/checkout/marketplace.png";
        private integraMobile.ExternalWS.WSintegraMobile wswi = null;
        private string cs = ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ToString();

        public AccountController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;

            IBackOfficeRepository boR = new SQLBackOfficeRepository(cs);
            SQLGeograficAndTariffsRepository gR = new SQLGeograficAndTariffsRepository(cs);
            
            this.backOfficeRepository = boR;
            this.geograficRepository = gR;

            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;

            wswi = new integraMobile.ExternalWS.WSintegraMobile(boR, customersRepository, infraestructureRepository, gR);
        }


        // public PermitsController()
        //{
        //    IBackOfficeRepository boR = new SQLBackOfficeRepository(cs);
        //    ICustomersRepository cuR = new SQLCustomersRepository(cs);
        //    IInfraestructureRepository isR = new SQLInfraestructureRepository(cs);
        //    SQLGeograficAndTariffsRepository gR = new SQLGeograficAndTariffsRepository(cs);
        //    this.backOfficeRepository = boR;
        //    this.customersRepository = cuR;
        //    this.infrastructureRepository = isR;
        //    this.geograficRepository = gR;

        //    wswi = new integraMobile.ExternalWS.WSintegraMobile(boR, cuR, isR, gR);
        //    m_Log = new CLogWrapper(typeof(PermitsController));
        //}



        [Authorize]
        public ActionResult SelectSuscriptionType(string strURLSufix)
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {              
                string strSuscriptionType = "";
                RefundBalanceType eRefundBalType = RefundBalanceType.rbtAmount;
                customersRepository.GetUserPossibleSuscriptionTypes(ref oUser, infraestructureRepository, out strSuscriptionType, out eRefundBalType);

                if (strSuscriptionType.Length == 0)
                {
                    SelectSuscriptionTypeModel model = new SelectSuscriptionTypeModel();
                    model.SuscriptionType = oUser.USR_SUSCRIPTION_TYPE;

                    NumberFormatInfo provider = new NumberFormatInfo();
                    provider.NumberDecimalSeparator = ".";

                    ViewData["CurrencyISOCode"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                    ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(ViewData["CurrencyISOCode"].ToString()) + "}", 
                        Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(ViewData["CurrencyISOCode"].ToString()));
                    ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];
                    ViewData["ChargeCurrency"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                    ViewData["ChargeValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(ViewData["CurrencyISOCode"].ToString()) + "}", GetPayPerTransactionChargeAmount(ViewData["ChargeCurrency"].ToString()));
                    ViewData["oUser"] = oUser;

                    return View(model);
                }
                else
                {
                    int iSuscriptionType = Convert.ToInt32(strSuscriptionType);
                    if (oUser.USR_SUSCRIPTION_TYPE == null)
                    {
                        //Update user value to default one   
                        if (!customersRepository.SetUserSuscriptionType(ref oUser, (PaymentSuscryptionType)iSuscriptionType))
                        {
                            return LogOff();
                        }
                        if (!customersRepository.SetUserRefundBalanceType(ref oUser, eRefundBalType))
                        {
                            return LogOff();
                        }
                        Session["USER_ID"] = oUser.USR_ID;
                    }
                    return RedirectToAction("SelectPayMethod" + strURLSufix, "Account");
                }
            }
            else
            {
                return LogOff();
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult SelectSuscriptionType(SelectSuscriptionTypeModel model, string strURLSufix)
        {
            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";

                ViewData["CurrencyISOCode"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(ViewData["CurrencyISOCode"].ToString()) + "}", 
                    Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(ViewData["CurrencyISOCode"].ToString()));
                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];
                ViewData["ChargeCurrency"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                ViewData["ChargeValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(ViewData["CurrencyISOCode"].ToString()) + "}", 
                    GetPayPerTransactionChargeAmount(ViewData["ChargeCurrency"].ToString()));
                

                if (ModelState.IsValid)
                {
                    if (oUser.USR_SUSCRIPTION_TYPE != null)
                    {
                        if ((PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && (PaymentSuscryptionType)model.SuscriptionType.Value == PaymentSuscryptionType.pstPrepay)
                        { 
                            // Switching from PayPerTransaction to PrePay
                            Session["SuscriptionTypeSwitch"] = "PPT2PP";
                        }
                        else if ((PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPrepay && (PaymentSuscryptionType)model.SuscriptionType.Value == PaymentSuscryptionType.pstPerTransaction)
                        { 
                            // Switching from PrePay to PayPerTransaction
                            Session["SuscriptionTypeSwitch"] = "PP2PPT";
                        }
                        else 
                        {
                            Session["SuscriptionTypeSwitch"] = string.Empty;
                        }
                    }
                    else 
                    {
                        Session["SuscriptionTypeSwitch"] = string.Empty;
                    }
                    //Update user value to default one   
                    if (customersRepository.SetUserSuscriptionType(ref oUser, (PaymentSuscryptionType)model.SuscriptionType.Value))
                    {
                        Session["USER_ID"] = oUser.USR_ID;
                        return RedirectToAction("SelectPayMethod", "Account", new { bForceChange = true });
                    }
                    else
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                        return View(model);
                    }

                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return LogOff();
            }

        }




        [Authorize]
        public ActionResult SelectSuscriptionTypeINT(string strURLSufix)
        {
            USER oUser= GetUserFromSession();
            if (oUser != null)
            {
                IsUserBalanceCorrect(ref oUser);
                Session["USER_ID"] = oUser.USR_ID;
            }
            else
            {
                LogOff();
            }
            return SelectSuscriptionType("INT");
        }


        [HttpPost]
        [Authorize]
        public ActionResult SelectSuscriptionTypeINT(SelectSuscriptionTypeModel model, string strURLSufix)
        {
            USER oUser = GetUserFromSession();
            if (oUser != null)
            {
                IsUserBalanceCorrect(ref oUser);
                Session["USER_ID"] = oUser.USR_ID;
            }
            else
            {
                LogOff();
            }
            return SelectSuscriptionType(model,"INT");
        }




        [Authorize]
        public ActionResult SelectPayMethod(bool? bForceChange, string strURLSufix, decimal? InstallationId, bool? ReturnToPermits = false)
        {
            if (ReturnToPermits != null)
            {
                if ((bool)ReturnToPermits)
                {
                    Session["ReturnToPermits"] = true;
                }
                else
                {
                    if (Session["ReturnToPermits"] == null)
                    {
                        Session["ReturnToPermits"] = false;
                    }
                }
            }
            else 
            {
                if (Session["ReturnToPermits"] == null)
                {
                    Session["ReturnToPermits"] = false;
                }
            }

            if (Session["InstallationID"] == null)
            {
                Session["InstallationID"] = InstallationId;                
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;


            USER oUser= GetUserFromSession();

            if (oUser != null)
            {
                bool bAllowNoPaymentMethod = (infraestructureRepository.GetParameterValue("AllowNoPaymentMethod") == "1");

                if ((oUser.CUSTOMER.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Count() > 0)&&(string.IsNullOrEmpty(strURLSufix)))
                {
                    return RedirectToAction("SelectPayMethodINT", "Account", new { bForceChange = bForceChange });
                }

                string strSuscriptionType = "";
                RefundBalanceType eRefundBalType = RefundBalanceType.rbtAmount;
                customersRepository.GetUserPossibleSuscriptionTypes(ref oUser, infraestructureRepository, out strSuscriptionType, out eRefundBalType);

                ViewData["AutomaticRecharge"] = false;
                ViewData["SelectedQuantity"] = "-1";
                ViewData["SelectedQuantityBelow"] = "-1";
                ViewData["CurrencyISOCode"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                ViewData["SuscriptionType"] = oUser.USR_SUSCRIPTION_TYPE;
                ViewData["AcceptChargeValue"] = false;
                ViewData["ChargeCurrency"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                ViewData["ChargeValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUser.CURRENCy.CUR_ISO_CODE) + "}", GetPayPerTransactionChargeAmount(ViewData["ChargeCurrency"].ToString()));               
                ViewData["ForceChange"] = bForceChange;
                ViewData["OverWriteCreditCardValue"] = false;
                ViewData["SuscriptionTypeConf"] = strSuscriptionType;


                ViewData["ShowCheckCreditCard"] = false;

                bool bInvalidated = false;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId, out bInvalidated);

                if (oUserPaymentMean != null)
                {
                    ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                    if ((oUserPaymentMean.CUSPM_VALID == 1) && (oUserPaymentMean.CUSPM_ENABLED == 1)
                            && (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                    {

                        if (strURLSufix != "SUS")
                        {
                            return RedirectToAction("SelectPayMethodSUS", "Account", new { bForceChange = bForceChange });
                        }

                        ViewData["ShowCheckCreditCard"] = true;
                        ViewData["OverWriteCreditCardValue"] = Convert.ToBoolean(Session["OVERWRITE_CARD"]);
                        ViewData["CurrentPaymentType_PAN"] = oUserPaymentMean.CUSPM_TOKEN_MASKED_CARD_NUMBER;
                    }
                    else if ((bAllowNoPaymentMethod)&&((!bForceChange.HasValue) || (!bForceChange.Value)))
                    {
                        return RedirectToAction("Main", "Account");
                    }


                }                


                if ((!bForceChange.HasValue) || (!bForceChange.Value))
                {

                    if (oUserPaymentMean != null)
                    {

                        if (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                        {
                            if ((oUserPaymentMean.CUSPM_VALID == 1) && (oUserPaymentMean.CUSPM_ENABLED == 1))
                            {
                                //if ((oUser.USR_BALANCE <= 0) && (!bAllowNoPaymentMethod))
                                //{
                                    if (Session["SuscriptionTypeSwitch"] != null)
                                    {
                                        if (Session["SuscriptionTypeSwitch"].ToString() == "PPT2PP")
                                        {
                                            // JIRA IPM-2399
                                            Session["SuscriptionTypeSwitch"] = string.Empty;
                                            return RedirectToAction("Main", "Account");
                                        }
                                    }
                                    return RedirectToAction("Recharge" + strURLSufix, "Account");
                                //}
                                //else
                                //{
                                //    return RedirectToAction("Main", "Account");
                                //}
                            }

                        }
                        else if (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction)
                        {
                            if ((oUserPaymentMean.CUSPM_VALID == 1) &&
                                (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                                (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1))
                            {
                                return RedirectToAction("Main", "Account");
                            }
                            else if ((oUserPaymentMean.CUSPM_VALID == 1) &&
                                (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                                (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 0) &&
                                (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                            {
                                return RedirectToAction("Main", "Account");
                            }

                        }

                    }
                    else
                    {
                        if (bAllowNoPaymentMethod)
                        {
                            return RedirectToAction("Main", "Account");
                        }
                    }
  
                }


                switch ((PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE)
                {
                    case PaymentSuscryptionType.pstPrepay:
                        ViewData["SuscriptionTypeString"] = ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType") + " " + ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType_Prepay");
                        break;
                    case PaymentSuscryptionType.pstPerTransaction:
                        ViewData["SuscriptionTypeString"] = ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType") + " " + ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType_PerTransaction");
                        ViewData["AutomaticRecharge"] = true;
                        break;
                }



                if ((oUserPaymentMean != null) &&
                    //(oUserPaymentMean.CUSPM_VALID == 1) && 
                    (oUserPaymentMean.CUSPM_ENABLED == 1))
                {

                    SelectPayMethodModel model = new SelectPayMethodModel();
                    model.PaymentMean = oUserPaymentMean.CUSPM_PAT_ID.ToString();
                    model.AutomaticRecharge = oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1;
                    model.AutomaticRechargeQuantity = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.HasValue ? oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value.ToString() : "0";
                    model.AutomaticRechargeWhenBelowQuantity = oUserPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS.HasValue ? oUserPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS.Value.ToString() : "0";
                    ViewData["AutomaticRecharge"] = model.AutomaticRecharge;
                    if (model.AutomaticRecharge)
                    {
                        ViewData["SelectedQuantity"] = model.AutomaticRechargeQuantity;
                        ViewData["SelectedQuantityBelow"] = model.AutomaticRechargeWhenBelowQuantity;
                    }

                    if (strURLSufix == "SUS")
                    {
                        return SelectPayMethodSUS (
                            model,
                            model.AutomaticRecharge.ToString(),
                            "true",
                            "true",
                            bForceChange,
                            strURLSufix
                        );
                    }
                    else if (strURLSufix == "INT")
                    {
                        return SelectPayMethodINT(
                                model,
                                model.AutomaticRecharge.ToString(),
                                "true",
                                "true",
                                bForceChange,
                                strURLSufix
                            );
                    }
                    else
                    {
                        return SelectPayMethod(
                                model,
                                model.AutomaticRecharge.ToString(),
                                "true",
                                "true",
                                bForceChange,
                                strURLSufix                                
                            );
                    }

                    //return View(model);
                }
                else
                {
                    SelectPayMethodModel model = new SelectPayMethodModel();
                    model.PaymentMean = Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard).ToString();
                    model.AutomaticRecharge = false;
                    model.AutomaticRechargeQuantity = "0";
                    model.AutomaticRechargeWhenBelowQuantity = "0";

                    return SelectPayMethod(
                               model,
                               model.AutomaticRecharge.ToString(),
                               "true",
                               "true",
                               bForceChange,
                               strURLSufix
                           );

                    //return View();
                }


              
   
            }
            else
            {
                return LogOff();
            }
        }



        [HttpPost]
        [Authorize]
        public ActionResult SelectPayMethod(SelectPayMethodModel model, string AutomaticRecharge, string AcceptCharge,string OverWriteCreditCard, bool? bForceChange,string strURLSufix)
        {
            USER oUser= GetUserFromSession();

            if (oUser != null)
            {
                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;

                string strSuscriptionType = "";
                RefundBalanceType eRefundBalType = RefundBalanceType.rbtAmount;
                customersRepository.GetUserPossibleSuscriptionTypes(ref oUser, infraestructureRepository, out strSuscriptionType, out eRefundBalType);

                model.AutomaticRecharge = ((!String.IsNullOrEmpty(AutomaticRecharge)) && (AutomaticRecharge.ToLower() == "true"));
                bool bAcceptCharge = ((!String.IsNullOrEmpty(AcceptCharge)) && (AcceptCharge.ToLower() == "true"));
                bool bOverWriteCreditCard = ((!String.IsNullOrEmpty(OverWriteCreditCard)) && (OverWriteCreditCard.ToLower() == "true"));

                ViewData["AutomaticRecharge"] = model.AutomaticRecharge;
                ViewData["SelectedQuantity"] = model.AutomaticRechargeQuantity;
                ViewData["SelectedQuantityBelow"] = model.AutomaticRechargeWhenBelowQuantity;
                ViewData["CurrencyISOCode"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                ViewData["SuscriptionType"] = oUser.USR_SUSCRIPTION_TYPE;
                ViewData["AcceptChargeValue"] = bAcceptCharge;
                ViewData["ChargeCurrency"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                ViewData["ChargeValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(ViewData["CurrencyISOCode"].ToString()) + "}", 
                    GetPayPerTransactionChargeAmount(ViewData["ChargeCurrency"].ToString()));               
                ViewData["OverWriteCreditCardValue"] = bOverWriteCreditCard;
                ViewData["SuscriptionTypeConf"] = strSuscriptionType;
                Session["FORCED_CARD_RECREATION"] = false;


                ViewData["ShowCheckCreditCard"] = false;

                if (model.PaymentMeanTypeEnabled((PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, customersRepository, infraestructureRepository, ref oUser, (PaymentMeanType)Convert.ToInt32(model.PaymentMean)))
                    ViewData["PaymentType"] = Convert.ToInt32(model.PaymentMean);
                else
                    ModelState.AddModelError("PaymentMean", ResourceExtension.GetLiteral("ErrorsMsg_InvalidPaymentMeanType"));

                bool bInvalidated = false;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId, out bInvalidated);

                if (oUserPaymentMean != null)
                {
                    if ((oUserPaymentMean.CUSPM_VALID == 1) && (oUserPaymentMean.CUSPM_ENABLED == 1)
                            && (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                    {
                        ViewData["ShowCheckCreditCard"] = true;
                        ViewData["CurrentPaymentType_PAN"] = oUserPaymentMean.CUSPM_TOKEN_MASKED_CARD_NUMBER;
                    }
                }                




                switch ((PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE)
                {
                    case PaymentSuscryptionType.pstPrepay:
                        ViewData["SuscriptionTypeString"] = ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType") + " " + ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType_Prepay");
                        break;
                    case PaymentSuscryptionType.pstPerTransaction:
                        ViewData["SuscriptionTypeString"] = ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType") + " " + ResourceExtension.GetLiteral("SelectPayMethod_SuscriptionType_PerTransaction");
                        model.AutomaticRecharge = true;
                        model.AutomaticRechargeQuantity = "0";
                        model.AutomaticRechargeWhenBelowQuantity = "0";
                        break;

                }

                

                if (ModelState.IsValid)
                {

if (Convert.ToInt32(model.PaymentMean) == (int)PaymentMeanType.pmtPaypal)
{
    LogOff();
}
                    

                    bool bAddNewPayMean = false;

                    if ((oUserPaymentMean != null) &&
                        (oUserPaymentMean.CUSPM_VALID == 1) &&
                        (oUserPaymentMean.CUSPM_ENABLED == 1))
                    {

                        if (oUserPaymentMean.CUSPM_PAT_ID != Convert.ToInt32(model.PaymentMean))
                        {
                            bAddNewPayMean = true;
                        }
                        else
                        {
                            if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                            {
                                PaymentMeanCreditCardProviderType eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
                                try
                                {
                                    eProviderType = (PaymentMeanCreditCardProviderType)oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER;
                                    
                                }
                                catch
                                {
                                    eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
                                }

                                if (oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER != (int)eProviderType)
                                {
                                    bAddNewPayMean = true;
                                }
                            }
                            else if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                            {
                                if (Convert.ToBoolean(oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE) != model.AutomaticRecharge)
                                {
                                    bAddNewPayMean = true;
                                }

                            }

                        }

                    }
                    else
                    {
                        bAddNewPayMean=true;
                        if ((oUser.USR_SUSCRIPTION_TYPE.Value == (int)PaymentSuscryptionType.pstPrepay)&&(oUser.USR_BALANCE>0))
                        {
                            Session["FORCED_CARD_RECREATION"] = true;
                        }
                        
                    }


                    if ((!bAcceptCharge) && (oUser.USR_SUSCRIPTION_TYPE.Value == (int)PaymentSuscryptionType.pstPerTransaction)&&(bAddNewPayMean))
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("SelectPayMethod_PerTransactionCheckBox_Error"));
                        return View(model);
                    }


                    bool bResult = false;

                    Session["RechargeCreationType"] = PaymentMeanRechargeCreationType.pmrctRegularRecharge;

                    if ((oUserPaymentMean == null) && (oUser.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Count == 0))
                    {
                        Session["RechargeCreationType"] = PaymentMeanRechargeCreationType.pmrctUserCreationRecharge;
                    }                    

                    if (bAddNewPayMean)
                    {

                        if (oUser.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Count > 0)
                        {
                            Session["RechargeCreationType"] = PaymentMeanRechargeCreationType.pmrctChangePaymentMeanRecharge;
                        }

                        bResult = AddPayMeanToUser(ref oUser, model);
                    }
                    else
                    {
                        if (bOverWriteCreditCard)
                        {
                            bResult =CopyCurrentUserPayMean (ref oUser, oUserPaymentMean, model);
                        }
                        else
                        {
                            bResult = UpdateUserPayMean(ref oUser, oUserPaymentMean, model);
                        }

                        if (oUser.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Count > 0)
                        {
                            Session["RechargeCreationType"] = PaymentMeanRechargeCreationType.pmrctChangePaymentMeanRecharge;
                        }


                    }


                    if (!bResult)
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                        return View(model);
                    }

                    oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                     Session["USER_ID"] = oUser.USR_ID;
                     Session["OVERWRITE_CARD"] = false;

                    if (oUser.USR_SUSCRIPTION_TYPE.Value == (int)PaymentSuscryptionType.pstPrepay)
                    {
                        
                        //if (oUser.USR_BALANCE <= 0)
                        //{
                            if (Session["SuscriptionTypeSwitch"] != null)
                            {
                                if (Session["SuscriptionTypeSwitch"].ToString() == "PPT2PP")
                                {
                                    // JIRA IPM-2399
                                    Session["SuscriptionTypeSwitch"] = string.Empty;
                                    return RedirectToAction("Main", "Account");
                                }
                            }
                            if (bOverWriteCreditCard)
                            {
                                Session["OVERWRITE_CARD"] = true;
                            }
                            return RedirectToAction("Recharge" + strURLSufix, "Account");
                        //}
                        //else
                        //{
                        //    if (((((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtPaypal) &&
                        //        oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 0) ||
                        //        ((oUserPaymentMean.CUSPM_VALID == 1) &&
                        //        (oUserPaymentMean.CUSPM_ENABLED == 1))) && (!bOverWriteCreditCard))
                        //    {
                        //        return RedirectToAction("Main", "Account");
                        //    }
                        //    else
                        //    {
                        //        if (Session["SuscriptionTypeSwitch"] != null)
                        //        {
                        //            if (Session["SuscriptionTypeSwitch"].ToString() == "PPT2PP")
                        //            {
                        //                // JIRA IPM-2399
                        //                Session["SuscriptionTypeSwitch"] = string.Empty;
                        //                return RedirectToAction("Main", "Account");
                        //            }
                        //        }
                        //        if (bOverWriteCreditCard)
                        //        {
                        //            Session["OVERWRITE_CARD"] = true;
                        //        }
                        //        return RedirectToAction("Recharge" + strURLSufix, "Account");

                        //    }
                            

                    }
                    else //Pay per transaction
                    {

                        if ((oUserPaymentMean.CUSPM_VALID == 1) &&
                                (oUserPaymentMean.CUSPM_ENABLED == 1) && (!(bOverWriteCreditCard)))
                         {
                               return RedirectToAction("Main", "Account");
                         }
                         else
                         {
                                Session["OVERWRITE_CARD"] = true;
                                
                                NumberFormatInfo provider = new NumberFormatInfo();
                                provider.NumberDecimalSeparator = ".";
                                Session["CurrencyToRecharge"] = infraestructureRepository.GetCurrencyIsoCode((int)oUser.USR_CUR_ID);
                                Session["QuantityToRecharge"] = GetPayPerTransactionChargeAmount(Session["CurrencyToRecharge"].ToString()).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUser.CURRENCy.CUR_ISO_CODE) + "", provider);
                                Session["QuantityToRechargeBase"] = Session["QuantityToRecharge"];
                                Session["OperationChargeType"] = ChargeOperationsType.ServiceCharge;


                                m_Log.LogMessage(LogLevels.logINFO, string.Format("SelectPayMethod (Pay per Transaction) (POST): User={0} ; Session[QuantityToRecharge]={1}",
                                               oUser.USR_EMAIL, Session["QuantityToRecharge"]));


                                if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                                                           
                                {
                                    string strCreditCardProviderPrefix = "";
                                    string strCreditCardSessionName = "";

                                    string actionName = string.Empty;
                                    switch ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER)
                                    {
                                        case PaymentMeanCreditCardProviderType.pmccpCreditCall:
                                            strCreditCardProviderPrefix = "CC";
                                            strCreditCardSessionName = "InCreditCallPayment";
                                            actionName = strCreditCardProviderPrefix + "RedirectPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpIECISA:
                                            strCreditCardProviderPrefix = "CC2";
                                            strCreditCardSessionName = "InIECISAPayment";
                                            actionName = strCreditCardProviderPrefix + "RedirectPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpStripe:
                                            strCreditCardProviderPrefix = "CC3";
                                            strCreditCardSessionName = "InStripePayment";
                                            actionName = strCreditCardProviderPrefix + "RedirectPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpPayu:
                                            strCreditCardProviderPrefix = "CC4";
                                            strCreditCardSessionName = "InPayuPayment";
                                            actionName = "PayuRequestPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpTransbank:
                                            strCreditCardProviderPrefix = "CC5";
                                            strCreditCardSessionName = "InTransBankPayment";
                                            actionName = "TransBankRequestPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpMoneris:
                                            strCreditCardProviderPrefix = "CC6";
                                            strCreditCardSessionName = "InMonerisPayment";
                                            actionName = "MonerisRequestPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpBSRedsys:
                                            strCreditCardProviderPrefix = "CC7";
                                            strCreditCardSessionName = "InBSRedsysPayment";
                                            actionName = "BSRedsysRequestPT";
                                            break;
                                        case PaymentMeanCreditCardProviderType.pmccpPaysafe:                                            
                                            strCreditCardSessionName = "InPaysafePayment";
                                            actionName = "PaysafeRequestPT";
                                            break;
                                        default:
                                            break;
                                    }


                                    Session[strCreditCardSessionName] = true;

                                    if (Session["SuscriptionTypeSwitch"] != null)
                                    {
                                        if (Session["SuscriptionTypeSwitch"].ToString() == "PP2PPT")
                                        {
                                            if ((oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_VALID == 1) && (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_ENABLED == 1))
                                            {
                                                // JIRA IPM-2399
                                                Session["SuscriptionTypeSwitch"] = string.Empty;
                                                return RedirectToAction("Main", "Account");
                                            }
                                        }
                                    }
                                    return RedirectToAction(actionName);
                                }
                                else if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                                {
                                    Session["InPaypalPayment"] = true;
                                    return RedirectToAction("PaypalPreapprovalPayRedirectPT");
                                }
                                else
                                {
                                    return LogOff();
                                }

                        
                         }
                    }

                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return LogOff();
            }

        }


        [Authorize]
        public ActionResult SelectPayMethodINT(bool? bForceChange)
        {
            USER oUser = GetUserFromSession();
            if (oUser != null)
            {
                IsUserBalanceCorrect(ref oUser);
                Session["USER_ID"] = oUser.USR_ID;
            }
            else
            {
                LogOff();
            }
            return SelectPayMethod(bForceChange,"INT",null);
        }



        [HttpPost]
        [Authorize]
        public ActionResult SelectPayMethodINT(SelectPayMethodModel model, string AutomaticRecharge, string AcceptCharge, string OverWriteCreditCard, bool? bForceChange, string strURLSufix)
        {
            USER oUser = GetUserFromSession();
            if (oUser != null)
            {
                IsUserBalanceCorrect(ref oUser);
                Session["USER_ID"] = oUser.USR_ID;
            }
            else
            {
                LogOff();
            }
            return SelectPayMethod(model, AutomaticRecharge, AcceptCharge, OverWriteCreditCard, bForceChange, "INT");
        }


        [Authorize]
        public ActionResult SelectPayMethodSUS(bool? bForceChange)
        {
            USER oUser = GetUserFromSession();
            if (oUser != null)
            {
                IsUserBalanceCorrect(ref oUser);
                Session["USER_ID"] = oUser.USR_ID;
            }
            else
            {
                LogOff();
            }
            return SelectPayMethod(bForceChange, "SUS",null);
        }



        [HttpPost]
        [Authorize]
        public ActionResult SelectPayMethodSUS(SelectPayMethodModel model, string AutomaticRecharge, string AcceptCharge, string OverWriteCreditCard, bool? bForceChange, string strURLSufix)
        {
            USER oUser = GetUserFromSession();
            if (oUser != null)
            {
                IsUserBalanceCorrect(ref oUser);
                Session["USER_ID"] = oUser.USR_ID;
            }
            else
            {
                LogOff();
            }
            return SelectPayMethod(model, AutomaticRecharge, AcceptCharge, OverWriteCreditCard, bForceChange, "SUS");
        }


        [Authorize]
        public ActionResult Recharge(string strURLSufix)
        {
            Session["InCreditCallPayment"] = false;
            Session["InPaypalPayment"] = false;

            USER oUser = GetUserFromSession();
            RechargeModel model = new RechargeModel();

            if (oUser != null)
            {
                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                if (oUserPaymentMean == null)
                {
                    return RedirectToAction("SelectPayMethod" + strURLSufix, new { bForceChange = true });
                }

                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                ViewData["Name"]=oUser.CUSTOMER.CUS_FIRST_NAME+" "+oUser.CUSTOMER.CUS_SURNAME1+" "+oUser.CUSTOMER.CUS_SURNAME2;
	            ViewData["Address"]=oUser.CUSTOMER.CUS_STREET+" "+
                                    oUser.CUSTOMER.CUS_STREE_NUMBER.ToString()+" "+
                                    oUser.CUSTOMER.CUS_LEVEL_NUM.ToString()+" "+
                                    oUser.CUSTOMER.CUS_DOOR+" "+
                                    oUser.CUSTOMER.CUS_LETTER+" "+
                                    oUser.CUSTOMER.CUS_STAIR;
	            ViewData["City"]= oUser.CUSTOMER.CUS_CITY;
	            ViewData["State"]= oUser.CUSTOMER.CUS_STATE;
	            ViewData["ZipCode"]= oUser.CUSTOMER.CUS_ZIPCODE;
                ViewData["Country"] = oUser.CUSTOMER.COUNTRy.COU_DESCRIPTION;

                if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                {
                    ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                }
                else if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                {
                    ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                }
                else
                {
                    return LogOff();
                }

                model.AutomaticRecharge = oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE > 0 ? true : false;
                model.AutomaticRechargeQuantity = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.ToString();
                model.AutomaticRechargeWhenBelowQuantity = oUserPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS.ToString();
                model.PaypalID = oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID;


                ViewData["AutomaticRecharge"] = model.AutomaticRecharge;
                ViewData["SelectedQuantity"] = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE;
                ViewData["SelectedQuantityBelow"] = oUserPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS;

                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                ViewData["Culture"] = ((CultureInfo)Session["Culture"]).Name.Replace("-","_");

                decimal dPercVAT1;
                decimal dPercVAT2;
                decimal dPercFEE;
                decimal dPercFEETopped;
                decimal dFixedFEE;
                int? iPaymentTypeId = (int)ViewData["PaymentType"];
                int? iPaymentSubtypeId = null;

                if (customersRepository.GetFinantialParams(oUserPaymentMean.CURRENCy.CUR_ISO_CODE, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.BalanceRecharge,
                                                            out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                {

                    model.PercVAT1 = dPercVAT1;
                    model.PercVAT2 = dPercVAT2;
                    model.PercFEE = dPercFEE;
                    model.PercFEETopped = dPercFEETopped;
                    model.FixedFEE = dFixedFEE;

                    int iQFEE = 0;
                    decimal dQFEE = 0;
                    int iQVAT = 0;
                    int iQTotal = 0;
                    int iPartialVAT1;
                    int iPartialPercFEE;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;

                    List<string> oRechargeValuesBase = new List<string>();
                    List<string> oRechargeValuesFEE = new List<string>();
                    List<string> oRechargeValuesVAT = new List<string>();
                    List<string> oRechargeValues = new List<string>();

                    string sRechargesValues = "";
                    string sRechargeDefaultValueIndex = "";
                    string sRechargeMinValueIndex = "";
                    if ((Convert.ToBoolean(Session["OVERWRITE_CARD"])) || (Convert.ToBoolean(Session["FORCED_CARD_RECREATION"])))
                    {
                        sRechargesValues = ConfigurationManager.AppSettings["RechargeValuesChangeCard"] ?? "";
                        sRechargeDefaultValueIndex = ConfigurationManager.AppSettings["RechargeDefaultValueIndexChangeCard"] ?? "";
                        sRechargeMinValueIndex = ConfigurationManager.AppSettings["RechargeMinValueIndexChangeCard"] ?? "";
                    }
                    if (string.IsNullOrWhiteSpace(sRechargesValues))
                        sRechargesValues = ConfigurationManager.AppSettings["RechargeValues"] ?? "1000*2000*3000*4000*5000";
                    if (string.IsNullOrWhiteSpace(sRechargeDefaultValueIndex))
                        sRechargeDefaultValueIndex = ConfigurationManager.AppSettings["RechargeDefaultValueIndex"] ?? "0";
                    if (string.IsNullOrWhiteSpace(sRechargeMinValueIndex))
                        sRechargeMinValueIndex = ConfigurationManager.AppSettings["RechargeMinValueIndex"] ?? "";


                    string strCultureName = "";

                    try
                    {
                        strCultureName = ((CultureInfo)Session["Culture"]).Name;
                    }
                    catch { }

                    string strTraceRechargeValuesBase = "";

                    foreach (int iQuantity in sRechargesValues.Split('*').Select(sValue => Convert.ToInt32(sValue)))
                    {
                        iQTotal = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                        dQFEE = Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero);
                        if (dPercFEETopped > 0 && dQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                        dQFEE += dFixedFEE;
                        iQFEE = Convert.ToInt32(dQFEE);


                        
                        iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;

                        strTraceRechargeValuesBase += (iQuantity / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE))).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)) + "|";

                        oRechargeValuesBase.Add((iQuantity / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE))).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)));
                        if (dPercFEE != 0 || dFixedFEE != 0)
                            oRechargeValuesFEE.Add((iQFEE / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE))).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)));
                        else
                            oRechargeValuesFEE.Add("");
                        if (dPercVAT1 > 0 || dPercFEE > 0 || dFixedFEE > 0)
                            oRechargeValuesVAT.Add((iQVAT != 0 ? (iQVAT / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE))).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)) : ""));
                        else
                            oRechargeValuesVAT.Add("");

                        oRechargeValues.Add((iQTotal / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE))).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)));
                    }

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Recharge: User={0} ; Culture={1}; RechargeValuesBase={2}",
                                                               oUser.USR_EMAIL, strCultureName, strTraceRechargeValuesBase));
                    
                    ViewData["RechargeValuesBase"] = oRechargeValuesBase;
                    ViewData["RechargeValuesFEE"] = oRechargeValuesFEE;
                    ViewData["RechargeValuesVAT"] = oRechargeValuesVAT;
                    ViewData["RechargeValues"] = oRechargeValues;
                    ViewData["RechargeDefaultValueIndex"] = Convert.ToInt32(sRechargeDefaultValueIndex);
                    ViewData["RechargeMinValueIndex"] = sRechargeMinValueIndex;

                }

            }
            else
            {
                return LogOff();
            }


            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Recharge(RechargeModel model, string AutomaticRecharge, string strURLSufix)
        {
            USER oUser= GetUserFromSession();

            if (oUser != null)
            {
                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                NumberFormatInfo provider = new NumberFormatInfo();
                string strCreditCardProviderPrefix = "CC";
                string strCreditCardSessionName = "";
                provider.NumberDecimalSeparator = ".";

                // Get finantial parameters again to avoid security issues
                decimal dPercVAT1;
                decimal dPercVAT2;
                decimal dPercFEE;
                decimal dPercFEETopped;
                decimal dFixedFEE;
                int? iPaymentTypeId = (int)oUserPaymentMean.CUSPM_PAT_ID;
                int? iPaymentSubtypeId = null;

                if (customersRepository.GetFinantialParams(oUserPaymentMean.CURRENCy.CUR_ISO_CODE, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.BalanceRecharge,
                                                            out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                {
                    model.PercVAT1 = dPercVAT1;
                    model.PercVAT2 = dPercVAT2;
                    model.PercFEE = dPercFEE;
                    model.PercFEETopped = dPercFEETopped;
                    model.FixedFEE = dFixedFEE;
                }
                else
                {
                    ModelState.AddModelError("GetFinantialParams", "Error getting finantial parameters");
                }

                int iQuantity;
                int iQTotal;
                int iPartialVAT1;
                int iPartialPercFEE;
                int iPartialFixedFEE;
                int iPartialPercFEEVAT;
                int iPartialFixedFEEVAT;

                double dQuantity = Convert.ToDouble(model.RechargeQuantity.Replace(",", "."), provider);
                iQuantity = Convert.ToInt32(dQuantity * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));

                iQTotal = customersRepository.CalculateFEE(iQuantity, model.PercVAT1, model.PercVAT2, model.PercFEE, model.PercFEETopped, model.FixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                Session["QuantityToRecharge"] = (iQTotal / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE), provider);

                string strCultureName="";

                try
                {
                    strCultureName = ((CultureInfo)Session["Culture"]).Name;
                }
                catch { }

                m_Log.LogMessage(LogLevels.logINFO, string.Format("Recharge (POST): User={5} ; Culture={8} ; model.RechargeQuantity={0} ; dQuantity={6} ; Convert.ToDouble(model.RechargeQuantity)={7} ; iQuantity={1} ; iQTotal={2} ; Session[QuantityToRecharge]={3} ; (iQTotal / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)))={4}",
                                                            model.RechargeQuantity, iQuantity, iQTotal, Session["QuantityToRecharge"],
                                                            (iQTotal / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE))), oUser.USR_EMAIL, dQuantity, Convert.ToDouble(model.RechargeQuantity),
                                                            strCultureName));



                Session["QuantityToRechargeBase"] = Convert.ToDecimal(dQuantity).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE), provider);
                Session["PercVAT1"] = model.PercVAT1.ToString("#0.00#",provider);
                Session["PercVAT2"] = model.PercVAT2.ToString("#0.00#",provider);
                Session["PercFEE"] = model.PercFEE.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE), provider);
                Session["PercFEETopped"] = model.PercFEETopped;
                Session["FixedFEE"] = model.FixedFEE;

                Session["CurrencyToRecharge"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                Session["OperationChargeType"] = ChargeOperationsType.BalanceRecharge;
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

                ViewData["Name"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1 + " " + oUser.CUSTOMER.CUS_SURNAME2;
                ViewData["Address"] = oUser.CUSTOMER.CUS_STREET + " " +
                                    oUser.CUSTOMER.CUS_STREE_NUMBER.ToString() + " " +
                                    oUser.CUSTOMER.CUS_LEVEL_NUM.ToString() + " " +
                                    oUser.CUSTOMER.CUS_DOOR + " " +
                                    oUser.CUSTOMER.CUS_LETTER + " " +
                                    oUser.CUSTOMER.CUS_STAIR;
                ViewData["City"] = oUser.CUSTOMER.CUS_CITY;
                ViewData["State"] = oUser.CUSTOMER.CUS_STATE;
                ViewData["ZipCode"] = oUser.CUSTOMER.CUS_ZIPCODE;
                ViewData["Country"] = oUser.CUSTOMER.COUNTRy.COU_DESCRIPTION;
                string actionName = string.Empty;
                if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                {
                    ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;

                    // MICHEL DEV
                    //oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER = (int)PaymentMeanCreditCardProviderType.pmccpPayu;
                    //oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER = (int)PaymentMeanCreditCardProviderType.pmccpMoneris;
                    //oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER = (int)PaymentMeanCreditCardProviderType.pmccpTransbank;


                    switch ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER)
                    {
                        case PaymentMeanCreditCardProviderType.pmccpCreditCall:
                            strCreditCardProviderPrefix="CC";
                            strCreditCardSessionName = "InCreditCallPayment";
                            actionName = strCreditCardProviderPrefix + "Redirect" + strURLSufix;
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpIECISA:
                            strCreditCardProviderPrefix = "CC2";
                            strCreditCardSessionName = "InIECISAPayment";
                            actionName = strCreditCardProviderPrefix + "Redirect" + strURLSufix;
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpStripe:
                            strCreditCardProviderPrefix = "CC3";
                            strCreditCardSessionName = "InStripePayment";
                            actionName = strCreditCardProviderPrefix + "Redirect" + strURLSufix;
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPayu:
                            strCreditCardProviderPrefix = "CC4";
                            strCreditCardSessionName = "InPayuPayment";
                            actionName = "PayuRequest";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpTransbank:
                            strCreditCardProviderPrefix = "CC5";
                            strCreditCardSessionName = "InTransBankPayment";
                            actionName = "TransBankRequest";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpMoneris:
                            strCreditCardProviderPrefix = "CC6";
                            strCreditCardSessionName = "InMonerisPayment";
                            actionName = "MonerisRequest";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpBSRedsys:
                            strCreditCardProviderPrefix = "CC7";
                            strCreditCardSessionName = "InBSRedsysPayment";
                            actionName = "BSRedsysRequest";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPaysafe:                            
                            strCreditCardSessionName = "InPaysafePayment";
                            actionName = "PaysafeRequest";
                            break;
                        default:
                            break;
                    }

                }
                else if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                {
                    ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                }
                else
                {
                    return LogOff();
                }

                model.AutomaticRecharge = ((!String.IsNullOrEmpty(AutomaticRecharge)) && (AutomaticRecharge.ToLower() == "true"));
                ViewData["AutomaticRecharge"] = model.AutomaticRecharge;
                ViewData["SelectedQuantity"] = model.AutomaticRechargeQuantity;
                ViewData["SelectedQuantityBelow"] = model.AutomaticRechargeWhenBelowQuantity;
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                ViewData["Culture"] = ((CultureInfo)Session["Culture"]).Name.Replace("-", "_");

                if (ModelState.IsValid)
                {

                    if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal) &&
                            (model.AutomaticRecharge) &&
                            (String.IsNullOrEmpty(model.PaypalID)))
                    {

                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_RequiredFieldPayPalID"));
                        return View(model);
                    }
                    else
                    {
                        if (UpdateUserPayMean(ref oUser, oUserPaymentMean, model))
                        {

                            Session["USER_ID"] = oUser.USR_ID;


                            if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard) &&
                                (oUser.USR_BALANCE<=0) &&
                                ((String.IsNullOrEmpty(oUserPaymentMean.CUSPM_TOKEN_CARD_HASH)) ||
                                (oUserPaymentMean.CUSPM_VALID == 0) ||
                                ((oUserPaymentMean.CUSPM_VALID == 1) && (Convert.ToBoolean(Session["OVERWRITE_CARD"]) == true))))
                            {
                                Session[strCreditCardSessionName] = true;
                                return RedirectToAction(actionName);
                            }
                            else if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard) &&
                                (oUser.USR_BALANCE > 0) &&
                                ((String.IsNullOrEmpty(oUserPaymentMean.CUSPM_TOKEN_CARD_HASH)) ||
                                (oUserPaymentMean.CUSPM_VALID == 0) ||
                                ((oUserPaymentMean.CUSPM_VALID == 1) && (Convert.ToBoolean(Session["OVERWRITE_CARD"]) == true))))
                            {
                                Session[strCreditCardSessionName] = true;
                                return RedirectToAction(actionName);
                            }
                            else if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard) &&
                                (!String.IsNullOrEmpty(oUserPaymentMean.CUSPM_TOKEN_CARD_HASH)) &&
                                (oUserPaymentMean.CUSPM_VALID == 1))
                            {
                                Session[strCreditCardSessionName] = true;
                                return RedirectToAction(actionName);
                            }
                            else if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal) &&
                                (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 0))
                            {
                                Session["InPaypalPayment"] = true;
                                return RedirectToAction("PaypalRedirect" + strURLSufix);
                            }
                            else if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal) &&
                                (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE != 0) && (oUser.USR_BALANCE <= 0) &&
                                ((String.IsNullOrEmpty(oUserPaymentMean.CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY)) ||
                                (oUserPaymentMean.CUSPM_VALID == 0)))
                            {
                                Session["InPaypalPayment"] = true;
                                return RedirectToAction("PaypalPreapprovalRedirect");
                            }
                            else if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal) &&
                                (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE != 0) && (oUser.USR_BALANCE > 0) &&
                                ((String.IsNullOrEmpty(oUserPaymentMean.CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY)) ||
                                (oUserPaymentMean.CUSPM_VALID == 0)))
                            {
                                Session["InPaypalPayment"] = true;
                                return RedirectToAction("PaypalPreapprovalRedirectINT");
                            }
                            else if ((oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal) &&
                                (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE != 0) &&
                                (!String.IsNullOrEmpty(oUserPaymentMean.CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY)) &&
                                (oUserPaymentMean.CUSPM_VALID == 1))
                            {
                                Session["InPaypalPayment"] = true;
                                return RedirectToAction("PaypalPreapprovalPayRedirect"+strURLSufix);
                            }
                            else
                            {
                                return LogOff();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                            return View(model);
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return LogOff();
            }

            

        }


        [Authorize]
        public ActionResult RechargeINT(string strURLSufix)
        {
            return Recharge("INT");
        }

        [Authorize]
        public ActionResult RechargeSUS(string strURLSufix)
        {
            return RedirectToAction("RechargeINT");
        }

        [HttpPost]
        [Authorize]
        public ActionResult RechargeINT(RechargeModel model, string AutomaticRecharge, string strURLSufix)
        {
            return Recharge(model,AutomaticRecharge,"INT");
        }

        #region Payu Payment

        [HttpGet]
        public ActionResult PayuRequestPT()
        {
            return RedirectToAction("PayuRequest", new { iSuffix = "PT" });
        }

        [HttpGet]
        public ActionResult PayuRequest(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InPayuPayment"])) return RedirectToAction("CCFailure" + iSuffix);

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }
           
            if (oGatewayConfig == null)
            {

                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT==1)||!r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpPayu))
                                        .FirstOrDefault();
            }

            Session["Payu_Config_id"] = oGatewayConfig.CPTGC_PAYUCON_ID;
            string email = oUser.USR_EMAIL;
            int amount = Convert.ToInt32(Session["QuantityToRecharge"].ToString().Replace(".", string.Empty));
            string isocode = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            string description = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");
            string utcdate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string culture = ((CultureInfo)Session["Culture"]).TwoLetterISOLanguageName;
            string ReturnURL = "";
            string hash = string.Empty;
            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            return payu.PayuRequest(string.Empty, email, amount, isocode, description, utcdate, culture, ReturnURL, hash);
        }

        [HttpPost]
        public ActionResult PayuResponsePT()
        {
            return PayuResponse("PT");
        }
                
        [HttpPost]
        public ActionResult PayuResponse(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InPayuPayment"])) return RedirectToAction("CCFailureINT");

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            Session["AmountCurrencyIsoCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            Session["PayerCurrencyISOCode"] = Session["AmountCurrencyIsoCode"];
            if (Session["CurrencyToRecharge"] != null) 
            {
                Session["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            }

            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            return payu.PayuResponse();
        }

        [HttpGet]
        public ActionResult PayuResultPT(string r)
        {
            return PayuResult(r, "PT");
        }

        [HttpGet]
        public ActionResult PayuResult(string r, string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            USER oUser = GetUserFromSession();
            Session["UserBalance"] = oUser.USR_BALANCE;            
            Session["PayerQuantity"] = Session["QuantityToRecharge"];

            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            string r_decrypted = payu.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

       
            if (j.result == "succeeded")
            {
                if (!CCSuccess(j.payu_reference.ToString(),
                              j.payu_transaction_id.ToString(),
                              null,
                              j.payu_date_time_local_fmt.ToString(),
                              j.payu_auth_code.ToString(),
                              j.errorCode.ToString(),
                              j.errorMessage.ToString(),
                              j.payu_card_hash.ToString(),
                              j.payu_card_reference.ToString(),
                              j.payu_card_scheme.ToString(),
                              j.payu_masked_card_number.ToString(),
                              PaymentMeanRechargeStatus.Committed,
                              null,
                              j.payu_name,
                              j.payu_document_id))
                {
                    j.result = "ERROR";
                    j.errorMessage = "ERROR";
                }
                else 
                {
                    Session["UserBalance"] = oUser.USR_BALANCE;   

                }
            }
            return payu.PayuResult(r);
            
        }

        #endregion

        #region TransBank Payment

        [HttpGet]
        public ActionResult TransBankRequestPT()
        {
            return RedirectToAction("TransBankRequest", new { iSuffix = "PT" });
        }

        [HttpGet]
        public ActionResult TransBankRequest(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InTransBankPayment"])) return RedirectToAction("CCFailure" + iSuffix);

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }
           
           
            if (oGatewayConfig == null)
            {
                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpTransbank))
                                        .FirstOrDefault();
            }

            Session["Transbank_Config_id"] = oGatewayConfig.CPTGC_TRBACON_ID;

            string email = oUser.USR_EMAIL;
            int amount = Convert.ToInt32(Session["QuantityToRecharge"].ToString().Replace(".", string.Empty));
            string isocode = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            string description = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");
            string utcdate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string culture = ((CultureInfo)Session["Culture"]).TwoLetterISOLanguageName;
            string ReturnURL = "";

            string hash = string.Empty;
            TransbankController transbank = new TransbankController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Transbank_Config_id"]));
            return transbank.TransbankRequest(string.Empty, email, amount, isocode, description, utcdate, ReturnURL, hash);
        }

        [HttpPost]
        public ActionResult TransbankResponsePT()
        {
            return TransbankResponse("PT");
        }

        [HttpPost]
        public ActionResult TransbankResponse(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InTransBankPayment"])) return RedirectToAction("CCFailureINT");

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            Session["AmountCurrencyIsoCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            Session["PayerCurrencyISOCode"] = Session["AmountCurrencyIsoCode"];
            if (Session["CurrencyToRecharge"] != null)
            {
                Session["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            }
            TransbankController transbank = new TransbankController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Transbank_Config_id"]));
            return transbank.TransbankResponse();
        }

        [HttpGet]
        public ActionResult TransBankResultPT(string r)
        {
            return TransBankResult(r, "PT");
        }

        [HttpGet]
        public ActionResult TransBankResult(string r, string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            USER oUser = GetUserFromSession();
            Session["UserBalance"] = oUser.USR_BALANCE;
            Session["PayerQuantity"] = Session["QuantityToRecharge"];

            TransbankController transbank = new TransbankController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Transbank_Config_id"]));
            string r_decrypted = transbank.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                if (!CCSuccess(j.transbank_reference.ToString(),
                              j.transbank_transaction_id.ToString(),
                              null,
                              j.transbank_date_time_local_fmt.ToString(),
                              j.transbank_auth_code.ToString(),
                              j.errorCode.ToString(),
                              j.errorMessage.ToString(),
                              j.transbank_card_hash.ToString(),
                              j.transbank_card_reference.ToString(),
                              j.transbank_card_scheme.ToString(),
                              j.transbank_masked_card_number.ToString(),
                              PaymentMeanRechargeStatus.Committed,
                              null,
                              null,
                              null))
                {
                    j.result = "ERROR";
                    j.errorMessage = "ERROR";
                }
                else
                {
                    Session["UserBalance"] = oUser.USR_BALANCE;
                }
            }
            return transbank.TransbankResult(r);
        }

        #endregion

        #region Moneris Payment

        [HttpGet]
        public ActionResult MonerisRequestPT()
        {
            return RedirectToAction("MonerisRequest", new { iSuffix = "PT" });
        }

        [HttpGet]
        public ActionResult MonerisRequest(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InMonerisPayment"])) return RedirectToAction("CCFailure" + iSuffix);

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }
          
            if (oGatewayConfig == null)
            {
                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpMoneris))
                                        .FirstOrDefault();
            }

            Session["Moneris_Config_id"] = oGatewayConfig.CPTGC_MONCON_ID;
            string Email = oUser.USR_EMAIL;
            int Amount = Convert.ToInt32(Session["QuantityToRecharge"].ToString().Replace(".", string.Empty));
            string CurrencyISOCODE = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            string Description = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");
            string UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string ReturnURL = "";
            string Hash = string.Empty;
            string culture = ((CultureInfo)Session["Culture"]).Name;
            Session["PAYMENT_ORIGIN"] = "AccountController";
            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));
            return moneris.MonerisRequestHT(string.Empty, Email, Amount, CurrencyISOCODE, Description, UTCDate, culture, ReturnURL, "", Hash);
        }

        //[HttpPost]
        //public ActionResult MonerisFailure()
        //{
        //    return MonerisFailure(Request["response_order_id"],
        //                        Request["response_code"],
        //                        Request["date_stamp"],
        //                        Request["time_stamp"],
        //                        Request["message"]);
        //}

        //[HttpGet]
        //public ActionResult MonerisFailure(string response_order_id,
        //                                   string response_code,
        //                                   string date_stamp,
        //                                   string time_stamp,
        //                                   string message)
        //{
        //    MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]), Session["Sufix"].ToString());
        //    return moneris.MonerisFailure(response_order_id, response_code, date_stamp, time_stamp, message);
        //}

        [HttpGet]
        public ActionResult MonerisResultPT(string r)
        {            
            return MonerisResult(r, "PT");
        }

        [HttpGet]
        public ActionResult MonerisResult(string r, string iSuffix)
        {
            Session["Suffix"] = iSuffix;

            USER oUser = GetUserFromSession();
            Session["UserBalance"] = oUser.USR_BALANCE;
            Session["PayerQuantity"] = Session["QuantityToRecharge"];

            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));
            string r_decrypted = moneris.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                DateTime? dtExpDate = null;
                try
                {
                    if ((j.moneris_expires_end_month.Length == 2) && (j.moneris_expires_end_year.Length == 2))
                    {
                        dtExpDate = new DateTime(Convert.ToInt32(j.moneris_expires_end_year)+2000, Convert.ToInt32(j.moneris_expires_end_month), 1).AddMonths(1).AddSeconds(-1);
                    }
                }
                catch { }


                if (!CCSuccess(j.moneris_reference.ToString(),
                              j.moneris_transaction_id.ToString(),
                              null,
                              j.moneris_date_time_local_fmt.ToString(),
                              j.moneris_auth_code.ToString(),
                              j.errorCode.ToString(),
                              j.errorMessage.ToString(),
                              j.moneris_card_hash.ToString(),
                              j.moneris_card_reference.ToString(),
                              j.moneris_card_scheme.ToString(),
                              j.moneris_masked_card_number.ToString(),
                              PaymentMeanRechargeStatus.Committed,
                              dtExpDate,
                              null,
                              null))
                {
                    j.result = "ERROR";
                    j.errorMessage = "ERROR";
                }
                else
                {
                    Session["UserBalance"] = oUser.USR_BALANCE;
                }
            }

            ViewData["Result"] = r_decrypted;
            if (Session["PayerQuantity"] != null) ViewData["PayerQuantity"] = Session["PayerQuantity"];
            if (Session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = Session["PayerCurrencyISOCode"];
            if (Session["UserBalance"] != null) ViewData["UserBalance"] = Session["UserBalance"];

            return View("MonerisResult"+iSuffix);
        }


        [HttpPost]
        public ActionResult MonerisResponseHTPT()
        {
            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));

            return moneris.MonerisResponseHT(Request["ResponseCode"],
                                     Request["DataKey"],
                                     Request["ErrorMessage"],
                                     Request["Bin"]);
        }

        [HttpPost]
        public ActionResult MonerisResponseHT()
        {
            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));

            return moneris.MonerisResponseHT(Request["ResponseCode"],
                                     Request["DataKey"],
                                     Request["ErrorMessage"],
                                     Request["Bin"]);
        }



        [HttpPost]
        public ActionResult MonerisMPIResponsePT()
        {
            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));

            return moneris.MonerisMPIResponse(Request["PaRes"],
                                               Request["MD"],
                                               true);
        }

        [HttpPost]
        public ActionResult MonerisMPIResponse()
        {
            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));

            return moneris.MonerisMPIResponse(Request["PaRes"],
                                               Request["MD"],
                                               true);
        }

        //[HttpPost]
        //public ActionResult MonerisSuccess()
        //{
        //    return MonerisSuccess(Request["response_order_id"],
        //                        Request["response_code"],
        //                        Request["date_stamp"],
        //                        Request["time_stamp"],
        //                        Request["eci"],
        //                        Request["txn_num"],
        //                        Request["bank_approval_code"],
        //                        Request["result"],
        //                        Request["trans_name"],
        //                        Request["gcardholder"],
        //                        Request["charge_total"],
        //                        Request["card"],
        //                        Request["f4l4"],
        //                        Request["message"],
        //                        Request["iso_code"],
        //                        Request["bank_transaction_id"],
        //                        Request["expiry_date"],
        //                        Request["cvd_response_code"],
        //                        Request["email"],
        //                        Request["cust_id"],
        //                        Request["note"]);
        //}

        //[HttpGet]
        //public ActionResult MonerisSuccess(string response_order_id,
        //                                   string response_code,
        //                                   string date_stamp,
        //                                   string time_stamp,
        //                                   string eci,
        //                                   string txn_num,
        //                                   string bank_approval_code,
        //                                   string result,
        //                                   string trans_name,
        //                                   string gcardholder,
        //                                   string charge_total,
        //                                   string card,
        //                                   string f4l4,
        //                                   string message,
        //                                   string iso_code,
        //                                   string bank_transaction_id,
        //                                   string expiry_date,
        //                                   string cvd_response_code,
        //                                   string email,
        //                                   string cust_id,
        //                                   string note)
        //{
        //    USER oUser = GetUserFromSession();
        //    if (oUser == null) return LogOff();

        //    Session["AmountCurrencyIsoCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
        //    Session["PayerCurrencyISOCode"] = Session["AmountCurrencyIsoCode"];
        //    if (Session["CurrencyToRecharge"] != null)
        //    {
        //        Session["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
        //    }

        //    MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]), Session["Sufix"].ToString());
        //    return moneris.MonerisSuccess(response_order_id, response_code, date_stamp, time_stamp, eci, txn_num, bank_approval_code, result, trans_name, gcardholder, charge_total, card, f4l4, message, iso_code, bank_transaction_id, expiry_date, cvd_response_code, email, cust_id, note);
        //}

        #endregion

        #region Credit Call Payment
        [Authorize]
        public ActionResult CCRedirect(string strURLSufix)
        {
        
            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {

                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpCreditCall))
                                        .FirstOrDefault();
            }


            ViewData["email"] = oUser.USR_EMAIL;
            ViewData["ekashu_form_url"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_EKASHU_FORM_URL;
            ViewData["ekashu_seller_id"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID;
            ViewData["ekashu_seller_key"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY.Substring(0, 8);
            ViewData["ekashu_amount"] = (string)Session["QuantityToRecharge"];
            ViewData["ekashu_currency"] = (string)Session["CurrencyToRecharge"];
            ViewData["ekashu_reference"] = CardEasePayments.UserReference();
            ViewData["ekashu_hash_code"] = CardEasePayments.HashCode(oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                     oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_HASH_KEY, 
                                                                     (string)ViewData["ekashu_reference"], 
                                                                     (string)Session["QuantityToRecharge"]);
            ViewData["ekashu_description"] = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");

            ViewData["css_url"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_CSS_URL;

            string strSellerName = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_SELLER_NAME;

            if (strSellerName.Length > 8)
                ViewData["ekashu_seller_name"] = strSellerName.Substring(0, 8);
            else
                ViewData["ekashu_seller_name"] = strSellerName;
            

            string requrl="";
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
            {
                requrl = Request.Url.ToString();
            }
            else
            {
                requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
            }

            ViewData["ekashu_failure_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCFailure" + strURLSufix;
            ViewData["ekashu_return_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCCancel" + strURLSufix;
            ViewData["ekashu_success_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCSuccess" + strURLSufix;


            return View();
        }

        [Authorize]
        public ActionResult CCRedirectINT()
        {
            return CCRedirect("INT");
        }

        [Authorize]
        public ActionResult CCRedirectPT()
        {
            return CCRedirect("PT");
        }


        [Authorize]
        public ActionResult CCSuccess(string strURLSufix)
        {
            string strCardReference = "";
            string strMaskedCardNumber = "";

            try
            {
                if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                    return RedirectToAction("CCFailure" + strURLSufix);

                string strReference = Request["ekashu_reference"];
                string strAuthCode = Request["ekashu_auth_code"];
                string strAuthResult = Request["ekashu_auth_result"];
                string strCardHash = Request["ekashu_card_hash"];
                strCardReference = Request["ekashu_card_reference"];
                string strCardScheme = Request["ekashu_card_scheme"];
                string strGatewayDate = Request["ekashu_date_time_local_fmt"];
                strMaskedCardNumber = Request["ekashu_masked_card_number"];
                string strTransactionId = Request["ekashu_transaction_id"];
                string strExpMonth = Request["ekashu_expires_end_month"];
                string strExpYear = Request["ekashu_expires_end_year"];

                DateTime? dtExpDate = null;
                if ((strExpMonth.Length == 2) && (strExpYear.Length == 4))
                {
                    dtExpDate = new DateTime(Convert.ToInt32(strExpYear), Convert.ToInt32(strExpMonth), 1).AddMonths(1).AddSeconds(-1);
                }

                if (CCSuccess(strReference,
                               strTransactionId,
                               null,
                               strGatewayDate,
                               strAuthCode,
                               strAuthResult,
                               "",
                               strCardHash,
                               strCardReference,
                               strCardScheme,
                               strMaskedCardNumber,
                               PaymentMeanRechargeStatus.Waiting_Commit,
                               dtExpDate,
                               null,
                               null))
                {
                    if (Session["ReturnToPermits"] == null)
                    {
                        Session["ReturnToPermits"] = false;
                    }

                    if ((bool)Session["ReturnToPermits"])
                    {
                        Session["ReturnToPermits"] = false;
                        return RedirectToAction("PayForPermits", "Account");
                    }
                    else
                    {
                        return View();
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CCSuccess Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CCSuccess: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));


            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InCreditCallPayment"] = null;
            return RedirectToAction("CCFailure" + strURLSufix);
        }

        [Authorize]
        public ActionResult CCSuccessINT()
        {
            return CCSuccess("INT");
        }

        [Authorize]
        public ActionResult CCSuccessPT()
        {
            return CCSuccess("PT");
        }


        [Authorize]
        public ActionResult CCFailure(string strURLSufix)
        {

            
            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            string strCardReference="";
            string strMaskedCardNumber = "";

            try
            {
                strCardReference = Request["ekashu_card_reference"];
                strMaskedCardNumber = Request["ekashu_masked_card_number"];
            }
            catch{}

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            try
            {
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)+"}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));
                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];

                Session["QuantityToRecharge"] = null;
                Session["QuantityToRechargeBase"] = null;
                Session["PercVAT1"] = null;
                Session["PercVAT2"] = null;
                Session["PercFEE"] = null;
                Session["PercFEETopped"] = null;
                Session["FixedFEE"] = null;
                Session["CurrencyToRecharge"] = null;
                Session["InCreditCallPayment"] = null;
                Session["OVERWRITE_CARD"] = false;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CCFailure Exception: {0}", e.Message));
            }

            return View();
        }

        [Authorize]
        public ActionResult CCFailureINT()
        {
            return CCFailure("INT");
        }


        [Authorize]
        public ActionResult CCFailurePT()
        {
            return CCFailure("PT");
        }
        
        
        [Authorize]
        public ActionResult CCCancel(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure" + strURLSufix);



            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            ViewData["oUser"] = oUser;
            ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
            ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
            ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)+"}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));
            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];



            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InCreditCallPayment"] = null;
            Session["OVERWRITE_CARD"] = false;


            return View();
        }


        [Authorize]
        public ActionResult CCCancelINT()
        {
            return CCCancel("INT");
        }


        [Authorize]
        public ActionResult CCCancelPT()
        {
            return CCCancel("PT");
        }




        [Authorize]
        public ActionResult CCPreapprovalPayRedirect(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            try
            {
                string requrl = "";
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
                {
                    requrl = Request.Url.ToString();
                }
                else
                {
                    requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
                } 
                
                string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCPreapprovalPaySuccess" + strURLSufix; ;
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                CardEasePayments cardPayment = new CardEasePayments();

                string strUserReference = null;
                string strAuthCode = null;
                string strAuthResult = null;
                string strGatewayDate = null;
                string strTransactionId = null;
                string strCardScheme = null;

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpCreditCall))
                                            .FirstOrDefault();
                }


                if (cardPayment.AutomaticPayment(oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                oUser.USR_EMAIL,
                                                Convert.ToDecimal(Session["QuantityToRecharge"].ToString(), numberFormatProvider),
                                                 Session["CurrencyToRecharge"].ToString(),
                                                 oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                 oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                 false,
                                                 out strUserReference,
                                                 out strAuthCode,
                                                 out strAuthResult,
                                                 out strGatewayDate,
                                                 out strCardScheme,
                                                 out strTransactionId))
                {
                    Session["CCUserReference"] = strUserReference;
                    Session["CCAuthCode"] = strAuthCode;
                    Session["CCAuthResult"] = strAuthResult;
                    Session["CCAuthResultDesc"] = "";
                    Session["CCGatewayDate"] = strGatewayDate;
                    Session["CCTransactionId"] = strTransactionId;
                    Session["CCCardScheme"] = strCardScheme;
                    Session["CCRechargeStatus"] = PaymentMeanRechargeStatus.Waiting_Commit;



                    return RedirectToAction("CCPreapprovalPaySuccess"+strURLSufix);
                }


            }
            catch (Exception e)
            {
                string msg = e.Message;

                return RedirectToAction("CCFailure" + strURLSufix);
            }


            return RedirectToAction("CCFailure" + strURLSufix);


        }

        [Authorize]
        public ActionResult CCPreapprovalPayRedirectINT()
        {
            return CCPreapprovalPayRedirect("INT");
        }

        [Authorize]
        public ActionResult CCPreapprovalPayRedirectPT()
        {
            return CCPreapprovalPayRedirect("PT");
        }


        [Authorize]
        public ActionResult CCPreapprovalPaySuccess(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure" + strURLSufix);

            if (Session["CCTransactionId"] == null)
                return RedirectToAction("CCFailure" + strURLSufix); 

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            try
            {
                if (CCPreapprovalSuccess())
                    return View();                
            }
                
            catch
            {

            }

            return RedirectToAction("CCFailure" + strURLSufix);


        }


        [Authorize]
        public ActionResult CCPreapprovalPaySuccessINT()
        {
            return CCPreapprovalPaySuccess("INT");
        }

        [Authorize]
        public ActionResult CCPreapprovalPaySuccessPT()
        {
            return CCPreapprovalPaySuccess("PT");
        }

        #endregion


        #region ICISA Payment
        [Authorize]
        public ActionResult CC2Redirect(string strURLSufix)
        {

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode((string)Session["CurrencyToRecharge"]));
            DateTime dtNow = DateTime.Now;
            DateTime dtUTCNow = DateTime.UtcNow;

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {

                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                        .FirstOrDefault();
            }


            string strTransactionId = null;
            string strOpReference = null;
            string errorMessage = null;
            string strCardHash = null;

            IECISAPayments cardPayment = new IECISAPayments();

            var uri = new Uri(Request.Url.AbsoluteUri);
            string strURLPath = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/"));
            string strLang = ((CultureInfo)Session["Culture"]).Name.Substring(0, 2);
            IECISAPayments.IECISAErrorCode eErrorCode;

            cardPayment.StartWebTransaction(oGatewayConfig.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_USER,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                            strURLPath + "/CC2Reply" + strURLSufix,
                                            strURLPath + "/CC2Reply" + strURLSufix,
                                            oUser.USR_EMAIL,
                                            strLang,
                                            iQuantityToRecharge,
                                            (string)Session["CurrencyToRecharge"],
                                            infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode((string)Session["CurrencyToRecharge"]),
                                            true,
                                            DateTime.Now,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strTransactionId,
                                            out strOpReference,
                                            out strCardHash);

            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
            {
                string errorCode = eErrorCode.ToString();

                m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                          errorCode, errorMessage));

                return RedirectToAction("CC2Failure" + strURLSufix);


            }
            else
            {
                string strRedirectURL = "";
                cardPayment.GetWebTransactionPaymentTypes(oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                        oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                        strTransactionId,
                                                        out eErrorCode,
                                                        out errorMessage,
                                                        out strRedirectURL);
                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                {
                    string errorCode = eErrorCode.ToString();

                    m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                              errorCode, errorMessage));
                    
                    return RedirectToAction("CC2Failure" + strURLSufix);

                }
                else
                {
                    customersRepository.StartRecharge(oGatewayConfig.CPTGC_ID,
                                                           oUser.USR_EMAIL,
                                                           dtUTCNow,
                                                           dtNow,
                                                           iQuantityToRecharge,
                                                           infraestructureRepository.GetCurrencyFromIsoCode((string)Session["CurrencyToRecharge"]),
                                                           "",
                                                           strOpReference,
                                                           strTransactionId,
                                                           "",
                                                           "",
                                                           "",
                                                           PaymentMeanRechargeStatus.Committed);

                    Session["cardHash"] = strCardHash;
                    Session["USER_ID"] = oUser.USR_ID;

                    return Redirect(strRedirectURL);
                }

            }

            return RedirectToAction("CC2Failure" + strURLSufix);
        }

        [Authorize]
        public ActionResult CC2RedirectINT()
        {
            return CC2Redirect("INT");
        }

        [Authorize]
        public ActionResult CC2RedirectPT()
        {
            return CC2Redirect("PT");
        }


        [Authorize]
        public ActionResult CC2Reply(string strURLSufix, string transactionId)
        {
            string strCardReference = "";
            string strMaskedCardNumber = "";

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            try
            {
                if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                    return RedirectToAction("CC2Failure" + strURLSufix);


                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                            .FirstOrDefault();
                }


                IECISAPayments cardPayment = new IECISAPayments();
                IECISAPayments.IECISAErrorCode eErrorCode;               
                DateTime? dtExpDate = null;
                DateTime? dtTransactionDate = null;
                string strExpMonth = "";
                string strExpYear = "";
                string errorMessage = "";
                string strOpReference = "";
                string strAuthCode = "";
                string strCFTransactionID = "";


                cardPayment.GetTransactionStatus(oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                 oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                 oGatewayConfig.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                 transactionId,
                                                out eErrorCode,
                                                out errorMessage,
                                                out strMaskedCardNumber,
                                                out strCardReference,
                                                out dtExpDate,
                                                out strExpMonth,
                                                out strExpYear,
                                                out dtTransactionDate,
                                                out strOpReference,
                                                out strCFTransactionID,
                                                out strAuthCode);
               
                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                {


                    customersRepository.FailedRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                    oUser.USR_EMAIL,
                                    transactionId,
                                    PaymentMeanRechargeStatus.Cancelled );

                    string errorCode = eErrorCode.ToString();
                    switch (eErrorCode)
                    {

                        case IECISAPayments.IECISAErrorCode.TransactionCancelled:
                        case IECISAPayments.IECISAErrorCode.TransactionCancelled2:
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}",
                               errorCode, errorMessage));

                              

                                return RedirectToAction("CC2Cancel" + strURLSufix);
                            }
                            break;

                        default:
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}",
                                errorCode, errorMessage));

                                return RedirectToAction("CC2Failure" + strURLSufix);
                            }
                            break;
                    }

                }
               


                Session["Sess_strMaskedCardNumber"] = strMaskedCardNumber;
                Session["Sess_strCardReference"] = strCardReference;


                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Reply: Exiting with CC2Success: errorCode={0}; Card Reference={1}; AuthCode={2}", eErrorCode.ToString(),strCardReference, strAuthCode));
             
                Session["Sess_strReference"] = strOpReference;
                Session["Sess_strTransactionId"] = transactionId;
                Session["Sess_strCFTransactionId"] = strCFTransactionID;
                Session["Sess_strGatewayDate"] = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                Session["Sess_strAuthCode"] = strAuthCode;
                Session["Sess_strAuthResult"] = "succeeded";
                Session["Sess_strAuthResultDesc"] = errorMessage;
                Session["Sess_strCardHash"] = Session["cardHash"].ToString();
                Session["Sess_strCardReference"] = strCardReference;
                Session["Sess_strCardScheme"] = "";
                Session["Sess_dtExpDate"] = dtExpDate;



                return RedirectToAction("CC2Success" + strURLSufix);


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            return RedirectToAction("CC2Failure" + strURLSufix);
        }

        [Authorize]
        public ActionResult CC2ReplyINT(string transactionId)
        {
            return CC2Reply("INT", transactionId);
        }

        [Authorize]
        public ActionResult CC2ReplyPT(string transactionId)
        {
            return CC2Reply("PT", transactionId);
        }


        [Authorize]
        public ActionResult CC2Success(string strURLSufix)
        {

            string strMaskedCardNumber = "";
            string strCardReference = "";

            try
            {
                if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                    return RedirectToAction("CC2Failure" + strURLSufix);
           
                USER oUser = GetUserFromSession();

                if (oUser == null)
                {
                    return LogOff();
                }

                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                string strReference = Session["Sess_strReference"].ToString();
                string strTransactionId = Session["Sess_strTransactionId"].ToString();
                string strCFTransactionId = Session["Sess_strCFTransactionId"].ToString();
                string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
                string strAuthCode = Session["Sess_strAuthCode"].ToString();
                string strAuthResult = Session["Sess_strAuthResult"].ToString();
                string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
                string strCardHash = Session["Sess_strCardHash"].ToString();
                strCardReference = Session["Sess_strCardReference"].ToString();
                string strCardScheme = Session["Sess_strCardScheme"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
                DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                            .FirstOrDefault();
                }


                customersRepository.CompleteStartRecharge(oGatewayConfig.CPTGC_ID,
                                                          oUser.USR_EMAIL,
                                                          strTransactionId,
                                                          strAuthResult,
                                                          strCFTransactionId,                                                          
                                                          strGatewayDate,
                                                          strAuthCode,
                                                          PaymentMeanRechargeStatus.Committed);

                if (CCSuccess(strReference,
                               strTransactionId,
                               strCFTransactionId,
                               strGatewayDate,
                               strAuthCode,
                               strAuthResult,
                               strAuthResultDesc,
                               strCardHash,
                               strCardReference,
                               strCardScheme,
                               strMaskedCardNumber,
                               PaymentMeanRechargeStatus.Committed,
                               dtExpDate,
                               null,
                               null))
                {
                    if (Session["ReturnToPermits"] == null)
                    {
                        Session["ReturnToPermits"] = false;
                    }

                    if ((bool)Session["ReturnToPermits"])
                    {
                        Session["ReturnToPermits"] = false;
                        return RedirectToAction("PayForPermits", "Account");
                    }
                    else
                    {
                        return View();
                    }                    
                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            Session["Sess_strReference"] = null;
            Session["Sess_strTransactionId"] = null;
            Session["Sess_strCFTransactionId"] = null;
            Session["Sess_strGatewayDate"] = null;
            Session["Sess_strAuthCode"] = null;
            Session["Sess_strAuthResult"] = null;
            Session["Sess_strAuthResultDesc"] = null;
            Session["Sess_strCardHash"] = null;
            Session["Sess_strCardReference"] = null;
            Session["Sess_strCardScheme"] = "";
            Session["Sess_strMaskedCardNumber"] = null;
            Session["Sess_dtExpDate"] = null;

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InIECISAPayment"] = null;
            Session["cardHash"] = null;
            return RedirectToAction("CC2Failure" + strURLSufix);
        }

        [Authorize]
        public ActionResult CC2SuccessINT()
        {
            return CC2Success("INT");
        }

        [Authorize]
        public ActionResult CC2SuccessPT()
        {
            return CC2Success("PT");
        }


        [Authorize]
        public ActionResult CC2Failure(string strURLSufix)
        {


            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            string strCardReference = "";
            string strMaskedCardNumber = "";

            try
            {
                strCardReference = Session["Sess_strCardReference"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
            }
            catch { }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Failure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            try
            {
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)+"}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));
                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];


                Session["Sess_strCardReference"] = null;
                Session["Sess_strMaskedCardNumber"] = null;

                Session["QuantityToRecharge"] = null;
                Session["QuantityToRechargeBase"] = null;
                Session["PercVAT1"] = null;
                Session["PercVAT2"] = null;
                Session["PercFEE"] = null;
                Session["PercFEETopped"] = null;
                Session["FixedFEE"] = null;
                Session["CurrencyToRecharge"] = null;
                Session["InIECISAPayment"] = null;
                Session["OVERWRITE_CARD"] = false;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Failure Exception: {0}", e.Message));
            }

            return View();
        }

        [Authorize]
        public ActionResult CC2FailureINT()
        {
            return CC2Failure("INT");
        }


        [Authorize]
        public ActionResult CC2FailurePT()
        {
            return CC2Failure("PT");
        }


        [Authorize]
        public ActionResult CC2Cancel(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure" + strURLSufix);



            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            ViewData["oUser"] = oUser;
            ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
            ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
            ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)+"}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));
            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];



            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InIECISAPayment"] = null;
            Session["OVERWRITE_CARD"] = false;


            return View();
        }


        [Authorize]
        public ActionResult CC2CancelINT()
        {
            return CC2Cancel("INT");
        }


        [Authorize]
        public ActionResult CC2CancelPT()
        {
            return CC2Cancel("PT");
        }




        [Authorize]
        public ActionResult CC2PreapprovalPayRedirect(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            try
            {

                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                string requrl = "";
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
                {
                    requrl = Request.Url.ToString();
                }
                else
                {
                    requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
                }

                string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CC2PreapprovalPaySuccess" + strURLSufix; ;

                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                IECISAPayments cardPayment = new IECISAPayments();

                string strAuthCode = null;
                string strAuthResult = null;
                string strTransactionId = null;


                int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), numberFormatProvider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode((string)Session["CurrencyToRecharge"]));
                string strOpReference = IECISAPayments.UserReference();
                DateTime dtNow = DateTime.Now;

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                            .FirstOrDefault();
                }



               
                string errorMessage = null;                

                var uri = new Uri(Request.Url.AbsoluteUri);
                string strURLPath = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/"));
                string strLang = ((CultureInfo)Session["Culture"]).Name.Substring(0, 2);
                IECISAPayments.IECISAErrorCode eErrorCode;
                DateTime dtUTCNow = DateTime.UtcNow;


                cardPayment.StartAutomaticTransaction(oGatewayConfig.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_USER,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                                oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                oUser.USR_EMAIL,
                                                iQuantityToRecharge,
                                                (string)Session["CurrencyToRecharge"],
                                                infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode((string)Session["CurrencyToRecharge"]),
                                                dtNow,
                                                out eErrorCode,
                                                out errorMessage,
                                                out strTransactionId,
                                                out strOpReference);

                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                {
                    string errorCode = eErrorCode.ToString();

                    m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                              errorCode, errorMessage));

                    return RedirectToAction("CC2Failure" + strURLSufix);


                }
                else
                {
                    string strRedirectURL = "";
                    cardPayment.GetWebTransactionPaymentTypes(oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                            strTransactionId,
                                                            out eErrorCode,
                                                            out errorMessage,
                                                            out strRedirectURL);
                    if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                    {
                        string errorCode = eErrorCode.ToString();

                        m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                  errorCode, errorMessage));

                        return RedirectToAction("CC2Failure" + strURLSufix);

                    }
                    else
                    {


                        customersRepository.StartRecharge(oGatewayConfig.CPTGC_ID,
                                                           oUser.USR_EMAIL,
                                                           dtUTCNow,
                                                           dtNow,
                                                           iQuantityToRecharge,
                                                           infraestructureRepository.GetCurrencyFromIsoCode((string)Session["CurrencyToRecharge"]),
                                                           "",
                                                           strOpReference,
                                                           strTransactionId,
                                                           "",
                                                           "",
                                                           "",
                                                           PaymentMeanRechargeStatus.Committed);

                        DateTime? dtTransactionDate=null;
                        string strCFTransactionID = null;
                        cardPayment.CompleteAutomaticTransaction(oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                               oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                               oGatewayConfig.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                               strTransactionId,
                                              out eErrorCode,
                                              out errorMessage,
                                              out dtTransactionDate,
                                              out strCFTransactionID,
                                              out strAuthCode);


                        if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                        {
                            string errorCode = eErrorCode.ToString();

                            m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                      errorCode, errorMessage));

                            return RedirectToAction("CC2Failure" + strURLSufix);

                        }
                        else
                        {

                            strAuthResult = "succeeded";
                            customersRepository.CompleteStartRecharge(oGatewayConfig.CPTGC_ID,
                                                          oUser.USR_EMAIL,
                                                          strTransactionId,
                                                          strAuthResult,
                                                          strCFTransactionID,
                                                          dtTransactionDate.Value.ToString("HHmmssddMMyyyy"),
                                                          strAuthCode,
                                                          PaymentMeanRechargeStatus.Committed);
                          


                            Session["CCUserReference"] = strOpReference;
                            Session["CCAuthCode"] = strAuthCode;
                            Session["CCAuthResult"] = strAuthResult;
                            Session["CCAuthResultDesc"] = errorMessage;
                            Session["CCGatewayDate"] = dtTransactionDate.Value.ToString("HHmmssddMMyyyy"); 
                            Session["CCTransactionId"] = strTransactionId;
                            Session["CCCFTransactionId"] = strCFTransactionID;
                            Session["CCCardScheme"] = "";
                            Session["CCRechargeStatus"] = PaymentMeanRechargeStatus.Committed;


                            return RedirectToAction("CC2PreapprovalPaySuccess" + strURLSufix);
                        }
                    }

                }

                return RedirectToAction("CC2Failure" + strURLSufix);


                

            }
            catch (Exception e)
            {
                string msg = e.Message;

                return RedirectToAction("CC2Failure" + strURLSufix);
            }


            return RedirectToAction("CC2Failure" + strURLSufix);


        }

        [Authorize]
        public ActionResult CC2PreapprovalPayRedirectINT()
        {
            return CC2PreapprovalPayRedirect("INT");
        }

        [Authorize]
        public ActionResult CC2PreapprovalPayRedirectPT()
        {
            return CC2PreapprovalPayRedirect("PT");
        }


        [Authorize]
        public ActionResult CC2PreapprovalPaySuccess(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure" + strURLSufix);

            if (Session["CCTransactionId"] == null)
                return RedirectToAction("CC2Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            try
            {
                if (CCPreapprovalSuccess())
                    return View();
            }

            catch
            {

            }

            return RedirectToAction("CC2Failure" + strURLSufix);


        }


        [Authorize]
        public ActionResult CC2PreapprovalPaySuccessINT()
        {
            return CC2PreapprovalPaySuccess("INT");
        }

        [Authorize]
        public ActionResult CC2PreapprovalPaySuccessPT()
        {
            return CC2PreapprovalPaySuccess("PT");
        }

        #endregion


        #region Stripe Payment
        [Authorize]
        public ActionResult CC3Redirect(string strURLSufix)
        {

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode((string)Session["CurrencyToRecharge"]));
            DateTime dtNow = DateTime.Now;


            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {

                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                        .FirstOrDefault();
            }




            string strImageURL = DEFAULT_STRIPE_IMAGE_URL;

            if (!string.IsNullOrEmpty(oGatewayConfig.STRIPE_CONFIGURATION.STRCON_IMAGE_URL))
            {
                strImageURL = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_IMAGE_URL;
            }

            ViewData["email"] = oUser.USR_EMAIL;
            ViewData["amount"] = iQuantityToRecharge.ToString(); ;
            ViewData["currency"] = (string)Session["CurrencyToRecharge"];
            ViewData["key"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_DATA_KEY;
            ViewData["commerceName"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_COMMERCE_NAME;
            ViewData["panelLabel"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_PANEL_LABEL;
            ViewData["description"] = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");
            ViewData["image"] = strImageURL;
            

            return View();
        }

        [Authorize]
        public ActionResult CC3RedirectINT()
        {
            return CC3Redirect("INT");
        }

        [Authorize]
        public ActionResult CC3RedirectPT()
        {
            return CC3Redirect("PT");
        }


        [Authorize]

        public ActionResult CC3Reply(string strURLSufix, StripeResponseModel oModel)
        {
            string strPAN = "";
            string strCardToken = "";

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            try
            {
                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                if (!Convert.ToBoolean(Session["InStripePayment"]))
                    return RedirectToAction("CC3Failure" + strURLSufix);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                            .FirstOrDefault();
                }



                if (!string.IsNullOrEmpty(oModel.stripeErrorCode) && oModel.stripeErrorCode == "window_closed")
                {
                    return RedirectToAction("CC3Cancel" + strURLSufix);

                }
                else
                {

                    if (!string.IsNullOrEmpty(oModel.stripeToken))
                    {

                        strCardToken = oModel.stripeToken;


                        if (oModel.stripeEmail == oUser.USR_EMAIL)
                        {
                            NumberFormatInfo provider = new NumberFormatInfo();
                            provider.NumberDecimalSeparator = ".";
                            string strCustomerId = "";
                            int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode((string)Session["CurrencyToRecharge"]));

                            string result = "";
                            string errorMessage = "";
                            string errorCode = "";
                            string strChargeID = "";
                            string strCardScheme = "";
                            string strStripeDateTime = "";

                            string strExpirationDateMonth = "";
                            string strExpirationDateYear = "";


                            if (StripePayments.PerformCharge(oGatewayConfig.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                            oModel.stripeEmail,
                                                            strCardToken,
                                                            ref strCustomerId,
                                                            iQuantityToRecharge,
                                                            Session["CurrencyToRecharge"].ToString(),
                                                            false,
                                                            out result,
                                                            out errorCode,
                                                            out errorMessage,
                                                            out strCardScheme,
                                                            out strPAN,
                                                            out strExpirationDateMonth,
                                                            out strExpirationDateYear,
                                                            out strChargeID,
                                                            out strStripeDateTime))
                            {

                                DateTime dtExpDate = DateTime.UtcNow;
                                if ((strExpirationDateMonth.Length == 2) && (strExpirationDateYear.Length == 4))
                                {
                                    dtExpDate = new DateTime(Convert.ToInt32(strExpirationDateYear), Convert.ToInt32(strExpirationDateMonth), 1).AddMonths(1).AddSeconds(-1);
                                }

                                Session["Sess_strMaskedCardNumber"] = strPAN;
                                Session["Sess_strCardReference"] = oModel.stripeToken;


                                Session["Sess_strReference"] = strChargeID;
                                Session["Sess_strTransactionId"] = strChargeID;
                                Session["Sess_strGatewayDate"] = DateTime.ParseExact(strStripeDateTime, "HHmmssddMMyy",
                                                                CultureInfo.InvariantCulture);
                                Session["Sess_strAuthCode"] = "";
                                Session["Sess_strAuthResult"] = "succeeded";
                                Session["Sess_strAuthResultDesc"] = "";
                                Session["Sess_strCardHash"] = strCustomerId;
                                Session["Sess_strCardScheme"] = strCardScheme;
                                Session["Sess_dtExpDate"] = dtExpDate;


                                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Success: Email={0}; ChargeID={1}; PAN={2}", oModel.stripeEmail, strChargeID, strPAN));
                                return RedirectToAction("CC3Success" + strURLSufix);

                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Failure: Error Performing charge Email={0}; ChargeID={1}; PAN={2}; Error={3}; ErrorMsg={4}", oModel.stripeEmail, strChargeID, strPAN, errorCode, errorMessage));
                            }

                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Failure: Email not match Email={0} | UserEMail = {1}", oModel.stripeEmail, oUser.USR_EMAIL));
                        }

                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Failure: Empty Token Received Email={0} | UserEMail = {1}", oModel.stripeEmail, oUser.USR_EMAIL));
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Success Exception: {0}", e.Message));
            }

            return RedirectToAction("CC3Failure" + strURLSufix);
        }

        [Authorize]
        public ActionResult CC3ReplyINT(StripeResponseModel oModel)
        {
            return CC3Reply("INT", oModel);
        }

        [Authorize]
        public ActionResult CC3ReplyPT(StripeResponseModel oModel)
        {
            return CC3Reply("PT",oModel);
        }


        [Authorize]
        public ActionResult CC3Success(string strURLSufix)
        {

            string strMaskedCardNumber = "";
            string strCardReference = "";

            try
            {
                if (!Convert.ToBoolean(Session["InStripePayment"]))
                    return RedirectToAction("CC3Failure" + strURLSufix);

                string strReference = Session["Sess_strReference"].ToString() ;
                string strTransactionId = Session["Sess_strTransactionId"].ToString();
                string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
                string strAuthCode = Session["Sess_strAuthCode"].ToString();
                string strAuthResult = Session["Sess_strAuthResult"].ToString();
                string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
                string strCardHash = Session["Sess_strCardHash"].ToString();
                strCardReference = Session["Sess_strCardReference"].ToString();
                string strCardScheme = Session["Sess_strCardScheme"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
                DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];


                if ( CCSuccess(strReference,
                               strTransactionId,
                               null,
                               strGatewayDate,
                               strAuthCode,
                               strAuthResult,
                               strAuthResultDesc,
                               strCardHash,
                               strCardReference,
                               strCardScheme,
                               strMaskedCardNumber,
                               PaymentMeanRechargeStatus.Waiting_Commit,
                               dtExpDate,
                               null,
                               null))
                {
                    if (Session["ReturnToPermits"] == null)
                    {
                        Session["ReturnToPermits"] = false;
                    }

                    if ((bool)Session["ReturnToPermits"])
                    {
                        Session["ReturnToPermits"] = false;
                        return RedirectToAction("PayForPermits", "Account");
                    }
                    else
                    {
                        return View();
                    }
                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Success Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Success: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            Session["Sess_strReference"] = null;
            Session["Sess_strTransactionId"] = null;
            Session["Sess_strGatewayDate"] = null;
            Session["Sess_strAuthCode"] = null;
            Session["Sess_strAuthResult"] = null;
            Session["Sess_strAuthResultDesc"] = null;
            Session["Sess_strCardHash"] = null;
            Session["Sess_strCardReference"] = null;
            Session["Sess_strCardScheme"] = "";
            Session["Sess_strMaskedCardNumber"] = null;
            Session["Sess_dtExpDate"] = null;

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InStripePayment"] = null;
            return RedirectToAction("CC3Failure" + strURLSufix);
        }

        [Authorize]
        public ActionResult CC3SuccessINT()
        {
            return CC3Success("INT");
        }

        [Authorize]
        public ActionResult CC3SuccessPT()
        {
            return CC3Success("PT");
        }


        [Authorize]
        public ActionResult CC3Failure(string strURLSufix)
        {


            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            string strCardReference = "";
            string strMaskedCardNumber = "";

            try
            {
                strCardReference = Session["Sess_strCardReference"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
            }
            catch { }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Failure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            try
            {
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)+"}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));
                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];


                Session["Sess_strCardReference"] = null;
                Session["Sess_strMaskedCardNumber"] = null;

                Session["QuantityToRecharge"] = null;
                Session["QuantityToRechargeBase"] = null;
                Session["PercVAT1"] = null;
                Session["PercVAT2"] = null;
                Session["PercFEE"] = null;
                Session["PercFEETopped"] = null;
                Session["FixedFEE"] = null;
                Session["CurrencyToRecharge"] = null;
                Session["InStripePayment"] = null;
                Session["OVERWRITE_CARD"] = false;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Failure Exception: {0}", e.Message));
            }

            return View();
        }

        [Authorize]
        public ActionResult CC3FailureINT()
        {
            return CC3Failure("INT");
        }


        [Authorize]
        public ActionResult CC3FailurePT()
        {
            return CC3Failure("PT");
        }


        [Authorize]
        public ActionResult CC3Cancel(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure" + strURLSufix);



            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

            ViewData["oUser"] = oUser;
            ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
            ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
            ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)+"}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));
            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];



            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InStripePayment"] = null;
            Session["OVERWRITE_CARD"] = false;


            return View();
        }


        [Authorize]
        public ActionResult CC3CancelINT()
        {
            return CC3Cancel("INT");
        }


        [Authorize]
        public ActionResult CC3CancelPT()
        {
            return CC3Cancel("PT");
        }




        [Authorize]
        public ActionResult CC3PreapprovalPayRedirect(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            try
            {
                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                string requrl = "";
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
                {
                    requrl = Request.Url.ToString();
                }
                else
                {
                    requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
                }

                string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CC3PreapprovalPaySuccess" + strURLSufix; ;

                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;


                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                            .FirstOrDefault();
                }

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                
                int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode((string)Session["CurrencyToRecharge"]));

                string result = "";
                string errorMessage = "";
                string errorCode = "";
                string strChargeID = "";
                string strCardScheme = "";
                string strStripeDateTime = "";
                string strCustomerId = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;
                string strExpirationDateMonth = "";
                string strExpirationDateYear = "";
                string strPAN = "";


                
                if (StripePayments.PerformCharge(oGatewayConfig.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                oUser.USR_EMAIL,
                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                ref strCustomerId,
                                                iQuantityToRecharge,
                                                Session["CurrencyToRecharge"].ToString(),
                                                false,
                                                out result,
                                                out errorCode,
                                                out errorMessage,
                                                out strCardScheme,
                                                out strPAN,
                                                out strExpirationDateMonth,
                                                out strExpirationDateYear,
                                                out strChargeID,
                                                out strStripeDateTime))
                {

                    DateTime dtExpDate = DateTime.UtcNow;
                    if ((strExpirationDateMonth.Length == 2) && (strExpirationDateYear.Length == 4))
                    {
                        dtExpDate = new DateTime(Convert.ToInt32(strExpirationDateYear), Convert.ToInt32(strExpirationDateMonth), 1).AddMonths(1).AddSeconds(-1);
                    }

                    Session["Sess_strMaskedCardNumber"] = strPAN;
                    Session["Sess_strCardReference"] = oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE;


                    Session["Sess_strReference"] = strChargeID;
                    Session["Sess_strTransactionId"] = strChargeID;
                    Session["Sess_strGatewayDate"] = DateTime.ParseExact(strStripeDateTime, "HHmmssddMMyy",
                                                    CultureInfo.InvariantCulture);
                    Session["Sess_strAuthCode"] = "";
                    Session["Sess_strAuthResult"] = "succeeded";
                    Session["Sess_strAuthResultDesc"] = "";
                    Session["Sess_strCardHash"] = strCustomerId;
                    Session["Sess_strCardScheme"] = strCardScheme;
                    Session["Sess_dtExpDate"] = dtExpDate;


                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3PreapprovalPayRedirect: Exiting with CC3PreapprovalPayRedirect: Email={0}; ChargeID={1}; PAN={2}", oUser.USR_EMAIL, strChargeID, strPAN));
                    return RedirectToAction("CC3PreapprovalPaySuccess" + strURLSufix);

                }
                else
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3PreapprovalPayRedirect: Exiting with CC3Failure: Error Performing charge Email={0}; ChargeID={1}; PAN={2}; Error={3}; ErrorMsg={4}", oUser.USR_EMAIL, strChargeID, strPAN, errorCode, errorMessage));
                }
            

            }
            catch (Exception e)
            {
                string msg = e.Message;

                return RedirectToAction("CC3Failure" + strURLSufix);
            }


            return RedirectToAction("CC3Failure" + strURLSufix);


        }

        [Authorize]
        public ActionResult CC3PreapprovalPayRedirectINT()
        {
            return CC3PreapprovalPayRedirect("INT");
        }

        [Authorize]
        public ActionResult CC3PreapprovalPayRedirectPT()
        {
            return CC3PreapprovalPayRedirect("PT");
        }


        [Authorize]
        public ActionResult CC3PreapprovalPaySuccess(string strURLSufix)
        {
            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure" + strURLSufix);

            if (Session["Sess_strTransactionId"] == null)
                return RedirectToAction("CC3Failure" + strURLSufix);

            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }

            try
            {
                string strReference = Session["Sess_strReference"].ToString();
                string strTransactionId = Session["Sess_strTransactionId"].ToString();
                string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
                string strAuthCode = Session["Sess_strAuthCode"].ToString();
                string strAuthResult = Session["Sess_strAuthResult"].ToString();
                string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
                string strCardHash = Session["Sess_strCardHash"].ToString();
                string strCardReference = Session["Sess_strCardReference"].ToString();
                string strCardScheme = Session["Sess_strCardScheme"].ToString();
                string strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
                DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];


                if (CCSuccess(strReference,
                               strTransactionId,
                               null,
                               strGatewayDate,
                               strAuthCode,
                               strAuthResult,
                               strAuthResultDesc,
                               strCardHash,
                               strCardReference,
                               strCardScheme,
                               strMaskedCardNumber,
                               PaymentMeanRechargeStatus.Waiting_Commit,
                               dtExpDate,
                               null,
                               null))
                {
                    return View();
                }
            }

            catch
            {

            }

            return RedirectToAction("CC3Failure" + strURLSufix);


        }


        [Authorize]
        public ActionResult CC3PreapprovalPaySuccessINT()
        {
            return CC3PreapprovalPaySuccess("INT");
        }

        [Authorize]
        public ActionResult CC3PreapprovalPaySuccessPT()
        {
            return CC3PreapprovalPaySuccess("PT");
        }

        #endregion


        //#region Paypal Payment
        //[Authorize]
        //public ActionResult PaypalRedirect(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 

        //    USER oUser= GetUserFromSession();

        //    if (oUser==null)
        //    {
        //        return LogOff();
        //    }

        //    try
        //    {

        //        string requrl = "";
        //        if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
        //        {
        //            requrl = Request.Url.ToString();
        //        }
        //        else
        //        {
        //            requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
        //        } 
                
        //        string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PaypalReturn" + strURLSufix;
        //        string cancelURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PaypalCancel" + strURLSufix;

        //        //ResourceExtension.GetLiteral("Account_Recharge_ItemDescription;
        //        AddressType address = new AddressType()
        //        {
       

        //            Name = oUser.CUSTOMER.CUS_FIRST_NAME,
        //            Street1 = oUser.CUSTOMER.CUS_STREET,
        //            Street2 = oUser.CUSTOMER.CUS_STREE_NUMBER + " " +
        //                      oUser.CUSTOMER.CUS_LEVEL_NUM + " " +
        //                      oUser.CUSTOMER.CUS_DOOR,
        //            CityName = oUser.CUSTOMER.CUS_CITY,
        //            StateOrProvince = oUser.CUSTOMER.CUS_STATE,
        //            PostalCode = oUser.CUSTOMER.CUS_ZIPCODE,
        //            Country = CountryCodeType.ES,
        //            Phone = infraestructureRepository.GetCountryTelephonePrefix((int)oUser.USR_MAIN_TEL_COUNTRY)+oUser.USR_MAIN_TEL
                    
        //        };              
                
        //        // Execute the API operation and obtain the response.
        //        SetExpressCheckoutResponseType pp_response = null;

        //        if (PaypalPayments.ExpressCheckoutPassOne(address,
        //                                                  Session["QuantityToRecharge"].ToString(),
        //                                                  Session["CurrencyToRecharge"].ToString(),
        //                                                  cancelURL,
        //                                                  returnURL,
        //                                                  ResourceExtension.GetLiteral("Account_Recharge_ItemDescription"),
        //                                                  out pp_response))
        //        {
        //            Response.Redirect(string.Format("{0}_express-checkout&token={1}",
        //                 ConfigurationManager.AppSettings["PAYPAL_API_REDIRECT_URL"], pp_response.Token));

        //        }
        //    }
        //    catch
        //    {

        //    }
            
            
        //    return RedirectToAction("PaypalFailure" + strURLSufix); 
           

        //}


        //[Authorize]
        //public ActionResult PaypalRedirectINT()
        //{
        //    return PaypalRedirect("INT");
        //}

        //[Authorize]
        //public ActionResult PaypalRedirectPT()
        //{
        //    return PaypalRedirect("PT");
        //}



        //[Authorize]
        //public ActionResult PaypalReturn(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 


        //    string token = Request.QueryString["token"];

        //    if (token != null)
        //    {
        //        Session["PaypalToken2"] = token;
        //        return RedirectToAction("PaypalConfirm" + strURLSufix); 

        //    }
        //    else
        //    {
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 

        //    }


        //}


        //[Authorize]
        //public ActionResult PaypalReturnINT()
        //{
        //    return PaypalReturn("INT");
        //}


        //[Authorize]
        //public ActionResult PaypalReturnPT()
        //{
        //    return PaypalReturn("PT");
        //}


        //[Authorize]
        //public ActionResult PaypalFailure(string strURLSufix)
        //{
        //    Session["PaypalToken2"] = null;
        //    Session["PaypalToken3"] = null;
        //    Session["PaypalPayerID"] = null;
        //    Session["PaypalPreapprovalKey"] = null;
        //    Session["QuantityToRecharge"] = null;
        //    Session["QuantityToRechargeBase"] = null;
        //    Session["PercVAT1"] = null;
        //    Session["PercVAT2"] = null;
        //    Session["PercFEE"] = null;
        //    Session["PercFEETopped"] = null;
        //    Session["FixedFEE"] = null;
        //    Session["CurrencyToRecharge"] = null;
        //    Session["InPaypalPayment"] = false;

        //    USER oUser = GetUserFromSession();

        //    if (oUser == null)
        //    {
        //        return LogOff();
        //    }

        //    decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //    CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //    ViewData["oUser"] = oUser;
        //    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //    ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;


        //    return View();
        //}



        //[Authorize]
        //public ActionResult PaypalFailureINT()
        //{
        //    return PaypalFailure("INT");
        //}

        //[Authorize]
        //public ActionResult PaypalFailurePT()
        //{
        //    return PaypalFailure("PT");
        //}



        //[Authorize]
        //public ActionResult PaypalConfirm(string strURLSufix)
        //{

        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix);

        //    if (Session["PaypalToken2"] == null)
        //        return RedirectToAction("PaypalFailure" + strURLSufix);


        //    USER oUser = GetUserFromSession();

        //    if (oUser == null)
        //    {
        //        return LogOff();
        //    }


        //    try
        //    {
        //        decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //        ViewData["oUser"] = oUser;
        //        ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //        ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //        ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //        GetExpressCheckoutDetailsResponseType pp_response = null;

        //        if (PaypalPayments.ExpressCheckoutPassTwo(Session["PaypalToken2"].ToString(), 
        //                                                  out pp_response))
        //        {


        //            ViewData["PayerName"] = pp_response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.FirstName;
        //            ViewData["PayerLastName"] = pp_response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.LastName;
        //            ViewData["PayerQuantity"]= pp_response.GetExpressCheckoutDetailsResponseDetails.PaymentDetails[0].OrderTotal.Value;
        //            ViewData["PayerCurrency"]= pp_response.GetExpressCheckoutDetailsResponseDetails.PaymentDetails[0].OrderTotal.currencyID;
        //            ViewData["PayerCurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //            Session["PaypalPayerID"] = pp_response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;
        //            Session["PaypalToken2"] = null;
        //            Session["PaypalToken3"] = pp_response.GetExpressCheckoutDetailsResponseDetails.Token;            

        //            return View();
        //        }
        //    }
        //    catch
        //    {

        //    }



        //    return RedirectToAction("PaypalFailure" + strURLSufix); 
        //}



        //[Authorize]
        //public ActionResult PaypalConfirmINT()
        //{
        //    return PaypalConfirm("INT");
        //}



        //[Authorize]
        //public ActionResult PaypalConfirmPT()
        //{
        //    return PaypalConfirm("PT");
        //}

        //[Authorize]
        //public ActionResult PaypalCancel(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix);

        //    USER oUser = GetUserFromSession();

        //    if (oUser == null)
        //    {
        //        return LogOff();
        //    }

        //    decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //    CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //    ViewData["oUser"] = oUser;
        //    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //    ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //    return View();
        //}


        //[Authorize]
        //public ActionResult PaypalCancelINT()
        //{
        //    return PaypalCancel("INT");
        //}



        //[Authorize]
        //public ActionResult PaypalCancelPT()
        //{
        //    return PaypalCancel("PT");
        //}


        //[HttpPost]
        //[Authorize]
        //public ActionResult PaypalSuccess(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix);

        //    if (Session["PaypalToken3"] == null)
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 

        //    USER oUser = GetUserFromSession();

        //    if (oUser == null)
        //    {
        //        return LogOff();
        //    }

        //    try
        //    {
        //        decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //        DoExpressCheckoutPaymentResponseType pp_response = null;

        //        if (PaypalPayments.ExpressCheckoutConfirm(Session["PaypalPayerID"].ToString(),
        //                                                  Session["PaypalToken3"].ToString(),
        //                                                  Session["QuantityToRecharge"].ToString(),
        //                                                  Session["CurrencyToRecharge"].ToString(),
        //                                                  out pp_response))
        //        {

        //            string AuthorizationCode = pp_response.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].TransactionID;
        //            DateTime TimeStamp = pp_response.Timestamp;

        //            ViewData["TransactionID"] = AuthorizationCode;
        //            ViewData["TransactionDate"] = TimeStamp.ToString();

        //            ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
        //            NumberFormatInfo provider = new NumberFormatInfo();
        //            provider.NumberDecimalSeparator = ".";
        //            decimal? dRechargeId = null;

        //            decimal dPercVAT1 = (Session["PercVAT1"] != null ? Convert.ToDecimal(Session["PercVAT1"].ToString(), provider) : 0);
        //            decimal dPercVAT2 = (Session["PercVAT2"] != null ? Convert.ToDecimal(Session["PercVAT2"].ToString(), provider) : 0);
        //            decimal dPercFEE = (Session["PercFEE"] != null ? Convert.ToDecimal(Session["PercFEE"].ToString(), provider) : 0);
        //            int iPercFEETopped = (Session["PercFEETopped"] != null ? Convert.ToInt32(Session["PercFEETopped"]) : 0);
        //            int iFixedFEE = (Session["FixedFEE"] != null ? Convert.ToInt32(Session["FixedFEE"]) : 0);
        //            int iQuantity = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRechargeBase"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE));

        //            int iPartialVAT1;
        //            int iPartialPercFEE;
        //            int iPartialFixedFEE;

        //            int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

        //            PaymentMeanRechargeCreationType rechargeCreationType=PaymentMeanRechargeCreationType.pmrctRegularRecharge;

        //            if (Session["RechargeCreationType"] != null)
        //            {
        //                rechargeCreationType = (PaymentMeanRechargeCreationType)Session["RechargeCreationType"];
        //            }


        //            if (customersRepository.RechargeUserBalance(ref oUser, 
        //                                    null,
        //                                    Convert.ToInt32(MobileOS.Web),
        //                                    (chargeType == ChargeOperationsType.BalanceRecharge),
        //                                    iQuantity,
        //                                    dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
        //                                    infraestructureRepository.GetCurrencyFromIsoCode(Session["CurrencyToRecharge"].ToString()),
        //                                    (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE,
        //                                    PaymentMeanRechargeStatus.Committed,
        //                                    rechargeCreationType,
        //                                    //0,
        //                                    null,
        //                                    AuthorizationCode,
        //                                    null,
        //                                    TimeStamp.ToString(),
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    null,
        //                                    (string)Session["PaypalToken3"],
        //                                    (string)Session["PaypalPayerID"],
        //                                    null,
        //                                    false,
        //                                    null, null,null,
        //                                    infraestructureRepository,
        //                                    out dRechargeId))
                                            
        //            {
        //                Session["USER_ID"] = oUser.USR_ID;
        //            }

        //            ViewData["oUser"] = oUser;
        //            ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //            ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //            ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //            ViewData["PayerQuantity"] = Convert.ToDouble(oUser.USR_BALANCE);
        //            ViewData["PayerCurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //            Session["PaypalToken2"] = null;
        //            Session["PaypalToken3"] = null;
        //            Session["PaypalPayerID"] = null;
        //            Session["QuantityToRecharge"] = null;
        //            Session["QuantityToRechargeBase"] = null;
        //            Session["PercVAT1"] = null;
        //            Session["PercVAT2"] = null;
        //            Session["PercFEE"] = null;
        //            Session["PercFEETopped"] = null;
        //            Session["FixedFEE"] = null;
        //            Session["CurrencyToRecharge"] = null;
        //            Session["InPaypalPayment"] = false;
        //            Session["RechargeCreationType"] = null;

        //            SendRechargeEmail(ref oUser, dRechargeId);


        //            return View();

        //        }

        //    }
        //    catch
        //    {

        //    }

        //    return RedirectToAction("PaypalFailure" + strURLSufix); 
        //}

        //[HttpPost]
        //[Authorize]
        //public ActionResult PaypalSuccessINT()
        //{
        //    return PaypalSuccess("INT");
        //}

        //[HttpPost]
        //[Authorize]
        //public ActionResult PaypalSuccessPT()
        //{
        //    return PaypalSuccess("PT");
        //}



        //[Authorize]
        //public ActionResult PaypalPreapprovalRedirect(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 

        //    USER oUser= GetUserFromSession();

        //    if (oUser==null)
        //    {
        //        return LogOff();
        //    }

        //    try
        //    {
        //        decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //        ViewData["oUser"] = oUser;
        //        ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //        ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //        ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //        Session["PaypalPreapprovalKey"] = null;
        //        string requrl = "";
        //        if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
        //        {
        //            requrl = Request.Url.ToString();
        //        }
        //        else
        //        {
        //            requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
        //        } 
                
        //        string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PayPalPreapprovalSuccess" + strURLSufix;
        //        string cancelURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PaypalCancel" + strURLSufix;
        //        string failureURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PaypalFailure" + strURLSufix;              

        //        PayPal.Services.Private.AP.PreapprovalResponse PResponse= null;

        //        if (!PaypalPayments.PreapprovalRequest(  oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID,
        //                                                 oUserPaymentMean.CURRENCy.CUR_ISO_CODE,
        //                                                  ((CultureInfo)Session["Culture"]).Name,
        //                                                  cancelURL,
        //                                                  returnURL,
        //                                                  out PResponse))
        //        {
        //            //HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
        //            return RedirectToAction("PaypalFailure" + strURLSufix); 
        //        }
        //        else
        //        {
        //            Session["PaypalPreapprovalKey"]= PResponse.preapprovalKey;
        //            //Session[Constants.SessionConstants.PREAPPROVALKEY] = PResponse.preapprovalKey;
        //            Response.Redirect(string.Format("{0}_ap-preapproval&preapprovalkey={1}",
        //                 ConfigurationManager.AppSettings["PAYPAL_API_REDIRECT_URL"], PResponse.preapprovalKey));

        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        string msg = e.Message;

        //        return RedirectToAction("PaypalFailure" + strURLSufix); 
        //    }


        //    return RedirectToAction("PaypalFailure" + strURLSufix); 
           

        //}

        //[Authorize]
        //public ActionResult PaypalPreapprovalRedirectINT()
        //{
        //    return PaypalPreapprovalRedirect("INT");
        //}


        //[Authorize]
        //public ActionResult PaypalPreapprovalRedirectPT()
        //{
        //    return PaypalPreapprovalRedirect("PT");
        //}

        //[Authorize]
        //public ActionResult PayPalPreapprovalSuccess(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 

        //    if (Session["PaypalPreapprovalKey"]==null)
        //        return RedirectToAction("PaypalFailure" + strURLSufix); 

        //    USER oUser= GetUserFromSession();

        //    if (oUser==null)
        //    {
        //        return LogOff();
        //    }

        //    try
        //    {
        //        decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //        ViewData["oUser"] = oUser;
        //        ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //        ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //        ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //        PayPal.Services.Private.AP.PreapprovalDetailsResponse PResponse= null;
        //        string strPreapprovalKey = "";

        //        if (PaypalPayments.PreapprovalConfirm(   Session["PaypalPreapprovalKey"].ToString(),
        //                                                  ((CultureInfo)Session["Culture"]).Name,
        //                                                  out PResponse,
        //                                                  out strPreapprovalKey))
        //        {

        //            if (UpdateUserPaypalPreapprovalPaymentMean(ref oUser,
        //                                                        strPreapprovalKey,
        //                                                        PResponse.startingDate,
        //                                                        PResponse.endingDate,
        //                                                        PResponse.maxNumberOfPayments,
        //                                                        PResponse.maxAmountPerPayment,
        //                                                        PResponse.maxAmountPerPaymentSpecified,
        //                                                        PResponse.maxTotalAmountOfAllPayments,
        //                                                        PResponse.maxTotalAmountOfAllPaymentsSpecified))
        //            {
        //                Session["USER_ID"] = oUser.USR_ID;
        //                Session["PaypalPreapprovalKey"] = null;
        //                ViewData["PayerQuantity"] = Convert.ToDouble(oUser.USR_BALANCE);
        //                ViewData["PayerCurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
        //                return View();
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
            
        //    Session["PaypalPreapprovalKey"] = null;

        //    return RedirectToAction("PaypalFailure" + strURLSufix); 
           

        //}


        //[Authorize]
        //public ActionResult PayPalPreapprovalSuccessINT()
        //{
        //    return PayPalPreapprovalSuccess("INT");
        //}

        //[Authorize]
        //public ActionResult PayPalPreapprovalSuccessPT()
        //{
        //    return PayPalPreapprovalSuccess("PT");
        //}


        //[Authorize]
        //public ActionResult PaypalPreapprovalPayRedirect(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix);

        //    USER oUser = GetUserFromSession();


        //    try
        //    {
        //        string requrl = "";
        //        if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
        //        {
        //            requrl = Request.Url.ToString();
        //        }
        //        else
        //        {
        //            requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
        //        } 
                
        //        string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PayPalPreapprovalPaySuccess" + strURLSufix;
        //        string cancelURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PaypalCancel" + strURLSufix;



        //        if (oUser == null)
        //        {
        //            return LogOff();
        //        }

        //        decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //        ViewData["oUser"] = oUser;
        //        ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //        ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //        ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

        //        NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
        //        numberFormatProvider.NumberDecimalSeparator = ".";

        //        PayPal.Services.Private.AP.PayResponse PResponse= null;

        //        if (!PaypalPayments.PreapprovalPayRequest(  oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID,
        //                                                    oUserPaymentMean.CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY,
        //                                                    Convert.ToDecimal(Session["QuantityToRecharge"].ToString(), numberFormatProvider),
        //                                                    Session["CurrencyToRecharge"].ToString(),
        //                                                    ((CultureInfo)Session["Culture"]).Name,
        //                                                    cancelURL,
        //                                                    returnURL,
        //                                                    out PResponse))
        //        {
        //            return RedirectToAction("PaypalFailure" + strURLSufix);
        //        }
        //        else
        //        {

        //            Session["PaypalPreapprovalPayKey"] = PResponse.payKey;
        //            if (PResponse.paymentExecStatus == "COMPLETED")
        //            {

        //                return RedirectToAction("PaypalPreapprovalPaySuccess" + strURLSufix);
        //            }
        //            else
        //            {
        //                Response.Redirect(string.Format("{0}_ap-payment&paykey={1}",
        //                        ConfigurationManager.AppSettings["PAYPAL_API_REDIRECT_URL"], PResponse.payKey));

        //            }

        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        string msg = e.Message;

        //        return RedirectToAction("PaypalFailure" + strURLSufix);
        //    }


        //    return RedirectToAction("PaypalFailure" + strURLSufix);


        //}


        //[Authorize]
        //public ActionResult PaypalPreapprovalPayRedirectINT()
        //{
        //    return PaypalPreapprovalPayRedirect("INT");
        //}


        //[Authorize]
        //public ActionResult PaypalPreapprovalPayRedirectPT()
        //{
        //    return PaypalPreapprovalPayRedirect("PT");
        //}


        //[Authorize]
        //public ActionResult PayPalPreapprovalPaySuccess(string strURLSufix)
        //{
        //    if (!Convert.ToBoolean(Session["InPaypalPayment"]))
        //        return RedirectToAction("PaypalFailure" + strURLSufix);

        //    if (Session["PaypalPreapprovalPayKey"] == null)
        //        return RedirectToAction("PaypalFailure" + strURLSufix);

        //    USER oUser = GetUserFromSession();

        //    if (oUser == null)
        //    {
        //        return LogOff();
        //    }

        //    try
        //    {
        //        decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
        //        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

        //        PayPal.Services.Private.AP.PaymentDetailsResponse PDResponse = null;

        //        if (PaypalPayments.PreapprovalPayConfirm(Session["PaypalPreapprovalPayKey"].ToString(),
        //                                                ((CultureInfo)Session["Culture"]).Name,
        //                                                out PDResponse))               
        //        {

        //            ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
        //            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
        //            numberFormatProvider.NumberDecimalSeparator = ".";
        //            decimal? dRechargeId = null;

        //            decimal dPercVAT1;
        //            decimal dPercVAT2;
        //            decimal dPercFEE;
        //            decimal dPercFEETopped;
        //            decimal dFixedFEE;
        //            int? iPaymentTypeId = null;
        //            int? iPaymentSubtypeId = null;
        //            if (oUserPaymentMean != null)
        //            {
        //                iPaymentTypeId = oUserPaymentMean.CUSPM_PAT_ID;
        //                iPaymentSubtypeId = oUserPaymentMean.CUSPM_PAST_ID;
        //            }

        //            if (!customersRepository.GetFinantialParams(oUser, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.BalanceRecharge,
        //                                                        out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
        //            {

        //            }

        //            int iPartialVAT1;
        //            int iPartialPercFEE;
        //            int iPartialFixedFEE;

        //            int iTotalQuantity = customersRepository.CalculateFEE(Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), numberFormatProvider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)), dPercVAT1, dPercVAT2, dPercFEE, 
        //                dPercFEETopped, dFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

        //            PaymentMeanRechargeCreationType rechargeCreationType = PaymentMeanRechargeCreationType.pmrctRegularRecharge;

        //            if (Session["RechargeCreationType"] != null)
        //            {
        //                rechargeCreationType = (PaymentMeanRechargeCreationType)Session["RechargeCreationType"];
        //            }

        //            int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
        //            int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

        //            if (customersRepository.RechargeUserBalance(ref oUser,
        //                                                       null,
        //                                                       Convert.ToInt32(MobileOS.Web),
        //                                                       (chargeType == ChargeOperationsType.BalanceRecharge),
        //                                                       Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), numberFormatProvider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUserPaymentMean.CURRENCy.CUR_ISO_CODE)),
        //                                                       dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
        //                                                       infraestructureRepository.GetCurrencyFromIsoCode(Session["CurrencyToRecharge"].ToString()),
        //                                                       (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE,
        //                                                       PaymentMeanRechargeStatus.Committed,
        //                                                       rechargeCreationType,
        //                                                       //0,
        //                                                       null,
        //                                                       PDResponse.paymentInfoList[0].transactionId,
        //                                                       null,
        //                                                       DateTime.Now.ToUniversalTime().ToString(),
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       null,
        //                                                       Session["PaypalPreapprovalPayKey"].ToString(),
        //                                                       false,
        //                                                       null, null,null,
        //                                                       infraestructureRepository,
        //                                                       out dRechargeId))
        //            {
        //                Session["USER_ID"] = oUser.USR_ID;
        //                ViewData["oUser"] = oUser;
        //                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
        //                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
        //                ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
        //                ViewData["PayerQuantity"] = Convert.ToDouble(oUser.USR_BALANCE);
        //                ViewData["PayerCurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
        //                Session["PaypalPreapprovalPayKey"] = null;
        //                Session["QuantityToRecharge"] = null;
        //                Session["QuantityToRechargeBase"] = null;
        //                Session["PercVAT1"] = null;
        //                Session["PercVAT2"] = null;
        //                Session["PercFEE"] = null;
        //                Session["PercFEETopped"] = null;
        //                Session["FixedFEE"] = null;
        //                Session["CurrencyToRecharge"] = null;
        //                Session["InPaypalPayment"] = false;
        //                Session["RechargeCreationType"] = null;

        //                SendRechargeEmail(ref oUser, dRechargeId);

        //                return View();
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    Session["PaypalPreapprovalPayKey"] = null;

        //    return RedirectToAction("PaypalFailure" + strURLSufix);


        //}


        //[Authorize]
        //public ActionResult PayPalPreapprovalPaySuccessINT()
        //{
        //    return PayPalPreapprovalPaySuccess("INT");
        //}

        //[Authorize]
        //public ActionResult PayPalPreapprovalPaySuccessPT()
        //{
        //    return PayPalPreapprovalPaySuccess("PT");
        //}

        //#endregion


        #region BSRedsys Payment

        [HttpGet]
        public ActionResult BSRedsysRequestPT()
        {
            return RedirectToAction("BSRedsysRequest", new { iSuffix = "PT" });
        }

        [HttpGet]
        public ActionResult BSRedsysRequest(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InBSRedsysPayment"])) return RedirectToAction("CCFailure" + iSuffix);

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {
                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpBSRedsys))
                                        .FirstOrDefault();
            }

            Session["BSRedsys_Config_id"] = oGatewayConfig.CPTGC_BSRCON_ID;
            string Email = oUser.USR_EMAIL;
            int Amount = Convert.ToInt32(Session["QuantityToRecharge"].ToString().Replace(".", string.Empty));
            string CurrencyISOCode = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            string Description = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");
            string UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");            

            string requrl = "";
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
            {
                requrl = Request.Url.ToString();
            }
            else
            {
                requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
            }

            string ReturnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "BSRedsysResult" + iSuffix;            


            string Hash = string.Empty;
            string culture = ((CultureInfo)Session["Culture"]).Name;
            Session["PAYMENT_ORIGIN"] = "AccountController";
            BSRedsysController bsredsys = new BSRedsysController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["BSRedsys_Config_id"]));
            return bsredsys.BSRedsysRequest(string.Empty, Email, Amount, CurrencyISOCode, Description, UTCDate, culture, ReturnURL, Hash);
        }

        [HttpPost]
        public ActionResult BSRedsysResultPT(string r)
        {
            return BSRedsysResult(r, "PT");
        }

        [HttpPost]
        public ActionResult BSRedsysResult(string r, string iSuffix)
        {
            Session["Suffix"] = iSuffix;

            USER oUser = GetUserFromSession();
            Session["UserBalance"] = oUser.USR_BALANCE;
            Session["PayerQuantity"] = Session["QuantityToRecharge"];

            /*if (!string.IsNullOrEmpty(Ds_MerchantParameters))
            {
                RedsysAPI r = new RedsysAPI();
                string sSignature = r.createMerchantSignatureNotif(Session["HashSeed"].ToString(), Ds_MerchantParameters);

                if (Ds_Signature == sSignature)
                {
                    ViewData["Result"] = r.GetParameterAll(true);

                    string sErrorCode = r.GetParameter("Ds_Response");
                    string sErrorMessage = "";
                    var eError = BSRedsysPayments.GetErrorInfo(sErrorCode, out sErrorMessage);

                    if (result == "OK")
                    {
                        string sReference = r.GetParameter("Ds_Order");
                        string sTransactionId = r.GetParameter("Ds_AuthorisationCode");
                        string sLocalDate = r.GetParameter("Ds_Date") + " " + r.GetParameter("Ds_Hour");
                        string sAuthCode = r.GetParameter("Ds_AuthorisationCode");                        
                        string sCardHash = r.GetParameter("Ds_Merchant_Identifier");
                        string sCardReference = r.GetParameter("Ds_Merchant_Identifier");
                        string sCardScheme = "";
                        string sCardMaskedCardNumber = " ";
                        DateTime? dtExpDate = null;
                        try
                        {
                            dtExpDate = new DateTime(Convert.ToInt32(r.GetParameter("Ds_ExpiryDate").Substring(0, 2)) + 2000, Convert.ToInt32(r.GetParameter("Ds_ExpiryDate").Substring(2, 2)), 1).AddMonths(1).AddSeconds(-1);
                        }
                        catch { }

                        if (!CCSuccess(sReference,
                                       sTransactionId,
                                       null,
                                       sLocalDate,
                                       sAuthCode,
                                       sErrorCode,
                                       sErrorMessage,
                                       sCardHash,
                                       sCardReference,
                                       sCardScheme,
                                       sCardMaskedCardNumber,
                                       PaymentMeanRechargeStatus.Committed,
                                       dtExpDate,
                                       null,
                                       null))
                        {
                            //j.result = "ERROR";
                            //j.errorMessage = "ERROR";
                        }
                        else
                        {
                            Session["UserBalance"] = oUser.USR_BALANCE;
                        }
                    }
                }
                else
                    result = "Failure";
            }
            else
                result = "Failure";

            //ViewData["Result"] = r_decrypted;
            if (Session["PayerQuantity"] != null) ViewData["PayerQuantity"] = Session["PayerQuantity"];
            if (Session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = Session["PayerCurrencyISOCode"];
            if (Session["UserBalance"] != null) ViewData["UserBalance"] = Session["UserBalance"];

            string sViewName = "BSRedsysResult";
            if (result == "OK")
                sViewName += "Success";
            else
                sViewName += "Failure";

            return View(sViewName + iSuffix);*/

            BSRedsysController bsredsys = new BSRedsysController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["BSRedsys_Config_id"]));
            string r_decrypted = bsredsys.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                DateTime? dtExpDate = null;
                try
                {
                    if ((j.bsredsys_expires_end_month.Length == 2) && (j.bsredsys_expires_end_year.Length == 2))
                    {
                        dtExpDate = new DateTime(Convert.ToInt32(j.bsredsys_expires_end_year) + 2000, Convert.ToInt32(j.bsredsys_expires_end_month), 1).AddMonths(1).AddSeconds(-1);
                    }
                }
                catch { }


                if (!CCSuccess(j.bsredsys_reference.ToString(),
                              j.bsredsys_transaction_id.ToString(),
                              null,
                              j.bsredsys_date_time_local_fmt.ToString(),
                              j.bsredsys_auth_code.ToString(),
                              j.errorCode.ToString(),
                              j.errorMessage.ToString(),
                              j.bsredsys_card_hash.ToString(),
                              j.bsredsys_card_reference.ToString(),
                              j.bsredsys_card_scheme.ToString(),
                              j.bsredsys_masked_card_number.ToString(),
                              PaymentMeanRechargeStatus.Committed,
                              dtExpDate,
                              null,
                              null))
                {
                    j.result = "ERROR";
                    j.errorMessage = "ERROR";
                }
                else
                {
                    Session["UserBalance"] = oUser.USR_BALANCE;
                }
            }

            ViewData["Result"] = r_decrypted;
            if (Session["PayerQuantity"] != null) ViewData["PayerQuantity"] = Session["PayerQuantity"];
            if (Session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = Session["PayerCurrencyISOCode"];
            if (Session["UserBalance"] != null) ViewData["UserBalance"] = Session["UserBalance"];

            string sViewName = "BSRedsysResult";
            if (j.result == "succeeded")
                sViewName += "Success";
            else
                sViewName += "Failure";

            return View(sViewName + iSuffix);            

        }

        #endregion

        #region Paysafe Payment

        [HttpGet]
        public ActionResult PaysafeRequestPT()
        {
            return RedirectToAction("PaysafeRequest", new { iSuffix = "PT" });
        }

        [HttpGet]
        public ActionResult PaysafeRequest(string iSuffix = "")
        {
            Session["Suffix"] = iSuffix;

            if (!Convert.ToBoolean(Session["InPaysafePayment"])) return RedirectToAction("CCFailure" + iSuffix);

            USER oUser = GetUserFromSession();
            if (oUser == null) return LogOff();

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {
                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpPaysafe))
                                        .FirstOrDefault();
            }

            Session["Paysafe_Config_id"] = oGatewayConfig.CPTGC_PYSCON_ID;
            string Email = oUser.USR_EMAIL;
            int Amount = Convert.ToInt32(Session["QuantityToRecharge"].ToString().Replace(".", string.Empty));
            string CurrencyISOCode = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
            string Description = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");
            string UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");

            string requrl = "";
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
            {
                requrl = Request.Url.ToString();
            }
            else
            {
                requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
            }

            string ReturnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "PaysafeResult" + iSuffix;


            string Hash = string.Empty;
            string culture = ((CultureInfo)Session["Culture"]).Name;
            Session["PAYMENT_ORIGIN"] = "AccountController";
            PaysafeController paysafe = new PaysafeController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Paysafe_Config_id"]));
            return paysafe.PaysafeRequest(string.Empty, Email, Amount, CurrencyISOCode, Description, UTCDate, culture, ReturnURL, Hash);
        }

        [HttpPost]
        public ActionResult PaysafeResultPT(string r)
        {
            return PaysafeResult(r, "PT");
        }

        [HttpPost]
        public ActionResult PaysafeResult(string r, string iSuffix)
        {
            Session["Suffix"] = iSuffix;

            USER oUser = GetUserFromSession();
            Session["UserBalance"] = oUser.USR_BALANCE;
            Session["PayerQuantity"] = Session["QuantityToRecharge"];

            PaysafeController paysafe = new PaysafeController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Paysafe_Config_id"]));
            string r_decrypted = paysafe.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                DateTime? dtExpDate = null;
                try
                {
                    if ((j.paysafe_expires_end_month.Length > 0) && (j.paysafe_expires_end_year.Length == 2))
                    {
                        dtExpDate = new DateTime(Convert.ToInt32(j.paysafe_expires_end_year) + 2000, Convert.ToInt32(j.paysafe_expires_end_month), 1).AddMonths(1).AddSeconds(-1);
                    }
                }
                catch { }


                if (!CCSuccess(j.paysafe_reference.ToString(),
                              j.paysafe_transaction_id.ToString(),
                              null,
                              j.paysafe_date_time_local_fmt.ToString(),
                              j.paysafe_auth_code.ToString(),
                              j.errorCode.ToString(),
                              j.errorMessage.ToString(),
                              j.paysafe_card_hash.ToString(),
                              j.paysafe_card_reference.ToString(),
                              j.paysafe_card_scheme.ToString(),
                              j.paysafe_masked_card_number.ToString(),
                              PaymentMeanRechargeStatus.Committed,
                              dtExpDate,
                              null,
                              j.paysafe_zip.ToString()))
                {
                    j.result = "ERROR";
                    j.errorMessage = "ERROR";
                }
                else
                {
                    Session["UserBalance"] = oUser.USR_BALANCE;
                }
            }

            ViewData["Result"] = r_decrypted;
            if (Session["PayerQuantity"] != null) ViewData["PayerQuantity"] = Session["PayerQuantity"];
            if (Session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = Session["PayerCurrencyISOCode"];
            if (Session["UserBalance"] != null) ViewData["UserBalance"] = Session["UserBalance"];

            string sViewName = "PaysafeResult";
            if (j.result == "succeeded")
                sViewName += "Success";
            else if (j.result == "cancel")
                sViewName += "Cancel";
            else
                sViewName += "Failure";

            return View(sViewName + iSuffix);

        }

        #endregion

        [Authorize]
        public ActionResult Main( int? Type,
                                  string DateIni,
                                  string DateEnd,
                                  int? Plate,
                                  GridSortOptions gridSortOptions, 
                                  int? page)
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {
                //if (IsUserBalanceCorrect(ref oUser))
                //{
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);
                    
                    if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
                    {
                        gridSortOptions.Column = "Date";
                        gridSortOptions.Direction = MvcContrib.Sorting.SortDirection.Descending;
                    }

                    DateTimeFormatInfo provider = ((CultureInfo)Session["Culture"]).DateTimeFormat;
                    DateTime? dtDateIni=null;
                    DateTime? dtDateEnd=null;

                    if ((DateIni!=null)&&(DateIni.Length>0))
                    {
                        try
                        {
                            dtDateIni = Convert.ToDateTime(DateIni, provider);
                        }
                        catch
                        {

                        }
                    }

                    if ((DateEnd != null) && (DateEnd.Length > 0))
                    {
                        try
                        {
                            dtDateEnd = Convert.ToDateTime(DateEnd, provider);
                        }
                        catch
                        {

                        }

                    }

                    var operationsFilterViewModel = new OperationFilterModel();
                    operationsFilterViewModel.SelectedType = Type ?? -1;
                    operationsFilterViewModel.SelectedPlate = Plate ?? -1;
                    operationsFilterViewModel.CurrentDateIni = dtDateIni;
                    operationsFilterViewModel.CurrentDateEnd = dtDateEnd;
                    operationsFilterViewModel.CurrentGridSortOptions = gridSortOptions;
                    operationsFilterViewModel.Fill(oUser);

                    int iNumRows = 0;
                    int iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;

                    try
                    {
                        iGridMaxRows = Convert.ToInt32(ConfigurationManager.AppSettings["OperationsGridNumRows"]);
                        if (iGridMaxRows == 0)
                        {
                            iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;
                        }
                    }
                    catch
                    {
                        iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;
                    }


                    var operationsList = GetUserOperations( ref oUser,
                                                            Type,
                                                            dtDateIni,
                                                            dtDateEnd,
                                                            Plate,
                                                            gridSortOptions,
                                                            page,
                                                            iGridMaxRows,
                                                            out iNumRows);

                   

                    var operationsPagedList = new CustomPagination<OperationRowModel>(
                                       operationsList,
                                       page.GetValueOrDefault(1),
                                       iGridMaxRows,
                                       iNumRows);

                    var operationsListContainer = new OperationsListContainerViewModel
                    {
                        OperationsPagedList = operationsPagedList,
                        OperationsFilterViewModel = operationsFilterViewModel,
                        GridSortOptions = gridSortOptions
                    };

                    return View(operationsListContainer);
                //}
                //else
                //{
                //    return RedirectToAction("RechargeINT", "Account");
                //}

            }
            else
            {
                return LogOff();
            }

            
        }

        [Authorize]
        public FileResult MainExport(int? Type, string DateIni, string DateEnd, int? Plate, GridSortOptions gridSortOptions, string format)
        {
            MemoryStream output = new MemoryStream();
            string sContentType = "";
            string sFileName = "";
                        
            USER oUser = GetUserFromSession();

            if (oUser != null)
            {
                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
                    {
                        gridSortOptions.Column = "Date";
                        gridSortOptions.Direction = MvcContrib.Sorting.SortDirection.Descending;
                    }

                    DateTimeFormatInfo provider = ((CultureInfo)Session["Culture"]).DateTimeFormat;
                    DateTime? dtDateIni=null;
                    DateTime? dtDateEnd=null;

                    if ((DateIni!=null)&&(DateIni.Length>0))
                    {
                        try
                        {
                            dtDateIni = Convert.ToDateTime(DateIni, provider);
                        }
                        catch
                        {

                        }
                    }

                    if ((DateEnd != null) && (DateEnd.Length > 0))
                    {
                        try
                        {
                            dtDateEnd = Convert.ToDateTime(DateEnd, provider);
                        }
                        catch
                        {

                        }

                    }

                    Type = (Type == -1 ? null : Type);
                    Plate = (Plate == -1 ? null : Plate);

                    int iNumRows = 0;

                    var operationsList = GetUserOperations( ref oUser,
                                                            Type,
                                                            dtDateIni,
                                                            dtDateEnd,
                                                            Plate,
                                                            gridSortOptions,
                                                            null,
                                                            -1,
                                                            out iNumRows);
                    string[] arrColumns = { "Type", "Installation", "Sector", "Tariff", "Plate", "Date", "AmountStr", "AmountFEEStr", "AmountVATStr", "AmountTotalStr", "DateIni", "DateEnd", "Time", "TicketNumber", "TicketData", "Source" };
                    string[] arrHeaders = { "Account_Op_Operation", "Account_Op_Installation", "Account_Op_Zone", "Account_Op_Tariff", "Account_Op_LicensePlate", "Account_Op_Date", "Account_Op_Amount", "Account_Op_AmountFEE", "Account_Op_AmountVAT", "Account_Op_AmountTotal", "Account_Op_Start_Date", "Account_Op_End_Date", "Account_Op_Duration", "Account_Op_TicketNumber", "Account_Op_TicketData", "Account_Op_Source" };
                    switch (format)
                    {
                        case "xls":
                            ExportExcel(typeof(OperationRowModel), operationsList, arrColumns, arrHeaders, output);
                            sContentType = "application/vnd.ms-excel";
                            sFileName = ResourceExtension.GetLiteral("Account_Op_Export_XLSFilename") + ".xls";
                            break;
                        case "pdf":
                            ExportPdf(typeof(OperationRowModel), operationsList, arrColumns, arrHeaders, output);
                            sContentType = "application/pdf";
                            sFileName = ResourceExtension.GetLiteral("Account_Op_Export_PDFFilename") + ".pdf";
                            break;
                    }

                }

            }

            //Return the result to the end user
            return File(output.ToArray(), sContentType, sFileName);
        }

        [Authorize]
        public ActionResult Invoices(string DateIni,
                                  string DateEnd,
                                  GridSortOptions gridSortOptions,
                                  int? page)
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {
                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
                    {
                        gridSortOptions.Column = "Date";
                        gridSortOptions.Direction = MvcContrib.Sorting.SortDirection.Descending;
                    }

                    DateTimeFormatInfo provider = ((CultureInfo)Session["Culture"]).DateTimeFormat;
                    DateTime? dtDateIni = null;
                    DateTime? dtDateEnd = null;

                    if ((DateIni != null) && (DateIni.Length > 0))
                    {
                        try
                        {
                            dtDateIni = Convert.ToDateTime(DateIni, provider);
                        }
                        catch
                        {

                        }
                    }

                    if ((DateEnd != null) && (DateEnd.Length > 0))
                    {
                        try
                        {
                            dtDateEnd = Convert.ToDateTime(DateEnd, provider);
                        }
                        catch
                        {

                        }

                    }

                    var invoicesFilterViewModel = new InvoiceFilterModel();
                    invoicesFilterViewModel.CurrentDateIni = dtDateIni;
                    invoicesFilterViewModel.CurrentDateEnd = dtDateEnd;

                    int iNumRows = 0;
                    int iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;

                    try
                    {
                        iGridMaxRows = Convert.ToInt32(ConfigurationManager.AppSettings["OperationsGridNumRows"]);
                        if (iGridMaxRows == 0)
                        {
                            iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;
                        }
                    }
                    catch
                    {
                        iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;
                    }


                    var invoiceList = GetCustomerInvoices(ref oUser,
                                                            dtDateIni,
                                                            dtDateEnd,
                                                            gridSortOptions,
                                                            page,
                                                            iGridMaxRows,
                                                            out iNumRows);



                    var invoicesPagedList = new CustomPagination<InvoiceRowModel>(
                                               invoiceList,
                                               page.GetValueOrDefault(1),
                                               iGridMaxRows,
                                               iNumRows);

                    var invoicesListContainer = new InvoicesListContainerViewModel
                    {
                        InvoicesPagedList = invoicesPagedList,
                        InvoicesFilterViewModel = invoicesFilterViewModel,
                        GridSortOptions = gridSortOptions
                    };

                    return View(invoicesListContainer);
                }
                else
                {
                    return RedirectToAction("RechargeINT", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }



        [Authorize]
        public ActionResult Invoice(int invoiceID)
        {

           
            USER oUser = GetUserFromSession();

            if (oUser != null)
            {
                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    try
                    {                                            
                        CUSTOMER_INVOICE oInvoice=oUser.CUSTOMER.CUSTOMER_INVOICEs.Where(r => r.CUSINV_ID == invoiceID).First();

                        if (oInvoice != null)
                        {
                            string sServerPath = HttpContext.Server.MapPath("~/Invoicing/");
                            string sFileName = oInvoice.CUSTOMER.CUS_ID.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".pdf";

                            string sGeneratedPdfPath = "";

                            switch (oInvoice.CUSINV_INVOICE_VERSION)
                            {
                                case 1:
                                    {
                                        sGeneratedPdfPath = ExportInvoicePdf(oInvoice, sServerPath, sFileName);
                                        break;
                                    }

                                case 2:
                                    {
                                        sGeneratedPdfPath = ExportInvoiceReportPdf(oInvoice, sServerPath, sFileName);
                                        break;
                                    }
                            }


                            if (!string.IsNullOrEmpty(sGeneratedPdfPath))
                            {
                                Response.Clear();
                                Response.Buffer = false;
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("Content-disposition", "attachment; filename=invoice_" + oInvoice.CUSINV_INV_DATE.Value.ToString("yyyyMMdd") + ".pdf");

                                FileInfo oFileInfo = new FileInfo(sGeneratedPdfPath);
                                long lfull_size = oFileInfo.Length;
                                oFileInfo = null;

                                Response.AddHeader("Content-length", lfull_size.ToString());
                                Response.WriteFile(sGeneratedPdfPath);
                                Response.End();
                                System.IO.File.Delete(sGeneratedPdfPath);
                                return View();

                            }
                        }
                    }
                    catch(Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR,"Invoice",e);                                            

                    }

                    return LogOff();

                }
                else
                {
                    return RedirectToAction("RechargeINT", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }



        [Authorize]
        public ActionResult UserData()
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                if (IsUserBalanceCorrect(ref oUser))
                {

                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);
                    
                    bool bUsernameEqualsEmail = (ViewData["UsernameEqualsEmail"].ToString() == "1");


                    UserDataModel model= null;
                    if (!GetUserDataModelFromUser(ref oUser, out model))
                    {
                        return LogOff();
                    }

                    if (bUsernameEqualsEmail)
                    {
                        model.Username = "XXXXXXXXXXX";
                    }

                    decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                    CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                    if (oUserPaymentMean != null)
                    {
                        if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                        {
                            ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                        }
                        else if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                        {
                            ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                        }
                    }
                    else
                    {
                        ViewData["PaymentType"] = "-1";
                    }


                    if (oUser.USR_SUSCRIPTION_TYPE != null)
                    {
                        ViewData["SuscriptionType"] = oUser.USR_SUSCRIPTION_TYPE;
                    }
                    else
                    {
                        ViewData["SuscriptionType"] = "-1";
                    }


                    ViewData["SelectedMainPhoneNumberPrefix"] = model.MainPhoneNumberPrefix;
                    ViewData["SelectedAlternativePhoneNumberPrefix"] = model.AlternativePhoneNumberPrefix;
                    ViewData["SelectedCountry"] = model.Country;
                    ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();                   

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Recharge", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }




        [HttpPost]
        [Authorize]
        public ActionResult UserData(UserDataModel model)
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["SelectedMainPhoneNumberPrefix"] = model.MainPhoneNumberPrefix;
                    ViewData["SelectedAlternativePhoneNumberPrefix"] = model.AlternativePhoneNumberPrefix;
                    ViewData["SelectedCountry"] = model.Country;
                    ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
                    ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];
                    bool bUsernameEqualsEmail = (ViewData["UsernameEqualsEmail"].ToString() == "1");
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                    CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
                    
                    if (oUserPaymentMean != null)
                    {
                        if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                        {
                            ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                        }
                        else if (oUserPaymentMean.CUSPM_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                        {
                            ViewData["PaymentType"] = oUserPaymentMean.CUSPM_PAT_ID;
                        }
                    }
                    else
                    {
                        ViewData["PaymentType"] = "-1";
                    }



                    if (oUser.USR_SUSCRIPTION_TYPE != null)
                    {
                        ViewData["SuscriptionType"] = oUser.USR_SUSCRIPTION_TYPE;
                    }
                    else
                    {
                        ViewData["SuscriptionType"] = "-1";
                    }

                    model.Fill(oUser,true);


                    if (model.Plates.Count==0)
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_PlateListCantbeEmpty"));
                        model.Fill(oUser, false);
                        model.platesChanges = "";
                        return View(model);
                    }


                    if (ModelState.IsValid)
                    {


                        string strOldUsername = oUser.USR_USERNAME;
                        string currentPassword = "";
                        if (!FormAuthMemberShip.MembershipService.GetPassword(oUser.USR_USERNAME, ref currentPassword))
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                            return View(model);
                        }


                        if (model.CurrentPassword != currentPassword)
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_InvalidPassword"));
                            return View(model);
                        }


                        if (bUsernameEqualsEmail)
                        {
                            model.Username = model.Email;
                        }

                        if (model.Username != strOldUsername)
                        {
                            if (customersRepository.ExistUsername(model.Username))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrMsg_UsernameAlreadyExist"));
                                if (bUsernameEqualsEmail)
                                {
                                    model.Username = "XXXXXXXXXXX";
                                }
                                return View(model);
                            }
                        }


                        if (!SetDomainCustomerAndUserFromUserDataModel(ref oUser, model))
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                            if (bUsernameEqualsEmail)
                            {
                                model.Username = "XXXXXXXXXXX";
                            }
                            return View(model);
                        }

                        if (model.Username != strOldUsername)
                        {
                            string password = "";


                            if (!string.IsNullOrEmpty(model.NewPassword))
                            {
                                password = model.NewPassword;
                            }
                            else
                            {
                                password = currentPassword;
                            }

                            FormAuthMemberShip.FormsService.SignOut();
                            bool bDeleted = FormAuthMemberShip.MembershipService.DeleteUser(strOldUsername);

                            if (bDeleted)
                            {
                                FormAuthMemberShip.MembershipService.CreateUser(model.Username, password, oUser.USR_EMAIL);
                                FormAuthMemberShip.FormsService.SignIn(model.Username, false);
                            }
                            
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.NewPassword))
                            {
                                FormAuthMemberShip.MembershipService.ChangePassword(oUser.USR_USERNAME, currentPassword, model.NewPassword);
                            }
                        }


                        Session["USER_ID"] = oUser.USR_ID;

                        return RedirectToAction("UserData", "Account");
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Recharge", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }


        [Authorize]
        public ActionResult ChangeEmailOrMobile()
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    ChangeEmailOrMobileModel model = null;
                    if (!GetUserDataModelFromUser(oUser, out model))
                    {
                        return LogOff();
                    }
                    
                    ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
                    ViewData["SelectedMainPhoneNumberPrefix"] = model.MainPhoneNumberPrefix;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Recharge", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }






        [HttpPost]
        [Authorize]
        public ActionResult ChangeEmailOrMobile(ChangeEmailOrMobileModel model)
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["SelectedMainPhoneNumberPrefix"] = model.MainPhoneNumberPrefix;
                    ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    if (ModelState.IsValid)
                    {


                        string strOldUsername = oUser.USR_USERNAME;
                        string currentPassword = "";
                        if (!FormAuthMemberShip.MembershipService.GetPassword(oUser.USR_USERNAME, ref currentPassword))
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                            return View(model);
                        }

                        if (model.CurrentPassword != currentPassword)
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_InvalidPassword"));
                            return View(model);
                        }


                        if ((oUser.USR_MAIN_TEL_COUNTRY != Convert.ToInt32(model.MainPhoneNumberPrefix)) ||
                            (oUser.USR_MAIN_TEL != model.MainPhoneNumber))
                        {

                            if (customersRepository.ExistMainTelephone(Convert.ToInt32(model.MainPhoneNumberPrefix), model.MainPhoneNumber))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorTelephoneAlreadyExist"));
                                return View(model);
                            }
                        }

                        if (oUser.USR_EMAIL != model.Email)
                        {
                            if (customersRepository.ExistEmail(model.Email))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorEmailAlreadyExist"));
                                return View(model);
                            }
                        }




                        if ((oUser.USR_MAIN_TEL_COUNTRY != Convert.ToInt32(model.MainPhoneNumberPrefix)) ||
                            (oUser.USR_MAIN_TEL != model.MainPhoneNumber) ||
                            (oUser.USR_EMAIL != model.Email))
                        {

                            USERS_SECURITY_OPERATION oSecOperation=null;

                            if (!SetDomainUserSecurityOperationFromChangeEmailOrMobileModel(ref oUser, model, out oSecOperation))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                return View(model);
                            }
                            else
                            {
                                if (!SendEmailAndSMS(ref oUser, oSecOperation,ResourceExtension.GetLiteral("ChangeEmailOrTelephone_EmailHeader"),
                                        ResourceExtension.GetLiteral("ChangeEmailOrTelephone_EmailBody"), ResourceExtension.GetLiteral("ChangeEmailOrTelephone_SMS")))
                                {
                                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorSendingActivation"));
                                    return View(model);
                                }

                                customersRepository.UpdateSecurityOperationRetries(ref oSecOperation);

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorNothingHasChanged"));
                            return View(model);
                        }



                        Session["USER_ID"] = oUser.USR_ID;

                        return RedirectToAction("ChangeEmailOrMobileEnd", "Account");
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Recharge", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }


        [Authorize]
        public ActionResult ChangeEmailOrMobileEnd()
        {
            return View();
        }



        [Authorize]
        public ActionResult DeleteUser()
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    return View();

                }
                else
                {
                    return RedirectToAction("Recharge", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }


        [HttpPost]
        [Authorize]
        public ActionResult DeleteUser(DeleteUserModel model)
        {

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {

                if (IsUserBalanceCorrect(ref oUser))
                {
                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                    ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);

                    if (ModelState.IsValid)
                    {


                        if (model.ConfirmDeletion)
                        {

                            string strOldUsername = oUser.USR_USERNAME;
                            string currentPassword = "";
                            if (!FormAuthMemberShip.MembershipService.GetPassword(oUser.USR_USERNAME, ref currentPassword))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                return View(model);
                            }

                            if (model.CurrentPassword != currentPassword)
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_InvalidPassword"));
                                return View(model);
                            }

                            bool bDeleted = FormAuthMemberShip.MembershipService.DeleteUser(strOldUsername);

                            if (bDeleted)
                            {
                                if (!customersRepository.DeleteUser(ref oUser))
                                {
                                    FormAuthMemberShip.MembershipService.CreateUser(oUser.USR_USERNAME, currentPassword, oUser.USR_EMAIL);
                                    FormAuthMemberShip.FormsService.SignOut();
                                    FormAuthMemberShip.FormsService.SignIn(oUser.USR_USERNAME, false);
                                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                }
                                else
                                {
                                    Session["USER_ID"] = null;
                                    FormAuthMemberShip.FormsService.SignOut();
                                    return RedirectToAction("DeleteUserEnd", "Account");
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                            }

                            

                        }
                        else
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrMsg_DeleteUserMustBeAccepted"));                           

                        }

                    }


                    return View(model);

                }
                else
                {
                    return RedirectToAction("Recharge", "Account");
                }

            }
            else
            {
                return LogOff();
            }


        }

        [Authorize]
        public ActionResult LogOff()
        {

            Session["USER_ID"] = null;
            Session["CurrentModel"] = null;

            FormAuthMemberShip.FormsService.SignOut();
            return RedirectToAction("LogOn", "Home");
        }


        public ActionResult DeleteUserEnd()
        {
            return View();
        }


        public ActionResult SecurityOperation()
        {
            string urlCode = Request.QueryString["code"];

            USERS_SECURITY_OPERATION oSecOperation = customersRepository.GetUserSecurityOperation(urlCode);


            if (oSecOperation != null)
            {

                Session["SecOperationID"] = oSecOperation.USOP_ID;

                switch ((SecurityOperationType)oSecOperation.USOP_OP_TYPE)
                {
                    case SecurityOperationType.ChangeEmail_Telephone:
                        return RedirectToAction("SecurityOperationChangeEmailOrTelephone", "Account", new { code = urlCode });
                    case SecurityOperationType.RecoverPassword:
                        return RedirectToAction("SecurityOperationForgotPassword", "Account", new { code = urlCode });
                    case SecurityOperationType.ResetPassword:
                        return RedirectToAction("SecurityOperationResetPassword", "Account", new { code = urlCode });
                    default:
                        break;
                }
                
            }
            else
            {
                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ActivationURLIncorrect"));
            }


            return View();

        }


        public ActionResult SecurityOperationChangeEmailOrTelephone()
        {
            ViewData["CodeExpired"] = false;
            ViewData["CodeAlreadyUsed"] = false;
            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;


            string urlCode = Request.QueryString["code"];

            USERS_SECURITY_OPERATION oSecOperation = customersRepository.GetUserSecurityOperation(urlCode);


            if (oSecOperation != null)
            {

                Session["SecOperationID"] = oSecOperation.USOP_ID;
                string culture = oSecOperation.USER.USR_CULTURE_LANG;
                CultureInfo ci = new CultureInfo(culture);
                Session["Culture"] = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                integraMobile.Properties.Resources.Culture = ci;


                if (customersRepository.IsUserSecurityOperationExpired(oSecOperation))
                {
                    ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                    ViewData["CodeExpired"] = true;
                }
                else if (customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;
                }


                ViewData["EndSufixMainPhone"] = oSecOperation.USOP_NEW_MAIN_TEL.Substring(oSecOperation.USOP_NEW_MAIN_TEL.Length - 2, 2)
                                                .PadLeft(oSecOperation.USOP_NEW_MAIN_TEL.Length, '*');

            }
            else
            {
                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ActivationURLIncorrect"));
            }


            return View();

        }


        [HttpPost]
        public ActionResult SecurityOperationChangeEmailOrTelephone(string type, string confirmationcode)
        {
            USERS_SECURITY_OPERATION oSecOperation = GetSecurityOperationFromSession();



            ViewData["EndSufixMainPhone"] = oSecOperation.USOP_NEW_MAIN_TEL.Substring(oSecOperation.USOP_NEW_MAIN_TEL.Length - 2, 2)
                                                .PadLeft(oSecOperation.USOP_NEW_MAIN_TEL.Length, '*');

            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;
            Session["ValidCustomerInscription"] = false;

            if (!customersRepository.IsUserSecurityOperationExpired(oSecOperation))
            {
                if (!customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                {


                    if (type == "confirmcode")
                    {
                        if (String.IsNullOrEmpty(confirmationcode))
                        {
                            ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ValidationCodeIsRequired"));
                        }
                        else
                        {


                            if (oSecOperation.USOP_ACTIVATION_CODE != confirmationcode)
                            {
                                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ValidationCodeIsIncorrect"));
                            }
                            else
                            {
                                return (RedirectToAction("SecurityOperationChangeEmailOrTelephoneConfirmation"));
                            }

                        }
                    }
                    else if (type == "resendsms")
                    {

                        if (ReSendSMS(oSecOperation, ResourceExtension.GetLiteral("ChangeEmailOrTelephone_SMS")))
                        {
                            customersRepository.UpdateSecurityOperationRetries(ref oSecOperation);
                            ViewData["ActivationRetries"] = string.Format(ResourceExtension.GetLiteral("CustomerInscriptionModel_SMSReSent"), oSecOperation.USOP_ACTIVATION_RETRIES);
                            Session["SecOperationID"] = oSecOperation.USOP_ID;

                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;

                }
            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                ViewData["CodeExpired"] = true;
            }

            return View();


        }



        public ActionResult SecurityOperationChangeEmailOrTelephoneConfirmation()
        {
            USERS_SECURITY_OPERATION oSecOperation = GetSecurityOperationFromSession();
            ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];
            bool bUsernameEqualsEmail = (ViewData["UsernameEqualsEmail"].ToString() == "1");

            ViewData["ConfirmationError"] = false;
            if (oSecOperation != null)
            {
                bool bEmailHasChanged= (oSecOperation.USER.USR_EMAIL!= oSecOperation.USOP_NEW_EMAIL);
                string strOldUsername = oSecOperation.USER.USR_USERNAME;

                if (!customersRepository.ModifyUserEmailOrTelephone(oSecOperation, bUsernameEqualsEmail))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                    ViewData["ConfirmationError"] = true;
                }
                else
                {

                    if (bEmailHasChanged)
                    {                        

                        if (bUsernameEqualsEmail)
                        {
                            string currentPassword = "";
                            if (!FormAuthMemberShip.MembershipService.GetPassword(strOldUsername, ref currentPassword))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                ViewData["ConfirmationError"] = true;
                            }
                            else
                            {
                                FormAuthMemberShip.FormsService.SignOut();
                                bool bDeleted = FormAuthMemberShip.MembershipService.DeleteUser(strOldUsername);

                                if (bDeleted)
                                {
                                    FormAuthMemberShip.MembershipService.CreateUser(oSecOperation.USOP_NEW_EMAIL, currentPassword, oSecOperation.USOP_NEW_EMAIL);
                                }
                            }

                        }
                        else                        
                        {
                            FormAuthMemberShip.MembershipService.ChangeEmail(oSecOperation.USER.USR_USERNAME, oSecOperation.USOP_NEW_EMAIL);
                        }

                    }


                    
                    ViewData["Email"] = oSecOperation.USOP_NEW_EMAIL;
                    ViewData["Telephone"] = "+" + oSecOperation.COUNTRy.COU_TEL_PREFIX + " " + oSecOperation.USOP_NEW_MAIN_TEL;
                    Session["SecOperationID"] = null;

                }
            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                ViewData["ConfirmationError"] = true;
            }

            return View();


        }



        public ActionResult ForgotPassword(string culture, string urlreferer)
        {

            if (!string.IsNullOrEmpty(culture))
            {
                CultureInfo ci = new CultureInfo(culture);

                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en-US";
                    ci = new CultureInfo(langName);
                }
                Session["Culture"] = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                integraMobile.Properties.Resources.Culture = ci;
                urlreferer = urlreferer.Replace(culture, "{0}");

                if (!string.IsNullOrEmpty(urlreferer))
                {
                    Session["URLReferer"] = urlreferer;
                }

                return RedirectToAction("ForgotPassword","Account");

            }
            else
            {
                return View();
            }                     

        }



        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {

            if (ModelState.IsValid)
            {
                USER oUser = null;

                if (!customersRepository.GetUserData(ref oUser, model.Username))
                {
                    oUser = null;
                    if (!customersRepository.GetUserDataByEmail(ref oUser, model.Username))
                    {
                        oUser = null;
                    }
                }


                if (oUser == null)
                {
                    bool bUserExist = false;
                    //Si el usuario no existe se verifica el los Servidores Externos
                    List<COUNTRIES_REDIRECTION> oCountriesRedirectionsList = infraestructureRepository.GetCountriesRedirections();
                    if (oCountriesRedirectionsList != null || oCountriesRedirectionsList.Count > 0)
                    {
                        try
                        {
                            ForgotPasswordRequets oForgotPasswordRequets = new ForgotPasswordRequets();
                            oForgotPasswordRequets = oForgotPasswordRequets.getRequest(model.Username);
                            string sInJson = Tools.ToJsonRequest(oForgotPasswordRequets);

                            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sInJson);
                            m_Log.LogMessage(LogLevels.logINFO, String.Format(Tools.PrettyXml(doc.InnerXml)));

                            string strOut = string.Empty;
                            integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                            ForgotPasswordResponse oForgotPasswordResponse = null;
                            foreach (COUNTRIES_REDIRECTION cr in oCountriesRedirectionsList)
                            {
                                strOut = CallServiceIntegraMobileWS(Tools.METHODS_FORGET_PASSWORD_JSON, sInJson, cr);
                                oForgotPasswordResponse = JsonConvert.DeserializeObject<ForgotPasswordResponse>(strOut);
                                if (oForgotPasswordResponse.r == ResultType.Result_OK)
                                {
                                    return RedirectToAction("ForgotPasswordEnd", "Account");
                                }
                                
                            }
                        }
                        catch (Exception)
                        {
                            return View(model);
                        }
                    }

                    if (!bUserExist)
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_UserNotExists"));
                        return View(model);
                    }
                }
                else
                {

                     USERS_SECURITY_OPERATION oSecOperation = null;

                     if (!SetDomainUserSecurityOperationForgotPassword(ref oUser, out oSecOperation))
                     {
                         ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                         return View(model);
                     }
                     else
                     {

                         switch ((SecurityOperationType)oSecOperation.USOP_OP_TYPE)
                         {

                             case SecurityOperationType.RecoverPassword:
                                 {
                                     if (!SendEmailAndSMS(ref oUser, oSecOperation, ResourceExtension.GetLiteral("ForgotPassword_EmailHeader"),
                                                                     ResourceExtension.GetLiteral("ForgotPassword_EmailBody"), ResourceExtension.GetLiteral("ForgotPassword_SMS")))
                                     {
                                         ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorSendingActivation"));
                                         return View(model);
                                     }

                                 }
                                 break;
                             case SecurityOperationType.ResetPassword:
                                 {
                                     if (!SendEmail(ref oUser, oSecOperation, ResourceExtension.GetLiteral("ForgotPassword_EmailHeader"),
                                                                     ResourceExtension.GetLiteral("ForgotPassword_EmailBody")))
                                     {
                                         ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorSendingActivation"));
                                         return View(model);
                                     }

                                 }
                                 break;

                         }

                        

                         customersRepository.UpdateSecurityOperationRetries(ref oSecOperation);

                         return RedirectToAction("ForgotPasswordEnd", "Account");

                     }
                 }
               
            }
            return View(model);

        }




        public ActionResult ForgotPasswordEnd()
        {
            return View();
        }



        public ActionResult SecurityOperationForgotPassword()
        {
            ViewData["CodeExpired"] = false;
            ViewData["CodeAlreadyUsed"] = false;
            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;


            string urlCode = Request.QueryString["code"];

            USERS_SECURITY_OPERATION oSecOperation = customersRepository.GetUserSecurityOperation(urlCode);


            if (oSecOperation != null)
            {

                Session["SecOperationID"] = oSecOperation.USOP_ID;
                string culture = oSecOperation.USER.USR_CULTURE_LANG;
                CultureInfo ci = new CultureInfo(culture);
                Session["Culture"] = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                integraMobile.Properties.Resources.Culture = ci;


                if (customersRepository.IsUserSecurityOperationExpired(oSecOperation))
                {
                    ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                    ViewData["CodeExpired"] = true;
                }
                else if (customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;
                }


                ViewData["EndSufixMainPhone"] = oSecOperation.USOP_NEW_MAIN_TEL.Substring(oSecOperation.USOP_NEW_MAIN_TEL.Length - 2, 2)
                                                .PadLeft(oSecOperation.USOP_NEW_MAIN_TEL.Length, '*');

            }
            else
            {
                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ActivationURLIncorrect"));
            }


            return View();

        }


        [HttpPost]
        public ActionResult SecurityOperationForgotPassword(string type, string confirmationcode)
        {
            USERS_SECURITY_OPERATION oSecOperation = GetSecurityOperationFromSession();

            ViewData["EndSufixMainPhone"] = oSecOperation.USOP_NEW_MAIN_TEL.Substring(oSecOperation.USOP_NEW_MAIN_TEL.Length - 2, 2)
                                                .PadLeft(oSecOperation.USOP_NEW_MAIN_TEL.Length, '*');

            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;
            Session["ValidCustomerInscription"] = false;

            if (!customersRepository.IsUserSecurityOperationExpired(oSecOperation))
            {
                if (!customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                {


                    if (type == "confirmcode")
                    {
                        if (String.IsNullOrEmpty(confirmationcode))
                        {
                            ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ValidationCodeIsRequired"));
                        }
                        else
                        {


                            if (oSecOperation.USOP_ACTIVATION_CODE != confirmationcode)
                            {
                                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ValidationCodeIsIncorrect"));
                            }
                            else
                            {
                                string currentPassword = null;
                                if (!FormAuthMemberShip.MembershipService.GetPassword(oSecOperation.USER.USR_USERNAME, ref currentPassword))
                                {
                                    ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrorsMsg_UserNotExists"));
                                }
                                else
                                {
                                    return (RedirectToAction("SecurityOperationForgotPasswordConfirmation"));
                                }
                            }

                        }
                    }
                    else if (type == "resendsms")
                    {

                        if (ReSendSMS(oSecOperation, ResourceExtension.GetLiteral("ForgotPassword_SMS")))
                        {
                            customersRepository.UpdateSecurityOperationRetries(ref oSecOperation);
                            ViewData["ActivationRetries"] = string.Format(ResourceExtension.GetLiteral("CustomerInscriptionModel_SMSReSent"), oSecOperation.USOP_ACTIVATION_RETRIES);
                            Session["SecOperationID"] = oSecOperation.USOP_ID;

                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;

                }
            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                ViewData["CodeExpired"] = true;
            }

            return View();


        }



        public ActionResult SecurityOperationForgotPasswordConfirmation()
        {
            USERS_SECURITY_OPERATION oSecOperation = GetSecurityOperationFromSession();

            ViewData["ConfirmationError"] = false;
            if (oSecOperation != null)
            {


                if (!customersRepository.ConfirmSecurityOperation(oSecOperation))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                    ViewData["ConfirmationError"] = true;
                }
                else
                {
                    string currentPassword = null;
                    if (!FormAuthMemberShip.MembershipService.GetPassword(oSecOperation.USER.USR_USERNAME, ref currentPassword))
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_UserNotExists"));
                        ViewData["ConfirmationError"] = true;
                    }
                    else                    
                    {
                        Session["SecOperationID"] = null;
                        ViewData["Password"] = currentPassword;
                    }
                }

            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                ViewData["ConfirmationError"] = true;
            }

            return View();


        }


        public ActionResult SecurityOperationResetPassword()
        {
            ViewData["CodeExpired"] = false;
            ViewData["CodeAlreadyUsed"] = false;
            ViewData["ConfirmationCodeError"] = false;
            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;


            string urlCode = Request.QueryString["code"];

            USERS_SECURITY_OPERATION oSecOperation = customersRepository.GetUserSecurityOperation(urlCode);


            if (oSecOperation != null)
            {

                Session["SecOperationID"] = oSecOperation.USOP_ID;
                string culture = oSecOperation.USER.USR_CULTURE_LANG;
                CultureInfo ci = new CultureInfo(culture);
                Session["Culture"] = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                integraMobile.Properties.Resources.Culture = ci;
                ViewData["username"] = oSecOperation.USER.USR_USERNAME;

                if (customersRepository.IsUserSecurityOperationExpired(oSecOperation))
                {
                    ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                    ViewData["CodeExpired"] = true;
                }
                else if (customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;
                }
            }
            else
            {
                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ActivationURLIncorrect"));
                ViewData["ConfirmationCodeError"] = true;
            }


            return View();

        }


        [HttpPost]
        public ActionResult SecurityOperationResetPassword(ResetPasswordModel model)
        {
            USERS_SECURITY_OPERATION oSecOperation = GetSecurityOperationFromSession();


            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;
            ViewData["username"] = oSecOperation.USER.USR_USERNAME;


            if (!customersRepository.IsUserSecurityOperationExpired(oSecOperation))
            {
                if (!customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                {
                    if (ModelState.IsValid)
                    {

                        string strCurrPassword = "";
                        if (FormAuthMemberShip.MembershipService.GetPassword(oSecOperation.USER.USR_USERNAME, ref strCurrPassword))
                        {
                            if (!customersRepository.ConfirmSecurityOperation(oSecOperation))
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                return View(model);
                            }
                            else
                            {
                                FormAuthMemberShip.MembershipService.ChangePassword(oSecOperation.USER.USR_USERNAME, strCurrPassword, model.Password);
                                return (RedirectToAction("SecurityOperationResetPasswordConfirmation"));
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                            return View(model);
                        }


                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                return View(model);

            }

            return View(model);


        }



        public ActionResult SecurityOperationResetPasswordConfirmation()
        {
            USERS_SECURITY_OPERATION oSecOperation = GetSecurityOperationFromSession();

            ViewData["ConfirmationError"] = false;
            if (oSecOperation != null)
            {


                if (!customersRepository.ConfirmSecurityOperation(oSecOperation))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                    ViewData["ConfirmationError"] = true;
                }
                else
                {
                    string currentPassword = null;
                    if (!FormAuthMemberShip.MembershipService.GetPassword(oSecOperation.USER.USR_USERNAME, ref currentPassword))
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_UserNotExists"));
                        ViewData["ConfirmationError"] = true;
                    }
                    else
                    {
                        Session["SecOperationID"] = null;
                        ViewData["Password"] = currentPassword;
                    }
                }

            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                ViewData["ConfirmationError"] = true;
            }

            return View();


        }


        [Authorize]
        public ActionResult WebParking()
        {
            USER oUser = GetUserFromSession();
            string strAutoPassword = "";
            FormAuthMemberShip.MembershipService.GetPassword(oUser.USR_USERNAME, ref strAutoPassword);
            string sAuth = Encryptor.Encrypt(oUser.USR_USERNAME + "*" + strAutoPassword + "*" + DateTime.UtcNow.Ticks.ToString() + "*" + ((CultureInfo)Session["Culture"]).Name, (ConfigurationManager.AppSettings["EncryptorKey"] ?? ""));

            string sUrl = string.Format((ConfigurationManager.AppSettings["WebParkingUrl"] ?? ""), HttpUtility.UrlEncode(sAuth));

            //return JavaScript(string.Format("window.open('{0}');", sUrl));
            
            return Redirect(sUrl);
        }


        private bool IsUserBalanceCorrect(ref USER oUser)
        {
            bool bAllowNoPaymentMethod = (infraestructureRepository.GetParameterValue("AllowNoPaymentMethod") == "1");
            customersRepository.RenewUserData(ref oUser);
            return ((oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction) || (oUser.USR_BALANCE > 0) || (oUser.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Count()>0) || bAllowNoPaymentMethod);
        }


        private bool GetUserDataModelFromUser(ref USER oUser, out UserDataModel model)
        {
            bool bRes = true;
            
            model = null;

            try
            {
                model = new UserDataModel
                    {
                        Username = oUser.USR_USERNAME,                        
                        Email = oUser.USR_EMAIL,
                        CurrentPassword ="",
                        NewPassword = "",
                        ConfirmNewPassword = "",
                        Name =  oUser.CUSTOMER.CUS_FIRST_NAME,
                        Surname1 = oUser.CUSTOMER.CUS_SURNAME1,
                        Surname2 =  oUser.CUSTOMER.CUS_SURNAME2,
                        DocId = oUser.CUSTOMER.CUS_DOC_ID,
                        MainPhoneNumberPrefix = oUser.USR_MAIN_TEL_COUNTRY.ToString(),
                        MainPhoneNumber = oUser.USR_MAIN_TEL,
                        AlternativePhoneNumberPrefix = oUser.USR_SECUND_TEL_COUNTRY.ToString(),
                        AlternativePhoneNumber = oUser.USR_SECUND_TEL,
                        StreetName = oUser.CUSTOMER.CUS_STREET,
                        StreetNumber = oUser.CUSTOMER.CUS_STREE_NUMBER.ToString(),
                        LevelInStreetNumber = oUser.CUSTOMER.CUS_LEVEL_NUM.ToString(),
                        DoorInStreetNumber = oUser.CUSTOMER.CUS_DOOR,
                        LetterInStreetNumber = oUser.CUSTOMER.CUS_LETTER,
                        StairInStreetNumber = oUser.CUSTOMER.CUS_STAIR,
                        Country = oUser.CUSTOMER.CUS_COU_ID.ToString(),
                        State = oUser.CUSTOMER.CUS_STATE,
                        City = oUser.CUSTOMER.CUS_CITY,
                        ZipCode = oUser.CUSTOMER.CUS_ZIPCODE,
                        platesChanges =""
                    };

                model.Fill(oUser,false);
               
            }
            catch(Exception)
            {
                bRes = false;
               
                oUser = null;

            }

            return bRes;
        }


        private bool GetUserDataModelFromUser(USER oUser, out ChangeEmailOrMobileModel model)
        {
            bool bRes = true;
            
            model = null;

            try
            {
                model = new ChangeEmailOrMobileModel
                    {
                        Email = oUser.USR_EMAIL,
                        CurrentPassword ="",
                        MainPhoneNumberPrefix = oUser.USR_MAIN_TEL_COUNTRY.ToString(),
                        MainPhoneNumber = oUser.USR_MAIN_TEL,
                    };
               
               
            }
            catch(Exception)
            {
                bRes = false;
               
                oUser = null;

            }

            return bRes;
        }


        

        private bool SetDomainCustomerAndUserFromUserDataModel(ref USER oUser,
                                                               UserDataModel model)
        {
            bool bRes = true;

            try
            {
                oUser.USR_USERNAME = model.Username;
                oUser.CUSTOMER.CUS_FIRST_NAME = model.Name;
                oUser.CUSTOMER.CUS_SURNAME1 = model.Surname1;
                oUser.CUSTOMER.CUS_SURNAME2 = model.Surname2;
                oUser.CUSTOMER.CUS_DOC_ID = model.DocId;
                oUser.USR_SECUND_TEL_COUNTRY = Convert.ToDecimal(model.AlternativePhoneNumberPrefix);
                oUser.USR_SECUND_TEL = model.AlternativePhoneNumber;
                oUser.CUSTOMER.CUS_STREET = model.StreetName;
                oUser.CUSTOMER.CUS_STREE_NUMBER = Convert.ToInt32(model.StreetNumber);
                oUser.CUSTOMER.CUS_LEVEL_NUM = (model.LevelInStreetNumber != null &&
                                        model.LevelInStreetNumber.Length > 0) ? Convert.ToInt32(model.LevelInStreetNumber) : (int?)null;
                oUser.CUSTOMER.CUS_DOOR = model.DoorInStreetNumber;
                oUser.CUSTOMER.CUS_LETTER = model.LetterInStreetNumber;
                oUser.CUSTOMER.CUS_STAIR= model.StairInStreetNumber;
                oUser.CUSTOMER.CUS_COU_ID = Convert.ToInt32(model.Country);
                oUser.CUSTOMER.CUS_STATE = model.State;
                oUser.CUSTOMER.CUS_CITY=model.City;
                oUser.CUSTOMER.CUS_ZIPCODE = model.ZipCode;

                IList<string> Plates = new List<string>();

                foreach(SelectListItem item in model.Plates)
                {
                    Plates.Add(item.Text);
                }

                bRes = customersRepository.UpdateUser(ref oUser, Plates);

            }
            catch (Exception)
            {
                bRes = false;

            }

            return bRes;
        }


        private bool SetDomainUserSecurityOperationFromChangeEmailOrMobileModel(ref USER oUser,
                                                                                ChangeEmailOrMobileModel model,
                                                                                out USERS_SECURITY_OPERATION oSecOperation)
        {
            bool bRes = false;
            oSecOperation = null;
            
            try
            {
                
                oSecOperation = new USERS_SECURITY_OPERATION
                    {
                        USOP_NEW_EMAIL = model.Email,
                        USOP_NEW_MAIN_TEL_COUNTRY = Convert.ToInt32(model.MainPhoneNumberPrefix),
                        USOP_NEW_MAIN_TEL = model.MainPhoneNumber,
                        USOP_ACTIVATION_RETRIES = 0,
                        USOP_OP_TYPE = (int)SecurityOperationType.ChangeEmail_Telephone,
                        USOP_STATUS = (int)SecurityOperationStatus.Inserted,
                        USOP_LAST_SENT_DATE = DateTime.UtcNow,
                        USOP_UTCDATETIME = DateTime.UtcNow,
                        USOP_USR_ID = oUser.USR_ID
                    };

                bRes = customersRepository.AddSecurityOperation(ref oUser, oSecOperation);

            }
            catch (Exception)
            {
                bRes = false;

            }

            return bRes;
        }


        private bool SetDomainUserSecurityOperationForgotPassword(ref USER oUser,
                                                                  out USERS_SECURITY_OPERATION oSecOperation)
        {
            bool bRes = false;
            oSecOperation = null;
            
            try
            {

                string strPasswordRecoveryType = infraestructureRepository.GetParameterValue("PasswordRecoveryType");
                PasswordRecoveryType ePasswordRecoveryType = PasswordRecoveryType.Recover;

                if (!string.IsNullOrEmpty(strPasswordRecoveryType))
                {
                    try
                    {
                        ePasswordRecoveryType = (PasswordRecoveryType)Convert.ToInt32(strPasswordRecoveryType);
                    }
                    catch { }
                }


                oSecOperation = new USERS_SECURITY_OPERATION
                    {
                        USOP_NEW_EMAIL = oUser.USR_EMAIL,
                        USOP_NEW_MAIN_TEL = oUser.USR_MAIN_TEL,
                        USOP_NEW_MAIN_TEL_COUNTRY = oUser.USR_MAIN_TEL_COUNTRY,
                        USOP_ACTIVATION_RETRIES = 0,
                        USOP_OP_TYPE = (ePasswordRecoveryType == PasswordRecoveryType.Recover) ? (int)SecurityOperationType.RecoverPassword : (int)SecurityOperationType.ResetPassword,
                        USOP_STATUS = (int)SecurityOperationStatus.Inserted,
                        USOP_LAST_SENT_DATE = DateTime.UtcNow,
                        USOP_UTCDATETIME = DateTime.UtcNow,
                        USOP_USR_ID = oUser.USR_ID
                    };

                bRes = customersRepository.AddSecurityOperation(ref oUser, oSecOperation);

            }
            catch (Exception )
            {
                bRes = false;

            }

            return bRes;
        }


        

        private bool SendEmailAndSMS(ref USER oUser,USERS_SECURITY_OPERATION oSecOperation, string strResEmailHeader, string strResEmailBody, string strResSMS)
        {
            bool bRes = true;
            try
            {
                string requrl = "";
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
                {
                    requrl = Request.Url.ToString();
                }
                else
                {
                    requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
                }

                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                string url = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "SecurityOperation";
                string urlWithParam = url + "?code=" + oSecOperation.USOP_URL_PARAMETER;
                string strEmailSubject = strResEmailHeader;
                string strEmailBody = string.Format(strResEmailBody, urlWithParam, url);
                string strSMS = string.Format(strResSMS, oSecOperation.USOP_ACTIVATION_CODE);

                long lSenderId = infraestructureRepository.SendEmailTo(oSecOperation.USOP_NEW_EMAIL, strEmailSubject, strEmailBody, dSourceApp);

                if (lSenderId > 0)
                {
                    string strCompleteTelephone = "";
                    customersRepository.InsertUserEmail(ref oUser, oSecOperation.USOP_NEW_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                    lSenderId = infraestructureRepository.SendSMSTo(Convert.ToInt32(oSecOperation.USOP_NEW_MAIN_TEL_COUNTRY.Value), oSecOperation.USOP_NEW_MAIN_TEL, strSMS, dSourceApp, ref strCompleteTelephone);

                    if (lSenderId > 0)
                    {
                        customersRepository.InsertUserSMS(ref oUser, strCompleteTelephone, strSMS, lSenderId);
                    }

                }


            }
            catch
            {
                bRes = false;

            }

            return bRes;
        }

        private bool SendEmail(ref USER oUser, USERS_SECURITY_OPERATION oSecOperation, string strResEmailHeader, string strResEmailBody)
        {
            bool bRes = true;
            try
            {
                string requrl = "";
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
                {
                    requrl = Request.Url.ToString();
                }
                else
                {
                    requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Account/";
                }


                string url = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "SecurityOperation";
                string urlWithParam = url + "?code=" + oSecOperation.USOP_URL_PARAMETER;
                string strEmailSubject = strResEmailHeader;
                string strEmailBody = string.Format(strResEmailBody, urlWithParam, url);
                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;


                long lSenderId = infraestructureRepository.SendEmailTo(oSecOperation.USOP_NEW_EMAIL, strEmailSubject, strEmailBody, dSourceApp);

                if (lSenderId > 0)
                {                    
                    customersRepository.InsertUserEmail(ref oUser, oSecOperation.USOP_NEW_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                }


            }
            catch
            {
                bRes = false;

            }

            return bRes;
        }


        private bool ReSendSMS(USERS_SECURITY_OPERATION oSecOperation, string strResSMS)
        {
            bool bRes = true;
            try
            {
                USER oUser = oSecOperation.USER;
                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                string strSMS = string.Format(strResSMS, oSecOperation.USOP_ACTIVATION_CODE);
                string strCompleteTelephone = "";

                long lSenderId = infraestructureRepository.SendSMSTo(Convert.ToInt32(oSecOperation.USOP_NEW_MAIN_TEL_COUNTRY.Value), oSecOperation.USOP_NEW_MAIN_TEL, strSMS, dSourceApp, ref strCompleteTelephone);

                
                if (lSenderId > 0)
                {
                    customersRepository.InsertUserSMS(ref oUser, strCompleteTelephone, strSMS, lSenderId);
                }
            }
            catch
            {
                bRes = false;

            }

            return bRes;
        }

        private IQueryable<OperationRowModel> GetUserOperations(ref USER oUser,
                                                                int? Type,
                                                                DateTime? DateIni,
                                                                DateTime? DateEnd,
                                                                int? Plate,
                                                                GridSortOptions gridSortOptions,
                                                                int? page,
                                                                int pageSize,
                                                                out int iNumRows)
        {

            var predicate = PredicateBuilder.True<ALL_OPERATION>();


            if (Type.HasValue)
            {
                predicate = predicate.And(a => a.OPE_TYPE == Type);
            }

            if (DateIni.HasValue)
            {
                predicate = predicate.And(a => a.OPE_DATE >= DateIni);
            }

            if (DateEnd.HasValue)
            {
                predicate = predicate.And(a => a.OPE_DATE <= DateEnd);
            }

            if (Plate.HasValue)
            {
                predicate = predicate.And(a => a.USRP_ID == Plate || a.OPE_PLATE2_USRP_ID == Plate || a.OPE_PLATE3_USRP_ID == Plate || a.OPE_PLATE4_USRP_ID == Plate || a.OPE_PLATE5_USRP_ID == Plate || a.OPE_PLATE6_USRP_ID == Plate || a.OPE_PLATE7_USRP_ID == Plate || a.OPE_PLATE8_USRP_ID == Plate || a.OPE_PLATE9_USRP_ID == Plate || a.OPE_PLATE10_USRP_ID == Plate);
            }

            string orderbyField = "";
            switch (gridSortOptions.Column)
            {
                case "TypeId":
                    orderbyField = "OPE_TYPE";
                    break;
                case "Installation":
                    orderbyField = "INS_DESCRIPTION";
                    break;
                case "Date":
                    orderbyField = "OPE_DATE";
                    break;
                case "Plate":
                    orderbyField = "USRP_PLATE";
                    break;
                case "Amount":
                    orderbyField = "OPE_AMOUNT";
                    break;
                case "AmountFEE":
                    orderbyField = "OPE_FEE";
                    break;
                case "AmountBONUS":
                    orderbyField = "OPE_BONUS";
                    break;
                case "AmountVAT":
                    orderbyField = "OPE_VAT";
                    break;
                case "AmountTotal":
                    orderbyField = "OPE_TOTAL_AMOUNT";
                    break;
                case "ChangeApplied":
                    orderbyField = "OPE_CHANGE_APPLIED";
                    break;
                case "DateIni":
                    orderbyField = "OPE_INIDATE";
                    break;
                case "DateEnd":
                    orderbyField = "OPE_ENDDATE";
                    break;
                case "Time":
                    orderbyField = "OPE_TIME";
                    break;
                case "TicketNumber":
                    orderbyField = "TIPA_TICKET_NUMBER";
                    break;
                case "TicketData":
                    orderbyField = "TIPA_TICKET_DATA";
                    break;
                case "Sector":
                    orderbyField = "GRP_DESCRIPTION";
                    break;
                case "Tariff":
                    orderbyField = "TAR_DESCRIPTION";
                    break;                
                case "AdditionalUser":
                    orderbyField = "OPE_ADDITIONAL_USR_USERNAME";
                    break;
                default:
                    orderbyField = "OPE_DATE";
                    break;

            }

            IQueryable<OperationRowModel> modelOps = null;

            if (!page.HasValue && pageSize == -1)
            {
                iNumRows = -1;
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                modelOps = from domOp in customersRepository.GetUserOperations(ref oUser, predicate,
                                   orderbyField, gridSortOptions.Direction.ToString())
                           select new OperationRowModel
                           {
                               TypeId = domOp.OPE_TYPE,
                               Type = GetOperationStringType(domOp.OPE_TYPE, domOp.TAR_TYPE),
                               Installation = domOp.INS_DESCRIPTION,
                               Date = domOp.OPE_DATE,
                               DateIni = domOp.OPE_INIDATE,
                               DateEnd = domOp.OPE_ENDDATE,
                               Amount = Convert.ToDouble(domOp.OPE_AMOUNT),
                               AmountFEE = Convert.ToDouble(domOp.OPE_FEE),
                               AmountBONUS = Convert.ToDouble(domOp.OPE_BONUS),
                               AmountVAT = Convert.ToDouble(domOp.OPE_VAT),
                               AmountTotal = Convert.ToDouble(domOp.OPE_TOTAL_AMOUNT),
                               CurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                               Time = domOp.OPE_TIME,
                               ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                               PlateId = Convert.ToInt32(domOp.USRP_ID),
                               PlateId2 = Convert.ToInt32(domOp.OPE_PLATE2_USRP_ID),
                               PlateId3 = Convert.ToInt32(domOp.OPE_PLATE3_USRP_ID),
                               PlateId4 = Convert.ToInt32(domOp.OPE_PLATE4_USRP_ID),
                               PlateId5 = Convert.ToInt32(domOp.OPE_PLATE5_USRP_ID),
                               PlateId6 = Convert.ToInt32(domOp.OPE_PLATE6_USRP_ID),
                               PlateId7 = Convert.ToInt32(domOp.OPE_PLATE7_USRP_ID),
                               PlateId8 = Convert.ToInt32(domOp.OPE_PLATE8_USRP_ID),
                               PlateId9 = Convert.ToInt32(domOp.OPE_PLATE9_USRP_ID),
                               PlateId10 = Convert.ToInt32(domOp.OPE_PLATE10_USRP_ID),
                               Plate = domOp.USRP_PLATE,
                               Plate2 = domOp.USRP_PLATE2,
                               Plate3 = domOp.USRP_PLATE3,
                               Plate4 = domOp.USRP_PLATE4,
                               Plate5 = domOp.USRP_PLATE5,
                               Plate6 = domOp.USRP_PLATE6,
                               Plate7 = domOp.USRP_PLATE7,
                               Plate8 = domOp.USRP_PLATE8,
                               Plate9 = domOp.USRP_PLATE9,
                               Plate10 = domOp.USRP_PLATE10,
                               Plates = GetPlateList(domOp.USRP_PLATE, domOp.USRP_PLATE2, domOp.USRP_PLATE3, domOp.USRP_PLATE4, domOp.USRP_PLATE5, domOp.USRP_PLATE6, domOp.USRP_PLATE7, domOp.USRP_PLATE8, domOp.USRP_PLATE9, domOp.USRP_PLATE10),
                               TicketNumber = domOp.TIPA_TICKET_NUMBER,
                               TicketData = domOp.TIPA_TICKET_DATA,
                               Sector = domOp.GRP_DESCRIPTION,
                               Tariff = domOp.TAR_DESCRIPTION,
                               Source = GetOperationSourceString((OperationSourceType)domOp.EPO_SRCTYPE, domOp.EPO_SRCIDENT),                              
                               AdditionalUser = domOp.OPE_ADDITIONAL_USR_USERNAME,
                               CurrencyMinorUnit = domOp.OPE_AMOUNT_CUR_MINOR_UNIT
                           };
            }
            else
            {
                modelOps = from domOp in customersRepository.GetUserOperations(ref oUser, predicate,
                                   orderbyField, gridSortOptions.Direction.ToString(),
                                   page.GetValueOrDefault(1), pageSize, out iNumRows)
                               select new OperationRowModel
                               {
                                   TypeId = domOp.OPE_TYPE,
                                   Type = GetOperationStringType(domOp.OPE_TYPE, domOp.TAR_TYPE),
                                   Installation = domOp.INS_DESCRIPTION,
                                   Date = domOp.OPE_DATE,
                                   DateIni = domOp.OPE_INIDATE,
                                   DateEnd = domOp.OPE_ENDDATE,
                                   Amount = Convert.ToDouble(domOp.OPE_AMOUNT),
                                   AmountFEE = Convert.ToDouble(domOp.OPE_FEE),
                                   AmountVAT = Convert.ToDouble(domOp.OPE_VAT),
                                   AmountBONUS = Convert.ToDouble(domOp.OPE_BONUS),
                                   AmountTotal = Convert.ToDouble(domOp.OPE_TOTAL_AMOUNT),
                                   CurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                                   Time = domOp.OPE_TIME,
                                   ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                                   PlateId = Convert.ToInt32(domOp.USRP_ID),
                                   PlateId2 = Convert.ToInt32(domOp.OPE_PLATE2_USRP_ID),
                                   PlateId3 = Convert.ToInt32(domOp.OPE_PLATE3_USRP_ID),
                                   PlateId4 = Convert.ToInt32(domOp.OPE_PLATE4_USRP_ID),
                                   PlateId5 = Convert.ToInt32(domOp.OPE_PLATE5_USRP_ID),
                                   PlateId6 = Convert.ToInt32(domOp.OPE_PLATE6_USRP_ID),
                                   PlateId7 = Convert.ToInt32(domOp.OPE_PLATE7_USRP_ID),
                                   PlateId8 = Convert.ToInt32(domOp.OPE_PLATE8_USRP_ID),
                                   PlateId9 = Convert.ToInt32(domOp.OPE_PLATE9_USRP_ID),
                                   PlateId10 = Convert.ToInt32(domOp.OPE_PLATE10_USRP_ID),
                                   Plate = domOp.USRP_PLATE,
                                   Plate2 = domOp.USRP_PLATE2,
                                   Plate3 = domOp.USRP_PLATE3,
                                   Plate4 = domOp.USRP_PLATE4,
                                   Plate5 = domOp.USRP_PLATE5,
                                   Plate6 = domOp.USRP_PLATE6,
                                   Plate7 = domOp.USRP_PLATE7,
                                   Plate8 = domOp.USRP_PLATE8,
                                   Plate9 = domOp.USRP_PLATE9,
                                   Plate10 = domOp.USRP_PLATE10,
                                   Plates = GetPlateList(domOp.USRP_PLATE, domOp.USRP_PLATE2, domOp.USRP_PLATE3, domOp.USRP_PLATE4, domOp.USRP_PLATE5, domOp.USRP_PLATE6, domOp.USRP_PLATE7, domOp.USRP_PLATE8, domOp.USRP_PLATE9, domOp.USRP_PLATE10),
                                   TicketNumber = domOp.TIPA_TICKET_NUMBER,
                                   TicketData = domOp.TIPA_TICKET_DATA,
                                   Sector = domOp.GRP_DESCRIPTION,
                                   Tariff = domOp.TAR_DESCRIPTION,
                                   Source = GetOperationSourceString((OperationSourceType)domOp.EPO_SRCTYPE, domOp.EPO_SRCIDENT),
                                   AdditionalUser = domOp.OPE_ADDITIONAL_USR_USERNAME,
                                   CurrencyMinorUnit = domOp.OPE_AMOUNT_CUR_MINOR_UNIT
                               };
            }

            return modelOps;
        }

        private string GetPlateList(string P1, string P2, string P3, string P4, string P5, string P6, string P7, string P8, string P9, string P10)
        {
            string list = string.Empty;
            if (!string.IsNullOrEmpty(P1)) { list += string.Format(", {0}", P1); }
            if (!string.IsNullOrEmpty(P2)) { list += string.Format(", {0}", P2); }
            if (!string.IsNullOrEmpty(P3)) { list += string.Format(", {0}", P3); }
            if (!string.IsNullOrEmpty(P4)) { list += string.Format(", {0}", P4); }
            if (!string.IsNullOrEmpty(P5)) { list += string.Format(", {0}", P5); }
            if (!string.IsNullOrEmpty(P6)) { list += string.Format(", {0}", P6); }
            if (!string.IsNullOrEmpty(P7)) { list += string.Format(", {0}", P7); }
            if (!string.IsNullOrEmpty(P8)) { list += string.Format(", {0}", P8); }
            if (!string.IsNullOrEmpty(P9)) { list += string.Format(", {0}", P9); }
            if (!string.IsNullOrEmpty(P10)) { list += string.Format(", {0}", P10); }
            if (!string.IsNullOrEmpty(list)) { list = list.Substring(2); }
            return list;
        }

        private string GetOperationStringType(int opType, int? tarType)
        {
            string strRes = "";
            switch ((ChargeOperationsType)opType)
            {
                case ChargeOperationsType.ParkingOperation:
                    if (tarType == 1)
                    {
                        strRes = ResourceExtension.GetLiteral("Permits_Permit");
                    }
                    else
                    {
                        strRes = ResourceExtension.GetLiteral("Account_Op_Type_Parking");
                    }
                    break;
                case ChargeOperationsType.Permit:
                    strRes = ResourceExtension.GetLiteral("Permits_Permit");
                    break;
                case ChargeOperationsType.ExtensionOperation:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_Extension");
                    break;
                case ChargeOperationsType.ParkingRefund:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_Refund");
                    break;
                case ChargeOperationsType.TicketPayment:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_TicketPayment");
                    break;
                case ChargeOperationsType.BalanceRecharge:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_Recharge");
                    break;
                case ChargeOperationsType.ServiceCharge:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_ServiceCharge");
                    break;
                case ChargeOperationsType.Discount:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_Discount");
                    break;
                case ChargeOperationsType.OffstreetEntry:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_OffstreetEntry");
                    break;
                case ChargeOperationsType.OffstreetExit:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_OffstreetExit");
                    break;
                case ChargeOperationsType.OffstreetOverduePayment:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_OffstreetOverduePayment");
                    break;
                case ChargeOperationsType.BalanceTransfer:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_BalanceTransfer");
                    break;
                case ChargeOperationsType.BalanceReception:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_BalanceReception");
                    break;
                case ChargeOperationsType.TollPayment:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_TollPayment");
                    break;
                case ChargeOperationsType.TollLock:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_TollLock");
                    break;
                case ChargeOperationsType.TollUnlock:
                    strRes = ResourceExtension.GetLiteral("Account_Op_Type_TollUnlock");
                    break;
                default:
                    strRes = "";
                    break;


            }
            return strRes;


        }

        private string GetOperationSourceString(OperationSourceType srcType, string sSrcIdent)
        {            
            string sType = ResourceExtension.GetLiteral("Account_Op_SourceType_" + (srcType).ToString());
            string sIdent = "";
            if (!string.IsNullOrWhiteSpace(sSrcIdent))
            {
                sIdent = string.Format(ResourceExtension.GetLiteral("Account_Op_SourceIdent"), sSrcIdent);
            }
            return string.Format(ResourceExtension.GetLiteral("Account_Op_SourceType"), sType, sIdent); ;
        }


        

       

        private IQueryable<InvoiceRowModel> GetCustomerInvoices(ref USER oUser,
                                                                DateTime? DateIni,
                                                                DateTime? DateEnd,
                                                                GridSortOptions gridSortOptions,
                                                                int? page,
                                                                int pageSize,
                                                                out int iNumRows)
        {

            var predicate = PredicateBuilder.True<CUSTOMER_INVOICE>();



            if (DateIni.HasValue)
            {
                predicate = predicate.And(a => a.CUSINV_INV_DATE >= DateIni);
            }

            if (DateEnd.HasValue)
            {
                predicate = predicate.And(a => a.CUSINV_INV_DATE <= DateEnd);
            }

            predicate = predicate.And(a => a.CUSINV_INV_NUMBER != null);

            string orderbyField = "";
            switch (gridSortOptions.Column)
            {
                case "InvoiceId":
                    orderbyField = "CUSINV_ID";
                    break;
                case "InvoiceNumber":
                    orderbyField = "CUSINV_ID";
                    break;
                case "Date":
                    orderbyField = "CUSINV_INV_DATE";
                    break;              
                case "Amount":
                    orderbyField = "CUSINV_INV_AMOUNT";
                    break;
                case "AmountOps":
                    orderbyField = "CUSINV_INV_AMOUNT_OPS";
                    break;
                case "DownloadURL":
                    orderbyField = "CUSINV_ID";
                    break;
                default:
                    orderbyField = "CUSINV_INV_DATE";
                    break;

            }


            var modelInvoices = from domInv in customersRepository.GetUserInvoices(ref oUser, predicate,
                               orderbyField, gridSortOptions.Direction.ToString(),
                               page.GetValueOrDefault(1), pageSize, out iNumRows)
                           select new InvoiceRowModel
                           {
                               InvoiceId = domInv.CUSINV_ID,
                               InvoiceNumber = string.Format(domInv.OPERATOR.OPR_INVOICE_NUMBER_FORMAT,Convert.ToInt32(domInv.CUSINV_INV_NUMBER),domInv.CUSINV_INV_DATE),
                               Date = domInv.CUSINV_INV_DATE,
                               Amount = Convert.ToDouble(domInv.CUSINV_INV_AMOUNT),
                               AmountOps = domInv.CUSINV_INV_AMOUNT_OPS,
                               CurrencyIsoCode = domInv.CURRENCy.CUR_ISO_CODE,
                           };

            return modelInvoices;
        }





        bool AddPayMeanToUser(ref USER oUser,SelectPayMethodModel model)
        {


            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
            CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(dInstallationId);

            if (oGatewayConfig != null &&
                       !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {
                oGatewayConfig = oUser.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard))
                                        .FirstOrDefault();
            }

            decimal? dGatewayConfigId = ((oGatewayConfig != null) ? oGatewayConfig.CPTGC_ID : (decimal?)null);

            PaymentMeanCreditCardProviderType eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
            try
            {
                if (oGatewayConfig != null)
                {
                    eProviderType = (PaymentMeanCreditCardProviderType)oGatewayConfig.CPTGC_PROVIDER;
                }
            }
            catch
            {
                eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
            }

            decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

            return customersRepository.SetUserPaymentMean(ref oUser,
                                           infraestructureRepository,
                                           new CUSTOMER_PAYMENT_MEAN
                                            {
                                                CUSPM_PAT_ID = Convert.ToInt32(model.PaymentMean),
                                                CUSPM_PAST_ID = ((Convert.ToInt32(model.PaymentMean) == (int)PaymentMeanType.pmtPaypal) && (!model.AutomaticRecharge)) ? 
                                                                (int)PaymentMeanSubType.pmstPaypal : (int)PaymentMeanSubType.pmstUndefined,       
                                                CUSPM_CREDIT_CARD_PAYMENT_PROVIDER = (Convert.ToInt32(model.PaymentMean) == (int)PaymentMeanType.pmtDebitCreditCard ? (int)eProviderType : 
                                                                                                                                                                      (int)PaymentMeanCreditCardProviderType.pmccpUndefined),
                                                //CUSPM_DESCRIPTION=,
                                                //CUSPM_LAST_TIME_USERD=,
                                                CUSPM_AUTOMATIC_RECHARGE = model.AutomaticRecharge ? 1 : 0,
                                                CUSPM_AMOUNT_TO_RECHARGE = model.AutomaticRecharge ? Convert.ToInt32(model.AutomaticRechargeQuantity) : (int?)null,
                                                CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS = model.AutomaticRecharge ? Convert.ToInt32(model.AutomaticRechargeWhenBelowQuantity) : (int?)null,
                                                CUSPM_TOKEN_PAYPAL_ID = "",
                                                CUSPM_CUR_ID = oUser.USR_CUR_ID,
                                                CUSPM_VALID = (( Convert.ToInt32(model.PaymentMean) == (int)PaymentMeanType.pmtPaypal)&&(!model.AutomaticRecharge))?1:0,
                                                CUSPM_CPTGC_ID = dGatewayConfigId,
                                                CUSPM_CREATION_SOAPP_ID = dSourceApp,
                                                CUSPM_LAST_SOAPP_ID = dSourceApp,
                                            });


        }


        bool UpdateUserPayMean(ref USER oUser, CUSTOMER_PAYMENT_MEAN oUserPaymentMean, SelectPayMethodModel model)
        {
            bool bRes = true;
            bool bUserAutomaticRecharge = oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE > 0 ? true : false;
            int iModelAutomaticRechargeQuantity = Convert.ToInt32(model.AutomaticRechargeQuantity);
            int iModelAutomaticRechargeWhenBelowQuantity = Convert.ToInt32(model.AutomaticRechargeWhenBelowQuantity);
            string strPaypalID = oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID;

            if ((bUserAutomaticRecharge != model.AutomaticRecharge) ||
                ((oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE != iModelAutomaticRechargeQuantity) && (model.AutomaticRecharge)) ||
                ((oUserPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS != iModelAutomaticRechargeWhenBelowQuantity) && (model.AutomaticRecharge)) ||
                (oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID != strPaypalID))
            {
                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                bRes = customersRepository.UpdateUserPaymentMean(ref oUser, oUserPaymentMean,
                                         model.AutomaticRecharge ? 1 : 0,
                                         model.AutomaticRecharge ? iModelAutomaticRechargeQuantity : (int?)null,
                                         model.AutomaticRecharge ? iModelAutomaticRechargeWhenBelowQuantity : (int?)null,
                                         model.AutomaticRecharge ? strPaypalID : null, dSourceApp);

            }

            return bRes;

        }


        bool CopyCurrentUserPayMean(ref USER oUser, CUSTOMER_PAYMENT_MEAN oUserPaymentMean, SelectPayMethodModel model)
        {
            bool bRes = true;
            bool bUserAutomaticRecharge = oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE > 0 ? true : false;
            int iModelAutomaticRechargeQuantity = Convert.ToInt32(model.AutomaticRechargeQuantity);
            int iModelAutomaticRechargeWhenBelowQuantity = Convert.ToInt32(model.AutomaticRechargeWhenBelowQuantity);
            string strPaypalID = oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID;


            bRes = customersRepository.CopyCurrentUserPaymentMean(ref oUser, oUserPaymentMean,
                                        model.AutomaticRecharge ? 1 : 0,
                                        model.AutomaticRecharge ? iModelAutomaticRechargeQuantity : (int?)null,
                                        model.AutomaticRecharge ? iModelAutomaticRechargeWhenBelowQuantity : (int?)null,
                                        model.AutomaticRecharge ? strPaypalID : null);



            return bRes;

        }




        bool UpdateUserPayMean(ref USER oUser, CUSTOMER_PAYMENT_MEAN oUserPaymentMean, RechargeModel model)
        {
            bool bRes = true;
            bool bUserAutomaticRecharge = oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE > 0 ? true : false;
            int iModelAutomaticRechargeQuantity = Convert.ToInt32(model.AutomaticRechargeQuantity);
            int iModelAutomaticRechargeWhenBelowQuantity= Convert.ToInt32(model.AutomaticRechargeWhenBelowQuantity);
            string strPaypalID = model.PaypalID;

            if ((bUserAutomaticRecharge != model.AutomaticRecharge) ||
                ((oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE != iModelAutomaticRechargeQuantity) && (model.AutomaticRecharge)) ||
                ((oUserPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS != iModelAutomaticRechargeWhenBelowQuantity) && (model.AutomaticRecharge)) ||
                (oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID != strPaypalID))
            {

                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                bRes = customersRepository.UpdateUserPaymentMean(ref oUser, oUserPaymentMean,
                                         model.AutomaticRecharge ? 1 : 0,
                                         model.AutomaticRecharge ? iModelAutomaticRechargeQuantity : (int?)null,
                                         model.AutomaticRecharge ? iModelAutomaticRechargeWhenBelowQuantity : (int?)null,
                                         model.AutomaticRecharge ? strPaypalID: null, dSourceApp);

            }

            return bRes;

        }




        bool UpdateUserPaypalPreapprovalPaymentMean(ref USER oUser, string preapprovalKey,
                                                    DateTime startingDate,
                                                    DateTime endingDate,
                                                    int maxNumberOfPayments,
                                                    decimal maxAmountPerPayment,
                                                    bool maxAmountPerPaymentSpecified,
                                                    decimal maxTotalAmountOfAllPayments,
                                                    bool maxTotalAmountOfAllPaymentsSpecified)
        {

            bool bRes = customersRepository.UpdateUserPaypalPreapprovalPaymentMean(ref oUser,
                                                            preapprovalKey,
                                                            startingDate,
                                                            endingDate,
                                                            maxNumberOfPayments,
                                                            maxAmountPerPaymentSpecified ? maxAmountPerPayment : (decimal?)null,
                                                            maxTotalAmountOfAllPaymentsSpecified?maxTotalAmountOfAllPayments:(decimal?)null);
            

            return bRes;

        }


        private void SendRechargeEmail(ref USER oUser,decimal? dRechargeId)
        {

            try
            {
                if (dRechargeId != null)
                {
                    CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                    if (customersRepository.GetRechargeData(ref oUser, dRechargeId.Value, out oRecharge))
                    {
                        if ((PaymentSuscryptionType)oRecharge.CUSPMR_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPrepay)
                        {
                            int iQuantity = oRecharge.CUSPMR_AMOUNT;
                            decimal dPercVAT1 = oRecharge.CUSPMR_PERC_VAT1 ?? 0;
                            decimal dPercVAT2 = oRecharge.CUSPMR_PERC_VAT2 ?? 0;
                            decimal dPercFEE = oRecharge.CUSPMR_PERC_FEE ?? 0;
                            int iPercFEETopped = (int)(oRecharge.CUSPMR_PERC_FEE_TOPPED ?? 0);
                            int iFixedFEE = (int)(oRecharge.CUSPMR_FIXED_FEE ?? 0);

                            int iPartialVAT1;
                            int iPartialPercFEE;
                            int iPartialFixedFEE;
                            int iPartialPercFEEVAT;
                            int iPartialFixedFEEVAT;

                            int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                            int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                            if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                            iQFEE += iFixedFEE;
                            int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                            int iQSubTotal = iQuantity + iQFEE;

                            int iLayout = 0;
                            if (iQFEE != 0 || iQVAT != 0)
                            {
                                OPERATOR oOperator = customersRepository.GetDefaultOperator();
                                if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                            }


                            string sLayoutSubtotal = "";
                            string sLayoutTotal = "";

                            string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID));

                            if (iLayout == 2)
                            {
                                sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                                string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode));
                            }
                            else if (iLayout == 1)
                            {
                                sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                             string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                             string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                             (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                             (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                             string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode));
                            }                           

                            string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmNoAutomaticRecharge_EmailHeader");
                            /*
                                ID: {0}<br>
                             *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                             *  Cantidad Recargada: {2} 
                             */
                            string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmRecharge_EmailBody"),
                                oRecharge.CUSPMR_ID,
                                oRecharge.CUSPMR_DATE,
                                string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                              infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                              infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                string.Format("{0}{1}{2}",Request.Url.GetLeftPart(UriPartial.Authority),Url.Content("~"),ConfigurationManager.AppSettings["EmailSignatureGraphic"] ),
                                sLayoutSubtotal, sLayoutTotal,
                                GetEmailFooter(ref oUser));
                                                              

                            SendEmail(ref oUser, strRechargeEmailSubject, strRechargeEmailBody);

                        }
                    }
                }
            }
            catch { }
        }

        private bool SendEmail(ref USER oUser, string strEmailSubject, string strEmailBody)
        {
            bool bRes = true;
            try
            {
                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                long lSenderId = infraestructureRepository.SendEmailTo(oUser.USR_EMAIL, strEmailSubject, strEmailBody, dSourceApp);

                if (lSenderId > 0)
                {
                    customersRepository.InsertUserEmail(ref oUser, oUser.USR_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                }

            }
            catch
            {
                bRes = false;
            }

            return bRes;
        }


        private bool CCSuccess(string strReference,
                                string strTransactionId,
                                string strCFTransactionId,
                                string strGatewayDate,
                                string strAuthCode,
                                string strAuthResult,
                                string strAuthResultDesc,
                                string strCardHash,
                                string strCardReference,
                                string strCardScheme,
                                string strMaskedCardNumber,
                                PaymentMeanRechargeStatus rechargeStatus,
                                DateTime? dtExpDate,
                                string strName,
                                string strDocumentId)
        {
            bool bRes = false;
            try
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"].ToString();
                int iQuantity = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRechargeBase"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));

                USER oUser = GetUserFromSession();

                ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
                
                decimal? dRechargeId = null;
                bool bRefundServiceCharge = false;

                decimal dPercVAT1 = 0;
                decimal dPercVAT2 = 0;
                decimal dPercFEE = 0;
                int iPercFEETopped = 0;
                int iFixedFEE = 0;
                

                int iPartialVAT1=0;
                int iPartialPercFEE=0;
                int iPartialFixedFEE=0;

                int iTotalQuantity = iQuantity;

                if (chargeType == ChargeOperationsType.ServiceCharge)
                {
                    bRefundServiceCharge = true;
                    rechargeStatus = rechargeStatus == PaymentMeanRechargeStatus.Waiting_Commit ? PaymentMeanRechargeStatus.Waiting_Cancellation : PaymentMeanRechargeStatus.Waiting_Refund;
                }
                else
                {

                    dPercVAT1 = (Session["PercVAT1"] != null ? Convert.ToDecimal(Session["PercVAT1"].ToString(), provider) : 0);
                    dPercVAT2 = (Session["PercVAT2"] != null ? Convert.ToDecimal(Session["PercVAT2"].ToString(), provider) : 0);
                    dPercFEE = (Session["PercFEE"] != null ? Convert.ToDecimal(Session["PercFEE"].ToString(), provider) : 0);
                    iPercFEETopped = (Session["PercFEETopped"] != null ? Convert.ToInt32(Session["PercFEETopped"], provider) : 0);
                    iFixedFEE = (Session["FixedFEE"] != null ? Convert.ToInt32(Session["FixedFEE"], provider) : 0);
                    iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

                }
                PaymentMeanRechargeCreationType rechargeCreationType = PaymentMeanRechargeCreationType.pmrctRegularRecharge;

                if (Session["RechargeCreationType"] != null)
                {
                    rechargeCreationType = (PaymentMeanRechargeCreationType)Convert.ToInt32(Session["RechargeCreationType"]);
                }

                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                if (customersRepository.RechargeUserBalance(ref oUser,
                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                            Convert.ToInt32(MobileOS.Web),
                                                            (chargeType == ChargeOperationsType.BalanceRecharge),
                                                            iQuantity,iQuantity,
                                                            dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                                            infraestructureRepository.GetCurrencyFromIsoCode(Session["CurrencyToRecharge"].ToString()),
                                                            (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE,
                                                            rechargeStatus,
                                                            rechargeCreationType,
                    //0,
                                                            strReference,
                                                            strTransactionId,
                                                            strCFTransactionId,
                                                            strGatewayDate,
                                                            strAuthCode,
                                                            strAuthResult,
                                                            strAuthResultDesc,
                                                            strCardHash,
                                                            strCardReference,
                                                            strCardScheme,
                                                            strMaskedCardNumber,
                                                            strName,
                                                            strDocumentId,
                                                            dtExpDate,
                                                            null,
                                                            null,
                                                            null,
                                                            (bool)Session["OVERWRITE_CARD"],
                                                            null, null, null,"","","", "","",null, null, dSourceApp,
                                                            infraestructureRepository,
                                                            out dRechargeId))
                {

                    Session["USER_ID"] = oUser.USR_ID;
                    Session["OVERWRITE_CARD"] = false;

                    if (chargeType == ChargeOperationsType.BalanceRecharge)
                    {



                        ViewData["oUser"] = oUser;
                        ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                        ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                        ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

                        ViewData["PayerQuantity"] = Convert.ToDouble(oUser.USR_BALANCE);
                        ViewData["PayerCurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

                        SendRechargeEmail(ref oUser, dRechargeId);



                    }
                    else if (chargeType == ChargeOperationsType.ServiceCharge)
                    {
                        Session["OVERWRITE_CARD"] = false;
                        decimal dOperationID = -1;
                        /*if (!bRefundServiceCharge)
                        {
                            if (customersRepository.ChargeServiceOperation(ref oUser,
                                                    Convert.ToInt32(MobileOS.Web),
                                                    false,
                                                    ServiceChargeType.NewPaymentMean,
                                                    PaymentSuscryptionType.pstPerTransaction,
                                                    DateTime.Now,
                                                    DateTime.UtcNow,
                                                    Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * 100),
                                                    infraestructureRepository.GetCurrencyFromIsoCode(Session["CurrencyToRecharge"].ToString()),
                                                    infraestructureRepository.GetCurrencyFromIsoCode(Session["CurrencyToRecharge"].ToString()),
                                                    1.0,
                                                    0,
                                                    Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * 100),
                                                    dRechargeId,
                                                    out dOperationID))
                            {
                                Session["USER_ID"] = oUser.USR_ID;
                                ViewData["oUser"] = oUser;
                                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                                ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                                ViewData["DiscountValue"] = string.Format("{0:0.00}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / 100);
                                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];
                            }
                        }
                        else
                        {*/
                            Session["USER_ID"] = oUser.USR_ID;
                            ViewData["oUser"] = oUser;
                            ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}", Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
                            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];

                        //}
                    }

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CCSuccess: PAN={0}; Card Reference={1}, Quantity={2} {3}",
                                                                strMaskedCardNumber, strCardReference, Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString())),
                                                                Session["CurrencyToRecharge"].ToString()));

                    bRes = true;
                    Session["Sess_strReference"] = null;
                    Session["Sess_strTransactionId"] = null;
                    Session["Sess_strGatewayDate"] = null;
                    Session["Sess_strAuthCode"] = null;
                    Session["Sess_strAuthResult"] = null;
                    Session["Sess_strAuthResultDesc"] = null;
                    Session["Sess_strCardHash"] = null;
                    Session["Sess_strCardReference"] = null;
                    Session["Sess_strCardScheme"] = "";
                    Session["Sess_strMaskedCardNumber"] = null;
                    Session["Sess_dtExpDate"] = null;
                    Session["QuantityToRecharge"] = null;
                    Session["QuantityToRechargeBase"] = null;
                    Session["PercVAT1"] = null;
                    Session["PercVAT2"] = null;
                    Session["PercFEE"] = null;
                    Session["PercFEETopped"] = null;
                    Session["FixedFEE"] = null;
                    Session["CurrencyToRecharge"] = null;
                    Session["InIECISAPayment"] = null;
                    Session["RechargeCreationType"] = null;
                    Session["ReturnToPermits"] = null;
                    Session["InstallationID"] = null;


                }


               
                if (!bRes)
                    customersRepository.FailedRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                        oUser.USR_EMAIL,
                                                        strTransactionId,
                                                        rechargeStatus == PaymentMeanRechargeStatus.Waiting_Commit ? PaymentMeanRechargeStatus.Waiting_Cancellation : PaymentMeanRechargeStatus.Waiting_Refund);


            }
            catch
            {
                bRes = false;
            }


            return bRes;

        }




        private bool CCPreapprovalSuccess()
        {
            bool bRes = false;
            try
            {

                USER oUser = GetUserFromSession();
                ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                decimal? dRechargeId = null;

                decimal dPercVAT1 = (Session["PercVAT1"] != null ? Convert.ToDecimal(Session["PercVAT1"].ToString(), provider) : 0);
                decimal dPercVAT2 = (Session["PercVAT1"] != null ? Convert.ToDecimal(Session["PercVAT2"].ToString(), provider) : 0);
                decimal dPercFEE = (Session["PercVAT1"] != null ? Convert.ToDecimal(Session["PercFEE"].ToString(), provider) : 0);
                int iPercFEETopped = (Session["PercFEETopped"] != null ? Convert.ToInt32(Session["PercFEETopped"]) : 0);
                int iFixedFEE = (Session["FixedFEE"] != null ? Convert.ToInt32(Session["FixedFEE"]) : 0);
                int iQuantity = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRechargeBase"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));

                int iPartialVAT1;
                int iPartialPercFEE;
                int iPartialFixedFEE;

                int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

                PaymentMeanRechargeCreationType rechargeCreationType = PaymentMeanRechargeCreationType.pmrctRegularRecharge;

                if (Session["RechargeCreationType"] != null)
                {
                    rechargeCreationType = (PaymentMeanRechargeCreationType)Session["RechargeCreationType"];
                }

                string strCFTransactionID = Session["CCCFTransactionId"] != null ? Session["CCCFTransactionId"].ToString() : (string)null;

                decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, dInstallationId);

                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                if (customersRepository.RechargeUserBalance(ref oUser,
                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                            Convert.ToInt32(MobileOS.Web),
                                                            (chargeType == ChargeOperationsType.BalanceRecharge),
                                                            iQuantity,iQuantity,
                                                            dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                                            infraestructureRepository.GetCurrencyFromIsoCode(Session["CurrencyToRecharge"].ToString()),
                                                            (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE,
                                                            (PaymentMeanRechargeStatus)Session["CCRechargeStatus"],
                                                            rechargeCreationType,
                                                            //0,
                                                            Session["CCUserReference"].ToString(),
                                                            Session["CCTransactionId"].ToString(),
                                                            strCFTransactionID,
                                                            Session["CCGatewayDate"].ToString(),
                                                            Session["CCAuthCode"].ToString(),
                                                            Session["CCAuthResult"].ToString(),
                                                            Session["CCAuthResultDesc"].ToString(),
                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                            Session["CCCardScheme"].ToString(),
                                                            oUserPaymentMean.CUSPM_TOKEN_MASKED_CARD_NUMBER,
                                                            null,
                                                            null,
                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE,
                                                            null,
                                                            null,
                                                            null,
                                                            false,
                                                            null, null, null,"","","","","",null,null,dSourceApp, 
                                                            infraestructureRepository,
                                                            out dRechargeId))
                {

                    Session["USER_ID"] = oUser.USR_ID;
                    ViewData["PayerQuantity"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["PayerCurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;

                    Session["CCUserReference"] = null;
                    Session["CCAuthCode"] = null;
                    Session["CCAuthResult"] = null;
                    Session["CCGatewayDate"] = null;
                    Session["CCCardScheme"] = null;
                    Session["CCTransactionId"] = null;

                    Session["QuantityToRecharge"] = null;
                    Session["QuantityToRechargeBase"] = null;
                    Session["PercVAT1"] = null;
                    Session["PercVAT2"] = null;
                    Session["PercFEE"] = null;
                    Session["PercFEETopped"] = null;
                    Session["FixedFEE"] = null;
                    Session["CurrencyToRecharge"] = null;
                    Session["InIECISAPayment"] = null;
                    Session["RechargeCreationType"] = null;
                    Session["ReturnToPermits"] = null;
                    Session["InstallationID"] = null;

                    
                    
                    ViewData["oUser"] = oUser;
                    ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    ViewData["CurrencyISOCode"] = oUserPaymentMean.CURRENCy.CUR_ISO_CODE;
                    SendRechargeEmail(ref oUser, dRechargeId);
                    bRes = true;
                }

                if (!bRes)
                    customersRepository.FailedRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                        oUser.USR_EMAIL,
                                                        Session["CCTransactionId"].ToString(),
                                                       (PaymentMeanRechargeStatus)Session["CCRechargeStatus"] == PaymentMeanRechargeStatus.Waiting_Commit ? PaymentMeanRechargeStatus.Waiting_Cancellation : PaymentMeanRechargeStatus.Waiting_Refund);

            }
            catch
            {
                bRes = false;
            }

            return bRes;

        }



        public USER GetUserFromSession()
        {
            USER oUser = null;
            SetCulture();
            try
            {
                if (Session["USER_ID"]!=null)
                {
                    decimal dUserId = Convert.ToDecimal(Session["USER_ID"]);
                    if (!customersRepository.GetUserDataById(ref oUser, dUserId))
                    {
                        oUser = null;

                    }
                    else
                    {
                        ViewData["SuscriptionTypeGeneral"] = oUser.USR_SUSCRIPTION_TYPE;
                    }

                }
               
            }
            catch
            {
                oUser = null;
            }

            return oUser;

        }

        public USERS_SECURITY_OPERATION GetSecurityOperationFromSession()
        {
            USERS_SECURITY_OPERATION oSecurityOp= null;
            try
            {
                if (Session["SecOperationID"]!=null)
                {
                    decimal dSecOpId = (decimal)Session["SecOperationID"];
                    oSecurityOp = customersRepository.GetUserSecurityOperation(dSecOpId);
                }
               
            }
            catch
            {
                oSecurityOp = null;
            }

            return oSecurityOp;

        }

        public double GetPayPerTransactionChargeAmount(string strCurrencyISOCode)
        {
            double dRes = 0;
            try
            {
                List<string> oPerTransactionParameters = new List<string>();

                oPerTransactionParameters = ConfigurationManager.AppSettings["SuscriptionType2_AddPayMethChargeValue"].ToString().Split(';').ToList();

                for (int i = 0; i < oPerTransactionParameters.Count; i++)
                {
                    if (oPerTransactionParameters[i] == strCurrencyISOCode)
                    {
                        dRes = Convert.ToDouble(oPerTransactionParameters[i + 1]) / (double)infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurrencyISOCode);
                        break;
                    }

                }


            }
            catch
            {                
            }

            return dRes;

        }





        #region Export Methods

        private void ExportExcel(Type modelType, IQueryable rows, string[] columns, string[] headers, MemoryStream output)
        {
            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = ExportExcel_CreateSheet(workbook, columns, headers, modelType);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (object row in rows)
            {
                if (row.GetType()==typeof(OperationRowModel))
                {
                    ((OperationRowModel)row).RecalculateAmountStrings();
                }


                if (rowNumber >= 0xFFFF)
                {
                    sheet = ExportExcel_CreateSheet(workbook, columns, headers, modelType);
                    rowNumber = 1;
                }
                //Create a new row
                var sheetRow = sheet.CreateRow(rowNumber++);

                var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;

                //Set values for the cells
                int j = 0;
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != "")
                    {
                        string value = "";
                        string value2 = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(columns[i] + "_FK");
                        if (propInfo == null) propInfo = row.GetType().GetProperty(columns[i]);
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null)
                        {
                            if (propInfo.PropertyType != typeof(DateTime) && propInfo.PropertyType != typeof(DateTime?))
                                value = obj.ToString();
                            else
                            {
                                value = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortDatePattern);
                                value2 = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortTimePattern);
                            }
                        }
                        sheetRow.CreateCell(i + j).SetCellValue(value);
                        if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?))
                        {
                            j += 1;
                            sheetRow.CreateCell(i + j).SetCellValue(value2);
                        }
                    }
                }
            }

            //Write the workbook to a memory stream            
            workbook.Write(output);

        }

        private NPOI.SS.UserModel.ISheet ExportExcel_CreateSheet(HSSFWorkbook workbook, string[] columns, string[] headers, Type modelType)
        {
            var sheet = workbook.CreateSheet();

            /*for (int i = 0; i < columns.Length; i++)
            {
                sheet.SetColumnWidth(i, 10 * 256);
            }*/

            var headerRow = sheet.CreateRow(0);

            PropertyInfo propInfo = null;
            int j = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    headerRow.CreateCell(i + j).SetCellValue(ResourceExtension.GetLiteral(headers[i]));
                    propInfo = modelType.GetProperty(columns[i]);
                    if (propInfo != null && (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)))
                    {
                        j += 1;
                        headerRow.CreateCell(i + j).SetCellValue(ResourceExtension.GetLiteral(headers[i] + "_Time"));;
                    }
                }
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            return sheet;
        }

        private void ExportPdf(Type modelType, IQueryable rows, string[] columns, string[] headers, MemoryStream output)
        {
            Rectangle pageSize = PageSize.A4;
            if (columns.Length > 5) pageSize = pageSize.Rotate();

            var document = new Document(pageSize, 10, 10, 10, 10);

            PdfWriter.GetInstance(document, output);

            document.Open();

            var numOfColumns = columns.Count(e => e != "");
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;

            dataTable.DefaultCell.BorderWidth = 2;
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Font hFont = new Font(Font.FontFamily.COURIER, 8, Font.BOLD);
            Font rFont = new Font(Font.FontFamily.COURIER, 8, Font.NORMAL);

            // Adding headers
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    dataTable.AddCell(new PdfPCell(new Phrase(ResourceExtension.GetLiteral(headers[i]), hFont)));
                    //dataTable.AddCell(ResourceExtension.GetLiteral("ResourceManager.GetString("Account_Op_" + columns[i]));                
                }
            }

            dataTable.HeaderRows = 1;
            dataTable.DefaultCell.BorderWidth = 1;

            long iCount = 0;
            foreach (object row in rows)
            {
                if (row.GetType() == typeof(OperationRowModel))
                {
                    ((OperationRowModel)row).RecalculateAmountStrings();
                }

                foreach (string column in columns)
                {
                    if (column != "")
                    {
                        string value = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(column + "_FK");
                        if (propInfo == null) propInfo = row.GetType().GetProperty(column);
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null) value = obj.ToString();
                        dataTable.AddCell(new PdfPCell(new Phrase(value, rFont)));
                    }
                }
                iCount++;
            }

            if (iCount == 0)
            {
                for (int i = 0; i < columns.Length; i++)
                    if (columns[i] != "") dataTable.AddCell("");
            }

            document.Add(dataTable);

            document.Close();

        }

        #endregion

        #region Invoice Methods

        private string ExportInvoicePdf(CUSTOMER_INVOICE oInvoice, string sServerPath, string sFileName)
        {
            string sRet = "";

            OPERATOR oOperator = oInvoice.OPERATOR;

            InvoicePdfGenerator pdfGenerator = new InvoicePdfGenerator(sServerPath,
                            oOperator.OPR_INVOICE_FORMAT_LAST_PAGE_FILE,
                            oOperator.OPR_INVOICE_FORMAT_NO_LAST_PAGE_FILE,
                            sFileName);
            pdfGenerator.TestMode = false;

            InvoiceData invData = new InvoiceData();
            invData.CompanyName = oOperator.OPR_NAME_FOR_INVOICE;
            invData.CompanyInfo = ResourceExtension.GetLiteral("Account_Invoice_PDF_NIF") + oOperator.OPR_VAT_NUMBER + "\n" + oOperator.OPR_ADDRESS_FOR_INVOICE.Replace("\\n", "\n");
            invData.CustomerName = !string.IsNullOrEmpty(oInvoice.CUSTOMER.CUS_NAME) ? oInvoice.CUSTOMER.CUS_NAME : oInvoice.CUSTOMER.CUS_FIRST_NAME + " " + oInvoice.CUSTOMER.CUS_SURNAME1 + " " + oInvoice.CUSTOMER.CUS_SURNAME2;
            invData.CustomerInfo = oInvoice.CUSTOMER.CUS_STREET + " " + oInvoice.CUSTOMER.CUS_STREE_NUMBER + " " + oInvoice.CUSTOMER.CUS_LETTER +
                                    " " + oInvoice.CUSTOMER.CUS_LEVEL_NUM + " " + oInvoice.CUSTOMER.CUS_DOOR + "\n" +
                                    oInvoice.CUSTOMER.CUS_ZIPCODE + " " + oInvoice.CUSTOMER.CUS_CITY + "(" + oInvoice.CUSTOMER.COUNTRy.COU_DESCRIPTION + ")";
            invData.NIF = oInvoice.CUSTOMER.CUS_DOC_ID;
            invData.Post = "";
            invData.Date = oInvoice.CUSINV_INV_DATE.Value.ToString("dd/MM/yyyy");
            invData.Ref = string.Format(oOperator.OPR_INVOICE_NUMBER_FORMAT, Convert.ToInt32(oInvoice.CUSINV_INV_NUMBER), oInvoice.CUSINV_INV_DATE);
            invData.Contract = "";
            invData.InvoiceNum = "";
            invData.TotalBase = (Convert.ToDouble(oInvoice.CUSINV_INV_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("###########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE;
            invData.TotalIVA = "0 " + oInvoice.CURRENCy.CUR_ISO_CODE;
            invData.Total = invData.TotalBase;



            invData.LabelNIF = ResourceExtension.GetLiteral("Account_Invoice_PDF_DNI_NIF");
            invData.LabelPost = "";
            invData.LabelDate = ResourceExtension.GetLiteral("Account_Invoice_PDF_Date");
            invData.LabelRef = ResourceExtension.GetLiteral("Account_Invoice_PDF_InvoiceNumber");
            invData.LabelContract = "";
            invData.LabelInvoiceNum = "";
            invData.LabelTotalBase = ResourceExtension.GetLiteral("Account_Invoice_PDF_BaseAmount");
            invData.LabelTotalIVA = ResourceExtension.GetLiteral("Account_Invoice_PDF_VAT");
            invData.LabelTotal = ResourceExtension.GetLiteral("Account_Invoice_PDF_Total");
            invData.LabelLineUnits = ResourceExtension.GetLiteral("Account_Invoice_PDF_Units");
            invData.LabelLineDescription = ResourceExtension.GetLiteral("Account_Invoice_PDF_Detail");
            invData.LabelLinePrice = ResourceExtension.GetLiteral("Account_Invoice_PDF_Unit_Amount");
            invData.LabelLineAmount = ResourceExtension.GetLiteral("Account_Invoice_PDF_Amount");
            invData.LabelFooter = "Domicilio social: Cardenal Marcelo Spinola, 50-52 28016 Madrid. Inscripta en el REgistro Mercantil de Madrid ...";








            foreach (CUSTOMER_PAYMENT_MEANS_RECHARGES_HI oRecharge in oInvoice.CUSTOMER_PAYMENT_MEANS_RECHARGES_HIs.OrderBy(r => r.CUSPMR_DATE))
            {
                if (oRecharge.CUSPMR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                {
                    invData.AddLine(new InvoiceLineData("1",
                                                         string.Format("{0} {1:dd/MM/yyyy HH:mm}", ResourceExtension.GetLiteral("Account_Op_Type_Recharge"), oRecharge.CUSPMR_DATE),
                                                          (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                                          (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                }
                else
                {

                    if (oRecharge.HIS_OPERATIONs.Count() > 0)
                    {
                        HIS_OPERATION oOper = oRecharge.HIS_OPERATIONs.First();
                        string strType = "";
                        if (oOper.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation)
                        {
                            strType = ResourceExtension.GetLiteral("Account_Op_Type_Parking");
                        }
                        else if (oOper.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation)
                        {
                            strType = ResourceExtension.GetLiteral("Account_Op_Type_Extension");
                        }

                        invData.AddLine(new InvoiceLineData("1",
                                        string.Format("{0} {1} {2} {3:dd/MM/yyyy HH:mm} ", strType,
                                        oOper.USER_PLATE.USRP_PLATE,
                                        oOper.INSTALLATION.INS_DESCRIPTION,
                                        oOper.OPE_DATE),
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE));
                    }
                    else if (oRecharge.TICKET_PAYMENTs.Count() > 0)
                    {
                        TICKET_PAYMENT oTicket = oRecharge.TICKET_PAYMENTs.First();

                        invData.AddLine(new InvoiceLineData("1",
                                        string.Format("{0} {1} {2} {3:dd/MM/yyyy HH:mm}", ResourceExtension.GetLiteral("Account_Op_Type_TicketPayment"),
                                                oTicket.TIPA_TICKET_NUMBER,
                                                oTicket.INSTALLATION.INS_DESCRIPTION,
                                                oTicket.TIPA_DATE),
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                    }
/*                    else if (oRecharge..Count() > 0)
                    {
                        invData.AddLine(new InvoiceLineData("1",
                                        string.Format("{0} {1:dd/MM/yyyy HH:mm}", ResourceExtension.GetLiteral("Account_Op_Type_ServiceCharge"), oRecharge.SERVICE_CHARGEs.First().SECH_DATE),
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)).ToString("##########" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInvoice.CURRENCy.CUR_ISO_CODE)) + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                    }*/



                }

            }


            pdfGenerator.Data = invData;

            if (pdfGenerator.generatePdf())
            {
                sRet = pdfGenerator.generatedPDFPath();
            }

            return sRet;
        }

        private string ExportInvoiceReportPdf(CUSTOMER_INVOICE oInvoice, string sServerPath, string sFileName)
        {
            //integraMobile.Reports.ReportHelper.CurrentPlugin = "PBPPlugin";

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            // set any deviceInfo settings if necessary
            System.Collections.Hashtable deviceInfo =
                new System.Collections.Hashtable();

            Telerik.Reporting.TypeReportSource typeReportSource =
                         new Telerik.Reporting.TypeReportSource();

            // reportName is the Assembly Qualified Name of the report
            Type oReportType = null;
            try
            {
                oReportType = System.Type.GetType(ConfigurationManager.AppSettings["invoiceReportSource"] ?? "integraMobile.Reports.Invoicing.eysa_invoice");
            }
            catch (Exception ex) { }
            if (oReportType == null) oReportType = typeof(integraMobile.Reports.Invoicing.eysa_invoice);
            typeReportSource.TypeName = oReportType.AssemblyQualifiedName;            
            typeReportSource.Parameters.Add(new Telerik.Reporting.Parameter("InvoiceId", oInvoice.CUSINV_ID));

            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", typeReportSource, deviceInfo);

            //string path = System.IO.Path.GetTempPath();
            string sFilePath = System.IO.Path.Combine(sServerPath, sFileName);

            using (System.IO.FileStream fs = new System.IO.FileStream(sFilePath, System.IO.FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }

            return sFilePath;
        }

        #endregion

        [HttpGet]
        public ActionResult StreetSectionPackage(string mose_session)
        {

            bool bRet = false;

            USER oUser = null;
            try
            {
                decimal dInstallationID = -1;
                if (customersRepository.GetUserFromOpenSession(mose_session, out dInstallationID, ref oUser))
                {
                    byte[] oFile = null;

                    if (infraestructureRepository.GetLastStreetSectionPackage(dInstallationID, out oFile))
                    {
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/x-compressed";
                        Response.Charset = string.Empty;
                        Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                        Response.AddHeader("Content-Disposition", "attachment; filename=mapdata.zip");
                        Response.BinaryWrite(oFile);
                        Response.OutputStream.Flush();
                        Response.OutputStream.Close();
                        Response.End();
                        m_Log.LogMessage(LogLevels.logERROR, "End StreetSectionPackage");

                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "Error in GetLastStreetSectionPackage");
                    }
                }
                else
                {
                    m_Log.LogMessage(LogLevels.logERROR, "Error in GetUserFromOpenSession");
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Error in StreetSectionPackage", e);
            }

            return new EmptyResult();

        }

        private string GetEmailFooter(ref USER oUser)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}_{1}", oUser.CURRENCy.CUR_ISO_CODE, oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}", oUser.CURRENCy.CUR_ISO_CODE));
                }
            }
            catch
            {

            }

            return strFooter;
        }

        private string GetLiteralByUserCountry(string strLiteralName, ref USER oUser)
        {
            string strLiteral = "";

            try
            {
                strLiteral = ResourceExtension.GetLiteral(string.Format("{0}_COU_{1}", strLiteralName, oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strLiteral))
                {
                    strLiteral = ResourceExtension.GetLiteral(strLiteralName);
                }

            }
            catch
            {

            }

            return strLiteral;
        }

        private bool GetUserInstallationPermit(decimal currencyId)
        {
            bool bTariffPermit = false;
            
            List<WSintegraMobile.City> installationsAndSuperInstallations = wswi.GetCities(string.Empty, currencyId);
            if (installationsAndSuperInstallations.Count > 0)
            {
                bTariffPermit = true;
            }

            return bTariffPermit;
        }

        private integraMobileWS.integraMobileWS Start(COUNTRIES_REDIRECTION countriesRedirection)
        {

            integraMobileWS.integraMobileWS oIntegraMobileWS = null;
            if (countriesRedirection == null)
            {
                oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = ConfigurationManager.AppSettings["WSUrl"];
                oIntegraMobileWS.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSTimeout"]);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WSHttpUser"]))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["WSHttpUser"], ConfigurationManager.AppSettings["WSHttpPassword"]);
                }
            }
            else
            {
                oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                int iwsurl = countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.IndexOf('/', countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.Length - 1);
                string ws_url = string.Empty;
                if (iwsurl != -1)
                {
                    ws_url = countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.Remove(countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.Length - 1, 1);
                }
                else
                {
                    ws_url = countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL;
                }
                oIntegraMobileWS.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSTimeout"]);

                if (!string.IsNullOrEmpty(countriesRedirection.COURE_COUNTRY_REDIRECTION_HTTP_USER) && !string.IsNullOrEmpty(countriesRedirection.COURE_COUNTRY_REDIRECTION_PASSWORD))
                {
                    oIntegraMobileWS.Url = ws_url;
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(countriesRedirection.COURE_COUNTRY_REDIRECTION_HTTP_USER, countriesRedirection.COURE_COUNTRY_REDIRECTION_PASSWORD);
                }
            }
            return oIntegraMobileWS;
        }


        private string CallServiceIntegraMobileWS(string methods, string sInJson, COUNTRIES_REDIRECTION countriesRedirection)
        {
            string jsonOut = string.Empty;
            try
            {
                integraMobileWS.integraMobileWS oIntegraMobileWS = Start(countriesRedirection);
                if (oIntegraMobileWS != null)
                {
                    switch (methods)
                    {
                        case Tools.METHODS_FORGET_PASSWORD_JSON:
                            jsonOut = oIntegraMobileWS.ForgetPasswordJSON(sInJson);
                            jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return jsonOut;
        }



        protected void SetCulture()
        {

            if (Session["Culture"] == null)
            {
                Session["Culture"] = new CultureInfo("en-US");
            }

            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];
            Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session["Culture"];
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(((CultureInfo)Session["Culture"]).Name);
            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];
        }
    }
}
