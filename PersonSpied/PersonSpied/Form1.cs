using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using PersonSpied.Class;
using PersonSpied.DataBaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonSpied
{
    public partial class Form1 : Form
    {
        #region Properties
        public List<PersonClass> PersonList { get; set; }
        public String FileName { get; set; }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region Private Methods
        private void MainMethod(String[] sResult)
        {
            List<ApiResponse> oResult = GetApi(sResult);
            if (checkExportExcel.Checked)
            {
                ExportExcel(oResult);
            }

            if (checkExportTXT.Checked)
            {
                ExportTxt(oResult);
            }

            if (checkSaveToDB.Checked)
            {
                SaveDB(oResult);
            }
            PersonList = null;

            MessageBox.Show("End process.");
            this.txtEmails.Text = String.Empty;
        }

        private String[] GetEmails()
        {
            String[] sResult = null;
            try
            {
                if (!String.IsNullOrEmpty(txtEmails.Text))
                {
                    sResult = txtEmails.Text.Split(',');
                    MainMethod(sResult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return sResult;
        }

        private List<ApiResponse> GetApi(String[] sResult)
        {
            Boolean bException = false;
            List<ApiResponse> list = new List<ApiResponse>();
            String responseFromServer = String.Empty;
            //*******  TO DO: CHAPUZA **********
            Int32 count =0;
            //*******  TO DO: FIN CHAPUZA **********

            foreach (String sEmail in sResult)
            {
                count++;
                String sCadena = sEmail.Replace("\r\n", "");
                sCadena = sEmail.Replace(",", "");
                System.Text.StringBuilder url = new System.Text.StringBuilder();
                url.Append(new Uri(SettingsClass.APIUrl));
                url.Append(ConstantsClass.PARAMETERS_NAME_EMAIL + sCadena);
                url.Append(ConstantsClass.PARAMETERS_SIGNO_AMPERSAND + "match_requirements=" + "names+and+addresses+and+phones");
                url.Append(ConstantsClass.PARAMETERS_SIGNO_AMPERSAND + ConstantsClass.PARAMETERS_NAME_KEY + SettingsClass.APIAuthHashKey);

                try
                {
                    WebRequest request = WebRequest.Create(url.ToString());
                    WebResponse response = request.GetResponse();
                    Stream data = response.GetResponseStream();
                    StreamReader reader = new StreamReader(data);
                    responseFromServer = reader.ReadToEnd();
                    ////*******  TO DO: CHAPUZA **********
                    //if (count == 1)
                    //{
                    //    responseFromServer = @"{'@http_status_code': 200,'@visible_sources': 6,'@available_sources': 251,'@persons_count': 1,'@search_id': '1808211454234354981062149294274786807','query': {'emails': [{'address': 'carroyo@integraparking.com','address_md5': '86e5d94af345f08f58526c4876c9ae25'}]},'available_data': {'premium': {'jobs': 1,'addresses': 1,'phones': 2,'mobile_phones': 1,'landline_phones': 1,'languages': 1,'user_ids': 1,'social_profiles': 1,'names': 2,'genders': 1,'emails': 2}},'person': {'@id': 'b3a57470-72c2-4c7a-8ae6-39d185327e81','@match': 1.0,'@search_pointer': 'ff5f29a86b125007245a14667789196f87e8062a3de20c440a170d035f00285c69d9582099820e0a847a524be0e334af8ccdb34b58d732b65b990c0fec82662ce259d3321d4df3f16357487ed4ac8dd391aeb2b9f1b1b9f0d375bc5afd0df17af471d0bb81dd7f26ccfc2a60c7764a18513d73cee7e77026a12e29871601877b148234290f2f899ca53df784f9e4bc14f1c2bd6caaa3a7dd7730446fcf9a7d319ecc9a367f33ae6f1cc555e8cf29b82b67073360ac9451bd3e4ff1495ba0d45a13e8259ce6b506be10b4be7fc70f85abe6aeb01442126c50dc782d205d0a6fbacc61de94329e4a7bd5ea2e3d6c1dd5f58153ccb56e32be4ecd2ecb0410a5beeed0c62e1db74119b5d0a9e9a8083ebe4e301ed723de75f5e17b3011c1b7bd7237759cccd879497635c7179d6bcdce4f2ad62cf03724c089c69cb48fddfe289284487c6f2799d46f32551238cf1d8f42040e88f322cd7b25c5298427411ee65a7f49b922c6d026db5c1653ba46953379e005565d56a39294c2b2613377343b9c6e8e1968108bb27cf80b656b399389cf0e6de48244157e6f3a3e92c93c1be17532ae6fc60a118daadc4717b64db529632bdbb311868d9a8f76b4a15ce3454780dad7b5cb795e2bc030ab11a6c31e1ce09230c6b834395970717bd8f0fcf4ad12e231d71a9df80026beefe1cddfc0e92aa9eb62005582d2ed4c8e1f869a9a32aaf870925c520fa51713e7993396f29e5e10a4e4853b10d22553aa853725f9abbc43195d3ce19b605e673d84eca95aa14cee85d34c8cc7debb2c0798cea0edcaf2c1f1f7580ba81c52526b1b83cd7ff67da752ff629bf4cce41964703ad16d3eb416f998cdc6f95c90a85d2c4a46d6f1c16e9d81a9207de266f0abcb64d87713282ff6514b0fbffeecbbfbffbb4cb62bbf2053635de365ebf7ae7e157b9383496778eb3ef380731445290a81a93df0487e3a042fdf4be8eec5d6b165fd6fa2a782a4fbdccc23709ae430e84a5a775ab0940a544ec3748e01acae1426316af8a9d99a0eccec7fac5c24be283efa153fbbf95265b344cf73dddf70b11ac4c48d72cc5d0c7feb4eccedf3909cc7e0bbe0f7f2b9d7355a2715fd60d2f5002d9c426cc6f2dc25bab62446de1fcb909373e49b53366c5b74e1bf9030a00a55f24261666c2b644e7f3288c383e58e60d319c7c51c40a2ba82dfba441411a69c0463125a6afaac7fe6b9be1ea51e8437416f4b1de69aba3af84c76b8002614f7f26e7b79fad26a311218f05db7fd61c84512684a2d41bd05a7b23069fedeba0b795733c5cff6','names': [{'@valid_since': '2008-10-08','first': 'Carles','middle': 'Arroyo','last': 'San Jose','display': 'Carles Arroyo San Jose'},{'first': 'Carles','last': 'Arroyo','display': 'Carles Arroyo'}],'emails': [{'@valid_since': '2014-12-01','@last_seen': '2018-01-24','@email_provider': false,'address': 'full.email.available@business.subscription','address_md5': '4ae4685c539a1550a2a303ec1e21fb7b'},{'@valid_since': '2014-12-01','@type': 'work','@email_provider': false,'address': 'full.email.available@business.subscription','address_md5': '86e5d94af345f08f58526c4876c9ae25'}],'phones': [{'@valid_since': '2008-10-08','country_code': 34,'number': 934606139,'display': '934 60 61 39','display_international': '+34 934 60 61 39'}],'gender': {'content': 'male'},'languages': [{'language': 'es','display': 'es'}],'addresses': [{'@valid_since': '2008-10-08','country': 'ES','city': 'Badalona','street': 'Maria Auxiliadora 132','zip_code': '08912','display': 'Maria Auxiliadora 132, Badalona, Spain'}]}}";
                    //}
                    //else if (count == 2)
                    //{
                    //    responseFromServer = @"{'@http_status_code': 200,'@visible_sources': 0,'@available_sources': 1,'@persons_count': 1,'@search_id': '1808211456093260063327837180300170778','query': {'emails': [{'address': 'mperdia@integraparking.com','address_md5': 'df1778d8e5f4e08f8643991909c60a92'}]},'available_data': {'premium': {'usernames': 1,'addresses': 1,'languages': 1,'social_profiles': 1,'names': 1,'dobs': 1,'genders': 1}},'possible_persons': [{'@match': 0.0,'@search_pointer': '4114d5d3453e0976ac52e31f9b69239e72e7d6370d358cf3df7c697efb3ed888231d81a19db7a17f134d605a6b03a0f20828f940ea843678a81fc38cca85aff7519a03843bff52cfd2cc89ee96874db13fa5be7ac082ff2ba66cad2c49f70ad7822942aef19e302907b3221adf50c15c94f15c5aa86ab6c71619603ee697d6b190a9bea41dffd5171e1919c7d475649744d98e1f25eb3e33c834a8bf7eb7e2f5a7532cd7cbb82c91100aabb3420ba556b3d0025d3feb4bdeaadb55b05a5218024653418ed701b4125e2dd6fedb86b1531d348da897072a97ff3b1193ccd6f6e496f0302add9c700c93605a9a0d1d364426cceea78da355256f9b7cae94d6818a59cbce2c879d963a231604411ac796c64137157b78110a1545b0dea7031b37b55a21c92b7ff0a179dca3e3136b0fc1a5a4ad19de17a0ce618d192f613c380cbca7e0c688e3453ef118e5c89a1b706f6119ae1ad38c27ec222eeb60c4b521318f2c6c8b3b299e9c262d528d4b68e6c207','names': [{'@valid_since': '2012-07-18','first': 'M\u00aa','last': 'D\u00edaz','display': 'M\u00aa D\u00edaz'}],'gender': {'@valid_since': '2012-07-18','content': 'female'},'dob': {'@valid_since': '2012-07-18','date_range': {'start': '1949-02-10','end': '1949-02-10'},'display': '69 years old'},'languages': [{'language': 'es','display': 'es'}],'addresses': [{'@valid_since': '2012-07-18','country': 'ES','display': 'Spain'}]}]}";
                    //}
                    //////*******  TO DO: FIN CHAPUZA **********


                    if (!String.IsNullOrEmpty(responseFromServer))
                    {
                        PersonClass oPersonClass = new PersonClass(sCadena, responseFromServer);
                        if (PersonList == null)
                        {
                            PersonList = new List<PersonClass>();
                        }
                        PersonList.Add(oPersonClass);
                    }
                }
                catch(Exception ex)
                {
                    bException = true;
                    Int32 iError=0;
                    if (ex.Message.Contains("(403)"))
                    {
                        iError = 403;
                    }
                    switch (iError)
                    {
                        case 403:
                            MessageBox.Show("API error: You have exceeded your demo usage allowance.");
                            break;
                        default:
                            MessageBox.Show("Generic API error: " + ex.Message);
                            break;
                    }
                }
                if (bException)
                {
                    break;
                }
            }


            if (PersonList != null && PersonList.Count > 0)
            {
                foreach (PersonClass  oPerson in PersonList)
                {
                    ApiResponse result = JsonConvert.DeserializeObject<ApiResponse>(oPerson.JsonPerson);
                    if (result != null)
                    {
                        list.Add(result);
                    }
                }
            }
            return list;
        }

        private void SaveDB(List<ApiResponse> result)
        {

            ConnectionClass oConnectionClass = new ConnectionClass();
            foreach (PersonClass operson in PersonList)
            {
                Int32 iCount = ConnectionClass.Count(SettingsClass.QueryCount + operson.Email.Trim() + "'");
                if (iCount > 0)
                {
                    List<TableUsers> list = ConnectionClass.Select<TableUsers>(SettingsClass.QuerySelect + operson.Email.Trim() + "'");
                    if (list.Count > 0)
                    {
                        String sJson = operson.JsonPerson.Trim().Replace("'", "\"");
                        Boolean bSave = ConnectionClass.Update(SettingsClass.QueryUpdateFirst + sJson + SettingsClass.QueryUpdateLast + operson.Email.Trim() + "'");
                        if (!bSave)
                        {
                            MessageBox.Show("Error trying to update the database");
                        }
                    }
                }
                else
                {
                    String sJson = operson.JsonPerson.Trim().Replace("'", "\"");
                    Boolean bSave = ConnectionClass.Insert(SettingsClass.QueryInsert + operson.Email.Trim() + "','" + sJson + "')");
                    if (!bSave)
                    {
                        MessageBox.Show("Error trying to update the database");
                    }
                }
            }
        }

        private void ExportExcel(List<ApiResponse> result)
        {
            if (result != null && result.Count > 0)
            {
                Int32 i = 1;
                Int32 j = 0;

                Microsoft.Office.Interop.Excel.Application xlApp;
                Workbook xlWorkBook;
                Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = true;
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Worksheet)xlWorkBook.ActiveSheet;

                //Column
                xlWorkSheet.Cells[1, 1] = "Address";
                xlWorkSheet.Cells[1, 2] = "First Name";
                xlWorkSheet.Cells[1, 3] = "Last Name";
                xlWorkSheet.Cells[1, 4] = "Phone";

                foreach (ApiResponse oApiResponse in result)
                {
                    i++;
                    j = 1;
                    if (oApiResponse.query != null)
                    {
                        if (oApiResponse.query.emails.Count > 0)
                        {
                            xlWorkSheet.Cells[i, j] = oApiResponse.query.emails[0].address;
                        }
                    }

                    if (oApiResponse.person != null)
                    {
                        if (oApiResponse.person.names.Count > 0)
                        {
                            j++;
                            xlWorkSheet.Cells[i, j] = oApiResponse.person.names[0].first;
                            j++;
                            xlWorkSheet.Cells[i, j] = oApiResponse.person.names[0].last;
                        }

                        if (oApiResponse.person.phones.Count > 0)
                        {
                            j++;
                            xlWorkSheet.Cells[i, j] = oApiResponse.person.phones[0].number;
                        }
                    }
                }

                String sRouteComplet = GetRoute(SettingsClass.FileNameExcel);
                xlWorkBook.SaveAs(sRouteComplet);
                xlWorkBook.Close();
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);

            }
            else
            {
                MessageBox.Show("No information found");
            }
        
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void ExportTxt(List<ApiResponse> result)
        {
            if (result != null && result.Count > 0)
            {
                String sRouteComplet = GetRoute(SettingsClass.FileNameTxt);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(sRouteComplet))
                {
                    foreach (PersonClass person in PersonList)
                    {
                        file.WriteLine(person.Email.PadRight(50,' ') + person.JsonPerson);
                    }
                }
            }
            else
            {
                MessageBox.Show("No information found");
            }
        }

        private String GetRoute(String nameFile)
        {
            String sRoute = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String sRouteComplet = sRoute + "\\" + nameFile;
            if (System.IO.File.Exists(sRouteComplet))
            {
                System.IO.File.Delete(sRouteComplet);
            }
            return sRouteComplet;
        }
        #endregion

        #region Button
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
           
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                btnSearch2.Enabled = true;
                FileName = theDialog.FileName;
            }
            else
            {
                MessageBox.Show("You have to select a document.");
            }
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(FileName))
            {
                String[] sResult = File.ReadAllLines(FileName);
                MainMethod(sResult);
            }
            else
            {
                MessageBox.Show("Error trying to get the document path.");
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            String[] sResult = GetEmails();
        }
        #endregion
    }
}
