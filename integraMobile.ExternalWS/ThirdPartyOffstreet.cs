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
using System.IO;
using System.Net;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Ninject;
using Newtonsoft.Json;

namespace integraMobile.ExternalWS
{
    public enum ResultTypeMeyparOffstreetWS
    {
        ResultMOffstreet_OK = 1,
        ResultMOffstreet_Error_Generic = -9,
        ResultMOffstreet_Error_Invalid_Id = -15,
        ResultMOffstreet_Error_Invalid_Input_Parameter = -19,
        ResultMOffstreet_Error_Missing_Input_Parameter = -20,
        ResultMOffstreet_Error_OperationNotFound = -38,
        ResultMOffstreet_Error_OperationAlreadyClosed = -39,        
        ResultMOffstreet_Error_Max_Multidiscount_Reached = -54,
        ResultMOffstreet_Error_Discount_NotAllowed = -55,
        ResultMOffstreet_Error_InvoiceGeneration = -57
    }

    public enum ResultTypeIParkControlOffstreetWS
    {
        Result_OK = 1,
        Result_Error_InvalidAuthentication = -101,
        Result_Error_PlatePaymentBlacklist = -200,
        Result_Error_UnkownParkingOperation = -201,
        Result_Error_ParkingOperationExit = -202,
        Result_Error_ParkingOperationMissmatched = -203,
        Result_Error_ParkingOperationPaid = -204,
        Result_Error_TariffNotFound = -205,
        Result_Error_ParkingOpQueryExpired = -206,
        Result_PlatePaymentWithSpecialGrants = -210,
        Result_Error_GroupNotFound = -218,
        Result_Error_InstallationNotFound = -220,
        Result_Error_InvalidExternalProvider = -226,
        Result_Error_PaymentError = -300,
        Result_Error_PaymentDenied = -301,
        Result_Error_CurrencyNotFound = -302,
        Result_Error_CouponUnknown = -500,
        Result_Error_CouponAlreadyUsed = -501,
        Result_Error_CouponCanNotBeApplied = -502,
        Result_Error_CouponsMissmatched = -503,
        Result_Error_InstallationServicesMissmatched = -600,
        Result_Error_InvalidInputParameters = -900,
        Result_Error_MissingInputParameters = -901,
        Result_Error_InvalidAuthenticationHash = -920,
        Result_ErrorDetected = -990,
        Result_Error_Generic = -999
    }

    public enum ResultTypeMeyparAdventaOffstreetWS
    {
        Ok = 1,
        Reserva_Confirmada_Correctamente = 2,
        Reserva_Cancelada_Correctamente = 3,
        Error_autenticacion = -10,
        Dispositivo_No_Valido = -11,
        Usuario_No_Permitido = -12,
        Parking_Invalido = -13,
        Soporte_Invalido = -20,
        Soporte_No_Encontrado = -21,
        Error_Generación_Recipiente = -22,
        Descuento_No_Encontrado = -23,
        Operacion_No_Encontrada = -24,
        Abonado_No_Existente = -30,
        Token_No_Valido = -31,
        Error_Creacion_Reserva_Parking = -40,
        Reserva_Desconocida = -41,
        Reserva_Ya_Cancelada = -42,
        Tiempo_Confirmacion_Reserva_Agotado = -43,
        Emision_Nuevas_Reservas_Bloqueada = -44,
        Cupo_Maximo_Completo = -45,
        Limite_Tiempo_Creacion_Reserva_Excedido = -46,
        Limite_Tiempo_Cancelación_Reserva_Excedido = -47,
        Parametros_Entrada_Incorrectos = -900,
        Faltan_Parametros_Entrada = -901,
        Hash_Autenticacion_No_Valido = -920,
        Proveedor_Externo_No_Valido = -921,
        Parking_No_Permitido = -922,
        Error_Conexion_Servidor_Parking = -923,
        Error_Comunicacion_Servidor_Parking = -924,
        Error_Generico = -999	
    }

