using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.WS.WebAPI.Services.Interfaces;
using integraMobile.WS.WebAPI.Actions.Meypar;
using integraMobile.WS.WebAPI.Meypar;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace integraMobile.WS.WebAPI.Services
{
    public class MeyparService : IMeyparService
    {
        #region Private Attributes

        protected CLogWrapper m_oLog = new CLogWrapper(typeof(MeyparService));
        
        private readonly ICustomersRepository m_oCustomersRepository;
        private readonly IInfraestructureRepository m_oInfraestructureRepository;
        private readonly IGeograficAndTariffsRepository m_oGeograficAndTariffsRepository;
        private readonly IBackOfficeRepository m_oBackOfficeRepository;

        private readonly integraMobile.ExternalWS.ThirdPartyOffstreet m_oThirdPartyOffstreet;

        #endregion

        #region Constructor

        public MeyparService(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, IGeograficAndTariffsRepository oGeograficAndTariffsRepository, IBackOfficeRepository oBackOfficeRepository,
                             integraMobile.ExternalWS.ThirdPartyOffstreet oThirdPartyOffstreet)
        {
            m_oCustomersRepository = oCustomersRepository;
            m_oInfraestructureRepository = oInfraestructureRepository;
            m_oGeograficAndTariffsRepository = oGeograficAndTariffsRepository;
            m_oBackOfficeRepository = oBackOfficeRepository;

            m_oThirdPartyOffstreet = oThirdPartyOffstreet;
        }

        #endregion

        #region Public Methods

        public bool SupportValidation(string sParkingId, int iTerminalId, TerminalDeviceTypeEnum eDeviceType, string sPlate, DateTime dtOperation, DateTime dtOperationUtc, out ResultTypeEnum eResult, out string sExternalId)
        {
            bool bRet = true;

            eResult = ResultTypeEnum.Success;
            sExternalId = null;

            try
            {
                // verificar zona
                GROUPS_OFFSTREET_WS_CONFIGURATION oParkingConfiguration = null;
                DateTime? dtgroupDateTime = null;
                if (!m_oGeograficAndTariffsRepository.getOffStreetConfigurationByExtOpsId(sParkingId, iTerminalId, ref oParkingConfiguration, ref dtgroupDateTime))
                {
                    bRet = false;
                    eResult = ResultTypeEnum.ZoneNotExist;
                }

                GROUP oGroup = null;
                DateTime? dtinstDateTime = null;
                if (bRet)
                {                    
                    if (!m_oGeograficAndTariffsRepository.getGroup(oParkingConfiguration.GROUP.GRP_ID, ref oGroup, ref dtinstDateTime))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.ZoneNotExist;
                    }
                    if (oGroup.GRP_TYPE != (int)GroupType.OffStreet)
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.ZoneNotExist;
                    }
                }

                // Verificar matrícula
                if (bRet)
                {
                    if (!m_oInfraestructureRepository.ExistPlateInSystem(sPlate))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.PlateNotExist;
                    }
                }

                USER oUser = null;
                if (bRet)
                {
                    IEnumerable<USER> oUsersList = null;
                    if (!m_oCustomersRepository.GetUsersWithPlate(sPlate, out oUsersList))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.GenericError;
                    }
                    else if (oUsersList.Count() > 0)
                    {
                        if (oUsersList.Count() == 1)
                        {
                            oUser = oUsersList.First();
                        }
                        else
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.PlateWithMultipleUsers;
                        }
                    }
                    else
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.PlateNotExist;
                    }
                }


                // Verificar deute usuari si és entrada



                // Verificar la última operació
                // Get last offstreet operation with the same group id and logical id (<g> and <ope_id>)
                OPERATIONS_OFFSTREET oLastParkOp = null;
                if (bRet)
                {
                    if (!m_oCustomersRepository.GetLastOperationOffstreetData(oGroup.GRP_ID, oUser.USR_ID, sPlate, out oLastParkOp))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.GenericError;
                    }                    
                }

                if (bRet)
                {

                    if (eDeviceType == TerminalDeviceTypeEnum.ESP_Entry)
                    {

                        CUSTOMER_PAYMENT_MEAN oUserPaymentMean = m_oCustomersRepository.GetUserPaymentMean(ref oUser, oGroup.INSTALLATION);
                        bRet = ((oUserPaymentMean != null) &&
                                (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                                (oUserPaymentMean.CUSPM_VALID == 1));

                        bRet = (bRet || (oUser.USR_BALANCE > 0));
                        if (!bRet)
                        {
                            eResult = ResultTypeEnum.OperationNotAllowed_InvalidPaymentMethod;
                        }

                        if (bRet)
                        {
                            int iTotalDebt = 0;
                            bRet = m_oCustomersRepository.GetUserTotalDebt(ref oUser, out iTotalDebt);
                            bRet &= (iTotalDebt == 0);
                            if (!bRet)
                                eResult = ResultTypeEnum.OperationNotAllowed_UserWithDebt;
                        }

                        if (bRet)
                        {
                            // Check if entry operation already exists
                            bool bEntryExist = (oLastParkOp != null && oLastParkOp.OPEOFF_TYPE == (int)OffstreetOperationType.Entry);

                            if (!bEntryExist)
                            {
                                int iCurrencyChargedQuantity = 0;
                                decimal? dRechargeId;
                                bool bRestoreBalanceInCaseOfRefund = true;
                                int? iBalanceAfterRecharge = null;

                                integraMobile.WS.integraCommonService oCommonService = new integraMobile.WS.integraCommonService(m_oCustomersRepository, m_oInfraestructureRepository, m_oGeograficAndTariffsRepository);

                                bool bPaymentInPerson = false;
                                string str3DSURL = null;

                                decimal dSourceApp = m_oGeograficAndTariffsRepository.GetDefaultSourceApp();
                                long lEllapsedTime = 0;

                                var parametersOut = new SortedList();
                                decimal dOperationID = -1;

                                integraMobile.ExternalWS.ResultType rtIntegraMobileWS =
                                    oCommonService.ChargeOffstreetOperation(OffstreetOperationType.Entry, sPlate, 1.0, 0, 0,
                                                                              dtOperation, dtOperation, null, null, null,
                                                                              oParkingConfiguration, oGroup, "", "", iTerminalId.ToString(), "", false,
                                                                              ref oUser, (int)MobileOS.None, null, null, null, "",
                                                                              0, 0, 0, 0, 0,
                                                                              0, 0, 0, 0,
                                                                              null, "", "", "", "", "", "", "", "",
                                                                              "", "", "", "", "", "", "", "", "", "", "", "", "", "",dSourceApp, bPaymentInPerson, null,
                                                                              ref parametersOut, out iCurrencyChargedQuantity, out dOperationID,
                                                                              out dRechargeId, out iBalanceAfterRecharge, out bRestoreBalanceInCaseOfRefund, out str3DSURL, out lEllapsedTime);

                                sExternalId = dOperationID.ToString();

                                eResult = Convert_integraMobileExternalWSResultType_TO_ResultTypeEnum(rtIntegraMobileWS);
                                bRet = (eResult == ResultTypeEnum.Success);
                            }
                            else
                            {
                                if (oLastParkOp != null) sExternalId = oLastParkOp.OPEOFF_ID.ToString();

                                bRet = false;
                                eResult = ResultTypeEnum.OperationEntryAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        if (oLastParkOp != null && oLastParkOp.OPEOFF_TYPE != (int)OffstreetOperationType.Entry)
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.OperationAlreadyClosed;
                        }
                        else if (oLastParkOp == null)
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.EntryOperationNotExist;
                        }
                        else 
                        {
                            sExternalId = oLastParkOp.OPEOFF_ID.ToString();
                        }
                    }
                    

                }

            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "SupportValidation::Exception", ex);
                bRet = false;
                eResult = ResultTypeEnum.GenericError;
            }

            return bRet;
        }

        public bool StandardNotification(string sParkingId, int iTerminalId, decimal dOperationID, TerminalDeviceTypeEnum eDeviceType, string sPlate, DateTime dtOperation, DateTime dtOperationUtc, int iAmount, DateTime? dtEntry, DateTime? dtEntryUtc, DateTime? dtStayEnd, DateTime? dtStayEndUtc, string sTicketId, string sTicketNumber, out ResultTypeEnum eResult)
        {
            bool bRet = true;

            eResult = ResultTypeEnum.Success;

            try
            {
                // verificar zona
                GROUPS_OFFSTREET_WS_CONFIGURATION oParkingConfiguration = null;
                DateTime? dtgroupDateTime = null;
                if (!m_oGeograficAndTariffsRepository.getOffStreetConfigurationByExtOpsId(sParkingId, iTerminalId, ref oParkingConfiguration, ref dtgroupDateTime))
                {
                    bRet = false;
                    eResult = ResultTypeEnum.ZoneNotExist;
                }

                GROUP oGroup = null;
                DateTime? dtinstDateTime = null;
                if (bRet)
                {
                    if (!m_oGeograficAndTariffsRepository.getGroup(oParkingConfiguration.GROUP.GRP_ID, ref oGroup, ref dtinstDateTime))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.ZoneNotExist;
                    }
                    if (oGroup.GRP_TYPE != (int)GroupType.OffStreet)
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.ZoneNotExist;
                    }
                }

                // Check if entry operation exists
                OPERATIONS_OFFSTREET oLastParkOp = null;
                if (bRet)
                {
                    if (!m_oCustomersRepository.GetOperationOffstreetById(dOperationID, out oLastParkOp))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.GenericError;
                    }
                    else if (oLastParkOp == null)
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.EntryOperationNotExist;
                    }
                    else if (oLastParkOp.USER_PLATE.USRP_PLATE != sPlate)
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.InvalidPlate;
                    }
                    else if (oLastParkOp.OPEOFF_TYPE != (int)OffstreetOperationType.Entry) // verificar que oLastParkOp és una entrada
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.InvalidEntryOperation;
                    }
                }
                // Verificar matrícula
                if (bRet)
                {
                    if (!m_oInfraestructureRepository.ExistPlateInSystem(sPlate))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.PlateNotExist;
                    }
                }

                USER oUser = null;
                if (bRet)
                {
                    IEnumerable<USER> oUsersList = null;
                    if (!m_oCustomersRepository.GetUsersWithPlate(sPlate, out oUsersList))
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.GenericError;
                    }
                    else if (oUsersList.Count() > 0)
                    {
                        if (oUsersList.Count() == 1)
                        {
                            oUser = oUsersList.First();
                        }
                        else
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.PlateWithMultipleUsers;
                        }
                    }
                    else
                    {
                        bRet = false;
                        eResult = ResultTypeEnum.PlateNotExist;
                    }
                }

                if (bRet)
                {
                    if (eDeviceType == TerminalDeviceTypeEnum.ESP_Entry)
                    {
                        // Actualitzar dades de oLastParkOp
                        if (!m_oCustomersRepository.UpdateOperationOffstreetEntryData(oLastParkOp.OPEOFF_ID, dtOperation, dtOperationUtc, sTicketId, sTicketNumber, false, out oLastParkOp))
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.GenericError;
                        }

                    }
                    else
                    {

                        // Verificar que no hi ha cap operació de sortida fent referència a aquesta entrada
                        OPERATIONS_OFFSTREET oExitParkOp = null;
                        if (!m_oCustomersRepository.GetOperationOffstreetByEntryOpeId(oLastParkOp.OPEOFF_ID, out oExitParkOp))
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.GenericError;
                        }
                        else if (oExitParkOp != null)
                        {
                            bRet = false;
                            eResult = ResultTypeEnum.OperationAlreadyClosed;
                        }


                        if (bRet)
                        {
                            integraMobile.WS.integraCommonService oCommonService = new integraMobile.WS.integraCommonService(m_oCustomersRepository, m_oInfraestructureRepository, m_oGeograficAndTariffsRepository);

                            // Fer el pagament
                            double dChangeToApply = 1.0;

                            if (oParkingConfiguration.GROUP.INSTALLATION.CURRENCy.CUR_ISO_CODE != m_oInfraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
                            {
                                dChangeToApply = oCommonService.GetChangeToApplyFromInstallationCurToUserCur(oParkingConfiguration.GROUP.INSTALLATION, oUser);
                                if (dChangeToApply < 0)
                                {
                                    bRet = false;
                                    eResult = ResultTypeEnum.GenericError;
                                }
                            }


                            if (bRet)
                            {
                                decimal dPercVAT1;
                                decimal dPercVAT2;
                                decimal dPercFEE;
                                decimal dPercFEETopped;
                                decimal dFixedFEE;
                                int iPartialVAT1;
                                int iPartialPercFEE;
                                int iPartialFixedFEE;
                                int iPartialPercFEEVAT;
                                int iPartialFixedFEEVAT;
                                int iTotalQuantity;
                                string sDiscounts = "";

                                int? iPaymentTypeId = null;
                                int? iPaymentSubtypeId = null;
                                if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                                {
                                    iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                                    iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                                }
                                if (!m_oCustomersRepository.GetFinantialParams(oUser, oGroup, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId,
                                                                               out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                                {
                                    bRet = false;
                                    eResult = ResultTypeEnum.GenericError;
                                    //Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error getting installation FEE parameters: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                }

                                if (bRet)
                                {
                                    iTotalQuantity = m_oCustomersRepository.CalculateFEE(iAmount, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE,
                                                                                                                  out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                                                                  out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                                    int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                                    int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                                    bool bPaymentInPerson = false;
                                    string str3DSURL = null;

                                    decimal dSourceApp = m_oGeograficAndTariffsRepository.GetDefaultSourceApp();

                                    var parametersOut = new SortedList();

                                    integraMobile.ExternalWS.ResultType rtIntegraMobileWS =
                                    oCommonService.ConfirmCarPayment(oParkingConfiguration, oGroup, oUser,
                                                                      sTicketId, OffstreetOperationIdType.MeyparId,
                                                                      sPlate, "", iTerminalId.ToString(),
                                                                      OffstreetOperationType.Exit, iAmount, 0, dChangeToApply, oLastParkOp.CURRENCy.CUR_ISO_CODE,
                                                                      (dtEntry ?? oLastParkOp.OPEOFF_ENTRY_DATE), dtgroupDateTime.Value, dtOperation, (dtStayEnd ?? dtOperation),
                                                                      (int)MobileOS.None, null, null, null, "",
                                                                      dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE,
                                                                      iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iTotalQuantity,
                                                                      sDiscounts, "", "", "", "", "", "", "", "",
                                                                      "", "", "", "", "", "", "", "", "", "", "", "", "", "",dSourceApp, bPaymentInPerson,
                                                                      oLastParkOp, this.m_oThirdPartyOffstreet,
                                                                      ref parametersOut, out str3DSURL);

                                    eResult = Convert_integraMobileExternalWSResultType_TO_ResultTypeEnum(rtIntegraMobileWS);
                                    bRet = (eResult == ResultTypeEnum.Success);

                                }
                            }

                            if (bRet)
                            {
                                // Insertar operació de sortida

                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "SupportValidation::Exception", ex);
                bRet = false;
                eResult = ResultTypeEnum.GenericError;
            }

            return bRet;
        }

        #endregion

        private ResultTypeEnum Convert_integraMobileExternalWSResultType_TO_ResultTypeEnum(integraMobile.ExternalWS.ResultType oExtResultType)
        {
            ResultTypeEnum rtResultType = ResultTypeEnum.GenericError;

            switch (oExtResultType)
            {
                case integraMobile.ExternalWS.ResultType.Result_OK:
                    rtResultType = ResultTypeEnum.Success;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_InvalidAuthenticationHash:
                    rtResultType = ResultTypeEnum.InvalidAuthenticationHash;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Invalid_Input_Parameter:
                    rtResultType = ResultTypeEnum.InvalidInputParameter;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Missing_Input_Parameter:
                    rtResultType = ResultTypeEnum.MissingInputParameter;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Generic:
                    rtResultType = ResultTypeEnum.GenericError;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_OperationAlreadyClosed:
                    rtResultType = ResultTypeEnum.OperationAlreadyClosed;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_OperationEntryAlreadyExists:
                    rtResultType = ResultTypeEnum.OperationEntryAlreadyExists;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Recharge_Failed:
                    rtResultType = ResultTypeEnum.RechargeFailed;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Recharge_Not_Possible:
                    rtResultType = ResultTypeEnum.RechargeNotPossible;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Invalid_Payment_Mean:
                    rtResultType = ResultTypeEnum.InvalidPaymentMean;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Not_Enough_Balance:
                    rtResultType = ResultTypeEnum.NotEnoughBalance;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Toll_is_Not_from_That_installation:
                    rtResultType = ResultTypeEnum.GenericError;
                    break;

                default:
                    break;
            }
            return rtResultType;
        }
    }
}