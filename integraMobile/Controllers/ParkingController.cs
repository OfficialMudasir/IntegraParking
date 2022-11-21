using System;
using System.Collections;
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
using System.Xml;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using System.Text.RegularExpressions;
using integraMobile.ExternalWS;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Reflection;
using integraMobile.Helper;
using integraMobile.Domain.Concrete;
using System.Resources;
using integraMobile.Properties;
using Newtonsoft.Json;
using System.Web.Services;
using System.Net.Http;

namespace integraMobile.Controllers
{
    public class ParkingController : Controller
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(ParkingController));
        private IInfraestructureRepository infraestructureRepository;
        private IBackOfficeRepository backOfficeRepository;
        private ICustomersRepository customersRepository;
        private SQLGeograficAndTariffsRepository geographicAndTariffsRepository;
        private string cs = ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ToString();
        ResourceManager resBundle = Resources.ResourceManager;
        private integraMobile.Services.ExternalService external = new Services.ExternalService();

        ParkingModel model = null;
        private integraMobile.ExternalWS.WSintegraMobile wswi = null;

        protected const long BIG_PRIME_NUMBER = 472189635;

        #region Constructor

        public ParkingController(IInfraestructureRepository _infraestructureRepository, IBackOfficeRepository _backofficeRepository, ICustomersRepository _customersRepository)
        {
            this.infraestructureRepository = _infraestructureRepository;
            this.backOfficeRepository = _backofficeRepository;
            this.customersRepository = _customersRepository;
            geographicAndTariffsRepository = new SQLGeograficAndTariffsRepository(cs);

            wswi = new integraMobile.ExternalWS.WSintegraMobile(backOfficeRepository, customersRepository, infraestructureRepository, geographicAndTariffsRepository);
        }
        
        #endregion

        public ActionResult PaymentDirect (decimal? g = null, decimal? z = null, string theme = null)
        {
            decimal? parameterIn = null;
            if (g != null)
            {
                parameterIn = g;
            }
            else if (z != null)
            {
                parameterIn = z;
            }

            if (parameterIn != null)
            {
                model = new ParkingModel();
                GROUP grp = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>()).Where(t => t.GRP_ID == Convert.ToDecimal(parameterIn)).FirstOrDefault();
                if (grp != null)
                {
                    model.Group = Convert.ToInt32(parameterIn);
                    model.Installation = grp.GRP_INS_ID;
                    model.GroupText = grp.GRP_DESCRIPTION;
                    if (grp.INSTALLATION != null)
                    {
                        model.InstallationText = grp.INSTALLATION.INS_DESCRIPTION;
                    }
                    model.Culture = GetCultureFromBrowser();
                    model.LanguageCode = GetLangFromCulture();
                }
            }
            else
            {
                LoadModel();
            }

            if (model.Group > 0)
            {
                dynamic parametersOut = null;
                ResultType res = wswi.QueryAvailableTariffsGuestUser(model.Group.ToString(), model.Culture, out parametersOut);

                if (res != ResultType.Result_OK)
                {
                    string url = string.Empty;
                    try
                    {
                        url = HttpContext.Request.Url.AbsoluteUri;
                    }
                    catch (Exception ex)
                    {
                        url = "Unable to get (" + ex.Message + ")";
                    }
                    m_Log.LogMessage(LogLevels.logWARN, "Plates::AvailableTariff not OK - Res = " + res.ToString() + " - URL = " + url);

                    if (res == ResultType.Result_Error_Tariffs_Not_Available)
                    {
                        if (parametersOut.ipark_out.custom_message != null)
                        {
                            string err = parametersOut.ipark_out.custom_message.ToString();
                            model.Error = "<p><strong>" + err.Replace("|", "</strong></p><p><strong>") + "</strong></p>";
                        }
                        else
                        {
                            model.Error = "<p><strong>" + resBundle.GetString(string.Format("Permits_Error_{0}", res)) + "</strong></p>";
                        }
                        model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                        SaveModel();
                        return View("Advice", model);
                    }
                    else
                    {
                        model.Error = "<p><strong>" + resBundle.GetString(string.Format("Permits_Error_{0}", res)) + "</strong></p>";
                        model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                        SaveModel();
                        return View("Advice", model);
                    }
                }
                else
                {
                    model.Error = string.Empty;
                    model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                    model.PreSelectionDone = true;
                    SaveModel();
                    return RedirectToAction("Plates"); // with no parameters (group and theme are already in the model)
                }
            }
            else
            {
                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }
                m_Log.LogMessage(LogLevels.logWARN, "Plates::Invalid group - Redirect to error page - URL = " + url);

                model.Error = "<p><strong>" + Resources.Parking_UnknownLocation + "</strong></p>";
                model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                SaveModel();
                return View("Advice", model);
            }
        }

        public ActionResult Payment (decimal? g = null, decimal? z = null, string theme = null)
        {
            decimal? parameterIn = null;
            if (g != null) {
                parameterIn = g;
            }
            else if (z != null) {
                parameterIn = z;
            }

            if (parameterIn != null)
            {
                model = new ParkingModel();
                GROUP grp = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>()).Where(t => t.GRP_ID == Convert.ToDecimal(parameterIn)).FirstOrDefault();
                if (grp != null)
                {
                    model.Group = Convert.ToInt32(parameterIn);
                    model.Installation = grp.GRP_INS_ID;
                    model.GroupText = grp.GRP_DESCRIPTION;
                    if (grp.INSTALLATION != null)
                    {
                        model.InstallationText = grp.INSTALLATION.INS_DESCRIPTION;
                    }
                    model.Culture = GetCultureFromBrowser();
                    model.LanguageCode = GetLangFromCulture();
                }
            }
            else
            {
                LoadModel();
            }

            if (model.Group > 0)
            {
                dynamic parametersOut = null;
                ResultType res = wswi.QueryAvailableTariffsGuestUser(model.Group.ToString(), model.Culture, out parametersOut);

                if (res != ResultType.Result_OK)
                {

                    string url = string.Empty;
                    try
                    {
                        url = HttpContext.Request.Url.AbsoluteUri;
                    }
                    catch (Exception ex)
                    {
                        url = "Unable to get (" + ex.Message + ")";
                    }
                    m_Log.LogMessage(LogLevels.logWARN, "Plates::AvailableTariff not OK - Res = " + res.ToString() + " - URL = " + url);

                    if (res == ResultType.Result_Error_Tariffs_Not_Available)
                    {
                        if (parametersOut.ipark_out.custom_message != null)
                        {
                            string err = parametersOut.ipark_out.custom_message.ToString();
                            model.Error = "<p><strong>" + err.Replace("|", "</strong></p><p><strong>") + "</strong></p>";
                        }
                        else
                        {
                            model.Error = "<p><strong>" + resBundle.GetString(string.Format("Permits_Error_{0}", res)) + "</strong></p>";
                        }
                        model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                        SaveModel();
                        return View("Advice", model);
                    }
                    else
                    {
                        model.Error = "<p><strong>" + resBundle.GetString(string.Format("Permits_Error_{0}", res)) + "</strong></p>";
                        model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                        SaveModel();
                        return View("Advice", model);
                    }
                }
                else
                {
                    model.Error = string.Empty;
                    model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                    model.PreSelectionDone = true;
                    SaveModel();
                    return View(model);
                }
            }
            else
            {
                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }
                m_Log.LogMessage(LogLevels.logWARN, "Plates::Invalid group - Redirect to error page - URL = " + url);

                model.Error = "<p><strong>" + Resources.Parking_UnknownLocation + "</strong></p>";
                model.Theme = !string.IsNullOrEmpty(theme) ? theme : string.Empty;
                SaveModel();
                return View("Advice", model);
            }
        }

        #region Public Method

        public ActionResult Plates(decimal? g = null, string theme = "")
        {
            if (g != null)
            {
                model = new ParkingModel();                
                GROUP grp = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>()).Where(t => t.GRP_ID == Convert.ToDecimal(g)).FirstOrDefault();
                if (grp != null)
                {
                    model.Group = Convert.ToInt32(g);
                    model.Installation = grp.GRP_INS_ID;
                    model.GroupText = grp.GRP_DESCRIPTION;
                    if (grp.INSTALLATION != null)
                    {
                        model.InstallationText = grp.INSTALLATION.INS_DESCRIPTION;
                    }
                    model.Culture = GetCultureFromBrowser();
                    model.LanguageCode = GetLangFromCulture();
                }
            }
            else {
                LoadModel();
            }

            if (model.Group == 0)
            {
                model.Group = 43501;
                model.Theme = "banff";

                GROUP grp = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>()).Where(t => t.GRP_ID == Convert.ToDecimal(model.Group)).FirstOrDefault();
                if (grp != null)
                {
                    model.Installation = grp.GRP_INS_ID;
                    model.GroupText = grp.GRP_DESCRIPTION;
                    if (grp.INSTALLATION != null)
                    {
                        model.InstallationText = grp.INSTALLATION.INS_DESCRIPTION;
                    }
                    model.Culture = GetCultureFromBrowser();
                    model.LanguageCode = GetLangFromCulture();

                    string url = string.Empty;
                    try
                    {
                        url = HttpContext.Request.Url.AbsoluteUri;
                    }
                    catch (Exception ex)
                    {
                        url = "Unable to get (" + ex.Message + ")";
                    }
                    m_Log.LogMessage(LogLevels.logWARN, string.Format("Plates::Group (43501) restored - URL = {0}", url));
                }
            }

            if (model.Group == 0)
            {
                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }
                m_Log.LogMessage(LogLevels.logWARN, "Plates::Invalid group - Redirect to error page - URL = " + url);

                model.Error = "<p><strong>" + Resources.Parking_UnknownLocation + "</strong></p>";
                SaveModel();
                return View("Advice", model);
            }

            model.Error = string.Empty;

            if (!string.IsNullOrEmpty(theme)) 
            {
                model.Theme = theme;
            }

            dynamic parametersOut = null;
            ResultType res = wswi.QueryAvailableTariffsGuestUser(model.Group.ToString(), model.Culture, out parametersOut);

            if (res != ResultType.Result_OK)
            {

                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }
                m_Log.LogMessage(LogLevels.logWARN, "Plates::AvailableTariff not OK - Res = " + res.ToString() + " - URL = " + url);

                if (res == ResultType.Result_Error_Tariffs_Not_Available)
                {
                    if (parametersOut.ipark_out.custom_message != null)
                    {
                        string err = parametersOut.ipark_out.custom_message.ToString();
                        model.Error = "<p><strong>" + err.Replace("|", "</strong></p><p><strong>") + "</strong></p>";
                    }
                    else
                    {
                        model.Error = "<p><strong>" + resBundle.GetString(string.Format("Permits_Error_{0}", res)) + "</strong></p>";
                    }
                    SaveModel();
                    return View("Advice", model);
                }
                else 
                {
                    model.Error = "<p><strong>" + resBundle.GetString(string.Format("Permits_Error_{0}", res)) + "</strong></p>";
                    SaveModel();
                    return View("Advice", model);
                }
            }
            else
            {
                if (model.Theme == "banff")
                {
                    model.Error = string.Empty;
                    SaveModel();
                    return View(model);
                }
                else if (model.PreSelectionDone)
                {
                    model.Error = string.Empty;
                    SaveModel();
                    return View(model);
                }
                else
                {
                    model.Error = string.Empty;
                    SaveModel();
                    return RedirectToAction("Payment", new { g = model.Group });
                }
            }
        }

        public ActionResult PaymentSelector()
        {
            LoadModel();
            return View(model);
        }

        public ActionResult AddPlate(string g = "")
        {
            LoadModel();
            if (!string.IsNullOrEmpty(g))
            {
                model.Guid = g;
            }
            SaveModel();
            return View(model);
        }

        public ActionResult EditPlate(int i)
        {
            LoadModel();
            model.PlateIndex = i;
            SaveModel();

            return View(model);
        }

        public ActionResult SelectRate(string p, string g)
        {
            LoadModel();

            if (model.Group == null || model.Group == 0 || string.IsNullOrEmpty(p))
            {
                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }
                m_Log.LogMessage(LogLevels.logWARN, string.Format("SelectRate::Invalid parameters [Group = {0} | p = {1}] - Redirect to home - URL = {2}", model.Group, p, url));

                return RedirectToAction("Plates");
            }

            model.Plate = Regex.Replace(p, "[^0-9A-Za-z]", "", RegexOptions.None).ToUpper();

            if (!string.IsNullOrEmpty(g))
            {
                model.Guid = g;
            }

            model.Tariffs.Clear();

            dynamic parametersOut = null;
            ResultType res = wswi.QueryParkingTariffsGuestUser(model.Group.ToString(), model.Plate, model.Culture, out parametersOut, model.SessionID, model.Guid);

            if (res == ResultType.Result_OK) {

                model.SessionID = parametersOut.ipark_out.SessionID;
                model.Guid = parametersOut.ipark_out.guid;
                model.User = parametersOut.ipark_out.u;
                model.Email = parametersOut.ipark_out.email;

                foreach (dynamic tariff in parametersOut.ipark_out.ltar.ad)
                {
                    ParkingTariff pt = new ParkingTariff();
                    pt.Id = Convert.ToDecimal(tariff.id);
                    pt.Description = tariff.desc.ToString();
                    model.Tariffs.Add(pt);
                }
            }
            else {
                model.Error = resBundle.GetString(string.Format("Permits_Error_{0}", res));
            }

            if (model.Tariffs.Count == 1)
            {
                model.Rate = model.Tariffs[0].Id;
                model.RateText = model.Tariffs[0].Description;
                SaveModel();

                return RedirectToAction("SelectAmount", new { r = model.Rate });
            }

            SaveModel();

            return View(model);
        }

        public ActionResult SelectAmount(decimal? r)
        {
            LoadModel();

            if (string.IsNullOrEmpty(model.SessionID))
            {
                return RedirectToAction("Plates");
            }

            if (r != null)
            {
                model.Rate = 0;
                model.RateText = string.Empty;

                TARIFF tar = backOfficeRepository.GetTariffs(PredicateBuilder.True<TARIFF>()).Where(t => t.TAR_ID == Convert.ToDecimal(r)).FirstOrDefault();
                if (tar != null)
                {
                    model.Rate = Convert.ToDecimal(r);
                    model.RateText = tar.TAR_DESCRIPTION;
                }
            }

            if (model.Rate == 0) {

                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }                

                if (!string.IsNullOrEmpty(model.Plate) && !string.IsNullOrEmpty(model.Guid))
                {
                    m_Log.LogMessage(LogLevels.logWARN, "SelectAmount::Invalid rate - Redirect to rate selector - URL = " + url);
                    return RedirectToAction("SelectRate", new { p = model.Plate, g = model.Guid });
                }
                else
                {
                    m_Log.LogMessage(LogLevels.logWARN, "SelectAmount::Invalid rate / plate / guid - Redirect to home - URL = " + url);
                    return RedirectToAction("Plates");
                }
            }

            model.Buttons.Clear();
            model.Steps.Clear();

            dynamic parametersOut = null;
            ResultType res = wswi.QueryParkingOperationWithTimeStepsGuestUser(model.Group, model.Plate, model.Rate, model.SessionID, model.User, (int)model.LanguageCode, out parametersOut);

            if (res == ResultType.Result_OK)
            {
                model.Rate = parametersOut.ipark_out.ad;
                model.Layout = parametersOut.ipark_out.layout;

                TARIFF tar = backOfficeRepository.GetTariffs(PredicateBuilder.True<TARIFF>()).Where(t => t.TAR_ID == Convert.ToDecimal(model.Rate)).FirstOrDefault();
                if (tar != null)
                {
                    model.RateText = tar.TAR_DESCRIPTION;
                }

                CultureInfo provider = CultureInfo.InvariantCulture;
                model.InitialDate = FormatDate(DateTime.ParseExact(parametersOut.ipark_out.di.ToString(), "HHmmssddMMyy", provider));
                model.InitialDate_Clean = parametersOut.ipark_out.di.ToString();
                model.LabelParking = parametersOut.ipark_out.ServiceParkingLbl;
                model.LabelFee = parametersOut.ipark_out.ServiceFeeLbl;
                model.LabelFeeVat = parametersOut.ipark_out.ServiceFeeVATLbl;
                model.LabelTotal = parametersOut.ipark_out.ServiceTotalLbl;
                model.CurrencyCode = parametersOut.ipark_out.cur;

                model.FreeTimeTariff = false;
                if (parametersOut.ipark_out.free_time_tariff != null && parametersOut.ipark_out.free_time_tariff.ToString() == "1") {
                    model.FreeTimeTariff = true;
                }

                model.PaymentParams = new ParkingPaymentParams();
                model.PaymentParams.Culture = model.Culture;
                model.PaymentParams.CurrencyISOCODE = model.CurrencyCode;
                model.PaymentParams.Description = "Anonymous parking payment";
                model.PaymentParams.Email = model.Email;
                model.PaymentParams.UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
                model.PaymentParams.ReturnURL = ConfigurationManager.AppSettings["ParkingPaymentReturnURL"].ToString();

                model.CreditCardEnabled = false;
                model.PayPalEnabled = false;

                if (parametersOut.ipark_out.CreditCardAnonymousEnabled != null && parametersOut.ipark_out.CreditCardAnonymousEnabled == "1")
                {
                    model.CreditCardEnabled = true;

                    PaymentMeanCreditCardProviderType CCProvider = PaymentMeanCreditCardProviderType.pmccpUndefined;
                    if (parametersOut.ipark_out.ccprovider != null)
                    {
                        CCProvider = (PaymentMeanCreditCardProviderType)Convert.ToInt32(parametersOut.ipark_out.ccprovider);
                        model.PaymentProvider = CCProvider;
                    }

                    ParkingPaymentProviderData pd = new ParkingPaymentProviderData();
                    switch (CCProvider)
                    {
                        case PaymentMeanCreditCardProviderType.pmccpMoneris:

                            pd.RequestURL = parametersOut.ipark_out.moneris_token_url;
                            pd.Guid = parametersOut.ipark_out.moneris_guid;
                            pd.HashSeed = parametersOut.ipark_out.moneris_hash_seed_key;
                            model.PaymentParams.CreditCard = pd;
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPayu:
                            pd.RequestURL = parametersOut.ipark_out.payu_token_url;
                            pd.Guid = parametersOut.ipark_out.payu_guid;
                            pd.HashSeed = parametersOut.ipark_out.payu_hash_seed_key;
                            model.PaymentParams.CreditCard = pd;
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpBSRedsys:
                            pd.RequestURL = parametersOut.ipark_out.bsredsys_token_url;
                            pd.Guid = parametersOut.ipark_out.bsredsys_guid;
                            pd.HashSeed = parametersOut.ipark_out.bsredsys_hash_seed_key;
                            model.PaymentParams.CreditCard = pd;
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpStripe:
                            pd.RequestURL = parametersOut.ipark_out.stripe_token_url;
                            pd.Guid = parametersOut.ipark_out.stripe_guid;
                            pd.HashSeed = parametersOut.ipark_out.stripe_hash_seed_key;
                            model.PaymentParams.CreditCard = pd;
                            break;
                        default:
                            model.CreditCardEnabled = false;
                            break;
                    }
                }

                if (parametersOut.ipark_out.PaypalAnonymousEnabled != null && parametersOut.ipark_out.PaypalAnonymousEnabled == "1")
                {
                    model.PayPalEnabled = true;

                    ParkingPaymentProviderData pd = new ParkingPaymentProviderData();

                    if (parametersOut.ipark_out.Paypal_url != null &&
                        parametersOut.ipark_out.Paypal_guid != null &&
                        parametersOut.ipark_out.Paypal_hash_seed_key != null)
                    {
                        pd.RequestURL = parametersOut.ipark_out.Paypal_url;
                        pd.Guid = parametersOut.ipark_out.Paypal_guid;
                        pd.HashSeed = parametersOut.ipark_out.Paypal_hash_seed_key;
                        model.PaymentParams.PayPal = pd;
                    }
                    else 
                    {
                        model.PayPalEnabled = false;
                    }
                }

                NumberFormatInfo prvdr = new NumberFormatInfo();
                prvdr.NumberDecimalSeparator = ".";

                if (parametersOut.ipark_out.buttons != null)
                {
                    foreach (dynamic button in parametersOut.ipark_out.buttons.button)
                    {
                        ParkingButton pb = new ParkingButton();
                        pb.Type = (ParkingButtonType)Convert.ToInt32(button.btntype);
                        pb.Text = button.lit.ToString();
                        pb.Minutes = Convert.ToInt32(button.min);
                        switch (pb.Type)
                        {
                            case ParkingButtonType.Increment:
                                pb.Function = "Increment(" + pb.Minutes + ");";
                                break;
                            case ParkingButtonType.RateStep:
                                pb.Function = "RateStep(" + pb.Minutes + ");";
                                break;
                            case ParkingButtonType.RateMaximum:
                                pb.Function = "RateMaximum();";
                                break;
                        }
                        model.Buttons.Add(pb);
                    }
                }
                foreach (dynamic step in parametersOut.ipark_out.steps.step)
                {
                    ParkingStep ps = new ParkingStep();
                    ps.Time = Convert.ToInt32(step.t);

                    int hours = Convert.ToInt32(Math.Truncate((double)(ps.Time / 60)));
                    if (hours >= 24) {
                        int days = Convert.ToInt32(Math.Truncate((double)(hours / 24)));
                        if (days == 1)
                        {
                            ps.TimeFormatted = string.Format("{0} {1}", days, Resources.Parking_Day);
                        }
                        else
                        {
                            ps.TimeFormatted = string.Format("{0} {1}", days, Resources.Parking_Days);
                        }                        
                        hours = hours - (days * 24);                        
                    }
                    if (hours == 1)
                    {
                        if (!string.IsNullOrEmpty(ps.TimeFormatted))
                        {
                            ps.TimeFormatted += string.Format(", {0} {1}", hours, Resources.Parking_Hour);
                        }
                        else
                        {
                            ps.TimeFormatted += string.Format("{0} {1}", hours, Resources.Parking_Hour);
                        }
                    }
                    else if (hours > 1)
                    {
                        if (!string.IsNullOrEmpty(ps.TimeFormatted))
                        {
                            ps.TimeFormatted += string.Format(", {0} {1}", hours, Resources.Parking_Hours);
                        }
                        else
                        {
                            ps.TimeFormatted += string.Format("{0} {1}", hours, Resources.Parking_Hours);
                        }
                    }
                    int minutes = ps.Time - Convert.ToInt32(Math.Truncate((double)(ps.Time / 60))) * 60;
                    if (minutes == 1)
                    {
                        if (!string.IsNullOrEmpty(ps.TimeFormatted))
                        {
                            ps.TimeFormatted += string.Format(", {0} {1}", minutes, Resources.Parking_Minute);
                        }
                        else
                        {
                            ps.TimeFormatted += string.Format("{0} {1}", minutes, Resources.Parking_Minute);
                        }
                    }
                    else if (minutes > 1)
                    {
                        if (!string.IsNullOrEmpty(ps.TimeFormatted))
                        {
                            ps.TimeFormatted += string.Format(", {0} {1}", minutes, Resources.Parking_Minutes);
                        }
                        else
                        {
                            ps.TimeFormatted += string.Format("{0} {1}", minutes, Resources.Parking_Minutes);
                        }
                    }

                    ps.TimeBalanceUsed = Convert.ToInt32(step.time_bal_used);
                    ps.Quantity = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q) / 100.0, model.CurrencyCode);
                    ps.QuantityVat = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q_vat) / 100.0, model.CurrencyCode);
                    ps.QuantityPlusVat = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q_plus_vat) / 100.0, model.CurrencyCode);
                    ps.QuantityTotal = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q_total) / 100.0, model.CurrencyCode);
                    ps.QuantityFee = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q_fee) / 100.0, model.CurrencyCode);
                    ps.QuantityFeeVat = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q_fee_plus_vat) / 100.0, model.CurrencyCode);
                    ps.QuantityReal = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.real_q) / 100.0, model.CurrencyCode);
                    ps.QuantityWithoutBon = string.Format(prvdr, "{0:0.00} {1}", Convert.ToDouble(step.q_without_bon) / 100.0, model.CurrencyCode);

                    ps.Quantity_Clean = Convert.ToDecimal(step.q);
                    ps.QuantityVat_Clean = Convert.ToDecimal(step.q_vat);
                    ps.QuantityPlusVat_Clean = Convert.ToDecimal(step.q_plus_vat);
                    ps.QuantityTotal_Clean = Convert.ToDecimal(step.q_total);
                    ps.QuantityFee_Clean = Convert.ToDecimal(step.q_fee);
                    ps.QuantityFeeVat_Clean = Convert.ToDecimal(step.q_fee_plus_vat);
                    ps.QuantityReal_Clean = Convert.ToDecimal(step.real_q);
                    ps.QuantityWithoutBon_Clean = Convert.ToDecimal(step.q_without_bon);

                    DateTime dt = DateTime.ParseExact(step.d.ToString(), "HHmmssddMMyy", provider);
                    ps.EndDate = FormatDate(dt);
                    ps.EndDate_Clean = step.d.ToString();

                    if (Convert.ToInt32(step.num_days) == 0)
                    {
                        ps.Days = Resources.Parking_Amount_Today;
                        ps.DaysClass = "badge-day-0";
                    }
                    else if (Convert.ToInt32(step.num_days) == 1)
                    {
                        ps.Days = Resources.Parking_Amount_Tomorrow;
                        ps.DaysClass = "badge-day-1";
                    }
                    else
                    {
                        ps.Days = string.Format(Resources.Parking_Amount_In_X_days, Convert.ToInt32(step.num_days).ToString());
                        ps.DaysClass = "badge-day-more";
                    }                    

                    model.Steps.Add(ps);
                }
            }
            else
            {
                model.Error = resBundle.GetString(string.Format("Permits_Error_{0}", res));
            }

            SaveModel();
            if (model.Buttons.Count > 0)
            {
                return View(model);
            }
            else
            {
                return View("SelectTime", model);
            }
        }

        [HttpPost]
        public ActionResult ConfirmPayment(decimal? SelectedStep)
        {
            LoadModel();

            string RedirectOnFail = "SelectAmount";
            string RedirectOnSuccess = "PaymentSuccessful";

            foreach (ParkingStep s in model.Steps)
            {
                if (s.Time == Convert.ToInt32(SelectedStep))
                {
                    model.Step = s;
                    break;
                }
            }

            model.Error = string.Empty;

            dynamic wsParameters;

            ResultType res = wswi.ConfirmParkingOperationGuestUser(
                model.Step.Quantity_Clean,
                DateTime.Now.ToString("HHmmssddMMyy"),
                model.Step.EndDate_Clean,
                model.InitialDate_Clean,
                model.Group,
                model.Plate,
                model.Step.QuantityWithoutBon_Clean,
                model.Step.QuantityReal_Clean,
                model.Step.QuantityFee_Clean,
                model.Rate,
                model.Step.Time,
                model.Step.TimeBalanceUsed,
                model.Step.QuantityTotal_Clean,
                model.Step.QuantityVat_Clean,
                model.SessionID,
                model.User,                
                out wsParameters
            );

            if (wsParameters.ipark_out.operationid != null) 
            {
                model.OperationId = Convert.ToInt64(wsParameters.ipark_out.operationid);
            }

            if (res != ResultType.Result_OK)
            {
                model.Error = resBundle.GetString(string.Format("Permits_Error_{0}", res.ToString()));
                SaveModel();
                return RedirectToAction(RedirectOnFail);
            }
            else
            {
                SaveModel();
                return RedirectToAction(RedirectOnSuccess);
            }
        }
        /*
        // Create an assessment to analyze the risk of an UI action.
        // projectID: GCloud Project ID.
        // recaptchaSiteKey: Site key obtained by registering a domain/app to use recaptcha.
        // token: The token obtained from the client on passing the recaptchaSiteKey.
        // recaptchaAction: Action name corresponding to the token.
        public bool CreateAssessment(string projectID, string recaptchaSiteKey, string token, string recaptchaAction, out string error, out string info)
        {
            error = string.Empty;
            info = string.Empty;

            // Create the client.
            // TODO: To avoid memory issues, move this client generation outside
            // of this example, and cache it (recommended) or call client.close()
            // before exiting this method.
            RecaptchaEnterpriseServiceClient client = RecaptchaEnterpriseServiceClient.Create();

            ProjectName projectName = new ProjectName(projectID);

            // Build the assessment request.
            CreateAssessmentRequest createAssessmentRequest = new CreateAssessmentRequest()
            {
                Assessment = new Assessment()
                {
                    // Set the properties of the event to be tracked.
                    Event = new Event()
                    {
                        SiteKey = recaptchaSiteKey,
                        Token = token,
                        ExpectedAction = recaptchaAction
                    },
                },
                ParentAsProjectName = projectName
            };

            Assessment response = client.CreateAssessment(createAssessmentRequest);

            // Check if the token is valid.
            if (response.TokenProperties.Valid == false)
            {
                error = "The CreateAssessment call failed because the token was: " + response.TokenProperties.InvalidReason.ToString();
                return false;
            }

            // Check if the expected action was executed.
            if (response.TokenProperties.Action != recaptchaAction)
            {
                error = "The action attribute in reCAPTCHA tag is: " + response.TokenProperties.Action.ToString() + ". The action attribute in the reCAPTCHA tag does not match the action you are expecting to score";
                return false;
            }

            // Get the risk score and the reason(s).
            // For more information on interpreting the assessment,
            // see: https://cloud.google.com/recaptcha-enterprise/docs/interpret-assessment
            info = "The reCAPTCHA score is: " + ((decimal)response.RiskAnalysis.Score);

            foreach (RiskAnalysis.Types.ClassificationReason reason in response.RiskAnalysis.Reasons)
            {
                info += ". " + reason.ToString();
            }

            return true;
        }
        */
        public string RecaptchaVerify(string recaptchaToken)
        {
            string url = $"https://www.google.com/recaptcha/api/siteverify?secret=" + ConfigurationManager.AppSettings["RecaptchaSecret"] + "&response=" + recaptchaToken;
            using (var httpClient = new HttpClient())
            {
                string responseString = httpClient.GetStringAsync(url).Result;
                return responseString;
            }
        }

        public class ResponseToken
        {
            public DateTime challenge_ts { get; set; }
            public List<string> ErrorCodes { get; set; }
            public bool Success { get; set; }
            public string hostname { get; set; }
        }

        [HttpPost]
        public ActionResult PayWithCreditCard(decimal? SelectedStep)
        {
            LoadModel();
            m_Log.LogMessage(LogLevels.logWARN, string.Format("PayWithCreditCard attempt: IP: {0}", HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]));
            string RecaptchaToken = string.Empty;

            if (Request != null && Request.Form != null && Request.Form.AllKeys.Contains("g-recaptcha-response"))
            {
                RecaptchaToken = Request.Form["g-recaptcha-response"];
            }

            if (string.IsNullOrEmpty(RecaptchaToken)) 
            {
                m_Log.LogMessage(LogLevels.logERROR, "PayWithCreditCard::Recaptcha failed (token is empty)");
                model.Error = Resources.RecaptchaFail;
                SaveModel();
                return RedirectToAction("SelectAmount");
            }

            var responseString = RecaptchaVerify(RecaptchaToken);
            ResponseToken response = new ResponseToken();
            response = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseToken>(responseString);

            m_Log.LogMessage(LogLevels.logWARN, string.Format("PayWithCreditCard Recaptcha response: Success: {0} - ChallengeTS: {1} - HostName: {2}", response.Success, response.challenge_ts, response.hostname));

            if (response.ErrorCodes != null && response.ErrorCodes.Count > 0) {
                foreach (string ErrorCode in response.ErrorCodes)
                {
                    m_Log.LogMessage(LogLevels.logWARN, string.Format("PayWithCreditCard Recaptcha ErrorCode: {0}", ErrorCode));
                }
            }

            if (response.Success == false) 
            {
                m_Log.LogMessage(LogLevels.logERROR, "PayWithCreditCard::Recaptcha failed (Recaptcha challenge not passed)");
                model.Error = Resources.RecaptchaFail;
                SaveModel();
                return RedirectToAction("SelectAmount");
            }

            /*
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @ConfigurationManager.AppSettings["RecaptchaKeyPath"], EnvironmentVariableTarget.Process);

            string RecaptchaId = ConfigurationManager.AppSettings["RecaptchaId"];

            if (!CreateAssessment("webapp-recaptcha-integra", RecaptchaId, RecaptchaToken, "CreditCardForm", out error, out info)) 
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PayWithCreditCard::CreateAssessment error: {0}", error));
                model.Error = error;
                SaveModel();
                return RedirectToAction("SelectAmount");
            }
            m_Log.LogMessage(LogLevels.logINFO, string.Format("PayWithCreditCard::CreateAssessment info: {0}", info));
            */

            foreach (ParkingStep s in model.Steps)
            {
                if (s.Time == Convert.ToInt32(SelectedStep))
                {
                    model.Step = s;
                    break;
                }
            }

            model.PaymentParams.Description = string.Format("Anonymous parking payment: Plate {0} | Zone {1} | Rate {2} | IniDate {3} | EndDate {4} | Amount {5}", model.Plate, model.Group, model.Rate, model.InitialDate, model.Step.EndDate, model.Step.QuantityTotal);
            model.PaymentParams.Amount = Convert.ToInt32(model.Step.Quantity_Clean);
            model.PaymentParams.UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            model.PaymentParams.ExternalId = string.Format("{0}{1}", Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            if (model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe) 
            {
                model.PaymentParams.Hash = wswi.CalculateHash(
                    model.PaymentParams.CreditCard.Guid,
                    model.PaymentParams.Email,
                    (int)model.Step.QuantityTotal_Clean,
                    model.PaymentParams.CurrencyISOCODE,
                    model.PaymentParams.Description,
                    model.PaymentParams.UTCDate,
                    "",
                    model.PaymentParams.ReturnURL,
                    model.PaymentParams.CreditCard.HashSeed
                );
            }
            else if (model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys)
            {
                model.PaymentParams.Hash = wswi.CalculateHash(
                        model.PaymentParams.CreditCard.Guid,
                        model.PaymentParams.Email,
                        (int)model.Step.QuantityTotal_Clean,
                        model.PaymentParams.CurrencyISOCODE,
                        model.PaymentParams.Description,
                        model.PaymentParams.UTCDate,
                        model.PaymentParams.Culture,
                        model.PaymentParams.ReturnURL,
                        model.PaymentParams.CreditCard.HashSeed
                    );
            }
            else
            {
                model.PaymentParams.Hash = wswi.CalculateHash(
                    model.PaymentParams.CreditCard.Guid, 
                    model.PaymentParams.Email, 
                    (int)model.Step.QuantityTotal_Clean, 
                    model.PaymentParams.CurrencyISOCODE, 
                    model.PaymentParams.Description, 
                    model.PaymentParams.UTCDate, 
                    model.PaymentParams.Culture, 
                    model.PaymentParams.ReturnURL, 
                    model.PaymentParams.CreditCard.HashSeed,
                    "",
                    "",
                    "",
                    "",
                    model.PaymentParams.ExternalId
                );
            }

            model.HashSeed = model.PaymentParams.CreditCard.HashSeed;

            SaveModel();
            SaveSession(model.PaymentParams.ExternalId);

            return View(model);
        }

        [HttpPost]
        public ActionResult PayWithPayPal(decimal? SelectedStep)
        {
            LoadModel();
            m_Log.LogMessage(LogLevels.logWARN, string.Format("PayWithPayPal attempt: IP: {0}", HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]));
            string RecaptchaToken = string.Empty;

            if (Request != null && Request.Form != null && Request.Form.AllKeys.Contains("g-recaptcha-response"))
            {
                RecaptchaToken = Request.Form["g-recaptcha-response"];
            }

            if (string.IsNullOrEmpty(RecaptchaToken))
            {
                m_Log.LogMessage(LogLevels.logERROR, "PayWithPayPal::Recaptcha failed (token is empty)");
                model.Error = Resources.RecaptchaFail;
                SaveModel();
                return RedirectToAction("SelectAmount");
            }

            var responseString = RecaptchaVerify(RecaptchaToken);
            ResponseToken response = new ResponseToken();
            response = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseToken>(responseString);

            m_Log.LogMessage(LogLevels.logWARN, string.Format("PayWithPayPal Recaptcha response: Success: {0} - ChallengeTS: {1} - HostName: {2}", response.Success, response.challenge_ts, response.hostname));

            if (response.ErrorCodes != null && response.ErrorCodes.Count > 0)
            {
                foreach (string ErrorCode in response.ErrorCodes)
                {
                    m_Log.LogMessage(LogLevels.logWARN, string.Format("PayWithPayPal Recaptcha ErrorCode: {0}", ErrorCode));
                }
            }

            if (response.Success == false)
            {
                m_Log.LogMessage(LogLevels.logERROR, "PayWithPayPal::Recaptcha failed (Recaptcha challenge not passed)");
                model.Error = Resources.RecaptchaFail;
                SaveModel();
                return RedirectToAction("SelectAmount");
            }

            foreach (ParkingStep s in model.Steps)
            {
                if (s.Time == Convert.ToInt32(SelectedStep))
                {
                    model.Step = s;
                    break;
                }
            }

            model.PaymentParams.Description = string.Format("Anonymous parking payment: Plate {0} | Zone {1} | Rate {2} | IniDate {3} | EndDate {4} | Amount {5}", model.Plate, model.Group, model.Rate, model.InitialDate, model.Step.EndDate, model.Step.QuantityTotal);
            model.PaymentParams.Amount = Convert.ToInt32(model.Step.Quantity_Clean);
            model.PaymentParams.UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            model.PaymentParams.ExternalId = string.Format("{0}{1}", Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            model.PaymentParams.Hash = wswi.CalculateHash(
                model.PaymentParams.PayPal.Guid, 
                model.PaymentParams.Email, 
                (int)model.Step.QuantityTotal_Clean, 
                model.PaymentParams.CurrencyISOCODE, 
                model.PaymentParams.Description, 
                model.PaymentParams.UTCDate, 
                model.PaymentParams.Culture, 
                model.PaymentParams.ReturnURL, 
                model.PaymentParams.PayPal.HashSeed, 
                model.PaymentParams.CancelURL, 
                "0", 
                "0", 
                model.Step.QuantityTotal_Clean.ToString(), 
                model.PaymentParams.ExternalId
            );

            model.HashSeed = model.PaymentParams.PayPal.HashSeed;

            SaveModel();
            SaveSession(model.PaymentParams.ExternalId);

            return View(model);
        }

        [HttpGet]
        public ActionResult PaymentResultGet(string r, string ExternalId)
        {
            return PaymentResult(r, ExternalId);
        }

        [HttpPost]
        public ActionResult PaymentResult(string r, string ExternalId)
        {
            LoadModel();

            if (model.HashSeed == null)
            {
                LoadSession(ExternalId);
                LoadModel();
            }

            string RedirectOnFail = "SelectAmount";
            string RedirectOnSuccess = "PaymentSuccessful";

            string result = wswi.DecryptCryptResult(r, model.HashSeed);
            dynamic j = JsonConvert.DeserializeObject(result);
            bool TransactionFailed = false;

            PaymentReturnData prd = new PaymentReturnData();

            prd.Email = j.email.ToString();
            prd.Amount = j.amount.ToString();
            prd.Currency = j.currency.ToString();
            prd.Result = j.result.ToString();
            prd.ErrorCode = j.errorCode.ToString();
            prd.ErrorMessage = j.errorMessage.ToString();

            try
            {
                model.MaskedCardNumber = string.Empty;

                model.Paymeth = 1;

                if (j.paypal_PayerID != null &&
                    j.paypal_paymentId != null &&
                    j.paypal_token != null)
                {
                    prd.paypal_PayerID = j.paypal_PayerID;
                    prd.paypal_paymentId = j.paypal_paymentId;
                    prd.paypal_token = j.paypal_token;

                    model.Paymeth = 2;
                }
                else
                {
                    switch (model.PaymentProvider)
                    {
                        case PaymentMeanCreditCardProviderType.pmccpCreditCall:
                            if (j.ekashu_reference != null &&
                                j.ekashu_auth_code != null &&
                                j.ekashu_auth_result != null &&
                                j.ekashu_card_hash != null &&
                                j.ekashu_card_reference != null &&
                                j.ekashu_card_scheme != null &&
                                j.ekashu_date_time_local_fmt != null &&
                                j.ekashu_masked_card_number != null &&
                                j.ekashu_transaction_id != null &&
                                j.ekashu_expires_end_month != null &&
                                j.ekashu_expires_end_year != null)
                            {
                                prd.ekashu_reference = j.ekashu_reference.ToString();
                                prd.ekashu_auth_code = j.ekashu_auth_code.ToString();
                                prd.ekashu_auth_result = j.ekashu_auth_result.ToString();
                                prd.ekashu_card_hash = j.ekashu_card_hash.ToString();
                                prd.ekashu_card_reference = j.ekashu_card_reference.ToString();
                                prd.ekashu_card_scheme = j.ekashu_card_scheme.ToString();
                                prd.ekashu_date_time_local_fmt = j.ekashu_date_time_local_fmt.ToString();
                                prd.ekashu_masked_card_number = j.ekashu_masked_card_number.ToString();
                                prd.ekashu_transaction_id = j.ekashu_transaction_id.ToString();
                                prd.ekashu_expires_end_month = j.ekashu_expires_end_month.ToString();
                                prd.ekashu_expires_end_year = j.ekashu_expires_end_year.ToString();

                                model.MaskedCardNumber = prd.ekashu_masked_card_number;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpIECISA:
                            if (j.cardToken != null &&
                                j.cardHash != null &&
                                j.cardScheme != null &&
                                j.cardPAN != null &&
                                j.cardExpirationDate != null &&
                                j.chargeDateTime != null &&
                                j.cardCFAuthCode != null &&
                                j.cardCFTicketNumber != null &&
                                j.cardCFTransactionID != null &&
                                j.cardTransactionID != null)
                            {
                                prd.CardToken = j.cardToken.ToString();
                                prd.CardHash = j.cardHash.ToString();
                                prd.CardScheme = j.cardScheme.ToString();
                                prd.CardPAN = j.cardPAN.ToString();
                                prd.CardExpirationDate = j.cardExpirationDate.ToString();
                                prd.ChargeDateTime = j.chargeDateTime.ToString();
                                prd.CardCFAuthCode = j.cardCFAuthCode.ToString();
                                prd.CardCFTicketNumber = j.cardCFTicketNumber.ToString();
                                prd.CardCFTransactionID = j.cardCFTransactionID.ToString();
                                prd.CardTransactionID = j.cardTransactionID.ToString();

                                model.MaskedCardNumber = prd.CardPAN;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpStripe:
                            if (j.customerID != null &&
                                j.cardToken != null &&
                                j.cardScheme != null &&
                                j.cardPAN != null &&
                                j.cardExpMonth != null &&
                                j.cardExpYear != null &&
                                j.chargeID != null &&
                                j.chargeDateTime != null)
                            {
                                prd.stripe_customer_id = j.customerID.ToString();
                                prd.stripe_card_reference = j.cardToken.ToString();
                                prd.stripe_card_scheme = j.cardScheme.ToString();
                                prd.stripe_masked_card_number = j.cardPAN.ToString();
                                prd.stripe_expires_end_month = j.cardExpMonth.ToString();
                                prd.stripe_expires_end_year = j.cardExpYear.ToString();
                                prd.stripe_transaction_id = j.chargeID.ToString();
                                prd.stripe_date_time_utc = j.chargeDateTime.ToString();

                                model.MaskedCardNumber = prd.stripe_masked_card_number;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpMoneris:
                            if (j.moneris_card_reference != null &&
                                j.moneris_card_hash != null &&
                                j.moneris_card_scheme != null &&
                                j.moneris_masked_card_number != null &&
                                j.moneris_expires_end_month != null &&
                                j.moneris_expires_end_year != null &&
                                j.moneris_date_time_local_fmt != null &&
                                j.moneris_reference != null &&
                                j.moneris_transaction_id != null &&
                                j.moneris_auth_code != null &&
                                j.moneris_auth_result != null)
                            {
                                prd.moneris_card_reference = j.moneris_card_reference.ToString();
                                prd.moneris_card_hash = j.moneris_card_hash.ToString();
                                prd.moneris_card_scheme = j.moneris_card_scheme.ToString();
                                prd.moneris_masked_card_number = j.moneris_masked_card_number.ToString();
                                prd.moneris_expires_end_month = j.moneris_expires_end_month.ToString();
                                prd.moneris_expires_end_year = j.moneris_expires_end_year.ToString();
                                prd.moneris_date_time_local_fmt = j.moneris_date_time_local_fmt.ToString();
                                prd.moneris_reference = j.moneris_reference.ToString();
                                prd.moneris_transaction_id = j.moneris_transaction_id.ToString();
                                prd.moneris_auth_code = j.moneris_auth_code.ToString();
                                prd.moneris_auth_result = j.moneris_auth_result.ToString();

                                model.MaskedCardNumber = prd.moneris_masked_card_number;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpTransbank:
                            if (j.transbank_auth_code != null &&
                                j.transbank_card_hash != null &&
                                j.transbank_card_reference != null &&
                                j.transbank_card_scheme != null &&
                                j.transbank_date_time_local_fmt != null &&
                                j.transbank_masked_card_number != null &&
                                j.transbank_transaction_id != null)
                            {
                                prd.transbank_auth_code = j.transbank_auth_code.ToString();
                                prd.transbank_card_hash = j.transbank_card_hash.ToString();
                                prd.transbank_card_reference = j.transbank_card_reference.ToString();
                                prd.transbank_card_scheme = j.transbank_card_scheme.ToString();
                                prd.transbank_date_time_local_fmt = j.transbank_date_time_local_fmt.ToString();
                                prd.transbank_masked_card_number = j.transbank_masked_card_number.ToString();
                                prd.transbank_transaction_id = j.transbank_transaction_id.ToString();

                                model.MaskedCardNumber = prd.transbank_masked_card_number;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPayu:
                            if (j.payu_reference != null &&
                                j.payu_auth_code != null &&
                                j.payu_card_hash != null &&
                                j.payu_card_reference != null &&
                                j.payu_card_scheme != null &&
                                j.payu_date_time_local_fmt != null &&
                                j.payu_masked_card_number != null &&
                                j.payu_transaction_id != null)
                            {
                                prd.payu_reference = j.payu_reference.ToString();
                                prd.payu_auth_code = j.payu_auth_code.ToString();
                                prd.payu_card_hash = j.payu_card_hash.ToString();
                                prd.payu_card_reference = j.payu_card_reference.ToString();
                                prd.payu_card_scheme = j.payu_card_scheme.ToString();
                                prd.payu_date_time_local_fmt = j.payu_date_time_local_fmt.ToString();
                                prd.payu_masked_card_number = j.payu_masked_card_number.ToString();
                                prd.payu_transaction_id = j.payu_transaction_id.ToString();

                                model.MaskedCardNumber = prd.payu_masked_card_number;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpBSRedsys:
                            if (j.bsredsys_reference != null &&
                                j.bsredsys_auth_code != null &&
                                j.bsredsys_card_hash != null &&
                                j.bsredsys_card_reference != null &&
                                j.bsredsys_card_scheme != null &&
                                j.bsredsys_date_time_local_fmt != null &&
                                j.bsredsys_masked_card_number != null &&
                                j.bsredsys_transaction_id != null)
                            {
                                prd.bsredsys_reference = j.bsredsys_reference.ToString();
                                prd.bsredsys_auth_code = j.bsredsys_auth_code.ToString();
                                prd.bsredsys_auth_result = j.bsredsys_auth_result.ToString();
                                prd.bsredsys_card_hash = j.bsredsys_card_hash.ToString();
                                prd.bsredsys_card_reference = j.bsredsys_card_reference.ToString();
                                prd.bsredsys_card_scheme = j.bsredsys_card_scheme.ToString();
                                prd.bsredsys_date_time_local_fmt = j.bsredsys_date_time_local_fmt.ToString();
                                prd.bsredsys_masked_card_number = j.bsredsys_masked_card_number.ToString();
                                prd.bsredsys_transaction_id = j.bsredsys_transaction_id.ToString();
                                prd.bsredsys_expires_end_month = j.bsredsys_expires_end_month.ToString();
                                prd.bsredsys_expires_end_year = j.bsredsys_expires_end_year.ToString();

                                model.MaskedCardNumber = prd.bsredsys_masked_card_number;
                            }
                            else
                            {
                                TransactionFailed = true;
                            }
                            break;
                        default:
                            TransactionFailed = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TransactionFailed = true;
                if (!string.IsNullOrEmpty(prd.ErrorMessage))
                {
                    model.Error = string.Format("[{0}] {1}", prd.ErrorCode, prd.ErrorMessage).Replace("[] ", string.Empty);
                }
                else
                {
                    model.Error = string.Format("[{0}] {1}", prd.ErrorCode, ex.Message).Replace("[] ", string.Empty);
                }

                SaveModel();

                return RedirectToAction(RedirectOnFail);
            }

            model.PaymentReturn = prd;

            if (TransactionFailed)
            {
                if (!string.IsNullOrEmpty(prd.ErrorMessage) || !string.IsNullOrEmpty(prd.ErrorCode))
                {
                    model.Error = string.Format("[{0}] {1}", prd.ErrorCode, prd.ErrorMessage).Replace("[] ", string.Empty);
                }
                else
                {
                    model.Error = resBundle.GetString("Permits_Error_Result_Error_Generic");
                }

                SaveModel();

                return RedirectToAction(RedirectOnFail);
            }
            else
            {
                model.Error = string.Empty;

                string s3DS_URL = "";
                dynamic wsParameters;

                ResultType res = wswi.ConfirmParkingOperationGuestUser(
                    model.Step.Quantity_Clean,
                    DateTime.Now.ToString("HHmmssddMMyy"),
                    model.Step.EndDate_Clean,
                    model.InitialDate_Clean,
                    model.Group,
                    model.Plate,
                    model.Step.QuantityWithoutBon_Clean,
                    model.Step.QuantityReal_Clean,
                    model.Step.QuantityFee_Clean,
                    model.Rate,
                    model.Step.Time,
                    model.Step.TimeBalanceUsed,
                    model.Step.QuantityTotal_Clean,
                    model.Step.QuantityVat_Clean,
                    model.SessionID,
                    model.User,
                    0,
                    null,
                    null,
                    null,
                    false,
                    model.Paymeth,
                    (int)model.PaymentProvider,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_card_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_card_hash : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_card_scheme : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_masked_card_number : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_expires_end_month : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_expires_end_year : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_date_time_local_fmt : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_transaction_id : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_auth_code : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpMoneris ? model.PaymentReturn.moneris_auth_result : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_auth_code : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_card_hash : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_card_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_card_scheme : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_date_time_local_fmt : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_masked_card_number : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpPayu ? model.PaymentReturn.payu_transaction_id : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardToken : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardHash : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardScheme : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardPAN : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardExpirationDate : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.ChargeDateTime : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardCFAuthCode : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardCFTicketNumber : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardCFTransactionID : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpIECISA ? model.PaymentReturn.CardTransactionID : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_card_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_card_scheme : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_customer_id : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_date_time_utc : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_expires_end_month : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_expires_end_year : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_masked_card_number : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpStripe ? model.PaymentReturn.stripe_transaction_id : string.Empty,
                    model.Paymeth == 2 ? model.PaymentReturn.paypal_PayerID : string.Empty,
                    model.Paymeth == 2 ? model.PaymentReturn.paypal_paymentId : string.Empty,
                    model.Paymeth == 2 ? model.PaymentReturn.paypal_token : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_card_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_card_hash : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_card_scheme : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_masked_card_number : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_expires_end_month : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_expires_end_year : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_date_time_local_fmt : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_reference : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_transaction_id : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_auth_code : string.Empty,
                    model.Paymeth == 1 && model.PaymentProvider == PaymentMeanCreditCardProviderType.pmccpBSRedsys ? model.PaymentReturn.bsredsys_auth_result : string.Empty,
                    out s3DS_URL,
                    out wsParameters
                );

                if (wsParameters.ipark_out.operationid != null) {
                    model.OperationId = Convert.ToInt64(wsParameters.ipark_out.operationid);
                }

                if (res != ResultType.Result_OK)
                {
                    model.Error = resBundle.GetString(string.Format("Permits_Error_{0}", res.ToString()));
                    SaveModel();
                    return RedirectToAction(RedirectOnFail);
                }
                else
                {
                    SaveModel();
                    return RedirectToAction(RedirectOnSuccess);
                }
            }
        }

        public ActionResult PaymentSuccessful()
        {
            LoadModel();
            return View(model);
        }

        public JsonResult SendMail(string Email)
        {
            LoadModel();

            Dictionary<string, string> mailRes = new Dictionary<string, string>();

            dynamic parametersOut = null;
            ResultType res = wswi.SendParkingEmailTo(model.OperationId, Email, model.SessionID, model.User, out parametersOut);

            if (res != ResultType.Result_OK)
            {
                mailRes["Error"] = resBundle.GetString(string.Format("Permits_Error_{0}", res));
                mailRes["Success"] = "0";
            }
            else {
                mailRes["Success"] = "1";
            }

            return Json(mailRes, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Method

        private void LoadModel()
        {
            if (Session["ParkingModel"] == null)
            {
                model = new ParkingModel();
                model.PreSelectionDone = false;
            }
            else
            {
                model = (ParkingModel)Session["ParkingModel"];

                //Set the Culture.
                Thread.CurrentThread.CurrentCulture = new CultureInfo(model.Culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(model.Culture);
                Resources.Culture = new CultureInfo(model.Culture);
            }
        }

        private void SaveModel()
        {
            model.Culture = GetCultureFromBrowser();
            model.LanguageCode = GetLangFromCulture();

            Session["ParkingModel"] = model;
        }

        private string GetCultureFromBrowser()
        {
            string language = "en-US";

            //Detect User's Language.
            if (Request.UserLanguages != null)
            {
                //Set the Language.
                language = Request.UserLanguages[0];
            }

            if (string.IsNullOrEmpty(language))
            {
                language = "en-US";
            }

            //Set the Culture.
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Resources.Culture = new CultureInfo(language);            
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            if (ci != null)
            {
                return ci.ToString();
            }
            else
            {
                string url = string.Empty;
                try
                {
                    url = HttpContext.Request.Url.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    url = "Unable to get (" + ex.Message + ")";
                }
                m_Log.LogMessage(LogLevels.logWARN, "GetCultureFromBrowser::Unable to load Culture for Language = " + language + " - URL = " + url);

                language = "en-US";
                //Set the Culture.
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Resources.Culture = new CultureInfo(language);
                return language;
            }            
        }

        private ParkingLanguage GetLangFromCulture()
        {
            ParkingLanguage Lang = ParkingLanguage.enUS;

            switch (model.Culture)
            { 
                case "es-ES":
                    Lang = ParkingLanguage.esES;
                    break;
                case "en-US":
                case "en-CA":
                    Lang = ParkingLanguage.enUS;
                    break;
                case "fr-FR":
                case "fr-CA":
                    Lang = ParkingLanguage.frFR;
                    break;
                case "ca-ES":
                    Lang = ParkingLanguage.caES;
                    break;
                case "es-MX":
                    Lang = ParkingLanguage.esMX;
                    break;
                case "eu-ES":
                    Lang = ParkingLanguage.euES;
                    break;
                case "it-IT":
                    Lang = ParkingLanguage.itIT;
                    break;
                default:
                    Lang = ParkingLanguage.enUS;
                    break;
            }

            return Lang;
        }

        private string FormatDate(DateTime dt)
        {
            return UppercaseFirst(dt
                .ToString("MMM d, yyyy | h:mm tt", CultureInfo.InvariantCulture)
                .Replace("AM", "a.m.")
                .Replace("PM", "p.m."));
        }

        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        protected void SaveSession(string key)
        {
            Dictionary<string, string> oSessionDict = new Dictionary<string, string>();

            for (int i = 0; i < Session.Count; i++)
            {
                var crntSession = Session.Keys[i];

                Dictionary<string, string> oValueDict = new Dictionary<string, string>();

                if (Session[crntSession] == null)
                {
                    oValueDict["type"] = "null";
                }
                else
                {
                    oValueDict["type"] = Session[crntSession].GetType().ToString();
                }

                oValueDict["value"] = JsonConvert.SerializeObject(Session[crntSession]);

                oSessionDict[crntSession] = JsonConvert.SerializeObject(oValueDict);

            }

            var json = JsonConvert.SerializeObject(oSessionDict);

            infraestructureRepository.InsertOrUpdateSessionVariables(key, json);
        }


        protected void LoadSession(string key)
        {

            string jsonSession = infraestructureRepository.GetSessionVariables(key);

            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSession);

            foreach (KeyValuePair<string, string> kvValue in values)
            {

                string strType = "";
                string strValue = "";
                var value = JsonConvert.DeserializeObject<Dictionary<string, string>>(kvValue.Value);

                foreach (KeyValuePair<string, string> variable in value)
                {
                    if (variable.Key == "type")
                    {
                        strType = variable.Value;
                    }
                    else
                        strValue = variable.Value;

                }

                if (strType == "null")
                {
                    Session[kvValue.Key] = null;
                }
                else if (strType == "System.Globalization.CultureInfo")
                {
                    Session[kvValue.Key] = new CultureInfo(JsonConvert.DeserializeObject<string>(strValue));
                }
                else
                {
                    Session[kvValue.Key] = JsonConvert.DeserializeObject(strValue, Type.GetType(strType));
                }

            }
        }

        protected string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }

        #endregion

    }
}
