using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Diagnostics;
using Ninject;
using System.IO;
using System.Net;

namespace integraMobile.ExternalWS
{
    public enum ResultType
    {
        Result_ParkingWarning_HighPollutionEpisode_Level2 = 5,
        Result_ParkingWarning_HighPollutionEpisode_Level1 = 4,
        Result_3DS_Validation_Needed = 3,
        Result_Validate_Email = 2,
        Result_OK = 1,
        Result_Error_InvalidAuthenticationHash = -1,
        Result_Error_ParkingMaximumTimeUsed = -2,
        Result_Error_NotWaitedReentryTime = -3,
        Result_Error_RefundNotPossible = -4,
        Result_Error_Fine_Number_Not_Found = -5,
        Result_Error_Fine_Type_Not_Payable = -6,
        Result_Error_Fine_Payment_Period_Expired = -7,
        Result_Error_Fine_Number_Already_Paid = -8,
        Result_Error_Generic = -9,
        Result_Error_InvalidAuthentication = -11,
        Result_Error_LoginMaximumNumberOfTrialsReached = -12,
        Result_Error_Invalid_First_Name = -13,
        Result_Error_Invalid_Last_Name = -14,
        Result_Error_Invalid_Id = -15,
        Result_Error_Invalid_Country_Code = -16,
        Result_Error_Invalid_Cell_Number = -17,
        Result_Error_Invalid_Email_Address = -18,
        Result_Error_Invalid_Input_Parameter = -19,
        Result_Error_Missing_Input_Parameter = -20,
        Result_Error_Mobile_Phone_Already_Exist = -21,
        Result_Error_Email_Already_Exist = -22,
        Result_Error_Recharge_Failed = -23,
        Result_Error_Recharge_Not_Possible = -24,
        Result_Error_Invalid_City = -25,
        Result_Error_Invalid_User = -26,
        Result_Error_User_Not_Logged = -27,
        Result_Error_Tariffs_Not_Available = -28,
        Result_Error_Invalid_Payment_Mean = -29,
        Result_Error_Invalid_Recharge_Code = -30,
        Result_Error_Expired_Recharge_Code = -31,
        Result_Error_AlreadyUsed_Recharge_Code = -32,
        Result_Error_Not_Enough_Balance = -33,
        Result_Error_ResidentParkingExhausted = -34,
        Result_Error_OperationExpired = -35,
        Result_Error_InvalidTicketId = -36,
        Result_Error_ExpiredTicketId = -37,
        Result_Error_OperationNotFound = -38,
        Result_Error_OperationAlreadyClosed = -39,
        Result_Error_OperationEntryAlreadyExists = -40,
        Result_Error_ConfirmOperationAlreadyExecuting = -41,
        Result_Error_InvalidAppVersion_UpdateMandatory = -42,
        Result_Error_InvalidAppVersion_UpdateNotMandatory = -43,
        Result_Error_Madrid_Council_Platform_Is_Not_Available = -44,
        Result_Error_TransferingBalance = 0,
        Result_Error_InvalidUserReceiverEmail = -45,
        Result_Error_UserReceiverDisabled = -46,
        Result_Error_UserAccountBlocked = -47,
        Result_Error_UserAccountNotAproved = -48,
        Result_Error_UserBalanceNotEnough = -49,
        Result_Error_AmountNotValid = -50,
        Result_Error_UserAmountDailyLimitReached = -51,
        Result_Toll_is_Not_from_That_installation = -52,
        Result_Parking_Not_Allowed= -53,
        Result_Error_Max_Multidiscount_Reached = -54,
        Result_Error_Discount_NotAllowed = -55,
        Result_Error_Invalid_Plate = -56,
        Result_Error_Offstreet_InvoiceGeneration = -57,
        Result_Error_Getting_Transaction_Parameters = -58,
        Result_Error_Duplicate_Recharge = -59,
        Result_Error_Offstreet_OperationInFreePass = -60,
        Result_Error_Invalid_Payment_Gateway = -61,
        Result_Error_User_Has_no_Suscription_Type = -62,
        Result_Error_User_Is_Not_Activated = -63,
        Result_Error_Plate_Is_Assigned_To_Another_User = -64,
        Result_Error_CrossSourceExtensionNotPossible = -65,
        Result_Error_Only_One_Loyalty_Recharge_Code_Can_Be_Used = -66,
        Result_Error_Name_Recharge_Code_Only_Can_Be_Used_By_Addressed_Email = -67,
        Result_Error_RestrictedTariff = -68,
        Result_Error_Tariff_Not_Required = -69,
        Result_Error_PlateInBlackList = -70,
        Result_Error_NoParkingRequired_ZeroEmissions = -71,
        Result_Error_ParkingRefused_PollutionEpisode = -72,        
        Result_Error_PermitNotInRenewalPeriod = -73,        
        Result_Error_PermitAlreadyExist = -74,
        Result_Error_PermitMonthNotAllowed = -75,
        Result_Error_Incompatible_City_With_UserCurrency = -76,
        Result_Error_StartedOperation_Exist = -77,
        Result_Error_StartedOperation_NotExist = -78,
        Result_Error_Park_Min_Time_Not_Exceeded = -79,
        Result_Error_NearConfigurations_Not_Available = -80,
        Result_Error_CloseCampaign = -81,
        Result_Error_CouponCanNotBeUsedByExistingUsers = -82,
        Result_Error_TheCouponHasAlreadyBeenUsed = -83,
        Result_Error_BSM_Not_Answering = -84,
        Result_Error_RestrictedTariffWithSpecificMessage = -85,
        Result_Error_ParkingRefused_WithoutCategory = -86,
        Result_Error_ServiceLicensePlateRejectedState= -87,
        Result_Error_ServiceLicenseStatusPendingApproval = -88,
        Result_Error_NoServicesAssociatedWithCity = -89,
        Result_Error_CouponMaximumDailyUseReached = -90,
        Result_Error_CouponMaximumWeeklyUseReached = -91,
        Result_Error_CouponMaximumMonthlyUseReached = -92,
        Result_Error_CouponMinimumDailyDistintQRsNotReached = -93,
        Result_Error_CouponMinimumWeeklyDistintQRsNotReached = -94,
        Result_Error_CouponMinimumMonthlyDistintQRsNotReached = -95,
        Result_Error_Invalid_Status = -96,
        Result_Error_CollectingAlreadyExists = -97,
        Result_Error_TicketPaymentAlreadyExist = -98,
        Result_Error_ParkingRefused_MaxParkingOpsByUser = -99,
        Result_Error_ParkingRefused_MaxParkingOpsInPeriod = -100,
        Result_Error_CardPayment_Mode_NotApplicable = -101
    }

    public enum ResultTypeStandardParkingWS
    {
        ResultSP_ParkingWarning_HighPollutionEpisode_Level2 = 3,
        ResultSP_ParkingWarning_HighPollutionEpisode_Level1 = 2,
        ResultSP_OK = 1,
        ResultSP_Error_InvalidAuthenticationHash = -1,
        ResultSP_Error_ParkingMaximumTimeUsed = -2,
        ResultSP_Error_NotWaitedReentryTime = -3,
        ResultSP_Error_RefundNotPossible = -4,
        ResultSP_Error_Fine_Number_Not_Found = -5,
        ResultSP_Error_Fine_Type_Not_Payable = -6,
        ResultSP_Error_Fine_Payment_Period_Expired = -7,
        ResultSP_Error_Fine_Number_Already_Paid = -8,
        ResultSP_Error_Generic = -9,
        ResultSP_Error_Invalid_Input_Parameter = -10,
        ResultSP_Error_Missing_Input_Parameter = -11,
        ResultSP_Error_Invalid_City = -12,
        ResultSP_Error_Invalid_Group = -13,
        ResultSP_Error_Invalid_Tariff = -14,
        ResultSP_Error_Tariff_Not_Available = -15,
        ResultSP_Error_InvalidExternalProvider = -16,
        ResultSP_Error_OperationAlreadyExist = -17,
        ResultSP_Error_RestrictedTariff = -19,
        ResultSP_Error_CrossSourceExtensionNotPossible = -24,
        ResultSP_Error_Tariff_Not_Required = -25,
        ResultSP_Error_PlateInBlackList = -26,
        ResultSP_Error_NoParkingRequired_ZeroEmissions = -27,
        ResultSP_Error_ParkingRefused_PollutionEpisode = -28,
        ResultSP_Error_PermitNotInRenewalPeriod = -29,
        ResultSP_Error_PermitAlreadyExist = -30,
        ResultSP_Error_InvalidOperationId = -31,
        ResultSP_Error_PermitMonthNotAllowed = -32,
        ResultSP_Error_RestrictedTariffWithSpecificMessage = -33,
        ResultSP_Error_ParkingRefused_WithoutCategory = -34,
        ResultSP_Error_MaxParkingOpsByUser = -35,
        ResultSP_Error_MaxParkingOpsInPeriod = -36,
    }

    public enum ResultTypeBilbaoIntegrationParkingWS
    {
        ResultSP_OK = 1,
        ResultSP_Error_InvalidAuthenticationHash = -1,
        ResultSP_Error_ParkingMaximumTimeUsed = -2,
        ResultSP_Error_NotWaitedReentryTime = -3,
        ResultSP_Error_Generic = -9,
        ResultSP_Error_Invalid_Input_Parameter = -10,
        ResultSP_Error_Missing_Input_Parameter = -11,
        ResultSP_Error_Invalid_City = -12,
        ResultSP_Error_Invalid_Group = -13,
        ResultSP_Error_Invalid_Tariff = -14,
        ResultSP_Error_Tariff_Not_Available = -15,
        ResultSP_Error_InvalidExternalProvider = -16,
        ResultSP_Error_CrossSourceExtensionNotPossible = -24,
    }


    public enum ResultTypePICWS
    {
        ResultPICWS_OK = 1,
        ResultPICWS_Error_InvalidAuthenticationHash = -1,
        ResultPICWS_Error_Generic = -9,
        ResultPICWS_Error_Invalid_Input_Parameter = -10,
        ResultPICWS_Error_Missing_Input_Parameter = -11,
        ResultPICWS_Error_Invalid_City = -12,
        ResultPICWS_Error_Invalid_Group = -13,
        ResultPICWS_Error_InvalidExternalProvider = -14,
        ResultPICWS_Error_Invalid_Unit = -15,
        ResultPICWS_Error_Invalid_AlarmType = -16,
        ResultPICWS_Error_Invalid_Status = -17,
        ResultPICWS_Error_CollectingAlreadyExists = -18,
        ResultPICWS_Error_TicketPaymentAlreadyExist = -19,
        ResultPICWS_Error_TicketNotPayable = -20,
        ResultPICWS_Error_InvalidUserOrPassword = -21
    }

    public class ThirdPartyOperation :ThirdPartyBase
    {
        private const int DEFAULT_TIME_STEP = 5; //minutes
        private const int DEFAULT_AMOUNT_STEP = 5; //CENTS
        private static ulong _VERSION_1_2_4 = AppUtilities.AppVersion("1.2.4");


