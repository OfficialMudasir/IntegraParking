using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Offstreet.Test.WS;
using Offstreet.Test.WS.Data;
using Offstreet.Test.Models;

namespace OffstreetTest
{
    public partial class frmLogin : Form
    {
        private UserInfo m_oUserInfo = null;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            cmbCities.Items.Clear();

            WSCities oCities = new WSCities();

            WSIntegraMobile oWS = new WSIntegraMobile();
            SortedList oParametersOut = new SortedList();
            ResultType oRes = oWS.GetListOfCities(null, null, out oCities.Cities, ref oParametersOut);

            if (oRes == ResultType.Result_OK)
            {
                cmbCities.Items.AddRange(oCities.GetCities().Select(i => new ComboBoxItem() { Value = i.Id, Text = i.Description }).ToArray());
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text) && cmbCities.SelectedItem != null)
            {
                int iCityId = ((ComboBoxItem)cmbCities.SelectedItem).Value;
                string sCityName = ((ComboBoxItem)cmbCities.SelectedItem).Text;

                WSIntegraMobile oWS = new WSIntegraMobile();
                SortedList oParametersOut = new SortedList();

                string sSessionID = "";
                
                string sLanguage = (ConfigurationManager.AppSettings["Culture"] ?? "es-ES");

                ResultType oRes = oWS.QueryLogin(txtUsername.Text, txtPassword.Text, sLanguage, true, iCityId, null, null, out sSessionID, ref oParametersOut);
                if (oRes == ResultType.Result_OK)
                {
                    m_oUserInfo = new UserInfo()
                    {
                        User = txtUsername.Text,
                        Pwd = txtPassword.Text,
                        SessionID = sSessionID,
                        CityId = iCityId,
                        CityName = sCityName,
                        UserPlates = new WSUserPlates(oParametersOut),
                        UserPreferences = new WSUserPreferences(oParametersOut),
                        Cur = oParametersOut.GetValueString("cur"),
                        Balance = oParametersOut.GetValueInt("bal")
                    };


                        oParametersOut = new SortedList();
                        oRes = oWS.QueryCity(m_oUserInfo.User, m_oUserInfo.SessionID, m_oUserInfo.CityId, m_oUserInfo.LegalTermsVersion, ref oParametersOut);
                        if (oRes == ResultType.Result_OK)
                        {
                            m_oUserInfo.ZoneTar = new WSZoneTar(oParametersOut);

                            Parking oFrmParking = new Parking(m_oUserInfo);


                            oFrmParking.ShowDialog(this);

                            this.Close();
                            //ViewData["CityId"] = oUserInfo.CityId;

                            //return View("Plates", oUserInfo.UserPlates);
                            
                        }
                        else
                        {
                            MessageBox.Show("ParkController_Login_InvalidUsernameOrPassword", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    
                }
                else
                {
                    MessageBox.Show("ParkController_Login_InvalidUsernameOrPassword", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("ParkController_Login_InvalidFormInputData", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