    public class ThirdPartyOffstreet : ThirdPartyBase
    {
        public ThirdPartyOffstreet()
            : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyOffstreet));
        }

        public ThirdPartyOffstreet(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, IGeograficAndTariffsRepository oGeograficAndTariffsRepository, IRetailerRepository oRetailerRepository)
            : base(oCustomersRepository, oInfraestructureRepository, oGeograficAndTariffsRepository, oRetailerRepository)
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyOffstreet));

        }

        public ResultType MeyparQueryCarExitforPayment(GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType,ref string sPlate, DateTime dtCurrentDate, int? iWSTimeout,
                                                       ref SortedList parametersOut, out int iOp, out int iAmount, out decimal dVAT, out string sCurIsoCode, out int iTime, out DateTime dtEntryDate, out DateTime dtEndDate,
                                                       out string sTariff, out DateTime dtExitLimitDate, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            iOp = 0;
            iAmount = 0;
            dVAT = 0;
            sCurIsoCode = "";
            iTime = 0;
            dtEntryDate = dtCurrentDate;
            dtEndDate = dtCurrentDate;
            sTariff = "";
            dtExitLimitDate = dtCurrentDate;
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                MeyparThirdPartyOffstreetWS.InterfazPublicaWebService oOffstreetWS = new MeyparThirdPartyOffstreetWS.InterfazPublicaWebService();
                oOffstreetWS.Url = oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_URL;
                oOffstreetWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER))
                {
                    oOffstreetWS.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_PASSWORD);
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                strAuthHash = CalculateStandardWSHash(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY,
                                                      string.Format("{0}{1}{2}{3}{4:HHmmssddMMyyyy}{5}",
                                                                    oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, sOpLogicalId, (int) oOpeIdType, sPlate, dtCurrentDate, strvers));

                strMessage = string.Format("<ipark_in>" +
                                           "<parking_id>{0}</parking_id>" +
                                           "<ope_id>{1}</ope_id>" +
                                           "<ope_id_type>{2}</ope_id_type>" +
                                           "<p>{3}</p>" +
                                           "<d>{4:HHmmssddMMyyyy}</d>" +
                                           "<vers>{5}</vers>" +
                                           "<ah>{6}</ah>" +
                                           "</ipark_in>",
                                           oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, sOpLogicalId, (int)oOpeIdType, sPlate, dtCurrentDate, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("MeyparQueryCarExitforPayment Timeout={1} xmlIn ={0}", sXmlIn, oOffstreetWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oOffstreetWS.thirdpquerycarexitforpayment(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("MeyparQueryCarExitforPayment  xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeMeyparOffstreetWS_TO_ResultType((ResultTypeMeyparOffstreetWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {
                        // ****
                        //wsParameters["q"] = "20";
                        //wsParameters["vat_perc"] = "0.21";
                        // ****

                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        parametersOut["parking_id"] = wsParameters["parking_id"];
                        parametersOut["ope_id"] = wsParameters["ope_id"];
                        parametersOut["ope_id_type"] = wsParameters["ope_id_type"];
                        parametersOut["plate"] = wsParameters["plate"];
                        if (wsParameters.ContainsKey("plate") && wsParameters["plate"].ToString().Length>0)
                            sPlate = wsParameters["plate"].ToString();
                        parametersOut["op"] = wsParameters["op"];
                        parametersOut["q"] = wsParameters["q"];
                        parametersOut["cur"] = wsParameters["cur"];
                        parametersOut["t"] = wsParameters["t"];
                        parametersOut["bd"] = wsParameters["bd"];
                        parametersOut["ed"] = wsParameters["ed"];
                        parametersOut["tar_id"] = wsParameters["tar_id"];
                        parametersOut["med"] = wsParameters["med"];


                        iOp = Convert.ToInt32(wsParameters["op"]);                        
                        iAmount = Convert.ToInt32(wsParameters["q"]);

                        NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        numberFormatProvider.NumberDecimalSeparator = ".";
                        string sVAT = "";
                        try
                        {
                            sVAT = wsParameters["vat_perc"].ToString();
                            if (sVAT.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                            decimal dTryVAT = Convert.ToDecimal(sVAT, numberFormatProvider);
                            dVAT = dTryVAT;
                        }
                        catch
                        {
                            dVAT = 0;
                        }
                        

                        sCurIsoCode = wsParameters["cur"].ToString();
                        iTime = Convert.ToInt32(wsParameters["t"]);
                        dtEntryDate = DateTime.ParseExact(wsParameters["bd"].ToString(), "HHmmssddMMyy",
                                                          CultureInfo.InvariantCulture);
                        dtEndDate = DateTime.ParseExact(wsParameters["ed"].ToString(), "HHmmssddMMyy",
                                                        CultureInfo.InvariantCulture);
                        sTariff = wsParameters["tar_id"].ToString();
                        dtExitLimitDate = DateTime.ParseExact(wsParameters["med"].ToString(), "HHmmssddMMyy",
                                                            CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                    }
                }
                
            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "MeyparQueryCarExitforPayment::Exception", LogLevels.logERROR);
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

        public ResultType MeyparQueryCarDiscountforPayment(GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, 
                                                           string sDiscountId, DateTime dtCurrentDate, int? iWSTimeout,
                                                           ref SortedList parametersOut, out int iOp, out int iInitialAmount, out int iFinalAmount, out decimal dVAT, out string sCurIsoCode
                                                            /*, out int iTime, out DateTime dtEntryDate, out DateTime dtEndDate,
                                                           out string sTariff, out DateTime dtExitLimitDate*/, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            iOp = 0;
            iInitialAmount = 0;
            iFinalAmount = 0;
            dVAT = 0;
            sCurIsoCode = "";
            /*iTime = 0;
            dtEntryDate = dtCurrentDate;
            dtEndDate = dtCurrentDate;
            sTariff = "";
            dtExitLimitDate = dtCurrentDate;*/
            lEllapsedTime = 0;
            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                MeyparThirdPartyOffstreetWS.InterfazPublicaWebService oOffstreetWS = new MeyparThirdPartyOffstreetWS.InterfazPublicaWebService();
                oOffstreetWS.Url = oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_URL;
                oOffstreetWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER))
                {
                    oOffstreetWS.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_PASSWORD);
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                strAuthHash = CalculateStandardWSHash(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY,
                                                      string.Format("{0}{1}{2}{3}{4:HHmmssddMMyyyy}{5}",
                                                                    oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, sOpLogicalId, (int)oOpeIdType, sDiscountId, dtCurrentDate, strvers));

                strMessage = string.Format("<ipark_in>" +
                                           "<parking_id>{0}</parking_id>" +
                                           "<ope_id>{1}</ope_id>" +
                                           "<ope_id_type>{2}</ope_id_type>" +
                                           "<dc_id>{3}</dc_id>" +                                           
                                           "<d>{4:HHmmssddMMyyyy}</d>" +
                                           "<vers>{5}</vers>" +
                                           "<ah>{6}</ah>" +
                                           "</ipark_in>",
                                           oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, sOpLogicalId, (int)oOpeIdType, sDiscountId, dtCurrentDate, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("MeyparQueryCarDiscountforPayment Timeout={1} xmlIn ={0}", sXmlIn, oOffstreetWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oOffstreetWS.thirdPQueryCarDiscountforPayment(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("MeyparQueryCarDiscountforPayment  xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeMeyparOffstreetWS_TO_ResultType((ResultTypeMeyparOffstreetWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        parametersOut["parking_id"] = wsParameters["parking_id"];
                        parametersOut["ope_id"] = wsParameters["ope_id"];
                        parametersOut["ope_id_type"] = wsParameters["ope_id_type"];
                        //parametersOut["plate"] = wsParameters["p"];
                        parametersOut["op"] = wsParameters["op"];
                        parametersOut["qi"] = wsParameters["qi"];
                        parametersOut["qf"] = wsParameters["qf"];
                        parametersOut["cur"] = wsParameters["cur"];
                        //parametersOut["t"] = wsParameters["t"];
                        //parametersOut["bd"] = wsParameters["bd"];
                        //parametersOut["ed"] = wsParameters["ed"];
                        //parametersOut["tar_id"] = wsParameters["tar_id"];
                        //parametersOut["med"] = wsParameters["med"];

                        iOp = Convert.ToInt32(wsParameters["op"]);
                        iInitialAmount = Convert.ToInt32(wsParameters["qi"]);
                        iFinalAmount = Convert.ToInt32(wsParameters["qf"]);
                        sCurIsoCode = wsParameters["cur"].ToString();

                        NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        numberFormatProvider.NumberDecimalSeparator = ".";
                        string sVAT = "";
                        try
                        {
                            sVAT = wsParameters["vat_perc"].ToString();
                            if (sVAT.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                            decimal dTryVAT = Convert.ToDecimal(sVAT, numberFormatProvider);
                            dVAT = dTryVAT;
                        }
                        catch
                        {
                            dVAT = 0;
                        }

                        /*iTime = Convert.ToInt32(wsParameters["t"]);
                        dtEntryDate = DateTime.ParseExact(wsParameters["bd"].ToString(), "HHmmssddMMyy",
                                                          CultureInfo.InvariantCulture);
                        dtEndDate = DateTime.ParseExact(wsParameters["ed"].ToString(), "HHmmssddMMyy",
                                                        CultureInfo.InvariantCulture);
                        sTariff = wsParameters["tar_id"].ToString();
                        dtExitLimitDate = DateTime.ParseExact(wsParameters["med"].ToString(), "HHmmssddMMyy",
                                                            CultureInfo.InvariantCulture);*/
                    }
                    else
                    {
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                    }
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MeyparQueryCarDiscountforPayment::Exception", LogLevels.logERROR);
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

        public ResultType MeyparNotifyCarPayment(int iWSNumber, GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, string sPlate, int iAmount, string sCurIsoCode, int iTime, DateTime dtEntryDate, DateTime dtEndDate,
                                                 string sGate, string sTariff, decimal dOperationId, int? iWSTimeout,
                                                 ref USER oUser,                                                 
                                                 ref SortedList parametersOut, out string s3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            s3dPartyOpNum = "";
            lEllapsedTime = 0;

            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                MeyparThirdPartyOffstreetWS.InterfazPublicaWebService oOffstreetWS = new MeyparThirdPartyOffstreetWS.InterfazPublicaWebService();                
                oOffstreetWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";
                string strExternalGroupId = "";



                switch (iWSNumber)
                {
                    case 1:
                        oOffstreetWS.Url = oOffstreetParkingConfiguration.GOWC_EXIT_WS1_URL;
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS1_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_USER))
                        {
                            oOffstreetWS.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_PASSWORD);
                        }
                        strExternalGroupId=oOffstreetParkingConfiguration.GROUP.GRP_EXT1_ID;
                        break;

                    case 2:
                        oOffstreetWS.Url = oOffstreetParkingConfiguration.GOWC_EXIT_WS2_URL;
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_USER))
                        {
                            oOffstreetWS.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_PASSWORD);
                        }
                        strExternalGroupId=oOffstreetParkingConfiguration.GROUP.GRP_EXT2_ID;
                        break;

                    case 3:
                        oOffstreetWS.Url = oOffstreetParkingConfiguration.GOWC_EXIT_WS3_URL;
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_USER))
                        {
                            oOffstreetWS.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_PASSWORD);
                        }
                        strExternalGroupId=oOffstreetParkingConfiguration.GROUP.GRP_EXT3_ID;
                        break;


                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("MeyparNotifyCarPayment::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }
                

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                string sUserName = oUser.CUSTOMER.CUS_FIRST_NAME;
                if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_SURNAME1)) sUserName += " " + oUser.CUSTOMER.CUS_SURNAME1;
                if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_SURNAME2)) sUserName += " " + oUser.CUSTOMER.CUS_SURNAME2;

                string sUserAdressStreet = (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_STREET) ? oUser.CUSTOMER.CUS_STREET + " " : "");
                string sUserAddressStreetNum = oUser.CUSTOMER.CUS_STREE_NUMBER.ToString();
                if (oUser.CUSTOMER.CUS_LEVEL_NUM.HasValue) sUserAddressStreetNum += " " + oUser.CUSTOMER.CUS_LEVEL_NUM.Value.ToString();
                if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_DOOR)) sUserAddressStreetNum += " " + oUser.CUSTOMER.CUS_DOOR;
                if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_LETTER)) sUserAddressStreetNum += " " + oUser.CUSTOMER.CUS_LETTER;
                if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_STAIR)) sUserAddressStreetNum += " " + oUser.CUSTOMER.CUS_STAIR;                
                string sUserAdressCity = (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_CITY) ? " " + oUser.CUSTOMER.CUS_CITY : "");
                string sUserAdressState = (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_STATE) ? " " + oUser.CUSTOMER.CUS_STATE : "");
                string sUserAdressZipCode = (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_ZIPCODE) ? " " + oUser.CUSTOMER.CUS_ZIPCODE : "");

                string sUserAddress = string.Format("{0}{1}{2}{3}{4} {5}", oUser.CUSTOMER.CUS_STREET, sUserAddressStreetNum, sUserAdressCity, sUserAdressState, sUserAdressZipCode, oUser.CUSTOMER.COUNTRy.COU_DESCRIPTION);

                

                strAuthHash = CalculateStandardWSHash(strHashKey,
                                                      string.Format("{0}{1}{2}{3}{4}{5}{6}{7:HHmmssddMMyyyy}{8:HHmmssddMMyyyy}{9}{10}{11}{12}{13}{14}{15}{16}",
                                                                    strExternalGroupId, sOpLogicalId, (int)oOpeIdType, sPlate, iAmount, sCurIsoCode, iTime, dtEntryDate, dtEndDate, sGate, sTariff, dOperationId, strvers,
                                                                    oUser.USR_EMAIL, oUser.CUSTOMER.CUS_DOC_ID, sUserName, sUserAddress));

                strMessage = string.Format("<ipark_in>" +
                                           "<parking_id>{0}</parking_id>" +
                                           "<ope_id>{1}</ope_id>" +
                                           "<ope_id_type>{2}</ope_id_type>" +
                                           "<p>{3}</p>" +
                                           "<q>{4}</q>" +
                                           "<cur>{5}</cur>" +
                                           "<t>{6}</t>" +
                                           "<bd>{7:HHmmssddMMyyyy}</bd>" +
                                           "<ed>{8:HHmmssddMMyyyy}</ed>" +
                                           "<gate_id>{9}</gate_id>" +
                                           "<tar_id>{10}</tar_id>" +
                                           "<opnum>{11}</opnum>" +
                                           "<vers>{12}</vers>" +
                                           "<user_email>{13}</user_email>" +
                                           "<user_identity_card>{14}</user_identity_card>" +
                                           "<user_name>{15}</user_name>" +
                                           "<user_address>{16}</user_address>" +
                                           "<ah>{17}</ah>" +
                                           "</ipark_in>",
                                           strExternalGroupId, sOpLogicalId, (int)oOpeIdType, sPlate, iAmount, sCurIsoCode, iTime, dtEntryDate, dtEndDate, sGate, sTariff, dOperationId, strvers, 
                                           oUser.USR_EMAIL, oUser.CUSTOMER.CUS_DOC_ID, sUserName, sUserAddress,
                                           strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("MeyparNotifyCarPayment Timeout={1} xmlIn ={0}", sXmlIn, oOffstreetWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oOffstreetWS.thirdpnotifycarpayment(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("MeyparNotifyCarPayment xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = Convert_ResultTypeMeyparOffstreetWS_TO_ResultType((ResultTypeMeyparOffstreetWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    if (rtRes == ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        parametersOut["opnum"] = wsParameters["opnum"];

                        if (wsParameters.Contains("opnum") && wsParameters["opnum"] != null)
                            s3dPartyOpNum = wsParameters["opnum"].ToString();
                    }
                    else
                    {                        
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                    }
                }


            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "MeyparNotifyCarPayment::Exception", LogLevels.logERROR);
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

        public ResultType MeyparNotifyCarEntryManual(decimal dGroupId, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, string sPlate, DateTime xEntryDate, 
                                                     string sGate, string sTariff, int? iWSTimeout, ref SortedList parametersOut, out DateTime xRealEntryDate, 
                                                     out string sGateOut, out string sTariffOut)
        {
            ResultType rtRes = ResultType.Result_OK;
            xRealEntryDate = xEntryDate;
            sGateOut = "";
            sTariffOut = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {

                // ...

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "MeyparNotifyCarEntryManual::Exception", LogLevels.logERROR);
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

        public ResultType MeyparGetParkingListOcuppation(List<decimal> lstGroupsIds, DateTime xEntryDate, int? iWSTimeout, ref SortedList parametersOut, out List<OffstreetParkingOccupation> lstParkings)
        {
            ResultType rtRes = ResultType.Result_OK;
            lstParkings = new List<OffstreetParkingOccupation>();

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {

                // ...

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "MeyparGetParkingListOcuppation::Exception", LogLevels.logERROR);
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


        public ResultType iParkControlQueryCarExitforPayment(GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, 
                                                             ref string sPlate, DateTime dtCurrentDate, int? iWSTimeout,ref SortedList parametersOut, out int iOp, out int iAmount, 
                                                             out decimal dVAT, out string sCurIsoCode, out int iTime, out DateTime dtEntryDate, out DateTime dtEndDate,
                                                             out string sTariff, out DateTime dtExitLimitDate, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            iOp = 0;
            iAmount = 0;
            dVAT = 0;
            sCurIsoCode = "";
            iTime = 0;
            dtEntryDate = dtCurrentDate;
            dtEndDate = dtCurrentDate;
            sTariff = "";
            dtExitLimitDate = dtCurrentDate;
            lEllapsedTime = 0;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_URL;
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER))
                {
                    request.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_PASSWORD);
                }

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["installationid"] =  oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID;
                oDataObjectDict["operationcode"] = sOpLogicalId;
                oDataObjectDict["plate"] = sPlate;
                oDataObjectDict["date"] = dtCurrentDate.ToString("HHmmssddMMyy");
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY,
                                        string.Format("{0}{1}{2}{3:HHmmssddMMyy}{4}", oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, sOpLogicalId, sPlate, dtCurrentDate, strProviderName)); ;
                oiparkticketInObjectDict["Data"] = JsonConvert.SerializeObject(oDataObjectDict).ToString(); 
                oiparkticketInObjectDict["ah"] = CalculateStandardWSHash(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY, oiparkticketInObjectDict["Data"].ToString());

                ojsonInObjectDict["iparkcontrol_in"] = oiparkticketInObjectDict;
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                string postString = string.Format("jsonIn={0}", json);

                Logger_AddLogMessage(string.Format("iParkControlQueryCarExitforPayment request.url={0}, Timeout={2}  request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(postString);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("iParkControlQueryCarExitforPayment response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeIParkControlOffstreetWS_TO_ResultType((ResultTypeIParkControlOffstreetWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeIParkControlOffstreetWS_TO_ResultType((ResultTypeIParkControlOffstreetWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                                parametersOut["parking_id"] = oData["operationid"];
                                parametersOut["ope_id"] = oData["operationcode"];
                                parametersOut["ope_id_type"] = (int)OffstreetOperationIdType.MeyparId;
                                parametersOut["plate"] = oData["plate"];
                                if (oData["plate"]!=null && oData["plate"].ToString().Length > 0)
                                    sPlate = oData["plate"].ToString();
                                parametersOut["op"] = oData["operationid"];
                                parametersOut["q"] = oData["amounttotal"];
                                parametersOut["cur"] = oData["currency"];
                                parametersOut["bd"] = oData["datestart"];
                                parametersOut["ed"] =oData["dateend"];
                                parametersOut["tar_id"] = oData["tariffdescription"];
                                parametersOut["med"] = oData["maxexitdate"]; 

                                iOp = Convert.ToInt32(oData["operationid"]);
                                iAmount = Convert.ToInt32(oData["amounttotal"]);

                                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                                numberFormatProvider.NumberDecimalSeparator = ".";
                                string sVAT = "";
                                try
                                {
                                    sVAT = oData["vatper"].ToString();
                                    if (sVAT.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                                    decimal dTryVAT = Convert.ToDecimal(sVAT, numberFormatProvider);
                                    dVAT = dTryVAT/100;
                                }
                                catch
                                {
                                    dVAT = 0;
                                }


                                sCurIsoCode = oData["currency"];
                                
                                dtEntryDate = DateTime.ParseExact(oData["datestart"].ToString(), "HHmmssddMMyy",
                                                                  CultureInfo.InvariantCulture);
                                dtEndDate = DateTime.ParseExact(oData["dateend"].ToString(), "HHmmssddMMyy",
                                                                CultureInfo.InvariantCulture);

                                iTime = Convert.ToInt32((dtEndDate - dtEntryDate).TotalMinutes);
                                parametersOut["t"] = iTime.ToString();

                                sTariff = parametersOut["tar_id"].ToString();
                                dtExitLimitDate = DateTime.ParseExact(oData["maxexitdate"].ToString(), "HHmmssddMMyy",
                                                                    CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                            }

                        }


                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "iParkControlQueryCarExitforPayment::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "iParkControlQueryCarExitforPayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

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


        public ResultType iParkControlNotifyCarPayment(int iWSNumber, GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, 
                                                       string sPlate, int iAmount, string sCurIsoCode, int iTime, DateTime dtEntryDate, DateTime dtEndDate,
                                                       string sGate, string sTariff, decimal dOperationId, int? iWSTimeout,
                                                       ref USER oUser,
                                                       ref SortedList parametersOut, out string s3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            lEllapsedTime = 0;
            s3dPartyOpNum = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                string strHashKey = "";
                string strExternalGroupId = "";
                string strURL = "";

                WebRequest request = null;

                switch (iWSNumber)
                {
                    case 1:
                        strURL = oOffstreetParkingConfiguration.GOWC_EXIT_WS1_URL;
                        request = WebRequest.Create(strURL);
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS1_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_USER))
                        {
                            request.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_PASSWORD);
                        }
                        strExternalGroupId = oOffstreetParkingConfiguration.GROUP.GRP_EXT1_ID;
                        break;

                    case 2:
                        strURL = oOffstreetParkingConfiguration.GOWC_EXIT_WS2_URL;
                        request = WebRequest.Create(strURL);
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_USER))
                        {
                            request.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_PASSWORD);
                        }
                        strExternalGroupId = oOffstreetParkingConfiguration.GROUP.GRP_EXT2_ID;
                        break;

                    case 3:
                        strURL = oOffstreetParkingConfiguration.GOWC_EXIT_WS3_URL;
                        request = WebRequest.Create(strURL);
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_USER))
                        {
                            request.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_PASSWORD);
                        }
                        strExternalGroupId = oOffstreetParkingConfiguration.GROUP.GRP_EXT3_ID;
                        break;


                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("iParkControlNotifyCarPayment::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["installationid"] = strExternalGroupId;
                oDataObjectDict["operationcode"] = sOpLogicalId;
                oDataObjectDict["plate"] = sPlate;
                oDataObjectDict["date"] = dtEndDate.ToString("HHmmssddMMyy");
                oDataObjectDict["totalamount"] = iAmount;
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(strHashKey,
                                        string.Format("{0}{1}{2}{3:HHmmssddMMyy}{4}{5}", strExternalGroupId, sOpLogicalId, sPlate, dtEndDate, iAmount, strProviderName)); ;
                oiparkticketInObjectDict["Data"] = JsonConvert.SerializeObject(oDataObjectDict).ToString();
                oiparkticketInObjectDict["ah"] = CalculateStandardWSHash(strHashKey, oiparkticketInObjectDict["Data"].ToString());

                ojsonInObjectDict["iparkcontrol_in"] = oiparkticketInObjectDict;
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                string postString = string.Format("jsonIn={0}", json);

                Logger_AddLogMessage(string.Format("iParkControlNotifyCarPayment request.url={0}, Timeout={2}, request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(postString);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("iParkControlNotifyCarPayment response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeIParkControlOffstreetWS_TO_ResultType((ResultTypeIParkControlOffstreetWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeIParkControlOffstreetWS_TO_ResultType((ResultTypeIParkControlOffstreetWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                                parametersOut["opnum"] = oData["operationid"];

                                s3dPartyOpNum = oData["operationid"].ToString();
                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                            }

                        }


                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "iParkControlNotifyCarPayment::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "iParkControlNotifyCarPayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

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


        public ResultType MeyparAdventaQueryAmount(GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, ref string sPlate, DateTime dtCurrentDate,
                                                   int? iWSTimeout, ref SortedList parametersOut, out int iOp, out int iAmount, out decimal dVAT, out string sCurIsoCode, out int iTime, out DateTime dtEntryDate, 
                                                   out DateTime dtEndDate, out string sTariff, out DateTime dtExitLimitDate, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            iOp = 0;
            iAmount = 0;
            dVAT = 0;
            sCurIsoCode = "";
            iTime = 0;
            dtEntryDate = dtCurrentDate;
            dtEndDate = dtCurrentDate;
            sTariff = "";
            dtExitLimitDate = dtCurrentDate;
            lEllapsedTime = 0;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;
            string strParkingId = "";
            string strZoneId = "";


            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_URL;
                if (!strURL.EndsWith("/")) strURL += "/";
                strURL += "queryAmount";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER))
                {
                    request.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_PASSWORD);
                }
                
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oOffstreetParkingConfiguration.GROUP.INSTALLATION.INS_TIMEZONE_ID);                
                DateTime dtUtcCurrentDate = TimeZoneInfo.ConvertTime(dtCurrentDate, tzi, TimeZoneInfo.Utc);

                request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                string strProviderName = ConfigurationManager.AppSettings["MeyparAdventaCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oParkingInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["providerId"] = Convert.ToInt32(strProviderName);
                oDataObjectDict["SupportPaymentData"] = new Dictionary<string, object>() {                   
                    { "SupportId", sOpLogicalId }, 
                    { "SupportType", 1 }, 
                    { "Version", "2.0" } };
                oDataObjectDict["PaymentDate"] = dtCurrentDate.ToString("yyyyMMddHHmmssfff");
                oDataObjectDict["PaymentUTCDate"] = dtUtcCurrentDate.ToString("yyyyMMddHHmmssfff");
                oDataObjectDict["CurrentDate"] = dtCurrentDate.ToString("yyyyMMddHHmmssfff");
                oDataObjectDict["CurrentUTCDate"] = dtUtcCurrentDate.ToString("yyyyMMddHHmmssfff");

                //oDataObjectDict["ah"] = CalculateStandardWSHash(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY,
                //                        string.Format("{0}{1}{2}{3:HHmmssddMMyy}{4}", oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, sOpLogicalId, sPlate, dtCurrentDate, strProviderName)); ;
                ///oiparkticketInObjectDict["Data"] = JsonConvert.SerializeObject(oDataObjectDict).ToString();
                //oiparkticketInObjectDict["ah"] = CalculateStandardWSHash(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY, oiparkticketInObjectDict["Data"].ToString());


                var values = AuthenticationHash.Instance.GetConcatValues(oDataObjectDict as IDictionary<string, object>);
                var hash = AuthenticationHash.Instance.GetCalculatedHash(values, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY);
                oDataObjectDict["hash"] = hash;

                oParkingInObjectDict["parking_in"] = oDataObjectDict;
                var json = JsonConvert.SerializeObject(oParkingInObjectDict);

                ojsonInObjectDict["jsonIn"] = json;

                string postString = JsonConvert.SerializeObject(ojsonInObjectDict);
                sXmlIn = postString;

                Logger_AddLogMessage(string.Format("MeyparAdventaQueryAmount request.url={0}, Timeout={2}, request.json={1}", strURL, postString, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(postString);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        sXmlOut = responseFromServer;
                        // Display the content.

                        Logger_AddLogMessage(string.Format("MeyparAdventaQueryAmount response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        rtRes = ResultType.Result_Error_Generic;
                        if (oResponse.res != null)
                        {
                            long lResult = Convert.ToInt32(oResponse.res.Value);
                            rtRes = Convert_ResultTypeMeyparAdventaOffstreetWS_TO_ResultType((ResultTypeMeyparAdventaOffstreetWS)lResult);
                        }

                        parametersOut["r"] = Convert.ToInt32(rtRes);

                        if (rtRes == ResultType.Result_OK && oResponse.data != null)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse.data.Value);

                            //parametersOut["parking_id"] = wsParameters["parking_id"];
                            parametersOut["ope_id"] = oData.SupportId.Value; // wsParameters["ope_id"];
                            parametersOut["ope_id_type"] = oData.SupportType.Value; //wsParameters["ope_id_type"];
                            //parametersOut["plate"] = wsParameters["plate"];
                            //if (wsParameters.ContainsKey("plate") && wsParameters["plate"].ToString().Length > 0)
                            //    sPlate = wsParameters["plate"].ToString();
                            //parametersOut["op"] = wsParameters["op"];
                            parametersOut["q"] = oData.TotalAmount.Value;
                            parametersOut["cur"] = oData.Currency.Value;
                            strParkingId = oData.ParkingId.Value;

                           


                            if (oData.Concepts != null && oData.Concepts.Count > 0)
                            {
                                dynamic oConcept = oData.Concepts[0];
                                parametersOut["t"] = oConcept.PaidTime.Value;
                                dtEntryDate = DateTime.ParseExact(oConcept.InitialLocalDate.Value, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                                dtEndDate = DateTime.ParseExact(oConcept.EndLocalDate.Value, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                                dtExitLimitDate = DateTime.ParseExact(oConcept.LimitLocalDate.Value, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                                parametersOut["bd"] = dtEntryDate.ToString("HHmmssddMMyy");
                                parametersOut["ed"] = dtEndDate.ToString("HHmmssddMMyy");
                                parametersOut["tar_id"] = oConcept.TariffId.Value;
                                parametersOut["med"] = dtExitLimitDate.ToString("HHmmssddMMyy");

                                try
                                {
                                    int iZoneId = Convert.ToInt32(oConcept.ZoneId.Value);
                                    strZoneId = iZoneId.ToString();
                                }
                                catch { }



                                dVAT = Convert.ToDecimal(oConcept.Tax.Value) / 10000;
                                /*NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                                numberFormatProvider.NumberDecimalSeparator = ".";
                                string sVAT = "";
                                try
                                {
                                    sVAT =  wsParameters["vat_perc"].ToString();
                                    if (sVAT.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                                    decimal dTryVAT = Convert.ToDecimal(sVAT, numberFormatProvider);
                                    dVAT = dTryVAT;
                                }
                                catch
                                {
                                    dVAT = 0;
                                }*/

                              
                            }
                            
                            //iOp = Convert.ToInt32(wsParameters["op"]);
                            iAmount = Convert.ToInt32(oData.TotalAmount.Value);

                            sCurIsoCode = oData.Currency.Value.ToString();
                            iTime = Convert.ToInt32(parametersOut["t"]);
                            sTariff = parametersOut["tar_id"].ToString();

                        }


                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
     
                    if ((!IsTheSameMeyparAdventaParkingZone(oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, strParkingId, strZoneId)) && (rtRes == ResultType.Result_OK))
                    {
                        Logger_AddLogMessage(string.Format("MeyparAdventaQueryAmount::Input and output group doesn't match {0}!={1}", oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID, strParkingId), LogLevels.logERROR);
                        rtRes = ResultType.Result_Error_OperationNotFound;
                        parametersOut["r"] = Convert.ToInt32(rtRes);

                    }

                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "MeyparAdventaQueryAmount::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MeyparAdventaQueryAmount::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

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

        public ResultType MeyparAdventaQueryCarDiscountforPayment(GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, string sDiscountId, 
                                                                  DateTime dtCurrentDate, int iAmount, int? iWSTimeout,ref SortedList parametersOut, out int iOp, out int iInitialAmount, 
                                                                  out int iFinalAmount, out decimal dVAT, out string sCurIsoCode, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            iOp = 0;            
            dVAT = 0;
            iInitialAmount = iAmount;
            iFinalAmount = 0;
            sCurIsoCode = "";        
            lEllapsedTime = 0;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_URL;
                if (!strURL.EndsWith("/")) strURL += "/";
                strURL += "queryDiscount";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER))
                {
                    request.Credentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_USER, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_HTTP_PASSWORD);
                }

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oOffstreetParkingConfiguration.GROUP.INSTALLATION.INS_TIMEZONE_ID);
                DateTime dtUtcCurrentDate = TimeZoneInfo.ConvertTime(dtCurrentDate, tzi, TimeZoneInfo.Utc);

                request.Method = "POST";                
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                string strProviderName = ConfigurationManager.AppSettings["MeyparAdventaCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oParkingInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["providerId"] = Convert.ToInt32(strProviderName);
                oDataObjectDict["parkingId"] = GetMeyparAdventaParkingId(oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID);
                oDataObjectDict["SupportId"] = sOpLogicalId;
                oDataObjectDict["SupportType"] = 1;
                //oDataObjectDict["transactionId"] = "????";
                //oDataObjectDict["externalOperationNumber"] = "????";
                oDataObjectDict["discountId"] = sDiscountId;

                var values = AuthenticationHash.Instance.GetConcatValues(oDataObjectDict as IDictionary<string, object>);
                var hash = AuthenticationHash.Instance.GetCalculatedHash(values, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY);
                oDataObjectDict["hash"] = hash;

                oParkingInObjectDict["parking_in"] = oDataObjectDict;
                var json = JsonConvert.SerializeObject(oParkingInObjectDict);

                ojsonInObjectDict["jsonIn"] = json;

                string postString = JsonConvert.SerializeObject(ojsonInObjectDict);
                sXmlIn = postString;

                Logger_AddLogMessage(string.Format("MeyparAdventaQueryCarDiscountforPayment request.url={0}, Timeout={2}, request.json={1}", strURL, postString, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(postString);

                request.ContentLength = byteArray.Length;                
                Stream dataStream = request.GetRequestStream();                
                dataStream.Write(byteArray, 0, byteArray.Length);                
                dataStream.Close();                

                try
                {
                    WebResponse response = request.GetResponse();                    
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        sXmlOut = responseFromServer;
                        // Display the content.

                        Logger_AddLogMessage(string.Format("MeyparAdventaQueryCarDiscountforPayment response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);                        

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        rtRes = ResultType.Result_Error_Generic;
                        if (oResponse.res != null)
                        {
                            long lResult = Convert.ToInt32(oResponse.res.Value);
                            rtRes = Convert_ResultTypeMeyparAdventaOffstreetWS_TO_ResultType((ResultTypeMeyparAdventaOffstreetWS)lResult);
                        }

                        parametersOut["r"] = Convert.ToInt32(rtRes);

                        if (rtRes == ResultType.Result_OK && oResponse.data != null)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse.data.Value);
                                                    
                            parametersOut["parking_id"] = GetMeyparAdventaParkingId(oOffstreetParkingConfiguration.GROUP.GRP_QUERY_EXT_ID);
                            parametersOut["ope_id"] = sOpLogicalId; // wsParameters["ope_id"];
                            parametersOut["ope_id_type"] = 1; // wsParameters["ope_id_type"];                            
                            //parametersOut["op"] = wsParameters["op"]; ????
                            parametersOut["qi"] = iInitialAmount;
                            parametersOut["qf"] = oData.TotalAmount.Value;
                            //parametersOut["cur"] = wsParameters["cur"]; ????

                            iOp = Convert.ToInt32(oData.TransactionId);
                            // iInitialAmount = Convert.ToInt32(wsParameters["qi"]); ???
                            iFinalAmount = Convert.ToInt32(oData.TotalAmount.Value);
                            sCurIsoCode = oData.Currency;

                            // Tax ????
                            /*NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            string sVAT = "";
                            try
                            {
                                sVAT = wsParameters["vat_perc"].ToString();
                                if (sVAT.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                                decimal dTryVAT = Convert.ToDecimal(sVAT, numberFormatProvider);
                                dVAT = dTryVAT;
                            }
                            catch
                            {
                                dVAT = 0;
                            }*/

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "MeyparAdventaQueryCarDiscountforPayment::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MeyparAdventaQueryCarDiscountforPayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

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

        public ResultType MeyparAdventaNotifyCarPayment(int iWSNumber, GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetParkingConfiguration, string sOpLogicalId, OffstreetOperationIdType oOpeIdType, string sPlate, 
                                                        int iAmount, string sCurIsoCode, int iTime, DateTime dtEntryDate, DateTime dtEndDate, string sGate, string sTariff, decimal dOperationId, int? iWSTimeout,
                                                        ref USER oUser, ref SortedList parametersOut, out string s3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            s3dPartyOpNum = "";
            lEllapsedTime = 0;

            Stopwatch watch = null;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {                
                string sUrl = "";
                int iTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                string strHashKey = "";
                string strExternalGroupId = "";
                ICredentials oCredentials = null;


                switch (iWSNumber)
                {
                    case 1:
                        sUrl = oOffstreetParkingConfiguration.GOWC_EXIT_WS1_URL;
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS1_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_USER))
                        {
                            oCredentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS1_HTTP_PASSWORD);
                        }
                        strExternalGroupId = GetMeyparAdventaParkingId(oOffstreetParkingConfiguration.GROUP.GRP_EXT1_ID);
                        break;

                    case 2:
                        sUrl = oOffstreetParkingConfiguration.GOWC_EXIT_WS2_URL;
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS2_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_USER))
                        {
                            oCredentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS2_HTTP_PASSWORD);
                        }
                        strExternalGroupId = GetMeyparAdventaParkingId(oOffstreetParkingConfiguration.GROUP.GRP_EXT2_ID);
                        break;

                    case 3:
                        sUrl = oOffstreetParkingConfiguration.GOWC_EXIT_WS3_URL;
                        strHashKey = oOffstreetParkingConfiguration.GOWC_EXIT_WS3_AUTH_HASH_KEY;
                        if (!string.IsNullOrEmpty(oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_USER))
                        {
                            oCredentials = new System.Net.NetworkCredential(oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_USER, oOffstreetParkingConfiguration.GOWC_EXIT_WS3_HTTP_PASSWORD);
                        }
                        strExternalGroupId = GetMeyparAdventaParkingId(oOffstreetParkingConfiguration.GROUP.GRP_EXT3_ID);
                        break;


                    default:
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("MeyparAdventaNotifyCarPayment::Error: Bad WS Number", LogLevels.logERROR);
                            return rtRes;
                        }

                }

                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                                ((sender, certificate, chain, sslPolicyErrors) => true);

                if (!sUrl.EndsWith("/")) sUrl += "/";
                sUrl += "payment";
                WebRequest request = WebRequest.Create(sUrl);
                if (oCredentials != null)
                {
                    request.Credentials = oCredentials;
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iTimeout;

                string strProviderName = ConfigurationManager.AppSettings["MeyparAdventaCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oParkingInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["providerId"] = Convert.ToInt32(strProviderName);
                oDataObjectDict["SupportPaymentData"] = new Dictionary<string, object>() {
                    { "ParkingId", strExternalGroupId },
                    { "SupportId", sOpLogicalId }, 
                    { "SupportType", 1 }, 
                    { "Version", "2.0" } };
                oDataObjectDict["ExternalOperationNumber"] = dOperationId.ToString();

                var values = AuthenticationHash.Instance.GetConcatValues(oDataObjectDict as IDictionary<string, object>);
                var hash = AuthenticationHash.Instance.GetCalculatedHash(values, oOffstreetParkingConfiguration.GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY);
                oDataObjectDict["hash"] = hash;

                oParkingInObjectDict["parking_in"] = oDataObjectDict;
                var json = JsonConvert.SerializeObject(oParkingInObjectDict);

                ojsonInObjectDict["jsonIn"] = json;

                string postString = JsonConvert.SerializeObject(ojsonInObjectDict);
                sXmlIn = postString;

                Logger_AddLogMessage(string.Format("MeyparAdventaNotifyCarPayment request.url={0}, Timeout={2},  request.json={1}", sUrl, postString, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(postString);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        sXmlOut = responseFromServer;
                        // Display the content.

                        Logger_AddLogMessage(string.Format("MeyparAdventaNotifyCarPayment response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        rtRes = ResultType.Result_Error_Generic;
                        if (oResponse.res != null)
                        {
                            long lResult = Convert.ToInt32(oResponse.res.Value);
                            rtRes = Convert_ResultTypeMeyparAdventaOffstreetWS_TO_ResultType((ResultTypeMeyparAdventaOffstreetWS)lResult);
                        }

                        parametersOut["r"] = Convert.ToInt32(rtRes);

                        if (rtRes == ResultType.Result_OK && oResponse.data != null)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse.data.Value);

                                                  
                            parametersOut["opnum"] = oData.OperationNumber.Value;
                            s3dPartyOpNum = parametersOut["opnum"].ToString();
                            //if (wsParameters.Contains("opnum") && wsParameters["opnum"] != null)
                            //    s3dPartyOpNum = wsParameters["opnum"].ToString();

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "MeyparAdventaNotifyCarPayment::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MeyparAdventaNotifyCarPayment::Exception", LogLevels.logERROR);
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



        protected bool IsTheSameMeyparAdventaParkingZone(string strConfigurationZone, string strParkingId, string strZone)
        {
            bool bRes = false;

            string[] strConfigurationZoneComponents = strConfigurationZone.Split('/');

            if (strConfigurationZoneComponents.Count()==2)
            {
                bRes = ((strConfigurationZoneComponents[0] == strParkingId) && (strConfigurationZoneComponents[1] == strZone));          
            }
            if (strConfigurationZoneComponents.Count() == 1)
            {
                bRes = (strConfigurationZoneComponents[0] == strParkingId);
            }

            Logger_AddLogMessage(string.Format("IsTheSameMeyparAdventaParkingZone strConfigurationZone={0}, strParkingId={1}, strZone={2}, bRes={3}", strConfigurationZone, strParkingId, strZone, bRes), LogLevels.logINFO);

            return bRes;
        }


        protected string GetMeyparAdventaParkingId(string strConfigurationZone)
        {
            string strRes = strConfigurationZone;
            
            string[] strConfigurationZoneComponents = strConfigurationZone.Split('/');

            if (strConfigurationZoneComponents.Count() == 2)
            {
                strRes = strConfigurationZoneComponents[0];
            }

            Logger_AddLogMessage(string.Format("GetMeyparAdventaParkingId strConfigurationZone={0}, strRes={1}", strConfigurationZone, strRes), LogLevels.logINFO);

            return strRes;
        }


    }

    public class AuthenticationHash
    {
        #region Private members
        private const string TypeDictionary = "JsonDict";
        private const string TypeDictionary2 = "Dictionary`2";
        private const string TypeArray = "JsonArray";
        private const string TypeString = "String";
        private static AuthenticationHash _instance;

        private string HMacKey { get; set; }
        private byte[] _normKey;
        private byte[] NormKey
        {
            get
            {
                const int iKeyLength = 64;
                const long bigPrimeNumber = 2147483647;
                var iSum = 0;
                var keyBytes = Encoding.UTF8.GetBytes(HMacKey);
                _normKey = new byte[iKeyLength];
                for (var i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * bigPrimeNumber) %
                   (Byte.MaxValue + 1));
                }
                return _normKey;
            }
        }
        private HMACSHA256 HMacSha256 { get { return new HMACSHA256(NormKey); } }
        #endregion

        #region Private constructor
        private AuthenticationHash() { }
        #endregion
        #region Singleton
        public static AuthenticationHash Instance
        {
            get
            {
                return _instance ??
                    (_instance = new AuthenticationHash());
            }
        }
        #endregion
        #region Public methods
        public string GetCalculatedHash(string strInput, string hashKey)
        {
            HMacKey = hashKey;
            var hashResult = string.Empty;
            if (HMacSha256 == null)
                return hashResult;
            var inputBytes = Encoding.UTF8.GetBytes(strInput);
            var hash = HMacSha256.ComputeHash(inputBytes);
            if (hash.Length < 8)
                return hashResult;
            var sb = new StringBuilder();
            for (var i = hash.Length - 8; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));
            hashResult = sb.ToString();
            return hashResult;
        }
        public string GetConcatValues(object item)
        {
            var dictionary = item as IDictionary<string, object>;
            if (dictionary == null)
                return string.Empty;
            var values = new StringBuilder();
            foreach (var property in dictionary.Where(p => p.Key != "hash" && p.Value != null))
            {
                switch (property.Value.GetType().Name)
                {
                    case TypeDictionary:
                    case TypeDictionary2:
                        values.Append(GetConcatValues(property.Value));
                        break;
                    case TypeArray:
                        var collection = property.Value as ICollection;

                        if (collection != null)
                        {
                            foreach (var itemCollection in collection)
                                values.Append(GetConcatValues(itemCollection));
                        }
                        break;
                    case TypeString:
                        values.Append(property.Value);
                        break;
                    default:
                        values.Append(property.Value.ToString().Replace(",", "."));
                        break;
                }
            }
            return values.ToString();
        }
        public string GetConcatValuesExternalService(object item)
        {
            var values = new StringBuilder();
            var properties = item.GetType().GetProperties();
            foreach (var value in properties.Select(property =>
           property.GetValue(item)).Where(value => value != null))
                values.Append(value);
            return values.ToString();
        }
        #endregion
    }
    public static class DictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }
        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();
            var dictionary = new Dictionary<string, T>();
            foreach (System.ComponentModel.PropertyDescriptor property in
           System.ComponentModel.TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }
        private static void
       AddPropertyToDictionary<T>(System.ComponentModel.PropertyDescriptor property,
       object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))

                dictionary.Add(property.Name, (T)value);
        }
        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }
        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new System.ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }

}
