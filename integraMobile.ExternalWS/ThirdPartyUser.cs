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
using System.Dynamic;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain.Abstract;
using Ninject;
using Newtonsoft.Json;

namespace integraMobile.ExternalWS
{
   
    public class ThirdPartyUser : ThirdPartyBase
    {
        
        public ThirdPartyUser() : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyUser));
        }


        public ResultType ZendeskUserReplication(ref List<stUserReplicationResult> oUsersReps, ref Dictionary<string, object> oUsersDataDict, 
                                                string strURL, string strUsername, string strPassword, int iQueueBeforeReplication)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;
            
           

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {

                AddTLS12Support();

                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(strUsername))
                {
                    request.Credentials = new System.Net.NetworkCredential(strUsername, strPassword);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = Get3rdPartyWSTimeout();

                //var json = new JavaScriptSerializer().Serialize(oUsersDataDict);
                var json = JsonConvert.SerializeObject(oUsersDataDict);


                Logger_AddLogMessage(string.Format("ZendeskUserReplication request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                string strJobId = "";
                string strJobUrl = "";
                string strJobStatus = "";

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

                        Logger_AddLogMessage(string.Format("ZendeskUserReplication response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var oJobStatus = oResponse["job_status"];
                        strJobId = oJobStatus["id"];
                        strJobUrl = oJobStatus["url"];
                        strJobStatus = oJobStatus["status"];

                        Logger_AddLogMessage(string.Format("ZendeskUserReplication id={0}; job url={1}", strJobId, strJobUrl), LogLevels.logINFO);
                        if (strJobStatus == "queued")
                        {
                            rtRes = ResultType.Result_OK;
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "ZendeskUserReplication::Exception", LogLevels.logERROR);
                }

                stUserReplicationResult oUserRep;

                List<decimal> oLstUserReplications = oUsersReps.Select(r => r.m_dRepId).ToList();
                int i = 0;
                foreach (decimal dRepId in oLstUserReplications)
                {
                    oUserRep = oUsersReps.Where(r => r.m_dRepId == dRepId).FirstOrDefault();

                    oUsersReps.Remove(oUserRep);

                    if (rtRes == ResultType.Result_OK)
                    {
                        oUserRep.m_eUserReplicationStatus = UserReplicationStatus.Queued;
                        oUserRep.m_strJobId = strJobId;
                        oUserRep.m_strJobURL = strJobUrl;
                        oUserRep.m_iQueueBeforeReplication = iQueueBeforeReplication - i;
                        oUserRep.m_dReplicationTime = (decimal)watch.ElapsedMilliseconds;
                    }
                    else
                    {
                        oUserRep.m_eUserReplicationStatus = UserReplicationStatus.Error;
                        if (oUserRep.m_iCurrRetries.HasValue)
                            oUserRep.m_iCurrRetries++;
                        else
                            oUserRep.m_iCurrRetries = 1;
                    }

                    i++;
                    oUserRep.m_dtStatusDate = DateTime.UtcNow;
                    oUsersReps.Add(oUserRep);
                }


            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ZendeskUserReplication::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType ZendeskUpdateQueuedReplications(ref List<stUserReplicationResult> oUsersReps, string strUsername, string strPassword)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                AddTLS12Support();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                stUserReplicationResult oFirstUserRep = oUsersReps.FirstOrDefault();

                WebRequest request = WebRequest.Create(oFirstUserRep.m_strJobURL);
                if (!string.IsNullOrEmpty(strUsername))
                {
                    request.Credentials = new System.Net.NetworkCredential(strUsername, strPassword);
                }

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = Get3rdPartyWSTimeout();

                Logger_AddLogMessage(string.Format("ZendeskUpdateQueuedReplications request to url {0}", oFirstUserRep.m_strJobURL), LogLevels.logINFO);

                request.ContentLength = 0;

                stUserReplicationResult oUserRep;

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

                        Logger_AddLogMessage(string.Format("ZendeskUpdateQueuedReplications response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var oJobStatus = oResponse["job_status"];
                        string strJobStatus = oJobStatus["status"];

                        Logger_AddLogMessage(string.Format("ZendeskUpdateQueuedReplications url={0}; status={1}", oFirstUserRep.m_strJobURL,strJobStatus), LogLevels.logINFO);
                       
                        if (strJobStatus == "completed")
                        {
                            rtRes = ResultType.Result_OK;
                            var oResults = oJobStatus["results"];

                            int i = 1;
                            foreach (var oResult in oResults)
                            {
                                oUserRep = oUsersReps.Where(r => r.m_iInJobOrder==i).FirstOrDefault();

                                string strRepStatus = oResult["status"];
                                oUsersReps.Remove(oUserRep);
                                oUserRep.m_dtStatusDate = DateTime.UtcNow;
                                if ((strRepStatus == "Updated") || (strRepStatus == "Created"))
                                {
                                    oUserRep.m_strExternalReplicationId = oResult["id"];
                                    oUserRep.m_eUserReplicationStatus = UserReplicationStatus.Completed;
                                }
                                else
                                {
                                    oUserRep.m_strReplicationError = oResult["errors"];
                                    oUserRep.m_eUserReplicationStatus = UserReplicationStatus.Error;
                                    if (oUserRep.m_iCurrRetries.HasValue)
                                        oUserRep.m_iCurrRetries++;
                                    else
                                        oUserRep.m_iCurrRetries = 1;

                                }
                                oUsersReps.Add(oUserRep);

                                i++;



                            }

                        }
                        else
                        {

                            List<decimal> oLstUserReplications = oUsersReps.Select(r => r.m_dRepId).ToList();
                            foreach (decimal dRepId in oLstUserReplications)
                            {
                                oUserRep = oUsersReps.Where(r => r.m_dRepId == dRepId).FirstOrDefault();

                               
                            }


                        }

                        reader.Close();
                        dataStream.Close();
                    }
                    else
                    {
                        List<decimal> oLstUserReplications = oUsersReps.Select(r => r.m_dRepId).ToList();
                        foreach (decimal dRepId in oLstUserReplications)
                        {
                            oUserRep = oUsersReps.Where(r => r.m_dRepId == dRepId).FirstOrDefault();

                            oUsersReps.Remove(oUserRep);
                            oUserRep.m_dtStatusDate = DateTime.UtcNow;
                            oUserRep.m_eUserReplicationStatus = UserReplicationStatus.Error;
                            if (oUserRep.m_iCurrRetries.HasValue)
                                oUserRep.m_iCurrRetries++;
                            else
                                oUserRep.m_iCurrRetries = 1;

                            oUsersReps.Add(oUserRep);
                        }
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    List<decimal> oLstUserReplications = oUsersReps.Select(r => r.m_dRepId).ToList();
                    foreach (decimal dRepId in oLstUserReplications)
                    {
                        oUserRep = oUsersReps.Where(r => r.m_dRepId == dRepId).FirstOrDefault();

                        oUsersReps.Remove(oUserRep);
                        oUserRep.m_dtStatusDate = DateTime.UtcNow;
                        oUserRep.m_eUserReplicationStatus = UserReplicationStatus.Error;
                        if (oUserRep.m_iCurrRetries.HasValue)
                            oUserRep.m_iCurrRetries++;
                        else
                            oUserRep.m_iCurrRetries = 1;

                        oUsersReps.Add(oUserRep);
                    }

                    Logger_AddLogException(e, "ZendeskUpdateQueuedReplications::Exception", LogLevels.logERROR);

                }


            }
            catch (Exception e)
            {
            
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ZendeskUpdateQueuedReplications::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }



        public ResultType AparcAppGetBalance(string strURL, string strEmail, out int iBalance, out decimal? dCurId, out decimal? dCouId)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            iBalance = 0;
            dCurId = null;
            dCouId = null;

            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime dtServerTime = DateTime.Now;
                DateTime dtSpainDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                string strDate = dtSpainDateTime.ToString("ddMMyy");

                Random rndGen = new Random(dtSpainDateTime.Millisecond * dtSpainDateTime.Second * dtSpainDateTime.Minute) ;
                string strRand = rndGen.Next(100000000, 999999999).ToString();

                char[] chRand = strRand.ToCharArray();

                chRand[0] = strDate[1];
                chRand[3] = strDate[3];
                chRand[6] = strDate[5];

                //infraestructureRepository.Get
                string strURLParams = string.Format("?mail={0}&ti={1}", Uri.EscapeUriString(strEmail), new string(chRand));
                WebRequest request = WebRequest.Create(strURL+strURLParams);
                

                request.Method = "GET";
                request.ContentType = " application/json";
                request.Timeout = Get3rdPartyWSTimeout();

                Logger_AddLogMessage(string.Format("AparcAppGetBalance request to url {0} ; Email {1}", strURL + strURLParams, strEmail), LogLevels.logINFO);

                request.ContentLength = 0;              

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

                        Logger_AddLogMessage(string.Format("AparcAppGetBalance response.xml={0}", PrettyXml(responseFromServer)), LogLevels.logINFO);
                        XElement xmlTree = XElement.Parse(responseFromServer);
                        string strRes = xmlTree.Value;

                        if (!string.IsNullOrEmpty(strRes))
                        {
                            try
                            {
                                decimal d = Decimal.Parse(strRes.Replace(",", "."), CultureInfo.InvariantCulture);
                                iBalance = (int)(d * 100);

                                dCouId=(decimal ?)infraestructureRepository.GetCountryIdFromCountryCode("ES");
                                dCurId = (decimal ?)infraestructureRepository.GetCountryCurrency((int)dCouId.Value);

                                rtRes = ResultType.Result_OK;
                            }
                            catch
                            {

                            }

                        }                       
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "AparcAppGetBalance::Exception", LogLevels.logERROR);
                }


            }
            catch (Exception e)
            {

                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "AparcAppGetBalance::Exception", LogLevels.logERROR);
            }


            return rtRes;

        }


        public ResultType AparcAppDropUser(string strURL, string strEmail, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;

            lEllapsedTime = -1;
            Stopwatch watch = null;


           

            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime dtServerTime = DateTime.Now;
                DateTime dtSpainDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                string strDate = dtSpainDateTime.ToString("ddMMyy");

                Random rndGen = new Random(dtSpainDateTime.Millisecond * dtSpainDateTime.Second * dtSpainDateTime.Minute);
                string strRand = rndGen.Next(100000000, 999999999).ToString();

                char[] chRand = strRand.ToCharArray();

                chRand[0] = strDate[1];
                chRand[3] = strDate[3];
                chRand[6] = strDate[5];

                //infraestructureRepository.Get
                string strURLParams = string.Format("?mail={0}&ti={1}", Uri.EscapeUriString(strEmail), new string(chRand));
                WebRequest request = WebRequest.Create(strURL + strURLParams);


                request.Method = "GET";
                request.ContentType = " application/json";
                request.Timeout = Get3rdPartyWSTimeout();

                Logger_AddLogMessage(string.Format("AparcAppDropUser request to url {0} ; Email {1}", strURL + strURLParams, strEmail), LogLevels.logINFO);

                request.ContentLength = 0;
                watch = Stopwatch.StartNew();


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

                        lEllapsedTime = watch.ElapsedMilliseconds;

                        // Display the content.

                        Logger_AddLogMessage(string.Format("AparcAppDropUser response.xml={0}", PrettyXml(responseFromServer)), LogLevels.logINFO);
                        XElement xmlTree = XElement.Parse(responseFromServer);
                        string strRes = xmlTree.Value;

                        if (!string.IsNullOrEmpty(strRes))
                        {
                            if (strRes == "RESPONSE_OK")
                            {
                                rtRes = ResultType.Result_OK;
                            }

                        }
                        else
                        {
                            //NULL ya no existe, por tanto ha ido bien antes.
                            rtRes = ResultType.Result_OK;
                        }
                    }
                    else
                    {
                        lEllapsedTime = watch.ElapsedMilliseconds;
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "AparcAppDropUser::Exception", LogLevels.logERROR);
                }


            }
            catch (Exception e)
            {
                lEllapsedTime = watch.ElapsedMilliseconds;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "AparcAppDropUser::Exception", LogLevels.logERROR);
            }

            return rtRes;

        }

        public static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }
    }
}
