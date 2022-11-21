using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using IntegraTollUtilities;
using Capufe.iParkMe;


namespace Capufe
{
    public partial class Form1 : Form
    {

        private const long BIG_PRIME_NUMBER = 2147483647;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (QRText.Text.Length > 0)
            {
                QRDecoder oQRDecoder = new QRDecoder();
                QRDecoder.VerifyQRResult eResult = oQRDecoder.VerifyQR(QRText.Text, 5000, "MXN", DateTime.UtcNow.ToString("HHmmssfffddMMyy"));

                MessageBox.Show(eResult.ToString());

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            long lEllapsedTime = -1;
            Stopwatch watch = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((servicesender, certificate, chain, sslPolicyErrors) => true);

                
                integraExternalServices oWS = new integraExternalServices();                                
                oWS.Timeout = 20000;
                string strHashKey = "O3AG4otTY6K9S0akV14x6FWN";

                oWS.Url = "https://ws.iparkme.com/Preprod/integramobileWS/integraExternalServices.asmx";
                oWS.Credentials = new System.Net.NetworkCredential("integraMobilePreProd", "%2~p~C%A0Z");

                string strvers = "1.0";
                string strMessage = "";
                string strAuthHash = "";


                int iOpeSucceed = 1;
                string sQr = QRText.Text;
                DateTime dtTollDate = DateTime.Now;
                int iTollId = 1;
                string sTollDescription = "Toll description";
                int iTollRailId = 1;
                string sTollRailDescription = "Desc of the toll rail where the car passed";
                string sTollTariffDescription = "Tariff description";
                int iTollQTotal = 2000;
                string sTollCur = "MXN";


                strAuthHash = CalculateStandardWSHash(strHashKey,
                    string.Format("{0}{1}{2:HHmmssddMMyy}{3}{4}{5}{6}{7}{8}{9}{10}",
                    iOpeSucceed, sQr, dtTollDate, iTollId, sTollDescription, iTollRailId, sTollRailDescription, sTollTariffDescription, iTollQTotal, sTollCur, strvers));

                strMessage = string.Format("<ipark_in>" +
	                                            "<ope_succeed>{0}</ope_succeed>" +
	                                            "<opeqr>{1}</opeqr>" +
                                                "<tolldate>{2:HHmmssddMMyy}</tolldate>" +
	                                            "<tollid>{3}</tollid>" +
	                                            "<tolliddesc>{4}</tolliddesc>" +
	                                            "<tollrailid>{5}</tollrailid>" +
	                                            "<tollrailiddesc>{6}</tollrailiddesc>" +
	                                            "<tolltardesc>{7}</tolltardesc>" +
	                                            "<tollqtotal>{8}</tollqtotal>" +
	                                            "<tollcur>{9}</tollcur>" +
	                                            "<vers>{10}</vers>" +
                                                "<ah>{11}</ah>" +
                                            "</ipark_in>",
                                            iOpeSucceed, sQr, dtTollDate, iTollId, sTollDescription, iTollRailId, sTollRailDescription, sTollTariffDescription, iTollQTotal, sTollCur, strvers, strAuthHash);


                watch = Stopwatch.StartNew();
                string strOut = oWS.NotifyTollPayment(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                MessageBox.Show(strOut);


            }
            catch (Exception ex)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }                
            }


        }

        protected string CalculateStandardWSHash(string strMACKey, string strInput)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strMACKey);
                normMACKey = new byte[iKeyLength];
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
                    normMACKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                oMACsha256 = new HMACSHA256(normMACKey);


                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = oMACsha256.ComputeHash(inputBytes); ;

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
                
            }

            return strRes;
        }

    }
}
