using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Infrastructure;
using backOffice.Models;
using backOffice.Helper;
using PIC.Infrastructure.Logging;

namespace PBPPlugin.Controllers
{
    public class InvoiceController : Controller
    {
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(InvoiceController));

        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;

        public InvoiceController()
        {

        }

        public InvoiceController(IBackOfficeRepository _backOfficeRepository, IInfraestructureRepository _infrastructureRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;
            this.infrastructureRepository = _infrastructureRepository;
        }

        public ActionResult Export(int invoiceId)
        {

            try
            {

                var resBundle = ResourceBundle.GetInstance();

                

                var predicate = PredicateBuilder.True<CUSTOMER_INVOICE>();
                predicate = predicate.And(a => a.CUSINV_ID == invoiceId);
                IQueryable<CUSTOMER_INVOICE> invoices = backOfficeRepository.GetInvoices(predicate);

                CUSTOMER_INVOICE oInvoice = invoices.FirstOrDefault();

                if (oInvoice != null)
                {
                    string sServerPath = HttpContext.Server.MapPath(RouteConfig.BasePath + "Content/Invoicing/");
                    string sFileName = oInvoice.CUSTOMER.CUS_ID.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".pdf";

                    string sGeneratedPdfPath = "";

                    switch (oInvoice.CUSINV_INVOICE_VERSION)
                    {
                        case 1:
                            {
                                sGeneratedPdfPath = ExportInvoicePdf(oInvoice, sServerPath, sFileName);
                                break;
                            }

                        case 2:
                            {
                                sGeneratedPdfPath = ExportInvoiceReportPdf(oInvoice, sServerPath, sFileName);
                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(sGeneratedPdfPath))
                    {
                        Response.Clear();
                        Response.Buffer = false;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-disposition", "attachment; filename=invoice_" + oInvoice.CUSINV_INV_DATE.Value.ToString("yyyyMMdd") + ".pdf");

                        FileInfo oFileInfo = new FileInfo(sGeneratedPdfPath);
                        long lfull_size = oFileInfo.Length;
                        oFileInfo = null;

                        Response.AddHeader("Content-length", lfull_size.ToString());
                        Response.WriteFile(sGeneratedPdfPath);
                        Response.End();
                        System.IO.File.Delete(sGeneratedPdfPath);
                    }

                }

            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "Export", "Exception", ex);
                string error = ex.Message;
            }

            return View();

        }

