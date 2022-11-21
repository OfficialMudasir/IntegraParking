using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using integraMobile.Infrastructure.Logging.Tools;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Web;


namespace WSIparksuiteTest
{

    public partial class FormWSIParksuiteTest : Form
    {
        /*public partial class TariffComputerWS : ITraceable
        {
            private string componentName;
            private bool isTraceRequestEnabled;
            private bool isTraceResponseEnabled;

            public bool IsTraceRequestEnabled
            {
                get { return isTraceRequestEnabled; }
                set { isTraceRequestEnabled = value; }
            }

            public bool IsTraceResponseEnabled
            {
                get { return isTraceResponseEnabled; }
                set { isTraceResponseEnabled = value; }
            }

            public string ComponentName
            {
                get { return componentName; }
                set { componentName = value; }
            }
        }*/

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(FormWSIParksuiteTest));
        private const long BIG_PRIME_NUMBER = 2147483647;
        protected static string _xmlTagName = "ipark";
        protected const string OUT_SUFIX = "_out";


        int m_iFrequency;
        int m_iTimeout;
        bool m_bStop = false;
        Thread m_Thread = null;
        int m_iOK = 0;
        int m_iErrQuery = 0;
        int m_iErrSetParking = 0;
        int m_iCurrID = 0;


        delegate void IncreaseCounterCallBack();
        object m_lock0 = new object();
        object m_lock1 = new object();
        object m_lock2 = new object();
        object m_lock5 = new object();
        object m_lock6 = new object();
        //object m_PlatLocker = new object();

        //protected static PlatformService.AuthSession m_oPlatformServiceAuthSession = null;
        //protected static int m_iPlatformServiceOpenSessions = 0;


        public enum ResultTypeStandardParkingWS
        {
            ResultSP_OK = 1,
            ResultSP_Error_InvalidAuthenticationHash = -1,
            ResultSP_Error_ParkingMaximumTimeUsed = -2,
            ResultSP_Error_NotWaitedReentryTime = -3,
            ResultSP_Error_RefundNotPossible = -4,
            ResultSP_Error_Fine_Number_Not_Found = -5,
            ResultSP_Error_Fine_Type_Not_Payable = -6,
            ResultSP_Error_Fine_Payment_Period_Expired = -7,
            ResultSP_Error_Fine_Number_Already_Paid = -8,
            Result_Error_Generic = -9,
            ResultSP_Error_Invalid_Input_Parameter = -10,
            ResultSP_Error_Missing_Input_Parameter = -11,
            ResultSP_Error_Invalid_City = -12,
            ResultSP_Error_Invalid_Group = -13,
            ResultSP_Error_Invalid_Tariff = -14,
            ResultSP_Error_Tariff_Not_Available = -15,
            ResultSP_Error_InvalidExternalProvider = -16,
            ResultSP_Error_OperationAlreadyExist = -17,
            ResultSP_Error_CrossSourceExtensionNotPossible = -24,
        }

        public FormWSIParksuiteTest()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            m_iOK = 0;
            m_iErrQuery = 0;
            m_iErrSetParking = 0;

