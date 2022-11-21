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
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using System.Reflection;
using NPOI.HSSF.UserModel;
using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;

namespace integraMobile.Controllers
{

    [HandleError]
    [NoCache]
    public class AccountController : Controller
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(AccountController));

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;


        public AccountController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
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
                    case SecurityOperationType.RecoverPassword:
                        return RedirectToAction("SecurityOperationForgotPassword", "Account", new { code = urlCode });
                    case SecurityOperationType.ResetPassword:
                        return RedirectToAction("SecurityOperationForgotPassword", "Account", new { code = urlCode });
                    case SecurityOperationType.ActivateAccount:
                        return RedirectToAction("SecurityOperationActivateAccount", "Account", new { code = urlCode });
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


        public ActionResult SecurityOperationForgotPassword()
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
        public ActionResult SecurityOperationForgotPassword(ForgotPasswordModel model)
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
                                return (RedirectToAction("SecurityOperationForgotPasswordConfirmation"));
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


        public ActionResult SecurityOperationActivateAccount()
        {
            ViewData["CodeExpired"] = false;
            ViewData["CodeAlreadyUsed"] = false;
            ViewData["ConfirmationCodeError"] = false;


            string urlCode = Request.QueryString["code"];

            USERS_SECURITY_OPERATION oSecOperation = customersRepository.GetUserSecurityOperation(urlCode);


            if (oSecOperation != null)
            {


                string strUserMustBeActivated = infraestructureRepository.GetParameterValue("UserMustBeActivated");
                bool bUserMustBeActivated = false;
                int iNumMaxMinutesForActivation = Int32.MaxValue;

                if (!string.IsNullOrEmpty(strUserMustBeActivated))
                {
                    bUserMustBeActivated = (strUserMustBeActivated == "1");
                }

                if (bUserMustBeActivated)
                {
                    string strNumMaxMinutesForActivation = infraestructureRepository.GetParameterValue("NonActivatedUserUnBlockingTime");
                    try
                    {
                        iNumMaxMinutesForActivation = Convert.ToInt32(strNumMaxMinutesForActivation);
                    }
                    catch
                    { }

                    Session["SecOperationID"] = oSecOperation.USOP_ID;
                    string culture = oSecOperation.USER.USR_CULTURE_LANG;
                    CultureInfo ci = new CultureInfo(culture);
                    Session["Culture"] = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                    ViewData["username"] = oSecOperation.USER.USR_USERNAME;
                    Session["username"] = oSecOperation.USER.USR_USERNAME;

                    if (customersRepository.IsUserSecurityOperationExpired(oSecOperation, iNumMaxMinutesForActivation * 60))
                    {
                        ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired_2"));
                        ViewData["CodeExpired"] = true;
                    }
                    else if (customersRepository.IsUserSecurityOperationAlreadyUsed(oSecOperation))
                    {
                        ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                        ViewData["CodeAlreadyUsed"] = true;
                    }
                    else
                    {

                        if (!customersRepository.ActivateUser(oSecOperation))
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                        }
                        else
                        {
                            return (RedirectToAction("SecurityOperationActivateAccountSuccess"));
                        }

                    }

                }
            }
            else
            {
                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ActivationURLIncorrect"));
                ViewData["ConfirmationCodeError"] = true;
            }


            return View();

        }



        public ActionResult SecurityOperationActivateAccountSuccess()
        {
            ViewData["CodeExpired"] = false;
            ViewData["CodeAlreadyUsed"] = false;
            ViewData["ConfirmationCodeError"] = false;


            decimal? dSecOperation = (decimal?)Session["SecOperationID"];


            if (dSecOperation != null)
            {
                Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session["Culture"];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Thread.CurrentThread.CurrentUICulture.Name);
                Session["SecOperationID"] = null;
            }
            else
            {
                return (RedirectToAction("SecurityOperationActivateAccount"));
            }


            return View();

        }

        public ActionResult ParkingTicket(decimal oper_id, string mose_session, int lang_id)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                Dictionary<string, object> oUserData = null;

                if (customersRepository.GetUserFromOpenSession(mose_session, ref oUserData))
                {
                    USER oUser = null;
                    if (customersRepository.GetUserDataByEmail(ref oUser, oUserData["email"].ToString()))
                    {
                        OPERATION oOperation = null;
                        if (customersRepository.GetOperationData(ref oUser, oper_id, out oOperation))
                        {

                            Response.Clear();
                            Response.Buffer = false;
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-disposition", "attachment; filename=parkingticket_" + oOperation.OPE_ID.ToString() + ".pdf");

                            string sHtml = GetConfirmOperationHtml(oOperation, ref oUser, lang_id);

                            //CreatePDFfromHTML(sHtml).WriteTo(Response.OutputStream);
                            Codaxy.WkHtmlToPdf.PdfConvert.ConvertHtmlToPdf(new Codaxy.WkHtmlToPdf.PdfDocument() { Html = sHtml }, new Codaxy.WkHtmlToPdf.PdfOutput() { OutputStream = Response.OutputStream });
                            //CreatePDFfromHTML2(sHtml).WriteTo(Response.OutputStream);

                            
                            Response.Flush();
                            Response.Close();
                            Response.End();

                        }
                        else
                            sErrorInfo = "Invalid operation id";
                    }
                    else
                        sErrorInfo = "Invalid user";
                }
                else
                {
                    sErrorInfo = "Invalid session ID";
                }
            }
            catch (Exception ex)
            {                
                sErrorInfo = "Error generating pdf";
            }

            return View("Ticket",(object) sErrorInfo);
        }

        public ActionResult FinePaymentTicket(decimal tck_id, string mose_session, int lang_id)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                Dictionary<string, object> oUserData = null;

                if (customersRepository.GetUserFromOpenSession(mose_session, ref oUserData))
                {
                    USER oUser = null;
                    if (customersRepository.GetUserDataByEmail(ref oUser, oUserData["email"].ToString()))
                    {
                        TICKET_PAYMENT oTicketPayment = null;
                        if (customersRepository.GetTicketPaymentData(ref oUser, tck_id, out oTicketPayment))
                        {

                            Response.Clear();
                            Response.Buffer = false;
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-disposition", "attachment; filename=parkingticket_" + oTicketPayment.TIPA_ID.ToString() + ".pdf");

                            string sHtml = GetConfirmFinePaymentHtml(oTicketPayment, ref oUser, lang_id);
                            
                            //CreatePDFfromHTML(sHtml).WriteTo(Response.OutputStream);
                            Codaxy.WkHtmlToPdf.PdfConvert.ConvertHtmlToPdf(new Codaxy.WkHtmlToPdf.PdfDocument() { Html = sHtml }, new Codaxy.WkHtmlToPdf.PdfOutput() { OutputStream = Response.OutputStream });

                            Response.Flush();
                            Response.Close();
                            Response.End();

                        }
                        else
                            sErrorInfo = "Invalid fine payment id";
                    }
                    else
                        sErrorInfo = "Invalid user";
                }
                else
                {
                    sErrorInfo = "Invalid session ID";
                }
            }
            catch (Exception ex)
            {
                sErrorInfo = "Error generating pdf";
            }

            return View("Ticket", (object)sErrorInfo);
        }

        private bool SetDomainUserSecurityOperationForgotPassword(ref USER oUser,
                                                                  out USERS_SECURITY_OPERATION oSecOperation)
        {
            bool bRes = false;
            oSecOperation = null;
            
            try
            {
                
                oSecOperation = new USERS_SECURITY_OPERATION
                    {
                        USOP_NEW_EMAIL = oUser.USR_EMAIL,
                        USOP_NEW_MAIN_TEL = oUser.USR_MAIN_TEL,
                        USOP_NEW_MAIN_TEL_COUNTRY = oUser.USR_MAIN_TEL_COUNTRY,
                        USOP_ACTIVATION_RETRIES = 0,
                        USOP_OP_TYPE = (int)SecurityOperationType.RecoverPassword,
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



        public USER GetUserFromSession()
        {
            USER oUser = null;
            try
            {
                if (Session["USER_ID"]!=null)
                {
                    decimal dUserId = (decimal)Session["USER_ID"];
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

        private string GetConfirmOperationHtml(OPERATION oParkOp, ref USER oUser, int iLangId)
        {
            string culture = "";

            LANGUAGE oLanguage = null;
            if (infraestructureRepository.GetLanguage(iLangId, out oLanguage))            
                culture = oLanguage.LAN_CULTURE;
            
            if (string.IsNullOrEmpty(culture))
                culture =  oUser.USR_CULTURE_LANG;

            CultureInfo ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            int iQuantity = oParkOp.OPE_AMOUNT;
            decimal dPercVAT1 = oParkOp.OPE_PERC_VAT1 ?? 0;
            decimal dPercVAT2 = oParkOp.OPE_PERC_VAT2 ?? 0;
            decimal dPercFEE = oParkOp.OPE_PERC_FEE ?? 0;
            decimal dPercBonus = oParkOp.OPE_PERC_BONUS ?? 0;
            int iPercFEETopped = (int)(oParkOp.OPE_PERC_FEE_TOPPED ?? 0);
            int iFixedFEE = (int)(oParkOp.OPE_FIXED_FEE ?? 0);

            int iPartialVAT1;
            int iPartialPercFEE;
            int iPartialFixedFEE;
            int iPartialPercFEEVAT;
            int iPartialFixedFEEVAT;
            int iPartialBonusFEE;
            int iPartialBonusFEEVAT;

            //iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);
            int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);


            int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
            if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
            iQFEE += iFixedFEE;
            int iQBonus = iPartialBonusFEE - iPartialBonusFEEVAT;
            int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
            int iQSubTotal = iQuantity + iQFEE;

            int iLayout = 0;
            if (dPercBonus == 0)
            {
                if (iQFEE != 0 || iQVAT != 0)
                {
                    iLayout = oParkOp.INSTALLATION.INS_FEE_LAYOUT;
                }
            }
            else
                iLayout = 3;


            string sLayoutSubtotal = "";
            string sLayoutTotal = "";

            string strSpaceSection = "";

            if (!string.IsNullOrEmpty(oParkOp.OPE_SPACE_STRING))
            {
                strSpaceSection = string.Format(ResourceExtension.GetLiteral("ConfirmParking_EmailBody_SpaceSection"),
                                                oParkOp.OPE_SPACE_STRING);
            }

            string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oParkOp.OPE_AMOUNT_CUR_ID));

            if (iLayout == 2)
            {
                sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQSubTotal) / 100, sCurIsoCode),
                                                (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQVAT) / 100, sCurIsoCode));
            }
            else if (iLayout == 1)
            {
                sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQuantity) / 100, sCurIsoCode),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQFEE) / 100, sCurIsoCode),
                                                (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQVAT) / 100, sCurIsoCode));
            }


            //string strParkingEmailSubject = ResourceExtension.GetLiteral("ConfirmParking_EmailHeader");
            /*
                * ID: {0}<br>
                * Matr&iacute;cula: {1}<br>
                * Ciudad: {2}<br>
                * Zona: {3}<br>
                * Tarifa: {4}<br>
                * Fecha de emisi&ocuate;: {5:HH:mm:ss dd/MM/yyyy}<br>
                * Aparcamiento Comienza:  {6:HH:mm:ss dd/MM/yyyy}<br><b>
                * Aparcamiento Finaliza:  {7:HH:mm:ss dd/MM/yyyy}</b><br>
                * Cantidad Pagada: {8} 
                */
            INSTALLATION oInstallation = oParkOp.INSTALLATION;
            string strParkingEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmParking_EmailBody"),
                oParkOp.OPE_ID,
                oParkOp.USER_PLATE.USRP_PLATE,
                oParkOp.INSTALLATION.INS_DESCRIPTION,
                oParkOp.GROUP.GRP_DESCRIPTION,
                oParkOp.TARIFF.TAR_DESCRIPTION,
                oParkOp.OPE_DATE,
                oParkOp.OPE_INIDATE,
                oParkOp.OPE_ENDDATE,
                (oParkOp.OPE_AMOUNT_CUR_ID == oParkOp.OPE_BALANCE_CUR_ID ?
                string.Format("{0:0.00} {1}", Convert.ToDouble(oParkOp.OPE_TOTAL_AMOUNT) / 100, oParkOp.CURRENCy.CUR_ISO_CODE) :
                string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oParkOp.OPE_TOTAL_AMOUNT) / 100, oParkOp.CURRENCy.CUR_ISO_CODE,
                                                                Convert.ToDouble(oParkOp.OPE_FINAL_AMOUNT) / 100, oParkOp.CURRENCy1.CUR_ISO_CODE)),
                (oParkOp.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0) ?
                        string.Format(ResourceExtension.GetLiteral("Confirm_EmailBody_Balance"), string.Format("{0:0.00} {1}",
                                    Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                    infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                ConfigurationManager.AppSettings["EmailSignatureURL"],
                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                sLayoutSubtotal,
                sLayoutTotal,
                strSpaceSection,
                GetEmailFooter(ref oInstallation));
            
            return strParkingEmailBody;
        }

        private string GetConfirmFinePaymentHtml(TICKET_PAYMENT oTicketPayment, ref USER oUser, int iLangId)
        {
            string culture = "";

            LANGUAGE oLanguage = null;
            if (infraestructureRepository.GetLanguage(iLangId, out oLanguage))
                culture = oLanguage.LAN_CULTURE;

            if (string.IsNullOrEmpty(culture))
                culture = oUser.USR_CULTURE_LANG;

            CultureInfo ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            int iQuantity = oTicketPayment.TIPA_AMOUNT;
            decimal dPercVAT1 = oTicketPayment.TIPA_PERC_VAT1 ?? 0;
            decimal dPercVAT2 = oTicketPayment.TIPA_PERC_VAT2 ?? 0;
            decimal dPercFEE = oTicketPayment.TIPA_PERC_FEE ?? 0;
            int iPercFEETopped = (int)(oTicketPayment.TIPA_PERC_FEE_TOPPED ?? 0);
            int iFixedFEE = (int)(oTicketPayment.TIPA_FIXED_FEE ?? 0);

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
                iLayout = oTicketPayment.INSTALLATION.INS_FEE_LAYOUT;
            }

            string strEnforcUser = "";
            string strZone = "";
            string strSector = "";

            /*if (parametersIn.ContainsKey("enforcuser"))
                strEnforcUser = parametersIn["enforcuser"].ToString();
            if (parametersIn.ContainsKey("zone"))
                strZone = parametersIn["zone"].ToString();
            if (parametersIn.ContainsKey("sector"))
                strSector = parametersIn["sector"].ToString();*/

            string sLayoutSubtotal = "";
            string sLayoutTotal = "";

            if (iLayout == 2)
            {
                sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQSubTotal) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE),
                                                (oTicketPayment.TIPA_PERC_VAT1 != 0 ? string.Format("{0:0.00}% ", oTicketPayment.TIPA_PERC_VAT1 * 100) : "") +
                                                (oTicketPayment.TIPA_PERC_VAT2 != 0 && oTicketPayment.TIPA_PERC_VAT1 != oTicketPayment.TIPA_PERC_VAT2 ? string.Format("{0:0.00}%", oTicketPayment.TIPA_PERC_VAT2 * 100) : ""),
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(iQVAT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE));
            }
            else if (iLayout == 1)
            {
                sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                             string.Format("{0:0.00} {1}", Convert.ToDouble(iQuantity) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE),
                                             string.Format("{0:0.00} {1}", Convert.ToDouble(iQFEE) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE),
                                             (oTicketPayment.TIPA_PERC_VAT1 != 0 ? string.Format("{0:0.00}% ", oTicketPayment.TIPA_PERC_VAT1 * 100) : "") +
                                             (oTicketPayment.TIPA_PERC_VAT2 != 0 && oTicketPayment.TIPA_PERC_VAT1 != oTicketPayment.TIPA_PERC_VAT2 ? string.Format("{0:0.00}%", oTicketPayment.TIPA_PERC_VAT2 * 100) : ""),
                                             string.Format("{0:0.00} {1}", Convert.ToDouble(iQVAT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE));
            }

            //string strTicketPaymentEmailSubject = ResourceExtension.GetLiteral("ConfirmTicketPayment_EmailHeader");
            /*
             ID: {0}<br>
             Expediente: {1}<br>
             Matr&iacute;cula: {2}<br>
             Ciudad: {3}<br>
             Fecha de anulaci&oacute;n: {4:HH:mm:ss dd/MM/yyyy}<br>
             Artículo: {5}<br>
             Cantidad Pagada: {6} <br><br>
             */
            INSTALLATION oInst = oTicketPayment.INSTALLATION;
            string strTicketPaymentEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmTicketPayment_EmailBody"),
                oTicketPayment.TIPA_ID,
                oTicketPayment.TIPA_TICKET_NUMBER,
                oTicketPayment.TIPA_PLATE_STRING,
                oTicketPayment.INSTALLATION.INS_DESCRIPTION,
                oTicketPayment.TIPA_DATE,
                oTicketPayment.TIPA_TICKET_DATA,
                (oTicketPayment.TIPA_AMOUNT_CUR_ID == oTicketPayment.TIPA_BALANCE_CUR_ID ?
                string.Format("{0:0.00} {1}", Convert.ToDouble(oTicketPayment.TIPA_TOTAL_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE) :
                string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oTicketPayment.TIPA_TOTAL_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE,
                                                             Convert.ToDouble(oTicketPayment.TIPA_FINAL_AMOUNT) / 100, oTicketPayment.CURRENCy1.CUR_ISO_CODE)),
                (oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0) ?
                        string.Format(ResourceExtension.GetLiteral("Confirm_EmailBody_Balance"), string.Format("{0:0.00} {1}",
                                    Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                    infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                ConfigurationManager.AppSettings["EmailSignatureURL"],
                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                sLayoutSubtotal,
                sLayoutTotal,
                GetEmailFooter(ref oInst), strZone, strSector, strEnforcUser);

            return strTicketPaymentEmailBody;
        }

        private string GetEmailFooter(ref INSTALLATION oInstallation)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_INS_{0}", oInstallation.INS_SHORTDESC));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_COU_{0}", oInstallation.COUNTRy.COU_CODE));
                }

            }
            catch
            {

            }

            return strFooter;
        }

        private MemoryStream CreatePDFfromHTML(string html)
        {
            var oMsOutput = new MemoryStream();

            var bytes = System.Text.Encoding.UTF8.GetBytes(html);

            using (var input = new MemoryStream(bytes))
            {                
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, oMsOutput);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();                
                xmlWorker.ParseXHtml(writer, document, input, null);
                document.Close();
                oMsOutput.Position = 0;
            }

            return oMsOutput;
        }

        /*private MemoryStream CreatePDFfromHTML2(string html)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerateConfig config = new TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerateConfig() { PageSize = PdfSharp.PageSize.A4 };
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, config);
                pdf.Save(ms);
                return ms;
            }
        }*/
    }
}
