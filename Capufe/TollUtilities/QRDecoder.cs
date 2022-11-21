using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace IntegraTollUtilities
{


    public class QRDecoder
    {
        public enum VerifyQRResult
        {
            VQRR_AuthorizedPass =1,
            VQRR_InvalidQRFormat = 0,
            VQRR_NotEnoughBalance = -1,
            VQRR_QRIsTooOld = -2,
            VQRR_UserWithPendingPayments = -3,
            VQRR_InvalidUserPaymentMean = -4,
            VQRR_OnlineMandatory = -5,
            VQRR_InvalidDateFormat = -6
        }
        
        /*
      * input parameters:
                [1] string sReadQr – string with the QR received from the scanner
                [2] long lTollCharge – amount of cents to be charged: 5000 – for 50.00 MXN
                [3] long lTollCurrency – currency related to the lTollCharge (Currency ISO 4217 CODE (MXN for Mexican Peso))
                [4] string sValDateUTC – current UTC date in O.S of the computer in the following format hh24missfffddMMYY, for example: 133422003060116 stands for
                13:34 and 22 seconds and 003 miliseconds of January 6th of 2015
        output parameters:
	            [1]   1 :car can go on / paso autorizado
                      0 :formato de QR incorrecto
	                 -1 :saldo insuficiente
                     -2 :QR obsoleto / el usuario debe obtener un nuevo QR
                     -3 :Pagos pendientes / usuario no autorizado
                     -4 :Medio de pago no válido / el usuario debe introducir un medio de pago válido.       
                     -5 :QR generado en modo offline. Modo online requerido.
                     -6 :Invalid input utc date format.
      * 
      */
        public VerifyQRResult VerifyQR(string sReadQr, long lTollCharge, string lTollCurrency, string sValDateUTC)
        {
            VerifyQRResult oRet = VerifyQRResult.VQRR_AuthorizedPass;

            integraMobile.Infrastructure.QrDecoder.QrTollData oTollData = null;
            if (integraMobile.Infrastructure.QrDecoder.QrDecoderUtil.QRDecode(sReadQr, out oTollData))
            {
                DateTime? dtValDateUTC = null;
                try
                {
                    dtValDateUTC = DateTime.ParseExact(sValDateUTC, "HHmmssfffddMMyy", CultureInfo.InvariantCulture);
                }
                catch (Exception ex) { }

                if (dtValDateUTC.HasValue)
                {
                    if (Math.Abs((oTollData.OpeDateUTC - dtValDateUTC.Value).TotalSeconds) <= oTollData.ExpireQRSeconds)
                    //if (oTollData.OpeDateUTC.AddSeconds(oTollData.ExpireQRSeconds) <= dtValDateUTC)
                    {
                        if (oTollData.BlockingId <= 0) // Offline
                        {
                            if (oTollData.TollPaymentMode == 0)
                                oRet = VerifyQRResult.VQRR_OnlineMandatory;
                            else if (oTollData.TollPaymentMode == 1)
                                oRet = VerifyQRResult.VQRR_AuthorizedPass;
                            else if (oTollData.TollPaymentMode == 2)
                                oRet = (oTollData.Balance >= lTollCharge ? VerifyQRResult.VQRR_AuthorizedPass : VerifyQRResult.VQRR_NotEnoughBalance);
                            else if (oTollData.TollPaymentMode == 3)
                                oRet = (oTollData.BalanceAvg >= lTollCharge ? VerifyQRResult.VQRR_AuthorizedPass : VerifyQRResult.VQRR_NotEnoughBalance);
                        }
                        else // Online
                            oRet = VerifyQRResult.VQRR_AuthorizedPass;
                    }
                    else
                        oRet = VerifyQRResult.VQRR_QRIsTooOld;
                }
                else
                    oRet = VerifyQRResult.VQRR_InvalidDateFormat;
            }
            else
                oRet = VerifyQRResult.VQRR_InvalidQRFormat;
                
            return oRet;
        }

    }
}
