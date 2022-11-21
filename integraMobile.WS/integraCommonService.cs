using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Diagnostics;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.ExternalWS;
using integraMobile.WS.Resources;

namespace integraMobile.WS
{
    public class integraCommonService
    {

        private const long BIG_PRIME_NUMBER = 2147483647;
        private const long BIG_PRIME_NUMBER2 = 624159837;
        private const long BIG_PRIME_NUMBER_PAYMENT_GATEWAY = 472189635;
        private static ulong _VERSION_3_6 = AppUtilities.AppVersion("3.6");
        private static ulong _VERSION_3_7_1 = AppUtilities.AppVersion("3.7.1");


        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(integraCommonService));

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;
        private IGeograficAndTariffsRepository geograficAndTariffsRepository;

        public integraCommonService(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, IGeograficAndTariffsRepository oGeograficAndTariffsRepository)
        {
            this.customersRepository = oCustomersRepository;
            this.infraestructureRepository = oInfraestructureRepository;
            this.geograficAndTariffsRepository = oGeograficAndTariffsRepository;
        }

        #region Public Methods

        public ResultType ChargeOffstreetOperation(OffstreetOperationType operationType, string strPlate, double dChangeToApply, int iQuantity, int iTime,
                                                    DateTime dtEntryDate, DateTime dtNotifyEntryDate, DateTime? dtPaymentDate, DateTime? dtEndDate, DateTime? dtExitLimitDate, GROUPS_OFFSTREET_WS_CONFIGURATION oParkingconfiguration, GROUP oGroup, string sLogicalId, string sTariff, string sGate, string sSpaceDesc, bool bMustNotify,
                                                    ref USER oUser, int iOSType, decimal? dMobileSessionId, decimal? dLatitude, decimal? dLongitude, string strAppVersion,
                                                    decimal dPercVAT1, decimal dPercVAT2, decimal dPercFEE, int iPercFEETopped, int iFixedFEE, 
                                                    int iPartialVAT1, int iPartialPercFEE, int iPartialFixedFEE, int iTotalQuantity,
                                                    string sDiscountCodes, string strMD, string strCAVV, string strECI,
                                                    string strBSRedsys3DSTransID, string strBSRedsys3DSPares, string strBSRedsys3DSCres, string strBSRedsys3DSMethodData, 
                                                    string strMercadoPagoToken,
                                                    string strMPProTransactionId,
                                                    string strMPProReference,
                                                    string strMPProCardHash,
                                                    string strMPProCardReference,
                                                    string strMPProCardScheme,
                                                    string strMPProGatewayDate,
                                                    string strMPProMaskedCardNumber,
                                                    string strMPProExpMonth,
                                                    string strMPProExpYear,
                                                    string strMPProCardType,
                                                    string strMPProDocumentID,
                                                    string strMPProDocumentType,
                                                    string strMPProInstallaments,
                                                    string strMPProCVVLength,
                                                    decimal dSourceApp,bool bPaymentInPerson, decimal? dEntryOperationId,
                                                    ref SortedList parametersOut, out int iCurrencyChargedQuantity, out decimal dOperationID,
                                                    out decimal? dRechargeId, out int? iBalanceAfterRecharge, out bool bRestoreBalanceInCaseOfRefund, out string str3DSURL, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_OK;
            iCurrencyChargedQuantity = 0;
            double dChangeFee = 0;
            decimal dBalanceCurID = oUser.CURRENCy.CUR_ID;
            dOperationID = -1;
            dRechargeId = null;
            bRestoreBalanceInCaseOfRefund = true;
            PaymentSuscryptionType suscriptionType = PaymentSuscryptionType.pstPrepay;
            iBalanceAfterRecharge = null;
            str3DSURL = "";
            lEllapsedTime = 0;

            try
            {
                decimal dCurrencyId = oUser.USR_CUR_ID;
                PaymentSuscryptionType eSuscryptionType = (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE;
                int iUserBalance = oUser.USR_BALANCE;
                if (oGroup.INSTALLATION.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG != null)
                {
                    dCurrencyId = oGroup.INSTALLATION.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_CUR_ID;
                    eSuscryptionType = PaymentSuscryptionType.pstPerTransaction;
                    iUserBalance = 0;
                }
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, oGroup.INSTALLATION);

                string strMPProDescription = geograficAndTariffsRepository.GetSourceAppDescription(dSourceApp) + " - " + oGroup.INSTALLATION.INS_DESCRIPTION + " - " + oGroup.GRP_DESCRIPTION + " - " + strPlate;


                if (iTotalQuantity != 0)
                {
                    parametersOut["autorecharged"] = "0";
                    
                    if (oUser.USR_CUR_ID == dCurrencyId)                        
                        iCurrencyChargedQuantity = ChangeQuantityFromInstallationCurToUserCur(iTotalQuantity, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);
                    else if (dCurrencyId == oGroup.INSTALLATION.INS_CUR_ID)
                        iCurrencyChargedQuantity = iTotalQuantity;
                    else
                    {
                        // *** NO DEBERÍA PASAR NUNCA ***
                        iCurrencyChargedQuantity = iTotalQuantity;
                    }

                    if (iCurrencyChargedQuantity < 0)
                    {
                        rtRes = (ResultType)iCurrencyChargedQuantity;
                        Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error Changing quantity {0} ", rtRes.ToString()), LogLevels.logERROR);
                        return rtRes;
                    }



                    if ((iUserBalance > 0) ||
                        (eSuscryptionType == PaymentSuscryptionType.pstPrepay))
                    {
                        int iNewBalance = iUserBalance - iCurrencyChargedQuantity;


                        if (iNewBalance < 0)
                        {

                            if ((oUserPaymentMean != null) &&
                                (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                                (oUserPaymentMean.CUSPM_VALID == 1))
                            {


                                if ((eSuscryptionType == PaymentSuscryptionType.pstPrepay) &&
                                    (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1) &&
                                    (oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE > 0))
                                {

                                    int iQuantityToRecharge = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value;
                                    if (Math.Abs(iNewBalance) > oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value)
                                    {
                                        iQuantityToRecharge = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value + Math.Abs(iNewBalance);
                                    }

                                    rtRes = PerformPrepayRecharge(ref oUser, oUserPaymentMean, iOSType, true, iQuantityToRecharge, iQuantityToRecharge, false, dLatitude, dLongitude, strAppVersion, 
                                                                      PaymentMeanRechargeCreationType.pmrctAutomaticRecharge, strMD, strCAVV, strECI, 
                                                                      strBSRedsys3DSTransID, strBSRedsys3DSPares,  strBSRedsys3DSCres,  strBSRedsys3DSMethodData, strMercadoPagoToken,
                                                                      strMPProDescription,
                                                                      strMPProTransactionId,
                                                                      strMPProReference,
                                                                      strMPProCardHash,
                                                                      strMPProCardReference,
                                                                      strMPProCardScheme,
                                                                      strMPProGatewayDate,
                                                                      strMPProMaskedCardNumber,
                                                                      strMPProExpMonth,
                                                                      strMPProExpYear,
                                                                      strMPProCardType,
                                                                      strMPProDocumentID,
                                                                      strMPProDocumentType,
                                                                      strMPProInstallaments,
                                                                      strMPProCVVLength,
                                                                      dSourceApp, bPaymentInPerson, !bPaymentInPerson, out dRechargeId, out str3DSURL, out lEllapsedTime);
                                    if (rtRes != ResultType.Result_OK)
                                    {
                                        if (rtRes == ResultType.Result_3DS_Validation_Needed)
                                        {
                                            Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::3DS Validation needed using {0} ", str3DSURL), LogLevels.logINFO);
                                            //return rtRes;

                                        }
                                        else
                                        {
                                            Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error AutoRecharging {0} ", rtRes.ToString()), LogLevels.logERROR);
                                            //return rtRes;
                                        }
                                    }

                                    iBalanceAfterRecharge = oUser.USR_BALANCE;
                                    parametersOut["autorecharged"] = "1";
                                }
                                else if ((eSuscryptionType == PaymentSuscryptionType.pstPerTransaction))
                                {
                                    rtRes = PerformPrepayRecharge(ref oUser, oUserPaymentMean, iOSType, false, -iNewBalance, -iNewBalance,  false, dLatitude, dLongitude, strAppVersion,
                                                                    PaymentMeanRechargeCreationType.pmrctRegularRecharge, strMD, strCAVV, strECI, 
                                                                    strBSRedsys3DSTransID, strBSRedsys3DSPares,  strBSRedsys3DSCres,  strBSRedsys3DSMethodData, strMercadoPagoToken,
                                                                    strMPProDescription,
                                                                    strMPProTransactionId,
                                                                    strMPProReference,
                                                                    strMPProCardHash,
                                                                    strMPProCardReference,
                                                                    strMPProCardScheme,
                                                                    strMPProGatewayDate,
                                                                    strMPProMaskedCardNumber,
                                                                    strMPProExpMonth,
                                                                    strMPProExpYear,
                                                                    strMPProCardType,
                                                                    strMPProDocumentID,
                                                                    strMPProDocumentType,
                                                                    strMPProInstallaments,
                                                                    strMPProCVVLength,
                                                                    dSourceApp, bPaymentInPerson, !bPaymentInPerson, out dRechargeId, out str3DSURL, out lEllapsedTime);
                                    if (rtRes != ResultType.Result_OK)
                                    {
                                        if (rtRes == ResultType.Result_3DS_Validation_Needed)
                                        {
                                            Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::3DS Validation needed using {0} ", str3DSURL), LogLevels.logINFO);
                                            if (bPaymentInPerson) return rtRes;

                                        }
                                        else
                                        {
                                            Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error Charging Rest Of transaction {0} ", rtRes.ToString()), LogLevels.logERROR);
                                            if (bPaymentInPerson) return rtRes;
                                        }
                                    }
                                    iBalanceAfterRecharge = oUser.USR_BALANCE;
                                    parametersOut["autorecharged"] = "1";
                                }
                                else
                                {
                                    rtRes = ResultType.Result_Error_Not_Enough_Balance;
                                    Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error AutoRecharging {0} ", rtRes.ToString()), LogLevels.logERROR);
                                    if (bPaymentInPerson) return rtRes;
                                }
                            }
                            else
                            {
                                rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                                Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::{0} ", rtRes.ToString()), LogLevels.logERROR);
                                if (bPaymentInPerson) return rtRes;
                            }


                        }
                    }
                    else if ((iUserBalance == 0) &&
                             (eSuscryptionType == PaymentSuscryptionType.pstPerTransaction))
                    {
                        //Balance is 0 and suscription type is pertransaction

                        if ((oUserPaymentMean != null) &&
                            (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                            (oUserPaymentMean.CUSPM_VALID == 1))
                        {
                            rtRes = PerformPerTransactionRecharge(ref oUser, oUserPaymentMean, iOSType, iTotalQuantity, dLatitude, dLongitude, strAppVersion, strMD, strCAVV, strECI, 
                                                                strBSRedsys3DSTransID, strBSRedsys3DSPares,  strBSRedsys3DSCres,  strBSRedsys3DSMethodData, strMercadoPagoToken,
                                                                strMPProDescription,
                                                                strMPProTransactionId,
                                                                strMPProReference,
                                                                strMPProCardHash,
                                                                strMPProCardReference,
                                                                strMPProCardScheme,
                                                                strMPProGatewayDate,
                                                                strMPProMaskedCardNumber,
                                                                strMPProExpMonth,
                                                                strMPProExpYear,
                                                                strMPProCardType,
                                                                strMPProDocumentID,
                                                                strMPProDocumentType,
                                                                strMPProInstallaments,
                                                                strMPProCVVLength,
                                                                dSourceApp, bPaymentInPerson, !bPaymentInPerson, out dRechargeId, out str3DSURL, out lEllapsedTime);
                            if (rtRes != ResultType.Result_OK)
                            {
                                if (rtRes == ResultType.Result_3DS_Validation_Needed)
                                {
                                    Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::3DS Validation needed using {0} ", str3DSURL), LogLevels.logINFO);
                                    if (bPaymentInPerson) return rtRes;

                                }
                                else
                                {
                                    Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error charging per transaction value {0} ", rtRes.ToString()), LogLevels.logERROR);
                                    if (bPaymentInPerson) return rtRes;
                                }
                            }

                            //bRestoreBalanceInCaseOfRefund = false;
                            dBalanceCurID = oGroup.INSTALLATION.CURRENCy.CUR_ID;
                            dChangeToApply = 1.0;
                            dChangeFee = 0;
                            iCurrencyChargedQuantity = iTotalQuantity;
                            suscriptionType = PaymentSuscryptionType.pstPerTransaction;
                        }
                        else
                        {
                            rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                            Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::{0} ", rtRes.ToString()), LogLevels.logERROR);
                            if (bPaymentInPerson) return rtRes;
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                        Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::{0} ", rtRes.ToString()), LogLevels.logERROR);
                        if (bPaymentInPerson) return rtRes;
                    }
                }

                //bool bSubstractFromBalance = bRestoreBalanceInCaseOfRefund;

                DateTime? dtUTCEntryDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtEntryDate);
                DateTime? dtUTCNotifyEntryDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtNotifyEntryDate);
                DateTime? dtUTCPaymentDate = null;
                if (dtPaymentDate.HasValue) dtUTCPaymentDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtPaymentDate.Value);
                DateTime? dtUTCEndDate = null;
                if (dtEndDate.HasValue) dtUTCEndDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtEndDate.Value);
                DateTime? dtUTCExitLimitDate = null;
                if (dtExitLimitDate.HasValue) dtUTCExitLimitDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtExitLimitDate.Value);

                bool bConfirmedWs1 = true;
                bool bConfirmedWs2 = true;
                bool bConfirmedWs3 = true;
                if ((oParkingconfiguration.GOWC_OPT_OPERATIONCONFIRM_MODE ?? 0) == 1)
                {
                    bConfirmedWs1 = false;
                    bConfirmedWs2 = false;
                    bConfirmedWs3 = false;
                }

                if (oUser.USR_CUR_ID != dCurrencyId)
                    iCurrencyChargedQuantity = ChangeQuantityFromInstallationCurToUserCur(iTotalQuantity, dChangeToApply, oGroup.INSTALLATION, oUser, out dChangeFee);

                int iCurrencyDebtQuantity = 0;

                if (!customersRepository.ChargeOffstreetOperation(ref oUser,
                                                                  iOSType,
                                                                  true,
                                                                  suscriptionType,
                                                                  operationType,
                                                                  strPlate,
                                                                  oGroup.GRP_INS_ID,
                                                                  oGroup.GRP_ID,
                                                                  sLogicalId,
                                                                  sTariff, sGate, sSpaceDesc,
                                                                  dtEntryDate, dtNotifyEntryDate, dtPaymentDate, dtEndDate, dtExitLimitDate,
                                                                  dtUTCEntryDate.Value, dtUTCNotifyEntryDate.Value, dtUTCPaymentDate, dtUTCEndDate, dtUTCExitLimitDate,
                                                                  iTime,
                                                                  iQuantity,
                                                                  oGroup.INSTALLATION.INS_CUR_ID,
                                                                  dBalanceCurID,
                                                                  dChangeToApply,
                                                                  dChangeFee,
                                                                  iCurrencyChargedQuantity,
                                                                  dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                                                  dRechargeId,
                                                                  bMustNotify,
                                                                  bConfirmedWs1, bConfirmedWs2, bConfirmedWs3,
                                                                  dMobileSessionId,
                                                                  dLatitude, dLongitude, strAppVersion,dSourceApp, 
                                                                  sDiscountCodes,
                                                                  dEntryOperationId,
                                                                  out dOperationID,
                                                                  out iCurrencyDebtQuantity))
                {

                    Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error Inserting Parking Payment for plate {0} ", strPlate), LogLevels.logERROR);
                    return ResultType.Result_Error_Generic;
                }

                if (iCurrencyDebtQuantity > 0 /*rtRes != ResultType.Result_OK*/)
                {
                    // Insert amount into users debt table
                    if (!customersRepository.AddUserDebtOffstreetOperation(ref oUser, (dtPaymentDate ?? dtEndDate ?? dtEntryDate), (dtUTCPaymentDate ?? dtUTCEndDate ?? dtUTCEntryDate.Value), iCurrencyDebtQuantity, dOperationID))
                    {
                        Logger_AddLogMessage(string.Format("ChargeOffstreetOperation::Error Inserting User debt operation for plate {0} ", strPlate), LogLevels.logERROR);
                        return ResultType.Result_Error_Generic;
                    }

                }

