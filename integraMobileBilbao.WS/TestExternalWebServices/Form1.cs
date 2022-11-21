using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace TestExternalWebServices
{

    public partial class TestExternalWS : Form
    {
        public TestExternalWS()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //com.eysamobile.www.integraExternalServices oWs = new com.eysamobile.www.integraExternalServices();
            integraExternalServicesOverride oWs = new integraExternalServicesOverride();

            oWs.Credentials = new System.Net.NetworkCredential("integraMobile", "$%&MiLR(=!");

            txtOut.Text = oWs.NotifyPlateFine(txtIn.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ServiceReference1.IServiceExternalPayment oPagatelia = new ServiceReference1.ServiceExternalPaymentClient();

            try
            {

                //userLogin:dcapitan@integraparking.com,password:heG8NDpbL6MrUe1,date:164913041115,dateSpecified:True,gps:41.4772986 2.0908356,version:1.0,hash:6CDE428F2ECBB6D3
                //ServiceReference1.GPS oGPS = new ServiceReference1.GPS();
                //oGPS.Latitude = Convert.ToDecimal(41.4772986);
                //oGPS.LatitudeSpecified = true;
                //oGPS.Longitude = Convert.ToDecimal(2.0908356);
                //oGPS.LongitudeSpecified = true;

                //oPagatelia.DoLogin("dcapitan@integraparking.com", "heG8NDpbL6MrUe1", 
            }
            catch(Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
    }
}
