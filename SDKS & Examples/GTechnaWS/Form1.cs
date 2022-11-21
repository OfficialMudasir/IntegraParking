using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Security.Cryptography;


namespace GTechnaWS
{
    public partial class Form1 : Form
    {

        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;


        public Form1()
        {
            InitializeComponent();
            InitializeStatic();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                StandardThirdPartyFineWS.PayByPhoneSoapImplService oWS = new StandardThirdPartyFineWS.PayByPhoneSoapImplService();
                string strAuthHashCalc = CalculateHash("5017441730002811121.0");
                string strMessage = "<ipark_in><f>501744</f><d>173000281112</d><vers>1.0</vers><ah>" + strAuthHashCalc + "</ah></ipark_in>";
                string strOut = oWS.queryFinePaymentQuantity(strMessage);

                label.Text = strOut;
            }
            catch (Exception exc)
            {
                label.Text = exc.Message;
            }

        }

        private static void InitializeStatic()
        {

            int iKeyLength = 64;

            if (_hMacKey == null)
            {
                _hMacKey =System.Configuration.ConfigurationSettings.AppSettings["AuthHashKey"].ToString();
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



        public string CalculateHash(string strInput)
        {
            string strRes = "";
            try
            {
                if (_hmacsha256 != null)
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

            }
            catch
            {
            }


            return strRes;
        }



    }
}
