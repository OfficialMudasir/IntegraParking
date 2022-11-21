using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using integraMobile.Infrastructure;
using System.Globalization;

namespace TestWSWithoutWSDL
{
    public partial class Form1 : Form
    {
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
            Result_Error_ConfirmOperationAlreadyExecuting = -41,
            Result_Error_InvalidAppVersion_UpdateMandatory = -42,
            Result_Error_InvalidAppVersion_UpdateNotMandatory = -43,
            Result_Error_Madrid_Council_Platform_Is_Not_Available = -44,
            Result_Error_TransferingBalance = 0,
            Result_Error_InvalidUserReceiverEmail = -45,
            Result_Error_UserReceiverDisabled = -46,
            Result_Error_UserAccountBlocked = -47,
            Result_Error_UserAccountNotAproved = -48,
            Result_Error_UserBalanceNotEnough = -49,
            Result_Error_AmountNotValid = -50
        }


        internal class GDLGTechnaAPI
        {
            private WebService oWS = null;
            private string strResultString = "";

            public GDLGTechnaAPI(string webserviceEndpoint)
            {
                oWS = new WebService(webserviceEndpoint);               
            }

            public int Timeout
            {
                set
                {
                    oWS.Timeout = value;

                }
            }


            public string Username
            {
                set
                {
                    oWS.Username = value;

                }
            }


            public string Password
            {
                set
                {
                    oWS.Password = value;

                }
            }

            public string ResultXML
            {
                get
                {
                    return strResultString;
                }
            }

            public ResultType GetOutstandingTickets(string strTicketNumber, string strType, string strProvider, string strSecurityToken, out SortedList oTicket)
            {
                ResultType rtRes = ResultType.Result_Error_Generic;
                oTicket = null;
                oWS.PreInvoke();

                oWS.AddParameter("TicketNo", strTicketNumber);
                oWS.AddParameter("Type", strType);
                oWS.AddParameter("Source", strProvider);
                oWS.AddParameter("SecurityToken", strSecurityToken);

                try
                {
                    oWS.Invoke("GetOutstandingTicketsRequest", "GetOutstandingTicketsResponse", "http://soap.payment.seci.cc.gti.com/");
                    strResultString = oWS.ResultString;

                    if (oWS.GetOutputElementCount("Success") == 1)
                    {
                        SortedList oList = null;

                        int iCount = oWS.GetOutputElementCount("Success/OutstandingTickets/Ticket");

                        if (iCount > 1)
                        {
                            int i = 0;

                            while (i < iCount)
                            {
                                oList = oWS.GetOutputElement("Success/OutstandingTickets/Ticket/" + i.ToString());
                                if (oList["TicketNo"].ToString() == strTicketNumber)
                                {
                                    break;
                                }
                                else
                                    oList = null;
                                i++;
                            }
                        }
                        else if (iCount == 1)
                        {
                            oList = oWS.GetOutputElement("Success/OutstandingTickets/Ticket");
                            if (oList != null)
                            {
                                if (oList["TicketNo"].ToString() != strTicketNumber)
                                {
                                    oList = null;
                                    rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                                }
                            }
                        }                        
                        else
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                        }


                        if (oList != null)
                        {
                            oTicket = oList;

                            if (oTicket["TicketStatus"].ToString() == "PA")
                                rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                            else if (oTicket["PayableStatus"].ToString() == "Y")
                                rtRes = ResultType.Result_OK;
                            else
                                rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;

                        }
                    }
                    else if (oWS.GetOutputElementCount("InvalidParameters/InvalidParameter") == 1)
                    {

                        SortedList oList = oWS.GetOutputElement("InvalidParameters");

                        if (oList["InvalidParameter"].ToString().ToLower().Contains("invalid"))
                        {
                            rtRes = ResultType.Result_Error_Invalid_Input_Parameter;
                        }
                        else if (oList["InvalidParameter"].ToString().ToLower().Contains("found"))
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                        }

                    }
                    else if (oWS.GetOutputElementCount("Error/ErrorMessage") == 1)
                    {
                        rtRes = ResultType.Result_Error_Generic;
                    }


                }
                catch (Exception e)
                {
                    strResultString = e.Message;
                    rtRes = ResultType.Result_Error_Generic;
                }
                finally { oWS.PosInvoke(); }