                parametersOut["newbal"] = oUser.USR_BALANCE;


            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChargeOffstreetOperation::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public double GetChangeToApplyFromInstallationCurToUserCur(INSTALLATION oInstallation, USER oUser)
        {
            double dResult = 1.0;


            try
            {

                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    dResult = CCurrencyConvertor.GetChangeToApply(oInstallation.CURRENCy.CUR_ISO_CODE,
                                              infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)));
                    if (dResult < 0)
                    {
                        Logger_AddLogMessage(string.Format("GetChangeToApplyFromInstallationCurToUserCur::Error getting change from {0} to {1} ", oInstallation.CURRENCy.CUR_ISO_CODE, infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))), LogLevels.logERROR);
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

        public ResultType ConfirmCarPayment(GROUPS_OFFSTREET_WS_CONFIGURATION oParkingConfiguration, GROUP oGroup, USER oUser,
                                            string sOpeId, OffstreetOperationIdType oOpeIdType, string sPlate, string sTariff, string sGate,
                                            OffstreetOperationType operationType, int iAmount, int iTime, double dChangeToApply, string sCurIsoCode,
                                            DateTime dtEntryDate, DateTime dtPaymentDate, DateTime dtEndDate, DateTime dtExitLimitDate,
                                            int iOSType, decimal? dMobileSessionId, decimal? dLatitude, decimal? dLongitude, string sAppVersion,
                                            decimal dPercVAT1, decimal dPercVAT2, decimal dPercFEE, decimal dPercFEETopped, decimal dFixedFEE,
                                            int iPartialVAT1, int iPartialPercFEE, int iPartialFixedFEE, int iTotalQuantity,
                                            string sDiscountCodes, string strMD, string strCAVV, string strECI,
                                            string strBSRedsys3DSTransID, string strBSRedsys3DSPares, string strBSRedsys3DSCres, string strBSRedsys3DSMethodData, 
                                            string strMercadoPagoToken,
                                            string strMPProTransactionId,
                                            string strMPProReference,
                                            string strMPProCardHash,
                                            string strMPProCardReference,
                                            string strMPProCardScheme,
                                            string strMPProGatewayDate,
                                            string strMPProMaskedCardNumber,
                                            string strMPProExpMonth,
                                            string strMPProExpYear,
                                            string strMPProCardType,
                                            string strMPProDocumentID,
                                            string strMPProDocumentType,
                                            string strMPProInstallaments,
                                            string strMPProCVVLength,
                                            decimal dSourceApp, bool bPaymentInPerson,
                                            OPERATIONS_OFFSTREET oLastParkOp, ThirdPartyOffstreet oThirdPartyOffstreet,
                                            ref SortedList parametersOut, out string str3DSURL)
        {
            ResultType rt = ResultType.Result_OK;
            str3DSURL = "";


            // Get last offstreet operation with the same group id and logical id (<g> and <ope_id>)
            //OPERATIONS_OFFSTREET oLastParkOp = null;
            if (oLastParkOp == null)
            {
                if (!customersRepository.GetLastOperationOffstreetData(oGroup.GRP_ID, sOpeId, out oLastParkOp))
                {
                    rt = ResultType.Result_Error_Generic;
                    return rt;
                }
            }

            if (oLastParkOp != null && (oLastParkOp.OPEOFF_TYPE == (int)OffstreetOperationType.Exit || oLastParkOp.OPEOFF_TYPE == (int)OffstreetOperationType.OverduePayment) &&
                                       oLastParkOp.OPEOFF_EXIT_LIMIT_DATE.HasValue && oLastParkOp.OPEOFF_EXIT_LIMIT_DATE >= dtPaymentDate)
            {
                rt = ResultType.Result_Error_OperationAlreadyClosed;
                return rt;
            }

            int iEntryCurrencyChargedQuantity = 0;
            decimal dEntryOperationID = -1;
            int iCurrencyChargedQuantity = 0;            
            decimal dOperationID = -1;
            string str3dPartyOpNum = "";
            decimal? dRechargeId;
            bool bRestoreBalanceInCaseOfRefund = true;
            int? iBalanceAfterRecharge = null;
            
            DateTime dtNotifyEntryDate;

            long lEllapsedTime = 0;

            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(oGroup.INSTALLATION.INS_ID);

            if (oLastParkOp == null)
            {
                dtNotifyEntryDate = dtPaymentDate;
                // Add entry offstreet operation
                rt = ChargeOffstreetOperation(OffstreetOperationType.Entry, sPlate, dChangeToApply, 0, 0,
                                              dtEntryDate, dtNotifyEntryDate, null, null, null,
                                              oParkingConfiguration, oGroup, sOpeId, sTariff, sGate, "", false,
                                              ref oUser, iOSType, dMobileSessionId, dLatitude, dLongitude, sAppVersion,
                                              0, 0, 0, 0, 0, 
                                              0, 0, 0, 0,
                                              null,strMD, strCAVV, strECI,
                                              strBSRedsys3DSTransID, strBSRedsys3DSPares, strBSRedsys3DSCres, strBSRedsys3DSMethodData,
                                              strMercadoPagoToken,
                                              strMPProTransactionId,
                                              strMPProReference,
                                              strMPProCardHash,
                                              strMPProCardReference,
                                              strMPProCardScheme,
                                              strMPProGatewayDate,
                                              strMPProMaskedCardNumber,
                                              strMPProExpMonth,
                                              strMPProExpYear,
                                              strMPProCardType,
                                              strMPProDocumentID,
                                              strMPProDocumentType,
                                              strMPProInstallaments,
                                              strMPProCVVLength,
                                              dSourceApp, bPaymentInPerson, null,
                                              ref parametersOut, out iEntryCurrencyChargedQuantity, out dEntryOperationID,
                                              out dRechargeId, out iBalanceAfterRecharge, out bRestoreBalanceInCaseOfRefund, out str3DSURL, out lEllapsedTime);

                if (rt != ResultType.Result_OK)
                {
                    return rt;
                }

                iWSTimeout -= (int)lEllapsedTime;

            }
            else
            {
                dtNotifyEntryDate = oLastParkOp.OPEOFF_NOTIFY_ENTRY_DATE;
                dEntryOperationID = oLastParkOp.OPEOFF_ID;
            }
            int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
            int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

            rt = ChargeOffstreetOperation(operationType, sPlate, dChangeToApply, iAmount, iTime,
                                          dtEntryDate, dtNotifyEntryDate, dtPaymentDate, dtEndDate, dtExitLimitDate,
                                          oParkingConfiguration, oGroup, sOpeId, sTariff, sGate, "", false,
                                          ref oUser, iOSType, dMobileSessionId, dLatitude, dLongitude, sAppVersion,
                                          dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, 
                                          iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iTotalQuantity,
                                          sDiscountCodes, strMD, strCAVV, strECI, 
                                          strBSRedsys3DSTransID, strBSRedsys3DSPares, strBSRedsys3DSCres, strBSRedsys3DSMethodData,
                                          strMercadoPagoToken,
                                          strMPProTransactionId,
                                          strMPProReference,
                                          strMPProCardHash,
                                          strMPProCardReference,
                                          strMPProCardScheme,
                                          strMPProGatewayDate,
                                          strMPProMaskedCardNumber,
                                          strMPProExpMonth,
                                          strMPProExpYear,
                                          strMPProCardType,
                                          strMPProDocumentID,
                                          strMPProDocumentType,
                                          strMPProInstallaments,
                                          strMPProCVVLength,
                                          dSourceApp, bPaymentInPerson, dEntryOperationID,
                                          ref parametersOut, out iCurrencyChargedQuantity, out dOperationID,
                                          out dRechargeId, out iBalanceAfterRecharge, out bRestoreBalanceInCaseOfRefund, out str3DSURL, out lEllapsedTime);

            if (rt != ResultType.Result_OK)
            {
                return rt;
            }

            iWSTimeout -= (int)lEllapsedTime;

                        
            if ((oParkingConfiguration.GOWC_OPT_OPERATIONCONFIRM_MODE ?? 0) == 0)
            {
                if (oThirdPartyOffstreet == null)
                    oThirdPartyOffstreet = new ThirdPartyOffstreet();

                switch ((ConfirmExitOffstreetWSSignatureType)oParkingConfiguration.GOWC_EXIT_WS1_SIGNATURE_TYPE)
                {
                    case ConfirmExitOffstreetWSSignatureType.test:
                        {
                            str3dPartyOpNum = "EXT" + dOperationID.ToString();
                            rt = ResultType.Result_OK;
                        }
                        break;

                    case ConfirmExitOffstreetWSSignatureType.meypar:
                        {
                            rt = oThirdPartyOffstreet.MeyparNotifyCarPayment(1, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, 
                                                                            dtEndDate, sGate, sTariff, dOperationID, iWSTimeout, ref oUser,
                                                                            ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;
                    case ConfirmExitOffstreetWSSignatureType.meyparAdventa:
                        {
                            rt = oThirdPartyOffstreet.MeyparAdventaNotifyCarPayment(1, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, dtEndDate, sGate, 
                                                                                    sTariff, dOperationID, iWSTimeout, ref oUser,
                                                                                    ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;
                    case ConfirmExitOffstreetWSSignatureType.iparkcontrol:
                        {
                            rt = oThirdPartyOffstreet.iParkControlNotifyCarPayment(1, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, dtEndDate, sGate, 
                                                                                   sTariff, dOperationID, iWSTimeout, ref oUser,
                                                                                   ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;


                    case ConfirmExitOffstreetWSSignatureType.no_call:
                        rt = ResultType.Result_OK;
                        break;

                    default:
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                        break;
                }
            }


            if (rt != ResultType.Result_OK)
            {

                if (parametersOut.IndexOfKey("autorecharged") >= 0)
                    parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                if (parametersOut.IndexOfKey("newbal") >= 0)
                    parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));

                if (dEntryOperationID != -1)
                {
                    if (!customersRepository.RefundChargeOffstreetPayment(ref oUser, false, dEntryOperationID))
                    {
                        Logger_AddLogMessage(string.Format("RefundChargeOffstreetPayment::Error Refunding Entry Offstreet {0} ", dEntryOperationID), LogLevels.logERROR);                        
                    }
                }

                ResultType rtRefund = RefundChargeOffstreetPayment(ref oUser, dOperationID, dRechargeId, bRestoreBalanceInCaseOfRefund);
                if (rtRefund == ResultType.Result_OK)
                {
                    Logger_AddLogMessage(string.Format("ConfirmCarPayment::Payment Refund of {0}", iCurrencyChargedQuantity), LogLevels.logERROR);
                }
                else
                {
                    Logger_AddLogMessage(string.Format("ConfirmCarPayment::Error in Payment Refund: {0}", rtRefund.ToString()), LogLevels.logERROR);
                }

                return rt;
            }
            else
            {
                parametersOut["utc_offset"] = geograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oGroup.INSTALLATION.INS_ID);

                if (str3dPartyOpNum.Length > 0)
                {
                    customersRepository.UpdateThirdPartyIDInOffstreetOperation(ref oUser, 1, dOperationID, str3dPartyOpNum);
                }


                if (dRechargeId != null)
                {
                    customersRepository.ConfirmRecharge(ref oUser, dRechargeId.Value);

                    try
                    {
                        CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                        if (customersRepository.GetRechargeData(ref oUser, dRechargeId.Value, out oRecharge))
                        {
                            if ((PaymentSuscryptionType)oRecharge.CUSPMR_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPrepay)
                            {
                                string culture = oUser.USR_CULTURE_LANG;
                                CultureInfo ci = new CultureInfo(culture);
                                Thread.CurrentThread.CurrentUICulture = ci;
                                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                integraMobile.WS.Properties.Resource.Culture = ci;

                                iAmount = oRecharge.CUSPMR_AMOUNT;
                                dPercVAT1 = oRecharge.CUSPMR_PERC_VAT1 ?? 0;
                                dPercVAT2 = oRecharge.CUSPMR_PERC_VAT2 ?? 0;
                                dPercFEE = oRecharge.CUSPMR_PERC_FEE ?? 0;
                                iPercFEETopped = (int)(oRecharge.CUSPMR_PERC_FEE_TOPPED ?? 0);
                                iFixedFEE = (int)(oRecharge.CUSPMR_FIXED_FEE ?? 0);

                                int iPartialPercFEEVAT;
                                int iPartialFixedFEEVAT;

                                iTotalQuantity = customersRepository.CalculateFEE(iAmount, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                                int iQFEE = Convert.ToInt32(Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero));
                                if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                                iQFEE += iFixedFEE;
                                int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                                int iQSubTotal = iAmount + iQFEE;

                                int iLayout = 0;
                                if (iQFEE != 0 || iQVAT != 0)
                                {
                                    OPERATOR oOperator = customersRepository.GetDefaultOperator();
                                    if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                                }


                                string sLayoutSubtotal = "";
                                string sLayoutTotal = "";

                                sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID));
                                string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);


                                if (iLayout == 2)
                                {
                                    sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutSubtotal"),
                                                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                                    (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                    (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                                }
                                else if (iLayout == 1)
                                {
                                    sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotal"),
                                                                 string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iAmount) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                                 string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                                 (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                 (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                 string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                                }

                                string strRechargeEmailSubject = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmAutomaticRecharge_EmailHeader");
                                /*
                                    ID: {0}<br>
                                 *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                                 *  Cantidad Recargada: {2} 
                                 */
                                string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmRecharge_EmailBody"),
                                    oRecharge.CUSPMR_ID,
                                    oRecharge.CUSPMR_DATE,
                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                  infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}", Convert.ToDouble(iBalanceAfterRecharge) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                        infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                    ConfigurationManager.AppSettings["EmailSignatureURL"],
                                    ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                    sLayoutSubtotal, sLayoutTotal,
                                    GetEmailFooter(ref oUser));

                                SendEmail(ref oUser, strRechargeEmailSubject, strRechargeEmailBody, dSourceApp);

                            }
                        }
                    }
                    catch { }

                }

                if (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                {
                    int iDiscountValue = 0;
                    string strDiscountCurrencyISOCode = "";

                    try
                    {
                        iDiscountValue = Convert.ToInt32(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]);
                        strDiscountCurrencyISOCode = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];
                    }
                    catch
                    { }


                    if (iDiscountValue > 0)
                    {
                        double dDiscountChangeApplied = 0;
                        double dDiscountChangeFee = 0;
                        int iCurrencyDiscountQuantity = ChangeQuantityFromCurToUserCur(iDiscountValue, strDiscountCurrencyISOCode, oUser,                                                                                       
                                                                                       out dDiscountChangeApplied, out dDiscountChangeFee);

                        if (iCurrencyDiscountQuantity > 0)
                        {
                            DateTime? dtUTCTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtPaymentDate.AddSeconds(1));

                            /*customersRepository.AddDiscountToParkingOperation(ref oUser, iOSType, PaymentSuscryptionType.pstPrepay,
                                                                                dtPaymentDate.AddSeconds(1), dtUTCTime.Value, iDiscountValue,
                                                                                infraestructureRepository.GetCurrencyFromIsoCode(strDiscountCurrencyISOCode),
                                                                                oUser.CURRENCy.CUR_ID, dDiscountChangeApplied, dDiscountChangeFee, iCurrencyDiscountQuantity, dOperationID,
                                                                                dLatitude, dLongitude, sAppVersion);*/

                            parametersOut["newbal"] = oUser.USR_BALANCE;

                        }
                    }

                }


            }


            if (rt == ResultType.Result_OK)
            {
                //customersRepository.DeleteSessionOperationOffstreetInfo(ref oUser, parametersIn["SessionID"].ToString());

                try
                {
                    OPERATIONS_OFFSTREET oParkOp = null;
                    if (customersRepository.GetOperationOffstreetData(ref oUser, dOperationID, out oParkOp))
                    {
                        string culture = oUser.USR_CULTURE_LANG;
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                        integraMobile.WS.Properties.Resource.Culture = ci;


                        iAmount = oParkOp.OPEOFF_AMOUNT;                         
                        dPercVAT1 = oParkOp.OPEOFF_PERC_VAT1 ?? 0;
                        dPercVAT2 = oParkOp.OPEOFF_PERC_VAT2 ?? 0;
                        dPercFEE = oParkOp.OPEOFF_PERC_FEE ?? 0;                        
                        iPercFEETopped = (int)(oParkOp.OPEOFF_PERC_FEE_TOPPED ?? 0);
                        iFixedFEE = (int)(oParkOp.OPEOFF_FIXED_FEE ?? 0);

                        int iPartialPercFEEVAT;
                        int iPartialFixedFEEVAT;

                        if (oParkOp.OPEOFF_PARTIAL_VAT1.HasValue)
                        {
                            iPartialVAT1 = Convert.ToInt32(oParkOp.OPEOFF_PARTIAL_VAT1.Value);
                            iTotalQuantity = customersRepository.CalculateFEE(iAmount, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE,
                                                                              iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                              out iPartialPercFEEVAT, out iPartialFixedFEEVAT);
                        }
                        else
                            iTotalQuantity = customersRepository.CalculateFEE(iAmount, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE,
                                                                              out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                              out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                        int iQFEE = Convert.ToInt32(Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero));
                        if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                        iQFEE += iFixedFEE;                        
                        int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                        int iQSubTotal = iAmount + iQFEE;

                        int iLayout = 0;
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            iLayout = oParkingConfiguration.GOWC_FEE_LAYOUT;
                        }

                        string sLayoutSubtotal = "";
                        string sLayoutTotal = "";

                        sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oParkOp.OPEOFF_AMOUNT_CUR_ID));
                        string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);


                        if (iLayout == 2)
                        {
                            sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutSubtotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (oParkOp.OPEOFF_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPEOFF_PERC_VAT1 * 100) : "") +
                                                            (oParkOp.OPEOFF_PERC_VAT2 != 0 && oParkOp.OPEOFF_PERC_VAT1 != oParkOp.OPEOFF_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPEOFF_PERC_VAT2 * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }
                        else if (iLayout == 1)
                        {
                            sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotal"),
                                                         string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iAmount) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                         string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                         (oParkOp.OPEOFF_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPEOFF_PERC_VAT1 * 100) : "") +
                                                         (oParkOp.OPEOFF_PERC_VAT2 != 0 && oParkOp.OPEOFF_PERC_VAT1 != oParkOp.OPEOFF_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPEOFF_PERC_VAT2 * 100) : ""),
                                                         string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }

                        string strParkingEmailSubject = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmOffstreet_EmailHeader");
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
                        string strParkingEmailBody = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmOffstreet_EmailBody"),
                            oParkOp.OPEOFF_ID,
                            oParkOp.USER_PLATE.USRP_PLATE,
                            oParkOp.INSTALLATION.INS_DESCRIPTION,
                            oParkOp.GROUP.GRP_DESCRIPTION,
                            oParkOp.OPEOFF_TARIFF,
                            oParkOp.OPEOFF_PAYMENT_DATE,
                            oParkOp.OPEOFF_ENTRY_DATE,
                            oParkOp.OPEOFF_END_DATE,
                            (oParkOp.OPEOFF_AMOUNT_CUR_ID == oParkOp.OPEOFF_BALANCE_CUR_ID ?
                                string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oParkOp.CURRENCy.CUR_ISO_CODE) + "} {1}", Convert.ToDouble(oParkOp.OPEOFF_TOTAL_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oParkOp.CURRENCy.CUR_ISO_CODE), infraestructureRepository.GetCurSymbolFromIsoCode(oParkOp.CURRENCy.CUR_ISO_CODE)) :
                                string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oParkOp.CURRENCy.CUR_ISO_CODE) + "} {1} / {2:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oParkOp.CURRENCy1.CUR_ISO_CODE) + "} {3}", Convert.ToDouble(oParkOp.OPEOFF_TOTAL_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oParkOp.CURRENCy.CUR_ISO_CODE), infraestructureRepository.GetCurSymbolFromIsoCode(oParkOp.CURRENCy.CUR_ISO_CODE),
                                                                            Convert.ToDouble(oParkOp.OPEOFF_FINAL_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oParkOp.CURRENCy1.CUR_ISO_CODE), infraestructureRepository.GetCurSymbolFromIsoCode(oParkOp.CURRENCy1.CUR_ISO_CODE))),
                                                                            (oParkOp.OPEOFF_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0)?
                                    string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmOffstreet_EmailBody_Balance"), string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}",
                                                                                                             Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))), 
                                                                                                             infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal,
                            sLayoutTotal,
                            GetEmailFooter(ref oInstallation));

                        SendEmail(ref oUser, strParkingEmailSubject, strParkingEmailBody, dSourceApp);
                    }
                }
                catch { }


            }



            if ((oParkingConfiguration.GOWC_OPT_OPERATIONCONFIRM_MODE ?? 0) == 0)
            {
                iWSTimeout -= (int)lEllapsedTime;
                if (Convert.ToInt32(parametersOut["r"]) == Convert.ToInt32(ResultType.Result_OK))
                {
                    bool bConfirmed1 = true;
                    bool bConfirmed2 = true;
                    bool bConfirmed3 = true;

                    if (oParkingConfiguration.GOWC_EXIT_WS2_SIGNATURE_TYPE.HasValue)
                    {
                        SortedList parametersOutTemp = new SortedList();

                        switch ((ConfirmExitOffstreetWSSignatureType)oParkingConfiguration.GOWC_EXIT_WS2_SIGNATURE_TYPE)
                        {
                            case ConfirmExitOffstreetWSSignatureType.meypar:
                                {
                                    rt = oThirdPartyOffstreet.MeyparNotifyCarPayment(2, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, dtEndDate, sGate, 
                                                                                     sTariff, dOperationID, iWSTimeout, ref oUser,
                                                                                     ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;
                            case ConfirmExitOffstreetWSSignatureType.meyparAdventa:
                                {
                                    rt = oThirdPartyOffstreet.MeyparAdventaNotifyCarPayment(2, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, dtEndDate, sGate, 
                                                                                            sTariff, dOperationID, iWSTimeout, ref oUser,
                                                                                            ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmExitOffstreetWSSignatureType.no_call:
                                rt = ResultType.Result_OK;
                                break;

                            default:
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                                break;
                        }

                        if (rt != ResultType.Result_OK)
                        {
                            bConfirmed2 = false;
                            Logger_AddLogMessage(string.Format("ConfirmCarPayment::Error in WS 2 Confirmation"), LogLevels.logWARN);
                        }
                        else
                        {
                            if (str3dPartyOpNum.Length > 0)
                            {
                                customersRepository.UpdateThirdPartyIDInOffstreetOperation(ref oUser, 2, dOperationID, str3dPartyOpNum);
                            }

                        }
                    }


                    if (oParkingConfiguration.GOWC_EXIT_WS3_SIGNATURE_TYPE.HasValue)
                    {
                        iWSTimeout -= (int)lEllapsedTime;

                        SortedList parametersOutTemp = new SortedList();

                        switch ((ConfirmExitOffstreetWSSignatureType)oParkingConfiguration.GOWC_EXIT_WS3_SIGNATURE_TYPE)
                        {
                            case ConfirmExitOffstreetWSSignatureType.meypar:
                                {
                                    rt = oThirdPartyOffstreet.MeyparNotifyCarPayment(3, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, dtEndDate, 
                                                                                     sGate, sTariff, dOperationID, iWSTimeout, ref oUser,
                                                                                     ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;
                            case ConfirmExitOffstreetWSSignatureType.meyparAdventa:
                                {
                                    rt = oThirdPartyOffstreet.MeyparAdventaNotifyCarPayment(3, oParkingConfiguration, sOpeId, oOpeIdType, sPlate, iAmount + iPartialVAT1, sCurIsoCode, iTime, dtEntryDate, 
                                                                                            dtEndDate, sGate, sTariff, dOperationID, iWSTimeout, 
                                                                                            ref oUser, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmExitOffstreetWSSignatureType.no_call:
                                rt = ResultType.Result_OK;
                                break;

                            default:
                                break;
                        }

                        if (rt != ResultType.Result_OK)
                        {
                            bConfirmed3 = false;
                            Logger_AddLogMessage(string.Format("ConfirmCarPayment::Error in WS 3 Confirmation"), LogLevels.logWARN);
                        }
                        else
                        {
                            if (str3dPartyOpNum.Length > 0)
                            {
                                customersRepository.UpdateThirdPartyIDInOffstreetOperation(ref oUser, 3, dOperationID, str3dPartyOpNum);
                            }
                        }
                    }

                    if ((!bConfirmed2) || (!bConfirmed3))
                    {
                        customersRepository.UpdateThirdPartyConfirmedInOffstreetOperation(ref oUser, dOperationID, bConfirmed1, bConfirmed2, bConfirmed3);
                    }
                }
            }

            return rt;
        }

        public ResultType ChargeTollMovement(string strPlate, double dChangeToApply, int iQuantity, 
                                              DateTime dtPaymentDate, string sTollTariff, INSTALLATION oInstallation, TOLL oToll,
                                              ref USER oUser, int iOSType, 
                                              decimal dPercVAT1, decimal dPercVAT2, decimal dPercFEE, int iPercFEETopped, int iFixedFEE, 
                                              int iPartialVAT1, int iPartialPercFEE, int iPartialFixedFEE, int iTotalQuantity,
                                              string sExternalId, bool bOnline, ChargeOperationsType eType, string sQr, decimal? dLockMovementId,decimal dSourceApp,
                                              ref SortedList parametersOut, out int iCurrencyChargedQuantity, out decimal dMovementID,
                                              out DateTime? dtUTCInsertionDate, out decimal? dRechargeId, out int? iBalanceAfterRecharge, out bool bRestoreBalanceInCaseOfRefund, out DateTime? dtUTCPaymentDate)
        {
            ResultType rtRes = ResultType.Result_OK;
            iCurrencyChargedQuantity = 0;
            double dChangeFee = 0;
            decimal dBalanceCurID = oUser.CURRENCy.CUR_ID;
            dMovementID = -1;
            dRechargeId = null;
            bRestoreBalanceInCaseOfRefund = true;
            PaymentSuscryptionType suscriptionType = PaymentSuscryptionType.pstPrepay;
            iBalanceAfterRecharge = null;
            dtUTCInsertionDate = null;
            dtUTCPaymentDate = null;
            long lEllapsedTime = 0;

            try
            {
                decimal dCurrencyId = oUser.USR_CUR_ID;
                PaymentSuscryptionType eSuscryptionType = (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE;
                int iUserBalance = oUser.USR_BALANCE;
                if (oInstallation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG != null)
                {
                    dCurrencyId = oInstallation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_CUR_ID;
                    eSuscryptionType = PaymentSuscryptionType.pstPerTransaction;
                    iUserBalance = 0;
                }
                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, oInstallation);

                parametersOut["autorecharged"] = "0";
                
                if (oUser.USR_CUR_ID == dCurrencyId)                    
                    iCurrencyChargedQuantity = ChangeQuantityFromInstallationCurToUserCur(iTotalQuantity /*iQuantity*/, dChangeToApply, oInstallation, oUser, out dChangeFee);
                else if (dCurrencyId == oInstallation.INS_CUR_ID)
                    iCurrencyChargedQuantity = iTotalQuantity;
                else
                {
                    // *** NO DEBERÍA PASAR NUNCA ***
                    iCurrencyChargedQuantity = iTotalQuantity;
                }

                if (iCurrencyChargedQuantity < 0)
                {
                    rtRes = (ResultType)iCurrencyChargedQuantity;
                    Logger_AddLogMessage(string.Format("ChargeTollMovement::Error Changing quantity {0} ", rtRes.ToString()), LogLevels.logERROR);
                    return rtRes;
                }

                if (eType != ChargeOperationsType.TollUnlock)
                {

                    string str3DSURL = "";
                    int iMinimumBalanceAllowed = oInstallation.INS_MAX_UNPAID_BALANCE;

                    if ((iUserBalance > 0) ||
                        (eSuscryptionType == PaymentSuscryptionType.pstPrepay))
                    {
                        int iNewBalance = iUserBalance - iCurrencyChargedQuantity;

                        if (iNewBalance < 0)
                        {

                            if ((oUserPaymentMean != null) &&
                                (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                                (oUserPaymentMean.CUSPM_VALID == 1))
                            {


                                if ((eSuscryptionType == PaymentSuscryptionType.pstPrepay) &&
                                    (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1) &&
                                    (oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE > 0))
                                {

                                    int iQuantityToRecharge = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value;
                                    if (Math.Abs(iNewBalance) > oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value)
                                    {
                                        iQuantityToRecharge = oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value + Math.Abs(iNewBalance);
                                    }


                                    rtRes = PerformPrepayRecharge(ref oUser, oUserPaymentMean, iOSType, true, iQuantityToRecharge, iQuantityToRecharge, false, null, null, null,
                                                                  PaymentMeanRechargeCreationType.pmrctAutomaticRecharge,"","","", "","","","", "",
                                                                  "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", dSourceApp, false, false, out dRechargeId, out str3DSURL, out lEllapsedTime);
                                    if (rtRes != ResultType.Result_OK)
                                    {
                                        Logger_AddLogMessage(string.Format("ChargeTollMovement::Error AutoRecharging {0} ", rtRes.ToString()), LogLevels.logERROR);
                                        if (bOnline)
                                            return rtRes;
                                    }
                                    else
                                    {
                                        iBalanceAfterRecharge = oUser.USR_BALANCE;
                                        parametersOut["autorecharged"] = "1";
                                    }
                                }
                                else if ((eSuscryptionType == PaymentSuscryptionType.pstPerTransaction))
                                {
                                    rtRes = PerformPrepayRecharge(ref oUser, oUserPaymentMean, iOSType, false, -iNewBalance, -iNewBalance, false, null, null, null,
                                                PaymentMeanRechargeCreationType.pmrctRegularRecharge, "", "", "", "", "", "", "", "",
                                                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", dSourceApp,false, false,  out dRechargeId, out str3DSURL, out lEllapsedTime);
                                    if (rtRes != ResultType.Result_OK)
                                    {
                                        Logger_AddLogMessage(string.Format("ChargeTollMovement::Error Charging Rest Of transaction {0} ", rtRes.ToString()), LogLevels.logERROR);
                                        if (bOnline) return rtRes;
                                    }
                                    else
                                    {
                                        iBalanceAfterRecharge = oUser.USR_BALANCE;
                                        parametersOut["autorecharged"] = "1";
                                    }
                                }
                                else if (!bOnline || (iNewBalance >= iMinimumBalanceAllowed && iUserBalance >= 0))
                                {

                                }
                                else
                                {
                                    rtRes = ResultType.Result_Error_Not_Enough_Balance;
                                    Logger_AddLogMessage(string.Format("ChargeTollMovement::Error AutoRecharging {0} ", rtRes.ToString()), LogLevels.logERROR);
                                    if (bOnline) return rtRes;
                                }
                            }
                            else
                            {
                                rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                                Logger_AddLogMessage(string.Format("ChargeTollMovement::{0} ", rtRes.ToString()), LogLevels.logERROR);
                                if (bOnline) return rtRes;
                            }

                        }
                    }
                    else if ((iUserBalance <= 0) &&
                             (eSuscryptionType == PaymentSuscryptionType.pstPerTransaction))
                    {
                        //Balance is 0 and suscription type is pertransaction

                        if ((oUserPaymentMean != null) &&
                            (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                            (oUserPaymentMean.CUSPM_VALID == 1))
                        {                            
                            rtRes = PerformPerTransactionRecharge(ref oUser, oUserPaymentMean, iOSType, iTotalQuantity/*iQuantity*/, null, null, null,"","","","","","","","",
                                "", "", "", "", "", "", "", "", "", "", "", "", "", "","", dSourceApp, false, false, out dRechargeId,out str3DSURL, out lEllapsedTime);
                            if (rtRes != ResultType.Result_OK)
                            {
                                Logger_AddLogMessage(string.Format("ChargeTollMovement::Error charging per transaction value {0} ", rtRes.ToString()), LogLevels.logERROR);
                                if (bOnline) return rtRes;
                            }
                            else
                            {
                                //bRestoreBalanceInCaseOfRefund = false;
                                dBalanceCurID = oInstallation.CURRENCy.CUR_ID;
                                dChangeToApply = 1.0;
                                dChangeFee = 0;
                                iCurrencyChargedQuantity = iTotalQuantity/*iQuantity*/;
                                suscriptionType = PaymentSuscryptionType.pstPerTransaction;
                            }
                        }
                        else
                        {
                            rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                            Logger_AddLogMessage(string.Format("ChargeTollMovement::{0} ", rtRes.ToString()), LogLevels.logERROR);
                            if (bOnline) return rtRes;
                        }

                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                        Logger_AddLogMessage(string.Format("ChargeTollMovement::{0} ", rtRes.ToString()), LogLevels.logERROR);
                        if (bOnline) return rtRes;
                    }

                }

                //bool bSubstractFromBalance = bRestoreBalanceInCaseOfRefund;

                dtUTCPaymentDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtPaymentDate);

                decimal? dTollId = null;
                if (oToll != null) dTollId = oToll.TOL_ID;

                if (oUser.USR_CUR_ID != dCurrencyId)
                    iCurrencyChargedQuantity = ChangeQuantityFromInstallationCurToUserCur(iTotalQuantity, dChangeToApply, oInstallation, oUser, out dChangeFee);

                if (!customersRepository.ChargeTollMovement(ref oUser,
                                                          iOSType,
                                                          true,
                                                          suscriptionType,                                                          
                                                          strPlate,
                                                          oInstallation.INS_ID,
                                                          dTollId,
                                                          sTollTariff,
                                                          dtPaymentDate,
                                                          dtUTCPaymentDate.Value,
                                                          iQuantity,
                                                          oInstallation.INS_CUR_ID,
                                                          dBalanceCurID,
                                                          dChangeToApply,
                                                          dChangeFee,
                                                          iCurrencyChargedQuantity,
                                                          dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,                                                          
                                                          dRechargeId,                                                                                                                    
                                                          //strAppVersion,
                                                          sExternalId,
                                                          eType,
                                                          sQr,
                                                          dLockMovementId,
                                                          dSourceApp, 
                                                          out dMovementID,
                                                          out dtUTCInsertionDate))
                {

                    Logger_AddLogMessage(string.Format("ChargeTollMovement::Error Inserting Toll Payment for plate {0} ", strPlate), LogLevels.logERROR);
                    return ResultType.Result_Error_Generic;
                }

                parametersOut["newbal"] = oUser.USR_BALANCE;

                if (!bOnline && rtRes != ResultType.Result_OK)
                {                    
                    Logger_AddLogMessage(string.Format("ChargeTollMovement::Online=false, rtRes={0}. Force result ok: rtRes={1}.", rtRes.ToString(), ResultType.Result_OK.ToString()), LogLevels.logWARN);
                    rtRes = ResultType.Result_OK;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChargeTollMovement::Exception", LogLevels.logERROR);
            }


            return rtRes;
        }

        /*public ResultType ModifyTollMovement(decimal dMovementId, double dChangeToApply, int iQuantity,
                                              string sTollTariff, TOLL oToll,
                                              ref USER oUser, int iOSType,
                                              decimal dPercVAT1, decimal dPercVAT2, decimal dPercFEE, int iPercFEETopped, int iFixedFEE,
                                              int iPartialVAT1, int iPartialPercFEE, int iPartialFixedFEE, int iTotalQuantity,
                                              int iPreviousTotalQuantity,
                                              string sExternalId, bool bOnline, TollMovementType eType, string sQr,
                                              out decimal? dRechargeId, out int? iBalanceAfterRecharge, out bool bRestoreBalanceInCaseOfRefund)
        {
            ResultType rtRes = ResultType.Result_OK;            
            double dChangeFee = 0;
            decimal dBalanceCurID = oUser.CURRENCy.CUR_ID;            
            dRechargeId = null;
            bRestoreBalanceInCaseOfRefund = true;            
            iBalanceAfterRecharge = null;

            try
            {                
                int iCurrencyChargedQuantity = ChangeQuantityFromInstallationCurToUserCur(iTotalQuantity, dChangeToApply, oToll.INSTALLATION, oUser, out dChangeFee);

                if (iCurrencyChargedQuantity < 0)
                {
                    rtRes = (ResultType)iCurrencyChargedQuantity;
                    Logger_AddLogMessage(string.Format("ModifyTollMovement::Error Changing quantity {0} ", rtRes.ToString()), LogLevels.logERROR);
                    return rtRes;
                }

                int iMinimumBalanceAllowed = oToll.INSTALLATION.INS_MAX_UNPAID_BALANCE;

                int iCurrentChargedQuantityDiff = iCurrencyChargedQuantity - iPreviousTotalQuantity;

                if ((oUser.USR_BALANCE > 0) ||
                    (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay))
                {
                    int iNewBalance = oUser.USR_BALANCE - iCurrentChargedQuantityDiff;

                    if (iNewBalance < 0)
                    {

                        if ((oUser.CUSTOMER_PAYMENT_MEAN != null) &&
                            (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_ENABLED == 1) &&
                            (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_VALID == 1))
                        {


                            if ((oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay) &&
                                (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AUTOMATIC_RECHARGE == 1) &&
                                (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AMOUNT_TO_RECHARGE > 0))
                            {

                                int iQuantityToRecharge = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AMOUNT_TO_RECHARGE.Value;
                                if (Math.Abs(iNewBalance) > oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AMOUNT_TO_RECHARGE.Value)
                                {
                                    iQuantityToRecharge = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AMOUNT_TO_RECHARGE.Value + Math.Abs(iNewBalance);
                                }

                                rtRes = PerformPrepayRecharge(ref oUser, iOSType, true, iQuantityToRecharge, false, null, null, null,
                                                                PaymentMeanRechargeCreationType.pmrctAutomaticRecharge, out dRechargeId);
                                if (rtRes != ResultType.Result_OK)
                                {
                                    Logger_AddLogMessage(string.Format("ModifyTollMovement::Error AutoRecharging {0} ", rtRes.ToString()), LogLevels.logERROR);
                                    if (bOnline)
                                        return rtRes;
                                }
                                else
                                {
                                    iBalanceAfterRecharge = oUser.USR_BALANCE;                                    
                                }
                            }
                            else if ((oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction))
                            {
                                rtRes = PerformPrepayRecharge(ref oUser, iOSType, false, -iNewBalance, false, null, null, null,
                                            PaymentMeanRechargeCreationType.pmrctRegularRecharge, out dRechargeId);
                                if (rtRes != ResultType.Result_OK)
                                {
                                    Logger_AddLogMessage(string.Format("ModifyTollMovement::Error Charging Rest Of transaction {0} ", rtRes.ToString()), LogLevels.logERROR);
                                    if (bOnline) return rtRes;
                                }
                                else
                                {
                                    iBalanceAfterRecharge = oUser.USR_BALANCE;                                    
                                }
                            }
                            else if (!bOnline || (iNewBalance >= iMinimumBalanceAllowed && oUser.USR_BALANCE >= 0))
                            {

                            }
                            else
                            {
                                rtRes = ResultType.Result_Error_Not_Enough_Balance;
                                Logger_AddLogMessage(string.Format("ModifyTollMovement::Error AutoRecharging {0} ", rtRes.ToString()), LogLevels.logERROR);
                                if (bOnline) return rtRes;
                            }
                        }
                        else
                        {
                            rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                            Logger_AddLogMessage(string.Format("ModifyTollMovement::{0} ", rtRes.ToString()), LogLevels.logERROR);
                            if (bOnline) return rtRes;
                        }

                    }
                }
                else if ((oUser.USR_BALANCE <= 0) &&
                         (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction))
                {
                    //Balance is 0 and suscription type is pertransaction

                    if ((oUser.CUSTOMER_PAYMENT_MEAN != null) &&
                        (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_ENABLED == 1) &&
                        (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_VALID == 1))
                    {
                        rtRes = PerformPerTransactionRecharge(ref oUser, iOSType, iTotalQuantity, oToll.INSTALLATION.CURRENCy.CUR_ID, null, null, null, out dRechargeId);
                        if (rtRes != ResultType.Result_OK)
                        {
                            Logger_AddLogMessage(string.Format("ModifyTollMovement::Error charging per transaction value {0} ", rtRes.ToString()), LogLevels.logERROR);
                            if (bOnline) return rtRes;
                        }
                        else
                        {
                            bRestoreBalanceInCaseOfRefund = false;
                            dBalanceCurID = oToll.INSTALLATION.CURRENCy.CUR_ID;
                            dChangeToApply = 1.0;
                            dChangeFee = 0;
                            iCurrencyChargedQuantity = iTotalQuantity;
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                        Logger_AddLogMessage(string.Format("ModifyTollMovement::{0} ", rtRes.ToString()), LogLevels.logERROR);
                        if (bOnline) return rtRes;
                    }

                }
                else
                {
                    rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                    Logger_AddLogMessage(string.Format("ModifyTollMovement::{0} ", rtRes.ToString()), LogLevels.logERROR);
                    if (bOnline) return rtRes;
                }

                bool bSubstractFromBalance = bRestoreBalanceInCaseOfRefund;
                

                if (!customersRepository.ModifyTollMovement(dMovementId,
                                                            ref oUser,                                                            
                                                            bSubstractFromBalance,
                                                            oToll.TOL_ID,
                                                            sTollTariff,
                                                            iQuantity,
                                                            iCurrencyChargedQuantity,
                                                            dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                                            dRechargeId,
                                                            sExternalId,
                                                            eType,
                                                            sQr))
                {

                    Logger_AddLogMessage(string.Format("ModifyTollMovement::Error modifying Toll Payment {0} ", dMovementId), LogLevels.logERROR);
                    return ResultType.Result_Error_Generic;
                }

                //parametersOut["newbal"] = oUser.USR_BALANCE;

                if (!bOnline && rtRes != ResultType.Result_OK)
                {
                    Logger_AddLogMessage(string.Format("ModifyTollMovement::Onlin=false, rtRes={0}. Force result ok: rtRes={1}.", rtRes.ToString(), ResultType.Result_OK.ToString()), LogLevels.logWARN);
                    rtRes = ResultType.Result_OK;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ModifyTollMovement::Exception", LogLevels.logERROR);
            }


            return rtRes;
        }*/

        #endregion

        #region Private Methods

        public int ChangeQuantityFromInstallationCurToUserCur(int iQuantity, double dChangeToApply, INSTALLATION oInstallation, USER oUser, out double dChangeFee)
        {
            int iResult = iQuantity;
            dChangeFee = 0;

            try
            {

                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    int iFactor = infraestructureRepository.GetCurrenciesFactorDifference((int)oInstallation.INS_CUR_ID, (int)oUser.USR_CUR_ID);

                    double dConvertedValue = Convert.ToDouble(iQuantity) * dChangeToApply;
                    dConvertedValue = dConvertedValue * Math.Pow(10, (double)iFactor);
                    dConvertedValue = Math.Round(dConvertedValue, 4);

                    dChangeFee = Convert.ToDouble(infraestructureRepository.GetChangeFeePerc()) * dConvertedValue / 100;
                    iResult = Convert.ToInt32(dConvertedValue - dChangeFee + 0.5);
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }

        private int ChangeQuantityFromCurToUserCur(int iQuantity, string strISOCode, USER oUser, out double dChangeApplied, out double dChangeFee)
        {
            int iResult = iQuantity;
            dChangeApplied = 1;
            dChangeFee = 0;


            try
            {

                if (strISOCode != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
                {
                    int iFactor = infraestructureRepository.GetCurrenciesFactorDifference(strISOCode, infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)));

                    double dConvertedValue = CCurrencyConvertor.ConvertCurrency(Convert.ToDouble(iQuantity),
                                              strISOCode,
                                              infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)), out dChangeApplied);
                    if (dConvertedValue < 0)
                    {
                        Logger_AddLogMessage(string.Format("ChangeQuantityFromCurToUserCur::Error Converting {0} {1} to {2} ", iQuantity, strISOCode, infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))), LogLevels.logERROR);
                        return ((int)ResultType.Result_Error_Generic);
                    }

                    dConvertedValue = dConvertedValue * Math.Pow(10, (double)iFactor);
                    dChangeFee = Convert.ToDouble(infraestructureRepository.GetChangeFeePerc()) * dConvertedValue / 100;
                    iResult = Convert.ToInt32(dConvertedValue - dChangeFee + 0.5);
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }

        private ResultType PerformPrepayRecharge(ref USER oUser, CUSTOMER_PAYMENT_MEAN oUserPaymentMean, int iOSType, bool bAutomatic, int iQuantity, int iAmountToBeAddedToBalance, bool bAutoconf, decimal? dLatitude, decimal? dLongitude,
                                                string strAppVersion, PaymentMeanRechargeCreationType rechargeCreationType, string strMD, string strCAVV, string strECI,
                                                string strBSRedsys3DSTransID, string strBSRedsys3DSPares, string strBSRedsys3DSCres, 
                                                string strBSRedsys3DSMethodData, string strMercadoPagoToken,
                                                string strMPProDescription,
                                                string strMPProTransactionId,
                                                string strMPProReference,
                                                string strMPProCardHash,
                                                string strMPProCardReference,
                                                string strMPProCardScheme,
                                                string strMPProGatewayDate,
                                                string strMPProMaskedCardNumber,
                                                string strMPProExpMonth,
                                                string strMPProExpYear,
                                                string strMPProCardType,
                                                string strMPProDocumentID,
                                                string strMPProDocumentType,
                                                string strMPProInstallaments,
                                                string strMPProCVVLength,
                                                decimal dSourceApp,  bool bPaymentInPerson, bool bSaveIfFail, out decimal? dRechargeId, out string str3DSURL, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            dRechargeId = null;
            str3DSURL = "";
            Stopwatch watch = Stopwatch.StartNew();
            lEllapsedTime = 0;

            try
            {

                if ((oUserPaymentMean != null) &&
                    (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                    (oUserPaymentMean.CUSPM_VALID == 1))
                {

                    decimal dPercVAT1 = 0;
                    decimal dPercVAT2 = 0;
                    decimal dPercFEE = 0;
                    decimal dPercFEETopped = 0;
                    decimal dFixedFEE = 0;
                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    if (oUserPaymentMean != null)
                    {
                        iPaymentTypeId = oUserPaymentMean.CUSPM_PAT_ID;
                        iPaymentSubtypeId = oUserPaymentMean.CUSPM_PAST_ID;
                    }

                    int iQuantityToRecharge = iQuantity;

                    decimal dCurrencyId = oUser.USR_CUR_ID;
                    string sCurrencyIsoCode = oUser.CURRENCy.CUR_ISO_CODE;
                    string sCurrencyIsoCodeNum = oUser.CURRENCy.CUR_ISO_CODE_NUM;

                    if ((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtDebitCreditCard)
                    {
                        if (oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_MIN_CHARGE.HasValue)
                        {
                            if (iQuantity < oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_MIN_CHARGE.Value)
                            {
                                iQuantityToRecharge = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_MIN_CHARGE.Value;
                            }
                        }

                        dCurrencyId = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_CUR_ID;
                        sCurrencyIsoCode = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CURRENCy.CUR_ISO_CODE;
                        sCurrencyIsoCodeNum = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CURRENCy.CUR_ISO_CODE_NUM;
                    }


                    int iPartialVAT1 = 0;
                    int iPartialPercFEE = 0;
                    int iPartialFixedFEE = 0;

                    int iTotalQuantity = 0;

                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    decimal dQuantity = 0;
                    decimal dQuantityToCharge = 0;


                    if (oUser.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                    {

                        if (!customersRepository.GetFinantialParams(oUser, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.BalanceRecharge,
                                                                    out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                        {
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Error getting finantial parameters. Result = {0}", rtRes.ToString()), LogLevels.logERROR);
                        }


                        iTotalQuantity = customersRepository.CalculateFEE(iQuantityToRecharge, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

                        dQuantity = Convert.ToDecimal(iQuantityToRecharge, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode);
                        dQuantityToCharge = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode);
                    }
                    else
                    {
                        iPartialVAT1 = 0;
                        iPartialPercFEE = 0;
                        iPartialFixedFEE = 0;

                        iTotalQuantity = iQuantityToRecharge; // customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);*/                    

                        dQuantity = Convert.ToDecimal(iQuantityToRecharge, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode);
                        dQuantityToCharge = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode);

                    }




                    if ((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtDebitCreditCard)
                    {
                        string strUserReference = null;
                        string strAuthCode = null;
                        string strAuthResult = null;
                        string strAuthResultDesc = "";
                        string strGatewayDate = null;
                        string strTransactionId = null;
                        string strCardScheme = null;
                        string strCFTransactionID = null;
                        string strBSRedsysProtocolVersion = null;
                        int? iBSRedsysNumInlineForms = null;
                        bool? bBSRedsys3DSFrictionless = null;

                        string strCardHash = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;
                        string strCardReference = oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE;
                        string strMaskedCardNumber = oUserPaymentMean.CUSPM_TOKEN_MASKED_CARD_NUMBER;
                        string strCardName = oUserPaymentMean.CUSPM_TOKEN_CARD_NAME;
                        string strCardDocumentID = oUserPaymentMean.CUSPM_TOKEN_CARD_DOCUMENT_ID;
                        DateTime? dtExpirationDate = oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE;


                        bool bPayIsCorrect = false;
                        PaymentMeanRechargeStatus rechargeStatus = (bAutoconf ? PaymentMeanRechargeStatus.Committed : PaymentMeanRechargeStatus.Authorized);

                        if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpCreditCall)
                        {
                            CardEasePayments cardPayment = new CardEasePayments();

                            bPayIsCorrect = cardPayment.AutomaticPayment(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                        oUser.USR_EMAIL,
                                                                        dQuantityToCharge,
                                                                        sCurrencyIsoCode,
                                                                        oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                        bAutoconf,
                                                                        out strUserReference,
                                                                        out strAuthCode,
                                                                        out strAuthResult,
                                                                        out strGatewayDate,
                                                                        out strCardScheme,
                                                                        out strTransactionId);

                            if (!bPayIsCorrect)
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpIECISA)
                        {
                            int iQuantityToRechargeIECISA = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));
                            DateTime dtNow = DateTime.Now;
                            IECISAPayments.IECISAErrorCode eErrorCode;
                            DateTime dtUTCNow = DateTime.UtcNow;
                            IECISAPayments cardPayment = new IECISAPayments();
                            string strErrorMessage="";

                            cardPayment.StartAutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_USER,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                               oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                               oUser.USR_EMAIL,
                                               iQuantityToRechargeIECISA,
                                               sCurrencyIsoCode,
                                               sCurrencyIsoCodeNum,
                                               dtNow,
                                               out eErrorCode,
                                               out strErrorMessage,
                                               out strTransactionId,
                                               out strUserReference);

                            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                            {
                                string errorCode = eErrorCode.ToString();
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                                          errorCode, strErrorMessage));


                            }
                            else
                            {
                                string strRedirectURL = "";
                                cardPayment.GetWebTransactionPaymentTypes(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                        strTransactionId,
                                                                        out eErrorCode,
                                                                        out strErrorMessage,
                                                                        out strRedirectURL);
                                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                {
                                    string errorCode = eErrorCode.ToString();
                                    rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;

                                    m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                              errorCode, strErrorMessage));

                                  
                                }
                                else
                                {

                                    customersRepository.StartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                               oUser.USR_EMAIL,
                                                                               dtUTCNow,
                                                                               dtNow,
                                                                               iQuantityToRecharge,
                                                                               dCurrencyId,
                                                                               "",
                                                                               strUserReference,
                                                                               strTransactionId,
                                                                               "",
                                                                               "",
                                                                               "",
                                                                               PaymentMeanRechargeStatus.Committed);

                                    DateTime? dtTransactionDate = null;                                    
                                    cardPayment.CompleteAutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                           oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                           oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                                           strTransactionId,
                                                                          out eErrorCode,
                                                                          out strErrorMessage,
                                                                          out dtTransactionDate,
                                                                          out strCFTransactionID,
                                                                          out strAuthCode);


                                    if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                    {
                                        string errorCode = eErrorCode.ToString();
                                        rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;

                                        m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                                  errorCode, strErrorMessage));

                                       

                                    }
                                    else
                                    {

                                        strAuthResult = "succeeded";
                                        customersRepository.CompleteStartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                                  oUser.USR_EMAIL,
                                                                                  strTransactionId,
                                                                                  strAuthResult,
                                                                                  strCFTransactionID,
                                                                                  dtTransactionDate.Value.ToString("HHmmssddMMyyyy"),
                                                                                  strAuthCode,
                                                                                  PaymentMeanRechargeStatus.Committed);
                                        strGatewayDate = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                                        rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                        bPayIsCorrect=true;
                                       
                                    }
                                }

                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpStripe)
                        {

                            string result = "";
                            string errorMessage = "";
                            string errorCode = "";
                            string strPAN = "";
                            string strExpirationDateMonth = "";
                            string strExpirationDateYear = "";
                            string strCustomerId = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;

                            int iQuantityToRechargeStripe = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));
                            bPayIsCorrect = StripePayments.PerformCharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                                        oUser.USR_EMAIL,
                                                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                        ref strCustomerId,
                                                                        iQuantityToRechargeStripe,
                                                                        sCurrencyIsoCode,
                                                                        bAutoconf,
                                                                        out result,
                                                                        out errorCode,
                                                                        out errorMessage,
                                                                        out strCardScheme,
                                                                        out strPAN,
                                                                        out strExpirationDateMonth,
                                                                        out strExpirationDateYear,
                                                                        out strTransactionId,
                                                                        out strGatewayDate);

                            if (bPayIsCorrect)
                            {
                                strUserReference = strTransactionId;
                                strAuthCode = "";
                                strAuthResult = "succeeded";

                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }



                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                 PaymentMeanCreditCardProviderType.pmccpMoneris)
                        {
                            MonerisPayments cardPayment = new MonerisPayments();
                            string errorMessage = "";
                            MonerisPayments.MonerisErrorCode eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurrencyIsoCode), provider);

                            strUserReference = MonerisPayments.UserReference();


                            if ((AppUtilities.AppVersion(strAppVersion) >= _VERSION_3_6) &&
                                ((oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_3DS_TRANSACTIONS ?? 0) == 1) && bPaymentInPerson)
                            {

                                DateTime utcNow = DateTime.UtcNow;

                                if (!string.IsNullOrEmpty(strMD) && !string.IsNullOrEmpty(strCAVV))
                                {


                                    infraestructureRepository.UpdateMoneris3DSTransaction(strMD, oUser.USR_EMAIL, strCAVV, strECI, utcNow);

                                    bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep3(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                                                            strUserReference,
                                                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                                            strAmount,
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                                                            "",
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                                                                            strCAVV, strECI,
                                                                                            out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);
                                }
                                else
                                {

                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                    string strInlineForm = "";
                                    string strMDStep1 = "";


                                    bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep1(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                        strUserReference,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                        strAmount,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY, "",
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE.Value,
                                        strReturnURL, "Mozilla",
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                        out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate, out strInlineForm, out strMDStep1);


                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {
                                        decimal? dTransId = null;
                                        bPayIsCorrect = false;

                                        if (infraestructureRepository.AddMoneris3DSTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_ID,
                                                                                               strMDStep1, oUser.USR_EMAIL, iQuantity, utcNow, strInlineForm, out dTransId))
                                        {


                                            string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_GUID +
                                                dTransId.ToString() +
                                                strMDStep1 +
                                                oUser.USR_EMAIL +
                                                utcNow.ToString("HHmmssddMMyy") +
                                                oUser.USR_CULTURE_LANG;

                                            string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_HASH_SEED);


                                            str3DSURL = string.Format("{0}/MonerisMPIRequest?Guid={1}&id={2}&MD={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                                strBaseURL,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_GUID,
                                                dTransId.ToString(),
                                                HttpUtility.UrlEncode(strMDStep1),
                                                HttpUtility.UrlEncode(oUser.USR_EMAIL),
                                                utcNow.ToString("HHmmssddMMyy"),
                                                HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                                strCalcHash
                                                );

                                            str3DSURL = XmlEscape(str3DSURL);

                                            return ResultType.Result_3DS_Validation_Needed;
                                        }
                                    }

                                }

                            }

                            else
                            {

                                string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                string strInlineForm = "";
                                string strMDStep1 = "";


                                bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep1(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                    strUserReference,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                    strAmount,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY, "",
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE.Value,
                                    strReturnURL, "Mozilla",
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                    out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate, out strInlineForm, out strMDStep1);


                                if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                {


                                    bPayIsCorrect = cardPayment.AutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                                strUserReference,
                                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                strAmount,
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                                "",
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0, "",
                                                                out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);
                                }


                            }


                            if (bPayIsCorrect)
                            {
                                bPayIsCorrect = !MonerisPayments.IsError(eErrorCode);
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }


                        }    
                       
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                          PaymentMeanCreditCardProviderType.pmccpPayu)
                        {
                            PayuPayments cardPayment = new PayuPayments();
                            string errorMessage = "";
                            PayuPayments.PayuErrorCode eErrorCode = PayuPayments.PayuErrorCode.InternalError;
                            DateTime? dtTransaction = null;


                            string strLang = ((oUser.USR_CULTURE_LANG.ToLower() ?? "").Length >= 2) ? oUser.USR_CULTURE_LANG.Substring(0, 2) : "es";


                            bPayIsCorrect = cardPayment.AutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_ACCOUNT_ID,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_MERCHANT_ID,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_COUNTRY,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_IS_TEST != 1 ? false : true,
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                    PayuPayments.Language(strLang),
                                                                    oUser.USR_EMAIL,
                                                                    dQuantityToCharge,
                                                                    sCurrencyIsoCode,
                                                                    "RECARGA IPARKME",
                                                                    "",
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_NAME,
                                //oUserPaymentMean.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                    "",
                                                                    (!String.IsNullOrEmpty(oUserPaymentMean.CUSPM_CARD_SECURITY_CODE) ? DecryptCryptResult(oUserPaymentMean.CUSPM_CARD_SECURITY_CODE, ConfigurationManager.AppSettings["CryptKey"]) : String.Empty),
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strTransactionId,
                                                                    out strUserReference,
                                                                    out strAuthCode,
                                                                    out dtTransaction);

                            if (bPayIsCorrect)
                            {
                                strGatewayDate = dtTransaction.Value.ToString("HHmmssddMMyy");
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                          

                        }

                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                          PaymentMeanCreditCardProviderType.pmccpTransbank)
                        {
                            TransBankPayments cardPayment = new TransBankPayments();
                            string errorMessage = "";
                            TransBankPayments.TransBankErrorCode eErrorCode = TransBankPayments.TransBankErrorCode.InternalError;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurrencyIsoCode), provider);

                            strUserReference = TransBankPayments.UserReference();
                            bPayIsCorrect = cardPayment.AutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                oUser.USR_EMAIL,
                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                strUserReference,
                                                strAmount,
                                                out eErrorCode,
                                                out errorMessage,
                                                out strTransactionId,
                                                out strAuthCode,
                                                out strGatewayDate);


                            if (bPayIsCorrect)
                            {
                                bPayIsCorrect = !TransBankPayments.IsError(eErrorCode);
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }


                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                PaymentMeanCreditCardProviderType.pmccpBSRedsys)
                        {
                            
                            string sErrorMessage = "";
                            BSRedsysPayments.BSRedsysErrorCode eResult = BSRedsysPayments.GetErrorInfo(null, out sErrorMessage);
                            string strCustomerId = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;

                            int iQuantityToRechargeBSRedsys = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));

                            var oCardPayments = new BSRedsysPayments();


                            string strMerchantGroup = null;

                            if (oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP != null)
                            {
                                if (!string.IsNullOrEmpty(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                {
                                    strMerchantGroup = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP;
                                }
                            }

                            bool bRedsys3DSProcess = (AppUtilities.AppVersion(strAppVersion) >= _VERSION_3_7_1);

                            if (bRedsys3DSProcess)
                            {
                                Redsys3DSApplyMethod e3DSApplyMethod = (Redsys3DSApplyMethod)(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_3DS_TRANSACTIONS ?? 0);

                                switch (e3DSApplyMethod)
                                {
                                    case Redsys3DSApplyMethod.NotApplyForAllPayments:
                                        bRedsys3DSProcess = false;
                                        break;
                                    case Redsys3DSApplyMethod.ApplyForAllPayments:
                                        bRedsys3DSProcess = true;
                                        break;
                                    case Redsys3DSApplyMethod.ApplyForPaymentsWithAmountGreaterThanMinimum:
                                        bRedsys3DSProcess = (iQuantityToRechargeBSRedsys >= (oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_3DS_TRANSACTIONS_3DS_AMOUNT ?? 0));
                                        break;
                                    default:
                                        bRedsys3DSProcess = false;
                                        break;
                                }

                            }


                            if (bRedsys3DSProcess && bPaymentInPerson && (iOSType != (int)MobileOS.Web))
                            {                             
                                DateTime utcNow = DateTime.UtcNow;

                                if (string.IsNullOrEmpty(strBSRedsys3DSTransID))
                                {

                                    iBSRedsysNumInlineForms = 0;
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/BSRedsys3DSResponse";
                                    strMD = "";
                                    string strPaReq = "";
                                    string strCreq = "";
                                    string strInlineForm = "";

                                    bPayIsCorrect = oCardPayments.StandardPayment3DSStep1(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                       iQuantityToRechargeBSRedsys,
                                                                       sCurrencyIsoCodeNum,
                                                                       strMerchantGroup,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                       strReturnURL,
                                                                       out eResult, out sErrorMessage,
                                                                       out strUserReference,
                                                                       out strTransactionId,
                                                                       out strGatewayDate,
                                                                       out  strInlineForm,
                                                                       out  strMD,
                                                                       out strPaReq,
                                                                       out strCreq,
                                                                       out strBSRedsysProtocolVersion,
                                                                       out strBSRedsys3DSTransID);



                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {
                                        decimal? dTransId = null;
                                        bPayIsCorrect = false;
                                        string strTransId = (string.IsNullOrEmpty(strMD) ? strBSRedsys3DSTransID : strMD);

                                        infraestructureRepository.AddBSRedsys3DSTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_ID, strTransId, strUserReference,
                                            oUser.USR_EMAIL, iQuantityToRechargeBSRedsys, utcNow, strInlineForm, strBSRedsysProtocolVersion, out  dTransId);


                                        string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID +
                                            dTransId.ToString() +
                                            strTransId +
                                            oUser.USR_EMAIL +
                                            utcNow.ToString("HHmmssddMMyy") +
                                            oUser.USR_CULTURE_LANG;

                                        string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_HASH_SEED);


                                        str3DSURL = string.Format("{0}/BSRedsys3DSRequest?Guid={1}&id={2}&threeDSTransId={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                            strBaseURL,
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID,
                                            dTransId.ToString(),
                                            HttpUtility.UrlEncode(strTransId),
                                            HttpUtility.UrlEncode(oUser.USR_EMAIL),
                                            utcNow.ToString("HHmmssddMMyy"),
                                            HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                            strCalcHash
                                            );

                                        str3DSURL = XmlEscape(str3DSURL);

                                        return ResultType.Result_3DS_Validation_Needed;

                                    }

                                    bBSRedsys3DSFrictionless = true;



                                }
                                else if (string.IsNullOrEmpty(strBSRedsys3DSPares) && string.IsNullOrEmpty(strBSRedsys3DSCres))
                                {
                                    infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strBSRedsys3DSPares, strBSRedsys3DSCres,
                                        utcNow, out strUserReference, out strBSRedsysProtocolVersion, out iBSRedsysNumInlineForms);
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/BSRedsys3DSResponse";
                                    strMD = "";
                                    string strPaReq = "";
                                    string strCreq = "";
                                    string strInlineForm = "";

                                    bPayIsCorrect = oCardPayments.StandardPayment3DSStep2(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                       iQuantityToRechargeBSRedsys,
                                                                       sCurrencyIsoCodeNum,
                                                                       strMerchantGroup,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_HASH, strUserReference,
                                                                       strReturnURL, strBSRedsysProtocolVersion, strBSRedsys3DSTransID, "Y",
                                                                       out eResult, out sErrorMessage,
                                                                       out strTransactionId,
                                                                       out strGatewayDate,
                                                                       out  strInlineForm,
                                                                       out  strMD,
                                                                       out strPaReq,
                                                                       out strCreq);



                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {
                                        decimal? dTransId = null;
                                        bPayIsCorrect = false;

                                        infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strInlineForm, utcNow, out dTransId);


                                        string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID +
                                            dTransId.ToString() +
                                            strBSRedsys3DSTransID +
                                            oUser.USR_EMAIL +
                                            utcNow.ToString("HHmmssddMMyy") +
                                            oUser.USR_CULTURE_LANG;

                                        string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_HASH_SEED);


                                        str3DSURL = string.Format("{0}/BSRedsys3DSRequest?Guid={1}&id={2}&threeDSTransId={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                            strBaseURL,
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID,
                                            dTransId.ToString(),
                                            HttpUtility.UrlEncode(strBSRedsys3DSTransID),
                                            HttpUtility.UrlEncode(oUser.USR_EMAIL),
                                            utcNow.ToString("HHmmssddMMyy"),
                                            HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                            strCalcHash
                                            );

                                        str3DSURL = XmlEscape(str3DSURL);

                                        return ResultType.Result_3DS_Validation_Needed;

                                    }

                                    bBSRedsys3DSFrictionless = true;


                                }
                                else
                                {

                                    //((!string.IsNullOrEmpty(strBSRedsys3DSPares) || !string.IsNullOrEmpty(strBSRedsys3DSPares))&& (!string.IsNullOrEmpty(strBSRedsys3DSTransID)))

                                    infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strBSRedsys3DSPares, strBSRedsys3DSCres, utcNow, out strUserReference, out strBSRedsysProtocolVersion, out iBSRedsysNumInlineForms);

                                    string strProtocolBackup = strBSRedsysProtocolVersion;

                                    strMD = (string.IsNullOrEmpty(strBSRedsys3DSPares) ? "" : strBSRedsys3DSTransID);

                                    bPayIsCorrect = oCardPayments.StandardPayment3DSStep3(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                       iQuantityToRechargeBSRedsys,
                                                                       sCurrencyIsoCodeNum,
                                                                       strMerchantGroup,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_HASH, strUserReference,
                                                                       strMD, strBSRedsys3DSPares, strBSRedsys3DSCres, ref strBSRedsysProtocolVersion,
                                                                       out eResult, out sErrorMessage,
                                                                       out strTransactionId,
                                                                       out strGatewayDate);

                                    if (strProtocolBackup != strBSRedsysProtocolVersion)
                                    {
                                        infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strBSRedsysProtocolVersion, utcNow);
                                    }

                                    strMD = "";
                                    bBSRedsys3DSFrictionless = false;

                                }

                            }

                            else
                            {

                                bPayIsCorrect = oCardPayments.StandardPaymentNO3DS(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                          oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                          iQuantityToRechargeBSRedsys,
                                                                          sCurrencyIsoCodeNum,
                                                                          strMerchantGroup,
                                                                          oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                          out eResult, out sErrorMessage,
                                                                          out strTransactionId,
                                                                          out strUserReference,
                                                                          out strGatewayDate);
                            }

                           

                            if (bPayIsCorrect)
                            {
                                strAuthCode = "";
                                strAuthResult = "succeeded";
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;


                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                strCardScheme = oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA;
                                customersRepository.StartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                          oUser.USR_EMAIL,
                                                                          dtUTCNow,
                                                                          dtNow,
                                                                          iQuantityToRecharge,
                                                                          dCurrencyId,
                                                                          "",
                                                                          strUserReference,
                                                                          strTransactionId,
                                                                          "",
                                                                          strGatewayDate,
                                                                          strAuthCode,
                                                                          PaymentMeanRechargeStatus.Committed);

                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Failed_To_Commit;
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                PaymentMeanCreditCardProviderType.pmccpPaysafe)
                        {
                            string sErrorMessage = "";                                                        

                            int iQuantityToRechargePaysafe = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));

                            var oCardPayments = new PaysafePayments();
                            var oPaysafeConfig = new PaysafePayments.PaysafeMerchantInfo(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ACCOUNT_NUMBER, 
                                                                                         oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_KEY, 
                                                                                         oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_SECRET, 
                                                                                         oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ENVIRONMENT);

                            DateTime? dtPaysafeDateTime = null;
                            string strPAN = "";
                            string strExpirationDateMonth = "";
                            string strExpirationDateYear = "";

                            bPayIsCorrect = oCardPayments.Authorize(oPaysafeConfig, oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE, 
                                                                    iQuantityToRechargePaysafe, oUserPaymentMean.CUSPM_TOKEN_CARD_DOCUMENT_ID,                                                                          
                                                                    out strTransactionId, out strUserReference, out dtPaysafeDateTime, out strExpirationDateYear, out strExpirationDateMonth, out strPAN, out sErrorMessage);

                            if (bPayIsCorrect)
                            {
                                strAuthCode = "";
                                strAuthResult = "succeeded";
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                if (dtPaysafeDateTime.HasValue)
                                    strGatewayDate = dtPaysafeDateTime.Value.ToString("HHmmssddMMyy");
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }



                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                        {
                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();
                            string errorMessage = "";
                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            //string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurrencyIsoCode), provider);
                            //
                            //

                            bool bAllowAuthorizationAndCapture = MercadoPagoPayments.AllowAuthorizationAndCapture(oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                                                                  oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE);
                            bool bAllowTransactionWithoutCVV = MercadoPagoPayments.AllowTransactionWithoutCVV(oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                                                              oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE);

                            Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Card Schema={0}  Card Type={1} bAllowAuthorizationAndCapture={2} bAllowTransactionWithoutCVV={3}",
                                oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE,
                                bAllowAuthorizationAndCapture,
                                bAllowTransactionWithoutCVV), LogLevels.logINFO);


                            if (!bPaymentInPerson && !bAllowTransactionWithoutCVV)
                            {
                                rtRes = ResultType.Result_Error_Recharge_Not_Possible;
                                Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0} bPaymentInPerson={1} bAllowTransactionWithoutCVV={2}",
                                    rtRes.ToString(), bPaymentInPerson, bAllowTransactionWithoutCVV), LogLevels.logERROR);
                            }
                            else if (bAllowTransactionWithoutCVV)
                            {

                                strUserReference = MercadoPagoPayments.UserReference();

                                bPayIsCorrect = cardPayment.AutomaticTransaction(
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_API_URL,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_SERVICE_TIMEOUT,
                                    strUserReference,
                                    dQuantityToCharge,
                                    "",
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                    Convert.ToInt32(oUserPaymentMean.CUSPM_TOKEN_INSTALLMENTS),
                                    true,
                                    out eErrorCode,
                                    out errorMessage,
                                    out strTransactionId,
                                    out strGatewayDate);



                            }
                            else // if (!bAllowTransactionWithoutCVV)
                            {
                                if (!string.IsNullOrEmpty(strMercadoPagoToken))
                                {
                                    strUserReference = MercadoPagoPayments.UserReference();

                                    bPayIsCorrect = cardPayment.AutomaticTransaction(
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                            strUserReference,
                                            dQuantityToCharge,
                                            "",
                                            strMercadoPagoToken,
                                            oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                            Convert.ToInt32(oUserPaymentMean.CUSPM_TOKEN_INSTALLMENTS),
                                            true,
                                            false,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strTransactionId,
                                            out strGatewayDate);

                                }
                                else
                                {
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strCVVURL = strBaseURL + "/MercadoPagoCVVRequest";

                                    DateTime utcNow = DateTime.UtcNow;

                                    string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID +
                                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE +
                                                                                    oUserPaymentMean.CUSPM_CVV_LENGTH.ToString() +
                                                                                    utcNow.ToString("HHmmssddMMyy") +
                                                                                    oUser.USR_CULTURE_LANG;

                                    string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_HASH_SEED);


                                    str3DSURL = string.Format("{0}?Guid={1}&cardId={2}&cvvLength={3}&UTCDate={4}&Culture={5}&Hash={6}",
                                        strCVVURL,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                        oUserPaymentMean.CUSPM_CVV_LENGTH.ToString(),
                                        utcNow.ToString("HHmmssddMMyy"),
                                        HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                        strCalcHash
                                        );

                                    str3DSURL = XmlEscape(str3DSURL);

                                    return ResultType.Result_3DS_Validation_Needed;



                                }
                            }


                            if (bPayIsCorrect)
                            {
                                bPayIsCorrect = !MercadoPagoPayments.IsError(eErrorCode);
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                strCardScheme = oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA;
                                customersRepository.StartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                          oUser.USR_EMAIL,
                                                                          dtUTCNow,
                                                                          dtNow,
                                                                          iQuantityToRecharge,
                                                                          dCurrencyId,
                                                                          "",
                                                                          strUserReference,
                                                                          strTransactionId,
                                                                          "",
                                                                          strGatewayDate,
                                                                          "",
                                                                          PaymentMeanRechargeStatus.Committed);
                            }


                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                 PaymentMeanCreditCardProviderType.pmccpMercadoPagoPro)
                        {

                            if (!bPaymentInPerson)
                            {
                                rtRes = ResultType.Result_Error_Recharge_Not_Possible;
                                Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0} bPaymentInPerson={1}",
                                    rtRes.ToString(), bPaymentInPerson), LogLevels.logERROR);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(strMPProTransactionId))
                                {
                                    strTransactionId = strMPProTransactionId;
                                    strUserReference = strMPProReference;
                                    strGatewayDate = strMPProGatewayDate;
                                    //strMPProCardType;
                                    //strMPProDocumentType;
                                    //strMPProInstallaments;
                                    //strMPProCVVLength;
                                    strCardHash = strMPProCardHash;
                                    strCardReference = strMPProCardReference;
                                    strCardScheme = strMPProCardScheme;
                                    strMaskedCardNumber = strMPProMaskedCardNumber;
                                    strCardDocumentID = strMPProDocumentID;

                                    if ((strMPProExpMonth.Length > 0) && (strMPProExpYear.Length == 4))
                                    {
                                        dtExpirationDate = new DateTime(Convert.ToInt32(strMPProExpYear), Convert.ToInt32(strMPProExpMonth), 1).AddMonths(1).AddSeconds(-1);
                                    }
                                    bPayIsCorrect = true;

                                }
                                else
                                {
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;

                                    DateTime utcNow = DateTime.UtcNow;
                                    int iQuantityToRechargeMPPro = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));

                                    string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID +
                                                oUser.USR_EMAIL +
                                                iQuantityToRechargeMPPro.ToString() +
                                                sCurrencyIsoCode +
                                                strMPProDescription +
                                                utcNow.ToString("HHmmssddMMyy") +
                                                oUser.USR_CULTURE_LANG;

                                    string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_HASH_SEED);


                                    str3DSURL = string.Format("{0}?Guid={1}&Email={2}&Amount={3}&CurrencyISOCODE={4}&Description={5}&UTCDate={6}&Culture={7}&Hash={8}",
                                        strFormURL,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID,
                                        oUser.USR_EMAIL,
                                        iQuantityToRechargeMPPro.ToString(),
                                        sCurrencyIsoCode,
                                        HttpUtility.UrlEncode(strMPProDescription),
                                        utcNow.ToString("HHmmssddMMyy"),
                                        HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                        strCalcHash
                                        );

                                    str3DSURL = XmlEscape(str3DSURL);

                                    return ResultType.Result_3DS_Validation_Needed;

                                }
                            }


                            if (bPayIsCorrect)
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                            }


                        }





                        if (bPayIsCorrect || bSaveIfFail)
                        {
                            int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                            int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                            if (!customersRepository.RechargeUserBalance(ref oUser,
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                            iOSType,
                                            (!bSaveIfFail || rechargeStatus == PaymentMeanRechargeStatus.Committed) /*true*/,
                                            iQuantityToRecharge,
                                            iAmountToBeAddedToBalance,
                                            dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                //Convert.ToInt32(dQuantityToCharge * 100),                                             
                                            oUser.CURRENCy.CUR_ID,
                                            PaymentSuscryptionType.pstPrepay,
                                            rechargeStatus,
                                            rechargeCreationType,
                                //dVAT,
                                            strUserReference,
                                            strTransactionId,
                                            strCFTransactionID,
                                            strGatewayDate,
                                            strAuthCode,
                                            strAuthResult,
                                            strAuthResultDesc,
                                            strCardHash,
                                            strCardReference,
                                            strCardScheme,
                                            strMaskedCardNumber,
                                            strCardName,
                                            strCardDocumentID,
                                            dtExpirationDate,
                                            null,
                                            null,
                                            null,
                                            false,
                                            dLatitude,
                                            dLongitude,
                                            strAppVersion, strMD,strCAVV,strECI,
                                            strBSRedsys3DSTransID,
                                            strBSRedsysProtocolVersion,
                                            iBSRedsysNumInlineForms,
                                            bBSRedsys3DSFrictionless,
                                            dSourceApp,
                                            infraestructureRepository,
                                            out dRechargeId))
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                            }
                            else
                            {
                                rtRes = ResultType.Result_OK;
                            }

                        }
                        else
                        {
                            if (bAutomatic)
                            {
                                customersRepository.AutomaticRechargeFailure(ref oUser);
                            }
                            rtRes = ResultType.Result_Error_Recharge_Failed;
                            Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                        }

                    }
                    /*else if (((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtPaypal) &&
                        (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1))
                    {
                        PayPal.Services.Private.AP.PayResponse PResponse = null;

                        if (!PaypalPayments.PreapprovalPayRequest(oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID,
                                                                oUserPaymentMean.CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY,
                                                                dQuantityToCharge,
                                                                oUserPaymentMean.CURRENCy.CUR_ISO_CODE,
                                                                "en-US",
                                                                "http://localhost",
                                                                "http://localhost",
                                                                out PResponse))
                        {
                            if (bAutomatic)
                            {
                                customersRepository.AutomaticRechargeFailure(ref oUser);
                            }
                            rtRes = ResultType.Result_Error_Recharge_Failed;
                            Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                        }
                        else
                        {
                            if (PResponse.paymentExecStatus != "COMPLETED")
                            {
                                rtRes = ResultType.Result_Error_Recharge_Failed;
                                Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                            }
                            else
                            {
                                PayPal.Services.Private.AP.PaymentDetailsResponse PDResponse = null;

                                if (PaypalPayments.PreapprovalPayConfirm(PResponse.payKey,
                                                                            "en-US",
                                                                            out PDResponse))
                                {


                                    int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                                    int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                                    if (!customersRepository.RechargeUserBalance(ref oUser,
                                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                                                iOSType,
                                                                                true,
                                                                                iQuantity,
                                                                                dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                        //Convert.ToInt32(dQuantityToCharge * 100),
                                                                                dCurrencyId,
                                                                                PaymentSuscryptionType.pstPrepay,
                                                                                PaymentMeanRechargeStatus.Committed,
                                                                                rechargeCreationType,
                                        //dVAT,
                                                                                null,
                                                                                PDResponse.paymentInfoList[0].transactionId,
                                                                                null,
                                                                                DateTime.Now.ToUniversalTime().ToString(),
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                PResponse.payKey,
                                                                                false,
                                                                                dLatitude,
                                                                                dLongitude,
                                                                                strAppVersion,
                                                                                infraestructureRepository,
                                                                                out dRechargeId))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                                    }
                                    else
                                    {
                                        rtRes = ResultType.Result_OK;
                                    }

                                }
                                else
                                {
                                    if (bAutomatic)
                                    {
                                        customersRepository.AutomaticRechargeFailure(ref oUser);
                                    }
                                    rtRes = ResultType.Result_Error_Recharge_Failed;
                                    Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                                }
                            }
                        }
                    }*/
                    else
                    {
                        rtRes = ResultType.Result_Error_Recharge_Not_Possible;
                        Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);
                    }
                }
                else
                {
                    rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                    Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "PerformPrepayRecharge::Exception", LogLevels.logERROR);

            }
            lEllapsedTime = watch.ElapsedMilliseconds;
            watch.Stop();

            return rtRes;

        }

        private ResultType PerformPerTransactionRecharge(ref USER oUser, CUSTOMER_PAYMENT_MEAN oUserPaymentMean, int iOSType, int iQuantity, decimal? dLatitude, decimal? dLongitude, string strAppVersion,
                                                         string strMD, string strCAVV, string strECI, string strBSRedsys3DSTransID, string strBSRedsys3DSPares, 
                                                         string strBSRedsys3DSCres, string strBSRedsys3DSMethodData,string strMercadoPagoToken,
                                                         string strMPProDescription,
                                                         string strMPProTransactionId,
                                                         string strMPProReference,
                                                         string strMPProCardHash,
                                                         string strMPProCardReference,
                                                         string strMPProCardScheme,
                                                         string strMPProGatewayDate,
                                                         string strMPProMaskedCardNumber,
                                                         string strMPProExpMonth,
                                                         string strMPProExpYear,
                                                         string strMPProCardType,
                                                         string strMPProDocumentID,
                                                         string strMPProDocumentType,
                                                         string strMPProInstallaments,
                                                         string strMPProCVVLength,
                                                         decimal dSourceApp, bool bPaymentInPerson, bool bSaveIfFail, out decimal? dRechargeId, out string str3DSURL, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            dRechargeId = null;
            str3DSURL = "";

            Stopwatch watch = Stopwatch.StartNew();
            lEllapsedTime = 0;


            try
            {

                if ((oUserPaymentMean != null) &&
                    (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                    (oUserPaymentMean.CUSPM_VALID == 1))
                {
                    decimal dPercVAT1 = 0;
                    decimal dPercVAT2 = 0;
                    decimal dPercFEE = 0;
                    int iPercFEETopped = 0;
                    int iFixedFEE = 0;

                    int iQuantityToRecharge = iQuantity;

                    decimal dCurrencyId = oUser.USR_CUR_ID;
                    string sCurrencyIsoCode = oUser.CURRENCy.CUR_ISO_CODE;
                    string sCurrencyIsoCodeNum = oUser.CURRENCy.CUR_ISO_CODE_NUM;

                    if ((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtDebitCreditCard)
                    {
                        if (oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_MIN_CHARGE.HasValue)
                        {
                            if (iQuantity < oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_MIN_CHARGE.Value)
                            {
                                iQuantityToRecharge = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_MIN_CHARGE.Value;
                            }
                        }

                        dCurrencyId = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_CUR_ID;
                        sCurrencyIsoCode = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CURRENCy.CUR_ISO_CODE;
                        sCurrencyIsoCodeNum = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CURRENCy.CUR_ISO_CODE_NUM;
                    }

                    /*int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    if (oUserPaymentMean != null)
                    {
                        iPaymentTypeId = oUserPaymentMean.CUSPM_PAT_ID;
                        iPaymentSubtypeId = oUserPaymentMean.CUSPM_PAST_ID;
                    }

                    if (!customersRepository.GetFinantialParams(oUser, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.BalanceRecharge,
                                                                out dPercVAT1, out dPercVAT2, out dPercFEE, out iPercFEETopped, out iFixedFEE))
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Error getting finantial parameters. Result = {0}", rtRes.ToString()), LogLevels.logERROR);
                    }*/

                    int iPartialVAT1 = 0;
                    int iPartialPercFEE = 0;
                    int iPartialFixedFEE = 0;

                    int iTotalQuantity = iQuantityToRecharge; // customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);*/                    

                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    decimal dQuantity = Convert.ToDecimal(iQuantityToRecharge, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode);
                    decimal dQuantityToCharge = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode);


                    /*decimal dFeeVal = 0;
                    decimal dFeePerc = 0;

                    customersRepository.GetPaymentMeanFees(ref oUser, out dFeeVal, out dFeePerc);
                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    decimal dQuantity = Convert.ToDecimal(iQuantity, numberFormatProvider) / 100;
                    decimal dQuantityToCharge = Math.Round(dQuantity + (dQuantity  * dFeePerc / 100 + dFeeVal / 100), 2);*/

                    if ((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtDebitCreditCard)
                    {
                        string strUserReference = null;
                        string strAuthCode = null;
                        string strAuthResultDesc = "";
                        string strAuthResult = null;
                        string strGatewayDate = null;
                        string strTransactionId = null;
                        string strCardScheme = null;
                        string strCFTransactionID = null;
                        string strBSRedsysProtocolVersion = null;
                        int? iBSRedsysNumInlineForms = null;
                        bool? bBSRedsys3DSFrictionless = null;

                        string strCardHash = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;
                        string strCardReference = oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE;
                        string strMaskedCardNumber = oUserPaymentMean.CUSPM_TOKEN_MASKED_CARD_NUMBER;
                        string strCardName = oUserPaymentMean.CUSPM_TOKEN_CARD_NAME;
                        string strCardDocumentID = oUserPaymentMean.CUSPM_TOKEN_CARD_DOCUMENT_ID;
                        DateTime? dtExpirationDate = oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE;


                        bool bPayIsCorrect = false;
                        PaymentMeanRechargeStatus rechargeStatus = PaymentMeanRechargeStatus.Authorized;

                        if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpCreditCall)
                        {
                            CardEasePayments cardPayment = new CardEasePayments();

                            bPayIsCorrect = cardPayment.AutomaticPayment(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                        oUser.USR_EMAIL,
                                                                        dQuantityToCharge,
                                                                        sCurrencyIsoCode,
                                                                        oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                        false,
                                                                        out strUserReference,
                                                                        out strAuthCode,
                                                                        out strAuthResult,
                                                                        out strGatewayDate,
                                                                        out strCardScheme,
                                                                        out strTransactionId);

                            if (!bPayIsCorrect)
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpIECISA)
                        {

                            int iQuantityToRechargeIECISA = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));                       
                            DateTime dtNow = DateTime.Now;
                            IECISAPayments.IECISAErrorCode eErrorCode;
                            DateTime dtUTCNow = DateTime.UtcNow;
                            IECISAPayments cardPayment = new IECISAPayments();
                            string strErrorMessage = "";

                            cardPayment.StartAutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_USER,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                               oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                               oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                               oUser.USR_EMAIL,
                                               iQuantityToRechargeIECISA,
                                               sCurrencyIsoCode,
                                               sCurrencyIsoCodeNum,
                                               dtNow,
                                               out eErrorCode,
                                               out strErrorMessage,
                                               out strTransactionId,
                                               out strUserReference);

                            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                            {
                                string errorCode = eErrorCode.ToString();
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPerTransactionRecharge.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                                          errorCode, strErrorMessage));


                            }
                            else
                            {
                                string strRedirectURL = "";
                                cardPayment.GetWebTransactionPaymentTypes(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                        strTransactionId,
                                                                        out eErrorCode,
                                                                        out strErrorMessage,
                                                                        out strRedirectURL);
                                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                {
                                    string errorCode = eErrorCode.ToString();
                                    rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;

                                    m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPerTransactionRecharge.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                              errorCode, strErrorMessage));


                                }
                                else
                                {
                                    customersRepository.StartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                              oUser.USR_EMAIL,
                                                                              dtUTCNow,
                                                                              dtNow,
                                                                              iQuantityToRecharge,
                                                                              dCurrencyId,
                                                                              "",
                                                                              strUserReference,
                                                                              strTransactionId,
                                                                              "",
                                                                              "",
                                                                              "",
                                                                              PaymentMeanRechargeStatus.Committed);

                                    DateTime? dtTransactionDate = null;
                                    cardPayment.CompleteAutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                           oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                           oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                           strTransactionId,
                                                          out eErrorCode,
                                                          out strErrorMessage,
                                                          out dtTransactionDate,
                                                          out strCFTransactionID,
                                                          out strAuthCode);


                                    if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                    {
                                        string errorCode = eErrorCode.ToString();
                                        rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;

                                        m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPerTransactionRecharge.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                                  errorCode, strErrorMessage));



                                    }
                                    else
                                    {

                                        strAuthResult = "succeeded";
                                        customersRepository.CompleteStartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                                  oUser.USR_EMAIL,
                                                                                  strTransactionId,
                                                                                  strAuthResult,
                                                                                  strCFTransactionID,
                                                                                  dtTransactionDate.Value.ToString("HHmmssddMMyyyy"),
                                                                                  strAuthCode,
                                                                                  PaymentMeanRechargeStatus.Committed);
                                        strGatewayDate = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                                        rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                        bPayIsCorrect = true;

                                    }
                                }

                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                PaymentMeanCreditCardProviderType.pmccpStripe)
                        {

                            string result = "";
                            string errorMessage = "";
                            string errorCode = "";
                            string strPAN = "";
                            string strExpirationDateMonth = "";
                            string strExpirationDateYear = "";
                            string strCustomerId = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;

                            int iQuantityToRechargeStripe = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));
                            bPayIsCorrect = StripePayments.PerformCharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                                        oUser.USR_EMAIL,
                                                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                        ref strCustomerId,
                                                                        iQuantityToRechargeStripe,
                                                                        sCurrencyIsoCode,
                                                                        false,
                                                                        out result,
                                                                        out errorCode,
                                                                        out errorMessage,
                                                                        out strCardScheme,
                                                                        out strPAN,
                                                                        out strExpirationDateMonth,
                                                                        out strExpirationDateYear,
                                                                        out strTransactionId,
                                                                        out strGatewayDate);

                            if (bPayIsCorrect)
                            {
                                strUserReference = strTransactionId;
                                strAuthCode = "";
                                strAuthResult = "succeeded";

                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }

                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                         PaymentMeanCreditCardProviderType.pmccpMoneris)
                        {
                            MonerisPayments cardPayment = new MonerisPayments();
                            string errorMessage = "";
                            MonerisPayments.MonerisErrorCode eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurrencyIsoCode), provider);

                            strUserReference = MonerisPayments.UserReference();


                            if ((AppUtilities.AppVersion(strAppVersion) >= _VERSION_3_6) &&
                                ((oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_3DS_TRANSACTIONS ?? 0) == 1) && bPaymentInPerson)
                            {

                                DateTime utcNow = DateTime.UtcNow;

                                if (!string.IsNullOrEmpty(strMD) && !string.IsNullOrEmpty(strCAVV))
                                {


                                    infraestructureRepository.UpdateMoneris3DSTransaction(strMD, oUser.USR_EMAIL, strCAVV, strECI, utcNow);

                                    bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep3(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                                                            strUserReference,
                                                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                                            oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                                            strAmount,
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                                                            "",
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                                                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                                                                            strCAVV, strECI,
                                                                                            out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);

                                }
                                else
                                {

                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                    string strInlineForm = "";
                                    string strMDStep1 = "";


                                    bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep1(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                        strUserReference,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                        strAmount,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY, "",
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE.Value,
                                        strReturnURL, "Mozilla",
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                        out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate, out strInlineForm, out strMDStep1);


                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {
                                        decimal? dTransId = null;
                                        bPayIsCorrect = false;

                                        if (infraestructureRepository.AddMoneris3DSTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_ID,
                                                                                               strMDStep1, oUser.USR_EMAIL, iQuantity, utcNow, strInlineForm, out dTransId))
                                        {


                                            string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_GUID +
                                                dTransId.ToString() +
                                                strMDStep1 +
                                                oUser.USR_EMAIL +
                                                utcNow.ToString("HHmmssddMMyy") +
                                                oUser.USR_CULTURE_LANG;

                                            string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_HASH_SEED);


                                            str3DSURL = string.Format("{0}/MonerisMPIRequest?Guid={1}&id={2}&MD={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                                strBaseURL,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_GUID,
                                                dTransId.ToString(),
                                                HttpUtility.UrlEncode(strMDStep1),
                                                HttpUtility.UrlEncode(oUser.USR_EMAIL),
                                                utcNow.ToString("HHmmssddMMyy"),
                                                HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                                strCalcHash
                                                );

                                            str3DSURL = XmlEscape(str3DSURL);

                                            return ResultType.Result_3DS_Validation_Needed;
                                        }
                                    }

                                }

                            }

                            else
                            {

                                string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                string strInlineForm = "";
                                string strMDStep1 = "";


                                bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep1(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                    strUserReference,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                    strAmount,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY, "",
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_EXPIRATION_DATE.Value,
                                    strReturnURL, "Mozilla",
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                    out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate, out strInlineForm, out strMDStep1);


                                if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                {
                                    bPayIsCorrect = cardPayment.AutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                                strUserReference,
                                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE == oUserPaymentMean.CUSPM_TOKEN_CARD_HASH ? "" : oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                strAmount,
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                                "",
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0, "",
                                                                out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);

                                }

                            }


                            if (bPayIsCorrect)
                            {
                                bPayIsCorrect = !MonerisPayments.IsError(eErrorCode);
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }


                        }                            
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                          PaymentMeanCreditCardProviderType.pmccpPayu)
                        {
                            PayuPayments cardPayment = new PayuPayments();
                            string errorMessage = "";
                            PayuPayments.PayuErrorCode eErrorCode = PayuPayments.PayuErrorCode.InternalError;
                            DateTime? dtTransaction = null;


                            string strLang = ((oUser.USR_CULTURE_LANG.ToLower() ?? "").Length >= 2) ? oUser.USR_CULTURE_LANG.Substring(0, 2) : "es";


                            bPayIsCorrect = cardPayment.AutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_ACCOUNT_ID,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_MERCHANT_ID,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_COUNTRY,
                                                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_IS_TEST != 1 ? false : true,
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                    PayuPayments.Language(strLang),
                                                                    oUser.USR_EMAIL,
                                                                    dQuantityToCharge,
                                                                    sCurrencyIsoCode,
                                                                    "RECARGA IPARKME",
                                                                    "",
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_NAME,
                                //oUserPaymentMean.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                    "",
                                                                    (!String.IsNullOrEmpty(oUserPaymentMean.CUSPM_CARD_SECURITY_CODE) ? DecryptCryptResult(oUserPaymentMean.CUSPM_CARD_SECURITY_CODE, ConfigurationManager.AppSettings["CryptKey"]) : String.Empty),
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strTransactionId,
                                                                    out strUserReference,
                                                                    out strAuthCode,
                                                                    out dtTransaction);

                            if (bPayIsCorrect)
                            {
                                strGatewayDate = dtTransaction.Value.ToString("HHmmssddMMyy");
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }


                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                          PaymentMeanCreditCardProviderType.pmccpTransbank)
                        {
                            TransBankPayments cardPayment = new TransBankPayments();
                            string errorMessage = "";
                            TransBankPayments.TransBankErrorCode eErrorCode = TransBankPayments.TransBankErrorCode.InternalError;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurrencyIsoCode), provider);

                            strUserReference = TransBankPayments.UserReference();
                            bPayIsCorrect = cardPayment.AutomaticTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                oUser.USR_EMAIL,
                                                oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                strUserReference,
                                                strAmount,
                                                out eErrorCode,
                                                out errorMessage,
                                                out strTransactionId,
                                                out strAuthCode,
                                                out strGatewayDate);


                            if (bPayIsCorrect)
                            {
                                bPayIsCorrect = !TransBankPayments.IsError(eErrorCode);
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                PaymentMeanCreditCardProviderType.pmccpBSRedsys)
                        {
                            
                            string sErrorMessage = "";
                            BSRedsysPayments.BSRedsysErrorCode eResult = BSRedsysPayments.GetErrorInfo(null, out sErrorMessage);
                            string strCustomerId = oUserPaymentMean.CUSPM_TOKEN_CARD_HASH;

                            int iQuantityToRechargeBSRedsys = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));

                            var oCardPayments = new BSRedsysPayments();

                            string strMerchantGroup = null;

                            if (oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP != null)
                            {
                                if (!string.IsNullOrEmpty(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                {
                                    strMerchantGroup = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP;
                                }
                            }


                            bool bRedsys3DSProcess = (AppUtilities.AppVersion(strAppVersion) >= _VERSION_3_7_1);

                            if (bRedsys3DSProcess)
                            {
                                Redsys3DSApplyMethod e3DSApplyMethod = (Redsys3DSApplyMethod)(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_3DS_TRANSACTIONS ?? 0);

                                switch (e3DSApplyMethod)
                                {
                                    case Redsys3DSApplyMethod.NotApplyForAllPayments:
                                        bRedsys3DSProcess = false;
                                        break;
                                    case Redsys3DSApplyMethod.ApplyForAllPayments:
                                        bRedsys3DSProcess = true;
                                        break;
                                    case Redsys3DSApplyMethod.ApplyForPaymentsWithAmountGreaterThanMinimum:
                                        bRedsys3DSProcess = (iQuantityToRechargeBSRedsys >= (oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_3DS_TRANSACTIONS_3DS_AMOUNT ?? 0));
                                        break;
                                    default:
                                        bRedsys3DSProcess = false;
                                        break;
                                }

                            }


                            if (bRedsys3DSProcess && bPaymentInPerson && (iOSType != (int)MobileOS.Web))
                            {
                                DateTime utcNow = DateTime.UtcNow;

                                if (string.IsNullOrEmpty(strBSRedsys3DSTransID))
                                {

                                    iBSRedsysNumInlineForms = 0;
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/BSRedsys3DSResponse";
                                    strMD = "";
                                    string strPaReq = "";
                                    string strCreq = "";
                                    string strInlineForm = "";

                                    bPayIsCorrect = oCardPayments.StandardPayment3DSStep1(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                       iQuantityToRechargeBSRedsys,
                                                                       sCurrencyIsoCodeNum,
                                                                       strMerchantGroup,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                       strReturnURL,
                                                                       out eResult, out sErrorMessage,
                                                                       out strUserReference,
                                                                       out strTransactionId,
                                                                       out strGatewayDate,
                                                                       out  strInlineForm,
                                                                       out  strMD,
                                                                       out strPaReq,
                                                                       out strCreq,
                                                                       out strBSRedsysProtocolVersion,
                                                                       out strBSRedsys3DSTransID);



                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {
                                        decimal? dTransId = null;
                                        bPayIsCorrect = false;
                                        string strTransId = (string.IsNullOrEmpty(strMD) ? strBSRedsys3DSTransID : strMD);

                                        infraestructureRepository.AddBSRedsys3DSTransaction(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_ID, strTransId, strUserReference,
                                            oUser.USR_EMAIL, iQuantityToRechargeBSRedsys, utcNow, strInlineForm, strBSRedsysProtocolVersion, out  dTransId);


                                        string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID +
                                            dTransId.ToString() +
                                            strTransId +
                                            oUser.USR_EMAIL +
                                            utcNow.ToString("HHmmssddMMyy") +
                                            oUser.USR_CULTURE_LANG;

                                        string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_HASH_SEED);


                                        str3DSURL = string.Format("{0}/BSRedsys3DSRequest?Guid={1}&id={2}&threeDSTransId={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                            strBaseURL,
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID,
                                            dTransId.ToString(),
                                            HttpUtility.UrlEncode(strTransId),
                                            HttpUtility.UrlEncode(oUser.USR_EMAIL),
                                            utcNow.ToString("HHmmssddMMyy"),
                                            HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                            strCalcHash
                                            );

                                        str3DSURL = XmlEscape(str3DSURL);

                                        return ResultType.Result_3DS_Validation_Needed;

                                    }

                                    bBSRedsys3DSFrictionless = true;



                                }
                                else if (string.IsNullOrEmpty(strBSRedsys3DSPares) && string.IsNullOrEmpty(strBSRedsys3DSCres))
                                {
                                    infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strBSRedsys3DSPares, strBSRedsys3DSCres,
                                        utcNow, out strUserReference, out strBSRedsysProtocolVersion, out iBSRedsysNumInlineForms);
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/BSRedsys3DSResponse";
                                    strMD = "";
                                    string strPaReq = "";
                                    string strCreq = "";
                                    string strInlineForm = "";

                                    bPayIsCorrect = oCardPayments.StandardPayment3DSStep2(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                       iQuantityToRechargeBSRedsys,
                                                                       sCurrencyIsoCodeNum,
                                                                       strMerchantGroup,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_HASH, strUserReference,
                                                                       strReturnURL, strBSRedsysProtocolVersion, strBSRedsys3DSTransID, "Y",
                                                                       out eResult, out sErrorMessage,
                                                                       out strTransactionId,
                                                                       out strGatewayDate,
                                                                       out  strInlineForm,
                                                                       out  strMD,
                                                                       out strPaReq,
                                                                       out strCreq);



                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {
                                        decimal? dTransId = null;
                                        bPayIsCorrect = false;

                                        infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strInlineForm, utcNow, out dTransId);


                                        string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID +
                                            dTransId.ToString() +
                                            strBSRedsys3DSTransID +
                                            oUser.USR_EMAIL +
                                            utcNow.ToString("HHmmssddMMyy") +
                                            oUser.USR_CULTURE_LANG;

                                        string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_HASH_SEED);


                                        str3DSURL = string.Format("{0}/BSRedsys3DSRequest?Guid={1}&id={2}&threeDSTransId={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                            strBaseURL,
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_GUID,
                                            dTransId.ToString(),
                                            HttpUtility.UrlEncode(strBSRedsys3DSTransID),
                                            HttpUtility.UrlEncode(oUser.USR_EMAIL),
                                            utcNow.ToString("HHmmssddMMyy"),
                                            HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                            strCalcHash
                                            );

                                        str3DSURL = XmlEscape(str3DSURL);

                                        return ResultType.Result_3DS_Validation_Needed;

                                    }

                                    bBSRedsys3DSFrictionless = true;



                                }
                                else
                                {

                                    //((!string.IsNullOrEmpty(strBSRedsys3DSPares) || !string.IsNullOrEmpty(strBSRedsys3DSPares))&& (!string.IsNullOrEmpty(strBSRedsys3DSTransID)))

                                    infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strBSRedsys3DSPares, strBSRedsys3DSCres, utcNow, out strUserReference, out strBSRedsysProtocolVersion, out iBSRedsysNumInlineForms);

                                    string strProtocolBackup = strBSRedsysProtocolVersion;

                                    strMD = (string.IsNullOrEmpty(strBSRedsys3DSPares) ? "" : strBSRedsys3DSTransID);

                                    bPayIsCorrect = oCardPayments.StandardPayment3DSStep3(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                       oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                       iQuantityToRechargeBSRedsys,
                                                                       sCurrencyIsoCodeNum,
                                                                       strMerchantGroup,
                                                                       oUserPaymentMean.CUSPM_TOKEN_CARD_HASH, strUserReference,
                                                                       strMD, strBSRedsys3DSPares, strBSRedsys3DSCres, ref strBSRedsysProtocolVersion,
                                                                       out eResult, out sErrorMessage,
                                                                       out strTransactionId,
                                                                       out strGatewayDate);

                                    if (strProtocolBackup != strBSRedsysProtocolVersion)
                                    {
                                        infraestructureRepository.UpdateBSRedsys3DSTransaction(strBSRedsys3DSTransID, oUser.USR_EMAIL, strBSRedsysProtocolVersion, utcNow);
                                    }

                                    strMD = "";
                                    bBSRedsys3DSFrictionless = false;

                                }

                            }

                            else
                            {

                                bPayIsCorrect = oCardPayments.StandardPaymentNO3DS(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                                          oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                                          oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                          iQuantityToRechargeBSRedsys,
                                                                          sCurrencyIsoCodeNum,
                                                                          strMerchantGroup,
                                                                          oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                                                          out eResult, out sErrorMessage,
                                                                          out strTransactionId,
                                                                          out strUserReference,
                                                                          out strGatewayDate);
                            }

                         

                            if (bPayIsCorrect)
                            {                                
                                strAuthCode = "";
                                strAuthResult = "succeeded";
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;

                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                strCardScheme = oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA;
                                customersRepository.StartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                          oUser.USR_EMAIL,
                                                                          dtUTCNow,
                                                                          dtNow,
                                                                          iQuantityToRecharge,
                                                                          dCurrencyId,
                                                                          "",
                                                                          strUserReference,
                                                                          strTransactionId,
                                                                          "",
                                                                          strGatewayDate,
                                                                          strAuthCode,
                                                                          PaymentMeanRechargeStatus.Committed);

                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                PaymentMeanCreditCardProviderType.pmccpPaysafe)
                        {
                            string sErrorMessage = "";

                            int iQuantityToRechargePaysafe = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));

                            var oCardPayments = new PaysafePayments();
                            var oPaysafeConfig = new PaysafePayments.PaysafeMerchantInfo(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ACCOUNT_NUMBER,
                                                                                         oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_KEY,
                                                                                         oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_SECRET,
                                                                                         oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ENVIRONMENT);

                            DateTime? dtPaysafeDateTime = null;
                            string strPAN = "";
                            string strExpirationDateMonth = "";
                            string strExpirationDateYear = "";

                            bPayIsCorrect = oCardPayments.Authorize(oPaysafeConfig, oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                                                    iQuantityToRechargePaysafe, oUserPaymentMean.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                    out strTransactionId, out strUserReference, out dtPaysafeDateTime, out strExpirationDateYear, out strExpirationDateMonth, out strPAN, out sErrorMessage);

                            if (bPayIsCorrect)
                            {
                                strAuthCode = "";
                                strAuthResult = "succeeded";
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                if (dtPaysafeDateTime.HasValue)
                                    strGatewayDate = dtPaysafeDateTime.Value.ToString("HHmmssddMMyy");
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                            PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                        {
                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();
                            string errorMessage = "";
                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            //string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurrencyIsoCode), provider);
                            //
                            //

                            bool bAllowAuthorizationAndCapture = MercadoPagoPayments.AllowAuthorizationAndCapture(oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                                                                  oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE);
                            bool bAllowTransactionWithoutCVV = MercadoPagoPayments.AllowTransactionWithoutCVV(oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                                                              oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE);

                            Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Card Schema={0}  Card Type={1} bAllowAuthorizationAndCapture={2} bAllowTransactionWithoutCVV={3}",
                                oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE,
                                bAllowAuthorizationAndCapture,
                                bAllowTransactionWithoutCVV), LogLevels.logINFO);


                            if (!bPaymentInPerson && !bAllowTransactionWithoutCVV)
                            {
                                rtRes = ResultType.Result_Error_Recharge_Not_Possible;
                                Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0} bPaymentInPerson={1} bAllowTransactionWithoutCVV={2}",
                                    rtRes.ToString(), bPaymentInPerson, bAllowTransactionWithoutCVV), LogLevels.logERROR);
                            }
                            else if (bAllowTransactionWithoutCVV)
                            {

                                strUserReference = MercadoPagoPayments.UserReference();

                                bPayIsCorrect = cardPayment.AutomaticTransaction(
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_API_URL,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                    oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_SERVICE_TIMEOUT,
                                    strUserReference,
                                    dQuantityToCharge,
                                    "",
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                    oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                    Convert.ToInt32(oUserPaymentMean.CUSPM_TOKEN_INSTALLMENTS),
                                    true,
                                    out eErrorCode,
                                    out errorMessage,
                                    out strTransactionId,
                                    out strGatewayDate);



                            }
                            else // if (!bAllowTransactionWithoutCVV)
                            {
                                if (!string.IsNullOrEmpty(strMercadoPagoToken))
                                {
                                    strUserReference = MercadoPagoPayments.UserReference();

                                    bPayIsCorrect = cardPayment.AutomaticTransaction(
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                            strUserReference,
                                            dQuantityToCharge,
                                            "",
                                            strMercadoPagoToken,
                                            oUserPaymentMean.CUSPM_TOKEN_CARD_HASH,
                                            Convert.ToInt32(oUserPaymentMean.CUSPM_TOKEN_INSTALLMENTS),
                                            true,
                                            false,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strTransactionId,
                                            out strGatewayDate);

                                }
                                else
                                {
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strCVVURL = strBaseURL + "/MercadoPagoCVVRequest";

                                    DateTime utcNow = DateTime.UtcNow;

                                    string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID +
                                                                                    oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE +
                                                                                    oUserPaymentMean.CUSPM_CVV_LENGTH.ToString() +
                                                                                    utcNow.ToString("HHmmssddMMyy") +
                                                                                    oUser.USR_CULTURE_LANG;

                                    string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_HASH_SEED);


                                    str3DSURL = string.Format("{0}?Guid={1}&cardId={2}&cvvLength={3}&UTCDate={4}&Culture={5}&Hash={6}",
                                        strCVVURL,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID,
                                        oUserPaymentMean.CUSPM_TOKEN_CARD_REFERENCE,
                                        oUserPaymentMean.CUSPM_CVV_LENGTH.ToString(),
                                        utcNow.ToString("HHmmssddMMyy"),
                                        HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                        strCalcHash
                                        );

                                    str3DSURL = XmlEscape(str3DSURL);

                                    return ResultType.Result_3DS_Validation_Needed;



                                }
                            }


                            if (bPayIsCorrect)
                            {
                                bPayIsCorrect = !MercadoPagoPayments.IsError(eErrorCode);
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                strCardScheme = oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA;
                                customersRepository.StartRecharge(oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                          oUser.USR_EMAIL,
                                                                          dtUTCNow,
                                                                          dtNow,
                                                                          iQuantityToRecharge,
                                                                          dCurrencyId,
                                                                          "",
                                                                          strUserReference,
                                                                          strTransactionId,
                                                                          "",
                                                                          strGatewayDate,
                                                                          "",
                                                                          PaymentMeanRechargeStatus.Committed);
                            }
                            else
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Payment_Failed;
                            }


                        }
                        else if ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                PaymentMeanCreditCardProviderType.pmccpMercadoPagoPro)
                        {

                            if (!bPaymentInPerson)
                            {
                                rtRes = ResultType.Result_Error_Recharge_Not_Possible;
                                Logger_AddLogMessage(string.Format("PerformPrepayRecharge::Error: Result = {0} bPaymentInPerson={1}",
                                    rtRes.ToString(), bPaymentInPerson), LogLevels.logERROR);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(strMPProTransactionId))
                                {
                                    strTransactionId = strMPProTransactionId;
                                    strUserReference = strMPProReference;
                                    strGatewayDate = strMPProGatewayDate;
                                    //strMPProCardType;
                                    //strMPProDocumentType;
                                    //strMPProInstallaments;
                                    //strMPProCVVLength;
                                    strCardHash = strMPProCardHash;
                                    strCardReference = strMPProCardReference;
                                    strCardScheme = strMPProCardScheme;
                                    strMaskedCardNumber = strMPProMaskedCardNumber;
                                    strCardDocumentID = strMPProDocumentID;

                                    if ((strMPProExpMonth.Length > 0) && (strMPProExpYear.Length == 4))
                                    {
                                        dtExpirationDate = new DateTime(Convert.ToInt32(strMPProExpYear), Convert.ToInt32(strMPProExpMonth), 1).AddMonths(1).AddSeconds(-1);
                                    }
                                    bPayIsCorrect = true;

                                }
                                else
                                {
                                    string strFormURL = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;

                                    DateTime utcNow = DateTime.UtcNow;
                                    int iQuantityToRechargeMPPro = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));

                                    string strHashString = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID +
                                                oUser.USR_EMAIL +
                                                iQuantityToRechargeMPPro.ToString() +
                                                sCurrencyIsoCode +
                                                strMPProDescription +
                                                utcNow.ToString("HHmmssddMMyy") +
                                                oUser.USR_CULTURE_LANG;

                                    string strCalcHash = CalculatePaymentGatewayHash(strHashString, oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_HASH_SEED);


                                    str3DSURL = string.Format("{0}?Guid={1}&Email={2}&Amount={3}&CurrencyISOCODE={4}&Description={5}&UTCDate={6}&Culture={7}&Hash={8}",
                                        strFormURL,
                                        oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_GUID,
                                        oUser.USR_EMAIL,
                                        iQuantityToRechargeMPPro.ToString(),
                                        sCurrencyIsoCode,
                                        HttpUtility.UrlEncode(strMPProDescription),
                                        utcNow.ToString("HHmmssddMMyy"),
                                        HttpUtility.UrlEncode(oUser.USR_CULTURE_LANG),
                                        strCalcHash
                                        );

                                    str3DSURL = XmlEscape(str3DSURL);

                                    return ResultType.Result_3DS_Validation_Needed;
                                }
                            }


                            if (bPayIsCorrect)
                            {
                                rechargeStatus = PaymentMeanRechargeStatus.Committed;
                            }


                        }





                        if (bPayIsCorrect || bSaveIfFail)
                        {


                            if (!customersRepository.RechargeUserBalance(ref oUser,
                                            oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                            iOSType,
                                            (!bSaveIfFail || rechargeStatus == PaymentMeanRechargeStatus.Committed),
                                            iQuantityToRecharge,
                                            iQuantityToRecharge,
                                            dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                //Convert.ToInt32(dQuantityToCharge * 100),
                                            dCurrencyId,
                                            PaymentSuscryptionType.pstPerTransaction,
                                            rechargeStatus,
                                            PaymentMeanRechargeCreationType.pmrctRegularRecharge,
                                //0,
                                            strUserReference,
                                            strTransactionId,
                                            strCFTransactionID,
                                            strGatewayDate,
                                            strAuthCode,
                                            strAuthResult,
                                            strAuthResultDesc,
                                            strCardHash,
                                            strCardReference,
                                            strCardScheme,
                                            strMaskedCardNumber,
                                            strCardName,
                                            strCardDocumentID,
                                            dtExpirationDate,
                                            null,
                                            null,
                                            null,
                                            false,
                                            dLatitude,
                                            dLongitude,
                                            strAppVersion,strMD,strCAVV,strECI,
                                            strBSRedsys3DSTransID,
                                            strBSRedsysProtocolVersion,
                                            iBSRedsysNumInlineForms,
                                            bBSRedsys3DSFrictionless,
                                            dSourceApp,
                                            infraestructureRepository,
                                            out dRechargeId))
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                            }
                            else
                            {
                                rtRes = ResultType.Result_OK;
                            }

                        }
                        else
                        {
                            rtRes = ResultType.Result_Error_Recharge_Failed;
                            Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                        }

                    }
                    /*else if (((PaymentMeanType)oUserPaymentMean.CUSPM_PAT_ID == PaymentMeanType.pmtPaypal) &&
                        (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1))
                    {
                        PayPal.Services.Private.AP.PayResponse PResponse = null;

                        if (!PaypalPayments.PreapprovalPayRequest(oUserPaymentMean.CUSPM_TOKEN_PAYPAL_ID,
                                                                oUserPaymentMean.CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY,
                                                                dQuantityToCharge,
                                                                sCurrencyIsoCode,
                                                                "en-US",
                                                                "http://localhost",
                                                                "http://localhost",
                                                                out PResponse))
                        {
                            rtRes = ResultType.Result_Error_Recharge_Failed;
                            Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                        }
                        else
                        {
                            if (PResponse.paymentExecStatus != "COMPLETED")
                            {
                                rtRes = ResultType.Result_Error_Recharge_Failed;
                                Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                            }
                            else
                            {
                                PayPal.Services.Private.AP.PaymentDetailsResponse PDResponse = null;

                                if (PaypalPayments.PreapprovalPayConfirm(PResponse.payKey,
                                                                            "en-US",
                                                                            out PDResponse))
                                {



                                    if (!customersRepository.RechargeUserBalance(ref oUser,
                                                                                oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                                                iOSType,
                                                                                false,
                                                                                iQuantity,
                                                                                dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,
                                        //Convert.ToInt32(dQuantityToCharge * 100),
                                                                                dCurrencyId,
                                                                                PaymentSuscryptionType.pstPerTransaction,
                                                                                PaymentMeanRechargeStatus.Committed,
                                                                                PaymentMeanRechargeCreationType.pmrctRegularRecharge,
                                        //0,
                                                                                null,
                                                                                PDResponse.paymentInfoList[0].transactionId,
                                                                                null,
                                                                                DateTime.Now.ToUniversalTime().ToString(),
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                PResponse.payKey,
                                                                                false,
                                                                                dLatitude,
                                                                                dLongitude,
                                                                                strAppVersion,
                                                                                infraestructureRepository,
                                                                                out dRechargeId))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                                    }
                                    else
                                    {
                                        rtRes = ResultType.Result_OK;
                                    }

                                }
                                else
                                {
                                    rtRes = ResultType.Result_Error_Recharge_Failed;
                                    Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);

                                }
                            }
                        }
                    }*/
                    else
                    {
                        rtRes = ResultType.Result_Error_Recharge_Not_Possible;
                        Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);
                    }
                }
                else
                {
                    rtRes = ResultType.Result_Error_Invalid_Payment_Mean;
                    Logger_AddLogMessage(string.Format("PerformPerTransactionRecharge::Error: Result = {0}", rtRes.ToString()), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "PerformPerTransactionRecharge::Exception", LogLevels.logERROR);

            }

            lEllapsedTime = watch.ElapsedMilliseconds;
            watch.Stop();

            return rtRes;

        }


        private ResultType RefundChargeOffstreetPayment(ref USER oUser, decimal dOperationID, decimal? dRechargeID, bool bRestoreBalance)
        {
            ResultType rtRes = ResultType.Result_OK;


            try
            {

                if (!customersRepository.RefundChargeOffstreetPayment(ref oUser,
                                                                bRestoreBalance,
                                                                dOperationID))
                {

                    Logger_AddLogMessage(string.Format("RefundChargeOffstreetPayment::Error Refunding Offstreet Payment {0} ", dOperationID), LogLevels.logERROR);
                    return ResultType.Result_Error_Generic;
                }


                if (dRechargeID != null)
                {
                    if (!customersRepository.RefundRecharge(ref oUser,
                                                            dRechargeID.Value,
                                                            bRestoreBalance))
                    {

                        Logger_AddLogMessage(string.Format("RefundChargeOffstreetPayment::Error Refunding Recharge {0} ", dRechargeID.Value), LogLevels.logERROR);
                        return ResultType.Result_Error_Generic;
                    }
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "RefundChargeOffstreetPayment::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }

        private bool SendEmail(ref USER oUser, string strEmailSubject, string strEmailBody, decimal dSourceApp)
        {
            bool bRes = true;
            try
            {

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


        private string GetEmailFooter(ref USER oUser)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}_{1}", infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)), oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}", infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))));
                }
            }
            catch
            {

            }

            return strFooter;
        }

        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        private static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        private string DecryptCryptResult(string strHexByteArray, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;

                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }


                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));

                }

                strRes = DecryptStringFromBytes_Aes(StringToByteArray(strHexByteArray), _normKey, _iv);



            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);

            }


            return strRes;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        protected string CalculatePaymentGatewayHash(string strInput, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;
                HMACSHA256 _hmacsha256 = null;
                int iKeyLength = 64;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER_PAYMENT_GATEWAY) % (Byte.MaxValue + 1));

                }

                _hmacsha256 = new HMACSHA256(_normKey);

                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = null;


                hash = _hmacsha256.ComputeHash(inputBytes);


                if (hash.Length >= 8)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = hash.Length - 8; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strRes = sb.ToString();
                }


            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculatePaymentGatewayHash::Exception", LogLevels.logERROR);

            }


            return strRes;
        }


        private string XmlEscape(string sXml)
        {
            if (!string.IsNullOrEmpty(sXml))
            {
                return sXml.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
            }
            else
            {
                return "";
            }
        }


        private string GetEmailSourceAppEmailPrefix(decimal dSourceApp)
        {
            string strRes = "";

            decimal dDefaultSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();
            if (dSourceApp != dDefaultSourceApp)
            {
                strRes = geograficAndTariffsRepository.GetSourceAppCode(dSourceApp) + "_";
            }

            return strRes;
        }
        #endregion

    }
}