using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace integraMobile.Domain.Abstract
{
  
    public enum ChargeOperationsType
    {
        ParkingOperation=1,
        ExtensionOperation=2,
        ParkingRefund=3,
        TicketPayment=4,
        BalanceRecharge=5,
        ServiceCharge=6,
        Discount=7,
        OffstreetEntry = 8,
        OffstreetExit = 9,
        OffstreetOverduePayment = 10,
        BalanceRechargeRefund = 11,
        CouponCharge = 12,
        SubscriptionCharge = 13,
        BalanceTransfer = 14,
        BalanceReception = 15,
        TollPayment = 16,
        TollLock = 17,
        TollUnlock = 18,
        Permit = 19
    }

    public enum NotificationEventType
    {
        TicketInsertion=1,
        BeforeEndParking=2,
        ParkingInsertion=3,
        Info = 4,
        OffstreetParkingEntry = 5,
        OffstreetParkingExit = 6,
        Issue = 7,
        PasswordRecovery = 8,
        UserWarning = 9
    }

    public enum UserNotificationStatus
    {
        Inserted = 10,
        Generated = 20,
        Sending = 30,
        Finished_Partially = 40,
        Finished_Completely = 50
    }

    public enum UserWarningStatus
    {
        Inserted = 10,
        Send = 20,
        CreatedUserNotification=30,
        ObsoleteVersion=40,
        NoPushId = 50,
    }

    public enum UserWarningType
    {
        Push = 0,
        Login = 1,
        Park = 2,
        UnPark = 3,
        Fine = 4
    }

    public enum PushIdNotificationStatus
    {
        Inserted = 10,
        Sending = 20,
        Sent = 30,
        Waiting_Next_Retry = 40,
        Failed = 50,
        SubcriptionExpired = 60

    }

    public enum PlateMovSendingStatus
    {
        Inserted = 10,
        Sending = 20,
        Sent = 30,
        Waiting_Next_Retry = 40
    }

    public enum PlateSendingWSSignatureType
    {
        psst_test = 0,
        psst_internal = 1,
        psst_standard = 2,
        psst_eysa = 3
    }

    public enum MobileOS
    {
        None = 0,
        Android = 1,
        WindowsPhone = 2,
        iOS = 3,
        Blackberry = 4,
        Web = 5
    }

    public enum OperationSourceType
    {
        InternMobilePayment = 1,
        ExternalParkingMeter = 2,
        ExternalMobilePayment = 3
    }

    public enum GroupType
    {
        OnStreetZone = 0,
        OffStreet = 1,
        OnStreetLot = 2,
    }

    [Serializable]
    public struct FileAttachmentInfo
    {

        public string strName;
        public string strMediaType;
        public byte[] fileContent;
        public string filePath;

    }

    public enum OffstreetOperationIdType
    {
        MeyparId = 1,
        QRId = 2
    }

    public enum OffstreetParkingType
    {
        Ticket = 1,
        Barcode = 10,
        Cameras = 100
    }

    public enum OffstreetOperationType
    {
        Entry = 8,
        Exit = 9,
        OverduePayment = 10
    }

    public enum SignupScreenType
    {
        Iparkme = 1,
        BilboPark = 2
    };

    public enum ParkingMode
    {
        Normal = 0,
        StartStop = 1,
        StartStopHybrid = 2
    }

    public enum ParkingModeStatus
    {
        Opened = 0,
        Closed = 1,
        ClosedAutomatically = 2
    }

    public enum NoticChargesNow
    {
        NotShowMessage = 0,
        MessageBeforeConfirming = 1,
        MessageAfterConfirming = 2
    }

    public class OffstreetParkingOccupation
    {
        public decimal GroupId { get; set; }
        public OffstreetParkingType[] ParkingType { get; set; }
        public float OccupationPerc { get; set; }
        public string Colour { get; set; }
        public string ExternalNum { get; set; }
        public string Description { get; set; }
    }

    public class UserOperations
    {
        public decimal OpeId { get; set; }
        public decimal UsrMainTelCountry { get; set; }
        public string UsrMainTel { get; set; }
        public string GrpDescription { get; set; }
        public string Plate { get; set; }
        public string UsrCultureLang { get; set; }
        public string UsrEmail { get; set; }
        public decimal UsrLastSourceApp { get; set; }
        public int Minute { get; set; }
    }

    public interface IInfraestructureRepository
    {

        List<CURRENCy> Currencies { get; }
        List<COUNTRy> Countries { get; }       
        List<INSTALLATION> Installations { get; }
        List<FINAN_DIST_OPERATORS_INSTALLATION> FinanDistOperatorsInstallation { get; }
        List<CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG> PaymentGateways { get; }
        List<CURRENCY_RECHARGE_VALUE> getCURRENCY_RECHARGE_VALUEs(decimal dCurId, integraMobileDBEntitiesDataContext dbContext = null);

        string GetParameterValue(string strParName, integraMobileDBEntitiesDataContext dbContext = null);
        string GetParameterValueNoCache(string strParName);
        decimal GetVATPerc();
        decimal GetChangeFeePerc();
        string GetCountryTelephonePrefix(int iCountryId);
        int GetTelephonePrefixCountry(string strPrefix);
        COUNTRy GetCountry(int iCountryId);
        int? GetCountryIdFromCountryCode(string strCode);
        string GetCountryName(int iCountryId);
        int GetCountryCurrency(int iCountryId);
        bool GetCountryPossibleSuscriptionTypes(int iCountryId, out string sSuscriptionType, out RefundBalanceType eRefundBalType);
        List<COUNTRIES_REDIRECTION> GetCountriesRedirections();
        COUNTRIES_REDIRECTION GetCountriesRedirectionsByCityId(int iCity_ID);
        COUNTRIES_REDIRECTION GetCountriesRedirectionsByCountryId(decimal iCountry_ID);
        List<COUNTRIES_REDIRECTION> GetCountriesRedirectionsGroupByPICURL();
        string GetCurrencyIsoCode(int iCurrencyId);
        string GetCurrencySymbolOrIsoCode(int iCurrencyId);
        decimal GetCurrencyFromIsoCode(string strISOCode);
        string GetCurrencyIsoCodeNumericFromIsoCode(string strISOCode);
        int GetCurrencyDivisorFromIsoCode(string strISOCode);
        int GetCurrenciesFactorDifference(string strISOCode1, string strISOCode2);
        int GetCurrenciesFactorDifference(int iCurId1, int iCurId2);
        string GetDecimalFormatFromIsoCode(string strISOCode);
        string GetCurSymbolFromIsoCode(string strISOCode);
        long SendEmailTo(string strEmailAddress, string strSubject, string strMessageBody, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal);
        List<long> SendEmailToMultiRecipients(List<string> lstRecipients, string strSubject, string strMessageBody, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal);
        long SendEmailWithAttachmentsTo(string strEmailAddress, string strSubject, string strMessageBody, List<FileAttachmentInfo> lstAttachments, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal);
        List<long> SendEmailWithAttachmentsToMultiRecipients(List<string> lstRecipients, string strSubject, string strMessageBody, List<FileAttachmentInfo> lstAttachments, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal);
        List<long> SendEmailToMultiRecipientsTool(decimal dUniqueId, string strSubject, string strMessageBody, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal);
        long SendSMSTo(int iCountryCode, string strTelephone, string strMessage, decimal? dSourceApp, ref string strCompleteTelephone);        

        //STRIPE

        bool GetStripeConfiguration(string Guid, out STRIPE_CONFIGURATION oStripeConfiguration);

        //IECISA

        bool GetIECISAConfiguration(string Guid, out IECISA_CONFIGURATION oStripeConfiguration);

        //CREDIT CALL

        bool GetCreditCallConfiguration(string Guid, out CREDIT_CALL_CONFIGURATION oCreditCallConfiguration);

        //MONERIS

        bool GetMonerisConfiguration(string Guid, out MONERIS_CONFIGURATION oMonerisConfiguration);
        bool GetMonerisConfigurationById(decimal? id, out MONERIS_CONFIGURATION oMonerisConfiguration);
        bool AddMoneris3DSTransaction(decimal dMonerisConfiguration, string strMD, string strEmail, int iAmount, DateTime utcDate, string strInlineForm, out decimal? dTransId);
        bool UpdateMoneris3DSTransaction(string strMD,string strEmail ,string strCAVV,string strECI, DateTime utcdate);
        bool GetMoneris3DSTransactionInlineForm(decimal dId, string strMD, string strEmail, out string strInlineForm);

        //TRANSBANK

        bool GetTransBankConfiguration(string Guid, out TRANSBANK_CONFIGURATION oTransbankConfiguration);
        bool GetTransBankConfigurationById(decimal? id, out TRANSBANK_CONFIGURATION oTransbankConfiguration);

        //PAYU

        bool GetPayuConfiguration(string Guid, out PAYU_CONFIGURATION oPayuConfiguration);
        bool GetPayuConfigurationById(decimal? id, out PAYU_CONFIGURATION oPayuConfiguration);

        //BS REDSYS

        bool GetBSRedsysConfiguration(string Guid, out BSREDSYS_CONFIGURATION oConfiguration);
        bool GetBSRedsysConfigurationById(decimal? id, out BSREDSYS_CONFIGURATION oConfiguration);
        bool AddBSRedsys3DSTransaction(decimal dBSRedsysConfiguration, string strTransId, string strOrderId, string strEmail, int iAmount, DateTime utcDate, string strInlineForm, string strProtocolVersion, out decimal? dTransId);
        bool UpdateBSRedsys3DSTransaction(string TransId, string strEmail, string strBSRedsys3DSPares, string strBSRedsys3DSCres, DateTime utcdate, out string strOrderId,out string strProtocolVersion, out int? iBSRedsysNumInlineForms);
        bool UpdateBSRedsys3DSTransaction(string TransId, string strEmail, string strInlineForm, DateTime utcdate, out decimal? dTransId);
        bool UpdateBSRedsys3DSTransaction(string TransId, string strEmail, string strProtocolVersion, DateTime utcdate);
        bool GetBSRedsys3DSTransactionInlineForm(decimal dId, string TransId, string strEmail, out string strInlineForm);
        bool GetBSRedsys3DSTransactionEmail(string strTransID, out string strEmail);


        //PAYSAFE

        bool GetPaysafeConfiguration(string Guid, out PAYSAFE_CONFIGURATION oConfiguration);
        bool GetPaysafeConfigurationById(decimal? id, out PAYSAFE_CONFIGURATION oConfiguration);


        //PAYPAL

        bool GetPaypalConfiguration(string Guid, out PAYPAL_CONFIGURATION oPaypalConfiguration);
        bool GetPaypalConfigurationById(decimal? id, out PAYPAL_CONFIGURATION oPaypalConfiguration);


        //Mercadopago

        bool GetMercadoPagoConfiguration(string Guid, out MERCADOPAGO_CONFIGURATION oConfiguration);
        bool GetMercadoPagoConfigurationById(decimal? id, out MERCADOPAGO_CONFIGURATION oConfiguration);


        //NOTIFICATIONS
        bool GetFirstNotGeneratedUserNotification(out USERS_NOTIFICATION notif, out int iQueueLengthUserNotificationMultiplier, List<decimal> oListRunningUserNotificationMultiplier);
        bool GenerateUserNotification(ref USERS_NOTIFICATION notif);
        bool GetFirstNotSentNotification(out PUSHID_NOTIFICATION notif, int iResendTime, out int iQueueLengthPushIdNotificationSender, List<decimal> oListRunningPushIdNotificationSender);
        bool PushIdNotificationSent(decimal dPushNotifID);
        bool PushIdNotificationFailed(decimal dPushNotifID, int iMaxRetries);
        bool PushIdExpired(decimal dPushNotifID, string strNewPushId);

        //ENVIO DE SMS
        bool FilterSMS();
        bool GetListOfUsersToSendSMS(out List<UserOperations> userOperations, int iMaxUserOperationsToReturn, out int iQueueLengthUserOperationsSenderSMS, List<decimal> oListRunningUserOperationsSenderSMS);
        bool MarkSMSOperation(UserOperations userOperation);
        bool GetLiteralsOfTheMessage(out List<LITERAL_LANGUAGE> listLiterlLanguage, int iLiteralIdSMS);

        //USERS WARNINGS
        bool GetListOfUsersWarningsToSend(out List<USERS_WARNING> userWarningList, int iMaxUserWarningsToReturn, out int iQueueLengthUsersWarnings, List<decimal> oListRunningUsersWarnings);
        bool SetUsersWarningsStatus(USERS_WARNING userWarning,decimal notif);

        //PLATE SYNCRO
        bool GeneratePlatesSending();
        IEnumerable<USER_PLATE_MOVS_SENDING> GetPlatesForSending(int iMaxNumPlates);
        bool ErrorSedingPlates(IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList);
        bool ConfirmSentPlates(IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList);


        //EXTERNAL TICKETS AND PARKS SYNCRO
        bool ExistPlateInSystem(string strPlate);
        bool AddExternalPlateFine(decimal dInstallation,
                                  string strPlate,
                                  DateTime dtTicket,
                                  DateTime dtTicketUTC,
                                  string strFineNumber,
                                  int iQuantity,
                                  DateTime dtLimit,
                                  DateTime dtLimitUTC,
                                  string strArticleType,
                                  string strArticleDescription);

        bool AddExternalPlateParking(decimal dInstallation,
                                  string strPlate,
                                  DateTime dtDate,
                                  DateTime dtDateUTC,
                                  DateTime dtEndDate,
                                  DateTime dtEndDateUTC,
                                  decimal? dGroup,
                                  decimal? dTariff,
                                  DateTime? dtIniDate,
                                  DateTime? dtIniDateUTC,
                                  int? iQuantity,
                                  int? iTime,
                                  decimal dExternalProvider,
                                  OperationSourceType operationSourceType,
                                  string strSourceIdent,
                                  ChargeOperationsType chargeType,
                                  string strOperationId1, string strOperationId2,
                                  out decimal dOperationId);

        bool GetInsertionTicketNotificationData(out EXTERNAL_TICKET oTicket, out int iQueueLengthExternalTicket, List<decimal> oListRunningExternalTicket);
        bool GetInsertionUserSecurityDataNotificationData(out USERS_SECURITY_OPERATION oSecurityOperation, out int iQueueLengthoUsersSecurityOperationNotification, List<decimal> oListRunningUsersSecurityOperationNotification);
        bool GetInsertionParkingNotificationData(out EXTERNAL_PARKING_OPERATION oParking, out int iQueueLengthInsertionParkingNotification, List<decimal> oListRunningInsertionParkingNotification);
        bool GetBeforeEndParkingNotificationData(int iNumMinutesBeforeEndToWarn, out EXTERNAL_PARKING_OPERATION oParking, out int iQueueLengthBeforeEndParkingNotification, List<decimal> oListRunningBeforeEndParkingNotification);
        bool GetOffstreetOperationNotificationData(out OPERATIONS_OFFSTREET oOperation, out int iQueueLengthOperationOffStreetNotification, List<decimal> oListRunningOperationOffStreetNotification);

        bool MarkAsGeneratedInsertionTicketNotification(EXTERNAL_TICKET oTicket);
        bool MarkAsGeneratedInsertionParkingNotificationData(EXTERNAL_PARKING_OPERATION oParking);
        bool MarkAsGeneratedBeforeEndParkingNotificationData(EXTERNAL_PARKING_OPERATION oParking);
        bool MarkAsGeneratedOffstreetOperationNotificationData(OPERATIONS_OFFSTREET oOperation);
        bool MarkAsGeneratedUserSecurityDataNotificationData(USERS_SECURITY_OPERATION oSecurityOperation, decimal oNotifID);

        bool getCarrouselVersion(int iVersion, int iLang, decimal dSourceApp, out CARROUSEL_SCREEN_VERSION oCarrouselVersion);

        string GetLiteral(decimal literalId, string langCulture);
        string GetLiteralFromKey(string literalKey, string langCulture);

        bool GetLanguage(decimal dLanId, out LANGUAGE oLanguage);
        decimal GetLanguage(string sLanguage);

        bool IsInternalEmail(string sEmail);

        bool GetTariffFromQueryExtId(decimal dInsId, string sExtId, out TARIFF oTariff);
        bool InsertTariffIfRequired(decimal dInsId, string sExtId, string sDescription, TariffType eType, TariffBehavior eBehavior, out decimal dTarId);

        bool SetSignUpGuidCountriesRedirections(string sSignUpGuid, decimal iCoU_ID);
        SIGNUP_GUID_COUNTRIES_REDIRECTION GetSignUpGuidCountriesRedirections(string sSignUpGuid);

        string GetRestrictedTariffMessage(DateTime date, decimal grpId, string lang);
        string GetCustomErrorMessage(DateTime date, int iErrorCode, decimal grpId, decimal? tarId, string lang);
        string GetCustomErrorMessageForOperation(DateTime date, int iErrorCode, decimal grpId, decimal? tarId, string lang);
        string GetCustomErrorMessage(DateTime date, int iErrorCode, decimal dInstId, string lang);
        bool InsertOrUpdateSessionVariables(string key, string values);
        string GetSessionVariables(string key);

        //SOURCE APPS
        decimal GetDefaultSourceApp();
        List<SOURCE_APPS_CONFIGURATION> GetSourceAppsConfigurations();
        SOURCE_APPS_CONFIGURATION GetSourceAppsConfiguration(decimal dSourceApp);
        string GetSourceAppCode(decimal dSourceApp);

        int GetRateWSTimeout(decimal dInstallation);
        int GetRateWSTimeout();
        int GetRateWSTimeoutIncreaseValue(decimal dInstallation);


        //STREET SECTIONS SYNC

        bool AddStreetSectionPackage(decimal dInstallationID, decimal id, byte[] file);
        bool DeleteOlderStreetSectionPackage(decimal dInstallationID, decimal id);
        bool GetLastStreetSectionPackageId(decimal dInstallationID, out decimal id);
        bool GetLastStreetSectionPackage(decimal dInstallationID, out byte[] file);


    }
}