        private string ExportInvoicePdf(CUSTOMER_INVOICE oInvoice, string sServerPath, string sFileName)
        {
            string sRet = "";

            var resBundle = ResourceBundle.GetInstance();
            
            OPERATOR oOperator = oInvoice.OPERATOR;

            InvoicePdfGenerator pdfGenerator = new InvoicePdfGenerator(sServerPath,
                                                        oOperator.OPR_INVOICE_FORMAT_LAST_PAGE_FILE,
                                                        oOperator.OPR_INVOICE_FORMAT_NO_LAST_PAGE_FILE,
                                                        sFileName);
            pdfGenerator.TestMode = false;

            InvoiceData invData = new InvoiceData();
            invData.CompanyName = oOperator.OPR_NAME_FOR_INVOICE;
            invData.CompanyInfo = resBundle.GetString("PBPPlugin", "Invoice_PDF_NIF") + oOperator.OPR_VAT_NUMBER + "\n" + oOperator.OPR_ADDRESS_FOR_INVOICE.Replace("\\n", "\n");
            invData.CustomerName = oInvoice.CUSTOMER.CUS_NAME + " " + oInvoice.CUSTOMER.CUS_SURNAME1 + " " + oInvoice.CUSTOMER.CUS_SURNAME2;
            invData.CustomerInfo = oInvoice.CUSTOMER.CUS_STREET + " " + oInvoice.CUSTOMER.CUS_STREE_NUMBER + " " + oInvoice.CUSTOMER.CUS_LETTER +
                                    " " + oInvoice.CUSTOMER.CUS_LEVEL_NUM + " " + oInvoice.CUSTOMER.CUS_DOOR + "\n" +
                                    oInvoice.CUSTOMER.CUS_ZIPCODE + " " + oInvoice.CUSTOMER.CUS_CITY + "(" + oInvoice.CUSTOMER.COUNTRy.COU_DESCRIPTION + ")";
            invData.NIF = oInvoice.CUSTOMER.CUS_DOC_ID;
            invData.Post = "";
            invData.Date = oInvoice.CUSINV_INV_DATE.Value.ToString("dd/MM/yyyy");
            invData.Ref = string.Format(oOperator.OPR_INVOICE_NUMBER_FORMAT, Convert.ToInt32(oInvoice.CUSINV_INV_NUMBER), oInvoice.CUSINV_INV_DATE);
            invData.Contract = "";
            invData.InvoiceNum = "";
            invData.TotalBase = (Convert.ToDouble(oInvoice.CUSINV_INV_AMOUNT) / 100).ToString("###########.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE;
            invData.TotalIVA = "0 " + oInvoice.CURRENCy.CUR_ISO_CODE;
            invData.Total = invData.TotalBase;



            invData.LabelNIF = resBundle.GetString("PBPPlugin", "Invoice_PDF_DNI_NIF");
            invData.LabelPost = "";
            invData.LabelDate = resBundle.GetString("PBPPlugin", "Invoice_PDF_Date");
            invData.LabelRef = resBundle.GetString("PBPPlugin", "Invoice_PDF_InvoiceNumber");
            invData.LabelContract = "";
            invData.LabelInvoiceNum = "";
            invData.LabelTotalBase = resBundle.GetString("PBPPlugin", "Invoice_PDF_BaseAmount");
            invData.LabelTotalIVA = resBundle.GetString("PBPPlugin", "Invoice_PDF_VAT");
            invData.LabelTotal = resBundle.GetString("PBPPlugin", "Invoice_PDF_Total");
            invData.LabelLineUnits = resBundle.GetString("PBPPlugin", "Invoice_PDF_Units");
            invData.LabelLineDescription = resBundle.GetString("PBPPlugin", "Invoice_PDF_Detail");
            invData.LabelLinePrice = resBundle.GetString("PBPPlugin", "Invoice_PDF_Unit_Amount");
            invData.LabelLineAmount = resBundle.GetString("PBPPlugin", "Invoice_PDF_Amount");
            invData.LabelFooter = "Domicilio social: Cardenal Marcelo Spinola, 50-52 28016 Madrid. Inscripta en el Registro Mercantil de Madrid, Folio 40 - Tomo 3.724. General 2.976, de la Secc. 3ª del Libro de Sociedades Hoja 28.373. N.I.F. A-28/385458";


            foreach (CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge in oInvoice.CUSTOMER_PAYMENT_MEANS_RECHARGEs.OrderBy(r => r.CUSPMR_DATE))
            {
                if (oRecharge.CUSPMR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                {
                    invData.AddLine(new InvoiceLineData("1",
                                                         string.Format("{0} {1:dd/MM/yyyy HH:mm}", resBundle.GetString("PBPPlugin", "ChargeOperationsType_BalanceRecharge"), oRecharge.CUSPMR_DATE),
                                                          (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                                          (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                }
                else
                {

                    if (oRecharge.OPERATIONs.Count() > 0)
                    {
                        OPERATION oOper = oRecharge.OPERATIONs.First();
                        string strType = "";
                        if (oOper.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation)
                        {
                            strType = resBundle.GetString("PBPPlugin", "ChargeOperationsType_ParkingOperation");
                        }
                        else if (oOper.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation)
                        {
                            strType = resBundle.GetString("PBPPlugin", "ChargeOperationsType_ExtensionOperation");
                        }

                        invData.AddLine(new InvoiceLineData("1",
                                        string.Format("{0} {1} {2} {3:dd/MM/yyyy HH:mm} ", strType,
                                        oOper.USER_PLATE.USRP_PLATE,
                                        oOper.INSTALLATION.INS_DESCRIPTION,
                                        oOper.OPE_DATE),
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE));
                    }
                    else if (oRecharge.TICKET_PAYMENTs.Count() > 0)
                    {
                        TICKET_PAYMENT oTicket = oRecharge.TICKET_PAYMENTs.First();

                        invData.AddLine(new InvoiceLineData("1",
                                        string.Format("{0} {1} {2} {3:dd/MM/yyyy HH:mm}", resBundle.GetString("PBPPlugin", "ChargeOperationsType_TicketPayment"),
                                                oTicket.TIPA_TICKET_NUMBER,
                                                oTicket.INSTALLATION.INS_DESCRIPTION,
                                                oTicket.TIPA_DATE),
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                    }
                    else if (oRecharge.SERVICE_CHARGEs.Count() > 0)
                    {
                        invData.AddLine(new InvoiceLineData("1",
                                        string.Format("{0} {1:dd/MM/yyyy HH:mm}", resBundle.GetString("PBPPlugin", "ChargeOperationsType_ServiceCharge"), oRecharge.SERVICE_CHARGEs.First().SECH_DATE),
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                        (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                    }



                }

            }

            pdfGenerator.Data = invData;

            if (pdfGenerator.generatePdf())
            {
                sRet = pdfGenerator.generatedPDFPath();
            }

            return sRet;
        }

        private string ExportInvoiceReportPdf(CUSTOMER_INVOICE oInvoice, string sServerPath, string sFileName)
        {
            integraMobile.Reports.ReportHelper.CurrentPlugin = "PBPPlugin";

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            // set any deviceInfo settings if necessary
            System.Collections.Hashtable deviceInfo =
                new System.Collections.Hashtable();

            Telerik.Reporting.TypeReportSource typeReportSource =
                         new Telerik.Reporting.TypeReportSource();

            // reportName is the Assembly Qualified Name of the report
            Type oReportType = null;
            try
            {
                oReportType = System.Type.GetType(ConfigurationManager.AppSettings["invoiceReportSource"] ?? "integraMobile.Reports.Invoicing.eysa_invoice, integraMobile.Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            }
            catch (Exception ex){ }
            if (oReportType == null) oReportType = typeof(integraMobile.Reports.Invoicing.eysa_invoice);            
            typeReportSource.TypeName = oReportType.AssemblyQualifiedName;
            typeReportSource.Parameters.Add(new Telerik.Reporting.Parameter("InvoiceId", oInvoice.CUSINV_ID));

            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", typeReportSource, deviceInfo);
            
            //string path = System.IO.Path.GetTempPath();
            string sFilePath = System.IO.Path.Combine(sServerPath, sFileName);

            using (System.IO.FileStream fs = new System.IO.FileStream(sFilePath, System.IO.FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }

            return sFilePath;
        }
    }
}