                return rtRes;
            }

            public ResultType PayTicket(string strTicketNumber, double dAmount, string strPaymentType, DateTime dtPayment, string strProvider, 
                                        decimal dTicketId,  string strSecurityToken, out SortedList oTransaction)
            {
                ResultType rtRes = ResultType.Result_Error_Generic;
                oTransaction = null;
                oWS.PreInvoke();

                oWS.AddParameter("TicketNo", strTicketNumber);
                oWS.AddParameter("Amount", dAmount.ToString(CultureInfo.InvariantCulture));
                oWS.AddParameter("PaymentType", strPaymentType);
                oWS.AddParameter("PaymentDateTime", dtPayment.ToString("yyyy-MM-dd HH:mm:ss ")+dtPayment.ToString("zzz").Replace(":",""));
                oWS.AddParameter("TransactionBy", strProvider);
                oWS.AddParameter("Source", strProvider);
                oWS.AddParameter("ReferenceNo", dTicketId.ToString());
                oWS.AddParameter("SecurityToken", strSecurityToken);


                try
                {
                    oWS.Invoke("PayTicketsRequest", "PayTicketsResponse", "http://soap.payment.seci.cc.gti.com/");
                    strResultString = oWS.ResultString;

                    if (oWS.GetOutputElementCount("Success") == 1)
                    {

                        oTransaction = oWS.GetOutputElement("Success/TransactionSet/Transaction");

                        if (oTransaction!=null)
                            rtRes = ResultType.Result_OK;

                    }
                    else if (oWS.GetOutputElementCount("InvalidParameters/InvalidParameter") == 1)
                    {

                        SortedList oList = oWS.GetOutputElement("InvalidParameters");

                        if (oList["InvalidParameter"].ToString().ToLower().Contains("invalid"))
                        {
                            rtRes = ResultType.Result_Error_Invalid_Input_Parameter;
                        }
                        else if (oList["InvalidParameter"].ToString().ToLower().Contains("found"))
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                        }

                    }
                    else if (oWS.GetOutputElementCount("Error/ErrorMessage") == 1)
                    {
                        rtRes = ResultType.Result_Error_Generic;
                    }


                }
                catch (Exception e)
                {
                    strResultString = e.Message;
                    rtRes = ResultType.Result_Error_Generic;
                }
                finally { oWS.PosInvoke(); }

                return rtRes;
            }
        }


        internal class IntegraMobileWSAPI
        {
            private WebService oWS = null;
            private SortedList oOutput = null;

            public IntegraMobileWSAPI(string webserviceEndpoint)
            {
                oWS = new WebService(webserviceEndpoint);
            }

            public int Timeout
            {
                set
                {
                    oWS.Timeout = value;

                }
            }


            public string Username
            {
                set
                {
                    oWS.Username = value;

                }
            }


            public string Password
            {
                set
                {
                    oWS.Password = value;

                }
            }


            public SortedList Output
            {
                get
                {
                    return oOutput;

                }
            }

            public string ResultXML
            {
                get
                {
                    return oWS.ResultString;
                }
            }

            public bool CallStandardMethod(string strMethod, string strXMLIn)
            {
                bool bRes = false;
                oOutput = null;
                oWS.PreInvoke();

                oWS.AddParameter("xmlIn", strXMLIn);

                try
                {
                    oWS.Invoke(strMethod);

                    if (oWS.GetOutputElementCount("ipark_out") == 1)
                    {
                        oOutput = oWS.GetOutputElement("ipark_out");
                        bRes = true;
                    }
                   
                }                
                finally { oWS.PosInvoke(); }

                return bRes;
            }


            public int GetOutputElementCount(string strPath)
            {
                return oWS.GetOutputElementCount(strPath, oOutput);
            }

            public SortedList GetOutputElement(string strPath)
            {
                return oWS.GetOutputElement(strPath, oOutput);
            }

            public string GetOutputStringElement(string strPath)
            {
                return oWS.GetOutputStringElement(strPath, oOutput);
            }

            public ArrayList GetOutputElementArray(string strPath)
            {
                return oWS.GetOutputElementArray(strPath, oOutput);
            }


        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*double d=CCurrencyConvertor2.ConvertCurrency(50,"MXN","EUR");

            d = CCurrencyConvertor2.ConvertCurrency(50, "EUR", "MXN");

            Console.Write(d);*/


            GDLGTechnaAPI oGDLGTechnaAPI = new GDLGTechnaAPI("https://gdl.gtechna.net/officercc/services/TicketPaymentListener");
            SortedList oTicket=null;
            SortedList oTransaction = null;
            ResultType rtRes = oGDLGTechnaAPI.GetOutstandingTickets("100001435", "P", "INTEGRA", "0a65bbf6656ddd6acee0e771f66684edb78d7890", out oTicket);

            MessageBox.Show(rtRes.ToString());

            if (rtRes == ResultType.Result_OK)
            {
                DateTime dt = DateTime.ParseExact(oTicket["InfractionDate"].ToString(),"yyyy-MM-dd HH:mm:ss zzz",CultureInfo.InvariantCulture);
                MessageBox.Show(dt.ToString());
            }


            rtRes =  oGDLGTechnaAPI.PayTicket("100001228", 530.0,  "WALLET", DateTime.Now,"INTEGRA", 1, "0a65bbf6656ddd6acee0e771f66684edb78d7890", out oTransaction);
           
            MessageBox.Show(rtRes.ToString());

            if (rtRes == ResultType.Result_OK)
            {
                int iTransactionId = Convert.ToInt32(oTransaction["TransactionID"].ToString());
                MessageBox.Show(iTransactionId.ToString());
            }

            /*
            IntegraMobileWSAPI oIntegraMobileAPi = new IntegraMobileWSAPI("https://ws.iparkme.com/integraMobileWS/integraMobileWS.asmx");
            oIntegraMobileAPi.Timeout = 10000;
            oIntegraMobileAPi.Username = "integraMobile";
            oIntegraMobileAPi.Password = "kI5~6.!5_j";

            bool bRes=oIntegraMobileAPi.CallStandardMethod("QueryLogin","<ipark_in>"+
	                                                              "<appvers>1.0</appvers>"+
	                                                              "<cityID>10004</cityID>"+
	                                                              "<IMEI>866695021566658</IMEI>"+
	                                                              "<keepsessionalive>1</keepsessionalive>"+
	                                                              "<lang>1</lang>"+
	                                                              "<pasw>1234</pasw>"+
	                                                              "<pushID>APA91bHcruOg3c7kBjp2CgbAG-g-g4sfL-qG6E7Bc_qgVjxzoACOJ9a9-z_rF6_bM6x0L28Z7QvxREkAUNzSiPohyMdDmZFpl3v-8-NgilS1ks4zgSSio4UWnmVJBqixdYENuC1eMQwx</pushID>"+
	                                                              "<u>febermejo@integraparking.com</u>"+
	                                                              "<WIFIMAC>b8:bc:1b:18:de:4e</WIFIMAC>"+
	                                                              "<OSID>1</OSID>"+
	                                                              "<utc_offset>120</utc_offset>"+
	                                                              "<ah>874FFDB2965BADDD</ah>"+
	                                                          "</ipark_in>");

            if (bRes)
            {
                //Example of getting simple value
                MessageBox.Show(oIntegraMobileAPi.Output["SessionID"].ToString());

                //a differente way to do the same
                MessageBox.Show(oIntegraMobileAPi.GetOutputStringElement("SessionID"));



                //Example of getting complex type
                SortedList oUserData = oIntegraMobileAPi.GetOutputElement("userDATA");

                if (oUserData != null)
                {
                    MessageBox.Show(oUserData["ccpan"].ToString());
                }

                //A differente way to do the same
                MessageBox.Show(oIntegraMobileAPi.GetOutputStringElement("userDATA/ccpan"));



                //Iterate in an array
                ArrayList oValues = oIntegraMobileAPi.GetOutputElementArray("userDATA/val_autbelow/value");

                int i = 0;
                while (i < oValues.Count)
                {
                    MessageBox.Show(oValues[i].ToString());
                    i++;
                }


                //Another way to do the same
                int iCount = oIntegraMobileAPi.GetOutputElementCount("userDATA/val_autbelow/value");

                i = 0;
                while (i < iCount)
                {
                    MessageBox.Show(oIntegraMobileAPi.GetOutputStringElement(string.Format("userDATA/val_autbelow/value/{0}",i)));
                    i++;
                }


            }
             */

        }
    }
}
