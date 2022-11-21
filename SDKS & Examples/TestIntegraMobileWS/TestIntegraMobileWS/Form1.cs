using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;

namespace TestIntegraMobileWS
{
    public partial class Form1 : Form
    {
        string _HMACKey = "L·hdf1852*=?(}/^3123M(!";
        byte[] _normTripleDesKey = null;
        private const long BIG_PRIME_NUMBER = 2147483647;


        public Form1()
        {
            InitializeComponent();
            
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(_HMACKey);
            _normTripleDesKey = new byte[64];
            int iSum = 0;

            for (int i = 0; i < 64; i++)
            {
                if (i < keyBytes.Length)
                {
                    iSum += keyBytes[i];
                }
                else
                {
                    iSum += i;
                }
                _normTripleDesKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
            oIntegraMobileWS.Credentials = new System.Net.NetworkCredential("integraMobile", "$%&MiLR(=!");

            if (encryptTextBox.Text.Length > 0)
            {
                HMAC_LOCAL.Text = CalculateHash(encryptTextBox.Text);
                HMAC_REMOTE.Text = oIntegraMobileWS.CalculateHash(encryptTextBox.Text);
            }

        }


        private string CalculateHash(string strInput)
        {
            string strRes = "";
            try
            {
                HMACSHA256 hmac = new HMACSHA256(_normTripleDesKey);

                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = hmac.ComputeHash(inputBytes, 0, inputBytes.Length);

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
                strRes = e.Message;

            }


            return strRes;
        }

    }
}
