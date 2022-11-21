using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardEaseXML;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;     // For setting the Localization of the thread to fit
using System.Globalization; // the of the MS Excel localization, because of the MS bug
using integraMobile.Infrastructure.Logging.Tools;

namespace Refunds
{
    public partial class Form1 : Form
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(Form1));

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet x2WorkSheet;
            Excel.Worksheet x3WorkSheet;
            Excel.Worksheet x4WorkSheet;
            Excel.Worksheet x5WorkSheet;

            Excel.Range rangeBD;
            Excel.Range rangeCreditCall;
            Excel.Range rangeOK;
            Excel.Range rangeERROR;
            Excel.Range rangeBD2;

            object misval = System.Reflection.Missing.Value;

            string str;
            int rCC_Cnt = 0;
            int rBD_Cnt = 0;
            int rBD2_Cnt = 0;

            int iRowsOK = 1;
            int iRowsError = 1;

            // FileInfo fi = new FileInfo("C:\\XLS\\Recargas2.xlsx");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            xlApp = new Excel.ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Open("C:\\work\\dev\\integra\\Recargas12.xls", 2, false,
                    5, "", "", true, Excel.XlPlatform.xlWindows, "",
                    false, false, 0, false, true, 0);
            // misval, misval, false, misval, misval, misval, misval, misval, misval, misval, misval, misval, misval, misval);
            // 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1); // DB
            x2WorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); // Credit Call
            x3WorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(3); // Results OK
            x4WorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(4); // Results Error
            x5WorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(5); // Transactions committed in DB

            rangeBD = xlWorkSheet.UsedRange;
            rangeCreditCall = x2WorkSheet.UsedRange;
            rangeOK = xlWorkSheet.UsedRange;
            rangeERROR = x2WorkSheet.UsedRange;
            rangeBD2 = x5WorkSheet.UsedRange;

            System.Array valBD = (System.Array)rangeBD.Cells.Value2;
            System.Array valCC = (System.Array)rangeCreditCall.Cells.Value2;
            System.Array valBD2 = (System.Array)rangeBD2.Cells.Value2;

            string strDbg;

            bool bFound = false;

            for (rCC_Cnt = 2; rCC_Cnt <= rangeCreditCall.Rows.Count; rCC_Cnt++)
            {

                if ((valCC.GetValue(rCC_Cnt, 0x2b).ToString() == "Approved") &&
                     (valCC.GetValue(rCC_Cnt, 0x2c).ToString() == "Committed"))  // Check if the transaction is in EYSAMobile DB

                // Check if Credit Call line is committed and approved
                {
                    string str12Val = valCC.GetValue(rCC_Cnt, 0x12).ToString(); //CardEase Reference
                    //string str1fVal = valCC.GetValue(rCC_Cnt, 0x1f).ToString();

                    Excel.Range currentFind = rangeBD2.Find(str12Val, Type.Missing,
                        Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart,
                        Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false,
                        Type.Missing, Type.Missing);
                    if (currentFind != null && currentFind.Count > 0)
                    {
                        for (int iCol = 1; iCol < rangeCreditCall.Columns.Count; iCol++)
                        {
                            ((Excel.Range)x3WorkSheet.Cells[iRowsOK, iCol]).Value2 = valCC.GetValue(rCC_Cnt, iCol).ToString();
                        }
                        iRowsOK++;
                        bFound = true;
                    }
                    else
                        bFound = false;

                    if (!bFound)
                    {
                        for (int iCol = 1; iCol < rangeCreditCall.Columns.Count; iCol++)
                        {
                            ((Excel.Range)x4WorkSheet.Cells[iRowsError, iCol]).Value2 = valCC.GetValue(rCC_Cnt, iCol).ToString();
                        }
                        iRowsError++;
                    }

                    /*for (rBD_Cnt = 2; rBD_Cnt <= rangeBD.Rows.Count; rBD_Cnt++)
                    {

                        if (str12Val.Equals(valBD.GetValue(rBD_Cnt, 0x12).ToString()) || // Transaction ID
                            str12Val.Equals(valBD.GetValue(rBD_Cnt, 0xD).ToString())  // Second Transaction ID
                            //str1fVal.Equals(valBD.GetValue(rBD_Cnt, 0x0f).ToString()) // AuthCode 
                            ) 
                        {
                            for (int iCol = 1; iCol < rangeCreditCall.Columns.Count; iCol++)
                            {
                                ((Excel.Range)x3WorkSheet.Cells[iRowsOK, iCol]).Value2 = valCC.GetValue(rCC_Cnt, iCol).ToString();
                            }
                            // strDbg = "CC card reference: " + valCC.GetValue(rCC_Cnt, 0x12).ToString() + "- CC authcode: " + valCC.GetValue(rCC_Cnt, 0x1f).ToString();
                            // MessageBox.Show(strDbg);

                            iRowsOK++;
                            bFound = true;
                            break;
                        }
                        else
                            strDbg = "KO";
                    }

                    if (!bFound  ) //rBD_Cnt == rangeBD.Rows.Count) // Registre no trobat   
                    {
                        for (int iCol = 1; iCol < rangeCreditCall.Columns.Count; iCol++)
                        {
                            ((Excel.Range)x4WorkSheet.Cells[iRowsError, iCol]).Value2 = valCC.GetValue(rCC_Cnt, iCol).ToString();
                        }
                        iRowsError++;
                    }
                    else bFound = false;
                    */

                }
            }

            xlWorkBook.Save();
            xlWorkBook.Close(true, null, null);

            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(x2WorkSheet);
            releaseObject(x3WorkSheet);
            releaseObject(x4WorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> btnRefunds_Click");

            /*
                <add key="CREDIT_CALL_EKASHU_FORM_URL" value="https://live.ekashu.com" />
                <add key="CREDIT_CALL_CARD_EASE_URL" value="https://live.cardeasexml.com/generic.cex" />
                <add key="CREDIT_CALL_CARD_EASE_TIMEOUT" value="45000" />
                <add key="CREDIT_CALL_TERMINAL_ID" value="25006563" />
                <add key="CREDIT_CALL_TRANSACTION_KEY" value="pEWQZ9qFdcKKvUt6" />
                <add key="CREDIT_CALL_SELLER_NAME" value="EYSAMobile" />
            */

            bool bRes = false;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet x4WorkSheet;
            Excel.Worksheet x2WorkSheet;

            Excel.Range rangeRefunds;
            Excel.Range rangeCC;

            object misval = System.Reflection.Missing.Value;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            xlApp = new Excel.ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Open("C:\\work\\dev\\integra\\Recargas12.xls", 2, false,
                    5, "", "", true, Excel.XlPlatform.xlWindows, "",
                    false, false, 0, false, true, 0);
            // misval, misval, false, misval, misval, misval, misval, misval, misval, misval, misval, misval, misval, misval);
            // 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            x4WorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(4); // Results Error            
            x2WorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);

            rangeRefunds = x4WorkSheet.UsedRange;
            rangeCC = x2WorkSheet.UsedRange;

            System.Array valRefunds = (System.Array)rangeRefunds.Cells.Value2;
            System.Array valCC = (System.Array)rangeCC.Cells.Value2;

            int rRefunds_Cnt = 0;

            string sCardEaseReference = "";
            string sUserReference = "";

            string sOutUserReference;
            string sOutAuthResult;
            string sOutGatewayDate;
            string sOutRefundTransactionId;

            int iCount = 0;

            for (rRefunds_Cnt = 1; rRefunds_Cnt <= rangeRefunds.Rows.Count; rRefunds_Cnt++)
            {
                if (rangeRefunds.Cells.Columns.Count < 53 || valRefunds.GetValue(rRefunds_Cnt, 51) == null || valRefunds.GetValue(rRefunds_Cnt, 51).ToString() == "")
                {
                    sCardEaseReference = valRefunds.GetValue(rRefunds_Cnt, 0x5).ToString(); //CardEase Reference
                    sUserReference = ""; // valRefunds.GetValue(rRefunds_Cnt, 0xF).ToString(); //User Reference

                    Excel.Range currentFind = rangeCC.Find(sCardEaseReference, Type.Missing,
                        Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart,
                        Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false,
                        Type.Missing, Type.Missing);
                    if (currentFind != null && currentFind.Count > 0)
                    {
                        sUserReference = valCC.GetValue(currentFind.Row, 0xF).ToString();
                    }
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 50]).Value2 = sUserReference;

                    sOutUserReference = "PROCESSING";
                    sOutAuthResult = "PROCESSING";
                    sOutGatewayDate = "PROCESSING";
                    sOutRefundTransactionId = "PROCESSING";

                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 51]).Value2 = sOutUserReference;
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 52]).Value2 = sOutAuthResult;
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 53]).Value2 = sOutGatewayDate;
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 54]).Value2 = sOutRefundTransactionId;

                    bRes = Refund(sUserReference, sCardEaseReference, out sOutUserReference, out sOutAuthResult, out sOutGatewayDate, out sOutRefundTransactionId);

                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 51]).Value2 = sOutUserReference;
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 52]).Value2 = sOutAuthResult;
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 53]).Value2 = sOutGatewayDate;
                    ((Excel.Range)x4WorkSheet.Cells[rRefunds_Cnt, 54]).Value2 = sOutRefundTransactionId;

                    iCount += 1;

                    Thread.Sleep(500);
                }
                //if (iCount == 8) break;
            }

            xlWorkBook.Save();
            xlWorkBook.Close(true, null, null);

            xlApp.Quit();

            releaseObject(x4WorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            m_Log.LogMessage(LogLevels.logDEBUG, "<<btnRefunds_Click" + bRes.ToString());         

        }


        private bool Refund(string sUserReference, string sCardEaseReference,
                            out string strUserReference, out string strAuthResult, out string strGatewayDate, out string strRefundTransactionId)
        {

            bool bRes = false;

            strUserReference = "FAIL";
            strAuthResult = "FAIL";
            strGatewayDate = "FAIL";
            strRefundTransactionId = "FAIL";

            // Setup the request
            Request request = new Request();

            request.SoftwareName = "SoftwareName";
            request.SoftwareVersion = "SoftwareVersion";
            request.TerminalID = "25006563";
            request.TransactionKey = "pEWQZ9qFdcKKvUt6";


            /*
             Date/Time (UTC)	Date/Time (Terminal)	Date/Time (Browser)	CardEase Reference	                User Reference	    Card Reference	                                Batch Reference	Creator Email Address	Card Number Start	Card Number End	Expiry Date	Start Date	Issue Number	Card Scheme	Card Type	Auth Code	Result	State	Settlement State	Settlement Date/Time (UTC)	Currency	Amount Authorised	Amount	Latitude	Longitude	Accuracy
             26/04/2014 9:58	26/04/2014 11:58	26/04/2014 11:58	   2d2e343f-29cd-e311-9c2e-0026b93f5045	2014042609580710540	0d4f9582-21ac-e311-a61c-0026b98f240b			                                        450619	               2182	01/2016			Visa	Credit	054506	Approved	Committed	Not Settled / Unknown		EUR	10,00	10,00			

             */

            m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundCommitedPayment.Request: User Reference:" + sUserReference + ", CardEase Reference: " + sCardEaseReference);

            // Setup the request detail
            request.UserReference = sUserReference; // "2014042609580710540"; // Obtenir del XLS
            request.CardEaseReference = sCardEaseReference; // "2d2e343f-29cd-e311-9c2e-0026b93f5045";   // Obtenir del XLS
            request.RequestType = RequestType.Refund;

            // Setup the client
            Client client = new Client();
            client.AddServerURL("https://live.cardeasexml.com/generic.cex", Convert.ToInt32("45000"));
            client.Request = request;

            try
            {
                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundCommitedPayment.Request: " + request.ToString());

                // Process the request
                client.ProcessRequest();
                Response response = client.Response;

                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundCommitedPayment.Response: " + response.ToString());


                bRes = (response.ResultCode == ResultCode.Approved);

                if (bRes)
                {
                    strUserReference = response.UserReference;
                    strAuthResult = response.ResultCode.ToString();
                    strGatewayDate = response.LocalDateTime;
                    strRefundTransactionId = response.CardEaseReference;

                    m_Log.LogMessage(LogLevels.logDEBUG, "UserReference: " + strUserReference.ToString());
                    m_Log.LogMessage(LogLevels.logDEBUG, "AuthResult: " + strAuthResult.ToString());
                    m_Log.LogMessage(LogLevels.logDEBUG, "GatewayDate: " + strGatewayDate.ToString());
                    m_Log.LogMessage(LogLevels.logDEBUG, "RefundTransactionId: " + strRefundTransactionId.ToString());
                }

            }
            catch (CardEaseXMLCommunicationException ex)
            {
                // There is something wrong with communication               
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundCommitedPayment:  ", ex);
            }
            catch (CardEaseXMLRequestException ex)
            {
                // There is something wrong with the request                
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundCommitedPayment: ", ex);
            }
            catch (CardEaseXMLResponseException ex)
            {
                // There is something wrong with the response                            
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundCommitedPayment: ", ex);
            }

            return bRes;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


    }
}
