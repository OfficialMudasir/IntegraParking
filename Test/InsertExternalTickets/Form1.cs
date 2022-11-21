using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Cryptography;
using System.Configuration;

namespace InsertExternalTickets
{
    public partial class Form1 : Form
    {
        private const long BIG_PRIME_NUMBER = 2147483647;

        //eysamobile prod     --------------    integraMobile -----   $%&MiLR(=!  ------  L·hdf1852*=?(}/^3123M(!  ----- https://www.eysamobile.com/integraMobileWS/integraExternalServices.asmx
        //eysamobile preprod  --------------    integraMobilePreProd ----- ZkDl:5cVyK ------  L·hdf1852*=?(}/^3123M(!   ------------https://www.eysamobile.com/PreProd/integraMobileWS/integraExternalServices.asmx
        //iparkme prod  --------------------    integraMobile -----   kI5~6.!5_j  ------  V#SwAphuWuPafr3t3UWruyen  ----- https://ws.iparkme.com/integraMobileWS/integraExternalServices.asmx
        //iparkme preprod ------------------    integraMobilePreProd ----- %2~p~C%A0Z ------  O3AG4otTY6K9S0akV14x6FWN   ------------https://ws.iparkme.com/PreProd/integraMobileWS/integraExternalServices.asmx
        //bilbaopark dev ------------------    integraMobilePreProd ----- %2~p~C%A0Z ------  3C_Lt>R7u]6KU*h65!CSFD/}   ------------https://ws.iparkme.com/PreProd/integraMobileWS/integraExternalServices.asmx
        //bilbaopark preprod ------------------    integraMobilePreProd ----- %2~p~C%A0Z ------  3C_Lt>R7u]6KU*h65!CSFD/}   ------------https://ws.iparkme.com/PreProd/integraMobileWS/integraExternalServices.asmx
        //bilbaopark prod ------------------    integraMobile -----   kI5~6.!5_j  ------  .s#fhN^nZ5xaNW].Tk:cxP=W   ------------https://ws.iparkme.com/PreProd/integraMobileWS/integraExternalServices.asmx

        //Eysamobile prod
        //private const string HTTP_USERNAME = "integraMobile";
        //private const string HTTP_PASSWORD = "$%&MiLR(=!";
        //Eysamobile preprod
        //private const string HTTP_USERNAME = "integraMobilePreProd";
        //private const string HTTP_PASSWORD = "ZkDl:5cVyK";
        // iparkme prod
        //private const string HTTP_USERNAME = "integraMobile";
        //private const string HTTP_PASSWORD = "kI5~6.!5_j";
        //iparkme preprod
        private const string HTTP_USERNAME = "integraMobilePreProd";
        private const string HTTP_PASSWORD = "%2~p~C%A0Z";


        public Form1()
        {
            InitializeComponent();

            edtDateOfTicket_TI.Text = DateTime.Now.ToString("HHmmssddMMyy");
            edtEndDate_TI.Text = DateTime.Now.AddMinutes(60).ToString("HHmmssddMMyy");
            edtBeginDate_PI.Text = DateTime.Now.ToString("HHmmssddMMyy");
            edtEndDate_PI.Text = DateTime.Now.AddMinutes(60).ToString("HHmmssddMMyy");
        }

