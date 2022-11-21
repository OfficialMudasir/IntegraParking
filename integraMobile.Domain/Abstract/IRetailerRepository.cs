using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace integraMobile.Domain.Abstract
{
    public interface IRetailerRepository
    {
        CURRENCy GetCurrencyByIsoCode(string sIsoCode);
        OPERATOR GetOperator(int iId, integraMobileDBEntitiesDataContext dbContext = null);
        PAYMENT_TYPE GetPaymentType(int iId);

        bool UpdateRetailerCoupons(ref RETAILER oNewRetailer,
                                   string sName, string sEmail, string sAddress, string sDocId, int iCoupons, decimal dCouponAmount, 
                                   string sCurrencyIsoCode, decimal dAmount,
                                   decimal dPercVAT1, decimal dPercVAT2, int iPartialVAT1, decimal dPercFEE, int iPercFEETopped, int iPartialPercFEE, int iFixedFEE, int iPartialFixedFEE,
                                   decimal dTotalAmount,
                                   decimal dCurrencyDivisorFromIsoCode,
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
        RETAILER GetRetailer(decimal iId);

        bool GetWaitingCommitRetailerPayment(out RETAILER_PAYMENT oRetailerPayment, int iConfirmWaitTime,
                                            int iNumSecondsToWaitInCaseOfRetry, int iMaxRetries);

        bool CommitTransaction(RETAILER_PAYMENT oRetailerPayment,
                            string strUserReference,
                            string strAuthResult,
                            string strGatewayDate,
                            string strCommitTransactionId);

        bool CommitTransaction(RETAILER_PAYMENT oRetailerPayment);
                   

        bool RetriesForCommitTransaction(RETAILER_PAYMENT oRetailerPayment, int iMaxRetries,
                                            string strUserReference,
                                            string strAuthResult,
                                            string strGatewayDate,
                                            string strCommitTransactionId);

      
       

    }
}
