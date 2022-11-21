using CSFacturacionBLL.Domain.Concrete;
using CSFacturacionBLL.Infraestructure;
using CSFacturacionBLL.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSFacturacionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonSendInvoices_Click(object sender, EventArgs e)
        {
            this.buttonSendInvoices.Enabled = false;

            Invoice invoice = new Invoice();

            invoice.RefID = "0001REF_ID_" + new Random().Next().ToString();

            invoice.PaymentManner = Invoice.ePaymentManner.UniquePayment;
            invoice.PaymentMethod = Invoice.ePaymentMethod.CreditCard;
            invoice.ExpeditionPlace = "Expedition place";

            invoice.DocumentType = "Integra Document Type";
            invoice.VoucherType = Invoice.eVoucherType.Entrance;

            invoice.TransferredTaxType = Invoice.eTaxType.IVA;
            invoice.TransferredTaxAmmount = 555;
            invoice.TransferredTaxRate = 16;
            invoice.TransferredTaxSubtotal = 555;

            invoice.DetainedTaxType = Invoice.eTaxType.IVA;
            invoice.DetainedTaxAmmount = 666;
            invoice.DetainedTaxSubtotal = 666;

            invoice.TaxVersion = Invoice.eTaxVersion._1_0;

            Emissor emissor = new Emissor();            
            emissor.Name = "Integra";
            emissor.Rfc = "AAA010101AAA";   //AAA010101AAA is the emRFC to use the provider sandbox
            emissor.Regime = "Integra Regime";
            /*
            EmissorAddress emissorAddress = new EmissorAddress();
            emissorAddress.Country = "Integra Country";
            emissorAddress.Locality = "Integra Locality";
            emissorAddress.PostalCode = "12345";
            emissorAddress.State = "Integra State";
            emissorAddress.Street = "Integra street";
            emissor.Address = emissorAddress;            
             */ 
            invoice.Emissor = emissor;            

            
            Receiver receiver = new Receiver();
            receiver.Name = "Buyer name";
            receiver.NIM = "Buyer NIM";            
            receiver.Buyer = "Buyer";
            receiver.CustomerNumber = "1234567";
            receiver.Email = "buyer@buyer.com";
            //receiver.Fax = "0000FAX";
            //receiver.Phone = "0000PHONE";
            receiver.Rfc = "XAXX010101000";
            ReceiverAddress receiverAddress = new ReceiverAddress();
            receiverAddress.Country = "Buyer Country";
            receiverAddress.Locality = "Buyer Locality";            
            receiverAddress.PostalCode = "54321";
            receiverAddress.NumberInterior = "23";
            receiverAddress.NumberExterior = "51";
            receiverAddress.State = "Buyer State";
            receiverAddress.Street = "Buyer street";
            receiverAddress.Colony = "Buyer Colony";
            receiverAddress.Town = "Buyer Town";
            receiver.Address = receiverAddress;            
            //We dont set the receiver because his data MUST be included ONLY when we stamp the ticket
            //invoice.Receiver = receiver;     
 
            invoice.Concepts = new List<Concept>();
            for (int i = 0; i < 3; i++)
            {
                Concept concept = new Concept();
                concept.Ammount = 1000 * (i + 1);
                concept.Description = "Description concept " + i;
                concept.Quantity = i + 1;
                concept.UnitValue = 1;
                concept.Unit = "servicio";

                invoice.Concepts.Add(concept);
            }            

            string invoiceString = invoice.ToString();

            Console.Write(invoiceString);

            /*
            //Write the files  
            try
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(invoiceString);

                File.WriteAllBytes("FilePlain.txt", textBytes);
                //File.WriteAllText("FileBase64.txt", Convert.ToBase64String(textBytes));

                MessageBox.Show("File created successfuly.");
            }
            catch
            {
                MessageBox.Show("Error writing the file.");
            }
            */

            //Call the webservie to upload
            try
            {
                bool uploadIsOk = CSFacturacionAPI.UploadFile(Encoding.UTF8.GetBytes(invoiceString));

                if (!uploadIsOk)
                {
                    MessageBox.Show("File upload error!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error uploading the file:\n" + ex.Message);
            }

            //try to retrieve the uploaded file
            try
            {
                DataTicketResponse response = CSFacturacionAPI.GetTicketData(invoice.RefID);

                if (response != null)
                {
                    MessageBox.Show("Get ticket from server:\n" + response.Data);
                }
                else
                {
                    MessageBox.Show("No response retrieving the ticket!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving the ticket:\n" + ex.Message);
            }

            //try to stamp the ticket
            try
            {
                StringBuilder json = new StringBuilder();
                string jsonTemp = receiver.toJSON();
                json.Append(jsonTemp.Remove(jsonTemp.Length - 1, 1));
                json.Append(",");
                jsonTemp = receiver.Address.toJSON();
                json.Append(jsonTemp.Remove(0, 1));

                //string finalString = System.Web.HttpUtility.HtmlEncode(json.ToString());
                StampTicketResponse response = CSFacturacionAPI.StampTicket(invoice.RefID, json.ToString());

                //if (response != null && !string.IsNullOrEmpty(response.PdfData))
                if (response != null && response.PdfData != null)
                {
                    string fileName = response.FileName + "_stamped.pdf";

                    byte[] decodedBytes = Convert.FromBase64String(response.PdfData);
                    File.WriteAllBytes(fileName, decodedBytes);

                    MessageBox.Show("Stamped file saved with name: " + fileName);
                }
                else
                {
                    MessageBox.Show("No response or file stamping the ticket!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stamping the ticket:\n" + ex.Message);
            }

            //try to verify the ticket
            try
            {
                VerifyTicketResponse response = CSFacturacionAPI.VerifyTicket(Convert.ToDouble(invoice.Total), invoice.RefID, invoice.Emissor.Rfc);

                if (response != null && !string.IsNullOrEmpty(response.PdfData64))
                {
                    string fileName = response.FileName + "_verification.pdf";

                    File.WriteAllBytes(fileName, Encoding.UTF8.GetBytes(response.PdfData64));

                    MessageBox.Show("Verification file saved with name: " + fileName);
                }
                else
                {
                    MessageBox.Show("No response or file verifying the ticket!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error verifying the ticket:\n" + ex.Message);
            }            

            this.buttonSendInvoices.Enabled = true;

        }
    }
}
