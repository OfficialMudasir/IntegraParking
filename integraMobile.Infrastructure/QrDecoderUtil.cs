using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace integraMobile.Infrastructure.QrDecoder
{



    public class QrTollData 
    {
        public decimal InsId { get; set; }
        public string Plate { get; set; }
        public DateTime OpeDateUTC { get; set; }
        public decimal UsrId { get; set; }
        public int Balance { get; set; }
        public decimal BalanceCudId { get; set; }
        public DateTime BalanceDate { get; set; }
        public int BalanceAvg { get; set; }
        public int TollPaymentMode { get; set; }
        public bool PaymentStatus { get; set; }
        public int PaymentBalDue { get; set; }
        public string IMEI { get; set; }
        public string WIFIMAC { get; set; }
        public int OSID { get; set; }
        public decimal BlockingId { get; set; }
        public int ExpireQRSeconds { get; set; }

    }

    public class QrDecoderUtil
    {
        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;


        public static bool QRDecode(string sCode, out QrTollData oQrData) 
        {
            bool bRet = false;

            oQrData = new QrTollData();

            try
            {
                

                sCode = sCode.Substring(sCode.Length - 1, 1) + sCode.Substring(0, sCode.Length - 1);

                string sHash = "";
                string sCodeFields = "";

                for (int i = 0; i < 146/*sCode.Length*/; i += 9)
                {
                    if (i < 146 /*sCode.Length*/)
                    {
                        if (i + 8 <= 146 /*sCode.Length*/)
                            sCodeFields += sCode.Substring(i, 8);
                        else
                            sCodeFields += sCode.Substring(i);
                        if (i+9 <= 146 /*sCode.Length*/)
                            sHash += sCode.Substring(i + 8, 1);
                    }
                }

                InitializeStatic();
                string sHashCalculated = CalculateHash(sCodeFields);

                bRet = (sHash == sHashCalculated);

                if (sCodeFields.Length >= 10)
                {
                    oQrData.InsId = Convert.ToDecimal(sCodeFields.Substring(0, 10));
                    sCodeFields = sCodeFields.Substring(10);
                }
                if (sCodeFields.Length >= 15)
                {
                    oQrData.Plate = sCodeFields.Substring(0, 15).Trim();
                    sCodeFields = sCodeFields.Substring(15);
                }
                if (sCodeFields.Length >= 13)
                {
                    oQrData.OpeDateUTC = ParseDateTime(sCodeFields.Substring(0, 13));
                    sCodeFields = sCodeFields.Substring(13);
                }
                if (sCodeFields.Length >= 15)
                {
                    oQrData.UsrId = Convert.ToDecimal(sCodeFields.Substring(0, 15));
                    sCodeFields = sCodeFields.Substring(15);
                }
                if (sCodeFields.Length >= 10)
                {
                    oQrData.Balance = ParseInt32(sCodeFields.Substring(0, 10));
                    sCodeFields = sCodeFields.Substring(10);
                }
                if (sCodeFields.Length >= 5)
                {
                    oQrData.BalanceCudId = Convert.ToDecimal(sCodeFields.Substring(0, 5));
                    sCodeFields = sCodeFields.Substring(5);
                }
                if (sCodeFields.Length >= 13)
                {
                    oQrData.BalanceDate = ParseDateTime(sCodeFields.Substring(0, 13));
                    sCodeFields = sCodeFields.Substring(13);
                }
                if (sCodeFields.Length >= 10)
                {
                    oQrData.BalanceAvg = ParseInt32(sCodeFields.Substring(0, 10));
                    sCodeFields = sCodeFields.Substring(10);
                }
                if (sCodeFields.Length >= 1)
                {
                    oQrData.TollPaymentMode = Convert.ToInt32(sCodeFields.Substring(0, 1));
                    sCodeFields = sCodeFields.Substring(1);
                }
                if (sCodeFields.Length >= 1)
                {
                    oQrData.PaymentStatus = (sCodeFields.Substring(0, 1) == "1");
                    sCodeFields = sCodeFields.Substring(1);
                }
                if (sCodeFields.Length >= 10)
                {
                    oQrData.PaymentBalDue = ParseInt32(sCodeFields.Substring(0, 10));
                    sCodeFields = sCodeFields.Substring(10);
                }
                if (sCodeFields.Length >= 15)
                {
                    oQrData.IMEI = sCodeFields.Substring(0, 15);
                    sCodeFields = sCodeFields.Substring(15);
                }
                if (sCodeFields.Length >= 12)
                {
                    oQrData.WIFIMAC = sCodeFields.Substring(0, 12);
                    sCodeFields = sCodeFields.Substring(12);
                }
                if (sCodeFields.Length >= 1)
                {
                    oQrData.OSID = Convert.ToInt32(sCodeFields.Substring(0, 1));
                    sCodeFields = sCodeFields.Substring(1);
                }
                if (sCodeFields.Length >= 18)
                {
                    oQrData.BlockingId = Convert.ToDecimal(sCodeFields.Substring(0, 18));
                    sCodeFields = sCodeFields.Substring(18);
                }
                if (sCodeFields.Length >= 5)
                {
                    oQrData.ExpireQRSeconds = ParseInt32(sCodeFields.Substring(0, 5));
                    sCodeFields = sCodeFields.Substring(5);
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                bRet = false;
            }

            return bRet;
        }

        /*private String getQRString()
        {
            StringBuilder builder = new StringBuilder();

            //Installation ID
            builder.append(TextUtils.getStringLeftPadded(userAccount.getCityId(), "0", 10));

            //Operation plate
            builder.append(TextUtils.getStringLeftPadded(mSelectedPlate, " ", 15));

            //Operation Date
            builder.append(DateTimeUtils.getPlainCurrentDate());

            //System user Unique ID
            builder.append(TextUtils.getStringLeftPadded(userAccount.getUserUniqueId(), "0", 15));

            //User balance
            builder.append(TextUtils.getStringLeftPadded(userAccount.getBalance(), "0", 10));

            //User currency
            builder.append(TextUtils.getStringLeftPadded(userAccount.getBalanceCurrency(), "0", 5));

            //User balance timestamp
            builder.append(userAccount.getBalanceTimeStamp());

            //User balance average
            builder.append(TextUtils.getStringLeftPadded(userAccount.getBalanceAverage(), "0", 10));

            //Payment mode allowed
            builder.append(String.valueOf(userAccount.getPaymentMode()));

            //Is a payment due
            builder.append(String.valueOf(userAccount.getPaymentStatus()));

            //Amount of the payment due
            builder.append(TextUtils.getStringLeftPadded(userAccount.getPaymentBalanceDue(), "0", 10));

            //Device IMEI
            builder.append(IntegraApp.getImei());

            //MAC Address
            builder.append(IntegraApp.getWifiMac().replace(":", ""));

            //OSID: Android
            builder.append(String.valueOf("1"));

            //Balance blocking ID
            //TODO: the id must be retrieved from API method 47
            String blockingId = "0";
            builder.append(TextUtils.getStringLeftPadded(blockingId, "0", 18));

            //Unused space
            builder.append(TextUtils.getStringLeftPadded(0, "0", 92));

            return builder.toString();
        }

    }

public class TextUtils {

    public static String getStringSatisfyingPlateFormat(String string) {

        String resultString;

        if (string != null && string.length() > 0) {
            String strippedUppercaseString = string.replaceAll("[^A-Za-z0-9]", "").toUpperCase();

            if (strippedUppercaseString.length() >= C.MAX_PLATE_LENGTH) {
                strippedUppercaseString = strippedUppercaseString.substring(0, C.MAX_PLATE_LENGTH);
            }

            resultString = strippedUppercaseString;

        } else {
            resultString = "";
        }
        return resultString;
    }

    public static String getStringLeftPadded(int originalValue, String padChar, int paddingLen) {
        return getStringLeftPadded(String.valueOf(originalValue), padChar, paddingLen);
    }

    public static String getStringLeftPadded(String originalValue, String padChar, int paddingLen) {
        StringBuilder sb = new StringBuilder();

        int padLen = paddingLen - originalValue.length();

        if (padLen > 0) {
            for (int i = padLen; i > 0; i--) {
                sb.append(padChar);
            }
        }

        sb.append(originalValue);

        return  sb.toString();
    }

    public static string getStringLeftPadded(string originalValue, string padChar, int paddingLen) 
    {
        StringBuilder sb = new StringBuilder();

        int originalLen = 0;
        if (originalValue != null) {
            originalLen = originalValue.length();
        }

        int padLen = paddingLen - originalLen;

        if (padLen > 0 && padChar != null) {
            for (int i = padLen; i > 0; i--) {
                sb.append(padChar);
            }
        }

        sb.append(originalValue);

        return  sb.toString();
    }*/

        private static DateTime ParseDateTime(string sDateTime)
        {
            DateTime dtRet = new DateTime(2000, 1, 1, 0, 0, 0, 0);

            if (sDateTime.Length == 13)
            {
                int iSeconds = Convert.ToInt32(sDateTime.Substring(0, 5));
                dtRet = dtRet.AddSeconds(iSeconds);
                int iMili = Convert.ToInt32(sDateTime.Substring(5, 3));
                dtRet = dtRet.AddMilliseconds(iMili);
                int iYear = Convert.ToInt32(sDateTime.Substring(11, 2));
                dtRet = dtRet.AddYears(iYear);
                int iDays = Convert.ToInt32(sDateTime.Substring(8, 3));
                dtRet = dtRet.AddDays(iDays-1);
            }
            else if (sDateTime.Length == 10)
            {
                int iSeconds = Convert.ToInt32(sDateTime.Substring(0, 5));
                dtRet = dtRet.AddSeconds(iSeconds);
                int iYear = Convert.ToInt32(sDateTime.Substring(8, 2));
                dtRet = dtRet.AddYears(iYear);
                int iDays = Convert.ToInt32(sDateTime.Substring(5, 3));
                dtRet = dtRet.AddDays(iDays-1);
            }
            else
            {
                throw new ArgumentException();
            }

            return dtRet;
        }

        private static int ParseInt32(string sInt32)
        {
            string sNum = "";
            for (int i=0; i<sInt32.Length; i++)
            {
                if (sInt32.Substring(i, 1) != "0" && sInt32.Substring(i, 1) != " ")
                {
                    sNum = sInt32.Substring(i);
                    break;
                }
            }

            if (string.IsNullOrEmpty(sNum))
                sNum = "0";

            return Convert.ToInt32(sNum);
        }

        private static void InitializeStatic()
        {

            int iKeyLength = 64;

            if (_hMacKey == null)
            {
                //_hMacKey = "3b6#7!:~s3CZ:>K%$>_5>3NN";
                _hMacKey = "jR]qrB)N2VH4¿_eYu,sQhX]p";
            }

            if (_normKey == null)
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(_hMacKey);
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
            }

            if (_hmacsha256 == null)
            {
                _hmacsha256 = new HMACSHA256(_normKey);
            }

        }

        private static string CalculateHash(string strInput)
        {
            string strRes = "";
            try
            {

                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = _hmacsha256.ComputeHash(inputBytes);

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
                Console.Write(e.Message);
            }

            return strRes;
        }


}

}
