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

namespace PlataformaMadridTest
{
    public partial class FormMadridPlatformTest : Form
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(FormMadridPlatformTest));
        private const long BIG_PRIME_NUMBER = 2147483647;
        protected static string _xmlTagName = "ipark";
        protected const string OUT_SUFIX = "_out";


        int m_iFrequency;
        int m_iTimeout;
        bool m_bStop = false;
        Thread m_Thread=null;
        int m_iOK = 0;
        int m_iErrQuery = 0;
        int m_iErrStartSess = 0;
        int m_iErrSetParking = 0;
        int m_iErrEndSess = 0;


        delegate void IncreaseCounterCallBack();
        object m_lock0 = new object();
        object m_lock1 = new object();
        object m_lock2 = new object();
        object m_lock3 = new object();
        object m_lock4 = new object();
        object m_lock5 = new object();
        //object m_PlatLocker = new object();
        
        //protected static PlatformService.AuthSession m_oPlatformServiceAuthSession = null;
        //protected static int m_iPlatformServiceOpenSessions = 0;



        public enum ResultType
        {
            Result_OK = 1,
            Result_Error_InvalidAuthenticationHash = -1,
            Result_Error_ParkingMaximumTimeUsed = -2,
            Result_Error_NotWaitedReentryTime = -3,
            Result_Error_RefundNotPossible = -4,
            Result_Error_Fine_Number_Not_Found = -5,
            Result_Error_Fine_Type_Not_Payable = -6,
            Result_Error_Fine_Payment_Period_Expired = -7,
            Result_Error_Fine_Number_Already_Paid = -8,
            Result_Error_Generic = -9,
            Result_Error_InvalidAuthentication = -11,
            Result_Error_LoginMaximumNumberOfTrialsReached = -12,
            Result_Error_Invalid_First_Name = -13,
            Result_Error_Invalid_Last_Name = -14,
            Result_Error_Invalid_Id = -15,
            Result_Error_Invalid_Country_Code = -16,
            Result_Error_Invalid_Cell_Number = -17,
            Result_Error_Invalid_Email_Number = -18,
            Result_Error_Invalid_Input_Parameter = -19,
            Result_Error_Missing_Input_Parameter = -20,
            Result_Error_Mobile_Phone_Already_Exist = -21,
            Result_Error_Email_Already_Exist = -22,
            Result_Error_Recharge_Failed = -23,
            Result_Error_Recharge_Not_Possible = -24,
            Result_Error_Invalid_City = -25,
            Result_Error_Invalid_User = -26,
            Result_Error_User_Not_Logged = -27,
            Result_Error_Tariffs_Not_Available = -28,
            Result_Error_Invalid_Payment_Mean = -29,
            Result_Error_Invalid_Recharge_Code = -30,
            Result_Error_Expired_Recharge_Code = -31,
            Result_Error_AlreadyUsed_Recharge_Code = -32,
            Result_Error_Not_Enough_Balance = -33,
            Result_Error_ResidentParkingExhausted = -34,
            Result_Error_OperationExpired = -35,
            Result_Error_InvalidTicketId = -36,
            Result_Error_ExpiredTicketId = -37,
            Result_Error_OperationNotFound = -38,
            Result_Error_OperationAlreadyClosed = -39,
            Result_Error_OperationEntryAlreadyExists = -40,
            Result_Error_ConfirmOperationAlreadyExecuting = -41
        }


        public FormMadridPlatformTest()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            m_iOK = 0;
            m_iErrQuery = 0;
            m_iErrStartSess = 0;
            m_iErrSetParking = 0;
            m_iErrEndSess = 0;

            lblOK.Text = m_iOK.ToString();
            lblErrQuery.Text = m_iErrQuery.ToString();
            lblErrStartSess.Text = m_iErrStartSess.ToString();
            lblErrSetParking.Text = m_iErrSetParking.ToString();
            lblErrEndSess.Text = m_iErrEndSess.ToString();

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

            
            if ((m_iFrequency > 0)&&(m_iTimeout>0))
            {
                m_bStop=false;
                m_iOK = 0;
                m_iErrQuery = 0;
                m_iErrStartSess = 0;
                m_iErrSetParking = 0;
                m_iErrEndSess = 0;

                lblOK.Text = m_iOK.ToString();
                lblErrQuery.Text = m_iErrQuery.ToString();
                lblErrStartSess.Text = m_iErrStartSess.ToString();
                lblErrSetParking.Text = m_iErrSetParking.ToString();
                lblErrEndSess.Text = m_iErrEndSess.ToString();

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
                int iQuantity=0;
                int iTime = 0;
                int id = 0;
                string strPlate = rand.Next(9999999).ToString().PadLeft(7,'0');
                DateTime dt = DateTime.Now;
                DateTime dtIni = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                


                bError = !EysaQueryParking(strPlate, dt, out iQuantity, out iTime, out id, out dtIni, out dtEnd);



                if (!bError)
                {
                    bError = !PlatformServiceConfirmParking(strPlate, dt, iQuantity, iTime, dtIni, dtEnd, rand.Next(), id);
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


        public bool EysaQueryParking(string strPlate, DateTime dtParkQuery, out int iQuantity, out int iTime, out int id, out DateTime dtIni, out DateTime dtEnd)
        {

            iQuantity=0;
            iTime = 0; 
            id=0;
            dtIni = DateTime.Now;
            dtEnd = DateTime.Now;

            bool bRes = false;

            string sXmlIn = "";
            string sXmlOut = "";

            try
            {
                EysaWS.Tarifas oParkWS = new EysaWS.Tarifas();
                oParkWS.Timeout = m_iTimeout;


                EysaWS.ConsolaSoapHeader authentication = new EysaWS.ConsolaSoapHeader();
                authentication.IdContrata = 725;
                authentication.IdUsuario = "user@gmail.com";
                oParkWS.ConsolaSoapHeaderValue = authentication;

                DateTime dtInstallation = dtParkQuery;
                string strvers = "1.0";
                string strCityID = "725";
                string strCompanyName = "EYSAMobileTest";


                string strMessage = "";
                string strAuthHash = "";

                string strExtTariffId = "7254";
                string strExtGroupId = "7250245";




                strAuthHash = CalculateEysaWSHash("L·hdf1852*=?(}/^3123M(!",
                    string.Format("1{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}{5}", strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strvers));

                strMessage = string.Format("<ipark_in><u>1</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><tar_id>{4}</tar_id><vers>{5}</vers><ah>{6}</ah><em>{7}</em></ipark_in>",
                    strCityID, strPlate, dtInstallation, strExtGroupId, strExtTariffId, strvers, strAuthHash, strCompanyName);



                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaThirdPartyQueryParkingOperationWithTimeSteps xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = oParkWS.rdPQueryParkingOperationWithTimeSteps(strMessage);
                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaThirdPartyQueryParkingOperationWithTimeSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                ResultType rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {
                        id = Convert.ToInt32(wsParameters["idP"].ToString());
                        dtIni = DateTime.ParseExact(wsParameters["di"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                                           CultureInfo.InvariantCulture);

                        int iNumSteps = Convert.ToInt32(wsParameters["steps_step_num"]);
                        for (int i = 0; i < iNumSteps; i++)
                        {
                            dtEnd = DateTime.ParseExact(wsParameters[string.Format("steps_step_{0}_d", i)].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                            iQuantity = Convert.ToInt32(wsParameters[string.Format("steps_step_{0}_q", i)].ToString());
                            iTime = Convert.ToInt32(wsParameters[string.Format("steps_step_{0}_t", i)].ToString());
                            break;


                        }


                        bRes = true;
                    }

                    
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "EysaQueryParking::Exception", LogLevels.logERROR);
            }

            return bRes;

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

        private void IncreaseStartSessError()
        {
            lock (m_lock3)
            {
                m_iErrStartSess++;
                lblErrStartSess.Text = m_iErrStartSess.ToString();
            }

        }

        private void IncreaseEndSessError()
        {
            lock (m_lock4)
            {
                m_iErrEndSess++;
                lblErrEndSess.Text = m_iErrEndSess.ToString();
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

        protected string CalculateEysaWSHash(string strMACKey, string strInput)
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
                Logger_AddLogException(e, "CalculateEysaWSHash::Exception", LogLevels.logERROR);
            }

            return strRes;
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

        protected ResultType FindOutParameters(string xmlIn, out SortedList parameters)
        {
            ResultType rtRes = ResultType.Result_OK;
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
                        rtRes = ResultType.Result_Error_Generic;

                    }


                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindInputParameters: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindInputParameters::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }



        public bool PlatformServiceConfirmParking(string strPlate, DateTime dtParkQuery, int iQuantity, int iTime,
                                                       DateTime dtIni, DateTime dtEnd, decimal dOperationId, decimal dAuthId)
        {

            bool bRes = false ;

            string strParamsIn = "";
            string strParamsOut = "";
            PlatformService.PublishServiceV12Client oService = null;
            PlatformService.AuthSession oAuthSession = null;

            try
            {
                oService = new PlatformService.PublishServiceV12Client();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender2, certificate, chain, sslPolicyErrors) => true);

                oService.ClientCredentials.UserName.UserName = "UPMEysa";
                oService.ClientCredentials.UserName.Password = "UPMEysa";

                oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, m_iTimeout);

                if (MadridPlatfomStartSession(oService, out oAuthSession))
                {
                    DateTime? dtUTCInstallation = dtParkQuery.AddHours(-2);
                    DateTime? dtUTCIni = dtIni.AddHours(-2);
                    DateTime? dtUTCEnd = dtEnd.AddHours(-2);

                    string strExtTariffId = "6";
                    string strExtGroupId = "045";

                    string sCodPhyZone = strExtGroupId.Split('~')[0];
                    string sSubBarrio = (strExtGroupId.Split('~').Length > 1 ? strExtGroupId.Split('~')[1] : "");

                    var oRequest = new PlatformService.PayParkingTransactionRequest()
                    {
                        PhyZone = new PlatformService.EntityFilterPhyZone()
                        {
                            CodSystem = "1624",
                            CodGeoZone = "01",
                            CodCity = "02",
                            CodPhyZone = sCodPhyZone
                        },
                        PrkTrans = new PlatformService.PayTransactionParking()
                        {
                            AuthId = Convert.ToInt64(dAuthId),
                            OperationDateUTC = dtUTCInstallation.Value,
                            TariffId = Convert.ToInt32(strExtTariffId),
                            TicketNum = string.Format("91{0}00000{1}{2}{3}", sCodPhyZone, dtParkQuery.DayOfYear.ToString("000"), dtIni.ToString("HHmm"), dtEnd.ToString("HHmm")),
                            TransId = Convert.ToInt64(dOperationId),
                            ParkingOper = new PlatformService.PayParking()
                            {
                                PrkBgnUtc = dtUTCIni.Value,
                                PrkEndUtc = dtUTCEnd.Value,
                                TotAmo = ((decimal)iQuantity / (decimal)100),
                                TotTim = new TimeSpan(0, iTime, 0)
                            },
                            UserPlate = strPlate
                        },
                        SubBarrio = sSubBarrio
                    };

                    strParamsIn = string.Format("sessionId={15};userName={16};" +
                                               "CodSystem={0};CodGeoZone={1};CodCity={2};CodPhyZone={3};" +
                                               "AuthId={4};OperationDateUTC={5:yyyy-MM-ddTHH:mm:ss.fff};TariffId={6};TicketNum={7};TransId={8};" +
                                               "PrkBgnUtc={9:yyyy-MM-ddTHH:mm:ss.fff};PrkEndUtc={10:yyyy-MM-ddTHH:mm:ss.fff};TotAmo={11};TotTim={12};UserPlate={13};" +
                                               "SubBarrio={14}",
                                                oRequest.PhyZone.CodSystem, oRequest.PhyZone.CodGeoZone, oRequest.PhyZone.CodCity, oRequest.PhyZone.CodPhyZone,
                                                oRequest.PrkTrans.AuthId, oRequest.PrkTrans.OperationDateUTC, oRequest.PrkTrans.TariffId, oRequest.PrkTrans.TicketNum, oRequest.PrkTrans.TransId,
                                                oRequest.PrkTrans.ParkingOper.PrkBgnUtc, oRequest.PrkTrans.ParkingOper.PrkEndUtc, oRequest.PrkTrans.ParkingOper.TotAmo, oRequest.PrkTrans.ParkingOper.TotTim, oRequest.PrkTrans.UserPlate,
                                                oRequest.SubBarrio,
                                                oAuthSession.sessionId, oAuthSession.userName);

                    Logger_AddLogMessage(string.Format("PlatformServiceConfirmParking parametersIn={0}", strParamsIn), LogLevels.logDEBUG);

                    var oParkingResp = oService.SetParkingTransaction(oAuthSession, oRequest);

                    strParamsOut = string.Format("Status={0};errorDetails={1}", oParkingResp.Status.ToString(), oParkingResp.errorDetails);
                    Logger_AddLogMessage(string.Format("PlatformServiceConfirmParking response={0}", strParamsOut), LogLevels.logDEBUG);

                    bRes = oParkingResp.Status == PlatformService.PublisherResponse.PublisherStatus.OK;

                    if (!bRes)
                    {
                        IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseSetParkingError);
                        this.Invoke(incErrCall, new object[] { });

                    }

                }
               


            }
            catch (Exception e)
            {
                IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseSetParkingError);
                this.Invoke(incErrCall, new object[] { });

                Logger_AddLogException(e, "PlatformServiceConfirmParking::Exception", LogLevels.logERROR);
            }
            finally
            {
                if (oService != null && oAuthSession != null)
                {
                    if (bRes)
                    {
                        bRes= MadridPlatfomEndSession(oService, oAuthSession);
                    }
                }
            }


            return bRes;
        }


        protected bool MadridPlatfomStartSession(PlatformService.PublishServiceV12Client oService, out PlatformService.AuthSession oAuthSession)
        {
            bool bRet = false;
            oAuthSession = null;

            try
            {
                    /*if (m_oPlatformServiceAuthSession == null)
                    {*/
                        Logger_AddLogMessage("MadridPlatfomStartSession - Starting session ... ", LogLevels.logDEBUG);
                        PlatformService.AuthLoginResponse oResponse = null;
                        oResponse = oService.startSession(oService.ClientCredentials.UserName.UserName, oService.ClientCredentials.UserName.Password, "es");
                        if (oResponse.Status == PlatformService.PublisherResponse.PublisherStatus.OK)
                        {
                            bRet = true;
                            //m_oPlatformServiceAuthSession = oResponse.Result;
                            oAuthSession = oResponse.Result;
                            //m_iPlatformServiceOpenSessions += 1;
                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session started successfully: Status='{0}', sessionId='{1}', userName='{2}'", oResponse.Status.ToString(), oAuthSession.sessionId, oAuthSession.userName), LogLevels.logDEBUG);
                        }
                        else
                        {
                            IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseStartSessError);
                            this.Invoke(incErrCall, new object[] { });

                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Error starting session: Status='{0}', errorDetails='{1}'", oResponse.Status.ToString(), oResponse.errorDetails), LogLevels.logERROR);
                        }
                    /*}
                    else
                    {
                        bRet = true;
                        oAuthSession = m_oPlatformServiceAuthSession;
                        m_iPlatformServiceOpenSessions += 1;
                        Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session reused: sessionId='{0}', userName='{1}'", m_oPlatformServiceAuthSession.sessionId, m_oPlatformServiceAuthSession.userName), LogLevels.logDEBUG);
                    }*/
                    //Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Sessions count: {0}", m_iPlatformServiceOpenSessions), LogLevels.logDEBUG);
                
            }
            catch (Exception ex)
            {
                IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseStartSessError);
                this.Invoke(incErrCall, new object[] { });
                Logger_AddLogException(ex, "MadridPlatfomStartSession::Exception", LogLevels.logERROR);
            }

            return bRet;
        }


        protected bool MadridPlatfomEndSession(PlatformService.PublishServiceV12Client oService, PlatformService.AuthSession oAuthSession)
        {
            bool bRet = false;

            try
            {
                
                    /*if (m_iPlatformServiceOpenSessions <= 1)
                    {*/
                        Logger_AddLogMessage("MadridPlatfomEndSession - Ending session ... ", LogLevels.logDEBUG);
                        //m_iPlatformServiceOpenSessions = 0;
                        var oResponse = oService.endSession(oAuthSession);
                        if (oResponse.Status == PlatformService.PublisherResponse.PublisherStatus.OK)
                        {
                            bRet = true;
                            //m_oPlatformServiceAuthSession = null;
                            Logger_AddLogMessage("MadridPlatfomEndSession - Session ended successfully.", LogLevels.logDEBUG);
                        }
                        else
                        {
                            IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseEndSessError);
                            this.Invoke(incErrCall, new object[] { });

                            Logger_AddLogMessage(string.Format("MadridPlatfomEndSession - Error ending session: Status='{0}', errorDetails='{1}'", oResponse.Status.ToString(), oResponse.errorDetails), LogLevels.logERROR);
                        }
                    /*}
                    else
                    {
                        m_iPlatformServiceOpenSessions -= 1;
                        bRet = true;
                    }
                    Logger_AddLogMessage(string.Format("MadridPlatfomEndSession - Sessions count: {0}", m_iPlatformServiceOpenSessions), LogLevels.logDEBUG);*/
                
            }
            catch (Exception ex)
            {
                IncreaseCounterCallBack incErrCall = new IncreaseCounterCallBack(IncreaseEndSessError);
                this.Invoke(incErrCall, new object[] { });

                Logger_AddLogException(ex, "MadridPlatfomEndSession::Exception", LogLevels.logERROR);
            }

            return bRet;
        }



    }
}
