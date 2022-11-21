using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationsGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            edtAndroidRawData.Text = "{\"alert\":\"Hello\",\"badge\":7,\"sound\":\"sound.caf\"}";
            edtParam.Text = "/SomeOtherView.xaml?finenumber=11111111";
            edtBackgroudImage.Text = "/Image.png";
        }



        private void btnWP_Click(object sender, EventArgs e)
        {
            if (edtUsername.Text.Length > 0)
            {

                int iCount=-1;
                try
                {
                    if (edtCount.Text.Length>0)
                    {
                        iCount = Convert.ToInt32(edtCount.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("Count must be a valid number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {

                    NoficationWS.integraExternalServices oWS = new NoficationWS.integraExternalServices();
                    oWS.Credentials = new System.Net.NetworkCredential("integraMobile", "$%&MiLR(=!");

                    if (!oWS.SendPushWPNotificationsTo(edtUsername.Text,
                                                       edtText1.Text,
                                                       edtText2.Text,
                                                       edtParam.Text,
                                                       edtTitle.Text,
                                                       iCount,
                                                       edtBackgroudImage.Text))
                    {
                        MessageBox.Show("Error Adding Notification", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Notification Added OK", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Error Calling to WS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    
               
                               

            }
            else
            {
                MessageBox.Show("Usermame can't be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAndroid_Click(object sender, EventArgs e)
        {
            if (edtUsername.Text.Length > 0)
            {              
                try
                {

                    NoficationWS.integraExternalServices oWS = new NoficationWS.integraExternalServices();
                    oWS.Credentials = new System.Net.NetworkCredential("integraMobile", "$%&MiLR(=!");

                    if (!oWS.SendPushAndroidNotificationsTo(edtUsername.Text,
                                                       edtAndroidRawData.Text))
                    {
                        MessageBox.Show("Error Adding Notification", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Notification Added OK", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Error Calling to WS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }




            }
            else
            {
                MessageBox.Show("Usermame can't be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