        private void btnSendTicket_Click(object sender, EventArgs e)
        {

            string strCityID_TI = edtCityID_TI.Text;
            string strPlate_TI = edtPlate_TI.Text;
            string strDateOfTicket_TI = edtDateOfTicket_TI.Text;
            string strFineNumber_TI = edtFineNumber_TI.Text;
            string strFineAmount_TI = edtFineAmount_TI.Text;
            string strEndDate_TI = edtEndDate_TI.Text;
            string strArtType_TI = edtArtType_TI.Text;
            string strArtDesc_TI = edtArtDesc_TI.Text;


            try
            {
                int iTest = Convert.ToInt32(strCityID_TI);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad City ID");
                return;
            }

            if (string.IsNullOrEmpty(strPlate_TI))
            {
                MessageBox.Show("Bad Plate");
                return;
            }


            try
            {
                DateTime dt = DateTime.ParseExact(strDateOfTicket_TI, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad Date of Ticket");
                return;
            }


            if (string.IsNullOrEmpty(strFineNumber_TI))
            {
                MessageBox.Show("Bad Fine Number");
                return;
            }


            try
            {
                int iTest = Convert.ToInt32(strFineAmount_TI);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad Amount");
                return;
            }



            try
            {
                DateTime dt = DateTime.ParseExact(strEndDate_TI, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad End Date of Ticket");
                return;
            }

            if (string.IsNullOrEmpty(strArtType_TI))
            {
                MessageBox.Show("Bad Article Type");
                return;
            }

            if (string.IsNullOrEmpty(strArtDesc_TI))
            {
                MessageBox.Show("Bad Article Description");
                return;
            }

            string strAuthHash = CalculateWSHash(ConfigurationManager.AppSettings["AuthHashKey"].ToString(),
                            string.Format("{0}{1}{2}{3}{4}{5}{6}{7}1.0", strCityID_TI,
                                                             strPlate_TI,
                                                             strDateOfTicket_TI,
                                                             strFineNumber_TI,
                                                             strFineAmount_TI,
                                                             strEndDate_TI,
                                                             strArtType_TI,
                                                             strArtDesc_TI));




            string strMessage = string.Format("<ipark_in><city_id>{0}</city_id><lp>{1}</lp><d>{2}</d><f>{3}</f><q>{4}</q><df>{5}</df><ta>{6}</ta><dta>{7}</dta><vers>1.0</vers><ah>{8}</ah></ipark_in>",
                                                             strCityID_TI,
                                                             strPlate_TI,
                                                             strDateOfTicket_TI,
                                                             strFineNumber_TI,
                                                             strFineAmount_TI,
                                                             strEndDate_TI,
                                                             strArtType_TI,
                                                             strArtDesc_TI,
                                                             strAuthHash);

            strMessage = "<ipark_in>" +
      "<city_id>300001</city_id>" +
      "<lp>ABC123</lp>" +
      "<d>190915250517</d>" +
      "<f>179980350</f>" +
      "<q>0</q>" +
      "<df>235959270517</df>" +
      "<ta>064.1.C</ta>" +
      "<dta>" +
      "  <lang>" +
      "    <es>ESTACIONAR EN UNA PLAZA DE UN ÁREA RESERVADA PARA PERSONAS CON DISCAPACIDAD, SIN ESTAR AUTORIZADO.</es>" +
      "    <eu>GAITASUN URRIKOENTZAT ERRESERBATUTA DAGOEN EREMUAN APARKATZEA HORRETARAKO BAIMENIK IZAN GABE.</eu>" +
      "    <fr />" +
      "    <en />" +
      "  </lang>" +
      "  <sector>60024</sector>" +
      "  <user>70881</user>" +
      "</dta>" +
      "<vers>1.0</vers>" +
      "<ah>5B17F4B2722AB26E</ah>" +
          "</ipark_in>";

            try
            {

                integraExternalServicesWS.integraExternalServices oWS = new integraExternalServicesWS.integraExternalServices();
                oWS.Credentials = new System.Net.NetworkCredential(HTTP_USERNAME, HTTP_PASSWORD); 

                string strOut = oWS.NotifyPlateFine(strMessage);

                MessageBox.Show("Result: " + strOut);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error calling WS"+exc.Message);
            }

            

            edtDateOfTicket_TI.Text = DateTime.Now.ToString("HHmmssddMMyy");
            edtEndDate_TI.Text = DateTime.Now.AddMinutes(60).ToString("HHmmssddMMyy");
            edtBeginDate_PI.Text = DateTime.Now.ToString("HHmmssddMMyy");
            edtEndDate_PI.Text = DateTime.Now.AddMinutes(60).ToString("HHmmssddMMyy");



        }

        private void btnSendParking_Click(object sender, EventArgs e)
        {
            string strCityID_PI = edtCityID_PI.Text;
            string strPlate_PI =edtPlate_PI.Text;
            string strEndDate_PI =edtEndDate_PI.Text;
            string strBeginDate_PI =edtBeginDate_PI.Text;
            string strParkingSector_PI =edtParkingSector_PI.Text;
            string strTariff_PI =edtTariff_PI.Text;
            string strAmount_PI =edtAmount_PI.Text;
            string strTime_PI =edtTime_PI.Text;





            try
            {
                int iTest = Convert.ToInt32(edtCityID_PI.Text);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad City ID");
                return;
            }

            if (string.IsNullOrEmpty(strPlate_PI))
            {
                MessageBox.Show("Bad Plate");
                return;
            }


            try
            {
                DateTime dt = DateTime.ParseExact(strEndDate_PI, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad End Date of Parking");
                return;
            }


            if (!string.IsNullOrEmpty(strBeginDate_PI))
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(strBeginDate_PI, "HHmmssddMMyy", CultureInfo.InvariantCulture);

                }
                catch (Exception)
                {

                    MessageBox.Show("Bad Begin Date of Parking");
                    return;
                }
            }


            if (!string.IsNullOrEmpty(strParkingSector_PI))
            {
                try
                {
                    int iTest = Convert.ToInt32(strParkingSector_PI);

                }
                catch (Exception)
                {

                    MessageBox.Show("Bad Parking Sector");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(strTariff_PI))
            {
                try
                {
                    int iTest = Convert.ToInt32(strTariff_PI);

                }
                catch (Exception)
                {

                    MessageBox.Show("Bad Tariff");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(strAmount_PI))
            {
                try
                {
                    int iTest = Convert.ToInt32(strAmount_PI);

                }
                catch (Exception)
                {

                    MessageBox.Show("Bad Amount");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(strTime_PI))
            {
                try
                {
                    int iTest = Convert.ToInt32(strTime_PI);

                }
                catch (Exception)
                {

                    MessageBox.Show("Bad Time");
                    return;
                }
            }





            string strAuthHash = CalculateWSHash(ConfigurationManager.AppSettings["AuthHashKey"].ToString(),
                            string.Format("{0}{1}{2}{3}{3}{4}{5}{6}{7}TEST111.0", strCityID_PI,
                                                                      strPlate_PI,
                                                                      strEndDate_PI,
                                                                      strBeginDate_PI,
                                                                      strParkingSector_PI,
                                                                      strTariff_PI,
                                                                      strAmount_PI,
                                                                      strTime_PI));







            string strMessage = string.Format("<ipark_in><city_id>{0}</city_id><p>{1}</p><ed>{2}</ed><bd>{3}</bd><d>{3}</d><g>{4}</g><ad>{5}</ad><q>{6}</q><t>{7}</t><prov_name>TEST</prov_name><srcType>1</srcType><type>1</type><vers>1.0</vers><ah>{8}</ah></ipark_in>", 
                                                                      strCityID_PI,
                                                                      strPlate_PI,
                                                                      strEndDate_PI,
                                                                      strBeginDate_PI,
                                                                      strParkingSector_PI,
                                                                      strTariff_PI,
                                                                      strAmount_PI,
                                                                      strTime_PI,
                                                                      strAuthHash);



            try
            {
                integraExternalServicesWS.integraExternalServices oWS = new integraExternalServicesWS.integraExternalServices();

                oWS.Credentials = new System.Net.NetworkCredential(HTTP_USERNAME, HTTP_PASSWORD); 

                string strOut = oWS.NotifyPlateParking(strMessage);

                MessageBox.Show("Result: " + strOut);
            }
            catch (Exception)
            {
                MessageBox.Show("Error calling WS");
            }



            edtDateOfTicket_TI.Text = DateTime.Now.ToString("HHmmssddMMyy");
            edtEndDate_TI.Text = DateTime.Now.AddMinutes(60).ToString("HHmmssddMMyy");
            edtBeginDate_PI.Text = DateTime.Now.ToString("HHmmssddMMyy");
            edtEndDate_PI.Text = DateTime.Now.AddMinutes(60).ToString("HHmmssddMMyy");




        }


        private string CalculateWSHash(string strMACKey, string strInput)
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

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void btNotifyCarEntry_Click(object sender, EventArgs e)
        {

            string sParkingId = txtNotifyCarEntryParkingId.Text;
            string sOpeId = txtNotifyCarEntryOpeId.Text;
            string sOpeIdType = txtNotifyCarEntryOpeIdType.Text;
            string sPlate = txtNotifyCarEntryPlate.Text;
            string sDate = txtNotifyCarEntryDate.Text;
            string sGateId = txtNotifyCarEntryGateId.Text;
            string sTariffId = txtNotifyCarEntryTariffId.Text;

            if (string.IsNullOrEmpty(sParkingId))
            {
                MessageBox.Show("Bad Parking Id");
                return;
            }

            if (string.IsNullOrEmpty(sOpeId))
            {
                MessageBox.Show("Bad OpeId");
                return;
            }

            try
            {
                int iTest = Convert.ToInt32(sOpeIdType);
                if (iTest != 0 && iTest != 1)
                {                    
                    throw new Exception();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bad OpeId type");
                return;
            }

            if (string.IsNullOrEmpty(sPlate))
            {
                MessageBox.Show("Bad Plate");
                return;
            }

            try
            {
                DateTime dt = DateTime.ParseExact(sDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad Entry Date");
                return;
            }

            string strAuthHash = CalculateWSHash(ConfigurationManager.AppSettings["AuthHashKey"].ToString(),
                            string.Format("{0}{1}{2}{3}{4}{5}{6}1.0", sParkingId,
                                                             sOpeId,
                                                             sOpeIdType,
                                                             sPlate,
                                                             sDate,
                                                             sGateId,
                                                             sTariffId));




            string strMessage = string.Format("<ipark_in><parking_id>{0}</parking_id><ope_id>{1}</ope_id><ope_id_type>{2}</ope_id_type><p>{3}</p><d>{4}</d><gate_id>{5}</gate_id><tariff_id>{6}</tariff_id><vers>1.0</vers><ah>{7}</ah></ipark_in>",
                                                             sParkingId,
                                                             sOpeId,
                                                             sOpeIdType,
                                                             sPlate,
                                                             sDate,
                                                             sGateId,
                                                             sTariffId,
                                                             strAuthHash);



            try
            {


                integraExternalServicesWS.integraExternalServices oWS = new integraExternalServicesWS.integraExternalServices();

                oWS.Credentials = new System.Net.NetworkCredential(HTTP_USERNAME, HTTP_PASSWORD); 

                string strOut = oWS.NotifyCarEntry(strMessage);

                MessageBox.Show("Result: " + strOut);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error calling WS" + exc.Message);
            }

        }

        private void btnNotifyCarExit_Click(object sender, EventArgs e)
        {
            string sParkingId = txtNotifyCarExitParkingId.Text;
            string sOpeId = txtNotifyCarExitOpeId.Text;
            string sOpeIdType = txtNotifyCarExitOpeIdType.Text;
            string sPlate = txtNotifyCarExitPlate.Text;
            string sDate = txtNotifyCarExitDate.Text;
            string sGateId = txtNotifyCarExitGateId.Text;

            if (string.IsNullOrEmpty(sParkingId))
            {
                MessageBox.Show("Bad Parking Id");
                return;
            }

            if (string.IsNullOrEmpty(sOpeId))
            {
                MessageBox.Show("Bad OpeId");
                return;
            }

            try
            {
                int iTest = Convert.ToInt32(sOpeIdType);
                if (iTest != 0 && iTest != 1)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bad OpeId type");
                return;
            }

            /*if (string.IsNullOrEmpty(sPlate))
            {
                MessageBox.Show("Bad Plate");
                return;
            }*/

            try
            {
                DateTime dt = DateTime.ParseExact(sDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad Entry Date");
                return;
            }

            string strAuthHash = CalculateWSHash(ConfigurationManager.AppSettings["AuthHashKey"].ToString(),
                            string.Format("{0}{1}{2}{3}{4}{5}1.0", sParkingId,
                                                             sOpeId,
                                                             sOpeIdType,
                                                             sPlate,
                                                             sDate,
                                                             sGateId));




            string strMessage = string.Format("<ipark_in><parking_id>{0}</parking_id><ope_id>{1}</ope_id><ope_id_type>{2}</ope_id_type><p>{3}</p><exd>{4}</exd><gate_id>{5}</gate_id><vers>1.0</vers><ah>{6}</ah></ipark_in>",
                                                             sParkingId,
                                                             sOpeId,
                                                             sOpeIdType,
                                                             sPlate,
                                                             sDate,
                                                             sGateId,                                                             
                                                             strAuthHash);



            try
            {

                integraExternalServicesWS.integraExternalServices oWS = new integraExternalServicesWS.integraExternalServices();

                oWS.Credentials = new System.Net.NetworkCredential(HTTP_USERNAME, HTTP_PASSWORD); 


                string strOut = oWS.NotifyCarExit(strMessage);

                MessageBox.Show("Result: " + strOut);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error calling WS" + exc.Message);
            }

        }

        private void btnTryCarPayment_Click(object sender, EventArgs e)
        {
            string sParkingId = txtTryCarPaymentParkingId.Text;
            string sOpeId = txtTryCarPaymentOpeId.Text;
            string sOpeIdType = txtTryCarPaymentOpeIdType.Text;
            string sPlate = txtTryCarPaymentPlate.Text;
            string sOp = txtTryCarPaymentOp.Text;
            string sAmount = txtTryCarPaymentAmount.Text;
            string sCur = txtTryCarPaymentCur.Text;
            string sTime = txtTryCarPaymentTime.Text;
            string sEntryDate = txtTryCarPaymentEntryDate.Text;
            string sEndDate = txtTryCarPaymentEndDate.Text;
            string sExitLimitDate = txtTryCarPaymentExitLimitDate.Text;
            string sGateId = txtTryCarPaymentGateId.Text;
            string sTariffId = txtTryCarPaymentTariffId.Text;

            if (string.IsNullOrEmpty(sParkingId))
            {
                MessageBox.Show("Bad Parking Id");
                return;
            }

            if (string.IsNullOrEmpty(sOpeId))
            {
                MessageBox.Show("Bad OpeId");
                return;
            }

            try
            {
                int iTest = Convert.ToInt32(sOpeIdType);
                if (iTest != 0 && iTest != 1)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bad OpeId type");
                return;
            }

            if (string.IsNullOrEmpty(sPlate))
            {
                MessageBox.Show("Bad Plate");
                return;
            }

            try
            {
                int iTest = Convert.ToInt32(sOp);
                if (iTest != 0 && iTest != 1)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bad Op");
                return;
            }

            try
            {
                int iTest = Convert.ToInt32(sAmount);
            }
            catch (Exception)
            {

                MessageBox.Show("Bad Amount");
                return;
            }

            if (string.IsNullOrEmpty(sCur))
            {
                MessageBox.Show("Bad Cur");
                return;
            }

            try
            {
                int iTest = Convert.ToInt32(sTime);
            }
            catch (Exception)
            {

                MessageBox.Show("Bad Time");
                return;
            }

            try
            {
                DateTime dt = DateTime.ParseExact(sEntryDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad Entry Date");
                return;
            }

            try
            {
                DateTime dt = DateTime.ParseExact(sEndDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad End Date");
                return;
            }

            try
            {
                DateTime dt = DateTime.ParseExact(sExitLimitDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {

                MessageBox.Show("Bad Exit Limit Date");
                return;
            }

            string strAuthHash = CalculateWSHash(ConfigurationManager.AppSettings["AuthHashKey"].ToString(),
                            string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}1.0", sParkingId,
                                                             sOpeId,
                                                             sOpeIdType,
                                                             sPlate,
                                                             sOp,
                                                             sAmount,
                                                             sCur, 
                                                             sTime,
                                                             sEntryDate,
                                                             sEndDate,
                                                             sExitLimitDate,
                                                             sGateId,
                                                             sTariffId));




            string strMessage = string.Format("<ipark_in><parking_id>{0}</parking_id><ope_id>{1}</ope_id><ope_id_type>{2}</ope_id_type><p>{3}</p><op>{4}</op><q>{5}</q><cur>{6}</cur><t>{7}</t><bd>{8}</bd><ed>{9}</ed><med>{10}</med><gate_id>{11}</gate_id><tariff_id>{12}</tariff_id><vers>1.0</vers><ah>{13}</ah></ipark_in>",
                                                             sParkingId,
                                                             sOpeId,
                                                             sOpeIdType,
                                                             sPlate,
                                                             sOp,
                                                             sAmount,
                                                             sCur,
                                                             sTime,
                                                             sEntryDate,
                                                             sEndDate,
                                                             sExitLimitDate,
                                                             sGateId,
                                                             sTariffId,
                                                             strAuthHash);



            try
            {
              

                integraExternalServicesWS.integraExternalServices oWS = new integraExternalServicesWS.integraExternalServices();

                oWS.Credentials = new System.Net.NetworkCredential(HTTP_USERNAME, HTTP_PASSWORD); 

                string strOut = oWS.TryCarPayment(strMessage);

                MessageBox.Show("Result: " + strOut);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error calling WS" + exc.Message);
            }

        }


    }
}
