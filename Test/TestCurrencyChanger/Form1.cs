using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Windows.Forms;


namespace TestCurrencyChanger
{
    public partial class Form1 : Form
    {

        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;

        public class ChangeRequest
        {
            public string SrcIsoCode { get; set; }
            public string DstIsoCode { get; set; }
            public string UTCTime { get; set; }
            public string AuthHash { get; set; }

        }

        public class ChangeResponse
        {
            public class CChangeData
            {
                public string SrcIsoCode { get; set; }
                public string DstIsoCode { get; set; }
                public double Change { get; set; }
                public DateTime AdquisitionUTCDateTime { get; set; }
            }

            public int Result { get; set; }
            public CChangeData ChangeData { get; set; }


        }

        private enum ResultType
        {
            Result_OK = 1,
            Result_Error_InvalidAuthenticationHash = -1,
            Result_Error_Invalid_UTCTime = -2,
            Result_Error_Getting_Change_From_Provider = -3,
            Result_Error_Generic = -4,
        }

        public Form1()
        {
            InitializeComponent();
            InitializeStatic();
        }

        private void button1_Click(object psender, EventArgs e)
        {


            try
            {

                DateTime dt = DateTime.UtcNow;
                string strUTCTime = dt.ToString("yyyyMMddHHmmssfff");
                string strSrcIsoCode = "USD";
                string strDstIsoCode = "EUR";
                string strHash = CalculateHash(strSrcIsoCode + strDstIsoCode + strUTCTime);


                string strURL = string.Format("http://localhost:9929/currencychange/{0}/{1}/{2}/{3}", strSrcIsoCode, strDstIsoCode, strUTCTime, strHash);

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WebRequest request = WebRequest.Create(strURL);

                request.Method = "GET";
                //request.ContentType = "application/json";
                request.Timeout = 5000;
                request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                

                //var json = JsonConvert.SerializeObject(oUsersDataDict);



                //byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //request.ContentLength = byteArray.Length;
                request.ContentLength = 0;
                // Get the request stream.
                //Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                //dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                //dataStream.Close();

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        ChangeResponse oResponse = (ChangeResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(ChangeResponse));

                        if (((ResultType)oResponse.Result) == ResultType.Result_OK)
                        {
                            MessageBox.Show(string.Format("{0}", oResponse.ChangeData.Change));
                        }
                        else
                        {
                            MessageBox.Show(string.Format("{0}", (ResultType)oResponse.Result));
                        }


                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception ex)
                {
                   
                }

            }
            catch (Exception ex)
            {

            }
           
        }

        private void button2_Click(object psender, EventArgs e)
        {
            try
            {
                string strURL = "http://localhost:9929/currencychange";

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5000;

                DateTime dt = DateTime.UtcNow;
                string strUTCTime = dt.ToString("yyyyMMddHHmmssfff");
                string strSrcIsoCode="USD";
                string strDstIsoCode="EUR";

                ChangeRequest oRequest = new ChangeRequest()
                {
                    SrcIsoCode=strSrcIsoCode,
                    DstIsoCode = strDstIsoCode,
                    UTCTime= strUTCTime,
                    AuthHash = CalculateHash(strSrcIsoCode + strDstIsoCode + strUTCTime),           
                };


                var json = JsonConvert.SerializeObject(oRequest);



                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        ChangeResponse oResponse = (ChangeResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(ChangeResponse));

                        if (((ResultType)oResponse.Result) == ResultType.Result_OK)
                        {
                            MessageBox.Show(string.Format("{0}", oResponse.ChangeData.Change));
                        }
                        else
                        {
                            MessageBox.Show(string.Format("{0}", (ResultType)oResponse.Result));
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {

            }
        }


        protected void InitializeStatic()
        {
            if (_hmacsha256 == null)
            {
                int iKeyLength = 64;

                if (_hMacKey == null)
                {
                    _hMacKey = @"2_)V6RQu\6ZZa9R~L>CQ)z?G";
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


                _hmacsha256 = new HMACSHA256(_normKey);
            }


        }

        protected string CalculateHash(string strInput)
        {
            string strRes = "";
            try
            {

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
                
            }


            return strRes;
        }


    }
}
