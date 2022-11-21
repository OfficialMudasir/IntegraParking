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
using Offstreet.Test.Models;
using Offstreet.Test.WS;


namespace OffstreetTest
{
    public partial class Parking : Form
    {
        private UserInfo m_oUserInfo = null;

        public Parking(UserInfo oUserInfo)
        {
            InitializeComponent();

            m_oUserInfo = oUserInfo;

            cmbPlates.Items.AddRange(m_oUserInfo.UserPlates.Plates.Select(i => new ComboBoxItem() { Text = i }).ToArray());
            cmbParkings.Items.AddRange(m_oUserInfo.ZoneTar.ZonesOffstreet.Select(i => new ComboBoxItem() { Value = i.Id, Text = i.Description }).ToArray());

        }

        private void btnQueryCarExitPayment_Click(object sender, EventArgs e)
        {
            btnQueryCarExitPayment.Enabled = false;

            btnQueryCarDiscount.Enabled = false;
            btnConfirmCarPayment.Enabled = false;

            if (cmbPlates.SelectedItem != null && cmbParkings.SelectedItem != null && !string.IsNullOrEmpty(txtOpeId.Text))
            {

                WSIntegraMobile oWS = new WSIntegraMobile();
                SortedList oParametersOut = new SortedList();

                m_oUserInfo.Plate = ((ComboBoxItem)cmbPlates.SelectedItem).Text;
                m_oUserInfo.GroupId = ((ComboBoxItem)cmbParkings.SelectedItem).Value;
                m_oUserInfo.OpeId = txtOpeId.Text;

                string sXmlOut = "";

                ResultType oRes = oWS.QueryCarExitForPayment(m_oUserInfo.User, m_oUserInfo.SessionID, m_oUserInfo.GroupId, m_oUserInfo.OpeId, 1, m_oUserInfo.Plate, DateTime.Now, ref oParametersOut, out sXmlOut);
                txtXmlOut.Text = sXmlOut;
                if (oRes == ResultType.Result_OK)
                {
                    btnQueryCarDiscount.Enabled = true;
                    btnConfirmCarPayment.Enabled = true;
                }
                else
                {
                    MessageBox.Show(string.Format("Parking_QueryCarExitPayment_QueryError:'{0}'", oRes.ToString()), "Parking", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Parking_QueryCarExitPayment_InvalidInputData", "Parking", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnQueryCarExitPayment.Enabled = true;
        }

        private void btnQueryCarDiscount_Click(object sender, EventArgs e)
        {
            btnQueryCarDiscount.Enabled = false;

            if (!string.IsNullOrEmpty(txtDiscountId.Text))
            {
                WSIntegraMobile oWS = new WSIntegraMobile();
                SortedList oParametersOut = new SortedList();

                string sXmlOut = "";

                ResultType oRes = oWS.QueryCarDiscountForPayment(m_oUserInfo.User, m_oUserInfo.SessionID, m_oUserInfo.GroupId, m_oUserInfo.OpeId, 1, txtDiscountId.Text, ref oParametersOut, out sXmlOut);
                txtXmlOut.Text = sXmlOut;
                if (oRes == ResultType.Result_OK)
                {
                    btnQueryCarDiscount.Enabled = true;
                    btnConfirmCarPayment.Enabled = true;
                }
                else
                {
                    MessageBox.Show(string.Format("Parking_QueryCarDiscountPayment_QueryError:'{0}'", oRes.ToString()), "Parking", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Parking_QueryCarDiscountPayment_InvalidInputData", "Parking", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnQueryCarDiscount.Enabled = true;
        }

        private void btnConfirmCarPayment_Click(object sender, EventArgs e)
        {
            btnConfirmCarPayment.Enabled = false;

            WSIntegraMobile oWS = new WSIntegraMobile();
            SortedList oParametersOut = new SortedList();

            string sXmlOut = "";

            ResultType oRes = oWS.ConfirmCarPayment(m_oUserInfo.User, m_oUserInfo.SessionID, m_oUserInfo.GroupId, txtOpeId.Text, 1, ref oParametersOut, out sXmlOut);
            txtXmlOut.Text = sXmlOut;
            if (oRes == ResultType.Result_OK)
            {                
                btnConfirmCarPayment.Enabled = true;
            }
            else
            {
                MessageBox.Show(string.Format("Parking_ConfirmCarPayment_ConfirmError:'{0}'", oRes.ToString()), "Parking", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnConfirmCarPayment.Enabled = true;
        }

    }
}
