using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace integraMobile.Domain.Abstract
{
    public interface IFineRepository
    {
        CURRENCy GetCurrencyByIsoCode(string sIsoCode);
        OPERATOR GetOperator(int iId, integraMobileDBEntitiesDataContext dbContext = null);
        PAYMENT_TYPE GetPaymentType(int iId);

        bool UpdateFine(ref TICKET_PAYMENTS_NON_USER oNewTicketPaymentNonUser,
                                   int iInstallation, string sTicketNumber, string sEmail,
                                   PaymentMeanCreditCardProviderType eProviderType,
                                   string sOpReference,
                                   string sTransactionId,
                                   string sGatewayDate,
                                   string sAuthCode,
                                   string sAuthResult,
                                   string sAuthResultDesc,
                                   string sCardHash,
                                   string sCardReference,
                                   string sCardScheme,
                                   string sMaskedCardNumber,
                                   DateTime? dtCardExpirationDate,
                                   string sPaypalToken,
                                   string sPaypalPayerId,
                                   string sPayPaypalPreapprovedPayKey);
        TICKET_PAYMENTS_NON_USER GetTicketPaymentNonUser(decimal iId);

        bool GetWaitingCommitTicketPaymentNonUserPayment(out TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment, int iConfirmWaitTime,
                                            int iNumSecondsToWaitInCaseOfRetry, int iMaxRetries);

        bool CommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment,
                            string strUserReference,
                            string strAuthResult,
                            string strGatewayDate,
                            string strCommitTransactionId);

        bool CommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment);


        bool RetriesForCommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment, int iMaxRetries,
                                            string strUserReference,
                                            string strAuthResult,
                                            string strGatewayDate,
                                            string strCommitTransactionId);

        bool ChargeFinePaymentNonUser(bool bSubstractFromBalance,
                        PaymentSuscryptionType suscriptionType,
                        decimal dInstallationID,
                        DateTime dtTicketPayment,
                        DateTime dtUTCPaymentDate,
                        string strPlate,
                        string strTicketNumber,
                        string strTicketData,
                        int iQuantity,
                        decimal dCurID,
                        decimal dBalanceCurID,
                        double dChangeApplied,
                        double dChangeFee,
                        int iCurrencyChargedQuantity,
                        decimal dPercVat1, decimal dPercVat2, int iPartialVat1, decimal dPercFEE, int iPercFEETopped, int iPartialPercFEE, int iFixedFEE, int iPartialFixedFEE, int iTotalAmount,
                        decimal? dRechargeId,
                        bool bConfirmedInWS,
                        decimal? dLatitude,
                        decimal? dLongitude, string strAppVersion,
                        decimal? dGrpId,
                        decimal? dBackofficeUsrId,
                        decimal? dPaymentType,
                        string sAdditionalinfo,
                        out decimal dTicketPaymentID,
                        out DateTime? dtUTCInsertionDate,
                        string strReference,
                        string strTransactionId,
                        string strCFTransactionId,
                        string strGatewayDate,
                        string strAuthCode,
                        string strAuthResult,
                        string strCardHash,
                        string strCardReference,
                        string strCardScheme,
                        string strMaskedCardNumber,
                        string strPaypal3tPayerId,
                        string strPaypal3tToken,
                        DateTime? dtExpDate,
                        INSTALLATION oInstallation,
                        PaymentMeanCreditCardProviderType providerType,
                        string currencyIsoCode,
                        decimal dGatewayConfigId,
                        string authId);

        bool UpdateThirdPartyIDInFinePaymentNonUser(decimal dTicketPaymentID,
                                             string str3rdPartyOpNum);

        IQueryable<TICKET_PAYMENTS_NON_USER> GetTicketsPaymentNonUser(DateTime dt);
		IQueryable<TICKET_PAYMENTS_NON_USER> GetTicketsPaymentNonUser(decimal idini, decimal idend);

        bool UpdateFine(decimal dFineID,
                                  string sOpReference,
                                  string sTransactionId,
                                  string sGatewayDate,
                                  string sAuthCode,
                                  string sAuthResult);

    }
}