        public ThirdPartyOperation() : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyOperation));
        }

        public ResultType EysaConfirmParking(int iWSNumber, string strPlate, DateTime dtUTCInsertionDate, USER oUser, INSTALLATION oInstallation, decimal? dGroupId, decimal? dTariffId, 
                                             int iQuantity, int iTime,DateTime dtIni, DateTime dtEnd, int iQFEE, int iQBonus, int iQFEEVAT, string sBonusMarca, int? iBonusType,  decimal? dLatitude, 
                                             decimal? dLongitude, int? iWSTimeout, ref SortedList parametersOut,out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 


                EysaThirdPartyConfirmParkWS.Ticket oParkWS = new EysaThirdPartyConfirmParkWS.Ticket();
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";

                switch (iWSNumber)
                {
                    case 1:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("EysaConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                EysaThirdPartyConfirmParkWS.ConsolaSoapHeader authentication = new EysaThirdPartyConfirmParkWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oParkWS.ConsolaSoapHeaderValue = authentication;

                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;
                string strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();



                string strMessage = "";
                string strAuthHash = "";


                string strExtTariffId = "";
                string strExtGroupId = "";


                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("EysaConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }


                strAuthHash = CalculateEysaWSHash(strHashKey,
                    string.Format("1{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}{5}{6}{7:yyyy-MM-ddTHH:mm:ss.fff}{8:yyyy-MM-ddTHH:mm:ss.fff}{9}{10}{11}{12}{13}{14}",
                    strCityID, strPlate, dtUTCInsertionDate, strExtGroupId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, iQFEE, iQBonus, iQFEEVAT, iBonusType, sBonusMarca));

                strMessage = string.Format("<ipark_in><u>1</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><tar_id>{4}</tar_id><q>{5}</q><t>{6}</t>" +
                                           "<bd>{7:yyyy-MM-ddTHH:mm:ss.fff}</bd><ed>{8:yyyy-MM-ddTHH:mm:ss.fff}</ed><vers>{9}</vers><ah>{10}</ah><em>{11}</em><qt>{12}</qt><qc>{13}</qc>"+
                                           "<iva>{14}</iva><o>{15}</o><marca>{16}</marca><lt_ticket>{17}</lt_ticket><lg_ticket>{18}</lg_ticket></ipark_in>",
                    strCityID, strPlate, dtUTCInsertionDate, strExtGroupId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, strAuthHash, strCompanyName, iQFEE, iQBonus, 
                    iQFEEVAT, iBonusType, sBonusMarca, 
                    dLatitude.HasValue? dLatitude.Value.ToString(CultureInfo.InvariantCulture): "-999",
                    dLongitude.HasValue? dLongitude.Value.ToString(CultureInfo.InvariantCulture): "-999");

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaConfirmParking Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.rdPConfirmParkingOperation(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaConfirmParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        str3dPartyOpNum = wsParameters["opnum"].ToString();
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch!=null)&&(lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "EysaConfirmParking::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {
            }
            
            return rtRes;

        }

        public ResultType StandardConfirmParking(int iWSNumber, string strPlate, List<string> oAdditionalPlates, DateTime dtParkQuery, USER oUser, INSTALLATION oInstallation, 
                                                 decimal? dGroupId, decimal? dStreetSectionId, decimal? dTariffId, int iQuantity, int iTime,DateTime dtIni, DateTime dtEnd, decimal dOperationId, string strPlaceString, 
                                                 int iPostpay, decimal? dAuthId, decimal? bon_mlt, decimal? bon_ext_mlt, int? iQuantityWithBon, int? iWSTimeout, ref SortedList parametersOut, 
                                                 out string str3dPartyOpNum, out string str3dPartyBaseOpNum, out long lEllapsedTime)                                                          
        {
            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            str3dPartyBaseOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 


                StandardParkingWS.TariffComputerWS oParkWS = new StandardParkingWS.TariffComputerWS();
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";

                switch (iWSNumber)
                {
                    case 1:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("StandardConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }


                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();



                string strMessage = "";
                string strAuthHash = "";


                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("StandardConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sExtStreetSectionId = "";
                if (dStreetSectionId.HasValue)
                {
                    STREET_SECTION oStreetSection = null;
                    if (geograficAndTariffsRepository.GetStreetSection(dStreetSectionId.Value, out oStreetSection))
                    {
                        sExtStreetSectionId = oStreetSection.STRSE_ID_EXT;
                    }

                }                

                string sAuthId = "";
                if (dAuthId.HasValue)
                {
                    sAuthId = string.Format("{0}", dAuthId.Value);
                }

                string sPlatesValues = strPlate;
                string sPlatesTags = string.Format("<lic_pla>{0}</lic_pla>", strPlate);
                if (oAdditionalPlates != null)
                {
                    for (int i = 0; i < oAdditionalPlates.Count; i += 1)
                    {
                        sPlatesValues += oAdditionalPlates[i];
                        sPlatesTags += string.Format("<lic_pla{0}>{1}</lic_pla{0}>", i + 2, oAdditionalPlates[i]);
                    }
                }

                string sBonMlt = string.Empty;
                string sBonExtMlt = string.Empty;
                if (bon_mlt.HasValue)
                {
                    sBonMlt = bon_mlt.Value.ToString(CultureInfo.InvariantCulture);
                }
                
                if (string.IsNullOrEmpty(sBonMlt) && !iQuantityWithBon.HasValue)
                {
                    strAuthHash = CalculateStandardWSHash(strHashKey,
                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8:HHmmssddMMyyyy}{9:HHmmssddMMyyyy}{10}{11}{12}{13}{14}{15}{16}",
                        strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers,
                        dOperationId, strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId));
                }
                else if (iQuantityWithBon.HasValue)
                {

                    strAuthHash = CalculateStandardWSHash(strHashKey,
                                           string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8:HHmmssddMMyyyy}{9:HHmmssddMMyyyy}{10}{11}{12}{13}{14}{15}{16}{17}",
                                           strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers,
                                           dOperationId, strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, iQuantityWithBon.Value));

                }
                else
                {
                    strAuthHash = CalculateStandardWSHash(strHashKey,
                                           string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8:HHmmssddMMyyyy}{9:HHmmssddMMyyyy}{10}{11}{12}{13}{14}{15}{16}{17}",
                                           strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers,
                                           dOperationId, strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, sBonMlt));
                }


                if (dAuthId.HasValue)
                {
                    sAuthId = string.Format("<auth_id>{0}</auth_id>", dAuthId.Value);
                }

                if (string.IsNullOrEmpty(sBonMlt) && !iQuantityWithBon.HasValue)
                {
                    strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<pur_date>{2:HHmmssddMMyyyy}</pur_date>" +
                                               "<grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id>" +
                                               "<amou_payed>{6}</amou_payed><time_payed>{7}</time_payed>" +
                                               "<ini_date>{8:HHmmssddMMyyyy}</ini_date>" +
                                               "<end_date>{9:HHmmssddMMyyyy}</end_date>" +
                                               "<ver>{10}</ver><oper_id>{11}</oper_id><prov>{12}</prov><ext_acc>{13}</ext_acc><space>{14}</space><postpay>{15}</postpay>{16}" +
                                               "<ah>{17}</ah></ipark_in>",
                        strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, dOperationId,
                        strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, strAuthHash);
                }
                else if (iQuantityWithBon.HasValue)
                {

                    strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<pur_date>{2:HHmmssddMMyyyy}</pur_date>" +
                                               "<grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id>" +
                                               "<amou_payed>{6}</amou_payed><time_payed>{7}</time_payed>" +
                                               "<ini_date>{8:HHmmssddMMyyyy}</ini_date>" +
                                               "<end_date>{9:HHmmssddMMyyyy}</end_date>" +
                                               "<ver>{10}</ver><oper_id>{11}</oper_id><prov>{12}</prov><ext_acc>{13}</ext_acc><space>{14}</space><postpay>{15}</postpay>{16}" +
                                               "<amou_with_bon_payed>{17}</amou_with_bon_payed>" +
                                               "<ah>{18}</ah></ipark_in>",
                        strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, dOperationId,
                        strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, iQuantityWithBon.Value, strAuthHash);

                }
                else
                {
                    strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<pur_date>{2:HHmmssddMMyyyy}</pur_date>" +
                                               "<grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id>" +
                                               "<amou_payed>{6}</amou_payed><time_payed>{7}</time_payed>" +
                                               "<ini_date>{8:HHmmssddMMyyyy}</ini_date>" +
                                               "<end_date>{9:HHmmssddMMyyyy}</end_date>" +
                                               "<ver>{10}</ver><oper_id>{11}</oper_id><prov>{12}</prov><ext_acc>{13}</ext_acc><space>{14}</space><postpay>{15}</postpay>{16}" +
                                               "<bon_mlt>{17}</bon_mlt>" +
                                               "<ah>{18}</ah></ipark_in>",
                        strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, dOperationId,
                        strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, sBonMlt, strAuthHash);
                }
                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardConfirmParking Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.InsertExternalParkingOperationInstallationTime(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardConfirmParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        str3dPartyOpNum = wsParameters["oper_id"].ToString();
                        if (wsParameters["base_oper_id"] != null)
                            str3dPartyBaseOpNum = wsParameters["base_oper_id"].ToString();
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "StandardConfirmParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType StandardCancelParking(int iWSNumber, string sOpeExtId, INSTALLATION oInstallation, int? iWSTimeout, 
                                                out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                StandardParkingWS.TariffComputerWS oParkWS = new StandardParkingWS.TariffComputerWS();
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";

                switch (iWSNumber)
                {
                    case 1:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("StandardConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }


                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();



                string strMessage = "";
                string strAuthHash = "";

                
                strAuthHash = CalculateStandardWSHash(strHashKey,
                        string.Format("{0}{1}{2}", strCityID, sOpeExtId, strCompanyName));


                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>" + 
                                               "<ope_id>{1}</ope_id>" +
                                               "<prov>{2}</prov>" +                                               
                                               "<ah>{3}</ah></ipark_in>",
                        strCityID, sOpeExtId, strCompanyName, strAuthHash);
                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardCancelParking Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.CancelParkingOperation(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardCancelParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;                
                Logger_AddLogException(e, "StandardCancelParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType BilbaoIntegrationConfirmParking(int iWSNumber, string strPlate, List<string> oAdditionalPlates, DateTime dtParkQuery, USER oUser, INSTALLATION oInstallation,
                                         decimal? dGroupId, decimal? dTariffId, int iQuantity, int iTime, DateTime dtIni, DateTime dtEnd, decimal dOperationId, string strPlaceString,
                                         int iPostpay, decimal? dAuthId, decimal? bon_mlt, decimal? bon_ext_mlt, int? iWSTimeout, ref SortedList parametersOut,
                                         out string str3dPartyOpNum, out string str3dPartyBaseOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            str3dPartyBaseOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                AddTLS12Support();
                var oParkWS = new BilbaoParkWsIntegraExternalService.integraExternalServices();
                oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                if (!string.IsNullOrEmpty(oInstallation.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new NetworkCredential(oInstallation.INS_PARK_WS_HTTP_USER, oInstallation.INS_PARK_WS_HTTP_PASSWORD);
                }

                string strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                string strMessage = "";
                string strAuthHash = "";
                string strExtTariffId = "";
                string strExtGroupId = "";
                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("BilbaoIntegrationConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sAuthId = "";
                if (dAuthId.HasValue)
                {
                    sAuthId = string.Format("{0}", dAuthId.Value);
                }

                string sPlatesValues = strPlate;
                string sPlatesTags = string.Format("<lic_pla>{0}</lic_pla>", strPlate);
                if (oAdditionalPlates != null)
                {
                    for (int i = 0; i < oAdditionalPlates.Count; i += 1)
                    {
                        sPlatesValues += oAdditionalPlates[i];
                        sPlatesTags += string.Format("<lic_pla{0}>{1}</lic_pla{0}>", i + 2, oAdditionalPlates[i]);
                    }
                }

                string sBonMlt = string.Empty;
                string sBonExtMlt = string.Empty;
                if (bon_mlt.HasValue)
                {
                    sBonMlt = bon_mlt.Value.ToString(CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(sBonMlt))
                {
                    strAuthHash = CalculateStandardWSHash(strHashKey,
                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7:HHmmssddMMyyyy}{8:HHmmssddMMyyyy}{9}{10}{11}{12}{13}{14}{15}",
                        strCityID, sPlatesValues, dtInstallation, strExtGroupId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers,
                        dOperationId, strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId));
                }
                else
                {
                    strAuthHash = CalculateStandardWSHash(strHashKey,
                                           string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7:HHmmssddMMyyyy}{8:HHmmssddMMyyyy}{9}{10}{11}{12}{13}{14}{15}{16}",
                                           strCityID, sPlatesValues, dtInstallation, strExtGroupId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers,
                                           dOperationId, strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, sBonMlt));
                }


                if (dAuthId.HasValue)
                {
                    sAuthId = string.Format("<auth_id>{0}</auth_id>", dAuthId.Value);
                }
                if (string.IsNullOrEmpty(sBonMlt))
                {
                    strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<pur_date>{2:HHmmssddMMyyyy}</pur_date>" +
                                               "<grp_id>{3}</grp_id><tar_id>{4}</tar_id>" +
                                               "<amou_payed>{5}</amou_payed><time_payed>{6}</time_payed>" +
                                               "<ini_date>{7:HHmmssddMMyyyy}</ini_date>" +
                                               "<end_date>{8:HHmmssddMMyyyy}</end_date>" +
                                               "<ver>{9}</ver><oper_id>{10}</oper_id><prov>{11}</prov><ext_acc>{12}</ext_acc><space>{13}</space><postpay>{14}</postpay>{15}" +
                                               "<ah>{16}</ah></ipark_in>",
                        strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, dOperationId,
                        strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, strAuthHash);
                }
                else
                {
                    strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<pur_date>{2:HHmmssddMMyyyy}</pur_date>" +
                                               "<grp_id>{3}</grp_id><tar_id>{4}</tar_id>" +
                                               "<amou_payed>{5}</amou_payed><time_payed>{6}</time_payed>" +
                                               "<ini_date>{7:HHmmssddMMyyyy}</ini_date>" +
                                               "<end_date>{8:HHmmssddMMyyyy}</end_date>" +
                                               "<ver>{9}</ver><oper_id>{10}</oper_id><prov>{11}</prov><ext_acc>{12}</ext_acc><space>{13}</space><postpay>{14}</postpay>{15}" +
                                               "<bon_mlt>{16}</bon_mlt>" +
                                               "<ah>{17}</ah></ipark_in>",
                        strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, iQuantity, iTime, dtIni, dtEnd, strvers, dOperationId,
                        strCompanyName, oUser.USR_USERNAME, strPlaceString, iPostpay, sAuthId, sBonMlt, strAuthHash);
                }
                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("BilbaoIntegrationConfirmParking Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.InsertExternalParkingOperationInstallationTime(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("BilbaoIntegrationConfirmParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        str3dPartyOpNum = wsParameters["oper_id"].ToString();
                        if (wsParameters["base_oper_id"] != null)
                            str3dPartyBaseOpNum = wsParameters["base_oper_id"].ToString();
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BilbaoIntegrationConfirmParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }





            return rtRes;

        }

        public ResultType GetLastOperations(int iWSNumber, decimal? dGroupId, decimal? dTariffId, ref String sExtGroupId, ref String sExtTariffId)
        {
            ResultType rtRes = ResultType.Result_OK;
            if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref sExtGroupId, ref sExtTariffId))
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogMessage("GetLastOperations::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
            }
            return rtRes;
        }
        
        
        public ResultType StandardConfirmUnParking(int iWSNumber, string strPlate, DateTime dtUnParkQuery, USER oUser, INSTALLATION oInstallation,
                                                   int iQuantity, int iTime, decimal dGroupId, decimal? dStreetSectionId, decimal dTariffId, DateTime dtIni, DateTime dtEnd,
                                                   decimal dOperationId, int? iWSTimeout, ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 


                StandardParkingWS.TariffComputerWS oUnParkWS = new StandardParkingWS.TariffComputerWS();
                oUnParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";

                switch (iWSNumber)
                {
                    case 1:
                        oUnParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oUnParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oUnParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oUnParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oUnParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oUnParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("StandardConfirmUnParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }


                DateTime dtInstallation = dtUnParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();

                string strExtTariffId = "";
                string strExtGroupId = "";


                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId, dTariffId, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("StandardConfirmUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sExtStreetSectionId = "";
                if (dStreetSectionId.HasValue)
                {
                    STREET_SECTION oStreetSection = null;
                    if (geograficAndTariffsRepository.GetStreetSection(dStreetSectionId.Value, out oStreetSection))
                    {
                        sExtStreetSectionId = oStreetSection.STRSE_ID_EXT;
                    }

                }

                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateStandardWSHash(strHashKey,
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strExtGroupId, sExtStreetSectionId, strExtTariffId, dOperationId, strCompanyName, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><ext_acc>{3}</ext_acc><grp_id>{4}</grp_id><stse_id>{5}</stse_id><tar_id>{6}</tar_id><oper_id>{7}</oper_id><prov>{8}</prov><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strExtGroupId, sExtStreetSectionId, strExtTariffId, dOperationId, strCompanyName, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardConfirmUnParking Timeout={1} xmlIn={0}", sXmlIn, oUnParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oUnParkWS.InsertExternalUnParkingOperationInstallationTime(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");


                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardConfirmUnParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);

                        if (wsParameters["oper_id"] != null)
                        {
                            str3dPartyOpNum = wsParameters["oper_id"].ToString();
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "StandardConfirmUnParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;
        }

        public ResultType BilbaoIntegrationConfirmUnParking(int iWSNumber, string strPlate, DateTime dtUnParkQuery, USER oUser, INSTALLATION oInstallation,
                                           int iQuantity, int iTime, decimal dGroupId, decimal dTariffId, DateTime dtIni, DateTime dtEnd,
                                           decimal dOperationId, int? iWSTimeout, ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                AddTLS12Support();
                var oUnParkWS = new BilbaoParkWsIntegraExternalService.integraExternalServices();
                oUnParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                oUnParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_PARK_WS_HTTP_USER))
                {
                    oUnParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                }


                DateTime dtInstallation = dtUnParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();

                string strExtTariffId = "";
                string strExtGroupId = "";


                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId, dTariffId, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("BilbaoIntegrationConfirmUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string strMessage = "";
                string strAuthHash = "";
                strAuthHash = CalculateStandardWSHash(oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}", strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strExtGroupId, strExtTariffId, dOperationId, strCompanyName, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><ext_acc>{3}</ext_acc><grp_id>{4}</grp_id><tar_id>{5}</tar_id><oper_id>{6}</oper_id><prov>{7}</prov><vers>{8}</vers><ah>{9}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strExtGroupId, strExtTariffId, dOperationId, strCompanyName, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("BilbaoIntegrationConfirmUnParking Timeout={1} xmlIn={0}", sXmlIn, oUnParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oUnParkWS.InsertExternalUnParkingOperationInstallationTime(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");


                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("BilbaoIntegrationConfirmUnParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);

                        if (wsParameters["oper_id"] != null)
                        {
                            str3dPartyOpNum = wsParameters["oper_id"].ToString();
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BilbaoIntegrationConfirmUnParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;

        }

        public ResultType GtechnaConfirmParking(int iWSNumber, decimal? dMobileSessionId, string strPlate, DateTime dtParkQuery, INSTALLATION oInstallation,decimal? dGroupId,decimal? dTariffId, 
                                                int iQuantity, int iTime,DateTime dtIni, DateTime dtEnd, decimal dOperationId, int? iWSTimeout, ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sParamsIn = "";
            string sParamsOut = "";
            Exception oNotificationEx = null;            

            try
            {
                AddTLS12Support();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                gTechnaThirdPartyParkingConfirmWS.MESParkingRightsSOAPFacadeService oParkWS = new gTechnaThirdPartyParkingConfirmWS.MESParkingRightsSOAPFacadeService();
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                switch (iWSNumber)
                {
                    case 1:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("GtechnaConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                DateTime dtInstallation = dtParkQuery;
                string strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();

                string strUsername = "";
                string strWIFIMAC = "";
                string strIMEI = "";

                MOBILE_SESSION oMobileSession = null;
                if (dMobileSessionId.HasValue)
                    customersRepository.GetMobileSessionById(dMobileSessionId.Value, out oMobileSession);
                if (oMobileSession != null)
                {
                    strUsername = oMobileSession.USER.USR_USERNAME;
                    strWIFIMAC = oMobileSession.MOSE_CELL_WIFI_MAC ?? "";
                    strIMEI = oMobileSession.MOSE_CELL_IMEI ?? "";
                }

                string strTerminalNumber = strIMEI + "/" + strWIFIMAC;

                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("GtechnaConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                    return rtRes;
                }


                bool bPrefixFound = false;
                /*foreach (string strProvince in CanadaAndUSAProvinces)
                {
                    if (strPlate.Substring(0, 2) == strProvince)
                    {
                        bPrefixFound = true;
                        break;
                    }
                }*/


                string strParameterPlate = "";
                string strParameterState = "";
                if (bPrefixFound)
                {
                    strParameterPlate = strPlate.Substring(2, strPlate.Length - 2);
                    strParameterState = strPlate.Substring(0, 2);
                }
                else
                {
                    strParameterPlate = strPlate;
                    strParameterState = "";
                }

                DateTime? dtInstallationUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtInstallation);
                DateTime? dtIniUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtIni);
                DateTime? dtEndUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtEnd);

                sParamsIn = string.Format("updateParkingRights({0},null,{1},{2},{3:HH:mm:ss dd/MM/yyyy},{4:HH:mm:ss dd/MM/yyyy},{5:HH:mm:ss dd/MM/yyyy},{6},{7},0,{8},{9},null,null,false,{10},null,null,null,null,false) Timeout={11}",
                                           strParameterPlate,
                                           strCompanyName,
                                           dOperationId.ToString(),
                                           dtInstallationUTC,
                                           dtIniUTC,
                                           dtEndUTC,
                                           Convert.ToDecimal(iQuantity) / 100,
                                           infraestructureRepository.GetCurrencyIsoCode( Convert.ToInt32(oInstallation.INS_CUR_ID)),
                                           strTerminalNumber,
                                           strExtGroupId,
                                           strParameterState,
                                           oParkWS.Timeout);

                Logger_AddLogMessage(sParamsIn, LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();

                string strRes = oParkWS.updateParkingRights(strParameterPlate,
                                            null,
                                            strCompanyName,
                                            dOperationId.ToString(),
                                            dtInstallationUTC.Value,
                                            dtIniUTC.Value,
                                            dtEndUTC.Value,
                                            Convert.ToDecimal(iQuantity) / 100,
                                            infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oInstallation.INS_CUR_ID)),
                                            0,
                                            strTerminalNumber,
                                            strExtGroupId,
                                            null,
                                            null,
                                            false,
                                            strParameterState,
                                            null,
                                            null,
                                            null,
                                            null,
                                            false);

                lEllapsedTime = watch.ElapsedMilliseconds;

                Logger_AddLogMessage(string.Format("GtechnaConfirmParking:updateParkingRights()={0}", strRes), LogLevels.logDEBUG);

                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                str3dPartyOpNum = strRes;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "GtechnaConfirmParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sParamsIn, sParamsOut, false, oNotificationEx);
            }
            catch
            { }


            return rtRes;
        }
        
        public ResultType EysaConfirmUnParking(int iWSNumber, string strPlate, DateTime dtInstallation, USER oUser, INSTALLATION oInstallation, 
                                               int iQuantity, int iTime, decimal? dGroupId,decimal? dTariffId,DateTime dtIni, DateTime dtEnd, int? iWSTimeout,
                                               ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                EysaThirdPartyConfirmParkWS.Ticket oParkWS = new EysaThirdPartyConfirmParkWS.Ticket();
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";

                switch (iWSNumber)
                {
                    case 1:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("EysaConfirmUnParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }



                EysaThirdPartyConfirmParkWS.ConsolaSoapHeader authentication = new EysaThirdPartyConfirmParkWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oParkWS.ConsolaSoapHeaderValue = authentication;

                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;
                string strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();


                string strExtTariffId = "";
                string strExtGroupId = "";


                if ((dGroupId.HasValue)&&(dTariffId.HasValue)&&
                    (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId)))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("EysaConfirmUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }


                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateEysaWSHash(strHashKey,
                    string.Format("1{0}{1:yyyy-MM-ddTHH:mm:ss.fff}{2}",
                    strPlate, dtInstallation, iQuantity, strvers));

                strMessage = string.Format("<ipark_in><u>1</u><m>{0}</m><d>{1:yyyy-MM-ddTHH:mm:ss.fff}</d><q>{2}</q>" +
                                           "<vers>{3}</vers><ah>{4}</ah><em>{5}</em><g>{6}</g><tar_id>{7}</tar_id></ipark_in>",
                    strPlate, dtInstallation, iQuantity, strvers, strAuthHash, strCompanyName, strExtGroupId, strExtTariffId);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaConfirmUnParking Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.rdPConfirmUnParkingOperation(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaConfirmUnParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);

                        if (wsParameters["opnum"] != null)
                        {
                            str3dPartyOpNum = wsParameters["opnum"].ToString();
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "EysaConfirmUnParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;

        }

        public ResultType EysaQueryParking(int iWSNumber, USER oUser, string strPlate, DateTime dtParkQuery, GROUP oGroup, TARIFF oTariff, bool bWithSteps, int? iMaxAmountAllowedToPay,
                                           double dChangeToApply, ulong ulAppVersion, bool bIsShopKeeperOperation, bool bApplyCampaingDiscount, decimal? dApplyCampaingDiscount, string culture,
                                           int? iWSTimeout, ref SortedList parametersOut, ref string strAuthId, out string strQPlusVATQs, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            strQPlusVATQs = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;



            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                EysaThirdPartyParkWS.Tarifas oParkWS = new EysaThirdPartyParkWS.Tarifas();
                oParkWS.Url = oGroup.INSTALLATION.INS_PARK_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new System.Net.NetworkCredential(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER, oGroup.INSTALLATION.INS_PARK_WS_HTTP_PASSWORD);
                }

                EysaThirdPartyParkWS.ConsolaSoapHeader authentication = new EysaThirdPartyParkWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oGroup.INSTALLATION.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oParkWS.ConsolaSoapHeaderValue = authentication;

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oGroup.INSTALLATION.INS_EYSA_CONTRATA_ID;
                string strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";

                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, oGroup, oTariff, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("EysaQueryParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sApplyCampaingDiscount = string.Empty;
                if (dApplyCampaingDiscount.HasValue)
                {
                    dApplyCampaingDiscount = dApplyCampaingDiscount.Value;
                    sApplyCampaingDiscount = dApplyCampaingDiscount.Value.ToString(CultureInfo.InvariantCulture);
                }

                if (strExtTariffId.Length == 0)
                {
                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateEysaWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("1{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}", strCityID, strPlate, dtInstallation, strExtGroupId, strvers));

                        strMessage = string.Format("<ipark_in><u>1</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><vers>{4}</vers><bon_mlt>{5}</bon_mlt><ah>{6}</ah><em>{7}</em></ipark_in>",
                            strCityID, strPlate, dtInstallation, strExtGroupId, strvers, sApplyCampaingDiscount, strAuthHash, strCompanyName);
                    }
                    else
                    {
                        strAuthHash = CalculateEysaWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("1{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}", strCityID, strPlate, dtInstallation, strExtGroupId, strvers));

                        strMessage = string.Format("<ipark_in><u>1</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><vers>{4}</vers><ah>{5}</ah><em>{6}</em></ipark_in>",
                            strCityID, strPlate, dtInstallation, strExtGroupId, strvers, strAuthHash, strCompanyName);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {

                        strAuthHash = CalculateEysaWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("1{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}{5}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strvers));

                        strMessage = string.Format("<ipark_in><u>1</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><tar_id>{4}</tar_id><vers>{5}</vers><bon_mlt>{6}</bon_mlt><ah>{7}</ah><em>{8}</em></ipark_in>",
                            strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strvers, sApplyCampaingDiscount, strAuthHash, strCompanyName);
                    }
                    else
                    {
                        strAuthHash = CalculateEysaWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("1{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}{5}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strvers));

                        strMessage = string.Format("<ipark_in><u>1</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><tar_id>{4}</tar_id><vers>{5}</vers><ah>{6}</ah><em>{7}</em></ipark_in>",
                            strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strvers, strAuthHash, strCompanyName);
                    }

                }

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaThirdPartyQueryParkingOperationWithTimeSteps Timeout={1} xmlIn={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.rdPQueryParkingOperationWithTimeSteps(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaThirdPartyQueryParkingOperationWithTimeSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        parametersOut["ad"] = oTariff.TAR_ID.ToString();
                        parametersOut["q1"] = wsParameters["q1"];
                        parametersOut["q2"] = wsParameters["q2"];
                        parametersOut["t1"] = wsParameters["t1"];
                        parametersOut["t2"] = wsParameters["t2"];
                        parametersOut["o"] = wsParameters["o"];
                        parametersOut["at"] = wsParameters["at"];
                        parametersOut["aq"] = wsParameters["aq"];
                        parametersOut["cur"] = oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE;

                        DateTime dt = DateTime.ParseExact(wsParameters["di"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                        parametersOut["di"] = dt.ToString("HHmmssddMMyy");

                        parametersOut["bonusper"] = wsParameters["bonusper"];
                        parametersOut["bonusid"] = wsParameters["bonusid"];
                        parametersOut["bonusmarca"] = wsParameters["marca"];
                        parametersOut["bonustype"] = wsParameters["oeysa"];

                        Logger_AddLogMessage(string.Format("EysaThirdPartyQueryParkingOperationWithTimeSteps CurrVersion {0}, 1.2.4 ={1}, Plate = {2}", ulAppVersion, _VERSION_1_2_4, strPlate), LogLevels.logDEBUG);

                        strAuthId = "";
                        if (wsParameters.ContainsKey("idP"))
                        {
                            strAuthId = wsParameters["idP"].ToString();
                        }

                        if (ulAppVersion >= _VERSION_1_2_4)
                        {
                            parametersOut["idP"] = strAuthId;
                            parametersOut["coe"] = wsParameters["coe"];
                            parametersOut["carcat_desc"] = wsParameters["ca"];
                            parametersOut["ocu_desc"] = wsParameters["ocu"];
                            if (wsParameters["coe"] != null && wsParameters["ca"] != null && wsParameters["ocu"] != null)
                                parametersOut["forcedisp"] = "1";
                            else
                                parametersOut["forcedisp"] = "0";

                            Logger_AddLogMessage(string.Format("EysaThirdPartyQueryParkingOperationWithTimeSteps Coe={0}, Ca={1}, Ocu={2}, ForceDisp={4}, Plate={3}", wsParameters["coe"], wsParameters["ca"], wsParameters["ocu"], strPlate, parametersOut["forcedisp"]), LogLevels.logDEBUG);

                        }
                        else
                            parametersOut["forcedisp"] = "0";


                        double dChangeFee = 0;
                        int iQChange = 0;

                        if (oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                        {
                            iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q1"]),
                                            dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                            parametersOut["qch1"] = iQChange.ToString();
                            iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q2"]),
                                            dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                            parametersOut["qch2"] = iQChange.ToString();

                        }


                        if (bWithSteps)
                        {
                            int iNumSteps = Convert.ToInt32(wsParameters["steps_step_num"]);                            

                            ChargeOperationsType oOperationType = (Convert.ToInt32(parametersOut["o"]) == 1 ? ChargeOperationsType.ParkingOperation : ChargeOperationsType.ExtensionOperation);

                            int iQ = 0;
                            int iQFEE = 0;
                            decimal dQFEE = 0;
                            int iQFEEChange = 0;
                            int iQBonus = 0;
                            int iQBonusChange = 0;
                            int iQVAT = 0;
                            int iQVATChange = 0;
                            int iQSubTotal = 0;
                            int iQTotal = 0;
                            int iQTotalChange = 0;
                            int iQSubTotalChange = 0;

                            decimal dVAT1;
                            decimal dVAT2;
                            int iPartialVAT1;
                            decimal dPercFEE;
                            decimal dPercFEETopped;
                            int iPartialPercFEE;
                            decimal dFixedFEE;
                            int iPartialFixedFEE;
                            int iPartialPercFEEVAT;
                            int iPartialFixedFEEVAT;
                            decimal dPercBonus = 0;
                            int iPartialBonusFEE;
                            int iPartialBonusFEEVAT;
                            int iQPlusIVA;
                            int iFeePlusIVA;

                            int? iPaymentTypeId = null;
                            int? iPaymentSubtypeId = null;
                            int? iTariffType = null;
                            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                            {
                                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                            }
                            if (oTariff != null)
                                iTariffType = oTariff.TAR_TYPE;
                            if (!customersRepository.GetFinantialParams(oUser, oGroup.INSTALLATION.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, oOperationType, iTariffType,
                                                                        out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogMessage("EysaQueryParking::Error getting finantial parameters", LogLevels.logERROR);
                            }

                            if (rtRes == ResultType.Result_OK)
                            {

                                int iAcumTime = 0;

                                try
                                {
                                    iAcumTime = Convert.ToInt32(parametersOut["at"].ToString());
                                }
                                catch{}

                                //TO DO: Se agrega bShowOutOfRateTime
                                parametersOut["NoticeChargesNow"] = oTariff.TAR_NOTICE_CHARGES_NOW;
                                String sLiteral = string.Empty;

                                if (oTariff.TAR_NOTICE_CHARGES_NOW != (int)NoticChargesNow.NotShowMessage && ((Convert.ToInt32(parametersOut["o"])) == (int)ChargeOperationsType.ParkingOperation && dtParkQuery < dt && iAcumTime==0))
                                {
                                    string sCulture = (!string.IsNullOrEmpty(culture) ? culture : "en-US");
                                    if (oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.HasValue)
                                    {
                                        sLiteral = infraestructureRepository.GetLiteral(oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.Value, culture);
                                        sLiteral = sLiteral.Replace("#", dt.ToShortTimeString() + " | " + dt.ToShortDateString());
                                    }
                                }
                                else
                                {
                                    parametersOut["NoticeChargesNow"] = (int)NoticChargesNow.NotShowMessage;
                                }
                                parametersOut["NoticeChargesNowLiteral"] = sLiteral;


                                if (parametersOut["bonusper"] != null)
                                {
                                    dPercBonus = Convert.ToDecimal(parametersOut["bonusper"]) / Convert.ToDecimal(100);
                                }

                                decimal dShopKeeperParkPerc = oGroup.INSTALLATION.INSTALLATION_SHOPKEEPER_PARAMETERs.FirstOrDefault() == null ? (decimal)0 : oGroup.INSTALLATION.INSTALLATION_SHOPKEEPER_PARAMETERs.First().INSSHO_PARK_PROFIT_PERC;
                                int iAmountTotal = 0;
                                int iAmountProfit = 0;

                                StringBuilder sb = new StringBuilder();
                                StringBuilder sb2 = new StringBuilder();


                                DateTime? dtNow = geograficAndTariffsRepository.getInstallationDateTime(oGroup.GRP_INS_ID) ?? DateTime.Now;
                                string strTodayColour = infraestructureRepository.GetParameterValue("ParkingScreenTodayColour");
                                string strTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenTomorrowColour");
                                string strMoreThanTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenMoreThanTomorrowColour");


                                for (int i = 0; i < iNumSteps; i++)
                                {

                                    dt = DateTime.ParseExact(wsParameters[string.Format("steps_step_{0}_d", i)].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                            CultureInfo.InvariantCulture);

                                    int iTotalDays = Convert.ToInt32((dt.Date - dtNow.Value.Date).TotalDays);
                                    if (iTotalDays < 0)
                                        iTotalDays = 0;

                                    iQ = Convert.ToInt32(wsParameters[string.Format("steps_step_{0}_q", i)].ToString());
                                    
                                    

                                   

                                    iQTotal = customersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus,eTaxMode,
                                                                               out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                               out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);
                                    dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                                    if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                                    dQFEE += dFixedFEE;
                                    iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));
                                    iQBonus = Convert.ToInt32(Math.Round(dQFEE * dPercBonus, MidpointRounding.AwayFromZero));
                                    iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
                                    iQSubTotal = iQ + iQFEE - iQBonus;
                                    iQPlusIVA = iQ + iPartialVAT1;
                                    iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

                                    if (i > 0)
                                    {
                                        sb2.Append("|");
                                    }
                                    sb2.AppendFormat("{0};{1}", iQ, iQPlusIVA);

                                    if (oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                                    {
                                        iQChange = ChangeQuantityFromInstallationCurToUserCur(iQ, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                                        iQFEEChange = ChangeQuantityFromInstallationCurToUserCur(iQFEE, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);
                                        iQVATChange = ChangeQuantityFromInstallationCurToUserCur(iQVAT, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);
                                        iQBonusChange = ChangeQuantityFromInstallationCurToUserCur(iQBonus, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);
                                        iQSubTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQSubTotal, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);
                                        iQTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQTotal, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);


                                        if (iMaxAmountAllowedToPay.HasValue)
                                        {
                                            if (iQTotalChange > iMaxAmountAllowedToPay)
                                            {
                                                if (i == 0)
                                                {
                                                    rtRes = ResultType.Result_Error_Not_Enough_Balance; ;
                                                    parametersOut["r"] = Convert.ToInt32(rtRes);
                                                }
                                                break;
                                            }
                                        }

                                        if (bIsShopKeeperOperation)
                                        {
                                           
                                            customersRepository.CalculateShopKeeperProfit(iQTotalChange,dShopKeeperParkPerc,out iAmountProfit, out iAmountTotal);
                                            
                                            sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><qch>{2}</qch><d>{3:HHmmssddMMyy}</d><q_fee>{4}</q_fee><qbonusam>{11}</qbonusam><q_vat>{5}</q_vat><q_subtotal>{6}</q_subtotal><q_total>{7}</q_total>"+
                                                            "<qch_fee>{8}</qch_fee><qch_vat>{15}</qch_vat><qchbonusam>{12}</qchbonusam><qch_subtotal>{9}</qch_subtotal><qch_total>{10}</qch_total><q_shopkeeper_profit>{13}</q_shopkeeper_profit>"+
                                                            "<q_shopkeeper>{14}</q_shopkeeper><num_days>{16}</num_days><num_days_colour>{17}</num_days_colour></step>",
                                                                   wsParameters[string.Format("steps_step_{0}_t", i)].ToString(),
                                                                   iQ,
                                                                   iQChange,
                                                                   dt,
                                                                   iQFEE, iQVAT, iQSubTotal, iQTotal,
                                                                   iQFEEChange, iQSubTotalChange, iQTotalChange,
                                                                   -iQBonus, -iQBonusChange,
                                                                   iAmountProfit, iAmountTotal,
                                                                   iQVATChange,
                                                                   iTotalDays,
                                                                   (iTotalDays<=0)?strTodayColour:((iTotalDays==1)?strTomorrowColour:strMoreThanTomorrowColour)
                                                                   );

                                        }
                                        else
                                        {



                                            sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><qch>{2}</qch><d>{3:HHmmssddMMyy}</d><q_fee>{4}</q_fee><qbonusam>{11}</qbonusam><q_vat>{5}</q_vat><q_subtotal>{6}</q_subtotal><q_total>{7}</q_total>"+
                                                            "<qch_fee>{8}</qch_fee><qch_vat>{13}</qch_vat><qchbonusam>{12}</qchbonusam><qch_subtotal>{9}</qch_subtotal><qch_total>{10}</qch_total><num_days>{14}</num_days><num_days_colour>{15}</num_days_colour></step>",
                                                                    wsParameters[string.Format("steps_step_{0}_t", i)].ToString(),
                                                                    wsParameters[string.Format("steps_step_{0}_q", i)].ToString(),
                                                                    iQChange,
                                                                    dt,
                                                                    iQFEE, iQVAT, iQSubTotal, iQTotal,
                                                                    iQFEEChange, iQSubTotalChange, iQTotalChange,
                                                                    -iQBonus, -iQBonusChange,
                                                                    iQVATChange,
                                                                     iTotalDays,
                                                                    (iTotalDays == 0) ? strTodayColour : ((iTotalDays == 1) ? strTomorrowColour : strMoreThanTomorrowColour));
                                        }
                                        parametersOut["q2"] = wsParameters[string.Format("steps_step_{0}_q", i)];
                                        parametersOut["t2"] = wsParameters[string.Format("steps_step_{0}_t", i)];


                                    }
                                    else
                                    {
                                        if (iMaxAmountAllowedToPay.HasValue)
                                        {
                                            if (iQTotal > iMaxAmountAllowedToPay)
                                            {
                                                if (i == 0)
                                                {
                                                    rtRes = ResultType.Result_Error_Not_Enough_Balance; ;
                                                    parametersOut["r"] = Convert.ToInt32(rtRes);
                                                }
                                                break;
                                            }
                                        }


                                        if (bIsShopKeeperOperation)
                                        {
                                            customersRepository.CalculateShopKeeperProfit(iQTotal, dShopKeeperParkPerc, out iAmountProfit, out iAmountTotal);

                                            sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><d>{2:HHmmssddMMyy}</d><q_fee>{3}</q_fee><qbonusam>{7}</qbonusam><q_vat>{4}</q_vat><q_subtotal>{5}</q_subtotal><q_total>{6}</q_total>"+
                                                             "<q_shopkeeper_profit>{8}</q_shopkeeper_profit><q_shopkeeper>{9}</q_shopkeeper><num_days>{10}</num_days><num_days_colour>{11}</num_days_colour></step>",
                                                                  wsParameters[string.Format("steps_step_{0}_t", i)].ToString(),
                                                                  wsParameters[string.Format("steps_step_{0}_q", i)].ToString(),
                                                                  dt,
                                                                  iQFEE, iQVAT,
                                                                  iQSubTotal, iQTotal,
                                                                  -iQBonus, iAmountProfit, iAmountTotal,
                                                                   iTotalDays,
                                                                   (iTotalDays == 0) ? strTodayColour : ((iTotalDays == 1) ? strTomorrowColour : strMoreThanTomorrowColour));
                                        }
                                        else
                                        {

                                            sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><d>{2:HHmmssddMMyy}</d><q_fee>{3}</q_fee><qbonusam>{7}</qbonusam><q_vat>{4}</q_vat><q_subtotal>{5}</q_subtotal><q_total>{6}</q_total>"+
                                                            "<num_days>{8}</num_days><num_days_colour>{9}</num_days_colour></step>",
                                                                    wsParameters[string.Format("steps_step_{0}_t", i)].ToString(),
                                                                    wsParameters[string.Format("steps_step_{0}_q", i)].ToString(),
                                                                    dt,
                                                                    iQFEE, iQVAT,
                                                                    iQSubTotal, iQTotal,
                                                                    -iQBonus,
                                                                   iTotalDays,
                                                                   (iTotalDays == 0) ? strTodayColour : ((iTotalDays == 1) ? strTomorrowColour : strMoreThanTomorrowColour));
                                        }
                                        parametersOut["q2"] = wsParameters[string.Format("steps_step_{0}_q", i)];
                                        parametersOut["t2"] = wsParameters[string.Format("steps_step_{0}_t", i)];


                                    }

                                }
                                parametersOut["steps"] = sb.ToString();
                                strQPlusVATQs = sb2.ToString();
                            }                            
                        }

                    }
                    else
                    {
                       

                        if (wsParameters["r"].ToString() == "-100")
                        {
                            rtRes = ResultType.Result_Parking_Not_Allowed;
                            parametersOut["r"] = Convert.ToInt32(rtRes);                            
                            parametersOut["rsub"] = wsParameters["rsub"]; ;
                            parametersOut["rsubcesc"] = wsParameters["rsubcesc"]; ;


                        }
                        else
                        {
                            parametersOut["r"] = Convert.ToInt32(wsParameters["r"]);
                            rtRes = (ResultType)parametersOut["r"];
                        }
                    }

                }



            }
            catch (Exception e)
            {

                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "EysaQueryParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;

        }

        public ResultType EysaQueryUnParking(int iWSNumber,USER oUser, string strPlate, DateTime dtUnParkQuery, INSTALLATION oInstallation, ulong ulAppVersion,
                                             int? iWSTimeout, ref SortedList parametersOut, ref List<SortedList> lstRefunds, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;            
            decimal? dGroupId = null;
            decimal? dTariffId = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 


                EysaThirdPartyConfirmParkWS.Ticket oUnParkWS = new EysaThirdPartyConfirmParkWS.Ticket();
                oUnParkWS.Url = oInstallation.INS_UNPARK_WS_URL;
                oUnParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_UNPARK_WS_HTTP_USER))
                {
                    oUnParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_UNPARK_WS_HTTP_USER, oInstallation.INS_UNPARK_WS_HTTP_PASSWORD);
                }

                EysaThirdPartyConfirmParkWS.ConsolaSoapHeader authentication = new EysaThirdPartyConfirmParkWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oUnParkWS.ConsolaSoapHeaderValue = authentication;

                DateTime dtInstallation = dtUnParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;
                string strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateEysaWSHash(oInstallation.INS_UNPARK_WS_AUTH_HASH_KEY,
                    string.Format("1{0}{1:yyyy-MM-ddTHH:mm:ss.fff}{2}", strPlate, dtInstallation, strvers));

                strMessage = string.Format("<ipark_in><u>1</u><p>{0}</p><d>{1:yyyy-MM-ddTHH:mm:ss.fff}</d><vers>{2}</vers><ah>{3}</ah><em>{4}</em><g>{5}</g></ipark_in>",
                    strPlate, dtInstallation, strvers, strAuthHash, strCompanyName, strCityID);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaQueryUnParking Timeout={1} xmlIn={0}", sXmlIn, oUnParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oUnParkWS.rdPQueryUnParkingOperation(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaQueryUnParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {
                        /*
                         	  <r>1</r>
	                          <q>165</q>
	                          <d1>04/12/2013 11:15:19</d1>
	                          <d2>04/12/2013 11:35:05</d2>
	                          <t>99</t>
	                        </ipark_out>                         
                         */
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);

                        SortedList oRefund = new SortedList();
                        oRefund["ins_id"] = oInstallation.INS_ID;
                        oRefund["p"] = strPlate;
                        oRefund["q"] = wsParameters["q"];
                        oRefund["t"] = wsParameters["t"];
                        oRefund["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                        DateTime dt1 = DateTime.ParseExact(wsParameters["d1"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                        DateTime dt2 = DateTime.ParseExact(wsParameters["d2"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                        oRefund["d1"] = dt1.ToString("HHmmssddMMyy");
                        oRefund["d2"] = dt2.ToString("HHmmssddMMyy");
                        oRefund["exp"] = (Conversions.RoundSeconds(dtInstallation) < Conversions.RoundSeconds(dt2)) ? "0" : "1";

                       
                        oRefund["bonusper"] = wsParameters["bonusper"];
                        oRefund["bonusid"] = wsParameters["bonusid"];

                    

                        string strExtTariffId="";
                        string strExtGroupId ="";

                        if ((wsParameters["tar_id"] != null) && (wsParameters["g"] != null))
                        {

                            strExtTariffId = wsParameters["tar_id"].ToString();
                            strExtGroupId = wsParameters["g"].ToString();

                            if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(iWSNumber, dtInstallation, oInstallation, strExtGroupId, 
                                    strExtTariffId, ref dGroupId, ref dTariffId))
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogMessage("EysaQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                return rtRes;
                            }

                            if (!dTariffId.HasValue)
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogMessage("EysaQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                return rtRes;
                            }

                            if (!dGroupId.HasValue)
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogMessage("EysaQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                return rtRes;
                            }


                            oRefund["ad"] = dTariffId.Value;
                            oRefund["g"] = dGroupId.Value;
                        }
                        
                        lstRefunds.Add(oRefund);
                    }
                    else
                    {
                        parametersOut["r"] = Convert.ToInt32(wsParameters["r"]);
                        rtRes = (ResultType)parametersOut["r"];
                    }                  

                }



            }
            catch (Exception e)
            {

                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "EysaQueryUnParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            { }


            return rtRes;

        }

        public ResultType StandardQueryParkingTimeSteps(int iWSNumber, USER oUser, string strPlate, List<string> oAdditionalPlates, Dictionary<int, List<string>> oExtraPlates, DateTime dtParkQuery, GROUP oGroup, STREET_SECTION oStreetSection, TARIFF oTariff, bool bWithSteps,
                                                        int? iMaxAmountAllowedToPay, double dChangeToApply, bool bIsShopKeeperOperation, bool bApplyCampaingDiscount, decimal? dApplyCampaingDiscount,
                                                        int? iCampaingFreeMinutes, int? iCampaingMinimumTimeToApply,
                                                        string sCulture, int? iWSTimeout, ref SortedList parametersOut,
                                                        ref List<SortedList> oAdditionals, out string strQPlusVATQs, out string sAuthId, out decimal? dBonMlt, out decimal? dBonExtMlt, out string sVehicleType, ref int? iCampaingAmountToSubstract,
                                                        ref Dictionary<int, List<SortedList>> oExtraParametersOut, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;

            strQPlusVATQs = "";
            sAuthId = "";
            dBonMlt = null;
            dBonExtMlt = null;
            sVehicleType = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;
            ResultType rtResDetail = ResultType.Result_OK;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 


                StandardParkingWS.TariffComputerWS oParkWS = new StandardParkingWS.TariffComputerWS();
                oParkWS.Url = oGroup.INSTALLATION.INS_PARK_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new System.Net.NetworkCredential(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER, oGroup.INSTALLATION.INS_PARK_WS_HTTP_PASSWORD);
                }

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oGroup.INSTALLATION.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";

                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, oGroup, oTariff, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("StandardQueryParkingTimeSteps::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sExtStreetSectionId = (oStreetSection != null ? oStreetSection.STRSE_ID_EXT : "");
                
                string sPlatesValues = strPlate;
                string sPlatesTags = string.Format("<lic_pla>{0}</lic_pla>", strPlate);
                if (oAdditionalPlates != null)
                {
                    for (int i = 0; i < oAdditionalPlates.Count; i += 1)
                    {
                        sPlatesValues += oAdditionalPlates[i];
                        sPlatesTags += string.Format("<lic_pla{0}>{1}</lic_pla{0}>", i + 2, oAdditionalPlates[i]);
                    }
                }
                if (oExtraPlates != null)
                {
                    foreach (int iExtra in oExtraPlates.Keys)
                    {
                        for (int i = 0; i < oExtraPlates[iExtra].Count; i += 1)
                        {
                            sPlatesValues += oExtraPlates[iExtra][i];
                            sPlatesTags += string.Format("<lic_pla{0}_{1}>{2}</lic_pla{0}_{1}>", iExtra, i + 1, oExtraPlates[iExtra][i]);
                        }
                    }
                }

                string sApplyCampaingDiscount = string.Empty;
                if (dApplyCampaingDiscount.HasValue)
                {
                    dApplyCampaingDiscount = dApplyCampaingDiscount.Value;
                    sApplyCampaingDiscount = dApplyCampaingDiscount.Value.ToString(CultureInfo.InvariantCulture);
                }

                if (!bWithSteps)
                {
                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><bon_mlt>{10}</bon_mlt><ah>{11}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount, strAuthHash);
                    }
                    else
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, strAuthHash);
                    }
                }
                else
                {
                    int? iTimeOffSet = DEFAULT_TIME_STEP; //minutes
                    geograficAndTariffsRepository.GetGroupAndTariffStepOffsetMinutes(oGroup, oTariff, out iTimeOffSet);

                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}{10}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iTimeOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><time_off>{6}</time_off><prov>{7}</prov><ext_acc>{8}</ext_acc><free_time>{9}</free_time><vers>{10}</vers><bon_mlt>{11}</bon_mlt><ah>{12}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iTimeOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount, strAuthHash);
                    }
                    else
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}{10}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iTimeOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><time_off>{6}</time_off><prov>{7}</prov><ext_acc>{8}</ext_acc><free_time>{9}</free_time><vers>{10}</vers><ah>{11}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iTimeOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, strAuthHash);
 
                    }

                }

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryParkingTimeSteps Timeout={1} xmlIn={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                string strOut = "";
                watch = Stopwatch.StartNew();
                if (!bWithSteps)
                {
                    strOut = oParkWS.QueryParkingOperation(strMessage);
                }
                else
                {
                    strOut = oParkWS.QueryParkingOperationWithTimeSteps(strMessage);
                }
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardQueryParkingTimeSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (Convert.ToInt32(rtRes)>Convert.ToInt32(ResultType.Result_OK))
                    {
                        rtResDetail = rtRes;
                        rtRes = ResultType.Result_OK;
                        parametersOut["rdetail"] = Convert.ToInt32(rtResDetail);
                    }


                    if (rtRes == ResultType.Result_OK)
                    {
                        rtRes = StandardQueryParkingComputeOutput("", iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, oStreetSection, oTariff, bWithSteps, iMaxAmountAllowedToPay, 
                            dChangeToApply, strExtGroupId, sExtStreetSectionId, strExtTariffId, bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                            iCampaingFreeMinutes, iCampaingMinimumTimeToApply, sCulture, ref wsParameters, 
                            ref parametersOut, ref strQPlusVATQs, out sAuthId, out dBonMlt, out dBonExtMlt, out sVehicleType, ref iCampaingAmountToSubstract);
                    }
                    else
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    parametersOut["numadditionals"] = 0;

                    if (wsParameters["numadditionals"] != null && Convert.ToInt32(wsParameters["numadditionals"]) > 0)
                    {
                        int iNumAdditionals = 0;
                        int i = 0;
                        bool bExit = false;
                        ResultType rtResInt;

                        do
                        {
                            bExit = (wsParameters[string.Format("additionals_parkingdata_{0}_r", i)] == null);

                            if (!bExit)
                            {
                                rtResInt = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters[string.Format("additionals_parkingdata_{0}_r", i)].ToString()));
                                rtResDetail = ResultType.Result_OK;
                                SortedList parametersOutTemp = new SortedList();

                                if (Convert.ToInt32(rtResInt) > Convert.ToInt32(ResultType.Result_OK))
                                {
                                    rtResDetail = rtResInt;
                                    rtResInt = ResultType.Result_OK;
                                    parametersOutTemp["rdetail"] = Convert.ToInt32(rtResDetail);
                                }


                                if (rtResInt == ResultType.Result_OK)
                                {

                                    if (wsParameters[string.Format("additionals_parkingdata_{0}_is_remote_extension", i)] != null &&
                                        Convert.ToInt32(wsParameters[string.Format("additionals_parkingdata_{0}_is_remote_extension", i)]) == 0)
                                    {
                                        string strQPlusVATQsTemp = "";
                                        string sAuthIdTemp = "";
                                        decimal? dBonMltTemp = null;
                                        string sVehicleTypeTemp = null;

                                        rtResInt = StandardQueryParkingComputeOutput(string.Format("additionals_parkingdata_{0}_", i),
                                                                                        iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, oStreetSection, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, sExtStreetSectionId, strExtTariffId, 
                                                                                        bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                                                                                        iCampaingFreeMinutes, iCampaingMinimumTimeToApply, 
                                                                                        sCulture, ref wsParameters, ref parametersOutTemp, ref strQPlusVATQsTemp,
                                                                                        out sAuthIdTemp, out dBonMltTemp, out dBonExtMlt, out sVehicleTypeTemp, ref iCampaingAmountToSubstract);


                                        if (rtResInt == ResultType.Result_OK)
                                        {
                                            oAdditionals.Add(parametersOutTemp);
                                            iNumAdditionals++;
                                        }

                                    }
                                }
                            }
                            i++;

                        }
                        while (!bExit);

                        if (iNumAdditionals > 0)
                        {
                            parametersOut["numadditionals"] = iNumAdditionals;
                            parametersOut["additionals"] = "";


                        }
                    }
                    

                    if (wsParameters["numextras"] != null && Convert.ToInt32(wsParameters["numextras"]) > 0)
                    {
                        int iNumExtras = Convert.ToInt32(wsParameters["numextras"]);
                        bool bExit = false;
                        ResultType rtResInt;

                        if (oExtraParametersOut == null)
                            oExtraParametersOut = new Dictionary<int,List<SortedList>>();                        

                        for (int iExtra = 0; iExtra < iNumExtras; iExtra += 1)
                        {
                            var oLstParametersOutTemp = new List<SortedList>();
                            
                            int iParkingData = 0;
                            do {

                                bExit = (wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_r", iExtra, iParkingData)] == null);
                                if (!bExit) 
                                {
                                    SortedList parametersOutTemp = new SortedList();

                                    rtResInt = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_r", iExtra, iParkingData)].ToString()));
                                    rtResDetail = ResultType.Result_OK;

                                    if (Convert.ToInt32(rtResInt) > Convert.ToInt32(ResultType.Result_OK))
                                    {
                                        rtResDetail = rtResInt;
                                        rtResInt = ResultType.Result_OK;
                                        parametersOutTemp["rdetail"] = Convert.ToInt32(rtResDetail);
                                    }

                                    if (rtResInt == ResultType.Result_OK)
                                    {
                                        if (iParkingData == 0 ||
                                            (wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_is_remote_extension", iExtra, iParkingData)] != null &&
                                             Convert.ToInt32(wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_is_remote_extension", iExtra, iParkingData)]) == 0))
                                        {
                                            string strQPlusVATQsTemp = "";
                                            string sAuthIdTemp = "";
                                            decimal? dBonMltTemp = null;
                                            string sVehicleTypeTemp = null;

                                            rtResInt = StandardQueryParkingComputeOutput(string.Format("extras_extradata_{0}_parkingdata_{1}_", iExtra, iParkingData),
                                                                                            iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, oStreetSection, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, sExtStreetSectionId, strExtTariffId, 
                                                                                            bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                                                                                            iCampaingFreeMinutes, iCampaingMinimumTimeToApply, 
                                                                                            sCulture, ref wsParameters, ref parametersOutTemp, ref strQPlusVATQsTemp,
                                                                                            out sAuthIdTemp, out dBonMltTemp, out dBonExtMlt, out sVehicleTypeTemp,ref iCampaingAmountToSubstract);
                                            if (iParkingData == 0 && !string.IsNullOrEmpty(strQPlusVATQsTemp))
                                                strQPlusVATQs += "~" + strQPlusVATQsTemp;
                                        }
                                    }
                                    else
                                    {
                                        parametersOutTemp["r"] = Convert.ToInt32(rtResInt);
                                    }
                                    oLstParametersOutTemp.Add(parametersOutTemp);
                                }
                                
                                iParkingData += 1;
                            }
                            while (!bExit);

                            oExtraParametersOut.Add(iExtra + 2, oLstParametersOutTemp);
                        }
                        if (iNumExtras > 0)
                        {
                            parametersOut["numextras"] = iNumExtras;
                            //parametersOut["extras"] = "";
                        }
                        
                    }

                }



            }
            catch (Exception e)
            {

                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "StandardQueryParkingTimeSteps::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }



        private ResultType StandardQueryParkingComputeOutput(string strXMLPrefix, int iWSNumber, USER oUser, string strPlate, List<string> oAdditionalPlates, DateTime dtParkQuery, GROUP oGroup, STREET_SECTION oStreetSection,
                                                    TARIFF oTariff, bool bWithSteps, int? iMaxAmountAllowedToPay, double dChangeToApply,
                                                    string strExtGroupId, string sExtStreetSectionId, string strExtTariffId, bool bIsShopKeeperOperation, bool bApplyCampaingDiscount, decimal? dApplyCampaingDiscount,
                                                    int? iCampaingFreeMinutes , int? iCampaingMinimumTimeToApply,
                                                    string culture, ref SortedList wsParameters, ref SortedList parametersOut, ref string strQPlusVATQs,
                                                    out string sAuthId, out decimal? dBonMlt, out decimal? dBonExtMlt, out string sVehicleType, ref int? iCampaingAmountToSubstract)
        {

            ResultType rtRes = ResultType.Result_OK;

            sAuthId = "";
            dBonMlt = null;
            dBonExtMlt = null;
            sVehicleType = null;
            bool bAddStep = true;


            try
            {
                parametersOut["r"] = Convert.ToInt32(rtRes);

                if ((wsParameters[strXMLPrefix + "tar_id"].ToString() != strExtTariffId) || 
                    ((wsParameters.ContainsKey(strXMLPrefix + "grp_id")) && (wsParameters[strXMLPrefix + "grp_id"].ToString() != strExtGroupId)))
                {

                    decimal? dGroupId = null;
                    decimal? dTariffId = null;

                    string strIntExtGroupId = strExtGroupId;
                    string strIntExtTariffId = strExtTariffId;

                    if (wsParameters.ContainsKey(strXMLPrefix + "grp_id"))
                    {
                        strIntExtGroupId = wsParameters[strXMLPrefix + "grp_id"].ToString();
                    }

                    strIntExtTariffId = wsParameters[strXMLPrefix + "tar_id"].ToString();

                    if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(iWSNumber, dtParkQuery, oGroup.INSTALLATION, strIntExtGroupId, strIntExtTariffId, ref dGroupId, ref dTariffId))
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("StandardQueryParkingComputeOutput::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                        return rtRes;
                    }

                    if (!dGroupId.HasValue)
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("StandardQueryParkingComputeOutput::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                        return rtRes;
                    }

                    if (!dTariffId.HasValue)
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("StandardQueryParkingComputeOutput::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                        return rtRes;
                    }

                    parametersOut["g"] = dGroupId.Value;
                    parametersOut["ad"] = dTariffId.Value;
                }
                else
                {
                    parametersOut["g"] = oGroup.GRP_ID;
                    parametersOut["ad"] = oTariff.TAR_ID;
                }

                if (oStreetSection != null)
                {
                    if (wsParameters.ContainsKey(strXMLPrefix + "stse_id") && wsParameters[strXMLPrefix + "stse_id"].ToString() != sExtStreetSectionId)
                    {
                        decimal? dStreetSectionId = null;                        

                        string sIntExtStreetSectionId = wsParameters[strXMLPrefix + "grp_id"].ToString();

                        if (!geograficAndTariffsRepository.GetStreetSectionFromExternalId(oGroup.INSTALLATION.INS_ID, sIntExtStreetSectionId, ref dStreetSectionId))
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("StandardQueryParkingComputeOutput::GetStreetSectionExternalTranslation Error", LogLevels.logERROR);
                            return rtRes;
                        }

                        if (!dStreetSectionId.HasValue)
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("StandardQueryParkingComputeOutput::GetStreetSectionExternalTranslation Error", LogLevels.logERROR);
                            return rtRes;
                        }

                        parametersOut["sts"] = dStreetSectionId.Value;
                    }
                    else
                    {
                        parametersOut["sts"] = oStreetSection.STRSE_ID;
                    }
                }

                if (wsParameters.ContainsKey(strXMLPrefix + "free_time_tariff"))
                {
                    parametersOut["free_time_tariff"] = wsParameters[strXMLPrefix + "free_time_tariff"].ToString();
                }

                parametersOut["q1"] = wsParameters[strXMLPrefix + "a_min"];
                parametersOut["q2"] = wsParameters[strXMLPrefix + "a_max"];
                parametersOut["t1"] = wsParameters[strXMLPrefix + "t_min"];
                parametersOut["t2"] = wsParameters[strXMLPrefix + "t_max"];
                parametersOut["o"] = wsParameters[strXMLPrefix + "op_type"];
                parametersOut["at"] = wsParameters[strXMLPrefix + "t_acum"];
                parametersOut["aq"] = wsParameters[strXMLPrefix + "a_acum"];
                parametersOut["cur"] = oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE;
                parametersOut["postpay"] = "0";
                parametersOut["notrefundwarning"] = "0";
                if (wsParameters.ContainsKey("postpay"))
                {
                    parametersOut["postpay"] = wsParameters[strXMLPrefix + "postpay"];
                }

                if (wsParameters.ContainsKey("notrefundwarning"))
                {
                    parametersOut["notrefundwarning"] = wsParameters[strXMLPrefix + "notrefundwarning"];
                }

                DateTime dt = DateTime.ParseExact(wsParameters[strXMLPrefix + "d_init"].ToString(), "HHmmssddMMyyyy",
                            CultureInfo.InvariantCulture);
                parametersOut["di"] = dt.ToString("HHmmssddMMyy");


                
                //TO DO: Se agrega bShowOutOfRateTime
                parametersOut["NoticeChargesNow"] = oTariff.TAR_NOTICE_CHARGES_NOW;
                String sLiteral = string.Empty;

                int iAcumTime = 0;

                try
                {
                    iAcumTime = Convert.ToInt32(parametersOut["at"].ToString());
                }
                catch{}


                if (oTariff.TAR_NOTICE_CHARGES_NOW != (int)NoticChargesNow.NotShowMessage && ((Convert.ToInt32(parametersOut["o"])) == (int)ChargeOperationsType.ParkingOperation && dtParkQuery < dt && iAcumTime==0))
                {
                    string sCulture = (!string.IsNullOrEmpty(culture) ? culture : "en-US");
                    if (oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.HasValue)
                    {
                        sLiteral = infraestructureRepository.GetLiteral(oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.Value, culture);
                        sLiteral = sLiteral.Replace("#", dt.ToShortTimeString() + " | " + dt.ToShortDateString());
                    }
                }
                else
                {
                    parametersOut["NoticeChargesNow"] = (int)NoticChargesNow.NotShowMessage;
                }
                parametersOut["NoticeChargesNowLiteral"] = sLiteral;

                //bool bShowOutOfRateTime = ((Convert.ToInt32(parametersOut["o"])) == (int)ChargeOperationsType.ParkingOperation && dtParkQuery < dt);
                //parametersOut["showOutOfRateTimeMsg"] = (bShowOutOfRateTime ? 1 : 0);

                double dChangeFee = 0;
                int iQChange = 0;

                if (oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                {
                    iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q1"]),
                                    dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                    parametersOut["qch1"] = iQChange.ToString();
                    iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q2"]),
                                    dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                    parametersOut["qch2"] = iQChange.ToString();

                }

                if (wsParameters.ContainsKey(strXMLPrefix + "auth_id") && wsParameters[strXMLPrefix + "auth_id"] != null)
                {
                    sAuthId = wsParameters[strXMLPrefix + "auth_id"].ToString();
                }

                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                string sBonMlt = "";
                try
                {
                    sBonMlt = wsParameters[strXMLPrefix + "bon_mlt"].ToString();
                    if (sBonMlt.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                    decimal dTryBonMlt = Convert.ToDecimal(sBonMlt, numberFormatProvider);
                    dBonMlt = dTryBonMlt;
                }
                catch
                {
                    dBonMlt = 1;
                }

                string sBonExtMlt = "";
                try
                {
                    sBonExtMlt = wsParameters[strXMLPrefix + "bonext_mlt"].ToString();
                    if (sBonExtMlt.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                    decimal dTryBonExtMlt = Convert.ToDecimal(sBonExtMlt, numberFormatProvider);
                    dBonExtMlt = dTryBonExtMlt;
                }
                catch
                {
                    //TO DO: MARIU Sino que voy a devolver aqui?
                    //dBonExtMlt = 1;
                }

                int iNumDisplayTexts = 0;
                try
                {
                    iNumDisplayTexts = Convert.ToInt32(wsParameters[strXMLPrefix + "numdisplaytexts"].ToString());
                }
                catch
                {
                    iNumDisplayTexts = 0;
                }
                if (iNumDisplayTexts > 0)
                {
                    try
                    {
                        string sIsoLang = "";
                        for (int i = 0; i < iNumDisplayTexts; i++)
                        {
                            sIsoLang = wsParameters[strXMLPrefix + string.Format("displaytexts_displaytext_{0}_isolang", i)].ToString();
                            if (sIsoLang == "es")
                            {
                                sVehicleType = wsParameters[strXMLPrefix + string.Format("displaytexts_displaytext_{0}_text", i)].ToString();
                                sVehicleType = sVehicleType.Replace("Tipo de vehículo", "").Trim();
                                break;
                            }
                        }
                    }
                    catch
                    {
                        Logger_AddLogMessage("StandardQueryParkingComputeOutput::Parsing Vehicle Type Error", LogLevels.logERROR);
                        sVehicleType = null;
                    }
                }

                if (bWithSteps)
                {
                    int iNumSteps = Convert.ToInt32(wsParameters[strXMLPrefix + "num_steps"]);
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    sb2.Append(strQPlusVATQs);

                    ChargeOperationsType oOperationType = (Convert.ToInt32(parametersOut["o"]) == 1 ? ChargeOperationsType.ParkingOperation : ChargeOperationsType.ExtensionOperation);

                    int iTime = 0;
                    int iQ = 0;
                    int iQWithoutBon = 0;
                    int iQWithoutBonChange = 0;
                    int iQFEE = 0;
                    decimal dQFEE = 0;
                    int iQFEEChange = 0;
                    int iQVAT = 0;
                    int iQVATChange = 0;
                    int iQTotal = 0;
                    int iQTotalChange = 0;
                    int iQSubTotal = 0;
                    int iQSubTotalChange = 0;
                    int iQPlusIVA = 0;
                    int iQPlusIVAChange = 0;
                    int iFeePlusIVA = 0;
                    int iFeePlusIVAChange = 0;

                    decimal dVAT1=0;
                    decimal dVAT2=0;
                    int iPartialVAT1=0;
                    decimal dPercFEE=0;
                    decimal dPercFEETopped=0;
                    int iPartialPercFEE=0;
                    decimal dFixedFEE=0;
                    int iPartialFixedFEE=0;
                    int iPartialPercFEEVAT=0;
                    int iPartialFixedFEEVAT=0;

                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    int? iTariffType = null;
                    IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                    if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                    {
                        iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                        iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                    }
                    if (oTariff != null)
                        iTariffType = oTariff.TAR_TYPE;
                    if (!customersRepository.GetFinantialParams(oUser, oGroup.INSTALLATION.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, oOperationType, iTariffType,
                                                                out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode ))
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("StandardQueryParkingTimeSteps::Error getting finantial parameters", LogLevels.logERROR);
                    }

/*if (oTariff.TAR_TYPE != (int)TariffType.RegularTariff)
{
    dPercFEE=0;
    dPercFEETopped=0;
    dFixedFEE=0;
}*/


                    List<StandardQueryParkingStep> oSteps = new List<StandardQueryParkingStep>();

                    if (rtRes == ResultType.Result_OK)
                    {

                        DateTime? dtNow = geograficAndTariffsRepository.getInstallationDateTime(oGroup.GRP_INS_ID) ?? DateTime.Now;
                        string strTodayColour = infraestructureRepository.GetParameterValue("ParkingScreenTodayColour");
                        string strTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenTomorrowColour");
                        string strMoreThanTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenMoreThanTomorrowColour");
                        decimal dShopKeeperParkPerc = oGroup.INSTALLATION.INSTALLATION_SHOPKEEPER_PARAMETERs.FirstOrDefault() == null ? (decimal)0 : oGroup.INSTALLATION.INSTALLATION_SHOPKEEPER_PARAMETERs.First().INSSHO_PARK_PROFIT_PERC;
                        int iAmountTotal = 0;
                        int iAmountProfit = 0;
                        decimal dINSCurID = oGroup.INSTALLATION.INS_CUR_ID;
                        decimal dUserCurID = oUser.USR_CUR_ID;
                        int iNumSelectableSteps = 0;
                        for (int i = 0; i < iNumSteps; i++)
                        {
                            dt = DateTime.ParseExact(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_d", i)].ToString(), "HHmmssddMMyyyy",
                                    CultureInfo.InvariantCulture);

                            int iTotalDays = Convert.ToInt32((dt.Date - dtNow.Value.Date).TotalDays);
                            if (iTotalDays < 0)
                                iTotalDays = 0;

                            iTime = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)]);

                            iQ = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_a", i)].ToString());
                            
                            if (wsParameters[strXMLPrefix + string.Format("steps_step_{0}_a_without_bon", i)] != null)
                                iQWithoutBon = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_a_without_bon", i)].ToString());
                            else
                                iQWithoutBon = iQ;

                            if ((bApplyCampaingDiscount) && (iCampaingFreeMinutes.HasValue) && (!iCampaingAmountToSubstract.HasValue) &&
                                (Convert.ToInt32(parametersOut["o"]) == (int)ChargeOperationsType.ParkingOperation) && (iQ>0))
                            {
                                if (iTime >= iCampaingFreeMinutes.Value)
                                {
                                    iCampaingAmountToSubstract = iQ;
                                }



                            }


                            if ((bApplyCampaingDiscount)&&(iCampaingAmountToSubstract.HasValue)&&(iCampaingMinimumTimeToApply.HasValue)&& 
                                (Convert.ToInt32(parametersOut["o"]) == (int)ChargeOperationsType.ParkingOperation))
                            {
                                if (iTime>= iCampaingMinimumTimeToApply)
                                {
                                    iQ -= iCampaingAmountToSubstract.Value;

                                    if (iQ<0)
                                    {
                                        break;
                                    }
                                }



                            }

                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                int iPartialVAT1Temp = 0;
                                int iPartialPercFEETemp = 0;
                                int iPartialFixedFEETemp = 0;
                                int iPartialPercFEEVATTemp = 0;
                                int iPartialFixedFEEVATTemp = 0;


                                int iQWithoutBonTotal = customersRepository.CalculateFEE(ref iQWithoutBon, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                            out iPartialVAT1Temp, out iPartialPercFEETemp, out iPartialFixedFEETemp,
                                                                            out iPartialPercFEEVATTemp, out iPartialFixedFEEVATTemp);
                            }
                            
                            int iFreeTimeUsed = 0;
                            int iRealQ = iQ;
                            if (wsParameters.ContainsKey(string.Format("steps_step_{0}_free_time_used", i)))
                            {
                                iFreeTimeUsed = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_free_time_used", i)].ToString());
                            }

                            
                            /*
                            iPercFEETopped, iFixedFEE, iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iPartialPercFEEVAT, iPartialFixedFEEVAT
                            "0                  30      130                 0               34                  0                   4
                             */

                            if (iQ > 0)
                            {
                                iQTotal = customersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE,eTaxMode,
                                                                            out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                            out iPartialPercFEEVAT, out iPartialFixedFEEVAT);                               

                                dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                                if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                                dQFEE += dFixedFEE;
                                iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));
                                
                                                               
                                iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                                iQSubTotal = iQ + iQFEE;
                                iQPlusIVA = iQ + iPartialVAT1;
                                iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;
                            }
                            else
                            {
                                iQTotal = iQ;
                                iQFEE = 0;
                                iQVAT = 0;
                                iQSubTotal = iQ;
                                iQPlusIVA = 0;
                                iFeePlusIVA = 0;
                            }

                            if (iQ != 0)
                            {
                                var oLastStep = oSteps.LastOrDefault();
                                if (oLastStep != null && oLastStep.Q == iQ && oLastStep.Time == iTime)
                                {
                                    //Logger_AddLogMessage(string.Format("StandardQueryParkingTimeSteps:: removing step id='{0}' amount={1} time={2}", i, iQ, oLastStep.Time), LogLevels.logINFO);
                                    oSteps.RemoveAt(oSteps.Count - 1);                                    
                                }
                            }

                            if (i > 0)
                            {
                                sb2.Append("|");
                            }
                            sb2.AppendFormat("{0};{1}", iQ, iQPlusIVA);


                            if (wsParameters.ContainsKey(string.Format("steps_step_{0}_real_a", i)))
                            {
                                if (string.IsNullOrEmpty(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_real_a", i)].ToString()))
                                {
                                    iRealQ = iQ;
                                }
                                else
                                {
                                    iRealQ = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_real_a", i)].ToString());
                                    
                                    iRealQ = Convert.ToInt32(Math.Round(iRealQ * (dBonMlt ?? 1), MidpointRounding.AwayFromZero));
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iRealQ = Convert.ToInt32(Convert.ToDecimal(iRealQ) / (1 + dVAT1));

                                    }
                                }

                            }

                            var oCurrentStep = new StandardQueryParkingStep()
                            {
                                Time = iTime,
                                Q = iQ,
                                Dt = dt,
                                QFEE = iQFEE,
                                QVAT = iQVAT,
                                QSubTotal = iQSubTotal,
                                QTotal = iQTotal,
                                FreeTimeUsed = iFreeTimeUsed,
                                RealQ = iRealQ,
                                QPlusIVA = iQPlusIVA,
                                FeePlusIVA = iFeePlusIVA,
                                QWithoutBon = iQWithoutBon,
                                BonMlt = (dBonMlt ?? 1),
                                NumDays = iTotalDays,
                                NumDaysColour = (iTotalDays == 0) ? strTodayColour : ((iTotalDays == 1) ? strTomorrowColour : strMoreThanTomorrowColour) 
                            };

                            if (wsParameters[strXMLPrefix + string.Format("steps_step_{0}_label", i)] != null)
                            {
                                oCurrentStep.Label = wsParameters[strXMLPrefix + string.Format("steps_step_{0}_label", i)].ToString();
                            }

                            if (wsParameters[strXMLPrefix + string.Format("steps_step_{0}_selectable", i)] != null)
                                oCurrentStep.Selectable = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_selectable", i)]);
                            if (wsParameters[strXMLPrefix + string.Format("steps_step_{0}_default", i)] != null)
                                oCurrentStep.Default = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_default", i)]);

                            parametersOut["dexp"] = dt.ToString("HHmmssddMMyy");

                            if (dINSCurID != dUserCurID)
                            {

                                //Logger_AddLogMessage("StandardQueryParkingComputeOutput::Change 1", LogLevels.logERROR);
                                iQChange = ChangeQuantityFromInstallationCurToUserCur(iQ, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQWithoutBonChange = ChangeQuantityFromInstallationCurToUserCur(iQWithoutBon, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);

                                iQFEEChange = ChangeQuantityFromInstallationCurToUserCur(iQFEE, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQVATChange = ChangeQuantityFromInstallationCurToUserCur(iQVAT, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQSubTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQSubTotal, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQTotal, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                //Logger_AddLogMessage("StandardQueryParkingComputeOutput::Change 2", LogLevels.logERROR);
                                if (iMaxAmountAllowedToPay.HasValue)
                                {
                                    if (iQTotalChange > iMaxAmountAllowedToPay)
                                    {
                                        if (iNumSelectableSteps == 0)
                                        {
                                            rtRes = ResultType.Result_Error_Not_Enough_Balance;
                                            parametersOut["r"] = Convert.ToInt32(rtRes);
                                            break;
                                        }
                                        bAddStep = false;
                                    }
                                }

                                //if (oGroup.INSTALLATION.INS_SHORTDESC != "SLP")
                                //{
                                if (bAddStep)
                                {
                                    if (oCurrentStep.Selectable == 1)
                                        iNumSelectableSteps++;

                                    if (bIsShopKeeperOperation)
                                    {

                                        customersRepository.CalculateShopKeeperProfit(iQTotalChange, dShopKeeperParkPerc, out iAmountProfit, out iAmountTotal);
                                        /*sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><qch>{2}</qch><d>{3:HHmmssddMMyy}</d><q_fee>{4}</q_fee><q_vat>{5}</q_vat><q_subtotal>{6}</q_subtotal><q_total>{7}</q_total>" +
                                                                    "<qch_fee>{8}</qch_fee><qch_vat>{15}</qch_vat><qch_subtotal>{9}</qch_subtotal><qch_total>{10}</qch_total><time_bal_used>{11}</time_bal_used><real_q>{12}</real_q>" +
                                                                    "<q_shopkeeper_profit>{13}</q_shopkeeper_profit><q_shopkeeper>{14}</q_shopkeeper>" +
                                                                    "<q_plus_vat>{16}</q_plus_vat><q_fee_plus_vat>{17}</q_fee_plus_vat><qch_plus_vat>{18}</qch_plus_vat><qch_fee_plus_vat>{19}</qch_fee_plus_vat>" +
                                                                    "<q_without_bon>{20}</q_without_bon><qch_without_bon>{21}</qch_without_bon><per_bon>{22}</per_bon></step>",
                                                                wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)].ToString(),
                                                                iQ,
                                                                iQChange,
                                                                dt,
                                                                iQFEE, iQVAT, iQSubTotal, iQTotal,
                                                                iQFEEChange, iQSubTotalChange, iQTotalChange, iFreeTimeUsed, iRealQ, iAmountProfit, iAmountTotal, iQVATChange, iQPlusIVA, iFeePlusIVA, iQPlusIVAChange, iFeePlusIVAChange,
                                                                iQWithoutBon, iQWithoutBonChange, (dBonMlt ?? 1));*/

                                        oCurrentStep.QChange = iQChange;
                                        oCurrentStep.QFEEChange = iQFEEChange;
                                        oCurrentStep.QSubTotalChange = iQSubTotalChange;
                                        oCurrentStep.QTotalChange = iQTotalChange;
                                        oCurrentStep.AmountProfit = iAmountProfit;
                                        oCurrentStep.AmountTotal = iAmountTotal;
                                        oCurrentStep.QVATChange = iQVATChange;
                                        oCurrentStep.QPlusIVAChange = iQPlusIVAChange;
                                        oCurrentStep.FeePlusIVAChange = iFeePlusIVAChange;
                                        oCurrentStep.QWithoutBonChange = iQWithoutBonChange;

                                    }
                                    else
                                    {

                                        /*sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><qch>{2}</qch><d>{3:HHmmssddMMyy}</d><q_fee>{4}</q_fee><q_vat>{5}</q_vat><q_subtotal>{6}</q_subtotal><q_total>{7}</q_total>" +
                                                                    "<qch_fee>{8}</qch_fee><qch_vat>{13}</qch_vat><qch_subtotal>{9}</qch_subtotal><qch_total>{10}</qch_total><time_bal_used>{11}</time_bal_used><real_q>{12}</real_q>"+
                                                                    "<q_plus_vat>{14}</q_plus_vat><q_fee_plus_vat>{15}</q_fee_plus_vat><qch_plus_vat>{16}</qch_plus_vat><qch_fee_plus_vat>{17}</qch_fee_plus_vat>" +
                                                                    "<q_without_bon>{18}</q_without_bon><qch_without_bon>{19}</qch_without_bon><per_bon>{20}</per_bon></step>",
                                                                    wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)].ToString(),
                                                                    iQ,
                                                                    iQChange,
                                                                    dt,
                                                                    iQFEE, iQVAT, iQSubTotal, iQTotal,
                                                                    iQFEEChange, iQSubTotalChange, iQTotalChange, iFreeTimeUsed, iRealQ, iQVATChange, iQPlusIVA, iFeePlusIVA, iQPlusIVAChange, iFeePlusIVAChange,
                                                                    iQWithoutBon, iQWithoutBonChange, (dBonMlt ?? 1));*/
                                        oCurrentStep.QChange = iQChange;
                                        oCurrentStep.QFEEChange = iQFEEChange;
                                        oCurrentStep.QSubTotalChange = iQSubTotalChange;
                                        oCurrentStep.QTotalChange = iQTotalChange;
                                        oCurrentStep.QVATChange = iQVATChange;
                                        oCurrentStep.QPlusIVAChange = iQPlusIVAChange;
                                        oCurrentStep.FeePlusIVAChange = iFeePlusIVAChange;
                                        oCurrentStep.QWithoutBonChange = iQWithoutBonChange;
                                    }
                                    parametersOut["q2"] = iQ;
                                    parametersOut["t2"] = wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)];
                                }


                                //Logger_AddLogMessage("StandardQueryParkingComputeOutput::Change 3", LogLevels.logERROR);

                            }
                            else
                            {


                                //if (oGroup.INSTALLATION.INS_SHORTDESC != "SLP")
                                //{
                                if (iMaxAmountAllowedToPay.HasValue)
                                {      
                                    
                                    if (iQTotal > iMaxAmountAllowedToPay)
                                    {
                                        if (iNumSelectableSteps == 0)
                                        {
                                            rtRes = ResultType.Result_Error_Not_Enough_Balance; ;
                                            parametersOut["r"] = Convert.ToInt32(rtRes);
                                            break;
                                        }
                                        bAddStep = false;
                                    }
                                }

                                if (bAddStep)
                                {
                                    if (oCurrentStep.Selectable == 1)
                                        iNumSelectableSteps++;

                                    if (bIsShopKeeperOperation)
                                    {

                                        customersRepository.CalculateShopKeeperProfit(iQTotal, dShopKeeperParkPerc, out iAmountProfit, out iAmountTotal);
                                        /*sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><d>{2:HHmmssddMMyy}</d><q_fee>{3}</q_fee><q_vat>{4}</q_vat><q_subtotal>{5}</q_subtotal><q_total>{6}</q_total><time_bal_used>{7}</time_bal_used><real_q>{8}</real_q>" +
                                                                    "<q_shopkeeper_profit>{9}</q_shopkeeper_profit><q_shopkeeper>{10}</q_shopkeeper><q_plus_vat>{11}</q_plus_vat><q_fee_plus_vat>{12}</q_fee_plus_vat>" +
                                                                    "<q_without_bon>{13}</q_without_bon><per_bon>{14}</per_bon></step>",
                                                                    wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)].ToString(),
                                                                    iQ,
                                                                    dt,
                                                                    iQFEE, iQVAT, iQSubTotal, iQTotal, iFreeTimeUsed, iRealQ, iAmountProfit, iAmountTotal, iQPlusIVA, iFeePlusIVA,
                                                                    iQWithoutBon, (dBonMlt ?? 1));*/
                                        oCurrentStep.AmountProfit = iAmountProfit;
                                        oCurrentStep.AmountTotal = iAmountTotal;
                                    }
                                    /*else
                                    {



                                        sb.AppendFormat("<step json:Array='true'><t>{0}</t><q>{1}</q><d>{2:HHmmssddMMyy}</d><q_fee>{3}</q_fee><q_vat>{4}</q_vat><q_subtotal>{5}</q_subtotal><q_total>{6}</q_total><time_bal_used>{7}</time_bal_used><real_q>{8}</real_q>" +
                                                                    "<q_plus_vat>{9}</q_plus_vat><q_fee_plus_vat>{10}</q_fee_plus_vat>" +
                                                                    "<q_without_bon>{11}</q_without_bon><per_bon>{12}</per_bon></step>",
                                                                    wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)].ToString(),
                                                                    iQ,
                                                                    dt,
                                                                    iQFEE, iQVAT, iQSubTotal, iQTotal, iFreeTimeUsed, iRealQ, iQPlusIVA, iFeePlusIVA,
                                                                    iQWithoutBon, (dBonMlt ?? 1));
                                    }*/


                                    parametersOut["q2"] = iQ;
                                    parametersOut["t2"] = wsParameters[strXMLPrefix + string.Format("steps_step_{0}_t", i)];

                                }
                                /*}
                                else
                                {
                                    // *** New tags for loyout 3 ***
                                    strSteps += string.Format("<step json:Array='true'><t>{0}</t><q>{1}</q><d>{2:HHmmssddMMyy}</d><q_fee>{3}</q_fee><q_vat>{4}</q_vat><q_subtotal>{5}</q_subtotal><q_total>{6}</q_total><qbonusam>{7}</qbonusam></step>",
                                                            wsParameters[strXMLPrefix+string.Format("steps_step_{0}_t", i)].ToString(),
                                                            wsParameters[strXMLPrefix+string.Format("steps_step_{0}_a", i)].ToString(),
                                                            dt,
                                                            iQFEE, dQVAT, iQSubTotal, iQTotal,
                                                            Convert.ToInt32(Math.Round(iQ * 0.1, MidpointRounding.AwayFromZero)));
                                }*/
                            }


                            if (bAddStep)
                            {
                                oSteps.Add(oCurrentStep);
                            }
                            else
                            {
                                if (oCurrentStep.Default!=0)
                                {
                                    if (oSteps.Count() > 0)
                                    {
                                        oSteps.Last().Default = 1;
                                    }
                                }                                   
                            }
                        }

                       
                    }

                    

                    if (oSteps.Any())
                    {
                        //parametersOut["steps"] = sb.ToString();
                        var oStandardQueryParkingSteps = new StandardQueryParkingSteps() { Steps = oSteps.ToArray() };
                        parametersOut["steps"] = oStandardQueryParkingSteps.ToCustomXml();
                    }
                    else
                        parametersOut["steps"] = "";
                    strQPlusVATQs = sb2.ToString();
                }

                if (wsParameters.ContainsKey("d_exp"))
                {
                    DateTime dtExpirationDate = DateTime.ParseExact(wsParameters[strXMLPrefix + "d_exp"].ToString(), "HHmmssddMMyyyy",
                             CultureInfo.InvariantCulture);
                    parametersOut["dexp"] = dtExpirationDate.ToString("HHmmssddMMyy");
                }

                if (wsParameters[strXMLPrefix + "num_buttons"] != null)
                {                    
                    int iNumButtons = Convert.ToInt32(wsParameters[strXMLPrefix + "num_buttons"]);
                    if (iNumButtons > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        for (int i = 0; i < iNumButtons; i++)
                        {
                            sb.AppendFormat("<button json:Array='true'><id>{0}</id><btntype>{1}</btntype><min>{2}</min></button>",
                                                                wsParameters[strXMLPrefix + string.Format("buttons_button_{0}_id", i)].ToString(),
                                                                Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("buttons_button_{0}_btntype", i)]),
                                                                Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("buttons_button_{0}_min", i)]));
                        }

                        parametersOut["buttons"] = sb.ToString();
                    }


                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardQueryParkingComputeOutput::Exception", LogLevels.logERROR);

            }


            return rtRes;

        }



        public ResultType StandardQueryParkingAmountSteps(int iWSNumber, USER oUser, string strPlate, List<string> oAdditionalPlates, Dictionary<int, List<string>> oExtraPlates, DateTime dtParkQuery, GROUP oGroup, STREET_SECTION oStreetSection, TARIFF oTariff, bool bWithSteps,
                                                          int? iMaxAmountAllowedToPay, double dChangeToApply, bool bIsShopKeeperOperation, bool bApplyCampaingDiscount, decimal? dApplyCampaingDiscount,
                                                          int? iCampaingFreeMinutes, int? iCampaingMinimumTimeToApply, string culture, int? iWSTimeout, ref SortedList parametersOut,
                                                          ref List<SortedList> oAdditionals, out string strQPlusVATQs, out string sAuthId, out decimal? dBonMlt, out decimal? dBonExtMlt, out string sVehicleType, ref int? iCampaingAmountToSubstract,
                                                          ref Dictionary<int,List<SortedList>> oExtraParametersOut, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            strQPlusVATQs = "";
            sAuthId = "";
            dBonMlt = null;
            dBonExtMlt = null;
            sVehicleType = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            ResultType rtResDetail = ResultType.Result_OK;


            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                StandardParkingWS.TariffComputerWS oParkWS = new StandardParkingWS.TariffComputerWS();
                oParkWS.Url = oGroup.INSTALLATION.INS_PARK_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new System.Net.NetworkCredential(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER, oGroup.INSTALLATION.INS_PARK_WS_HTTP_PASSWORD);
                }

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oGroup.INSTALLATION.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";

                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, oGroup, oTariff, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("StandardQueryParkingAmountSteps::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sExtStreetSectionId = (oStreetSection != null ? oStreetSection.STRSE_ID_EXT : "");

                string sPlatesValues = strPlate;
                string sPlatesTags = string.Format("<lic_pla>{0}</lic_pla>", strPlate);
                if (oAdditionalPlates != null)
                {
                    for (int i = 0; i < oAdditionalPlates.Count; i += 1)
                    {
                        sPlatesValues += oAdditionalPlates[i];
                        sPlatesTags += string.Format("<lic_pla{0}>{1}</lic_pla{0}>", i + 2, oAdditionalPlates[i]);
                    }
                }
                if (oExtraPlates != null)
                {
                    foreach (int iExtra in oExtraPlates.Keys)
                    {
                        for (int i = 0; i < oExtraPlates[iExtra].Count; i += 1)
                        {
                            sPlatesValues += oExtraPlates[iExtra][i];
                            sPlatesTags += string.Format("<lic_pla{0}_{1}>{2}</lic_pla{0}_{1}>", iExtra, i + 1, oExtraPlates[iExtra][i]);
                        }
                    }
                }
                
                string sApplyCampaingDiscount =string.Empty;
                if(dApplyCampaingDiscount.HasValue)
                {
                    dApplyCampaingDiscount = dApplyCampaingDiscount.Value;
                    sApplyCampaingDiscount = dApplyCampaingDiscount.Value.ToString(CultureInfo.InvariantCulture);
                }
                
                if (!bWithSteps)
                {
                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}{10}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><bon_mlt>{10}</bon_mlt><ah>{11}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount, strAuthHash);
                    }
                    else
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, strAuthHash);
 
                    }
                }
                else
                {
                    int? iAmountOffSet = DEFAULT_AMOUNT_STEP; //minutes
                    geograficAndTariffsRepository.GetGroupAndTariffStepOffsetMinutes(oGroup, oTariff, out iAmountOffSet);

                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}{10}{11}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iAmountOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><amou_off>{6}</amou_off><prov>{7}</prov><ext_acc>{8}</ext_acc><free_time>{9}</free_time><vers>{10}</vers><bon_mlt>{11}</bon_mlt><ah>{12}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iAmountOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount, strAuthHash);
                    }
                    else
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}{10}", strCityID, sPlatesValues, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iAmountOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));

                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><stse_id>{4}</stse_id><tar_id>{5}</tar_id><amou_off>{6}</amou_off><prov>{7}</prov><ext_acc>{8}</ext_acc><free_time>{9}</free_time><vers>{10}</vers><ah>{11}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, sExtStreetSectionId, strExtTariffId, iAmountOffSet, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, strAuthHash);
                    }


                }

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps Timeout={1} xmlIn={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                string strOut = "";
                watch = Stopwatch.StartNew();
                if (!bWithSteps)
                {
                    strOut = oParkWS.QueryParkingOperation(strMessage);
                }
                else
                {
                    strOut = oParkWS.QueryParkingOperationWithAmountSteps(strMessage);
                }
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (Convert.ToInt32(rtRes) > Convert.ToInt32(ResultType.Result_OK))
                    {
                        rtResDetail = rtRes;
                        rtRes = ResultType.Result_OK;
                        parametersOut["rdetail"] = Convert.ToInt32(rtResDetail);
                    }

                    if (rtRes == ResultType.Result_OK)
                    {
                        rtRes = StandardQueryParkingComputeOutput("", iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, oStreetSection, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, 
                            strExtGroupId, sExtStreetSectionId, strExtTariffId, bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                            iCampaingFreeMinutes, iCampaingMinimumTimeToApply,
                            culture, ref wsParameters, ref parametersOut, ref strQPlusVATQs, out sAuthId, out dBonMlt, out dBonExtMlt, out sVehicleType, ref iCampaingAmountToSubstract);
                    }
                    else
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    parametersOut["numadditionals"] = 0;

                    if (wsParameters["numadditionals"] != null && Convert.ToInt32(wsParameters["numadditionals"]) > 0)
                    {
                        int iNumAdditionals = 0;
                        int i = 0;
                        bool bExit = false;
                        ResultType rtResInt;

                        do
                        {
                            bExit = (wsParameters[string.Format("additionals_parkingdata_{0}_r", i)] == null);

                            if (!bExit)
                            {
                                rtResInt = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters[string.Format("additionals_parkingdata_{0}_r", i)].ToString()));

                                rtResDetail = ResultType.Result_OK;
                                SortedList parametersOutTemp = new SortedList();

                                if (Convert.ToInt32(rtResInt) > Convert.ToInt32(ResultType.Result_OK))
                                {
                                    rtResDetail = rtResInt;
                                    rtResInt = ResultType.Result_OK;
                                    parametersOutTemp["rdetail"] = Convert.ToInt32(rtResDetail);
                                }


                                if (rtResInt == ResultType.Result_OK)
                                {

                                    if (wsParameters[string.Format("additionals_parkingdata_{0}_is_remote_extension", i)] != null &&
                                        Convert.ToInt32(wsParameters[string.Format("additionals_parkingdata_{0}_is_remote_extension", i)]) == 0)
                                    {
                                        string strQPlusVATQsTemp = "";
                                        string sAuthIdTemp = "";
                                        decimal? dBonMltTemp = null;
                                        string sVehicleTypeTemp = null;

                                        rtResInt = StandardQueryParkingComputeOutput(string.Format("additionals_parkingdata_{0}_", i),
                                                                                        iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, oStreetSection, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, sExtStreetSectionId, 
                                                                                        strExtTariffId, bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                                                                                        iCampaingFreeMinutes, iCampaingMinimumTimeToApply, culture, ref wsParameters, ref parametersOutTemp, ref strQPlusVATQsTemp,
                                                                                        out sAuthIdTemp, out dBonMltTemp, out dBonExtMlt, out sVehicleTypeTemp, ref iCampaingAmountToSubstract);


                                        if (rtResInt == ResultType.Result_OK)
                                        {
                                            oAdditionals.Add(parametersOutTemp);
                                            iNumAdditionals++;
                                        }

                                    }
                                }
                            }
                            i++;

                        }
                        while (!bExit);

                        if (iNumAdditionals > 0)
                        {
                            parametersOut["numadditionals"] = iNumAdditionals;
                            parametersOut["additionals"] = "";


                        }
                    }

                    if (wsParameters["numextras"] != null && Convert.ToInt32(wsParameters["numextras"]) > 0)
                    {
                        int iNumExtras = Convert.ToInt32(wsParameters["numextras"]);
                        bool bExit = false;
                        ResultType rtResInt;

                        if (oExtraParametersOut == null)
                            oExtraParametersOut = new Dictionary<int, List<SortedList>>();

                        for (int iExtra = 0; iExtra < iNumExtras; iExtra += 1)
                        {
                            var oLstParametersOutTemp = new List<SortedList>();

                            int iParkingData = 0;
                            do
                            {

                                bExit = (wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_r", iExtra, iParkingData)] == null);
                                if (!bExit)
                                {
                                    SortedList parametersOutTemp = new SortedList();

                                    rtResInt = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_r", iExtra, iParkingData)].ToString()));

                                    rtResDetail = ResultType.Result_OK;

                                    if (Convert.ToInt32(rtResInt) > Convert.ToInt32(ResultType.Result_OK))
                                    {
                                        rtResDetail = rtResInt;
                                        rtResInt = ResultType.Result_OK;
                                        parametersOutTemp["rdetail"] = Convert.ToInt32(rtResDetail);
                                    }

                                    if (rtResInt == ResultType.Result_OK)
                                    {
                                        if (iParkingData == 0 ||
                                            (wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_is_remote_extension", iExtra, iParkingData)] != null &&
                                             Convert.ToInt32(wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_is_remote_extension", iExtra, iParkingData)]) == 0))
                                        {
                                            string strQPlusVATQsTemp = "";
                                            string sAuthIdTemp = "";
                                            decimal? dBonMltTemp = null;
                                            string sVehicleTypeTemp = null;

                                            rtResInt = StandardQueryParkingComputeOutput(string.Format("extras_extradata_{0}_parkingdata_{1}_", iExtra, iParkingData),
                                                                                            iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, oStreetSection, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, sExtStreetSectionId, strExtTariffId, bIsShopKeeperOperation,
                                                                                            bApplyCampaingDiscount, dApplyCampaingDiscount,
                                                                                            iCampaingFreeMinutes, iCampaingMinimumTimeToApply, culture, ref wsParameters, ref parametersOutTemp, ref strQPlusVATQsTemp,
                                                                                            out sAuthIdTemp, out dBonMltTemp, out dBonExtMlt, out sVehicleTypeTemp, ref iCampaingAmountToSubstract);
                                            if (iParkingData == 0 && !string.IsNullOrEmpty(strQPlusVATQsTemp))
                                                strQPlusVATQs += "~" + strQPlusVATQsTemp;
                                        }
                                    }
                                    else
                                    {
                                        parametersOutTemp["r"] = Convert.ToInt32(rtResInt);
                                    }
                                    oLstParametersOutTemp.Add(parametersOutTemp);
                                }

                                iParkingData += 1;
                            }
                            while (!bExit);

                            oExtraParametersOut.Add(iExtra + 2, oLstParametersOutTemp);
                        }
                        if (iNumExtras > 0)
                        {
                            parametersOut["numextras"] = iNumExtras;
                            //parametersOut["extras"] = "";
                        }

                    }

                }



            }
            catch (Exception e)
            {

                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "StandardQueryParkingAmountSteps::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }




        public ResultType BilbaoIntegrationQueryParkingAmountSteps(int iWSNumber, USER oUser, string strPlate, List<string> oAdditionalPlates, Dictionary<int, List<string>> oExtraPlates, DateTime dtParkQuery, GROUP oGroup, TARIFF oTariff, bool bWithSteps,
                                                   int? iMaxAmountAllowedToPay, double dChangeToApply, bool bIsShopKeeperOperation, bool bApplyCampaingDiscount, decimal? dApplyCampaingDiscount,
                                                   int? iCampaingFreeMinutes, int? iCampaingMinimumTimeToApply,string culture, int? iWSTimeout, ref SortedList parametersOut,
                                                   ref List<SortedList> oAdditionals, out string strQPlusVATQs, out string sAuthId, out decimal? dBonMlt, out decimal? dBonExtMlt, out string sVehicleType, ref int? iCampaingAmountToSubstract,
                                                   ref Dictionary<int, List<SortedList>> oExtraParametersOut, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            strQPlusVATQs = "";
            sAuthId = "";
            dBonMlt = null;
            dBonExtMlt = null;
            sVehicleType = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oGroup.INSTALLATION.INS_STANDARD_CITY_ID;
                string strMessage = "";
                string strAuthHash = "";

                string strExtTariffId = "";
                string strExtGroupId = "";
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, oGroup, oTariff, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("BilbaoIntegrationQueryParkingAmountSteps::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sPlatesValues = strPlate;
                string sPlatesTags = string.Format("<lic_pla>{0}</lic_pla>", strPlate);
                if (oAdditionalPlates != null)
                {
                    for (int i = 0; i < oAdditionalPlates.Count; i += 1)
                    {
                        sPlatesValues += oAdditionalPlates[i];
                        sPlatesTags += string.Format("<lic_pla{0}>{1}</lic_pla{0}>", i + 2, oAdditionalPlates[i]);
                    }
                }
                if (oExtraPlates != null)
                {
                    foreach (int iExtra in oExtraPlates.Keys)
                    {
                        for (int i = 0; i < oExtraPlates[iExtra].Count; i += 1)
                        {
                            sPlatesValues += oExtraPlates[iExtra][i];
                            sPlatesTags += string.Format("<lic_pla{0}_{1}>{2}</lic_pla{0}_{1}>", iExtra, i + 1, oExtraPlates[iExtra][i]);
                        }
                    }
                }

                string sApplyCampaingDiscount = string.Empty;
                if (dApplyCampaingDiscount.HasValue)
                {
                    dApplyCampaingDiscount = dApplyCampaingDiscount.Value;
                    sApplyCampaingDiscount = dApplyCampaingDiscount.Value.ToString(CultureInfo.InvariantCulture);
                }

                if (!bWithSteps)
                {
                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount));
                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><prov>{5}</prov><ext_acc>{6}</ext_acc><free_time>{7}</free_time><vers>{8}</vers><bon_mlt>{9}</bon_mlt><ah>{10}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount, strAuthHash);
                    }
                    else
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));
                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><prov>{5}</prov><ext_acc>{6}</ext_acc><free_time>{7}</free_time><vers>{8}</vers><ah>{9}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName,oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, strAuthHash);
                    }
                }
                else
                {
                    int? iAmountOffSet = DEFAULT_AMOUNT_STEP; //minutes
                    geograficAndTariffsRepository.GetGroupAndTariffStepOffsetMinutes(oGroup, oTariff, out iAmountOffSet);

                    if (!string.IsNullOrEmpty(sApplyCampaingDiscount))
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}{10}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, iAmountOffSet, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount));
                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><prov>{5}</prov><amou_off>{6}</amou_off><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><bon_mlt>{10}</bon_mlt><ah>{11}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, iAmountOffSet, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, sApplyCampaingDiscount, strAuthHash);
                    }
                    else
                    {
                        strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                            string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, iAmountOffSet, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers));
                        strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id>{1}<date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><prov>{5}</prov><amou_off>{6}</amou_off><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                            strCityID, sPlatesTags, dtInstallation, strExtGroupId, strExtTariffId, strCompanyName, iAmountOffSet, oUser.USR_USERNAME, oUser.USR_TIME_BALANCE, strvers, strAuthHash);
                    }
                }

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("BilbaoIntegrationQueryParkingAmountSteps xmlIn={0}", sXmlIn), LogLevels.logDEBUG);
                ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                AddTLS12Support();
                var oParkWS = new BilbaoParkWsIntegraExternalService.integraExternalServices();
                oParkWS.Url = oGroup.INSTALLATION.INS_PARK_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new System.Net.NetworkCredential(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER, oGroup.INSTALLATION.INS_PARK_WS_HTTP_PASSWORD);
                }
                string strOut = "";
                watch = Stopwatch.StartNew();
                if (!bWithSteps)
                {
                    strOut = oParkWS.QueryParkingOperations(strMessage);
                }
                else
                {
                    strOut = oParkWS.QueryParkingOperationWithAmountSteps(strMessage);
                }
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("BilbaoIntegrationQueryParkingAmountSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = Convert_ResultTypeBilbaoIntegrationParkingWS_TO_ResultType((ResultTypeBilbaoIntegrationParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {                      

                        rtRes = StandardQueryParkingComputeOutput("", iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, null, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply,
                            strExtGroupId, "", strExtTariffId, bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                            iCampaingFreeMinutes, iCampaingMinimumTimeToApply,
                            culture, ref wsParameters, ref parametersOut, ref strQPlusVATQs, out sAuthId, out dBonMlt, out dBonExtMlt, out sVehicleType, ref iCampaingAmountToSubstract);

                    }
                    else
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    parametersOut["numadditionals"] = 0;

                    if (wsParameters["numadditionals"] != null && Convert.ToInt32(wsParameters["numadditionals"]) > 0)
                    {
                        int iNumAdditionals = 0;
                        int i = 0;
                        bool bExit = false;
                        ResultType rtResInt;

                        do
                        {
                            bExit = (wsParameters[string.Format("additionals_parkingdata_{0}_r", i)] == null);

                            if (!bExit)
                            {
                                rtResInt = Convert_ResultTypeBilbaoIntegrationParkingWS_TO_ResultType((ResultTypeBilbaoIntegrationParkingWS)Convert.ToInt32(wsParameters[string.Format("additionals_parkingdata_{0}_r", i)].ToString()));

                                if (rtResInt == ResultType.Result_OK)
                                {

                                    if (wsParameters[string.Format("additionals_parkingdata_{0}_is_remote_extension", i)] != null &&
                                        Convert.ToInt32(wsParameters[string.Format("additionals_parkingdata_{0}_is_remote_extension", i)]) == 0)
                                    {
                                        SortedList parametersOutTemp = new SortedList();
                                        string strQPlusVATQsTemp = "";
                                        string sAuthIdTemp = "";
                                        decimal? dBonMltTemp = null;
                                        string sVehicleTypeTemp = null;


                                        rtResInt = StandardQueryParkingComputeOutput(string.Format("additionals_parkingdata_{0}_", i),
                                                                                        iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, null, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, "",
                                                                                        strExtTariffId, bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                                                                                        iCampaingFreeMinutes, iCampaingMinimumTimeToApply, culture, ref wsParameters, ref parametersOutTemp, ref strQPlusVATQsTemp,
                                                                                        out sAuthIdTemp, out dBonMltTemp, out dBonExtMlt, out sVehicleTypeTemp, ref iCampaingAmountToSubstract);


                                        if (rtResInt == ResultType.Result_OK)
                                        {
                                            oAdditionals.Add(parametersOutTemp);
                                            iNumAdditionals++;
                                        }
                                    }
                                }
                            }
                            i++;
                        }
                        while (!bExit);

                        if (iNumAdditionals > 0)
                        {
                            parametersOut["numadditionals"] = iNumAdditionals;
                            parametersOut["additionals"] = "";
                        }
                    }

                    if (wsParameters["numextras"] != null && Convert.ToInt32(wsParameters["numextras"]) > 0)
                    {
                        int iNumExtras = Convert.ToInt32(wsParameters["numextras"]);
                        bool bExit = false;
                        ResultType rtResInt;

                        if (oExtraParametersOut == null)
                            oExtraParametersOut = new Dictionary<int, List<SortedList>>();

                        for (int iExtra = 0; iExtra < iNumExtras; iExtra += 1)
                        {
                            var oLstParametersOutTemp = new List<SortedList>();

                            int iParkingData = 0;
                            do
                            {

                                bExit = (wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_r", iExtra, iParkingData)] == null);
                                if (!bExit)
                                {
                                    SortedList parametersOutTemp = new SortedList();

                                    rtResInt = Convert_ResultTypeBilbaoIntegrationParkingWS_TO_ResultType((ResultTypeBilbaoIntegrationParkingWS)Convert.ToInt32(wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_r", iExtra, iParkingData)].ToString()));

                                    if (rtResInt == ResultType.Result_OK)
                                    {
                                        if (iParkingData == 0 ||
                                            (wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_is_remote_extension", iExtra, iParkingData)] != null &&
                                             Convert.ToInt32(wsParameters[string.Format("extras_extradata_{0}_parkingdata_{1}_is_remote_extension", iExtra, iParkingData)]) == 0))
                                        {
                                            string strQPlusVATQsTemp = "";
                                            string sAuthIdTemp = "";
                                            decimal? dBonMltTemp = null;
                                            string sVehicleTypeTemp = null;

                                            rtResInt = StandardQueryParkingComputeOutput(string.Format("extras_extradata_{0}_parkingdata_{1}_", iExtra, iParkingData),
                                                                                     iWSNumber, oUser, strPlate, oAdditionalPlates, dtParkQuery, oGroup, null, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, "",
                                                                                     strExtTariffId, bIsShopKeeperOperation, bApplyCampaingDiscount, dApplyCampaingDiscount,
                                                                                     iCampaingFreeMinutes, iCampaingMinimumTimeToApply, culture, ref wsParameters, ref parametersOutTemp, ref strQPlusVATQsTemp,
                                                                                     out sAuthIdTemp, out dBonMltTemp, out dBonExtMlt, out sVehicleTypeTemp, ref iCampaingAmountToSubstract);


                                            if (iParkingData == 0 && !string.IsNullOrEmpty(strQPlusVATQsTemp))
                                                strQPlusVATQs += "~" + strQPlusVATQsTemp;
                                        }
                                    }
                                    else
                                    {
                                        parametersOutTemp["r"] = Convert.ToInt32(rtResInt);
                                    }
                                    oLstParametersOutTemp.Add(parametersOutTemp);
                                }

                                iParkingData += 1;
                            }
                            while (!bExit);

                            oExtraParametersOut.Add(iExtra + 2, oLstParametersOutTemp);
                        }
                        if (iNumExtras > 0)
                        {
                            parametersOut["numextras"] = iNumExtras;
                            //parametersOut["extras"] = "";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BilbaoIntegrationQueryParkingAmountSteps::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType StandardQueryUnParking(int iWSNumber,USER oUser, string strPlate,decimal? dInTariffId, DateTime dtUnParkQuery, INSTALLATION oInstallation, ulong ulAppVersion,
                                                int? iWSTimeout, ref SortedList parametersOut, ref List<SortedList> lstRefunds, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            decimal? dGroupId = null;
            decimal? dTariffId = null;
            

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;


            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 


                StandardParkingWS.TariffComputerWS oUnParkWS = new StandardParkingWS.TariffComputerWS();

                oUnParkWS.Url = oInstallation.INS_UNPARK_WS_URL;
                oUnParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_UNPARK_WS_HTTP_USER))
                {
                    oUnParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_UNPARK_WS_HTTP_USER, oInstallation.INS_UNPARK_WS_HTTP_PASSWORD);
                }

                DateTime dtInstallation = dtUnParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateStandardWSHash(oInstallation.INS_UNPARK_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}", strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strCompanyName, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><ext_acc>{3}</ext_acc><prov>{4}</prov><vers>{5}</vers><ah>{6}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strCompanyName, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryUnParking Timeout={1} xmlIn={0}", sXmlIn, oUnParkWS.Timeout), LogLevels.logDEBUG);
                watch = Stopwatch.StartNew();
                string strOut = oUnParkWS.QueryUnParkingOperation(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardQueryUnParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {
                     
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);

                        int iRefundsNum = Convert.ToInt32(wsParameters["numResults"]);
                        string sRefPrefix = "";

                        for (int iRef = 0; iRef < iRefundsNum; iRef += 1)
                        {
                            SortedList oRefund = new SortedList();

                            sRefPrefix = string.Format("refunds_refund_{0}", iRef);
                            oRefund["ins_id"] = oInstallation.INS_ID;
                            oRefund["p"] = strPlate;
                            oRefund["q"] = wsParameters[sRefPrefix + "_ref_amount"];
                            oRefund["t"] = wsParameters[sRefPrefix + "_ref_time"];
                            oRefund["q_rem"] = wsParameters[sRefPrefix + "_rem_amount"];
                            oRefund["t_rem"] = wsParameters[sRefPrefix + "_rem_time"];
                            oRefund["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                            DateTime dt1 = DateTime.ParseExact(wsParameters[sRefPrefix + "_d_ini"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);
                            DateTime dt2 = DateTime.ParseExact(wsParameters[sRefPrefix + "_d_end"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);
                            if (wsParameters.ContainsKey(sRefPrefix + "_d_prev_end"))
                            {
                                DateTime dt_prev_end = DateTime.ParseExact(wsParameters[sRefPrefix + "_d_prev_end"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);
                                oRefund["d_prev_end"] = dt_prev_end.ToString("HHmmssddMMyy");
                            }

                            oRefund["d1"] = dt1.ToString("HHmmssddMMyy");
                            oRefund["d2"] = dt2.ToString("HHmmssddMMyy");
                            oRefund["exp"] = (Conversions.RoundSeconds(dtInstallation) < Conversions.RoundSeconds(dt2)) ? "0" : "1";

                            string strExtTariffId = "";
                            string strExtGroupId = "";

                            if ((wsParameters[sRefPrefix + "_tar_id"] != null) && (wsParameters[sRefPrefix + "_grp_id"] != null))
                            {

                                strExtTariffId = wsParameters[sRefPrefix + "_tar_id"].ToString();
                                strExtGroupId = wsParameters[sRefPrefix + "_grp_id"].ToString();


                                if (dInTariffId.HasValue)
                                {
                                    if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(iWSNumber, dtInstallation, oInstallation, strExtGroupId,
                                            strExtTariffId, dInTariffId.Value, ref dGroupId, ref dTariffId))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage("StandardQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                        return rtRes;
                                    }

                                }
                                else
                                {
                                    if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(iWSNumber, dtInstallation, oInstallation, strExtGroupId,
                                            strExtTariffId, ref dGroupId, ref dTariffId))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage("StandardQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                        return rtRes;
                                    }
                                }

                                if (!dTariffId.HasValue)
                                {
                                    rtRes = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("StandardQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                    return rtRes;
                                }

                                if (!dGroupId.HasValue)
                                {
                                    rtRes = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("StandardQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                    return rtRes;
                                }


                                oRefund["ad"] = dTariffId.Value;
                                oRefund["g"] = dGroupId.Value;
                            }

                            if ((wsParameters[sRefPrefix + "_stse_id"] != null))
                            {

                                string sExtStreetSectionId = wsParameters[sRefPrefix + "_stse_id"].ToString();
                                decimal? dStreetSectionId = null;

                                if (!geograficAndTariffsRepository.GetStreetSectionFromExternalId(oInstallation.INS_ID, sExtStreetSectionId, ref dStreetSectionId))
                                {
                                    rtRes = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("StandardQueryUnParking::GetStreetSectionExternalTranslation Error", LogLevels.logERROR);
                                    return rtRes;
                                }

                                if (!dStreetSectionId.HasValue)
                                {
                                    rtRes = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("StandardQueryUnParking::GetStreetSectionExternalTranslation Error", LogLevels.logERROR);
                                    return rtRes;
                                }

                                oRefund["sts"] = dStreetSectionId.Value;                                
                            }

                            oRefund["base_oper_id"] = wsParameters[sRefPrefix + "_base_oper_id"];


                            if (wsParameters[sRefPrefix + "_ref_amount_without_bon"] != null)
                                oRefund["q_without_bon"] = wsParameters[sRefPrefix + "_ref_amount_without_bon"];
                            else
                                oRefund["q_without_bon"] = oRefund["q"];
                            if (wsParameters[sRefPrefix + "_rem_amount_without_bon"] != null)
                                oRefund["q_rem_without_bon"] = wsParameters[sRefPrefix + "_rem_amount_without_bon"];
                            else
                                oRefund["q_rem_without_bon"] = oRefund["q_rem"];


                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            decimal dBonMlt = 1;                            
                            try
                            {
                                string sBonMlt = wsParameters[sRefPrefix + "_bon_mlt"].ToString();
                                if (sBonMlt.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                                decimal dTryBonMlt = Convert.ToDecimal(sBonMlt, numberFormatProvider);
                                dBonMlt = dTryBonMlt;
                            }
                            catch
                            {
                                dBonMlt = 1;
                            }

                            numberFormatProvider.NumberDecimalSeparator = ".";
                            oRefund["per_bon"] = dBonMlt.ToString(numberFormatProvider);

                            int iNumDisplayTexts = 0;
                            try
                            {
                                iNumDisplayTexts = Convert.ToInt32(wsParameters[sRefPrefix + "_numdisplaytexts"].ToString());
                            }
                            catch
                            {
                                iNumDisplayTexts = 0;
                            }
                            if (iNumDisplayTexts > 0)
                            {
                                string sVehicleType = "";
                                try
                                {
                                    string sIsoLang = "";
                                    for (int i = 0; i < iNumDisplayTexts; i++)
                                    {
                                        sIsoLang = wsParameters[sRefPrefix + string.Format("_displaytexts_displaytext_{0}_isolang", i)].ToString();
                                        if (sIsoLang == "es")
                                        {
                                            sVehicleType = wsParameters[sRefPrefix + string.Format("_displaytexts_displaytext_{0}_text", i)].ToString();
                                            sVehicleType = sVehicleType.Replace("Tipo de vehículo", "").Trim();
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    Logger_AddLogMessage("StandardQueryUnParking::Parsing Vehicle Type Error", LogLevels.logERROR);
                                    sVehicleType = null;
                                }
                                oRefund["vehicletype"] = sVehicleType;
                            }

                            lstRefunds.Add(oRefund);
                        }
                    }
                    else
                    {
                        parametersOut["r"] = Convert.ToInt32(wsParameters["r"]);
                        rtRes = (ResultType)parametersOut["r"];
                    }

                }



            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "StandardQueryUnParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            { }


            return rtRes;

        }

        public ResultType BilbaoQueryUnParking(int iWSNumber, USER oUser, string strPlate, decimal? dInTariffId, DateTime dtUnParkQuery, INSTALLATION oInstallation, ulong ulAppVersion,
                                                int? iWSTimeout, ref SortedList parametersOut, ref List<SortedList> lstRefunds, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            decimal? dGroupId = null;
            decimal? dTariffId = null;
            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                AddTLS12Support();
                var oUnParkWS = new BilbaoParkWsIntegraExternalService.integraExternalServices();
                oUnParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                oUnParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                if (!string.IsNullOrEmpty(oInstallation.INS_PARK_WS_HTTP_USER))
                {
                    oUnParkWS.Credentials = new NetworkCredential(oInstallation.INS_PARK_WS_HTTP_USER, oInstallation.INS_PARK_WS_HTTP_PASSWORD);
                }

                DateTime dtInstallation = dtUnParkQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateStandardWSHash(oInstallation.INS_UNPARK_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}", strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strCompanyName, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><ext_acc>{3}</ext_acc><prov>{4}</prov><vers>{5}</vers><ah>{6}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, oUser.USR_USERNAME, strCompanyName, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("BilbaoQueryUnParking Timeout={1} xmlIn={0}", sXmlIn, oUnParkWS.Timeout), LogLevels.logDEBUG);
                watch = Stopwatch.StartNew();
                string strOut = oUnParkWS.QueryUnParkingOperation(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("BilbaoQueryUnParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {

                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);

                        int iRefundsNum = Convert.ToInt32(wsParameters["numResults"]);
                        string sRefPrefix = "";

                        for (int iRef = 0; iRef < iRefundsNum; iRef += 1)
                        {
                            SortedList oRefund = new SortedList();

                            sRefPrefix = string.Format("refunds_refund_{0}", iRef);
                            oRefund["ins_id"] = oInstallation.INS_ID;
                            oRefund["p"] = strPlate;
                            oRefund["q"] = wsParameters[sRefPrefix + "_ref_amount"];
                            oRefund["t"] = wsParameters[sRefPrefix + "_ref_time"];
                            oRefund["q_rem"] = wsParameters[sRefPrefix + "_rem_amount"];
                            oRefund["t_rem"] = wsParameters[sRefPrefix + "_rem_time"];
                            oRefund["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                            DateTime dt1 = DateTime.ParseExact(wsParameters[sRefPrefix + "_d_ini"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);
                            DateTime dt2 = DateTime.ParseExact(wsParameters[sRefPrefix + "_d_end"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);
                            if (wsParameters.ContainsKey(sRefPrefix + "_d_prev_end"))
                            {
                                DateTime dt_prev_end = DateTime.ParseExact(wsParameters[sRefPrefix + "_d_prev_end"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);
                                oRefund["d_prev_end"] = dt_prev_end.ToString("HHmmssddMMyy");
                            }

                            oRefund["d1"] = dt1.ToString("HHmmssddMMyy");
                            oRefund["d2"] = dt2.ToString("HHmmssddMMyy");
                            oRefund["exp"] = (Conversions.RoundSeconds(dtInstallation) < Conversions.RoundSeconds(dt2)) ? "0" : "1";

                            string strExtTariffId = "";
                            string strExtGroupId = "";

                            if ((wsParameters[sRefPrefix + "_tar_id"] != null) && (wsParameters[sRefPrefix + "_grp_id"] != null))
                            {

                                strExtTariffId = wsParameters[sRefPrefix + "_tar_id"].ToString();
                                strExtGroupId = wsParameters[sRefPrefix + "_grp_id"].ToString();


                                if (dInTariffId.HasValue)
                                {
                                    if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(iWSNumber, dtInstallation, oInstallation, strExtGroupId,
                                            strExtTariffId, dInTariffId.Value, ref dGroupId, ref dTariffId))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage("BilbaoQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                        return rtRes;
                                    }

                                }
                                else
                                {
                                    if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(iWSNumber, dtInstallation, oInstallation, strExtGroupId,
                                            strExtTariffId, ref dGroupId, ref dTariffId))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage("BilbaoQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                        return rtRes;
                                    }
                                }

                                if (!dTariffId.HasValue)
                                {
                                    rtRes = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("BilbaoQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                    return rtRes;
                                }

                                if (!dGroupId.HasValue)
                                {
                                    rtRes = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("BilbaoQueryUnParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                    return rtRes;
                                }


                                oRefund["ad"] = dTariffId.Value;
                                oRefund["g"] = dGroupId.Value;
                            }

                            oRefund["base_oper_id"] = wsParameters[sRefPrefix + "_base_oper_id"];


                            if (wsParameters[sRefPrefix + "_ref_amount_without_bon"] != null)
                                oRefund["q_without_bon"] = wsParameters[sRefPrefix + "_ref_amount_without_bon"];
                            else
                                oRefund["q_without_bon"] = oRefund["q"];
                            if (wsParameters[sRefPrefix + "_rem_amount_without_bon"] != null)
                                oRefund["q_rem_without_bon"] = wsParameters[sRefPrefix + "_rem_amount_without_bon"];
                            else
                                oRefund["q_rem_without_bon"] = oRefund["q_rem"];


                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            decimal dBonMlt = 1;
                            try
                            {
                                string sBonMlt = wsParameters[sRefPrefix + "_bon_mlt"].ToString();
                                if (sBonMlt.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                                decimal dTryBonMlt = Convert.ToDecimal(sBonMlt, numberFormatProvider);
                                dBonMlt = dTryBonMlt;
                            }
                            catch
                            {
                                dBonMlt = 1;
                            }

                            numberFormatProvider.NumberDecimalSeparator = ".";
                            oRefund["per_bon"] = dBonMlt.ToString(numberFormatProvider);

                            int iNumDisplayTexts = 0;
                            try
                            {
                                iNumDisplayTexts = Convert.ToInt32(wsParameters[sRefPrefix + "_numdisplaytexts"].ToString());
                            }
                            catch
                            {
                                iNumDisplayTexts = 0;
                            }
                            if (iNumDisplayTexts > 0)
                            {
                                string sVehicleType = "";
                                try
                                {
                                    string sIsoLang = "";
                                    for (int i = 0; i < iNumDisplayTexts; i++)
                                    {
                                        sIsoLang = wsParameters[sRefPrefix + string.Format("_displaytexts_displaytext_{0}_isolang", i)].ToString();
                                        if (sIsoLang == "es")
                                        {
                                            sVehicleType = wsParameters[sRefPrefix + string.Format("_displaytexts_displaytext_{0}_text", i)].ToString();
                                            sVehicleType = sVehicleType.Replace("Tipo de vehículo", "").Trim();
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    Logger_AddLogMessage("BilbaoQueryUnParking::Parsing Vehicle Type Error", LogLevels.logERROR);
                                    sVehicleType = null;
                                }
                                oRefund["vehicletype"] = sVehicleType;
                            }

                            lstRefunds.Add(oRefund);
                        }
                    }
                    else
                    {
                        parametersOut["r"] = Convert.ToInt32(wsParameters["r"]);
                        rtRes = (ResultType)parametersOut["r"];
                    }

                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardQueryUnParking::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            { }

            return rtRes;
        }

        public ResultType StandardModifyOperationPlates(int iWSNumber, OPERATION oOperation, string strPlate, List<string> oAdditionalPlates, int? iWSTimeout, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;            
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                StandardParkingWS.TariffComputerWS oParkWS = new StandardParkingWS.TariffComputerWS();
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";

                INSTALLATION oInstallation = oOperation.INSTALLATION;

                switch (iWSNumber)
                {
                    case 1:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD);
                        }
                        break;

                    case 2:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD);
                        }
                        break;

                    case 3:
                        oParkWS.Url = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        strHashKey = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oParkWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER, oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD);
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("StandardModifyOperationPlates::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                
                //string strvers = "1.0";
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();



                string strMessage = "";
                string strAuthHash = "";


                string sPlatesValues = strPlate;
                string sPlatesTags = string.Format("<lic_pla>{0}</lic_pla>", strPlate);
                if (oAdditionalPlates != null)
                {
                    for (int i = 0; i < oAdditionalPlates.Count; i += 1)
                    {
                        sPlatesValues += oAdditionalPlates[i];
                        sPlatesTags += string.Format("<lic_pla{0}>{1}</lic_pla{0}>", i + 2, oAdditionalPlates[i]);
                    }
                }

                string sOpeExternalId = "";
                if (iWSNumber == 1)
                    sOpeExternalId = oOperation.OPE_EXTERNAL_ID1;
                else if (iWSNumber == 2)
                    sOpeExternalId = oOperation.OPE_EXTERNAL_ID2;
                else if (iWSNumber == 3)
                    sOpeExternalId = oOperation.OPE_EXTERNAL_ID3;

                strAuthHash = CalculateStandardWSHash(strHashKey,
                    string.Format("{0}{1}{2}{3}",
                                   strCityID, sOpeExternalId, sPlatesValues, strCompanyName));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><ope_id>{1}</ope_id>{2}" +
                                           "<prov>{3}</prov>" +
                                           "<ah>{4}</ah></ipark_in>",
                                           strCityID, sOpeExternalId, sPlatesTags,  strCompanyName, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardModifyOperationPlates Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oParkWS.ModifyOperationPlates(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardModifyOperationPlates xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "StandardModifyOperationPlates::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType MadridPlatformConfirmParking(int iWSNumber, string strPlate, DateTime dtParkQuery, DateTime dtUTCInsertionDateUTCDate, USER oUser, INSTALLATION oInstallation, 
                                                       decimal? dGroupId, decimal? dTariffId, int iQuantity, int iTime,DateTime dtIni, DateTime dtEnd, decimal dOperationId, decimal dAuthId,
                                                       int? iWSTimeout,/*decimal? dBonExtMlt, */ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            MadridPlatform.PublishServiceV12Client oService = null;
            MadridPlatform.AuthSession oAuthSession = null;
            int iWSLocalTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();

            try
            {
                AddTLS12Support();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                oService = new MadridPlatform.PublishServiceV12Client();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender2, certificate, chain, sslPolicyErrors) => true);

                //string strHashKey = "";

                switch (iWSNumber)
                {
                    case 1:
                        oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_PARK_CONFIRM_WS_URL);
                        //strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oService.ClientCredentials.UserName.UserName = oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER;
                            oService.ClientCredentials.UserName.Password = oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD;
                        }
                        break;

                    case 2:
                        oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_PARK_CONFIRM_WS2_URL);
                        //strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oService.ClientCredentials.UserName.UserName = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER;
                            oService.ClientCredentials.UserName.Password = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD;
                        }
                        break;

                    case 3:
                        oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_PARK_CONFIRM_WS3_URL);
                        //strHashKey = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oService.ClientCredentials.UserName.UserName = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER;
                            oService.ClientCredentials.UserName.Password = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD;
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("MadridPlatformConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                DateTime? dtUTCInstallation = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtParkQuery);
                DateTime? dtUTCIni = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtIni);
                DateTime? dtUTCEnd = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtEnd);

                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("MadridPlatformConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sCodPhyZone = strExtGroupId.Split('~')[0];
                string sSubBarrio = (strExtGroupId.Split('~').Length > 1 ? strExtGroupId.Split('~')[1] : "");

                var oRequest = new MadridPlatform.PayParkingTransactionRequest()
                {
                    PhyZone = new MadridPlatform.EntityFilterPhyZone()
                    {
                        CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                        CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                        CodCity = oInstallation.INS_PHY_ZONE_COD_CITY,
                        CodPhyZone = sCodPhyZone
                    },
                    PrkTrans = new MadridPlatform.PayTransactionParking()
                    {
                        AuthId = Convert.ToInt64(dAuthId),
                        //OperationDateUTC = dtUTCInstallation.Value,
                        OperationDateUTC = dtUTCInsertionDateUTCDate,
                        TariffId = Convert.ToInt32(strExtTariffId),
                        TicketNum = string.Format("98{0}00000{1}{2}{3}", sCodPhyZone, dtParkQuery.DayOfYear.ToString("000"), dtParkQuery.ToString("HHmm"), dtEnd.ToString("HHmm")),
                        TransId = Convert.ToInt64(dOperationId),
                        ParkingOper = new MadridPlatform.PayParking()
                        {
                            PrkBgnUtc = dtUTCIni.Value,
                            PrkEndUtc = dtUTCEnd.Value,
                            TotAmo = ((decimal)iQuantity / (decimal)100),
                            TotTim = new TimeSpan(0, iTime, 0)
                        },
                        UserPlate = strPlate
                    }
                    //,SubBarrio = sSubBarrio
                };
                
                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {


                    long lEllapsedTimeLocal = 0;
                    if (MadridPlatfomStartSession(oService, out oAuthSession, iWSLocalTimeout, out lEllapsedTimeLocal))
                    {
                        strParamsIn = string.Format("sessionId={15};userName={16};" +
                                                       "CodSystem={0};CodGeoZone={1};CodCity={2};CodPhyZone={3};" +
                                                       "AuthId={4};OperationDateUTC={5:yyyy-MM-ddTHH:mm:ss.fff};TariffId={6};TicketNum={7};TransId={8};" +
                                                       "PrkBgnUtc={9:yyyy-MM-ddTHH:mm:ss.fff};PrkEndUtc={10:yyyy-MM-ddTHH:mm:ss.fff};TotAmo={11};TotTim={12};UserPlate={13};" +
                                                       "SubBarrio={14}",
                                                        oRequest.PhyZone.CodSystem, oRequest.PhyZone.CodGeoZone, oRequest.PhyZone.CodCity, oRequest.PhyZone.CodPhyZone,
                                                        oRequest.PrkTrans.AuthId, oRequest.PrkTrans.OperationDateUTC, oRequest.PrkTrans.TariffId, oRequest.PrkTrans.TicketNum, oRequest.PrkTrans.TransId,
                                                        oRequest.PrkTrans.ParkingOper.PrkBgnUtc, oRequest.PrkTrans.ParkingOper.PrkEndUtc, oRequest.PrkTrans.ParkingOper.TotAmo, oRequest.PrkTrans.ParkingOper.TotTim, oRequest.PrkTrans.UserPlate,
                                                        oRequest.SubBarrio,
                                                        oAuthSession.sessionId, oAuthSession.userName);

                        iWSLocalTimeout -= (int)lEllapsedTimeLocal;

                        oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSLocalTimeout);
                        Logger_AddLogMessage(string.Format("MadridPlatformConfirmParking Timeout={1} parametersIn={0}", strParamsIn, oService.InnerChannel.OperationTimeout.TotalMilliseconds), LogLevels.logDEBUG);

                        var serializer = new System.Xml.Serialization.XmlSerializer(oRequest.GetType());
                        var sb = new StringBuilder();
                        using (System.IO.TextWriter writer = new System.IO.StringWriter(sb))
                        {
                            serializer.Serialize(writer, oRequest);
                        }
                        strParamsIn = sb.ToString();
                        Logger_AddLogMessage(string.Format("MadridPlatformConfirmParking request={0}", strParamsIn), LogLevels.logDEBUG);

                        Newtonsoft.Json.JsonSerializer oScriptSerializer = Newtonsoft.Json.JsonSerializer.Create();
                        using (var sw = new System.IO.StringWriter())
                        {
                            oScriptSerializer.Serialize(sw, oRequest, oRequest.GetType());
                            strParamsIn = sw.ToString();
                        }
                        Logger_AddLogMessage(string.Format("MadridPlatformConfirmParking request={0}", strParamsIn), LogLevels.logDEBUG);

                        watch = Stopwatch.StartNew();
                        var oParkingResp = oService.SetParkingTransaction(oAuthSession, oRequest);
                        lEllapsedTime = watch.ElapsedMilliseconds;
                        iWSLocalTimeout -= (int)lEllapsedTime;


                        strParamsOut = string.Format("Status={0};errorDetails={1}", oParkingResp.Status.ToString(), oParkingResp.errorDetails);
                        Logger_AddLogMessage(string.Format("MadridPlatformConfirmParking response={0}", strParamsOut), LogLevels.logDEBUG);

                        if (oParkingResp.Status != MadridPlatform.PublisherResponse.PublisherStatus.OK &&
                            oParkingResp.authError.HasValue && oParkingResp.authError.Value == MadridPlatform.PublisherResponseAuthError.EXPIRED_SESSION)
                        {
                            MadridPlatfomEndSession();
                            bAuthRetry = true;
                        }
                        else
                        {
                            bAuthRetry = false;
                            rtRes = (oParkingResp.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK ? ResultType.Result_OK : ResultType.Result_Error_Generic);
                        }

                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                    }

                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    str3dPartyOpNum = dAuthId.ToString();
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "MadridPlatformConfirmParking::Exception", LogLevels.logERROR);
            }
            finally
            {
                
            } 
            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;
        }

        public ResultType Madrid2PlatformConfirmParking(int iWSNumber, string strPlate, DateTime dtParkQuery, DateTime dtUTCInsertionDateUTCDate, USER oUser, INSTALLATION oInstallation, 
                                                       decimal? dGroupId, decimal? dTariffId, int iQuantity, int iTime,DateTime dtIni, DateTime dtEnd, decimal dOperationId, decimal dAuthId,
                                                       int? iWSTimeout,/*decimal? dBonExtMlt, */ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            lEllapsedTime = -1;
            Stopwatch watch = null;

            MadridErrorResponse oErrorResponse = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            try
            {
                string sBaseUrl = "";
                string sClientId = "";
                string sClientSecret = "";
                string sTokenUrl = "";

                switch (iWSNumber)
                {
                    case 1:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        sTokenUrl = oInstallation.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            sClientId = oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER;
                            sClientSecret = oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD;
                        }
                        break;

                    case 2:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS2_URL;                        
                        sTokenUrl = oInstallation.INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            sClientId = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER;
                            sClientSecret = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD;
                        }
                        break;

                    case 3:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        sTokenUrl = oInstallation.INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            sClientId = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER;
                            sClientSecret = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD;
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("Madrid2PlatformConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }
                }

                DateTime? dtUTCInstallation = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtParkQuery);
                DateTime? dtUTCIni = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtIni);
                DateTime? dtUTCEnd = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtEnd);

                string strExtTariffId = "";
                string strExtGroupId = "";

                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("MadridPlatformConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                string sCodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM;
                string sCodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE;
                string sCodCity = oInstallation.INS_PHY_ZONE_COD_CITY;

                string sCodPhyZone = strExtGroupId.Split('~')[0];
                string sSubBarrio = (strExtGroupId.Split('~').Length > 1 ? strExtGroupId.Split('~')[1] : "");

                // *** Check if zone has new API assigned
                if (Madrid2AllowedZone(sCodPhyZone))
                {
                    Madrid2Params(out sBaseUrl, out sClientId, out sClientSecret, out sTokenUrl, out sCodSystem, out sCodGeoZone, out sCodCity);
                }
                // ***

                DateTime dtUTCIniTruncated = new DateTime(dtUTCIni.Value.Year, dtUTCIni.Value.Month, dtUTCIni.Value.Day, 
                                                          dtUTCIni.Value.Hour, dtUTCIni.Value.Minute, 0);
                var oBodyContent = new MadridPayTransactionRequest()
                {
                    phyZone = new MadridPhyZone()
                    {
                        codSystem = sCodSystem,
                        codGeoZone = sCodGeoZone,
                        codCity = sCodCity,
                        codPhyZone = sCodPhyZone
                    },
                    prkTrans = new MadridParkTrans()
                    {
                        authId = Convert.ToInt32(dAuthId),
                        operationDateUTC = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ssZ}", dtUTCInsertionDateUTCDate), 
                        tariffId = Convert.ToInt32(strExtTariffId),
                        ticketNum = string.Format("98{0}00000{1}{2}{3}", sCodPhyZone, dtParkQuery.DayOfYear.ToString("000"), dtParkQuery.ToString("HHmm"), dtEnd.ToString("HHmm")),
                        transId = Convert.ToInt32(dOperationId),
                        parkingOper = new MadridPayParking()
                        {
                            prkBgnUtc = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ssZ}", dtUTCIniTruncated),
                            prkEndUtc = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ssZ}", dtUTCEnd.Value),
                            totAmo = ((decimal)iQuantity / (decimal)100),
                            totTim = TimeSpanToString(new TimeSpan(0, iTime, 0))
                        },
                        userPlate = strPlate
                    }
                };

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    if (this.MadridToken(sTokenUrl, sClientId, sClientSecret, out sAccessToken))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        string sUrl = string.Format("{0}/parking-payment-transaction", sBaseUrl);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "POST";
                        oRequest.ContentType = "application/json";
                        oRequest.Timeout = Get3rdPartyWSTimeout();
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        string sJsonIn = JsonConvert.SerializeObject(oBodyContent);

                        Logger_AddLogMessage(string.Format("Madrid2PlatformConfirmParking request.url={0}, request.authorization={1}, request.jsonIn={2}", sUrl, sAccessToken, PrettyJSON(sJsonIn)), LogLevels.logINFO);

                        byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                        oRequest.ContentLength = byteArray.Length;
                        Stream dataStream = oRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        try
                        {
                            watch = Stopwatch.StartNew();

                            WebResponse response = oRequest.GetResponse();
                            // Display the status.
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            lEllapsedTime = watch.ElapsedMilliseconds;

                            bAuthRetry = false;

                            if (oWebResponse.StatusCode == HttpStatusCode.Created)
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("Madrid2PlatformConfirmParking response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                MadridPayTransactionResponse oResponse = (MadridPayTransactionResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(MadridPayTransactionResponse));

                                if (oResponse != null)
                                {
                                    str3dPartyOpNum = oResponse.id.ToString();
                                    rtRes = ResultType.Result_OK;
                                }
                                else
                                    rtRes = ResultType.Result_Error_Generic;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if ((watch != null) && (lEllapsedTime == 0))
                                lEllapsedTime = watch.ElapsedMilliseconds;

                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sMadridAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = MadridErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new MadridErrorResponse()
                                    {
                                        error = new MadridError()
                                        {
                                            status = (int)ex.Status
                                        },
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };

                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "Madrid2PlatformConfirmParking::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            if ((watch != null) && (lEllapsedTime == 0))
                                lEllapsedTime = watch.ElapsedMilliseconds;

                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "Madrid2PlatformConfirmParking::Exception", LogLevels.logERROR);
                        }

                    }
                    else                        
                        rtRes = ResultType.Result_Error_Generic;
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "Madrid2PlatformConfirmParking::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        public ResultType StandardQueryAvailableTariffs(int iWSNumber, string strPlate,  DateTime dtParkQuery, GROUP oGroup, IEnumerable<stTariff> oInTariffs, 
                                                        int? iWSTimeout, ref IEnumerable<stTariff> oOutTariffs, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            oOutTariffs = new List<stTariff>();

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;


            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                StandardParkingWS.TariffComputerWS oParkWS = new StandardParkingWS.TariffComputerWS();
                oParkWS.Url = oGroup.INSTALLATION.INS_PARK_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new System.Net.NetworkCredential(oGroup.INSTALLATION.INS_PARK_WS_HTTP_USER, oGroup.INSTALLATION.INS_PARK_WS_HTTP_PASSWORD);
                }

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = oGroup.INSTALLATION.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                string strMessage = "";
                string strAuthHash = "";

                string strExtTariffId = "";
                string strExtGroupId = "";
                string strTariffsList = "";


                int icount = 0;
                foreach (stTariff oTariff in oInTariffs)
                {

                    if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, oGroup.GRP_ID, oTariff.dID, ref strExtGroupId, ref strExtTariffId))
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage("StandardQueryAvailableTariffs::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                    }

                    if (icount == 0)
                    {
                        strTariffsList = strExtTariffId;
                    }
                    else
                    {
                        strTariffsList += string.Format("~{0}", strExtTariffId);
                    }
                    icount++;
                        
                }



                strAuthHash = CalculateStandardWSHash(oGroup.INSTALLATION.INS_PARK_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1:HHmmssddMMyyyy}{2}{3}{4}{5}", strCityID, dtInstallation, strExtGroupId, strTariffsList, strCompanyName, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><date>{1:HHmmssddMMyyyy}</date><grp_id>{2}</grp_id><tar_ids>{3}</tar_ids><prov>{4}</prov><vers>{5}</vers><ah>{6}</ah></ipark_in>",
                    strCityID , dtInstallation, strExtGroupId, strTariffsList , strCompanyName, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryAvailableTariffs Timeout={1} xmlIn={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);

                string strOut = "";

                watch = Stopwatch.StartNew();
                strOut = oParkWS.QueryAvailableTariffs(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;



                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardQueryAvailableTariffs xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {

                        if (wsParameters.ContainsKey("tar_ids"))
                        {
                            string strTarOuts = wsParameters["tar_ids"].ToString();
                            string[] arrstrTarOuts = strTarOuts.Split('~');
                            decimal? dGroupId = null;
                            decimal? dTariffId = null;

                            foreach (string strTarId in arrstrTarOuts)
                            {
                                foreach (stTariff oTariff in oInTariffs)
                                {
                                    if (geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(0, dtInstallation, oGroup.INSTALLATION, strExtGroupId, strTarId, oTariff.dID, ref dGroupId, ref dTariffId))
                                    {
                                        if (dTariffId.HasValue)
                                        {
                                            
                                            ((List<stTariff>)oOutTariffs).Add(oTariff);
                                            break;
                                            
                                        }
                                    }
                                }

                            }
                        }

                    }

                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
               
                Logger_AddLogException(e, "StandardQueryAvailableTariffs::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }




        public ResultType BSMNearConfigurations(INSTALLATION oInstallation, decimal dLatitude, decimal dLongitude, string sLocale, int? iWSTimeout,
                                                out BSMConfigurations oConfigurations, out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;            
            oConfigurations = null;
            oErrorResponse = null;

            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            int iLocalWSTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();


            try
            {
                string sBaseUrl = oInstallation.INS_PARK_WS_URL;
                string sUser = oInstallation.INS_PARK_WS_HTTP_USER;
                string sPassword = oInstallation.INS_PARK_WS_HTTP_PASSWORD;

                // ***
                /*sBaseUrl = "https://preapi.bsmsa.eu/ext/api";
                sUser = "xL6BxwxOgfXJ7wDEQD7oRT104iMa";
                sPassword = "3jhZBXTsG3jVhN78AsShrGgAoKka";*/
                // ***

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue) 
                        bAuthRetry = false;

                    long lLocalEllapsedTime = 0;
                    if (this.BSMToken(sBaseUrl, sUser, sPassword, iLocalWSTimeout, out sAccessToken, out lLocalEllapsedTime))
                    {
                        lEllapsedTime += lLocalEllapsedTime;
                        iLocalWSTimeout -= (int)lLocalEllapsedTime;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        numberFormatProvider.NumberDecimalSeparator = ".";

                        string sLong = dLongitude.ToString("##0.0000000", numberFormatProvider);
                        string sLat = dLatitude.ToString("##0.0000000", numberFormatProvider);

                        string sUrl = string.Format("{0}/nearConfigurations?lng={1}&lat={2}&locale={3}", sBaseUrl, sLong, sLat, sLocale);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "GET";
                        oRequest.ContentType = "application/x-www-form-urlencoded";
                        //oRequest.ContentType = "application/json";
                        oRequest.Timeout = iLocalWSTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));
                        if (watch != null)
                        {
                            watch.Stop();
                            watch = null;
                        }

                        watch = Stopwatch.StartNew();

                        Logger_AddLogMessage(string.Format("BSMNearConfigurations request.url={0}, Timeout={2}, request.authorization={1}", sUrl, sAccessToken, oRequest.Timeout), LogLevels.logINFO);

                        try
                        {

                            WebResponse response = oRequest.GetResponse();
                            // Display the status.
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;

                            iLocalWSTimeout -= (int)watch.ElapsedMilliseconds;
                            lEllapsedTime += watch.ElapsedMilliseconds;


                            if (oWebResponse.StatusCode == HttpStatusCode.OK)
                            {
                                // Get the stream containing content returned by the server.
                                Stream dataStream = response.GetResponseStream();
                                // Open the stream using a StreamReader for easy access.
                                StreamReader reader = new StreamReader(dataStream);
                                // Read the content.
                                string responseFromServer = reader.ReadToEnd();
                                // Display the content.

                                Logger_AddLogMessage(string.Format("BSMNearConfigurations response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                                // Clean up the streams.

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                oConfigurations = BSMConfigurations.Load(oResponse);


                                if ( oConfigurations!= null && oConfigurations.Configurations.Any())
                                {
                                    oConfigurations.Configurations = oConfigurations.Configurations.Where(r => r.cityId == Convert.ToInt32(oInstallation.INS_BSM_CITY_ID)).ToArray();                            
                                }


                                rtRes = (oConfigurations != null && oConfigurations.Configurations.Any() ? ResultType.Result_OK : ResultType.Result_Error_NearConfigurations_Not_Available);

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sBSMAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = BSMErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new BSMErrorResponse()
                                    {
                                        status = ex.Status.ToString(),
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };
                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "BSMNearConfigurations::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "BSMNearConfigurations::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;                
                Logger_AddLogException(e, "BSMNearConfigurations::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        public ResultType BSMPriceParking(USER oUser, GROUP oGroup, TARIFF oTariff, string sPlate, BSMConfiguration oConfiguration, DateTime dtStartTime, DateTime? dtEndTime,
                                          bool bApplyCampaingDiscount, decimal? dApplyCampaingDiscount, 
                                          string strCulture, string sLocale, int? iMaxAmountAllowedToPay, double dChangeToApply, bool bIsShopKeeperOperation, int? iWSTimeout, 
                                          out BSMModifiers oModifiers,out decimal? dBonMlt, out decimal? dBonExtMlt, ref SortedList parametersOut, ref string strQPlusVATQs,
                                          out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;

            oModifiers = null;
            BSMDetailedPrice oDetailedPrice = null;
            dBonMlt = 1;
            dBonExtMlt = 1;

            lEllapsedTime = 0;           

            try
            {

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oGroup.INSTALLATION.INS_TIMEZONE_ID);
                DateTime dtUtcStartTime = TimeZoneInfo.ConvertTime(dtStartTime, tzi, TimeZoneInfo.Utc);
                
                DateTime? dtUtcEndTime = null;
                if (dtEndTime.HasValue)
                    dtUtcEndTime = TimeZoneInfo.ConvertTime(dtEndTime.Value, tzi, TimeZoneInfo.Utc);

                string sCurIsoCode = oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE;

                decimal dPrice;
                decimal dBasePrice;
                DateTime? dtOutUtcEndTime;                
                BSMErrorResponse oErrorResponse;               
                string strRateDescription = "";

                rtRes = BSMPrice(oGroup.INSTALLATION, sPlate, oConfiguration.cityId, oConfiguration.configurationId, oConfiguration.zoneTypeId, dtUtcStartTime, null, dtUtcEndTime, strCulture, sLocale, iWSTimeout, out dPrice, out dBasePrice, out strRateDescription, out dtOutUtcEndTime, out oModifiers, out oDetailedPrice, out oErrorResponse, out lEllapsedTime);
              
                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    
                    if (dPrice < 0)
                    {
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        Logger_AddLogMessage(string.Format("BSMPrice::Negative Amount {0}", dPrice), LogLevels.logWARN);

                    }
                    else
                    {
                        decimal dPriceWithoutBon=dPrice;

                        if (bApplyCampaingDiscount)
                        {
                            dPrice = dPrice * dApplyCampaingDiscount.Value;
                            dBonMlt = dApplyCampaingDiscount.Value;
                           
                        }

                        int iTime = Convert.ToInt32(Math.Floor((dtOutUtcEndTime.Value - dtUtcStartTime).TotalMinutes));
                       

                        int iQ = Convert.ToInt32( Math.Round(dPrice*100, MidpointRounding.AwayFromZero) );

                        parametersOut["g"] = oGroup.GRP_ID;
                        parametersOut["ad"] = oTariff.TAR_ID;

                        parametersOut["q1"] = iQ; // wsParameters[strXMLPrefix + "a_min"];
                        parametersOut["q2"] = iQ; // wsParameters[strXMLPrefix + "a_max"];
                        parametersOut["t1"] = 1; // wsParameters[strXMLPrefix + "t_min"];
                        parametersOut["t2"] = 1; // wsParameters[strXMLPrefix + "t_max"];
                        parametersOut["o"] = Convert.ToInt32(ChargeOperationsType.ParkingOperation); // wsParameters[strXMLPrefix + "op_type"];
                        parametersOut["at"] = 0; // wsParameters[strXMLPrefix + "t_acum"];
                        parametersOut["aq"] = 0; // wsParameters[strXMLPrefix + "a_acum"];

                        parametersOut["cur"] = oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE;
                        parametersOut["postpay"] = "0";
                        parametersOut["notrefundwarning"] = "0";

                        parametersOut["di"] = dtStartTime.ToString("HHmmssddMMyy");

                        double dChangeFee = 0;
                        int iQChange = 0;

                        if (oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE != oUser.CURRENCy.CUR_ISO_CODE)
                        {
                            iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q1"]),
                                                                                  dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                            parametersOut["qch1"] = iQChange.ToString();
                            iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q2"]),
                                                                                  dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                            parametersOut["qch2"] = iQChange.ToString();

                        }

                        ChargeOperationsType oOperationType = (Convert.ToInt32(parametersOut["o"]) == 1 ? ChargeOperationsType.ParkingOperation : ChargeOperationsType.ExtensionOperation);

                        //int iQ = 0;
                        int iQWithoutBon = 0;
                        int iQWithoutBonChange = 0;
                        int iQFEE = 0;
                        decimal dQFEE = 0;
                        int iQFEEChange = 0;
                        int iQVAT = 0;
                        int iQVATChange = 0;
                        int iQTotal = 0;
                        int iQTotalChange = 0;
                        int iQSubTotal = 0;
                        int iQSubTotalChange = 0;
                        int iQPlusIVA = 0;
                        int iQPlusIVAChange = 0;
                        int iFeePlusIVA = 0;
                        int iFeePlusIVAChange = 0;

                        decimal dVAT1 = 0;
                        decimal dVAT2 = 0;
                        int iPartialVAT1 = 0;
                        decimal dPercFEE = 0;
                        decimal dPercFEETopped = 0;
                        int iPartialPercFEE = 0;
                        decimal dFixedFEE = 0;
                        int iPartialFixedFEE = 0;
                        int iPartialPercFEEVAT = 0;
                        int iPartialFixedFEEVAT = 0;

                        int? iPaymentTypeId = null;
                        int? iPaymentSubtypeId = null;
                        int? iTariffType = null;
                        IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                        if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                        {
                            iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                            iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                        }
                        if (oTariff != null)
                            iTariffType = oTariff.TAR_TYPE;
                        if (!customersRepository.GetFinantialParams(oUser, oGroup.INSTALLATION.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, oOperationType, iTariffType,
                                                                    out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("BSMPrice::Error getting finantial parameters", LogLevels.logERROR);
                        }

                        
                        

                        List<StandardQueryParkingStep> oSteps = new List<StandardQueryParkingStep>();

                        if (rtRes == ResultType.Result_OK)
                        {
                            decimal dShopKeeperParkPerc = oGroup.INSTALLATION.INSTALLATION_SHOPKEEPER_PARAMETERs.FirstOrDefault() == null ? (decimal)0 : oGroup.INSTALLATION.INSTALLATION_SHOPKEEPER_PARAMETERs.First().INSSHO_PARK_PROFIT_PERC;
                            int iAmountTotal = 0;
                            int iAmountProfit = 0;

                            decimal dINSCurID = oGroup.INSTALLATION.INS_CUR_ID;
                            decimal dUserCurID = oUser.USR_CUR_ID;

                            DateTime dt = TimeZoneInfo.ConvertTime(dtOutUtcEndTime.Value, TimeZoneInfo.Utc, tzi);

                            iQWithoutBon = Convert.ToInt32(dPriceWithoutBon * 100);


                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                int iPartialVAT1Temp = 0;
                                int iPartialPercFEETemp = 0;
                                int iPartialFixedFEETemp = 0;
                                int iPartialPercFEEVATTemp = 0;
                                int iPartialFixedFEEVATTemp = 0;


                                int iQWithoutBonTotal = customersRepository.CalculateFEE(ref iQWithoutBon, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                            out iPartialVAT1Temp, out iPartialPercFEETemp, out iPartialFixedFEETemp,
                                                                            out iPartialPercFEEVATTemp, out iPartialFixedFEEVATTemp);
                            }



                            int iFreeTimeUsed = 0;
                            int iRealQ = iQ;


                            /*
                            iPercFEETopped, iFixedFEE, iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iPartialPercFEEVAT, iPartialFixedFEEVAT
                            "0                  30      130                 0               34                  0                   4
                             */

                            if (iQ > 0)
                            {
                                iQTotal = customersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                            out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                            out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                                dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                                if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                                dQFEE += dFixedFEE;
                                iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));


                                iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                                iQSubTotal = iQ + iQFEE;
                                iQPlusIVA = iQ + iPartialVAT1;
                                iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;
                            }
                            else
                            {
                                iQTotal = iQ;
                                iQFEE = 0;
                                iQVAT = 0;
                                iQSubTotal = iQ;
                                iQPlusIVA = 0;
                                iFeePlusIVA = 0;
                            }

                            strQPlusVATQs += string.Format("{0};{1}", iQ, iQPlusIVA);


                            /*if (wsParameters.ContainsKey(string.Format("steps_step_{0}_real_a", i)))
                            {
                                if (string.IsNullOrEmpty(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_real_a", i)].ToString()))
                                {
                                    iRealQ = iQ;
                                }
                                else
                                {
                                    iRealQ = Convert.ToInt32(wsParameters[strXMLPrefix + string.Format("steps_step_{0}_real_a", i)].ToString());
                                    iRealQ = Convert.ToInt32(Math.Round(iRealQ * (dBonMlt ?? 1), MidpointRounding.AwayFromZero));
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iRealQ = Convert.ToInt32(Convert.ToDecimal(iRealQ) / (1 + dVAT1));

                                    }
                                }

                            }*/


                            DateTime? dtNow = geograficAndTariffsRepository.getInstallationDateTime(oGroup.GRP_INS_ID) ?? DateTime.Now;
                            string strTodayColour = infraestructureRepository.GetParameterValue("ParkingScreenTodayColour");
                            string strTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenTomorrowColour");
                            string strMoreThanTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenMoreThanTomorrowColour");
                            int iTotalDays = Convert.ToInt32((dt.Date - dtNow.Value.Date).TotalDays);
                            if (iTotalDays < 0)
                                iTotalDays = 0;


                            var oCurrentStep = new StandardQueryParkingStep()
                            {
                                Time = iTime,
                                Q = iQ,
                                Dt = dt,
                                QFEE = iQFEE,
                                QVAT = iQVAT,
                                QSubTotal = iQSubTotal,
                                QTotal = iQTotal,
                                FreeTimeUsed = iFreeTimeUsed,
                                RealQ = iRealQ,
                                QPlusIVA = iQPlusIVA,
                                FeePlusIVA = iFeePlusIVA,
                                QWithoutBon = iQWithoutBon,
                                BonMlt = (dApplyCampaingDiscount ?? 1),
                                NumDays = iTotalDays,
                                NumDaysColour = (iTotalDays == 0) ? strTodayColour : ((iTotalDays == 1) ? strTomorrowColour : strMoreThanTomorrowColour) 
                            };


                            if ((oDetailedPrice != null) && (oDetailedPrice.basePrice > 0))
                            {
                                string strCurSymbol = oGroup.INSTALLATION.CURRENCy.CUR_SYMBOL ?? sCurIsoCode;
                                strCurSymbol = strCurSymbol.Trim();

                                oCurrentStep.ParkingBaseQuantityLbl = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}/h", oDetailedPrice.basePrice, strCurSymbol);
                                oCurrentStep.ParkingVariableQuantityLbl= string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}/h", oDetailedPrice.rateApplied, strCurSymbol);

                            }
                            else if ((oDetailedPrice != null) && (oDetailedPrice.basePrice == 0))
                            {
                                oCurrentStep.ParkingBaseQuantityLbl = strRateDescription;
                                oCurrentStep.ParkingVariableQuantityLbl = "";

                            }
                            else
                            {
                                oCurrentStep.ParkingBaseQuantityLbl = strRateDescription;
                                oCurrentStep.ParkingVariableQuantityLbl = "";
                            }


                            oConfiguration.parkingBaseQuantityLbl = oCurrentStep.ParkingBaseQuantityLbl;
                            oConfiguration.parkingVariableQuantityLbl = oCurrentStep.ParkingVariableQuantityLbl;


                            parametersOut["dexp"] = dt.ToString("HHmmssddMMyy");
                          
                            if (oTariff != null)
                            {
                                parametersOut["NoticeChargesNow"] = oTariff.TAR_NOTICE_CHARGES_NOW;

                                int iAcumTime = 0;

                                try
                                {
                                    iAcumTime = Convert.ToInt32(parametersOut["at"].ToString());
                                }
                                catch { }

                                String sLiteral = string.Empty;

                                if (oTariff.TAR_NOTICE_CHARGES_NOW != (int)NoticChargesNow.NotShowMessage && ((Convert.ToInt32(parametersOut["o"])) == (int)ChargeOperationsType.ParkingOperation && dtStartTime < dtStartTime)) 
                                                                                            //esto no se va a cumplir NUNCA. bsm no devuelve la hora real de inicio con lo que no se puede comprobar
                                {
                                    string sCulture = (!string.IsNullOrEmpty(strCulture) ? strCulture : "en-US");
                                    if (oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.HasValue)
                                    {
                                        sLiteral = infraestructureRepository.GetLiteral(oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.Value, strCulture);
                                        sLiteral = sLiteral.Replace("#", dt.ToShortTimeString() + " | " + dt.ToShortDateString());
                                    }
                                }
                                parametersOut["NoticeChargesNowLiteral"] = sLiteral;
                            }
                            else
                            {
                                parametersOut["NoticeChargesNow"] = (int)NoticChargesNow.NotShowMessage;
                                parametersOut["NoticeChargesNowLiteral"] = string.Empty;
                            }
                            

                            if (dINSCurID != dUserCurID)
                            {

                                //Logger_AddLogMessage("StandardQueryParkingComputeOutput::Change 1", LogLevels.logERROR);
                                iQChange = ChangeQuantityFromInstallationCurToUserCur(iQ, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQWithoutBonChange = ChangeQuantityFromInstallationCurToUserCur(iQWithoutBon, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);

                                iQFEEChange = ChangeQuantityFromInstallationCurToUserCur(iQFEE, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQVATChange = ChangeQuantityFromInstallationCurToUserCur(iQVAT, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQSubTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQSubTotal, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQTotalChange = ChangeQuantityFromInstallationCurToUserCur(iQTotal, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);
                                iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, dINSCurID, dUserCurID, out dChangeFee);



                                if (iMaxAmountAllowedToPay.HasValue)
                                {
                                    if (iQTotalChange > iMaxAmountAllowedToPay)
                                    {
                                        rtRes = ResultType.Result_Error_Not_Enough_Balance; ;
                                        parametersOut["r"] = Convert.ToInt32(rtRes);
                                    }
                                }

                                if (rtRes == ResultType.Result_OK)
                                {

                                    if (bIsShopKeeperOperation)
                                    {
                                        customersRepository.CalculateShopKeeperProfit(iQTotalChange, dShopKeeperParkPerc, out iAmountProfit, out iAmountTotal);

                                        oCurrentStep.QChange = iQChange;
                                        oCurrentStep.QFEEChange = iQFEEChange;
                                        oCurrentStep.QSubTotalChange = iQSubTotalChange;
                                        oCurrentStep.QTotalChange = iQTotalChange;
                                        oCurrentStep.AmountProfit = iAmountProfit;
                                        oCurrentStep.AmountTotal = iAmountTotal;
                                        oCurrentStep.QVATChange = iQVATChange;
                                        oCurrentStep.QPlusIVAChange = iQPlusIVAChange;
                                        oCurrentStep.FeePlusIVAChange = iFeePlusIVAChange;
                                        oCurrentStep.QWithoutBonChange = iQWithoutBonChange;

                                    }
                                    else
                                    {
                                        oCurrentStep.QChange = iQChange;
                                        oCurrentStep.QFEEChange = iQFEEChange;
                                        oCurrentStep.QSubTotalChange = iQSubTotalChange;
                                        oCurrentStep.QTotalChange = iQTotalChange;
                                        oCurrentStep.QVATChange = iQVATChange;
                                        oCurrentStep.QPlusIVAChange = iQPlusIVAChange;
                                        oCurrentStep.FeePlusIVAChange = iFeePlusIVAChange;
                                        oCurrentStep.QWithoutBonChange = iQWithoutBonChange;
                                    }
                                    parametersOut["q2"] = iQ;
                                    parametersOut["t2"] = iTime;

                                }

                            }
                            else
                            {


                                //if (oGroup.INSTALLATION.INS_SHORTDESC != "SLP")
                                //{

                                if (iMaxAmountAllowedToPay.HasValue)
                                {
                                    if (iQTotal > iMaxAmountAllowedToPay)
                                    {
                                        rtRes = ResultType.Result_Error_Not_Enough_Balance; ;
                                        parametersOut["r"] = Convert.ToInt32(rtRes);
                                    }
                                }

                                if (rtRes == ResultType.Result_OK)
                                {

                                    if (bIsShopKeeperOperation)
                                    {
                                        customersRepository.CalculateShopKeeperProfit(iQTotal, dShopKeeperParkPerc, out iAmountProfit, out iAmountTotal);
                                        oCurrentStep.AmountProfit = iAmountProfit;
                                        oCurrentStep.AmountTotal = iAmountTotal;
                                    }

                                    parametersOut["q2"] = iQ;
                                    parametersOut["t2"] = iTime;

                                }
                            }

                            if (rtRes == ResultType.Result_OK)
                                oSteps.Add(oCurrentStep);

                            if (oSteps.Any())
                            {
                                var oStandardQueryParkingSteps = new StandardQueryParkingSteps() { Steps = oSteps.ToArray() };
                                parametersOut["steps"] = oStandardQueryParkingSteps.ToCustomXml();
                            }
                            else
                                parametersOut["steps"] = "";

                            // ***
                            /*if (oModifiers == null || oModifiers.Modifiers == null || !oModifiers.Modifiers.Any())
                            {
                                oModifiers = new BSMModifiers()
                                {
                                    Modifiers = new List<BSMModifier>() { new BSMModifier() { modifierId = "1", description = "Test modifier 1" } }.ToArray()
                                };
                            }*/
                            // ***
                            if (oModifiers != null && oModifiers.Modifiers.Any())
                            {
                                parametersOut["modifiers"] = oModifiers.ToCustomXml();
                                oConfiguration.Modifiers = oModifiers;
                            }

                            /*if (wsParameters.ContainsKey("d_exp"))
                            {
                                DateTime dtExpirationDate = DateTime.ParseExact(wsParameters[strXMLPrefix + "d_exp"].ToString(), "HHmmssddMMyyyy",
                                         CultureInfo.InvariantCulture);
                                parametersOut["dexp"] = dtExpirationDate.ToString("HHmmssddMMyy");
                            }*/

                        }
                    }
                }

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BSMPriceParking::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }
        public ResultType BSMPriceUnParking(USER oUser, GROUP oGroup, TARIFF oTariff, string sPlate, BSMConfiguration oConfiguration, DateTime dtStartTime, DateTime dtEndTime, 
                                            DateTime dtStartOpEndTime, int iStartOpAmount, int iStartOpAmountWithoutBon, decimal? dBonMlt, string sStartOpExternalBaseId, 
                                            string strCulture, string sLocale, int? iWSTimeout, 
                                            out BSMModifiers oModifiers, ref SortedList parametersOut, ref List<SortedList> lstRefunds, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            oModifiers = null;
            BSMDetailedPrice oDetailedPrice = null;
            lEllapsedTime = 0;

            try
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oGroup.INSTALLATION.INS_TIMEZONE_ID);
                DateTime dtUtcStartTime = TimeZoneInfo.ConvertTime(dtStartTime, tzi, TimeZoneInfo.Utc);                
                DateTime dtUtcEndTime = TimeZoneInfo.ConvertTime(dtEndTime, tzi, TimeZoneInfo.Utc);
                DateTime dtUtcStartOpEndTime = TimeZoneInfo.ConvertTime(dtStartOpEndTime, tzi, TimeZoneInfo.Utc);


                string sCurIsoCode = oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE;

                decimal dPrice;
                decimal dBasePrice;
                DateTime? dtOutUtcEndTime;
                BSMErrorResponse oErrorResponse;
                string strRateDescription = "";


                rtRes = BSMPrice(oGroup.INSTALLATION, sPlate, oConfiguration.cityId, oConfiguration.configurationId, oConfiguration.zoneTypeId, dtUtcStartTime, null, dtUtcEndTime, strCulture, sLocale, iWSTimeout, out dPrice, out dBasePrice, out strRateDescription, out dtOutUtcEndTime, out oModifiers, out oDetailedPrice,  out oErrorResponse, out lEllapsedTime);

                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    //if (dPrice != 0)
                    //{
                    if (dPrice < 0)
                    {
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        Logger_AddLogMessage(string.Format("BSMPriceUnParking::Negative Amount {0}", dPrice), LogLevels.logWARN);

                    }
                    else
                    {


                        decimal dPriceWithoutBon=dPrice;
                        int iTime = Convert.ToInt32((dtOutUtcEndTime.Value - dtUtcStartTime).TotalMinutes);
                        
                        dBonMlt = dBonMlt ?? 1;
                        dPrice = dPrice * dBonMlt.Value;

                        SortedList oRefund = new SortedList();

                        if ((oDetailedPrice != null) && (oDetailedPrice.basePrice > 0))
                        {
                            string strCurSymbol = oGroup.INSTALLATION.CURRENCy.CUR_SYMBOL ?? sCurIsoCode;
                            strCurSymbol = strCurSymbol.Trim();

                            oRefund["ServiceParkingBaseQuantityLbl"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}/h", oDetailedPrice.basePrice, strCurSymbol);
                            oRefund["ServiceParkingVariableQuantityLbl"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}/h", oDetailedPrice.rateApplied, strCurSymbol);

                        }
                        else if ((oDetailedPrice != null) && (oDetailedPrice.basePrice == 0))
                        {
                            oRefund["ServiceParkingBaseQuantityLbl"] = strRateDescription;
                            oRefund["ServiceParkingVariableQuantityLbl"] = "";

                        }
                        else
                        {
                            oRefund["ServiceParkingBaseQuantityLbl"] = strRateDescription;
                            oRefund["ServiceParkingVariableQuantityLbl"] = "";
                        }
                     

                        
                        oRefund["ins_id"] = oGroup.GRP_INS_ID;
                        oRefund["ExternalID"] = sStartOpExternalBaseId;
                        oRefund["p"] = sPlate;
                        oRefund["q"] = iStartOpAmount - Convert.ToInt32(Math.Round(dPrice * 100, MidpointRounding.AwayFromZero));
                        oRefund["q_rem"] =  Convert.ToInt32(Math.Round(dPrice * 100, MidpointRounding.AwayFromZero));
                        oRefund["t"] = Convert.ToInt32((dtUtcStartOpEndTime - dtOutUtcEndTime.Value).TotalMinutes);
                        oRefund["t_rem"] = Convert.ToInt32((dtOutUtcEndTime.Value - dtUtcStartTime).TotalMinutes);
                        oRefund["cur"] = oGroup.INSTALLATION.CURRENCy.CUR_ISO_CODE;

                        DateTime dt1 = dtStartTime;
                        DateTime dt2 = TimeZoneInfo.ConvertTime(dtOutUtcEndTime.Value, TimeZoneInfo.Utc, tzi);
                        DateTime dt_prev_end = dtStartOpEndTime;
                        oRefund["d1"] = dt1.ToString("HHmmssddMMyy");
                        oRefund["d2"] = dt2.ToString("HHmmssddMMyy");
                        oRefund["d_prev_end"] = dt_prev_end.ToString("HHmmssddMMyy");
                        oRefund["exp"] = (Conversions.RoundSeconds(dtStartTime) < Conversions.RoundSeconds(dt2)) ? "0" : "1";


                        oRefund["q_without_bon"] = iStartOpAmountWithoutBon-Convert.ToInt32(dPriceWithoutBon * 100);
                        oRefund["q_rem_without_bon"] = Convert.ToInt32(dPriceWithoutBon * 100);

                        oRefund["ad"] = oTariff.TAR_ID;
                        oRefund["g"] = oGroup.GRP_ID;

                        oRefund["base_oper_id"] = sStartOpExternalBaseId;

                        if (oModifiers != null && oModifiers.Modifiers.Any())
                        {
                            oRefund["modifiers"] = oModifiers.ToCustomXml();
                        }

                        NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        numberFormatProvider.NumberDecimalSeparator = ".";
                        oRefund["per_bon"] = dBonMlt.Value.ToString(numberFormatProvider);

                        lstRefunds.Add(oRefund);
                    }
                    //}
                    //else
                    //    rtRes = ResultType.Result_Error_Park_Min_Time_Not_Exceeded;
                }

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BSMPriceUnParking::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }
        /*public ResultType BSMPrice(INSTALLATION oInstallation, int iTicketId, DateTime? dtEndTime, string sLocale, out decimal dPrice, out decimal dBasePrice, out DateTime? dtOutEndTime, out BSMModifiers oModifiers, out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            return BSMPrice(oInstallation, null, null, null, null, null, iTicketId, dtEndTime, sLocale, out dPrice, out dBasePrice, out dtOutEndTime, out oModifiers, out oErrorResponse, out lEllapsedTime);
        }*/
        protected ResultType BSMPrice(INSTALLATION oInstallation, string sPlate, int? iCityId, int? iConfigurationId, int? iZoneTypeId, DateTime? dtUtcStartTime, 
                                      int? iTicketId, DateTime? dtUtcEndTime, string strCulture, string sLocale, int? iWSTimeout, out decimal dPrice, out decimal dBasePrice, 
                                      out string strRateDescription, out DateTime? dtOutUtcEndTime, out BSMModifiers oModifiers, out BSMDetailedPrice 
                                      oDetailedPrice,out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            dPrice = 0;
            dBasePrice = 0;
            dtOutUtcEndTime = null;
            oModifiers = null;
            oDetailedPrice=null;
            oErrorResponse = null;
            strRateDescription = "";

            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            int iLocalWSTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();


            try
            {
                string sBaseUrl = oInstallation.INS_PARK_WS_URL;
                string sUser = oInstallation.INS_PARK_WS_HTTP_USER;
                string sPassword = oInstallation.INS_PARK_WS_HTTP_PASSWORD;

                // ***
                /*sBaseUrl = "https://preapi.bsmsa.eu/ext/api";
                sUser = "xL6BxwxOgfXJ7wDEQD7oRT104iMa";
                sPassword = "3jhZBXTsG3jVhN78AsShrGgAoKka";*/
                // ***

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    long lLocalEllapsedTime = 0;
                    if (this.BSMToken(sBaseUrl, sUser, sPassword, iLocalWSTimeout, out sAccessToken, out lLocalEllapsedTime))
                    {
                        iLocalWSTimeout -= (int)lLocalEllapsedTime;
                        lEllapsedTime += lLocalEllapsedTime;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        //NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        //numberFormatProvider.NumberDecimalSeparator = ".";

                        string sUrl = string.Format("{0}/price?", sBaseUrl);

                        if (!string.IsNullOrEmpty(sPlate) && iCityId.HasValue && iConfigurationId.HasValue && iZoneTypeId.HasValue && dtUtcStartTime.HasValue)
                        {
                            string sStartTime = string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffZ}", dtUtcStartTime);
                            sStartTime = System.Net.WebUtility.UrlEncode(sStartTime);

                            sUrl = string.Format("{0}plateNumber={1}&cityId={2}&configurationId={3}&zoneTypeId={4}&startTime={5}&locale={6}", sUrl, sPlate, iCityId, iConfigurationId, iZoneTypeId, sStartTime, sLocale);

                        }
                        else if (iTicketId.HasValue)
                        {
                            sUrl = string.Format("{0}ticketId={1}&locale={2}", sUrl, iTicketId, sLocale);
                        }

                        if (dtUtcEndTime.HasValue)
                        {
                            string sEndTime = string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffZ}", dtUtcEndTime.Value);
                            sEndTime = System.Net.WebUtility.UrlEncode(sEndTime);
                            sUrl = string.Format("{0}&endTime={1}", sUrl, sEndTime);
                        }

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "GET";
                        oRequest.ContentType = "application/x-www-form-urlencoded";
                        //oRequest.ContentType = "application/json";
                        oRequest.Timeout = iLocalWSTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));
                        if (watch != null)
                        {
                            watch.Stop();
                            watch = null;
                        }

                        watch = Stopwatch.StartNew();

                        Logger_AddLogMessage(string.Format("BSMPrice request.url={0}, Timeout={2} request.authorization={1}", sUrl, sAccessToken, oRequest.Timeout), LogLevels.logINFO);

                        try
                        {

                            WebResponse response = oRequest.GetResponse();
                            // Display the status.
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;

                            iLocalWSTimeout -= (int)watch.ElapsedMilliseconds;
                            lEllapsedTime += watch.ElapsedMilliseconds;

                            if (oWebResponse.StatusCode == HttpStatusCode.OK)
                            {                                
                                Stream dataStream = response.GetResponseStream();                                
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();                                

                                Logger_AddLogMessage(string.Format("BSMPrice response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);                                

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                dPrice = (decimal)oResponse.price.Value;
                                dBasePrice = (decimal)oResponse.basePrice.Value;
                                if (oResponse.endTime != null)
                                    dtOutUtcEndTime = (DateTime)oResponse.endTime.Value;
                                oModifiers = BSMModifiers.Load(oResponse.modifiers);
                                if (oResponse.detailedPrice!=null)
                                    oDetailedPrice = new BSMDetailedPrice(oResponse.detailedPrice);

                                if (oModifiers != null && oModifiers.Modifiers.Any())
                                {
                                    foreach (BSMModifier oModifier in oModifiers.Modifiers)
                                    {
                                        oModifier.remark = oModifier.description +
                                                infraestructureRepository.GetLiteralFromKey(string.Format("{0}_{1}_{2}",Convert.ToInt32(oInstallation.INS_ID), oModifier.modifierId, "MODIFIER_REMARK"), strCulture);
                                    }
                                }

                                if (oResponse.rate!=null)
                                {
                                    strRateDescription = oResponse.rate.description;

                                    if (oResponse.rateText != null)
                                    {
                                        switch (strCulture.Substring(0,2).ToLower())
                                        {
                                            case "es":
                                                if (!string.IsNullOrEmpty(((string)oResponse.rateText.es)))
                                                {
                                                    strRateDescription = (string)oResponse.rateText.es;
                                                }
                                                break;
                                            case "ca":
                                                if (!string.IsNullOrEmpty(((string)oResponse.rateText.cat)))
                                                {
                                                    strRateDescription = (string)oResponse.rateText.cat;
                                                }
                                                break;
                                            case "en":
                                                if (!string.IsNullOrEmpty(((string)oResponse.rateText.en)))
                                                {
                                                    strRateDescription = (string)oResponse.rateText.en;
                                                }
                                                break;
                                            default:
                                                if (!string.IsNullOrEmpty(((string)oResponse.rateText.en)))
                                                {
                                                    strRateDescription = (string)oResponse.rateText.en;
                                                }
                                                break;


                                        }

                                    }

                                }

                                rtRes = ResultType.Result_OK;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sBSMAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = BSMErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new BSMErrorResponse()
                                    {
                                        status = ex.Status.ToString(),
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };
                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "BSMPrice::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "BSMPrice::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "BSMPrice::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        public ResultType BSMStart(int iWSNumber, INSTALLATION oInstallation, string sPlate, BSMConfiguration oConfiguration, DateTime dtUtcStartDate, decimal dLatitude, decimal dLongitude, int iQuantity,
                                   decimal? bon_mlt, decimal? bon_ext_mlt, int? iWSTimeout,
                                   ref SortedList parametersOut, out string str3dPartyOpNum, out string str3dPartyBaseOpNum, out DateTime? dtUtcIni, out DateTime? dtUtcEnd, out long lEllapsedTime, out bool bRequestTimeout)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            str3dPartyBaseOpNum = "";
            dtUtcIni = null;
            dtUtcEnd = null;
            lEllapsedTime = 0;
            bRequestTimeout = false;

            try 
            {

                int iTicketId = 0;
                decimal dAmount = 0;
                decimal dBasePrice = 0;
                BSMModifiers oModifiers = null;
                DateTime? dtOutUtcStartDate = null;
                DateTime? dtOutUtcMaxEndDate = null;
                List<DateTime> oOutUtcCronStartDates = null;
                List<DateTime> oOutUtcCronEndDates = null;
                BSMTicketStatus oStartStatus = null;
                BSMErrorResponse oErrorResponse = null;

                bool? bRetry = null;
                long lLocalEllapsed = 0;

                while (rtRes != ResultType.Result_OK && (!bRetry.HasValue || bRetry.Value))
                {
                    rtRes = BSMStart(iWSNumber, oInstallation, sPlate, oConfiguration.cityId, oConfiguration.configurationId, oConfiguration.zoneTypeId, oConfiguration.segmentId, dtUtcStartDate, dLatitude, dLongitude, iWSTimeout, 
                                     out iTicketId, out dAmount, out dBasePrice, out oModifiers, out dtOutUtcStartDate, out dtOutUtcMaxEndDate, out oOutUtcCronStartDates, out oOutUtcCronEndDates, out oStartStatus, out oErrorResponse, out lLocalEllapsed);
                    bRetry = (!bRetry.HasValue && rtRes != ResultType.Result_OK && oErrorResponse != null && oErrorResponse.timeout);
                    iWSTimeout -= (int)lEllapsedTime;
                    lEllapsedTime += lLocalEllapsed;

                }

               
                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {

                    if (dAmount < 0)
                    {
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        Logger_AddLogMessage(string.Format("BSMStart::Negative Amount {0}", dAmount), LogLevels.logWARN);

                    }
                    else
                    {
                        str3dPartyOpNum = iTicketId.ToString();
                        str3dPartyBaseOpNum = str3dPartyOpNum;

                        if (Convert.ToInt32(dAmount * 100) != iQuantity)
                        {
                            Logger_AddLogMessage(string.Format("BSMStart::different amounts price_amount={0} start_amount={1}", dAmount, iQuantity), LogLevels.logWARN);
                        }

                        str3dPartyOpNum = iTicketId.ToString();
                        str3dPartyBaseOpNum = iTicketId.ToString();

                        if (oOutUtcCronStartDates != null && oOutUtcCronStartDates.Any())
                            dtUtcIni = oOutUtcCronStartDates.First();
                        else
                            dtUtcIni = dtOutUtcStartDate;

                        if (oOutUtcCronEndDates != null && oOutUtcCronEndDates.Any())
                            dtUtcEnd = oOutUtcCronEndDates.Last();
                        else
                            dtUtcEnd = dtOutUtcMaxEndDate;
                    }
                }
                else if (oErrorResponse != null)
                {
                    bRequestTimeout = oErrorResponse.timeout;
                }
                

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BSMPrice::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }
        protected ResultType BSMStart(int iWSNumber, INSTALLATION oInstallation, string sPlate, int iCityId, int iConfigurationId, int iZoneTypeId, int iSegmentId, 
                                      DateTime dtUtcStartDate, decimal dLatitude, decimal dLongitude, int? iWSTimeout,
                                      out int iTicketId, out decimal dAmount, out decimal dBasePrice, out BSMModifiers oModifiers, out DateTime? dtOutUtcStartDate, 
                                      out DateTime? dtOutUtcMaxEndDate, out List<DateTime> oOutUtcCronStartDates, out List<DateTime> oOutUtcCronEndDates, 
                                      out BSMTicketStatus oStartStatus, out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            iTicketId = 0;
            dAmount = 0;
            dBasePrice = 0;            
            oModifiers = null;
            dtOutUtcStartDate = null;
            dtOutUtcMaxEndDate = null;
            oOutUtcCronStartDates = null;
            oOutUtcCronEndDates = null;
            oStartStatus = null;
            oErrorResponse = null;

            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            int iLocalWSTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();


            try
            {
                string sBaseUrl = "";
                string sUser = "";
                string sPassword = "";

                switch (iWSNumber)
                {
                    case 1:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        sUser = oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD;
                        break;

                    /*case 2:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        sUser = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD;
                        break;

                    case 3:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        sUser = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD;
                        break;*/

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("BSMStart::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }
                // ***
                /*sBaseUrl = "https://preapi.bsmsa.eu/ext/api";
                sUser = "xL6BxwxOgfXJ7wDEQD7oRT104iMa";
                sPassword = "3jhZBXTsG3jVhN78AsShrGgAoKka";*/
                // ***

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    long lLocalEllapsedTime = 0;
                    if (this.BSMToken(sBaseUrl, sUser, sPassword, iLocalWSTimeout, out sAccessToken, out lLocalEllapsedTime))
                    {
                        iLocalWSTimeout -= (int)lLocalEllapsedTime;
                        lEllapsedTime += lLocalEllapsedTime;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        //NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        //numberFormatProvider.NumberDecimalSeparator = ".";

                        string sUrl = string.Format("{0}/start", sBaseUrl);


                        Dictionary<string, object> oParamsDict = new Dictionary<string, object>();

                        oParamsDict["plateNumber"] = sPlate;
                        oParamsDict["segmentId"] = iSegmentId;
                        oParamsDict["cityId"] = iCityId;
                        oParamsDict["zoneTypeId"] = iZoneTypeId;
                        oParamsDict["configurationId"] = iConfigurationId;
                        oParamsDict["startDate"] = dtUtcStartDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                        oParamsDict["lng"] = dLongitude;
                        oParamsDict["lat"] = dLatitude;

                        var sJson = JsonConvert.SerializeObject(oParamsDict);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "POST";
                        //oRequest.ContentType = "application/x-www-form-urlencoded";
                        oRequest.ContentType = "application/json";
                        oRequest.Timeout = iLocalWSTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        Logger_AddLogMessage(string.Format("BSMStart request.url={0}, Timeout={3}, request.authorization={1}, request.json={2}", sUrl, sAccessToken, sJson, oRequest.Timeout), LogLevels.logINFO);

                        byte[] byteArray = Encoding.UTF8.GetBytes(sJson);

                        oRequest.ContentLength = byteArray.Length;                        
                        Stream dataStream = oRequest.GetRequestStream();                        
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        if (watch != null)
                        {
                            watch.Stop();
                            watch = null;
                        }

                        watch = Stopwatch.StartNew();


                        dataStream.Close();

                        try
                        {

                            WebResponse response = oRequest.GetResponse();                            
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;

                            iLocalWSTimeout -= (int)watch.ElapsedMilliseconds;
                            lEllapsedTime += watch.ElapsedMilliseconds;


                            if (oWebResponse.StatusCode == HttpStatusCode.OK)
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("BSMStart response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                iTicketId = (int)oResponse.ticketId.Value;
                                dAmount = (decimal)oResponse.amount.Value;
                                dBasePrice = (decimal)oResponse.basePrice.Value;
                                oModifiers = BSMModifiers.Load(oResponse.modifiers);
                                

                                if (oResponse.startDate != null)
                                    dtOutUtcStartDate = (DateTime)oResponse.startDate.Value;
                                if (oResponse.maxEndDate != null)
                                    dtOutUtcMaxEndDate = (DateTime)oResponse.maxEndDate.Value;
                                if (oResponse.cronStartDates != null)
                                {
                                    oOutUtcCronStartDates = new List<DateTime>();
                                    foreach (var oDate in oResponse.cronStartDates)
                                    {
                                        oOutUtcCronStartDates.Add((DateTime)oDate.Value);
                                    }
                                }
                                if (oResponse.cronEndDates != null)
                                {
                                    oOutUtcCronEndDates = new List<DateTime>();
                                    foreach (var oDate in oResponse.cronEndDates)
                                    {
                                        oOutUtcCronEndDates.Add((DateTime)oDate.Value);
                                    }
                                }
                                if (oResponse.startStatus != null)
                                {
                                    oStartStatus = new BSMTicketStatus()
                                    {
                                        ticketStatusId = (int)oResponse.startStatus.ticketStatusId.Value,
                                        description = oResponse.startStatus.description.Value
                                    };
                                }

                                rtRes = ResultType.Result_OK;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sBSMAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = BSMErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new BSMErrorResponse()
                                    {
                                        status = ex.Status.ToString(),
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };
                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "BSMStart::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "BSMStart::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "BSMStart::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        public ResultType BSMStop(int iWSNumber, INSTALLATION oInstallation, int iTicketId, DateTime dtUtcEndTime, decimal dLatitude, decimal dLongitude, int iQuantity, int? iWSTimeout,
                                  ref SortedList parametersOut, out string str3dPartyOpNum, out DateTime? dtOutUtcEnd, out int iOutQuantity, 
                                  out long lEllapsedTime, out bool bRequestTimeout)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            dtOutUtcEnd = null;
            lEllapsedTime = 0;
            bRequestTimeout = false;
            iOutQuantity = iQuantity;

            try
            {
                decimal dAmount = 0;
                DateTime? dtOutUtcStopDate = null;
                BSMTicketStatus oStartStatus = null;
                BSMTicketStatus oEndStatus = null;
                BSMErrorResponse oErrorResponse = null;

                bool? bRetry = null;
                long  lLocalEllapsed= 0;
                while (rtRes != ResultType.Result_OK && (!bRetry.HasValue || bRetry.Value))
                {
                    
                    rtRes = BSMStop(iWSNumber, oInstallation, iTicketId, dtUtcEndTime, dLatitude, dLongitude, iWSTimeout,
                                    out dAmount, out dtOutUtcStopDate, out oStartStatus, out oEndStatus, out oErrorResponse, out lLocalEllapsed);
                    bRetry = (!bRetry.HasValue && rtRes != ResultType.Result_OK && oErrorResponse != null && oErrorResponse.timeout);
                    iWSTimeout -= (int)lEllapsedTime;
                    lEllapsedTime += lLocalEllapsed;

                }


                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    if (dAmount < 0)
                    {
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        Logger_AddLogMessage(string.Format("BSMStop::Negative Amount {0}", dAmount), LogLevels.logWARN);

                    }
                    else
                    {

                        dtOutUtcEnd = dtOutUtcStopDate;
                        iOutQuantity = Convert.ToInt32(dAmount * 100);
                        if (iOutQuantity != iQuantity)
                        {
                            Logger_AddLogMessage(string.Format("BSMStop::different amounts price_amount={0} stop_amount={1}", iQuantity, iOutQuantity), LogLevels.logWARN);
                        }
                    }
                }
                else if (oErrorResponse != null)
                {
                    bRequestTimeout = oErrorResponse.timeout;
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BSMStop::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }
        public ResultType BSMStop(int iWSNumber, INSTALLATION oInstallation, int iTicketId, DateTime dtUtcEndTime, decimal dLatitude, decimal dLongitude, int? iWSTimeout,
                                  out decimal dAmount, out DateTime? dtOutUtcStopDate, out BSMTicketStatus oStartStatus, out BSMTicketStatus oEndStatus, 
                                  out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;            
            dAmount = 0;
            dtOutUtcStopDate = null;
            oStartStatus = null;
            oEndStatus = null;
            oErrorResponse = null;

            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            int iLocalWSTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();


            try
            {
                string sBaseUrl = "";
                string sUser = "";
                string sPassword = "";
                switch (iWSNumber)
                {
                    case 1:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        sUser = oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD;
                        break;

                    /*case 2:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        sUser = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD;
                        break;

                    case 3:
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        sUser = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD;
                        break;*/

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("BSMStop::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }
                }
                // ***
                /*sBaseUrl = "https://preapi.bsmsa.eu/ext/api";
                sUser = "xL6BxwxOgfXJ7wDEQD7oRT104iMa";
                sPassword = "3jhZBXTsG3jVhN78AsShrGgAoKka";*/
                // ***

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    long lLocalEllapsedTime = 0;
                    if (this.BSMToken(sBaseUrl, sUser, sPassword, iLocalWSTimeout, out sAccessToken, out lLocalEllapsedTime))
                    {
                        iLocalWSTimeout -= (int)lLocalEllapsedTime;
                        lEllapsedTime += lLocalEllapsedTime;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        //NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        //numberFormatProvider.NumberDecimalSeparator = ".";

                        string sUrl = string.Format("{0}/stop", sBaseUrl);


                        Dictionary<string, object> oParamsDict = new Dictionary<string, object>();

                        oParamsDict["ticketId"] = iTicketId;
                        oParamsDict["lng"] = dLongitude;
                        oParamsDict["lat"] = dLatitude;
                        oParamsDict["endTime"] = dtUtcEndTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                        var sJson = JsonConvert.SerializeObject(oParamsDict);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "POST";
                        //oRequest.ContentType = "application/x-www-form-urlencoded";
                        oRequest.ContentType = "application/json";
                        oRequest.Timeout = iLocalWSTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        Logger_AddLogMessage(string.Format("BSMStop request.url={0}, Timeout={3}, request.authorization={1}, request.json={2}", sUrl, sAccessToken, sJson, oRequest.Timeout), LogLevels.logINFO);

                        byte[] byteArray = Encoding.UTF8.GetBytes(sJson);

                        oRequest.ContentLength = byteArray.Length;                        
                        Stream dataStream = oRequest.GetRequestStream();                        
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        if (watch!=null)
                        {
                            watch.Stop();
                            watch = null;
                        }
                        watch = Stopwatch.StartNew();

                        dataStream.Close();

                        try
                        {

                            WebResponse response = oRequest.GetResponse();                            
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;
                            iLocalWSTimeout -= (int)watch.ElapsedMilliseconds;
                            lEllapsedTime += watch.ElapsedMilliseconds;


                            if (oWebResponse.StatusCode == HttpStatusCode.OK)
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("BSMStop response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);
                                
                                dAmount = (decimal)oResponse.amount.Value;
                                if (oResponse.stopDate != null)
                                    dtOutUtcStopDate = (DateTime)oResponse.stopDate.Value;
                                if (oResponse.startStatus != null)
                                {
                                    oStartStatus = new BSMTicketStatus()
                                    {
                                        ticketStatusId = (int)oResponse.startStatus.ticketStatusId.Value,
                                        description = oResponse.startStatus.description.Value
                                    };
                                }
                                if (oResponse.endStatus != null)
                                {
                                    oEndStatus = new BSMTicketStatus()
                                    {
                                        ticketStatusId = (int)oResponse.endStatus.ticketStatusId.Value,
                                        description = oResponse.endStatus.description.Value
                                    };
                                }

                                rtRes = ResultType.Result_OK;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sBSMAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = BSMErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new BSMErrorResponse()
                                    {
                                        status = ex.Status.ToString(),
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };
                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "BSMStop::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "BSMStop::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "BSMStop::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        #region Emisalba integration

        public ResultType EmisalbaConfirmParking(int iWSNumber, string strPlate, DateTime dtUTCInsertionDate, USER oUser, INSTALLATION oInstallation, decimal? dGroupId, decimal? dTariffId, 
                                                 int iQuantity, DateTime dtIni, DateTime dtEnd,
                                                 ChargeOperationsType operationType, string sExternalId, int? iWSTimeout,
                                                 ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string sJsonIn = "";
            string sJsonOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                // PRODUCCIOÓN: http:// 195.235.246.52:10136/api/Pagos/Post
                // PRUEBAS: http:// 195.235.246.52:10137/api/Pagos/Post

                string strURL = "";                
                var oExternUser = new EmisalbaExternUser();
                oExternUser.empresa = (ConfigurationManager.AppSettings["EmisalbaCompanyName"] ?? "");
                

                switch (iWSNumber)
                {
                    case 1:
                        strURL = oInstallation.INS_PARK_CONFIRM_WS_URL;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER))
                        {
                            oExternUser.user = oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER;
                            oExternUser.password = oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD;                            
                        }
                        break;

                    case 2:
                        strURL = oInstallation.INS_PARK_CONFIRM_WS2_URL;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER))
                        {
                            oExternUser.user = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER;
                            oExternUser.password = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD;
                        }
                        break;

                    case 3:
                        strURL = oInstallation.INS_PARK_CONFIRM_WS3_URL;
                        if (!string.IsNullOrEmpty(oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER))
                        {
                            oExternUser.user = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER;
                            oExternUser.password = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD;
                        }
                        break;

                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("EmisalbaConfirmParking::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strExtTariffId = "";
                string strExtGroupId = "";
                if (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iWSNumber, dGroupId.Value, dTariffId.Value, ref strExtGroupId, ref strExtTariffId))
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage("EmisalbaConfirmParking::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                }

                EmisalbaRequestMsgBase oMessage = null;
                if (operationType == ChargeOperationsType.ParkingOperation)
                    oMessage = new EmisalbaInsertarPago()
                    {
                        ExternUser = oExternUser,
                        matricula = strPlate,
                        fechaInicio = dtIni.ToString("dd/MM/yyyy HH:mm:ss"),
                        fechaFin = dtEnd.ToString("dd/MM/yyyy HH:mm:ss"),
                        idSector = Convert.ToInt32(strExtGroupId),
                        importeTotal = (iQuantity * 0.01).ToString().Replace(".", ",")
                    };
                else
                    oMessage = new EmisalbaActualizarEstacionamiento()
                    {
                        ExternUser = oExternUser,
                        idPago = Convert.ToInt64(sExternalId),
                        fechaFin = dtEnd.ToString("dd/MM/yyyy HH:mm:ss"),
                        importeTotal = (iQuantity * 0.01).ToString().Replace(".", ",")
                    };

                var oMsgRequest = new EmisalbaRequest()
                {
                    action = (operationType == ChargeOperationsType.ParkingOperation ? "InsertarPago" : "ActualizarEstacionamiento"),
                    message = oMessage
                };

                sJsonIn = JsonConvert.SerializeObject(oMsgRequest);

                Logger_AddLogMessage(string.Format("EmisalbaConfirmParking request.url={0}, Timeout={2}, request.json={1}", strURL, sJsonIn, request.Timeout), LogLevels.logINFO);

                watch = Stopwatch.StartNew();

                byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                request.ContentLength = byteArray.Length;                
                Stream dataStream = request.GetRequestStream();                
                dataStream.Write(byteArray, 0, byteArray.Length);                
                dataStream.Close();                

                try
                {
                    WebResponse response = request.GetResponse();                    
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                    lEllapsedTime = watch.ElapsedMilliseconds;

                    if (oWebResponse.StatusDescription == "OK")
                    {                        
                        dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        string responseFromServer = reader.ReadToEnd();
                        Logger_AddLogMessage(string.Format("EmisalbaConfirmParking response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);                        

                        sJsonOut = responseFromServer;

                        var oResponse = (EmisalbaResponse<EmisalbaPago>)JsonConvert.DeserializeObject(responseFromServer, typeof(EmisalbaResponse<EmisalbaPago>));

                        rtRes = Convert_ResultTypeEmisalba_TO_ResultType(Convert.ToInt32(oResponse.error), oResponse.message.error);

                        parametersOut["r"] = Convert.ToInt32(rtRes);

                        if (rtRes == ResultType.Result_OK)
                        {
                            if (oResponse.message.idPago.HasValue)
                                str3dPartyOpNum = oResponse.message.idPago.Value.ToString();
                            else
                                str3dPartyOpNum = sExternalId;
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    if ((watch != null) && (lEllapsedTime == 0))
                    {
                        lEllapsedTime = watch.ElapsedMilliseconds;
                    }
                    Logger_AddLogException(e, "EmisalbaConfirmParking::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "EmisalbaConfirmParking::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sJsonIn, sJsonOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;

        }

        #endregion

        #region SIR integration

        public ResultType SIRConfirmPayment(int iWS, INSTALLATION oInstallation, string sCuentaMP, DateTime dtPaymentDate, string sExternalReference, double dAmount, int iWSTimeout,
                                            out string str3dPartyOpNum, out SIRResponse<SIRCobroResponse> oResponse, out long lEllapsedTime)
        {
            ResultType rt = ResultType.Result_Error_Generic;
            oResponse = null;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;

            Stopwatch watch = null;
            Exception oNotificationEx = null;

            try
            {
                string sTokenBaseUrl = "";
                string sBaseUrl = "";
                string sUser = "";
                string sPassword = "";
                switch (iWS)
                {
                    case 1:
                        sTokenBaseUrl = oInstallation.INS_PARK_CONFIRM_WS_URL.Split('~')[0];
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS_URL.Split('~')[1];
                        sUser = oInstallation.INS_PARK_CONFIRM_WS_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS_HTTP_PASSWORD;
                        break;
                    case 2:
                        sTokenBaseUrl = oInstallation.INS_PARK_CONFIRM_WS2_URL.Split('~')[0];
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS2_URL.Split('~')[1];
                        sUser = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS2_HTTP_PASSWORD;
                        break;
                    case 3:
                        sTokenBaseUrl = oInstallation.INS_PARK_CONFIRM_WS3_URL.Split('~')[0];
                        sBaseUrl = oInstallation.INS_PARK_CONFIRM_WS3_URL.Split('~')[1];
                        sUser = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_USER;
                        sPassword = oInstallation.INS_PARK_CONFIRM_WS3_HTTP_PASSWORD;
                        break;
                }

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    if (this.SIRToken(sTokenBaseUrl, sUser, sPassword, out sAccessToken))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        string sUrl = string.Format(sBaseUrl + "{0}Gateway/sac/cobro", (!sBaseUrl.EndsWith("/") ? "/" : ""));

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        request.Accept = "application/json";
                        request.Timeout = Get3rdPartyWSTimeout();
                        request.Headers.Add(string.Format("Authorization: Bearer {0}", sAccessToken));
                        
                        //Dictionary<string, object> oParamsDict = new Dictionary<string, object>();
                        //oParamsDict["plateNumber"] = sPlate;
                        //oParamsDict["segmentId"] = iSegmentId;
                        //oParamsDict["cityId"] = iCityId;
                        //oParamsDict["zoneTypeId"] = iZoneTypeId;
                        //oParamsDict["configurationId"] = iConfigurationId;
                        //oParamsDict["startDate"] = dtUtcStartDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                        //oParamsDict["lng"] = dLongitude;
                        //oParamsDict["lat"] = dLatitude;

                        SIRCobro oCobro = new SIRCobro()
                        {
                            cuentaMP = sCuentaMP,
                            codigo = sExternalReference,
                            importe = dAmount
                        };
                        oCobro.SetFecha(dtPaymentDate);

                        var sJson = JsonConvert.SerializeObject(oCobro);

                        Logger_AddLogMessage(string.Format("SIRConfirmPayment request.url={0}, request.body={1}, Authorization: Bearer {2}", sUrl, sJson, sAccessToken), LogLevels.logINFO);

                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(sJson);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        if (watch != null)
                        {
                            watch.Stop();
                            watch = null;
                        }

                        watch = Stopwatch.StartNew();

                        try
                        {
                            using (WebResponse response = request.GetResponse())
                            {
                                bAuthRetry = false;

                                using (Stream strReader = response.GetResponseStream())
                                {
                                    lEllapsedTime += watch.ElapsedMilliseconds;

                                    if (strReader == null)
                                    {
                                        rt = ResultType.Result_Error_Generic;                                        
                                    }
                                    using (StreamReader objReader = new StreamReader(strReader))
                                    {
                                        string responseBody = objReader.ReadToEnd();

                                        Logger_AddLogMessage(string.Format("SIRConfirmPayment response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                        oResponse = JsonConvert.DeserializeObject<SIRResponse<SIRCobroResponse>>(responseBody);

                                        if (oResponse != null)
                                        {
                                            if (oResponse.success)
                                            {
                                                rt = ResultType.Result_OK;
                                                str3dPartyOpNum = oResponse.data.id;
                                            }
                                            else
                                            {
                                                // ...
                                                rt = ResultType.Result_Error_Generic;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            if (watch != null)
                                lEllapsedTime = watch.ElapsedMilliseconds;


                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sPermitsAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oResponse = SIRResponse<SIRCobroResponse>.Load((HttpWebResponse)ex.Response);
                                else
                                    oResponse = new SIRResponse<SIRCobroResponse>()
                                    {
                                        success = false,
                                        existsErrorMessages = true,
                                        messages = new List<SIRErrorMessage>() { new SIRErrorMessage() { esError = true, text = ex.Status.ToString() } }.ToArray(),
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };
                                //rt = oResponse.GetResultType();
                                rt = ResultType.Result_Error_Generic;
                            }
                            Logger_AddLogException(ex, "SIRConfirmPayment::WebException", LogLevels.logERROR);
                        }
                        catch (Exception ex)
                        {
                            if (watch != null)
                                lEllapsedTime = watch.ElapsedMilliseconds;

                            bAuthRetry = false;                            
                            rt = ResultType.Result_Error_Generic;
                            Logger_AddLogException(ex, "SIRConfirmPayment::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                    {
                        rt = ResultType.Result_Error_InvalidAuthentication;
                    }
                }
            }
            catch (Exception ex)
            {
                if (watch != null)
                    lEllapsedTime = watch.ElapsedMilliseconds;

                oNotificationEx = ex;
                rt = ResultType.Result_Error_Generic;                
                Logger_AddLogException(ex, "SIRConfirmPayment::Exception", LogLevels.logERROR);
            }

            return rt;
        } 

        #endregion

    }

    public class CustomXmlBase<T>
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(T));

        public static String RemoveNilTrue(String xmlContent, String schemePrefix = "xsi")
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDocument.NameTable);

            bool schemeExist = false;

            foreach (XmlAttribute attr in xmlDocument.DocumentElement.Attributes)
            {
                if (attr.Prefix.Equals("xmlns", StringComparison.InvariantCultureIgnoreCase)
                    && attr.LocalName.Equals(schemePrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    nsMgr.AddNamespace(attr.LocalName, attr.Value);
                    schemeExist = true;
                    break;
                }
            }

            // scheme exists - remove nodes
            if (schemeExist)
            {
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//*[@" + schemePrefix + ":nil='true']", nsMgr);

                foreach (XmlNode xmlNode in xmlNodeList)
                    xmlNode.ParentNode.RemoveChild(xmlNode);

                return xmlDocument.InnerXml;
            }
            else
                return xmlContent;
        }

        protected string ToCustomXml(string sArrayTagName)
        {
            string sXml = this.XmlSerializeToString();

            //m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("ToCustomXml: serlize1='{0}'", sXml));
            sXml = StandardQueryParkingSteps.RemoveNilTrue(sXml);
            //m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("ToCustomXml: serlize2='{0}'", sXml));
            string sTagOpen = string.Format("<{0}>", sArrayTagName);
            string sTagClose = string.Format("</{0}>", sArrayTagName);
            int iIniPos = sXml.IndexOf(sTagOpen) + (sTagOpen).Length;
            int iEndPos = sXml.IndexOf(sTagClose);
            sXml = sXml.Substring(iIniPos, iEndPos - iIniPos).Replace("jsonArray", "json:Array");
            //m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("ToCustomXml: serlize3='{0}'", sXml));
            return sXml;
        }
    }

    public class StandardQueryParkingSteps : CustomXmlBase<StandardQueryParkingSteps>
    {
        [System.Xml.Serialization.XmlArrayItem("step")]  
        public StandardQueryParkingStep[] Steps { get; set; }

        public string ToCustomXml()
        {
            string sXml = this.XmlSerializeToString();

            //m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("ToCustomXml: serlize1='{0}'", sXml));
            sXml = StandardQueryParkingSteps.RemoveNilTrue(sXml);
            //m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("ToCustomXml: serlize2='{0}'", sXml));
            int iIniPos = sXml.IndexOf("<Steps>") + ("<Steps>").Length;
            int iEndPos = sXml.IndexOf("</Steps>");
            sXml = sXml.Substring(iIniPos, iEndPos - iIniPos).Replace("jsonArray", "json:Array");
            //m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("ToCustomXml: serlize3='{0}'", sXml));
            return sXml;
        }

        public static StandardQueryParkingSteps FromCustomXml(string sXml)
        {
            StandardQueryParkingSteps oRet = null;

            try
            {
                sXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><StandardQueryParkingSteps xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Steps>{0}</Steps></StandardQueryParkingSteps>", sXml);
                sXml = sXml.Replace("json:Array=\"true\"", "");
                oRet = (StandardQueryParkingSteps)Conversions.XmlDeserializeFromString(sXml, typeof(StandardQueryParkingSteps));
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "StandardQueryParkingSteps::FromCustomXml", ex);
            }

            return oRet;
        }

        
    }

    public class StandardQueryParkingStep
    {
        public StandardQueryParkingStep()
        {
            JsonArray = "true";
            Selectable = 1;
            Default = 0;
        }

        [System.Xml.Serialization.XmlAttribute(@"jsonArray")]
        public string JsonArray { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "t")]
        public int Time { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "q")]
        public int Q { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "qch")]
        public int? QChange { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        public DateTime Dt { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "d")]
        public string Dt_String
        {
            get { return string.Format("{0:HHmmssddMMyy}", this.Dt); }
            set { Dt = DateTime.ParseExact(value, "HHmmssddMMyy", System.Globalization.CultureInfo.InvariantCulture); }
        }

        [System.Xml.Serialization.XmlElement(ElementName = "q_fee")]
        public int QFEE { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "q_vat")]
        public int QVAT { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "q_subtotal")]
        public int QSubTotal { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "q_total")]
        public int QTotal { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_fee")]
        public int? QFEEChange { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_vat")]
        public int? QVATChange { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_subtotal")]
        public int? QSubTotalChange { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_total")]
        public int? QTotalChange { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "time_bal_used")]
        public int FreeTimeUsed { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "real_q")]
        public int RealQ { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "q_shopkeeper_profit")]
        public int? AmountProfit { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "q_shopkeeper")]
        public int? AmountTotal { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "q_plus_vat")]
        public int QPlusIVA { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "q_fee_plus_vat")]
        public int FeePlusIVA { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_plus_vat")]
        public int? QPlusIVAChange { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_fee_plus_vat")]
        public int? FeePlusIVAChange { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "q_without_bon")]
        public int QWithoutBon { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "qch_without_bon")]
        public int? QWithoutBonChange { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "per_bon")]
        public decimal BonMlt { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "num_days")]
        public int NumDays { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "num_days_colour")]
        public string NumDaysColour { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "parking_base_quantity_lbl")]
        public string ParkingBaseQuantityLbl { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "parking_variable_quantity_lbl")]
        public string ParkingVariableQuantityLbl { get; set; }




        [System.Xml.Serialization.XmlElement(ElementName = "label")]
        //[System.Xml.Serialization.XmlIgnore()]
        public string Label { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "selectable")]
        public int Selectable { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "default")]
        public int Default { get; set; }

        public void AddStepsAmounts(List<StandardQueryParkingStep> oSteps)
        {
            if (oSteps != null && oSteps.Any())
            {
                this.Q += oSteps.Sum(s => s.Q);
                if (oSteps.Where(s => s.QChange.HasValue).Any())
                {
                    if (this.QChange.HasValue)
                        this.QChange += oSteps.Sum(s => (s.QChange ?? 0));
                    else
                        this.QChange = oSteps.Sum(s => (s.QChange ?? 0));
                }
                this.QFEE += oSteps.Sum(s => s.QFEE);
                this.QVAT += oSteps.Sum(s => s.QVAT);
                this.QSubTotal += oSteps.Sum(s => s.QSubTotal);
                this.QTotal += oSteps.Sum(s => s.QTotal);
                if (oSteps.Where(s => s.QFEEChange.HasValue).Any())
                {
                    if (this.QFEEChange.HasValue)
                        this.QFEEChange += oSteps.Sum(s => (s.QFEEChange ?? 0));
                    else
                        this.QFEEChange = oSteps.Sum(s => (s.QFEEChange ?? 0));
                }
                if (oSteps.Where(s => s.QVATChange.HasValue).Any())
                {
                    if (this.QVATChange.HasValue)
                        this.QVATChange += oSteps.Sum(s => (s.QVATChange ?? 0));
                    else
                        this.QVATChange = oSteps.Sum(s => (s.QVATChange ?? 0));
                }
                if (oSteps.Where(s => s.QSubTotalChange.HasValue).Any())
                {
                    if (this.QSubTotalChange.HasValue)
                        this.QSubTotalChange += oSteps.Sum(s => (s.QSubTotalChange ?? 0));
                    else
                        this.QSubTotalChange = oSteps.Sum(s => (s.QSubTotalChange ?? 0));
                }
                if (oSteps.Where(s => s.QTotalChange.HasValue).Any())
                {
                    if (this.QTotalChange.HasValue)
                        this.QTotalChange += oSteps.Sum(s => (s.QTotalChange ?? 0));
                    else
                        this.QTotalChange = oSteps.Sum(s => (s.QTotalChange ?? 0));
                }
                this.RealQ += oSteps.Sum(s => s.RealQ);
                if (oSteps.Where(s => s.AmountProfit.HasValue).Any())
                {
                    if (this.AmountProfit.HasValue)
                        this.AmountProfit += oSteps.Sum(s => (s.AmountProfit ?? 0));
                    else
                        this.AmountProfit = oSteps.Sum(s => (s.AmountProfit ?? 0));
                }
                if (oSteps.Where(s => s.AmountTotal.HasValue).Any())
                {
                    if (this.AmountTotal.HasValue)
                        this.AmountTotal += oSteps.Sum(s => (s.AmountTotal ?? 0));
                    else
                        this.AmountTotal = oSteps.Sum(s => (s.AmountTotal ?? 0));
                }
                this.QPlusIVA += oSteps.Sum(s => s.QPlusIVA);
                this.FeePlusIVA += oSteps.Sum(s => s.FeePlusIVA);
                if (oSteps.Where(s => s.QPlusIVAChange.HasValue).Any())
                {
                    if (this.QPlusIVAChange.HasValue)
                        this.QPlusIVAChange += oSteps.Sum(s => (s.QPlusIVAChange ?? 0));
                    else
                        this.QPlusIVAChange = oSteps.Sum(s => (s.QPlusIVAChange ?? 0));
                }
                if (oSteps.Where(s => s.FeePlusIVAChange.HasValue).Any())
                {
                    if (this.FeePlusIVAChange.HasValue)
                        this.FeePlusIVAChange += oSteps.Sum(s => (s.FeePlusIVAChange ?? 0));
                    else
                        this.FeePlusIVAChange = oSteps.Sum(s => (s.FeePlusIVAChange ?? 0));
                }
                this.QWithoutBon += oSteps.Sum(s => s.QWithoutBon);
                if (oSteps.Where(s => s.QWithoutBonChange.HasValue).Any())
                {
                    if (this.QWithoutBonChange.HasValue)
                        this.QWithoutBonChange += oSteps.Sum(s => (s.QWithoutBonChange ?? 0));
                    else
                        this.QWithoutBonChange = oSteps.Sum(s => (s.QWithoutBonChange ?? 0));
                }                

            }
        }

    }

    #region BSM Definitions

    public class BSMConfigurations : CustomXmlBase<BSMConfiguration>
    {        
        [System.Xml.Serialization.XmlArrayItem("configuration")]
        public BSMConfiguration[] Configurations { get; set; }

        public string ToCustomXml()
        {
            if (this.Configurations != null && this.Configurations.Any())
                return base.ToCustomXml("Configurations");
            else
                return "";
        }

        public static BSMConfigurations Load(dynamic oConfigurations)
        {
            BSMConfigurations oRet = new BSMConfigurations();

            List<BSMConfiguration> oList = new List<BSMConfiguration>();

            if (oConfigurations != null)
            {
                foreach (dynamic oCity in oConfigurations)
                {
                    foreach (dynamic oConfig in oCity.configurations)
                    {
                        foreach (dynamic oSegment in oConfig.segment)
                        {
                            oList.Add(new BSMConfiguration(oCity, oConfig, oSegment));
                        }
                    }
                }
            }

            oRet.Configurations = oList.ToArray();

            return oRet;
        }

    }

    public class BSMConfiguration 
    {
        [System.Xml.Serialization.XmlAttribute(@"jsonArray")]
        public string JsonArray { get; set; }

        public int cityId { get; set; }
        public string cityName { get; set; }
        public int zoneTypeId { get; set; }
        public int configurationId { get; set; }
        public int segmentId { get; set; }

        public string configurationDescription { get; set; }
        public string zoneName { get; set; }
        public string zoneRGB { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public int? rateId { get; set; }
        public string rateDescription { get; set; }        
        public string rateCode { get; set; }
        public decimal? tarId { get; set; }
        public int? scheduleId { get; set; }
        public string scheduleDescription { get; set; }
        public string scheduleCode { get; set; }
        public string segmentName { get; set; }

        public string parkingBaseQuantityLbl  { get; set; }
        public string parkingVariableQuantityLbl { get; set; }

        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }

        public BSMModifiers Modifiers { get; set; }

        public BSMConfiguration()
        {
            JsonArray = "true";
        }

        public BSMConfiguration(dynamic oCity, dynamic oConfig, dynamic oSegment)
        {
            JsonArray = "true";

            cityId = (int)oCity.cityId.Value;
            cityName = oCity.cityName.Value;
            zoneTypeId = (int)oCity.zoneTypeId.Value;
            configurationId = (int)oConfig.configurationId.Value;
            segmentId = (int)oSegment.segmentId.Value;

            configurationDescription = oConfig.description.Value;
            zoneName = oConfig.zoneType.name.Value;
            zoneRGB = oConfig.zoneType.rgb.Value;
            rateId = (int)oConfig.rate.rateId.Value;
            rateDescription = oConfig.rate.description.Value;
            rateCode = oConfig.rate.rateCode.Value;
            scheduleId = (int)oConfig.schedule.scheduleId.Value;
            scheduleDescription = oConfig.schedule.description.Value;
            scheduleCode = oConfig.schedule.scheduleCode.Value;
            segmentName = oSegment.segmentName.Value;

        }        
    }

    public class BSMModifiers : CustomXmlBase<BSMModifiers>
    {
        [System.Xml.Serialization.XmlArrayItem("modifier")]
        public BSMModifier[] Modifiers { get; set; }

        public string ToCustomXml()
        {
            return base.ToCustomXml("Modifiers");


        }

        public static BSMModifiers Load(dynamic oModifiers)
        {
            BSMModifiers oRet = new BSMModifiers();

            List<BSMModifier> oList = new List<BSMModifier>();

            if (oModifiers != null)
            {
                foreach (dynamic oItem in oModifiers)
                {
                    oList.Add(new BSMModifier(oItem));
                }
            }

            oRet.Modifiers = oList.ToArray();

            return oRet;
        }
    }
    public class BSMModifier
    {
        [System.Xml.Serialization.XmlAttribute(@"jsonArray")]
        public string JsonArray { get; set; }

        public string modifierId { get; set; }
        public string description { get; set; }
        public string remark { get; set; }
        public string value { get; set; }

        public BSMModifier()
        {
            JsonArray = "true";
        }
        public BSMModifier(dynamic oModifier)
        {
            JsonArray = "true";

            modifierId = oModifier.modifierId.Value;
            description = oModifier.description.Value;
            remark = "";
            value="";
            
            switch ((string)oModifier.type.Value)
            {
                case "P":
                     value= oModifier.value.Value + " %";
                    break;
                case "A":
                     value= oModifier.value.Value + " €/h";
                    break;
                case "E":
                     value= oModifier.value.Value + " €";
                    break;
                default:
                    break;
            }            
        }
    }

    public class BSMDetailedPrice
    {            
        public double basePrice { get; set; }
        public double totalAmount { get; set; }
        public double rateApplied { get; set; }


        public BSMDetailedPrice(dynamic oDetailedPrice)
        {
            rateApplied = 0;
            basePrice = oDetailedPrice.basePrice.Value;
            totalAmount = oDetailedPrice.totalAmount.Value;
            if (oDetailedPrice.rateApplied!=null)
                rateApplied = oDetailedPrice.rateApplied.Value;
        }

    }

    public class BSMTicketStatus
    {
        public int ticketStatusId { get; set; }
        public string description { get; set; }
    }

    public class BSMErrorResponse
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(BSMErrorResponse));

        public string errorCode { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string[] errors { get; set; }
        public bool timeout { get; set; }

        public BSMErrorResponse()
        {
            timeout = false;
        }

        public static BSMErrorResponse Load(HttpWebResponse oResponse)
        {
            BSMErrorResponse oRet = new BSMErrorResponse();
            
            string sJsonResponse = "";

            try
            {                
                using (var stream = oResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    sJsonResponse = reader.ReadToEnd();
                }
                oRet = JsonConvert.DeserializeObject<BSMErrorResponse>(sJsonResponse);
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "BSMErrorResponse::Load Exception", ex);
                oRet.timeout = true;
            }

            if (oResponse != null)
            {
                try
                {
                    oRet.timeout = (oResponse.StatusCode == HttpStatusCode.RequestTimeout);
                }
                catch (Exception) { }
            }

            m_oLog.LogMessage(LogLevels.logINFO, string.Format("BSMErrorResponse::Load response={0}", PrettyJSON(sJsonResponse)));

            return oRet;
        }

        public ResultType GetResultType()
        {
            ResultType eRet = ResultType.Result_Error_Generic;

            if (this.errorCode == "error_15")
                eRet = ResultType.Result_Error_InvalidTicketId;
            // "error_152": "S'ha trobat un tiquet actiu"
            // "error_153": "NomÃ©s es pot reemborsar un mÃ xim del 100% de lâ€™import original"
            // "error_158": "Alguns dels parÃ metres obligatoris no estan informats"
            // "error_163": "El tiquet ja estÃ  parat"
            // "error_165": "MatrÃ­cula no vÃ lida",
            else if (this.errorCode == "error_213")
                eRet = ResultType.Result_Error_Park_Min_Time_Not_Exceeded;
            else if (this.timeout)
                eRet = ResultType.Result_Error_BSM_Not_Answering;

            return eRet;
        }



       

        protected static string PrettyJSON(string json)
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
    }

    #endregion

    #region Emisalba Definitions

    public class EmisalbaInsertarPago : EmisalbaRequestMsgBase
    {        
        public string matricula { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int idSector { get; set; }
        public string importeTotal { get; set; }
    }

    public class EmisalbaActualizarEstacionamiento : EmisalbaRequestMsgBase
    {
        public long idPago { get; set; }
        public string fechaFin { get; set; }
        public string importeTotal { get; set; }
    }

    public class EmisalbaPago : EmisalbaResponseMsgBase
    {
        public long? idPago { get; set; }
    }

    #endregion

    #region Madrid Definitions

    public class MadridPhyZone
    {
        public string codSystem { get; set; }
        public string codGeoZone { get; set; }
        public string codCity { get; set; }
        public string codPhyZone { get; set; }
    }
    public class MadridAuthRequest
    {
        public int initialTariff { get; set; }
        public string isoLang { get; set; }
        public string userCode { get; set; }
        public string dtRequest { get; set; }
        public MadridPhyZone phyZone { get; set; }

    }

    public class MadridAuthorizedTime
    {
        public string maxTime { get; set; }
    }
    public class MadridPayParking
    {
        public string prkEndUtc { get; set; }
        public string totTim { get; set; }
        public string prkBgnUtc { get; set; }
        public decimal totAmo { get; set; }
    }
    public class MadridLocalizedText
    {
        public string isoLang { get; set; }
        public string text { get; set; }
    }
    public class MadridTariffDefinition
    {
        public int baseId { get; set; }
        public decimal bonMlt { get; set; }
        public MadridLocalizedText[] displayTexts { get; set; }
    }
    public class MadridAuthResponse
    {
        public MadridAuthorizedTime authTime { get; set; }
        public int authId { get; set; }
        public MadridPayParking curOper { get; set; }
        public MadridTariffDefinition tarDef { get; set; }
    }

    public class MadridParkTrans
    {
        public int tariffId { get; set; }
        public string userPlate { get; set; }
        public string operationDateUTC { get; set; }
        public int authId { get; set; }
        public MadridPayParking parkingOper { get; set; }
        public string ticketNum { get; set; }
        public int transId { get; set; }
    }
    public class MadridPayTransactionRequest
    {
        public MadridPhyZone phyZone { get; set; }
        public MadridParkTrans prkTrans { get; set; }
    }
    public class MadridPayTransactionResponse
    {
        public int id { get; set; }
    }

    public class MadridErrorDetails
    {
        public string description { get; set; }
        public string propertyName { get; set; }
    }
    public class MadridError
    {
        public int code { get; set; }
        public MadridErrorDetails[] details { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
    public class MadridErrorResponse
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(MadridErrorResponse));

        public MadridError error { get; set; }
        public bool timeout { get; set; }

        public MadridErrorResponse()
        {
            timeout = false;
        }

        public static MadridErrorResponse Load(HttpWebResponse oResponse)
        {
            MadridErrorResponse oRet = new MadridErrorResponse();

            string sJsonResponse = "";

            try
            {
                using (var stream = oResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    sJsonResponse = reader.ReadToEnd();
                }
                oRet = JsonConvert.DeserializeObject<MadridErrorResponse>(sJsonResponse);
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "MadridErrorResponse::Load Exception", ex);
                oRet.timeout = true;
            }

            if (oResponse != null)
            {
                try
                {
                    oRet.timeout = (oResponse.StatusCode == HttpStatusCode.RequestTimeout);
                }
                catch (Exception ex) { }
            }

            m_oLog.LogMessage(LogLevels.logINFO, string.Format("MadridErrorResponse::Load response={0}", PrettyJSON(sJsonResponse)));

            return oRet;
        }

        public ResultType GetResultType()
        {
            ResultType eRet = ResultType.Result_Error_Generic;

            if (this.error != null)
            {
                switch (this.error.code)
                {
                    case 1: eRet = ResultType.Result_Error_Generic; break;
                    case 2: eRet = ResultType.Result_Error_Generic; break;
                    case 3: eRet = ResultType.Result_Error_ParkingMaximumTimeUsed; break;
                    case 10: eRet = ResultType.Result_Error_Generic /*Result_Error_ParkingAlreadyPayed*/; break;
                    default:
                        eRet = ResultType.Result_Error_Generic;
                        break;
                }

            }

            return eRet;
        }

        protected static string PrettyJSON(string json)
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
    }

    #endregion

    #region SIR Definitions
    
    public class SIRCobro : SIRBase
    {
        public string cuentaMP { get; set; }
        public string codigo { get; set; }
        public double importe { get; set; }

        private DateTime? dtFecha;
            
        public string fecha
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.dtFecha);
            }
            set
            {
                this.dtFecha = GetDateTimeFromStringFormat(value);
            }
        }

        public void SetFecha(DateTime dt) { dtFecha = dt; }
        public DateTime? GetFecha() { return this.dtFecha; }
    }

    public class SIRCobroResponse
    {
        public string id { get; set; }
        public string cuentaMP { get; set; }
        public string codigo { get; set; }
    }
    #endregion
}
