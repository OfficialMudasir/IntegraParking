using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Properties;
using backOffice.Models;


namespace backOffice.Controllers
{
    public class InvoiceController : Controller
    {
        private ICustomersRepository customersRepository;
        private IBackOfficeRepository backOfficeRepository;

        public InvoiceController(ICustomersRepository _customersRepository, IBackOfficeRepository _backOfficeRepository)
        {
            this.customersRepository = _customersRepository;
            this.backOfficeRepository = _backOfficeRepository;            
        }

        #region Actions

        public ActionResult Index()
        {
            return RedirectToAction("Invoices");
        }

        public ActionResult Invoices()        
        {
            if (Helper.Helper.MenuOptionEnabled("Invoice#Invoices"))
            {
                //PopulateUsers();
                //PopulateCurrencies();
                backOffice.Helper.Helper.PopulateUsers(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);            
                return View(InvoiceDataModel.List(backOfficeRepository, PredicateBuilder.False<CUSTOMER_INVOICE>(), false));
            }
            else
                return RedirectToAction("BlankPage", "Home");
        }

        public ActionResult Invoices_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<CUSTOMER_INVOICE>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(InvoiceDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        public ActionResult Export(int invoiceId)
        {

            try
            {
                string serverPath = HttpContext.Server.MapPath("~/Content/Invoicing/");

                var predicate = PredicateBuilder.True<CUSTOMER_INVOICE>();
                predicate = predicate.And(a => a.CUSINV_ID == invoiceId);
                IQueryable<CUSTOMER_INVOICE> invoices = backOfficeRepository.GetInvoices(predicate);

                CUSTOMER_INVOICE oInvoice = invoices.First();                
                OPERATOR oOperator = oInvoice.OPERATOR;

                InvoicePdfGenerator pdfGenerator = new InvoicePdfGenerator(serverPath,
                                                            oOperator.OPR_INVOICE_FORMAT_LAST_PAGE_FILE,
                                                            oOperator.OPR_INVOICE_FORMAT_NO_LAST_PAGE_FILE,
                                                            oInvoice.CUSTOMER.CUS_ID.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".pdf");
                pdfGenerator.TestMode = false;

                InvoiceData invData = new InvoiceData();
                invData.CompanyName = oOperator.OPR_NAME_FOR_INVOICE;
                invData.CompanyInfo = Resources.Invoice_PDF_NIF + oOperator.OPR_VAT_NUMBER + "\n" + oOperator.OPR_ADDRESS_FOR_INVOICE.Replace("\\n", "\n");
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



                invData.LabelNIF = Resources.Invoice_PDF_DNI_NIF;
                invData.LabelPost = "";
                invData.LabelDate = Resources.Invoice_PDF_Date;
                invData.LabelRef = Resources.Invoice_PDF_InvoiceNumber;
                invData.LabelContract = "";
                invData.LabelInvoiceNum = "";
                invData.LabelTotalBase = Resources.Invoice_PDF_BaseAmount;
                invData.LabelTotalIVA = Resources.Invoice_PDF_VAT;
                invData.LabelTotal = Resources.Invoice_PDF_Total;
                invData.LabelLineUnits = Resources.Invoice_PDF_Units;
                invData.LabelLineDescription = Resources.Invoice_PDF_Detail;
                invData.LabelLinePrice = Resources.Invoice_PDF_Unit_Amount;
                invData.LabelLineAmount = Resources.Invoice_PDF_Amount;
                invData.LabelFooter = "Domicilio social: Cardenal Marcelo Spinola, 50-52 28016 Madrid. Inscripta en el REgistro Mercantil de Madrid ...";


                foreach (CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge in oInvoice.CUSTOMER_PAYMENT_MEANS_RECHARGEs.OrderBy(r => r.CUSPMR_DATE))
                {
                    if (oRecharge.CUSPMR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                    {
                        invData.AddLine(new InvoiceLineData("1",
                                                             string.Format("{0} {1:dd/MM/yyyy HH:mm}", Resources.ChargeOperationsType_BalanceRecharge, oRecharge.CUSPMR_DATE),
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
                                strType = Resources.ChargeOperationsType_ParkingOperation;
                            }
                            else if (oOper.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation)
                            {
                                strType = Resources.ChargeOperationsType_ExtensionOperation;
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
                                            string.Format("{0} {1} {2} {3:dd/MM/yyyy HH:mm}", Resources.ChargeOperationsType_TicketPayment,
                                                    oTicket.TIPA_TICKET_NUMBER,
                                                    oTicket.INSTALLATION.INS_DESCRIPTION,
                                                    oTicket.TIPA_DATE),
                                            (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                            (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                        }
                        else if (oRecharge.SERVICE_CHARGEs.Count() > 0)
                        {
                            invData.AddLine(new InvoiceLineData("1",
                                            string.Format("{0} {1:dd/MM/yyyy HH:mm}", Resources.ChargeOperationsType_ServiceCharge, oRecharge.SERVICE_CHARGEs.First().SECH_DATE),
                                            (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE,
                                            (Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100).ToString("##########0.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE));

                        }



                    }

                }

                pdfGenerator.Data = invData;

                if (pdfGenerator.generatePdf())
                {
                    Response.Clear();
                    Response.Buffer = false;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-disposition", "attachment; filename=invoice_" + oInvoice.CUSINV_INV_DATE.Value.ToString("yyyyMMdd") + ".pdf");

                    FileInfo oFileInfo = new FileInfo(pdfGenerator.generatedPDFPath());
                    long lfull_size = oFileInfo.Length;
                    oFileInfo = null;

                    Response.AddHeader("Content-length", lfull_size.ToString());
                    Response.WriteFile(pdfGenerator.generatedPDFPath());
                    Response.End();
                    System.IO.File.Delete(pdfGenerator.generatedPDFPath());
                    

                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return View();

        }

        #endregion

        #region  Methods

        private void PopulateUsers()
        {
            var users = UserDataModel.List(backOfficeRepository);
            ViewData["users"] = users;
            ViewData["defaultUser"] = users.First();
        }

        private void PopulateCurrencies()
        {
            var currencies = CurrencyDataModel.List(backOfficeRepository);
            ViewData["currencies"] = currencies;
            ViewData["defaultCurrency"] = currencies.First();
        }

        #endregion

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

    }
}