            lblOK.Text = m_iOK.ToString();
            lblErrQuery.Text = m_iErrQuery.ToString();
            lblErrSetParking.Text = m_iErrSetParking.ToString();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                m_iFrequency = Convert.ToInt32(txtOpMin.Text);


            }
            catch
            {
                m_iFrequency = -1;
                txtOpMin.Text = "-1";
            }

            try
            {
                m_iTimeout = Convert.ToInt32(txtTimeout.Text);


            }
            catch
            {
                m_iTimeout = -1;
                txtTimeout.Text = "-1";
            }


            if ((m_iFrequency > 0) && (m_iTimeout > 0))
            {
                m_bStop = false;
                m_iOK = 0;
                m_iErrQuery = 0;
                m_iErrSetParking = 0;

                lblOK.Text = m_iOK.ToString();
                lblErrQuery.Text = m_iErrQuery.ToString();
                lblErrSetParking.Text = m_iErrSetParking.ToString();

                btnStart.Enabled = false;
                btnStop.Enabled = true;

                m_Thread = new Thread(new ThreadStart(this.DoTests));
                m_Thread.Start();

            }


        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lock (m_lock0)
            {
                m_bStop = true;
                btnStop.Enabled = false;
                m_Thread.Join(10000);
                btnStart.Enabled = true;

            }

        }

        private void DoTests()
        {

            while (!m_bStop)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SimulateOperation), (object)this);
                Thread.Sleep(60000 / m_iFrequency);
            }

        }

        private void SimulateOperation(object input)
        {
            bool bError = false;

            try
            {
                System.Random rand = new System.Random();
                int iQuantity = 0;
                int iTime = 0;
                int id = 0;
                string strPlate = (rand.Next(9999999).ToString() + DateTime.Now.Millisecond.ToString()).PadLeft(10, '0');
                DateTime dt = DateTime.Now;
                DateTime dtIni = DateTime.Now;
                DateTime dtEnd = DateTime.Now;

                lock (m_lock6)
                {
                    id = m_iCurrID + 1;
                    m_iCurrID = id;
                }

                string strEmail="test@test.com";

                bError = !StandardQueryParkingAmountSteps(strPlate, strEmail, dt, 10505, 10501,out iQuantity, out iTime, out dtIni, out dtEnd);

                if (!bError)
                {                   

                    bError = !StandardConfirmParking(strPlate, strEmail, dt, 10505, 10501, iQuantity, iTime,
                                                    dtIni, dtEnd, id);

                    if (bError)
                    {
                        IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseSetParkingError);
                        this.Invoke(incErrCall, new object[] { });
                    }


                }
                else
                {
                    IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseQueryError);
                    this.Invoke(incErrCall, new object[] { });
                }



            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("FormPlatformServiceTest::SimulateOperation: Exception {0}", e.Message));
            }
            finally
            {

                if (!bError)
                {
                    IncreaseCounterCallBack incOkCall = new IncreaseCounterCallBack(IncreaseOK);
                    this.Invoke(incOkCall, new object[] { });
                }
               


            }

        }


        public bool StandardQueryParkingAmountSteps(string strPlate, string strEmail, DateTime dtParkQuery, int iGroup, int iTariff,out int iQuantity, out int iTime, out DateTime dtIni, out DateTime dtEnd)
        {

            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";
            iQuantity = 0;
            iTime = 0;
            dtIni = DateTime.Now;
            dtEnd = DateTime.Now;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WSIParksuiteTest.StandardParkingWS.TariffComputerWS oParkWS = new WSIParksuiteTest.StandardParkingWS.TariffComputerWS();
                oParkWS.Url = "https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/TariffComputer.asmx";
                oParkWS.Timeout = m_iTimeout;


                oParkWS.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");
                //oParkWS.IsTraceResponseEnabled = true;

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = "10005";
                string strCompanyName = "IPARKME";


                string strMessage = "";
                string strAuthHash = "";


                int? iAmountOffSet = 1; //cents


                strAuthHash = CalculateStandardWSHash("C?p@UnfepyvE6>m}?Df;?J]g",
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, strEmail, 0, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><amou_off>{5}</amou_off><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, strEmail, 0, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";

                strOut = oParkWS.QueryParkingOperationWithAmountSteps(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps xmlOut ={0}", ""/*strOut*/), LogLevels.logDEBUG);

                sXmlOut = PrettyXml(strOut);

                //Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultTypeStandardParkingWS.ResultSP_OK)
                {
                    //rtRes = StandardQueryParkingComputeOutput("", iWSNumber, oUser, strPlate, dtParkQuery, oGroup, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, strExtTariffId, bIsShopKeeperOperation, ref wsParameters, ref parametersOut);
                  
                    rtRes = (ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultTypeStandardParkingWS.ResultSP_OK)
                    {

                        iQuantity = Convert.ToInt32(wsParameters["a_min"]);
                        iTime = Convert.ToInt32(wsParameters["t_min"]);

                        dtIni = DateTime.ParseExact(wsParameters["d_init"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);

                        dtEnd = DateTime.ParseExact(wsParameters["d_min"].ToString(), "HHmmssddMMyyyy",
                                        CultureInfo.InvariantCulture);

                        bRes = true;
                    }
                    

                }

            }
            catch (Exception e)
            {
                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "StandardQueryParkingAmountSteps::Exception", LogLevels.logERROR);


            }

            return bRes;

        }


        public bool StandardConfirmParking( string strPlate, string strEmail, DateTime dtParkQuery, decimal dGroupId, decimal dTariffId, int iQuantity, int iTime,
                                                          DateTime dtIni, DateTime dtEnd, decimal dOperationId)
        {
            bool bRes = false;
            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;


            string sXmlIn = "";
            string sXmlOut = "";

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WSIParksuiteTest.StandardParkingWS.TariffComputerWS oParkWS = new WSIParksuiteTest.StandardParkingWS.TariffComputerWS();
                oParkWS.Url = "https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/TariffComputer.asmx";
                oParkWS.Timeout = m_iTimeout;


                oParkWS.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");
                //oParkWS.IsTraceResponseEnabled = true;

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = "10005";
                string strCompanyName = "IPARKME";


                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateStandardWSHash("C?p@UnfepyvE6>m}?Df;?J]g",
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7:HHmmssddMMyyyy}{8:HHmmssddMMyyyy}{9}{10}{11}{12}",
                    strCityID, strPlate, dtInstallation, dGroupId.ToString(), dTariffId.ToString(), iQuantity, iTime, dtIni, dtEnd, strvers,
                    dOperationId, strCompanyName, strEmail));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><pur_date>{2:HHmmssddMMyyyy}</pur_date>" +
                                           "<grp_id>{3}</grp_id><tar_id>{4}</tar_id>" +
                                           "<amou_payed>{5}</amou_payed><time_payed>{6}</time_payed>" +
                                           "<ini_date>{7:HHmmssddMMyyyy}</ini_date>" +
                                           "<end_date>{8:HHmmssddMMyyyy}</end_date>" +
                                           "<ver>{9}</ver><oper_id>{10}</oper_id><prov>{11}</prov><ext_acc>{12}</ext_acc>" +
                                           "<ah>{13}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, dGroupId.ToString(), dTariffId.ToString(), iQuantity, iTime, dtIni, dtEnd, strvers, dOperationId,
                    strCompanyName, strEmail, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardConfirmParking xmlIn ={0}", sXmlIn), LogLevels.logDEBUG);

                
                string strOut = oParkWS.InsertExternalParkingOperationInstallationTime(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardConfirmParking xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultTypeStandardParkingWS.ResultSP_OK)
                {

                    rtRes = (ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString());

                    if ((rtRes == ResultTypeStandardParkingWS.ResultSP_OK)||(rtRes == ResultTypeStandardParkingWS.ResultSP_Error_OperationAlreadyExist))
                    {
                        bRes = true;
                    }
                }

            }
            catch (Exception e)
            {

                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "StandardConfirmParking::Exception", LogLevels.logERROR);

            }



            return bRes;

        }


        public bool StandardQueryParking(string strPlate, DateTime dtParkQuery, int iGroup, int iTariff)
        {

            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WSIParksuiteTest.StandardParkingWS.TariffComputerWS oParkWS = new WSIParksuiteTest.StandardParkingWS.TariffComputerWS();
                oParkWS.Url = "https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/TariffComputer.asmx";
                oParkWS.Timeout = m_iTimeout;


                oParkWS.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = "10005";
                string strCompanyName = "IPARKME";


                string strMessage = "";
                string strAuthHash = "";


                int? iAmountOffSet = 1; //cents


                strAuthHash = CalculateStandardWSHash("C?p@UnfepyvE6>m}?Df;?J]g",
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, "test@test.com", 0, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><amou_off>{5}</amou_off><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, "test@test.com", 0, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StandardQueryParking xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";

                strOut = oParkWS.QueryParkingOperation(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StandardQueryParking xmlOut ={0}", ""/*sXmlOut*/), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultTypeStandardParkingWS.ResultSP_OK)
                {
                    //rtRes = StandardQueryParkingComputeOutput("", iWSNumber, oUser, strPlate, dtParkQuery, oGroup, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, strExtTariffId, bIsShopKeeperOperation, ref wsParameters, ref parametersOut);
                    bRes = true;

                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "StandardQueryParkingAmountSteps::Exception", LogLevels.logERROR);


            }

            return bRes;

        }

        public bool CheckService()
        {

            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WSIParksuiteTest.StandardParkingWS.TariffComputerWS oParkWS = new WSIParksuiteTest.StandardParkingWS.TariffComputerWS();
                oParkWS.Url = "https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/TariffComputer.asmx";
                oParkWS.Timeout = m_iTimeout;


                oParkWS.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");



                Logger_AddLogMessage(string.Format("CheckService xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";

                strOut = oParkWS.CheckService();

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("CheckService xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultTypeStandardParkingWS.ResultSP_OK)
                {
                    //rtRes = StandardQueryParkingComputeOutput("", iWSNumber, oUser, strPlate, dtParkQuery, oGroup, oTariff, bWithSteps, iMaxAmountAllowedToPay, dChangeToApply, strExtGroupId, strExtTariffId, bIsShopKeeperOperation, ref wsParameters, ref parametersOut);
                    bRes = true;

                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "CheckService::Exception", LogLevels.logERROR);


            }

            return bRes;

        }


        public bool CheckService2()
        {

            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            Stopwatch watch = null;




            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                WebRequest request = WebRequest.Create("https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/api/tariffcomputer");

                request.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = m_iTimeout;
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

                        Logger_AddLogMessage(string.Format("CheckService2 response.xml={0}", PrettyXml(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        /*dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var oJobStatus = oResponse["job_status"];
                        strJobId = oJobStatus["id"];
                        strJobUrl = oJobStatus["url"];
                        strJobStatus = oJobStatus["status"];

                        Logger_AddLogMessage(string.Format("ZendeskUserReplication id={0}; job url={1}", strJobId, strJobUrl), LogLevels.logINFO);
                        if (strJobStatus == "queued")
                        {
                            rtRes = ResultType.Result_OK;
                        }*/

                        bRes = true;

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "CheckService2::Exception", LogLevels.logERROR);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "CheckService2::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }


            return bRes;

        }



        public bool StandardQueryParkingAmountSteps2(string strPlate, DateTime dtParkQuery, int iGroup, int iTariff)
        {

            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            Stopwatch watch = null;




            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = "10005";
                string strCompanyName = "IPARKME";


                string strMessage = "";
                string strAuthHash = "";


                int? iAmountOffSet = 1; //cents


                strAuthHash = CalculateStandardWSHash("C?p@UnfepyvE6>m}?Df;?J]g",
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, "test@test.com", 0, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><amou_off>{5}</amou_off><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, "test@test.com", 0, strvers, strAuthHash);


                string strURL = "https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/api/tariffcomputer/QueryParkingOperationWithAmountSteps";

                //string strURL = "http://localhost:41858/api/tariffcomputer/QueryParkingOperationWithAmountSteps";

                byte[] byteArray = Encoding.UTF8.GetBytes(strMessage);


                WebRequest request = WebRequest.Create(strURL);
                request.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Timeout = m_iTimeout;
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps2 request.xml={0}", PrettyXml(strMessage)), LogLevels.logINFO);


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

                        Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps2 response.xml={0}", ""/*PrettyXml(responseFromServer)*/), LogLevels.logINFO);
                        // Clean up the streams.


                        /*dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var oJobStatus = oResponse["job_status"];
                        strJobId = oJobStatus["id"];
                        strJobUrl = oJobStatus["url"];
                        strJobStatus = oJobStatus["status"];

                        Logger_AddLogMessage(string.Format("ZendeskUserReplication id={0}; job url={1}", strJobId, strJobUrl), LogLevels.logINFO);
                        if (strJobStatus == "queued")
                        {
                            rtRes = ResultType.Result_OK;
                        }*/

                        bRes = true;

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "StandardQueryParkingAmountSteps2::Exception", LogLevels.logERROR);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "StandardQueryParkingAmountSteps2::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }


            return bRes;


        }

        public bool StandardQueryParkingAmountSteps3(string strPlate, DateTime dtParkQuery, int iGroup, int iTariff)
        {

            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            Stopwatch watch = null;




            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = "10005";
                string strCompanyName = "IPARKME";


                string strMessage = "";
                string strAuthHash = "";


                int? iAmountOffSet = 1; //cents


                strAuthHash = CalculateStandardWSHash("C?p@UnfepyvE6>m}?Df;?J]g",
                    string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, "test@test.com", 0, strvers));

                strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><lic_pla>{1}</lic_pla><date>{2:HHmmssddMMyyyy}</date><grp_id>{3}</grp_id><tar_id>{4}</tar_id><amou_off>{5}</amou_off><prov>{6}</prov><ext_acc>{7}</ext_acc><free_time>{8}</free_time><vers>{9}</vers><ah>{10}</ah></ipark_in>",
                    strCityID, strPlate, dtInstallation, iGroup.ToString(), iTariff.ToString(), iAmountOffSet, strCompanyName, "test@test.com", 0, strvers, strAuthHash);


                string strURL = "https://ws-iparksuite.iparkme.com/PreProd/TariffComputer.WS/api/tariffcomputer/QueryParkingOperationWithAmountSteps";

                //string strURL = "http://localhost:41858/api/tariffcomputer/QueryParkingOperationWithAmountSteps";

                byte[] byteArray = Encoding.UTF8.GetBytes(strMessage);


                WebRequest request = WebRequest.Create(strURL);
                request.Credentials = new System.Net.NetworkCredential("integraTariffsPrePro", "TkbiY#D/q.");
                request.Method = "GET";
                request.ContentType = "application/xml";
                request.Timeout = m_iTimeout;
                request.ContentLength = 0;

                Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps2 request.xml={0}", PrettyXml(strMessage)), LogLevels.logINFO);


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

                        Logger_AddLogMessage(string.Format("StandardQueryParkingAmountSteps2 response.xml={0}", PrettyXml(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        /*dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var oJobStatus = oResponse["job_status"];
                        strJobId = oJobStatus["id"];
                        strJobUrl = oJobStatus["url"];
                        strJobStatus = oJobStatus["status"];

                        Logger_AddLogMessage(string.Format("ZendeskUserReplication id={0}; job url={1}", strJobId, strJobUrl), LogLevels.logINFO);
                        if (strJobStatus == "queued")
                        {
                            rtRes = ResultType.Result_OK;
                        }*/

                        bRes = true;

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "StandardQueryParkingAmountSteps2::Exception", LogLevels.logERROR);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                Logger_AddLogException(e, "StandardQueryParkingAmountSteps2::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }


            return bRes;


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
                Logger_AddLogException(e, "CalculateStandardWSHash::Exception", LogLevels.logERROR);
            }

            return strRes;
        }



        private void IncreaseOK()
        {
            lock (m_lock1)
            {
                m_iOK++;
                lblOK.Text = m_iOK.ToString();
            }

        }

        private void IncreaseQueryError()
        {
            lock (m_lock2)
            {
                m_iErrQuery++;
                lblErrQuery.Text = m_iErrQuery.ToString();
            }

        }

       


        private void IncreaseSetParkingError()
        {
            lock (m_lock5)
            {
                m_iErrSetParking++;
                lblErrSetParking.Text = m_iErrSetParking.ToString();
            }

        }


        protected static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        protected static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }



        protected string PrettyXml(string xml)
        {

            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return "\r\n\t" + stringBuilder.ToString().Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + xml + "\r\n";
            }
        }

        protected ResultTypeStandardParkingWS FindOutParameters(string xmlIn, out SortedList parameters)
        {
            ResultTypeStandardParkingWS rtRes = ResultTypeStandardParkingWS.ResultSP_OK;
            parameters = new SortedList();


            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    if (xmlIn.StartsWith("<?xml"))
                    {
                        xmlInDoc.LoadXml(xmlIn);
                    }
                    else
                    {
                        xmlInDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xmlIn);
                    }

                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        switch (Node.Name)
                        {
                            default:

                                if (Node.HasChildNodes)
                                {
                                    if (Node.ChildNodes[0].HasChildNodes)
                                    {
                                        int i = 0;
                                        foreach (XmlNode ChildNode in Node.ChildNodes)
                                        {
                                            if (!ChildNode.ChildNodes[0].HasChildNodes)
                                            {
                                                if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString()] == null)
                                                {
                                                    parameters[Node.Name + "_" + ChildNode.Name] = ChildNode.InnerText.Trim();
                                                }
                                                else
                                                {
                                                    parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString()] = ChildNode.InnerText.Trim();
                                                }
                                            }
                                            else
                                            {
                                                int j = 0;
                                                foreach (XmlNode ChildNode2 in ChildNode.ChildNodes)
                                                {
                                                    if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] == null)
                                                    {
                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] = ChildNode2.InnerText.Trim();
                                                    }
                                                    else
                                                    {
                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString()] = ChildNode2.InnerText.Trim();
                                                    }

                                                }
                                            }
                                            i++;
                                            parameters[Node.Name + "_" + ChildNode.Name + "_num"] = i;
                                        }
                                    }
                                    else
                                    {
                                        parameters[Node.Name] = Node.InnerText.Trim();
                                    }
                                }
                                else
                                {
                                    parameters[Node.Name] = null;
                                }

                                break;

                        }

                    }

                    if (Nodes.Count == 0)
                    {
                        Logger_AddLogMessage(string.Format("FindParameters: Bad Input XML: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logERROR);
                        rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;

                    }


                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindInputParameters: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultTypeStandardParkingWS.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindInputParameters::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }







    }
}
