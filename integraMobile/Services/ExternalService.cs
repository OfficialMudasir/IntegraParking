using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using Ninject;
//using Ninject.Web;
using Newtonsoft.Json;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.ExternalWS;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Helper;

namespace integraMobile.Services
{
    [Serializable]
    public class ExternalService
    {
        #region Properties

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(ExternalService));
        private const string _xmlTagName = "ipark";
        private const string IN_SUFIX = "_in";
        private const string OUT_SUFIX = "_out";

        #endregion

        [WebMethod]
        public string GetFine(string sTicketNumber, int iInstallation, IFineRepository iFineRepository, ICustomersRepository iCustomersRepository, IInfraestructureRepository iInfraestructureRepository, IGeograficAndTariffsRepository iGeograficAndTariffsRepository, out decimal? InstallationIdFound, integraMobileDBEntitiesDataContext dbContext = null, string LicensePlate = "", string InstallationList = "", string StandardInstallationList = "")
        {
            string xmlOut = "";
            InstallationIdFound = null;
            sTicketNumber = sTicketNumber.Trim();
            if (!string.IsNullOrEmpty(LicensePlate))
            {
                LicensePlate = LicensePlate.Trim();
            }

            try
            {
                SortedList parametersOut = null;
                String SessionID = new Guid().ToString(); // For AddSessionTicketPaymentInfo 
                ResultType rt = ResultType.Result_OK;
                Decimal? dInstallationId = null;

                Logger_AddLogMessage(string.Format("GetFine: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logINFO);

                try
                {
                    Decimal dTryInstallationId = Convert.ToDecimal(iInstallation.ToString());
                    dInstallationId = dTryInstallationId;
                }
                catch
                {
                    dInstallationId = null;
                }

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                string strFineNumber = sTicketNumber;
                strFineNumber = strFineNumber.Trim();

                parametersOut = new SortedList();
                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                string strCulture = "";

                ThirdPartyFine oThirdPartyFine = new ThirdPartyFine();
                USER oUser = null;

                if (!(!string.IsNullOrEmpty(InstallationList) && !string.IsNullOrEmpty(LicensePlate) && !string.IsNullOrEmpty(sTicketNumber)))
                {
                    if (!iGeograficAndTariffsRepository.getInstallationById(dInstallationId,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return xmlOut;
                    }
                    else 
                    {
                        InstallationIdFound = dInstallationId;
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;
                

                    if (oInstallation != null) 
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }


                    // MICHEL DEV
                    //oInstallation.INS_FINE_WS_SIGNATURE_TYPE = (int)FineWSSignatureType.fst_test;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);

                    switch ((FineWSSignatureType)oInstallation.INS_FINE_WS_SIGNATURE_TYPE)
                    {
                        case FineWSSignatureType.fst_gtechna:
                            {
                                rt = oThirdPartyFine.GtechnaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (rt != ResultType.Result_OK)
                                {                              
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_standard:
                            {
                                rt = oThirdPartyFine.StandardQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_eysa:
                            {
                                string strCulturePrefix = strCulture.ToLower().Substring(0, 2);

                                rt = oThirdPartyFine.EysaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, strCulturePrefix, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                                else
                                {
                                    if (parametersOut.ContainsKey("sector"))
                                    {
                                        CultureInfo ci = new CultureInfo(strCulture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;


                                        parametersOut["zone"] = ResourceExtension.GetLiteral("Zone_" + parametersOut["sector"].ToString());
                                        parametersOut["sector"] = ResourceExtension.GetLiteral("Sector_" + parametersOut["sector"].ToString());
                                    }
                                    else
                                    {
                                        parametersOut["zone"] = "----";
                                        parametersOut["sector"] = "----";
                                    }
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_madidplatform:
                            {
                                rt = oThirdPartyFine.MadridPlatformQueryFinePaymentQuantity(strFineNumber, null,dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_valoriza:
                            {
                                rt = oThirdPartyFine.ValorizaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_internal:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        case FineWSSignatureType.fst_test:
                            switch (sTicketNumber.ToString().Substring(sTicketNumber.ToString().Length - 1, 1))
                            {
                                case "0":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 10000;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "1":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 200;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "2":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 300;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "3":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 400;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "4":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 500;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "5":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 600;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "6":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Not_Found;
                                    break;
                                case "7":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Type_Not_Payable;
                                    break;
                                case "8":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Payment_Period_Expired;
                                    break;
                                case "9":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Already_Paid;
                                    break;
                                default:
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 100;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0));
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0));
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;

                            }
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        default:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;

                    }
                }
                else 
                {
                    string sCityID = string.Empty;

                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(StandardInstallationList.Split(',')[0].ToString(),
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return xmlOut;
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                    string WSUrl = oInstallation.INS_FINE_WS_URL;
                    string WSAuthHashKey = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                    string WSHttpUser = oInstallation.INS_FINE_WS_HTTP_USER;
                    string WSHttpPassword = oInstallation.INS_FINE_WS_HTTP_PASSWORD;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);


                    rt = oThirdPartyFine.StandardQueryFinePaymentQuantityMultiInstallations(strFineNumber, dtinstDateTime.Value, oUser, StandardInstallationList.Split(',').ToList<string>(), 
                                                                                            WSUrl, WSAuthHashKey, WSHttpUser, WSHttpPassword, oInstallation.CURRENCy.CUR_ISO_CODE, iWSTimeout, 
                                                                                            ref parametersOut, out sCityID);
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();

                    Logger_AddLogMessage(string.Format("GetFine:: 1 --> CityId={0}; TicketNumber={1}",sCityID, sTicketNumber), LogLevels.logERROR);


                    if (rt != ResultType.Result_OK)
                    {
                        xmlOut = GenerateXMLOuput(parametersOut);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = oInstallation.INS_ID;
                        return xmlOut;
                    }
                    else if (parametersOut["lp"].ToString().Trim() != LicensePlate.Trim())
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Fine_Number_Not_Found);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return xmlOut;
                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 2 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                    

                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(sCityID,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return xmlOut;
                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 3 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    InstallationIdFound = oInstallation.INS_ID;
                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;

                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                }


                //double dChangeToApply = 1.0;

                if (rt == ResultType.Result_OK)
                {
                    int iQ = 0;
                    int iQFEE = 0;
                    decimal dQFEE = 0;
                  //  int iQFEEChange = 0;
                    int iQVAT = 0;
                    int iQTotal = 0;
                    //int iQTotalChange = 0;
                    int iQSubTotal = 0;
                    //int iQSubTotalChange = 0;

                    decimal dVAT1 = 0;
                    decimal dVAT2 = 0;
                    int iPartialVAT1;
                    decimal dPercFEE = 0;
                    decimal dPercFEETopped = 0;
                    int iPartialPercFEE;
                    decimal dFixedFEE = 0;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;

                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
                    
                    // Moved this line outside the if because amount must be calculated always 
                    iQ = Convert.ToInt32(parametersOut["q"].ToString());

                    //if (oUser != null)
                    //{ 
                    //if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                    //{
                    iPaymentTypeId = 1; // Credit/Debit Card // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                    iPaymentSubtypeId = -1; // UNDEFINED // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                    //}

                    if (!iCustomersRepository.GetFinantialParams(oInstallation.INS_ID, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.TicketPayment, null, 
                        out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                    {
                        rt = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("GetFine::Error getting finantial parameters", LogLevels.logERROR);
                    }

                    iQTotal = iCustomersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE,  eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                    dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                    if (dPercFEETopped > 0 && dQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                    dQFEE += dFixedFEE;
                    iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));
                    
                    iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                    iQSubTotal = iQ + iQFEE;
                    //}


                    //dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE,

                    parametersOut["q"] = iQ;
                    parametersOut["layout"] = oInstallation.INS_FEE_LAYOUT;
                    parametersOut["q_fee_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture);
                    parametersOut["q_vat_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture);
                    parametersOut["q_subtotalLbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_SUBTOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_total_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_fee"] = iQFEE;
                    parametersOut["q_vat"] = iQVAT;
                    parametersOut["q_vat_percent"] = dVAT2;
                    parametersOut["q_partial_vat1"] = iPartialVAT1;
                    parametersOut["q_subtotal"] = iQSubTotal;
                    parametersOut["q_total"] = iQTotal;
                    parametersOut["dPercVAT1"] = dVAT1;
                    parametersOut["dPercVAT2"] = dVAT2;
                    parametersOut["dPercFEE"] = dPercFEE;
                    parametersOut["iPercFEETopped"] = dPercFEETopped;
                    parametersOut["iPartialPercFEE"] = iPartialPercFEE;
                    parametersOut["iFixedFEE"] = Convert.ToInt32(dFixedFEE);
                    parametersOut["iPartialFixedFEE"] = iPartialFixedFEE;
                    parametersOut["eTaxMode"] = (int)eTaxMode;

                    /*
                    if (oUser != null)
                    {
                        int? iMaxAmountAllowedToPay = MaxAmountAllowedToPay(ref oUser);
                    
                        if (oInstallation.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                        {
                            double dChangeFee = 0;

                            dChangeToApply = GetChangeToApplyFromInstallationCurToUserCur(oInstallation, oUser);
                            if (dChangeToApply < 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                return xmlOut;
                            }

                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            parametersOut["chng"] = dChangeToApply.ToString(numberFormatProvider);

                            int iQChange = ChangeQuantityFromInstallationCurToUserCur(iQ, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);

                            parametersOut["qch"] = iQChange.ToString();

                            iQFEEChange = ChangeQuantityFromInstallationCurToUserCur(iQFEE, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);
                            iQSubTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQSubTotal, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);
                            iQTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQTotal, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);

                            parametersOut["qch_fee"] = iQFEEChange;
                            parametersOut["qch_subtotal"] = iQSubTotalChange;
                            parametersOut["qch_total"] = iQTotalChange;
                            if (iMaxAmountAllowedToPay.HasValue)
                            {
                                if (iMaxAmountAllowedToPay < iQTotalChange)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Not_Enough_Balance);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                        }
                        else
                        {
                            if (iMaxAmountAllowedToPay.HasValue)
                            {
                                if (iMaxAmountAllowedToPay < iQTotal)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Not_Enough_Balance);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                        }

                    }
                    */
                    DateTime? dtUTCDateTime = iGeograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtinstDateTime.Value);

                    decimal? dAuthId = null;
                    if (parametersOut["AuthId"] != null)
                    {
                        try
                        {
                            decimal dTryAuthId = Convert.ToDecimal(parametersOut["AuthId"].ToString());
                                                    dAuthId = dTryAuthId;
                        }
                        catch
                        {
                            dAuthId = null;
                        }
                    }

                    string sExtGrpId = "";
                    decimal? dGrpId = null;
                    if (parametersOut["ExtGrpId"] != null) sExtGrpId = parametersOut["ExtGrpId"].ToString();
                    if (!string.IsNullOrWhiteSpace(sExtGrpId))
                    {
                        GROUP oGroup = null;
                        DateTime? dtGroupDateTime = null;
                        if (iGeograficAndTariffsRepository.getGroupByExtOpsId(sExtGrpId, ref oGroup, ref dtGroupDateTime))
                        {
                            dGrpId = oGroup.GRP_ID;
                            parametersOut["GrpId"] = dGrpId.Value;
                        }
                    }
                }

                parametersOut["utc_offset"] = iGeograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oInstallation.INS_ID);

                xmlOut = GenerateXMLOuput(parametersOut);

                if (xmlOut.Length == 0)
                {
                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                }
                else
                {
                    Logger_AddLogMessage(string.Format("GetFine: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                }

                oUser = null;

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
            }

            return xmlOut;

        }


        #region my new chnages in methods


        [WebMethod]
        public XmlReturnModel GetTicketFineById(string sTicketNumber, long? TicketId, int iInstallation, IFineRepository iFineRepository, ICustomersRepository iCustomersRepository, IInfraestructureRepository iInfraestructureRepository, IGeograficAndTariffsRepository iGeograficAndTariffsRepository, out decimal? InstallationIdFound, integraMobileDBEntitiesDataContext dbContext = null, string LicensePlate = "", string InstallationList = "", string StandardInstallationList = "")
        {
            string xmlOut = "";
            InstallationIdFound = null;
            TicketReturnModel searchedTicket = null;
            List<TicketReturnModel> AllTickets = null;
            sTicketNumber = sTicketNumber.Trim();
            if (!string.IsNullOrEmpty(LicensePlate))
            {
                LicensePlate = LicensePlate.Trim();
            }

            try
            {
                SortedList parametersOut = null;
                String SessionID = new Guid().ToString(); // For AddSessionTicketPaymentInfo 
                ResultType rt = ResultType.Result_OK;
                Decimal? dInstallationId = null;

                Logger_AddLogMessage(string.Format("GetFine: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logINFO);

                try
                {
                    Decimal dTryInstallationId = Convert.ToDecimal(iInstallation.ToString());
                    dInstallationId = dTryInstallationId;
                }
                catch
                {
                    dInstallationId = null;
                }

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                string strFineNumber = sTicketNumber;
                strFineNumber = strFineNumber.Trim();

                parametersOut = new SortedList();
                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                string strCulture = "";

                ThirdPartyFine oThirdPartyFine = new ThirdPartyFine();
                USER oUser = null;

                if (!(!string.IsNullOrEmpty(InstallationList) && !string.IsNullOrEmpty(LicensePlate) && !string.IsNullOrEmpty(sTicketNumber)))
                {
                    if (!iGeograficAndTariffsRepository.getInstallationById(dInstallationId,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }
                    else
                    {
                        InstallationIdFound = dInstallationId;
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }


                    // MICHEL DEV
                    //oInstallation.INS_FINE_WS_SIGNATURE_TYPE = (int)FineWSSignatureType.fst_test;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);

                    switch ((FineWSSignatureType)oInstallation.INS_FINE_WS_SIGNATURE_TYPE)
                    {
                        case FineWSSignatureType.fst_gtechna:
                            {
                                rt = oThirdPartyFine.GtechnaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    //return xmlOut;
                                    return new XmlReturnModel { XML = xmlOut };

                                }
                            }
                            break;
                        case FineWSSignatureType.fst_standard:
                            {
                                rt = oThirdPartyFine.StandardQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    //return xmlOut;
                                    return new XmlReturnModel { XML = xmlOut };

                                }
                            }
                            break;
                        case FineWSSignatureType.fst_eysa:
                            {
                                string strCulturePrefix = strCulture.ToLower().Substring(0, 2);

                                rt = oThirdPartyFine.EysaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, strCulturePrefix, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    //return xmlOut;
                                    return new XmlReturnModel { XML = xmlOut };

                                }
                                else
                                {
                                    if (parametersOut.ContainsKey("sector"))
                                    {
                                        CultureInfo ci = new CultureInfo(strCulture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;


                                        parametersOut["zone"] = ResourceExtension.GetLiteral("Zone_" + parametersOut["sector"].ToString());
                                        parametersOut["sector"] = ResourceExtension.GetLiteral("Sector_" + parametersOut["sector"].ToString());
                                    }
                                    else
                                    {
                                        parametersOut["zone"] = "----";
                                        parametersOut["sector"] = "----";
                                    }
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_madidplatform:
                            {
                                rt = oThirdPartyFine.MadridPlatformQueryFinePaymentQuantity(strFineNumber, null, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_valoriza:
                            {
                                rt = oThirdPartyFine.ValorizaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_internal:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        case FineWSSignatureType.fst_test:
                            switch (sTicketNumber.ToString().Substring(sTicketNumber.ToString().Length - 1, 1))
                            {
                                case "0":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 10000;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "1":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 200;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "2":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 300;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "3":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 400;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "4":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 500;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "5":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 600;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "6":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Not_Found;
                                    break;
                                case "7":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Type_Not_Payable;
                                    break;
                                case "8":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Payment_Period_Expired;
                                    break;
                                case "9":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Already_Paid;
                                    break;
                                default:
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 100;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0));
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0));
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;

                            }
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        default:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;

                    }
                }
                else
                {
                    string sCityID = string.Empty;

                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(StandardInstallationList.Split(',')[0].ToString(),
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return new XmlReturnModel { XML = xmlOut };
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                    string WSUrl = oInstallation.INS_FINE_WS_URL;
                    string WSAuthHashKey = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                    string WSHttpUser = oInstallation.INS_FINE_WS_HTTP_USER;
                    string WSHttpPassword = oInstallation.INS_FINE_WS_HTTP_PASSWORD;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);


                    rt = oThirdPartyFine.StandardQueryTicketFinePaymentQuantityMultiInstallations(strFineNumber, TicketId, LicensePlate, dtinstDateTime.Value, oUser, StandardInstallationList.Split(',').ToList<string>(),
                                                                                            WSUrl, WSAuthHashKey, WSHttpUser, WSHttpPassword, oInstallation.CURRENCy.CUR_ISO_CODE, iWSTimeout,
                                                                                            ref parametersOut, out sCityID);
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();

                    Logger_AddLogMessage(string.Format("GetFine:: 1 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    if (rt != ResultType.Result_OK)
                    {
                        xmlOut = GenerateXMLOuput(parametersOut);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = oInstallation.INS_ID;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }
                    else if (parametersOut["lp"].ToString().Trim() != LicensePlate.Trim())
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Fine_Number_Not_Found);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 2 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(sCityID,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 3 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    InstallationIdFound = oInstallation.INS_ID;
                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;

                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                }


                //double dChangeToApply = 1.0;

                if (rt == ResultType.Result_OK)
                {
                    int iQ = 0;
                    int iQFEE = 0;
                    decimal dQFEE = 0;
                  //  int iQFEEChange = 0;
                    int iQVAT = 0;
                    int iQTotal = 0;
                    //int iQTotalChange = 0;
                    int iQSubTotal = 0;
                    //int iQSubTotalChange = 0;

                    decimal dVAT1 = 0;
                    decimal dVAT2 = 0;
                    int iPartialVAT1;
                    decimal dPercFEE = 0;
                    decimal dPercFEETopped = 0;
                    int iPartialPercFEE;
                    decimal dFixedFEE = 0;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;

                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                    // Moved this line outside the if because amount must be calculated always 
                    iQ = Convert.ToInt32(parametersOut["q"].ToString());

                    //if (oUser != null)
                    //{ 
                    //if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                    //{
                    iPaymentTypeId = 1; // Credit/Debit Card // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                    iPaymentSubtypeId = -1; // UNDEFINED // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                    //}

                    if (!iCustomersRepository.GetFinantialParams(oInstallation.INS_ID, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.TicketPayment, null,
                        out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                    {
                        rt = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("GetFine::Error getting finantial parameters", LogLevels.logERROR);
                    }

                    iQTotal = iCustomersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                    dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                    if (dPercFEETopped > 0 && dQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                    dQFEE += dFixedFEE;
                    iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));

                    iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                    iQSubTotal = iQ + iQFEE;
                    //}


                    //dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE,
                    searchedTicket = (TicketReturnModel)parametersOut["SearchedTicket"];
                    AllTickets = (List<TicketReturnModel>)parametersOut["AllTickets"];
                    parametersOut["q"] = iQ;
                    parametersOut["layout"] = oInstallation.INS_FEE_LAYOUT;
                    parametersOut["q_fee_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture);
                    parametersOut["q_vat_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture);
                    parametersOut["q_subtotalLbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_SUBTOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_total_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_fee"] = iQFEE;
                    parametersOut["q_vat"] = iQVAT;
                    parametersOut["q_vat_percent"] = dVAT2;
                    parametersOut["q_partial_vat1"] = iPartialVAT1;
                    parametersOut["q_subtotal"] = iQSubTotal;
                    parametersOut["q_total"] = iQTotal;
                    parametersOut["dPercVAT1"] = dVAT1;
                    parametersOut["dPercVAT2"] = dVAT2;
                    parametersOut["dPercFEE"] = dPercFEE;
                    parametersOut["iPercFEETopped"] = dPercFEETopped;
                    parametersOut["iPartialPercFEE"] = iPartialPercFEE;
                    parametersOut["iFixedFEE"] = Convert.ToInt32(dFixedFEE);
                    parametersOut["iPartialFixedFEE"] = iPartialFixedFEE;
                    parametersOut["eTaxMode"] = (int)eTaxMode;

                    /*
                    if (oUser != null)
                    {
                        int? iMaxAmountAllowedToPay = MaxAmountAllowedToPay(ref oUser);
                    
                        if (oInstallation.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                        {
                            double dChangeFee = 0;

                            dChangeToApply = GetChangeToApplyFromInstallationCurToUserCur(oInstallation, oUser);
                            if (dChangeToApply < 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                return xmlOut;
                            }

                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            parametersOut["chng"] = dChangeToApply.ToString(numberFormatProvider);

                            int iQChange = ChangeQuantityFromInstallationCurToUserCur(iQ, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);

                            parametersOut["qch"] = iQChange.ToString();

                            iQFEEChange = ChangeQuantityFromInstallationCurToUserCur(iQFEE, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);
                            iQSubTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQSubTotal, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);
                            iQTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQTotal, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);

                            parametersOut["qch_fee"] = iQFEEChange;
                            parametersOut["qch_subtotal"] = iQSubTotalChange;
                            parametersOut["qch_total"] = iQTotalChange;
                            if (iMaxAmountAllowedToPay.HasValue)
                            {
                                if (iMaxAmountAllowedToPay < iQTotalChange)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Not_Enough_Balance);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                        }
                        else
                        {
                            if (iMaxAmountAllowedToPay.HasValue)
                            {
                                if (iMaxAmountAllowedToPay < iQTotal)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Not_Enough_Balance);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                        }

                    }
                    */
                    DateTime? dtUTCDateTime = iGeograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtinstDateTime.Value);

                    decimal? dAuthId = null;
                    if (parametersOut["AuthId"] != null)
                    {
                        try
                        {
                            decimal dTryAuthId = Convert.ToDecimal(parametersOut["AuthId"].ToString());
                            dAuthId = dTryAuthId;
                        }
                        catch
                        {
                            dAuthId = null;
                        }
                    }

                    string sExtGrpId = "";
                    decimal? dGrpId = null;
                    if (parametersOut["ExtGrpId"] != null) sExtGrpId = parametersOut["ExtGrpId"].ToString();
                    if (!string.IsNullOrWhiteSpace(sExtGrpId))
                    {
                        GROUP oGroup = null;
                        DateTime? dtGroupDateTime = null;
                        if (iGeograficAndTariffsRepository.getGroupByExtOpsId(sExtGrpId, ref oGroup, ref dtGroupDateTime))
                        {
                            dGrpId = oGroup.GRP_ID;
                            parametersOut["GrpId"] = dGrpId.Value;
                        }
                    }
                }

                parametersOut["utc_offset"] = iGeograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oInstallation.INS_ID);

                xmlOut = GenerateXMLOuput(parametersOut);

                //if (xmlOut.Length == 0)
                //{
                //    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                //    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                //}
                //else
                //{
                //    Logger_AddLogMessage(string.Format("GetFine: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                //}

                oUser = null;

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
            }

            //return xmlOut;
            return new XmlReturnModel
            {
                XML = xmlOut,
                SearchedTicket = searchedTicket,
                AllTickets = AllTickets
            };


        }




        [WebMethod]
        public XmlReturnModel GetTicketFine(string sTicketNumber, long? Ticketid, int iInstallation, IAppealRepository appealRepository, IFineRepository iFineRepository, ICustomersRepository iCustomersRepository, IInfraestructureRepository iInfraestructureRepository, IGeograficAndTariffsRepository iGeograficAndTariffsRepository, out decimal? InstallationIdFound, integraMobileDBEntitiesDataContext dbContext = null, string LicensePlate = "", string InstallationList = "", string StandardInstallationList = "")
        {
            string xmlOut = "";
            InstallationIdFound = null;
            TicketReturnModel searchedTicket = null;
            List<TicketReturnModel> AllTickets = null; 
            if(Ticketid == null)
            {
                sTicketNumber = sTicketNumber.Trim();
            }
            if (!string.IsNullOrEmpty(LicensePlate))
            {
                LicensePlate = LicensePlate.Trim();
            }

            try
            {
                SortedList parametersOut = null;
                String SessionID = new Guid().ToString(); // For AddSessionTicketPaymentInfo 
                ResultType rt = ResultType.Result_OK;
                Decimal? dInstallationId = null;

                Logger_AddLogMessage(string.Format("GetFine: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logINFO);

                try
                {
                    Decimal dTryInstallationId = Convert.ToDecimal(iInstallation.ToString());
                    dInstallationId = dTryInstallationId;
                }
                catch
                {
                    dInstallationId = null;
                }

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;
                string strFineNumber = "";
                if (Ticketid == null)
                {
                    strFineNumber = sTicketNumber;
                    strFineNumber = strFineNumber.Trim();
                }

                parametersOut = new SortedList();
                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                string strCulture = "";

                ThirdPartyFine oThirdPartyFine = new ThirdPartyFine();
                USER oUser = null;

                if (!(!string.IsNullOrEmpty(InstallationList) && !string.IsNullOrEmpty(LicensePlate) && !string.IsNullOrEmpty(sTicketNumber)) && Ticketid == null)
                {
                    if (!iGeograficAndTariffsRepository.getInstallationById(dInstallationId,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }
                    else
                    {
                        InstallationIdFound = dInstallationId;
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }


                    // MICHEL DEV
                    //oInstallation.INS_FINE_WS_SIGNATURE_TYPE = (int)FineWSSignatureType.fst_test;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);

                    switch ((FineWSSignatureType)oInstallation.INS_FINE_WS_SIGNATURE_TYPE)
                    {
                        case FineWSSignatureType.fst_gtechna:
                            {
                                rt = oThirdPartyFine.GtechnaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    //return xmlOut;
                                    return new XmlReturnModel { XML = xmlOut };

                                }
                            }
                            break;
                        case FineWSSignatureType.fst_standard:
                            {
                                rt = oThirdPartyFine.StandardQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    //return xmlOut;
                                    return new XmlReturnModel { XML = xmlOut };

                                }
                            }
                            break;
                        case FineWSSignatureType.fst_eysa:
                            {
                                string strCulturePrefix = strCulture.ToLower().Substring(0, 2);

                                rt = oThirdPartyFine.EysaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, strCulturePrefix, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    //return xmlOut;
                                    return new XmlReturnModel { XML = xmlOut };

                                }
                                else
                                {
                                    if (parametersOut.ContainsKey("sector"))
                                    {
                                        CultureInfo ci = new CultureInfo(strCulture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;


                                        parametersOut["zone"] = ResourceExtension.GetLiteral("Zone_" + parametersOut["sector"].ToString());
                                        parametersOut["sector"] = ResourceExtension.GetLiteral("Sector_" + parametersOut["sector"].ToString());
                                    }
                                    else
                                    {
                                        parametersOut["zone"] = "----";
                                        parametersOut["sector"] = "----";
                                    }
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_madidplatform:
                            {
                                rt = oThirdPartyFine.MadridPlatformQueryFinePaymentQuantity(strFineNumber, null, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_valoriza:
                            {
                                rt = oThirdPartyFine.ValorizaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_internal:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        case FineWSSignatureType.fst_test:
                            switch (sTicketNumber.ToString().Substring(sTicketNumber.ToString().Length - 1, 1))
                            {
                                case "0":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 10000;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "1":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 200;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "2":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 300;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "3":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 400;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "4":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 500;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "5":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 600;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "6":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Not_Found;
                                    break;
                                case "7":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Type_Not_Payable;
                                    break;
                                case "8":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Payment_Period_Expired;
                                    break;
                                case "9":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Already_Paid;
                                    break;
                                default:
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 100;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0));
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0));
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;

                            }
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        default:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;

                    }
                }
                else
                {
                    string sCityID = string.Empty;

                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(StandardInstallationList.Split(',')[0].ToString(),
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return new XmlReturnModel { XML = xmlOut };
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                    string WSUrl = oInstallation.INS_FINE_WS_URL;
                    string WSAuthHashKey = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                    string WSHttpUser = oInstallation.INS_FINE_WS_HTTP_USER;
                    string WSHttpPassword = oInstallation.INS_FINE_WS_HTTP_PASSWORD;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);


                    rt = oThirdPartyFine.StandardQueryTicketFinePaymentQuantityMultiInstallations(strFineNumber, Ticketid, LicensePlate, dtinstDateTime.Value, oUser, StandardInstallationList.Split(',').ToList<string>(),
                                                                                            WSUrl, WSAuthHashKey, WSHttpUser, WSHttpPassword, oInstallation.CURRENCy.CUR_ISO_CODE, iWSTimeout,
                                                                                            ref parametersOut, out sCityID);
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();

                    Logger_AddLogMessage(string.Format("GetFine:: 1 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);

                    if(LicensePlate == null)
                    {
                        LicensePlate = Convert.ToString(parametersOut["lp"]);
                    }
                    if (rt != ResultType.Result_OK)
                    {
                        xmlOut = GenerateXMLOuput(parametersOut);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = oInstallation.INS_ID;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }
                    else if (parametersOut["lp"].ToString().Trim() != LicensePlate.Trim())
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Fine_Number_Not_Found);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 2 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(sCityID,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        //return xmlOut;
                        return new XmlReturnModel { XML = xmlOut };

                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 3 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    InstallationIdFound = oInstallation.INS_ID;
                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;

                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                }


                //double dChangeToApply = 1.0;

                if (rt == ResultType.Result_OK)
                {
                    int iQ = 0;
                    int iQFEE = 0;
                    decimal dQFEE = 0;
                    //int iQFEEChange = 0;
                    int iQVAT = 0;
                    int iQTotal = 0;
                    //int iQTotalChange = 0;
                    int iQSubTotal = 0;
                    //int iQSubTotalChange = 0;

                    decimal dVAT1 = 0;
                    decimal dVAT2 = 0;
                    int iPartialVAT1;
                    decimal dPercFEE = 0;
                    decimal dPercFEETopped = 0;
                    int iPartialPercFEE;
                    decimal dFixedFEE = 0;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;

                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                    // Moved this line outside the if because amount must be calculated always 
                    iQ = Convert.ToInt32(parametersOut["q"].ToString());

                    //if (oUser != null)
                    //{ 
                    //if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                    //{
                    iPaymentTypeId = 1; // Credit/Debit Card // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                    iPaymentSubtypeId = -1; // UNDEFINED // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                    //}

                    if (!iCustomersRepository.GetFinantialParams(oInstallation.INS_ID, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.TicketPayment, null,
                        out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                    {
                        rt = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("GetFine::Error getting finantial parameters", LogLevels.logERROR);
                    }

                    iQTotal = iCustomersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                    dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                    if (dPercFEETopped > 0 && dQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                    dQFEE += dFixedFEE;
                    iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));

                    iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                    iQSubTotal = iQ + iQFEE;
                    //}


                    //dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE,
                    if(Ticketid != null)
                    {
                        searchedTicket = (TicketReturnModel)parametersOut["Ticket"];
                    }
                    else
                    {
                        searchedTicket = (TicketReturnModel)parametersOut["SearchedTicket"];
                        AllTickets = (List<TicketReturnModel>)parametersOut["AllTickets"];
                    }
                    parametersOut["q"] = iQ;
                    parametersOut["layout"] = oInstallation.INS_FEE_LAYOUT;
                    parametersOut["q_fee_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture);
                    parametersOut["q_vat_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture);
                    parametersOut["q_subtotalLbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_SUBTOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_total_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_fee"] = iQFEE;
                    parametersOut["q_vat"] = iQVAT;
                    parametersOut["q_vat_percent"] = dVAT2;
                    parametersOut["q_partial_vat1"] = iPartialVAT1;
                    parametersOut["q_subtotal"] = iQSubTotal;
                    parametersOut["q_total"] = iQTotal;
                    parametersOut["dPercVAT1"] = dVAT1;
                    parametersOut["dPercVAT2"] = dVAT2;
                    parametersOut["dPercFEE"] = dPercFEE;
                    parametersOut["iPercFEETopped"] = dPercFEETopped;
                    parametersOut["iPartialPercFEE"] = iPartialPercFEE;
                    parametersOut["iFixedFEE"] = Convert.ToInt32(dFixedFEE);
                    parametersOut["iPartialFixedFEE"] = iPartialFixedFEE;
                    parametersOut["eTaxMode"] = (int)eTaxMode;

                    /*
                    if (oUser != null)
                    {
                        int? iMaxAmountAllowedToPay = MaxAmountAllowedToPay(ref oUser);
                    
                        if (oInstallation.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                        {
                            double dChangeFee = 0;

                            dChangeToApply = GetChangeToApplyFromInstallationCurToUserCur(oInstallation, oUser);
                            if (dChangeToApply < 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                return xmlOut;
                            }

                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            parametersOut["chng"] = dChangeToApply.ToString(numberFormatProvider);

                            int iQChange = ChangeQuantityFromInstallationCurToUserCur(iQ, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);

                            parametersOut["qch"] = iQChange.ToString();

                            iQFEEChange = ChangeQuantityFromInstallationCurToUserCur(iQFEE, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);
                            iQSubTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQSubTotal, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);
                            iQTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQTotal, dChangeToApply, oInstallation, oUser, iInfraestructureRepository, out dChangeFee);

                            parametersOut["qch_fee"] = iQFEEChange;
                            parametersOut["qch_subtotal"] = iQSubTotalChange;
                            parametersOut["qch_total"] = iQTotalChange;
                            if (iMaxAmountAllowedToPay.HasValue)
                            {
                                if (iMaxAmountAllowedToPay < iQTotalChange)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Not_Enough_Balance);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                        }
                        else
                        {
                            if (iMaxAmountAllowedToPay.HasValue)
                            {
                                if (iMaxAmountAllowedToPay < iQTotal)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Not_Enough_Balance);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                        }

                    }
                    */
                    DateTime? dtUTCDateTime = iGeograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtinstDateTime.Value);

                    decimal? dAuthId = null;
                    if (parametersOut["AuthId"] != null)
                    {
                        try
                        {
                            decimal dTryAuthId = Convert.ToDecimal(parametersOut["AuthId"].ToString());
                            dAuthId = dTryAuthId;
                        }
                        catch
                        {
                            dAuthId = null;
                        }
                    }

                    string sExtGrpId = "";
                    decimal? dGrpId = null;
                    if (parametersOut["ExtGrpId"] != null) sExtGrpId = parametersOut["ExtGrpId"].ToString();
                    if (!string.IsNullOrWhiteSpace(sExtGrpId))
                    {
                        GROUP oGroup = null;
                        DateTime? dtGroupDateTime = null;
                        if (iGeograficAndTariffsRepository.getGroupByExtOpsId(sExtGrpId, ref oGroup, ref dtGroupDateTime))
                        {
                            dGrpId = oGroup.GRP_ID;
                            parametersOut["GrpId"] = dGrpId.Value;
                        }
                    }
                }

                parametersOut["utc_offset"] = iGeograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oInstallation.INS_ID);

                xmlOut = GenerateXMLOuput(parametersOut);

                //if (xmlOut.Length == 0)
                //{
                //    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                //    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                //}
                //else
                //{
                //    Logger_AddLogMessage(string.Format("GetFine: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                //}

                oUser = null;

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
            }

            //return xmlOut;
            return new XmlReturnModel
            {
                XML = xmlOut,
                SearchedTicket = searchedTicket,
                AllTickets = AllTickets
            };


        }




        [WebMethod]
        public AppealResponse SaveAppeal(Models.AppealReturnModel model , string sTicketNumber, int iInstallation,IAppealRepository appealRepository, IFineRepository iFineRepository, ICustomersRepository iCustomersRepository, IInfraestructureRepository iInfraestructureRepository, IGeograficAndTariffsRepository iGeograficAndTariffsRepository, out decimal? InstallationIdFound, integraMobileDBEntitiesDataContext dbContext = null, string LicensePlate = "", string InstallationList = "", string StandardInstallationList = "")
        {
            AppealResponse response = new AppealResponse();
            string jsonData = JsonConvert.SerializeObject(model);
            string xmlOut = "";
            InstallationIdFound = null;
            sTicketNumber = sTicketNumber.Trim();
            if (!string.IsNullOrEmpty(LicensePlate))
            {
                LicensePlate = LicensePlate.Trim();
            }

            try
            {
                SortedList parametersOut = null;
                String SessionID = new Guid().ToString(); // For AddSessionTicketPaymentInfo 
                ResultType rt = ResultType.Result_OK;
                Decimal? dInstallationId = null;

                Logger_AddLogMessage(string.Format("GetFine: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logINFO);

                try
                {
                    Decimal dTryInstallationId = Convert.ToDecimal(iInstallation.ToString());
                    dInstallationId = dTryInstallationId;
                }
                catch
                {
                    dInstallationId = null;
                }

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                string strFineNumber = sTicketNumber;
                strFineNumber = strFineNumber.Trim();

                parametersOut = new SortedList();
                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                string strCulture = "";

                ThirdPartyFine oThirdPartyFine = new ThirdPartyFine();
                USER oUser = null;

                if (!(!string.IsNullOrEmpty(InstallationList) && !string.IsNullOrEmpty(LicensePlate) && !string.IsNullOrEmpty(sTicketNumber)))
                {
                    Tools.QuickLog(String.Format("Entering into IF condition from External Services.cs class : Ticket number = {0}, Plate number = {1}, Installation List = {2}", sTicketNumber, LicensePlate, InstallationList));

                    if (!iGeograficAndTariffsRepository.getInstallationById(dInstallationId,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return new AppealResponse { xmlOut = xmlOut };
                    }
                    else
                    {
                        InstallationIdFound = dInstallationId;
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }


                    // MICHEL DEV
                    //oInstallation.INS_FINE_WS_SIGNATURE_TYPE = (int)FineWSSignatureType.fst_test;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);

                    switch ((FineWSSignatureType)oInstallation.INS_FINE_WS_SIGNATURE_TYPE)
                    {
                        case FineWSSignatureType.fst_gtechna:
                            {
                                rt = oThirdPartyFine.GtechnaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return new AppealResponse { xmlOut = xmlOut };
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_standard:
                            {
                                rt = oThirdPartyFine.StandardQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return new AppealResponse { xmlOut = xmlOut };
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_eysa:
                            {
                                string strCulturePrefix = strCulture.ToLower().Substring(0, 2);

                                rt = oThirdPartyFine.EysaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, strCulturePrefix, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                if (parametersOut.ContainsKey("fnumber") && parametersOut["fnumber"] != null)
                                    strFineNumber = parametersOut["fnumber"].ToString();

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                                    return new AppealResponse { xmlOut = xmlOut };
                                }
                                else
                                {
                                    if (parametersOut.ContainsKey("sector"))
                                    {
                                        CultureInfo ci = new CultureInfo(strCulture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;


                                        parametersOut["zone"] = ResourceExtension.GetLiteral("Zone_" + parametersOut["sector"].ToString());
                                        parametersOut["sector"] = ResourceExtension.GetLiteral("Sector_" + parametersOut["sector"].ToString());
                                    }
                                    else
                                    {
                                        parametersOut["zone"] = "----";
                                        parametersOut["sector"] = "----";
                                    }
                                }
                            }
                            break;
                        case FineWSSignatureType.fst_madidplatform:
                            {
                                rt = oThirdPartyFine.MadridPlatformQueryFinePaymentQuantity(strFineNumber, null, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_valoriza:
                            {
                                rt = oThirdPartyFine.ValorizaQueryFinePaymentQuantity(strFineNumber, dtinstDateTime.Value, oUser, oInstallation, iWSTimeout, ref parametersOut);
                                parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            }
                            break;

                        case FineWSSignatureType.fst_internal:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        case FineWSSignatureType.fst_test:
                            switch (sTicketNumber.ToString().Substring(sTicketNumber.ToString().Length - 1, 1))
                            {
                                case "0":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 10000;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "1":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 200;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "2":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 300;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "3":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 400;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "4":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 500;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "5":
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 600;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0)).ToString("HHmmssddMMyy");
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;
                                case "6":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Not_Found;
                                    break;
                                case "7":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Type_Not_Payable;
                                    break;
                                case "8":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Payment_Period_Expired;
                                    break;
                                case "9":
                                    parametersOut["r"] = (int)ResultType.Result_Error_Fine_Number_Already_Paid;
                                    break;
                                default:
                                    parametersOut["r"] = (int)ResultType.Result_OK;
                                    parametersOut["q"] = 100;
                                    parametersOut["lp"] = "6789HJU"; //oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                    parametersOut["d"] = dtinstDateTime.Value.Subtract(new TimeSpan(0, 30, 0));
                                    parametersOut["df"] = dtinstDateTime.Value.Add(new TimeSpan(2, 30, 0));
                                    parametersOut["ta"] = "0.5.2";
                                    parametersOut["dta"] = "Test Article Description";
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    break;

                            }
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                        default:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;

                    }
                }
                else
                {
                    Logger_AddLogMessage("Entering into Else condition from External Services.cs class", LogLevels.logINFO);
                    Tools.QuickLog(String.Format("Entering into Else condition from External Services.cs class : Ticket number = {0}, Plate number = {1}, Installation List = {2}",sTicketNumber,LicensePlate,InstallationList));

                    string sCityID = string.Empty;

                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(StandardInstallationList.Split(',')[0].ToString(),
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return new AppealResponse { xmlOut = xmlOut};
                    }

                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;


                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ID;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                    string WSUrl = oInstallation.INS_FINE_WS_URL;
                    string WSAuthHashKey = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                    string WSHttpUser = oInstallation.INS_FINE_WS_HTTP_USER;
                    string WSHttpPassword = oInstallation.INS_FINE_WS_HTTP_PASSWORD;

                    int iWSTimeout = iInfraestructureRepository.GetRateWSTimeout(oInstallation.INS_ID);


                    rt = oThirdPartyFine.UploadAppealImages(jsonData, strFineNumber, dtinstDateTime.Value, oUser, StandardInstallationList.Split(',').ToList<string>(),
                                                                                            WSUrl, WSAuthHashKey, WSHttpUser, WSHttpPassword, oInstallation.CURRENCy.CUR_ISO_CODE, iWSTimeout,
                                                                                            ref parametersOut, out sCityID);
                    if(rt == ResultType.Result_OK)
                    {
                        model.AppealData.AppealImageIds = (List<TicketImageReturnModel>)parametersOut["AppealImageIds"];
                        jsonData = JsonConvert.SerializeObject(model);
                        rt = oThirdPartyFine.SaveAppealInformation(jsonData, strFineNumber, dtinstDateTime.Value, oUser, StandardInstallationList.Split(',').ToList<string>(),
                                                                                            WSUrl, WSAuthHashKey, WSHttpUser, WSHttpPassword, oInstallation.CURRENCy.CUR_ISO_CODE, iWSTimeout,
                                                                                            ref parametersOut, out sCityID);

                        if(rt == ResultType.Result_OK)
                        {
                            response.Result = (int?)ResultType.Result_OK;
                        }
                        if(rt == ResultType.Result_Error_Email_Already_Exist)
                        {
                            response.Result = (int)ResultType.Result_Error_Email_Already_Exist;
                            response.Message = "User is already existed";
                        }
                    }
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();

                    Logger_AddLogMessage(string.Format("GetFine:: 1 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    if (rt != ResultType.Result_OK)
                    {
                        xmlOut = GenerateXMLOuput(parametersOut);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = oInstallation.INS_ID;
                        return new AppealResponse { xmlOut = xmlOut, Result=response.Result,Message=response.Message };
                    }
                    else if (Convert.ToString(parametersOut["lp"]).Trim() != LicensePlate.Trim())
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Fine_Number_Not_Found);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return new AppealResponse { xmlOut = xmlOut, Result = response.Result, Message = response.Message };
                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 2 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    if (!iGeograficAndTariffsRepository.getInstallationByStandardIdWebPortal(sCityID,
                                                 ref oInstallation,
                                                 ref dtinstDateTime))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);
                        InstallationIdFound = null;
                        return new AppealResponse { xmlOut = xmlOut, Result = response.Result, Message = response.Message };
                    }

                    Logger_AddLogMessage(string.Format("GetFine:: 3 --> CityId={0}; TicketNumber={1}", sCityID, sTicketNumber), LogLevels.logERROR);


                    InstallationIdFound = oInstallation.INS_ID;
                    parametersOut["cityShortDesc"] = oInstallation.INS_SHORTDESC;

                    if (oInstallation != null)
                    {
                        if (oInstallation.CURRENCy != null)
                        {
                            parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                            parametersOut["cur_minor_unit"] = oInstallation.CURRENCy.CUR_MINOR_UNIT;
                        }
                        strCulture = oInstallation.INS_CULTURE_LANG;
                    }

                }


                //double dChangeToApply = 1.0;

                if (rt == ResultType.Result_OK)
                {
                    int iQ = 0;
                    int iQFEE = 0;
                    decimal dQFEE = 0;
                    //int iQFEEChange = 0;
                    int iQVAT = 0;
                    int iQTotal = 0;
                    //int iQTotalChange = 0;
                    int iQSubTotal = 0;
                    //int iQSubTotalChange = 0;

                    decimal dVAT1 = 0;
                    decimal dVAT2 = 0;
                    int iPartialVAT1;
                    decimal dPercFEE = 0;
                    decimal dPercFEETopped = 0;
                    int iPartialPercFEE;
                    decimal dFixedFEE = 0;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;

                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                    // Moved this line outside the if because amount must be calculated always 
                    iQ = Convert.ToInt32(parametersOut["q"].ToString());

                    //if (oUser != null)
                    //{ 
                    //if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                    //{
                    iPaymentTypeId = 1; // Credit/Debit Card // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                    iPaymentSubtypeId = -1; // UNDEFINED // oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                    //}

                    if (!iCustomersRepository.GetFinantialParams(oInstallation.INS_ID, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.TicketPayment, null,
                        out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                    {
                        rt = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("GetFine::Error getting finantial parameters", LogLevels.logERROR);
                    }

                    iQTotal = iCustomersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                    dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                    if (dPercFEETopped > 0 && dQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                    dQFEE += dFixedFEE;
                    iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));

                    iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                    iQSubTotal = iQ + iQFEE;
                    //}


                    //dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE,

                    parametersOut["q"] = iQ;
                    parametersOut["layout"] = oInstallation.INS_FEE_LAYOUT;
                    parametersOut["q_fee_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture);
                    parametersOut["q_vat_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture);
                    parametersOut["q_subtotalLbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_SUBTOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_total_lbl"] = iInfraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture);
                    parametersOut["q_fee"] = iQFEE;
                    parametersOut["q_vat"] = iQVAT;
                    parametersOut["q_vat_percent"] = dVAT2;
                    parametersOut["q_partial_vat1"] = iPartialVAT1;
                    parametersOut["q_subtotal"] = iQSubTotal;
                    parametersOut["q_total"] = iQTotal;
                    parametersOut["dPercVAT1"] = dVAT1;
                    parametersOut["dPercVAT2"] = dVAT2;
                    parametersOut["dPercFEE"] = dPercFEE;
                    parametersOut["iPercFEETopped"] = dPercFEETopped;
                    parametersOut["iPartialPercFEE"] = iPartialPercFEE;
                    parametersOut["iFixedFEE"] = Convert.ToInt32(dFixedFEE);
                    parametersOut["iPartialFixedFEE"] = iPartialFixedFEE;
                    parametersOut["eTaxMode"] = (int)eTaxMode;

                    
                    DateTime? dtUTCDateTime = iGeograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtinstDateTime.Value);

                    decimal? dAuthId = null;
                    if (parametersOut["AuthId"] != null)
                    {
                        try
                        {
                            decimal dTryAuthId = Convert.ToDecimal(parametersOut["AuthId"].ToString());
                            dAuthId = dTryAuthId;
                        }
                        catch
                        {
                            dAuthId = null;
                        }
                    }

                    string sExtGrpId = "";
                    decimal? dGrpId = null;
                    if (parametersOut["ExtGrpId"] != null) sExtGrpId = parametersOut["ExtGrpId"].ToString();
                    if (!string.IsNullOrWhiteSpace(sExtGrpId))
                    {
                        GROUP oGroup = null;
                        DateTime? dtGroupDateTime = null;
                        if (iGeograficAndTariffsRepository.getGroupByExtOpsId(sExtGrpId, ref oGroup, ref dtGroupDateTime))
                        {
                            dGrpId = oGroup.GRP_ID;
                            parametersOut["GrpId"] = dGrpId.Value;
                        }
                    }
                }

                parametersOut["utc_offset"] = iGeograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oInstallation.INS_ID);

                xmlOut = GenerateXMLOuput(parametersOut);

                if (xmlOut.Length == 0)
                {
                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                    Logger_AddLogMessage(string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
                }
                else
                {
                    Logger_AddLogMessage(string.Format("GetFine: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                }

                oUser = null;

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("GetFine::Error: CityId={0}; TicketNumber={1}", iInstallation.ToString(), sTicketNumber), LogLevels.logERROR);
            }

            return new AppealResponse { xmlOut = xmlOut, Result=response.Result, Message = "Success" }; ;

        }






        #endregion




        static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        static string PrettyXml(string xml)
        {
            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return "\r\n\t" + stringBuilder.ToString().Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + xml + "\r\n";
            }
        }

        private string GenerateXMLErrorResult(ResultType rt)
        {
            string strRes = "";
            try
            {
                Logger_AddLogMessage(string.Format("Error = {0}", rt.ToString()), LogLevels.logERROR);

                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + OUT_SUFIX);
                xmlOutDoc.AppendChild(root);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + OUT_SUFIX);
                XmlElement result = xmlOutDoc.CreateElement("r");
                result.InnerText = ((int)rt).ToString();
                rootNode.AppendChild(result);
                strRes = xmlOutDoc.OuterXml;

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "GenerateXMLErrorResult::Exception", LogLevels.logERROR);

            }
            return strRes;
        }

        private string GenerateXMLOuput(SortedList parametersOut)
        {
            string strRes = "";
            try
            {
                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + OUT_SUFIX);
                xmlOutDoc.AppendChild(root);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + OUT_SUFIX);

                foreach (DictionaryEntry item in parametersOut)
                {
                    try
                    {
                        XmlElement node = xmlOutDoc.CreateElement(item.Key.ToString());
                        node.InnerXml = item.Value != null ? item.Value.ToString().Trim() : "";
                        rootNode.AppendChild(node);
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "GenerateXMLOuput::Exception", LogLevels.logERROR);
                    }
                }

                strRes = xmlOutDoc.OuterXml;

                if (parametersOut["r"] != null)
                {
                    try
                    {
                        int ir = Convert.ToInt32(parametersOut["r"].ToString());
                        ResultType rt = (ResultType)ir;

                        if (ir < 0)
                        {
                            Logger_AddLogMessage(string.Format("Error = {0}", rt.ToString()), LogLevels.logERROR);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "GenerateXMLOuput::Exception", LogLevels.logERROR);

            }
            return strRes;
        }

        private int? MaxAmountAllowedToPay(ref USER oUser)
        {
            int? iAmount = oUser.USR_BALANCE;

            try
            {
                if ((oUser.CUSTOMER_PAYMENT_MEAN != null) &&
                    (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_ENABLED == 1) &&
                     (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_VALID == 1))
                {
                    if (((oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay) &&
                        (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AUTOMATIC_RECHARGE == 1) &&
                        (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AMOUNT_TO_RECHARGE > 0)) ||
                        (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction))
                    {
                        iAmount = null;
                    }
                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "MaxAmountAllowedToPay::Exception", LogLevels.logERROR);
            }

            return iAmount;
        }

        private int ChangeQuantityFromInstallationCurToUserCur(int iQuantity, double dChangeToApply, INSTALLATION oInstallation, USER oUser, IInfraestructureRepository iInfraestructureRepository, out double dChangeFee)
        {
            int iResult = iQuantity;
            dChangeFee = 0;

            try
            {

                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    double dConvertedValue = Convert.ToDouble(iQuantity) * dChangeToApply;
                    dConvertedValue = Math.Round(dConvertedValue, 4);

                    dChangeFee = Convert.ToDouble(iInfraestructureRepository.GetChangeFeePerc()) * dConvertedValue / 100;
                    //iResult = Convert.ToInt32(dConvertedValue - dChangeFee + 0.5);
                    iResult = Convert.ToInt32(Math.Round(dConvertedValue - dChangeFee, MidpointRounding.AwayFromZero));
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }

        private double GetChangeToApplyFromInstallationCurToUserCur(INSTALLATION oInstallation, USER oUser)
        {
            double dResult = 1.0;

            try
            {
                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    dResult = CCurrencyConvertor.GetChangeToApply(oInstallation.CURRENCy.CUR_ISO_CODE,
                                              oUser.CURRENCy.CUR_ISO_CODE);
                    if (dResult < 0)
                    {
                        Logger_AddLogMessage(string.Format("GetChangeToApplyFromInstallationCurToUserCur::Error getting change from {0} to {1} ", oInstallation.CURRENCy.CUR_ISO_CODE, oUser.CURRENCy.CUR_ISO_CODE), LogLevels.logERROR);
                        return ((int)ResultType.Result_Error_Generic);
                    }
                }
            }
            catch (Exception e)
            {
                dResult = -1.0;
                Logger_AddLogException(e, "GetChangeToApplyFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return dResult;
        }

        private string FindTicket(string Installation, String TicketNumber) 
        {
            return "";
        }

        public string ConfirmFinePaymentNonUser(string TicketNumber, int Quantity, 
                                                int TotalQuantity, string Plate, decimal PercFEE, decimal PercVAT1, 
                                                decimal PercVAT2, int PercFEETopped, int FixedFEE, int PartialVAT1, 
                                                int PartialPercFEE, int PartialFixedFEE, int PartialPercFEEVAT, 
                                                int PartialFixedFEEVAT, IsTAXMode eTaxMode, string Email, string strReference,
                                                string strTransactionId, string strCFTransactionId, string strGatewayDate,
                                                string strAuthCode, string strAuthResult, string strCardHash,
                                                string strCardReference, string strCardScheme, string strMaskedCardNumber,
                                                string strPaypal3tPayerId, string strPaypal3tToken,
                                                DateTime? dtExpDate, PaymentMeanCreditCardProviderType providerType, 
                                                decimal InstallationId, IGeograficAndTariffsRepository geo,
                                                decimal CurrencyId, string CurrencyIsoCode, decimal dGatewayConfigId,
                                                IFineRepository fine, int? dGroupId, string sAuthId, int iWSTimeout)
        {
            string xmlOut = "";
            //string strLockDictionaryString = "";

            try
            {
                SortedList parametersOut = null;
                //string strHash = "";
                //string strHashString = "";

                if (String.IsNullOrEmpty(TicketNumber))
                {
                    m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber");
                }
                else
                {
                    int iQuantity = Quantity;

                    string strAppVersion = "";
                    decimal? dInstallationId = InstallationId;
                    decimal? dLatitude = null;
                    decimal? dLongitude = null;

                    //INSTALLATION oInstallation = Installation;
                    //DateTime? dtinstDateTime = null;
                    //decimal? dLatitudeInst = null;
                    //decimal? dLongitudeInst = null;

                    string strFineNumber = TicketNumber;
                    strFineNumber = strFineNumber.Trim();

                    string strPlate = Plate;
                    string strArticleType = "";
                    string strArticleDescription = "";
                    double dChangeToApply = 1.0;
                    decimal? dAuthId = null;

                    DateTime dtSavedInstallationTime = (DateTime)geo.getInstallationDateTime(InstallationId);                    

                    //ChargeOperationsType operationType = ChargeOperationsType.TicketPayment;
                    decimal dPercVAT1 = PercVAT1;
                    decimal dPercVAT2 = PercVAT2;
                    decimal dPercFEE = PercFEE;
                    int iPercFEETopped = PercFEETopped;
                    int iFixedFEE = FixedFEE;
                    int iPartialVAT1 = PartialVAT1;
                    int iPartialPercFEE = PartialPercFEE;
                    int iPartialFixedFEE = PartialFixedFEE;
                    int iPartialPercFEEVAT = PartialPercFEEVAT;
                    int iPartialFixedFEEVAT = PartialFixedFEEVAT;
                    int iTotalQuantity = TotalQuantity;
                    decimal? dGrpId = dGroupId;

                    parametersOut = new SortedList();

                    int iCurrencyChargedQuantity = 0;
                    decimal dTicketPaymentID = -1;
                    string str3rdPartyOpNum = "";
                    decimal? dRechargeId = null;
                    bool bRestoreBalanceInCaseOfRefund = true;
                    int? iBalanceAfterRecharge = null;
                    DateTime? dtUTCInsertionDate = null;

                    iTotalQuantity = TotalQuantity;
                    parametersOut["ConfirmFinePayment"] = 0;

                    ResultType rt = ChargeFinePaymentNonUser(strFineNumber, dChangeToApply, iQuantity, 
                                                            dtSavedInstallationTime, strPlate, strArticleType, 
                                                            strArticleDescription, InstallationId, dLatitude, dLongitude, 
                                                            strAppVersion, dGrpId, dPercVAT1, dPercVAT2, dPercFEE, 
                                                            iPercFEETopped, iFixedFEE, iPartialVAT1, iPartialPercFEE, 
                                                            iPartialFixedFEE, iTotalQuantity, ref parametersOut, 
                                                            out iCurrencyChargedQuantity, out dTicketPaymentID, 
                                                            out dtUTCInsertionDate, out dRechargeId, out iBalanceAfterRecharge, 
                                                            out bRestoreBalanceInCaseOfRefund, strReference, strTransactionId,
                                                            strCFTransactionId, strGatewayDate, strAuthCode, strAuthResult,
                                                            strCardHash, strCardReference, strCardScheme, strMaskedCardNumber,
                                                            strPaypal3tPayerId,strPaypal3tToken,dtExpDate, providerType, 
                                                            CurrencyId, geo, CurrencyIsoCode, dGatewayConfigId, fine, sAuthId);

                    if (rt != ResultType.Result_OK)
                    {
                        xmlOut = GenerateXMLErrorResult(rt);
                        m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber");
                        return xmlOut;
                    }

                    long lEllapsedTime = 0;
                    
                    ThirdPartyFine oThirdPartyFine = new ThirdPartyFine();

                    INSTALLATION oInstallation = null;
                    DateTime? dtinstDateTime = null;
                    geo.getInstallationById(InstallationId,ref oInstallation, ref dtinstDateTime);

                    // MICHEL DEV
                    //oInstallation.INS_FINE_WS_SIGNATURE_TYPE = (int)FineWSSignatureType.fst_test;

                    //*************************************************************************
                    //*****Este switch es de la lógica de IntegraMobileWS. 
                    //*****Se deshabilita Madrid. Solo se implementa Valoriza
                    //*************************************************************************
                    switch ((FineWSSignatureType)oInstallation.INS_FINE_WS_SIGNATURE_TYPE)
                    {
                        //case FineWSSignatureType.fst_madidplatform:
                        //    {
                        //        str3rdPartyOpNum = sAuthId;
                        //    }
                        //    break;
                        case FineWSSignatureType.fst_valoriza:
                            {
                                str3rdPartyOpNum = sAuthId;
                            }
                            break;
                        default:
                            break;

                    }

                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                    {
                        iQuantity += iPartialVAT1;
                    }


                    switch ((FineWSSignatureType)oInstallation.INS_FINE_WS_SIGNATURE_TYPE)
                    {
                        case FineWSSignatureType.fst_gtechna:
                            {
                                rt = oThirdPartyFine.GtechnaConfirmFinePaymentNonUser(strFineNumber, dtSavedInstallationTime, iQuantity, dTicketPaymentID,
                                                                                oInstallation, iWSTimeout, ref parametersOut, out str3rdPartyOpNum, out lEllapsedTime);
                            }
                            break;
                        case FineWSSignatureType.fst_standard:
                            {
                                rt = oThirdPartyFine.StandardConfirmFinePaymentNonUser(strFineNumber, dtSavedInstallationTime, iQuantity, dTicketPaymentID,
                                                                                oInstallation, Email, iWSTimeout, ref parametersOut, out str3rdPartyOpNum, out lEllapsedTime);
                            }
                            break;
                        case FineWSSignatureType.fst_eysa:
                            {
                                rt = oThirdPartyFine.EysaConfirmFinePaymentNonUser(strFineNumber, dtSavedInstallationTime, iQuantity,
                                                                            oInstallation, iWSTimeout, ref parametersOut, out str3rdPartyOpNum, out lEllapsedTime);
                            }
                            break;
                        case FineWSSignatureType.fst_madidplatform:
                            {
                                rt = oThirdPartyFine.MadridPlatformConfirmFinePaymentNonUser(strFineNumber, dtSavedInstallationTime, dtUTCInsertionDate.Value, iQuantity,
                                                                                             oInstallation, dTicketPaymentID, dAuthId ?? 0, iWSTimeout, ref parametersOut, out str3rdPartyOpNum, out lEllapsedTime);                                
                            }
                            break;
                        case FineWSSignatureType.fst_valoriza:
                            {
                                rt = oThirdPartyFine.ValorizaConfirmFinePaymentNonUser(str3rdPartyOpNum, dtSavedInstallationTime, iQuantity, dTicketPaymentID,
                                                                            oInstallation, iWSTimeout, ref parametersOut, out str3rdPartyOpNum, out lEllapsedTime);
                            }
                            break;
                        case FineWSSignatureType.fst_bsm:
                            {
                                rt = oThirdPartyFine.BSMConfirmFinePayment(oInstallation, strFineNumber, dtSavedInstallationTime, iWSTimeout,
                                                                           ref parametersOut, out str3rdPartyOpNum, out lEllapsedTime);
                            }
                            break;
                        case FineWSSignatureType.fst_internal:
                            rt = ResultType.Result_Error_Generic;
                            
                            break;
                        case FineWSSignatureType.fst_test:
                            rt = ResultType.Result_OK;
                            break;
                        default:
                            rt = ResultType.Result_Error_Generic;
                            break;
                    }    

                    parametersOut["r"] = Convert.ToInt32(rt).ToString();
                    parametersOut["ConfirmFinePayment"] = 1;

                    if (str3rdPartyOpNum.Length > 0)
                    {
                        fine.UpdateThirdPartyIDInFinePaymentNonUser(dTicketPaymentID, str3rdPartyOpNum);
                    }

                    parametersOut["utc_offset"] = geo.GetInstallationUTCOffSetInMinutes(oInstallation.INS_ID);

                    xmlOut = GenerateXMLOuput(parametersOut);

                    if (xmlOut.Length == 0)
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                        m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber");
                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("ConfirmFinePayment: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                    }

                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber :" + e.Message);
            }

            return xmlOut;
        }

        private ResultType ChargeFinePaymentNonUser(string strFineNumber,double dChangeToApply, int iQuantity, 
                                                    DateTime dtPaymentDate, string strPlate, string strArticleType, 
                                                    string strArticleDescription, decimal installationId, 
                                                    decimal? dLatitude, decimal? dLongitude, string strAppVersion, 
                                                    decimal? dGrpId, decimal dPercVAT1, decimal dPercVAT2, decimal dPercFEE, 
                                                    int iPercFEETopped, int iFixedFEE, int iPartialVAT1, int iPartialPercFEE, 
                                                    int iPartialFixedFEE, int iTotalQuantity, ref SortedList parametersOut, 
                                                    out int iCurrencyChargedQuantity, out decimal dTicketPaymentID, 
                                                    out DateTime? dtUTCInsertionDate, out decimal? dRechargeId, 
                                                    out int? iBalanceAfterRecharge, out bool bRestoreBalanceInCaseOfRefund, 
                                                    string strReference, string strTransactionId, string strCFTransactionId, 
                                                    string strGatewayDate, string strAuthCode, string strAuthResult, 
                                                    string strCardHash, string strCardReference, string strCardScheme,
                                                    string strMaskedCardNumber, string strPaypal3tPayerId, string strPaypal3tToken, DateTime? dtExpDate,
                                                    PaymentMeanCreditCardProviderType providerType, decimal CurrencyId,
                                                    IGeograficAndTariffsRepository geo, string CurrencyIsoCode, decimal dGatewayConfigId,
                                                    IFineRepository fine, string authId)
        {            

            ResultType rtRes = ResultType.Result_OK;
            iCurrencyChargedQuantity = 0;
            double dChangeFee = 0;
            decimal dBalanceCurID = CurrencyId;
            dTicketPaymentID = -1;
            dRechargeId = null;
            bRestoreBalanceInCaseOfRefund = true;
            PaymentSuscryptionType suscriptionType = PaymentSuscryptionType.pstPerTransaction;
            iBalanceAfterRecharge = null;
            dtUTCInsertionDate = null;

            try
            {
                parametersOut["autorecharged"] = "0";

                iCurrencyChargedQuantity = iTotalQuantity;

                if (iCurrencyChargedQuantity < 0)
                {
                    rtRes = (ResultType)iCurrencyChargedQuantity;
                    Logger_AddLogMessage(string.Format("ChargeFinePayment::Error Changing Quantity {0} ", rtRes.ToString()), LogLevels.logERROR);
                    return rtRes;
                }

                suscriptionType = PaymentSuscryptionType.pstPerTransaction;

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;
                geo.getInstallationById(installationId, ref oInstallation, ref dtinstDateTime);
                                
                DateTime? dtUTCTime = geo.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtPaymentDate);

                if (dtUTCTime == null)
                {
                    dtUTCTime = dtPaymentDate;
                }

                bool bConfirmedWs = ((oInstallation.INS_OPT_FINECONFIRM_MODE ?? 0) == 0);

                if (!fine.ChargeFinePaymentNonUser(true,
                                                    suscriptionType,
                                                    installationId,
                                                    dtPaymentDate,
                                                    dtUTCTime.Value,
                                                    strPlate,
                                                    strFineNumber,
                                                    string.Format("{0} ({1})", strArticleType, strArticleDescription),
                                                    iQuantity,
                                                    CurrencyId,
                                                    dBalanceCurID,
                                                    dChangeToApply,
                                                    dChangeFee,
                                                    iCurrencyChargedQuantity,
                                                    dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                                    dRechargeId,
                                                    bConfirmedWs,
                                                    dLatitude, dLongitude,strAppVersion,
                                                    dGrpId,
                                                    null, 
                                                    null,
                                                    null,
                                                    out dTicketPaymentID,
                                                    out dtUTCInsertionDate,
                                                    strReference, strTransactionId,
                                                    strCFTransactionId, strGatewayDate, strAuthCode, strAuthResult,
                                                    strCardHash, strCardReference, strCardScheme, strMaskedCardNumber,
                                                    strPaypal3tPayerId,
                                                    strPaypal3tToken,
                                                    dtExpDate,
                                                    oInstallation,
                                                    providerType,
                                                    CurrencyIsoCode,
                                                    dGatewayConfigId,
                                                    authId))
                {

                    Logger_AddLogMessage(string.Format("ChargeFinePayment::Error Inserting Ticket Payment {0} ", strFineNumber), LogLevels.logERROR);
                    return ResultType.Result_Error_Generic;
                }              

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChargeFinePayment::Exception", LogLevels.logERROR);
            }


            return rtRes;
        }

   
    }

    public class XmlReturnModel
    {
        public string XML { get; set; }
        public TicketReturnModel SearchedTicket { get; set; }
        public List<TicketReturnModel> AllTickets { get; set; }
    }

}