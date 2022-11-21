using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;

using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Infrastructure
{
    public class CouponCodeGenerator
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CouponCodeGenerator));


        public static string GenerateCode(ref string sKeyCode)
        {
            string sCode = "";

            sCode = CalculateRandom();

            string sHashCode = CalculateHash(sCode);

            //m_Log.LogMessage(LogLevels.logINFO, string.Format("GenerateCode sCode={0} sHashcode={1}", sCode, sHashCode));  

            string sCrc32Code = CalculateCrc32(sHashCode.Substring(0, 8) + sCode + sHashCode.Substring(8, 8));

            string sFinalCode = sHashCode.Substring(8, 8) + sCrc32Code.Substring(0, 4) + sCode + sCrc32Code.Substring(4, 4) + sHashCode.Substring(0, 8);

            sKeyCode = sHashCode.Substring(14, 2) + sCrc32Code.Substring(0, 4) + sCode.Substring(0, 2);

            return sFinalCode;
        }

        #region Generate Random Code

        public static string CalculateRandom()
        {
            string sCode = "";

            int iCodeLength = 10;
            string sValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (ConfigurationManager.AppSettings.AllKeys.Contains("COUPON_CODE_LENGTH"))
                iCodeLength = Int32.Parse(ConfigurationManager.AppSettings["COUPON_CODE_LENGTH"]);
            if (ConfigurationManager.AppSettings.AllKeys.Contains("COUPON_CODE_VALIDCHARS"))
                sValidChars = ConfigurationManager.AppSettings["COUPON_CODE_VALIDCHARS"];

            char[] arrChars = sValidChars.ToCharArray();

            for (int i = 0; i < iCodeLength; i++)
            {
                //Random rnd = new Random(DateTime.Now.Millisecond);
                sCode += arrChars[GetRandomNumber(0, arrChars.Length - 1)];
            }

            return sCode;
        }

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        private static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        #endregion

        #region Generate Hash Code

        private static string _hMacKey = null;
        private static byte[] _normKey = null;
        private static MACTripleDES _mac3des = null;
        private static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;

        private static void InitializeHashStatic()
        {

            int iKeyLength = 24;

            if (_hMacKey == null)
            {
                _hMacKey = ConfigurationManager.AppSettings["COUPON_AuthHashKey"].ToString();
            }


            if (ConfigurationManager.AppSettings["COUPON_AuthHashAlgorithm"].ToString() == "HMACSHA256")
            {
                iKeyLength = 64;
            }
            else if (ConfigurationManager.AppSettings["COUPON_AuthHashAlgorithm"].ToString() == "MACTripleDES")
            {
                iKeyLength = 24;
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

            if (ConfigurationManager.AppSettings["COUPON_AuthHashAlgorithm"].ToString() == "HMACSHA256")
            {
                if (_hmacsha256 == null)
                {
                    _hmacsha256 = new HMACSHA256(_normKey);
                }
            }
            else if (ConfigurationManager.AppSettings["COUPON_AuthHashAlgorithm"].ToString() == "MACTripleDES")
            {
                if (_mac3des == null)
                {
                    _mac3des = new MACTripleDES(_normKey);
                }
            }

        }

        private static string CalculateHash(string strInput)
        {
            string strRes = "";
            try
            {
                if (_hMacKey == null) InitializeHashStatic();

                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = null;

                if (_mac3des != null)
                {
                    hash = _mac3des.ComputeHash(inputBytes);

                }
                else if (_hmacsha256 != null)
                {
                    hash = _hmacsha256.ComputeHash(inputBytes);
                }


                if (hash.Length >= 8)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = hash.Length - 8; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strRes = sb.ToString();
                    if ((strRes.Length % 2) != 0) 
                        strRes = "0" + strRes;
                }                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CalculateHash::Exception", e);                
            }

            if (strRes.Length != 16)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CalculateHash::Error Hash.Length!=16 --> {0}", strRes));  
            }
            return strRes;
        }

        #endregion

        #region Generate CRC32 Code

        private static string CalculateCrc32(string sInput)
        {
            byte[] bInput = System.Text.Encoding.UTF8.GetBytes(sInput); // StringToByteArray(sInput);
            
            uint iCrc = CH.Crc32.Crc.Crc32(bInput);

            string sCrc = iCrc.ToString("X2");
            if ((sCrc.Length % 2) != 0)
                sCrc = "0" + sCrc;

            if (sCrc.Length < 8)
            {
                sCrc = sCrc.PadLeft(8, '0');
            }
            
            return sCrc;                
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        #endregion

    }
}
