using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using System.Linq.Dynamic;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Reflection;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NPOI.HSSF.UserModel;
using iTextSharp.text;
using iTextSharp.text.pdf;

//using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Infrastructure;
using backOffice.Infrastructure.Security;
using backOffice.Models;
using backOffice.Helper;
using MaintenancePlugin.Models;

namespace PBPPlugin.Controllers
{
    public class OperationController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;

        public OperationController()
        {

        }

        public OperationController(IBackOfficeRepository _backOfficeRepository, IInfraestructureRepository _infrastructureRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;
            this.infrastructureRepository = _infrastructureRepository;
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OperationExt_Delete(int? typeId, decimal operationId)
        {
            bool bRet = false;
            string sErrorInfo = "";
            bool bErrorAccess = false;

            ResourceBundle resBundle = ResourceBundle.GetInstance();

            if (FormAuthMemberShip.HelperService.RoleAllowed("OPERATIONS_DELETE"))
            {
                int iTypeId = (typeId ?? 0);

                object oObjDeleted = null;
                int iBalanceBefore = 0;
                USER oUser = null;
                OPERATIONS_DISCOUNT oDiscountDeleted = null;
                bool bIsHisOperation = false;

                if ((ChargeOperationsType)iTypeId != ChargeOperationsType.Discount)
                    bRet = backOfficeRepository.DeleteOperation((ChargeOperationsType)iTypeId, operationId, out oObjDeleted, out iBalanceBefore, out oUser, out oDiscountDeleted, out bIsHisOperation, FormAuthMemberShip.HelperService.InstallationsRoleAllowed("OPERATIONS_DELETE"), out bErrorAccess);

                if (bRet)
                {
                    // send notification emails
                    string sEmails = ConfigurationManager.AppSettings["DeleteOperation_NotificationEmails"] ?? "";
                    if (!string.IsNullOrWhiteSpace(sEmails))
                    {
                        List<string> emails = sEmails.Split(';').ToList().Where(email => !string.IsNullOrWhiteSpace(email)).ToList();
                        if (emails.Count > 0)
                        {
                            string sSubject = "";
                            string sBody = "";
                            string sPaymentInfo = "";

                            switch ((ChargeOperationsType)iTypeId)
                            {
                                case ChargeOperationsType.ParkingOperation:
                                case ChargeOperationsType.ExtensionOperation:
                                case ChargeOperationsType.ParkingRefund:

                                    if ((ChargeOperationsType)iTypeId == ChargeOperationsType.ParkingOperation)
                                    {
                                        sSubject = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Parking_EmailHeader");
                                        sBody = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Parking_EmailBody");
                                    }
                                    else if ((ChargeOperationsType)iTypeId == ChargeOperationsType.ExtensionOperation)
                                    {
                                        sSubject = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Extension_EmailHeader");
                                        sBody = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Extension_EmailBody");
                                    }
                                    else
                                    {
                                        sSubject = resBundle.GetString("PBPPlugin", "OperationExt_Delete_UnParking_EmailHeader");
                                        sBody = resBundle.GetString("PBPPlugin", "OperationExt_Delete_UnParking_EmailBody");
                                    }

                                    dynamic oOperation;
                                    if (!bIsHisOperation)
                                        oOperation = (OPERATION)oObjDeleted;
                                    else
                                        oOperation = (HIS_OPERATION)oObjDeleted;

                                    if ((PaymentSuscryptionType)oOperation.OPE_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                    {
                                        sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.OpReference"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_OP_REFERENCE,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.TransactionId"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANSACTION_ID,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.AuthCode"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AUTH_CODE,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardHash"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_HASH,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardReference"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_REFERENCE,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardScheme"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_SCHEME,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.MaskedCardNumber"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_MASKED_CARD_NUMBER,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardExpirationDate"), oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_EXPIRATION_DATE);
                                    }

                                    string sDiscountInfo = "";
                                    if (oDiscountDeleted != null)
                                    {
                                        sDiscountInfo = string.Format(resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_Discount"),
                                                    (oDiscountDeleted.OPEDIS_AMOUNT_CUR_ID == oDiscountDeleted.OPEDIS_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oDiscountDeleted.OPEDIS_AMOUNT) / 100, oDiscountDeleted.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oDiscountDeleted.OPEDIS_AMOUNT) / 100, oDiscountDeleted.CURRENCy.CUR_ISO_CODE,
                                                                                                 Convert.ToDouble(oDiscountDeleted.OPEDIS_FINAL_AMOUNT) / 100,
                                                                                                 oDiscountDeleted.CURRENCy1.CUR_ISO_CODE)));
                                    }

                                    sBody = string.Format(sBody,
                                                oOperation.OPE_ID,
                                                oOperation.USER_PLATE.USRP_PLATE,
                                                oOperation.INSTALLATION.INS_DESCRIPTION,
                                                (oOperation.GROUP != null ? oOperation.GROUP.GRP_DESCRIPTION : ""),
                                                (oOperation.TARIFF != null ? oOperation.TARIFF.TAR_DESCRIPTION : ""),
                                                oOperation.OPE_DATE,
                                                oOperation.OPE_INIDATE,
                                                oOperation.OPE_ENDDATE,
                                                (oOperation.OPE_AMOUNT_CUR_ID == oOperation.OPE_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oOperation.OPE_AMOUNT) / 100, oOperation.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oOperation.OPE_AMOUNT) / 100, oOperation.CURRENCy.CUR_ISO_CODE,
                                                                                                 Convert.ToDouble(oOperation.OPE_FINAL_AMOUNT) / 100,
                                                                                                 oOperation.CURRENCy1.CUR_ISO_CODE)),
                                                oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceBefore"),
                                                                                      Convert.ToDouble(iBalanceBefore) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "",
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceAfter"),
                                                                                      Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "",
                                                sPaymentInfo,
                                                sDiscountInfo);
                                    break;

                                case ChargeOperationsType.TicketPayment:

                                    sSubject = resBundle.GetString("PBPPlugin", "OperationExt_Delete_TicketPayment_EmailHeader");
                                    sBody = resBundle.GetString("PBPPlugin", "OperationExt_Delete_TicketPayment_EmailBody");

                                    TICKET_PAYMENT oTicketPayment = (TICKET_PAYMENT)oObjDeleted;

                                    sBody = string.Format(sBody,
                                                oTicketPayment.TIPA_ID,
                                                oTicketPayment.TIPA_TICKET_NUMBER,
                                                oTicketPayment.TIPA_PLATE_STRING,
                                                oTicketPayment.INSTALLATION.INS_DESCRIPTION,
                                                oTicketPayment.TIPA_DATE,
                                                oTicketPayment.TIPA_TICKET_DATA,
                                                (oTicketPayment.TIPA_AMOUNT_CUR_ID == oTicketPayment.TIPA_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oTicketPayment.TIPA_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oTicketPayment.TIPA_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE,
                                                                                                Convert.ToDouble(oTicketPayment.TIPA_FINAL_AMOUNT) / 100, oTicketPayment.CURRENCy1.CUR_ISO_CODE)),
                                                oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceBefore"),
                                                                                      Convert.ToDouble(iBalanceBefore) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                        "",
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceAfter"),
                                                                                      Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "");

                                    break;

                                case ChargeOperationsType.BalanceRecharge:

                                    sSubject = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Recharge_EmailHeader");
                                    sBody = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Recharge_EmailBody");

                                    CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = (CUSTOMER_PAYMENT_MEANS_RECHARGE)oObjDeleted;

                                    sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.OpReference"), oRecharge.CUSPMR_OP_REFERENCE,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.TransactionId"), oRecharge.CUSPMR_TRANSACTION_ID,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.AuthCode"), oRecharge.CUSPMR_AUTH_CODE,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardHash"), oRecharge.CUSPMR_CARD_HASH,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardReference"), oRecharge.CUSPMR_CARD_REFERENCE,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardScheme"), oRecharge.CUSPMR_CARD_SCHEME,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.MaskedCardNumber"), oRecharge.CUSPMR_MASKED_CARD_NUMBER,
                                                                 resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardExpirationDate"), oRecharge.CUSPMR_CARD_EXPIRATION_DATE);

                                    sBody = string.Format(sBody,
                                                oRecharge.CUSPMR_ID,
                                                oRecharge.CUSPMR_DATE,
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100,
                                                                              infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                                string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceBefore"),
                                                                                  Convert.ToDouble(iBalanceBefore) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceAfter"),
                                                                                  Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                sPaymentInfo);

                                    break;

                                case ChargeOperationsType.ServiceCharge:

                                    sSubject = resBundle.GetString("PBPPlugin", "OperationExt_Delete_ServiceCharge_EmailHeader");
                                    sBody = resBundle.GetString("PBPPlugin", "Fields.AllOperationsExts.OperationExt_Delete_ServiceCharge_EmailBody");

                                    SERVICE_CHARGE oServiceCharge = (SERVICE_CHARGE)oObjDeleted;

                                    if ((PaymentSuscryptionType)oServiceCharge.SECH_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                    {
                                        sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.OpReference"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_OP_REFERENCE,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.TransactionId"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANSACTION_ID,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.AuthCode"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AUTH_CODE,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardHash"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_HASH,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardReference"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_REFERENCE,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardScheme"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_SCHEME,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.MaskedCardNumber"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_MASKED_CARD_NUMBER,
                                                                     resBundle.GetString("Maintenance", "Fields.AllOperationsExts.CardExpirationDate"), oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_EXPIRATION_DATE);
                                    }

                                    sBody = string.Format(sBody,
                                                oServiceCharge.SECH_ID,
                                                oServiceCharge.SECH_DATE,
                                                (oServiceCharge.SECH_AMOUNT_CUR_ID == oServiceCharge.SECH_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oServiceCharge.SECH_AMOUNT) / 100, oServiceCharge.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oServiceCharge.SECH_AMOUNT) / 100, oServiceCharge.CURRENCy.CUR_ISO_CODE,
                                                                                                Convert.ToDouble(oServiceCharge.SECH_FINAL_AMOUNT) / 100, oServiceCharge.CURRENCy1.CUR_ISO_CODE)),
                                                string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceBefore"),
                                                                                  Convert.ToDouble(iBalanceBefore) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                oServiceCharge.SECH_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", resBundle.GetString("PBPPlugin", "OperationExt_Deleted_EmailBody_BalanceAfter"),
                                                                                      Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "",
                                                sPaymentInfo);

                                    break;

                                case ChargeOperationsType.Discount:


                                    break;

                            }
                            infrastructureRepository.SendEmailToMultiRecipients(emails, sSubject, sBody, integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                        }
                    }
                }
                else
                {
                    sErrorInfo = resBundle.GetString("PBPPlugin", "OperationExt_Delete_Error");
                }                
            }
            else
                bErrorAccess = true;
            
            if (bErrorAccess) sErrorInfo = resBundle.GetString("PBPPlugin", "OperationController.OperationDelete.AccessDenied");

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo }, JsonRequestBehavior.AllowGet);
        }

        public FileResult Export([DataSourceRequest]DataSourceRequest request, string modelName, string columns, string format)
        {
            MaintenancePlugin.Models.MaintenanceDataModel model = MaintenancePlugin.Models.MaintenanceFactory.GetInstance(modelName);

            IEnumerable rows = null;
            /*switch (model)
            {
                case "OperationsExt":
                    rows = OperationExtDataModel.List(backOfficeRepository).ToDataSourceResult(request).Data;
                    break;
            }*/
            // Invoke static method 'List' from DataModel class
            /*Type modelType = Type.GetType(String.Format("backOffice.Models.{0}DataModel", model));            
            MethodInfo info = modelType.GetMethod("List", new Type[] { typeof(IBackOfficeRepository) });
            IQueryable value = (IQueryable)info.Invoke(null, new object[] { backOfficeRepository });*/

            IQueryable value;
            if (modelName.ToUpper() == ("AllCurrOperationsExts").ToUpper())
            {
                value = AllCurrOperationsExtDataModel.List(backOfficeRepository, null);
            }
            else if (modelName.ToUpper() == ("AllOperationsExtNorecharges").ToUpper()) {
                var predicate = integraMobile.Domain.Helper.PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                predicate = predicate.And(a => a.OPE_TYPE != 5);
                value = AllOperationsExtDataModel.List(backOfficeRepository, predicate);
            }
            else {
                value = AllOperationsExtDataModel.List(backOfficeRepository, null);
            }

            if (!string.IsNullOrWhiteSpace(model.InsFilter))
            {
                List<int> oInstallationsAllowed = model.InstallationsRead;

                Kendo.Mvc.CompositeFilterDescriptor oInsFilter = new Kendo.Mvc.CompositeFilterDescriptor();
                oInsFilter.LogicalOperator = Kendo.Mvc.FilterCompositionLogicalOperator.Or;
                if (oInstallationsAllowed.Count > 0)
                {
                    foreach (int iInstallationId in oInstallationsAllowed)
                    {
                        oInsFilter.FilterDescriptors.Add(new Kendo.Mvc.FilterDescriptor("InstallationId", Kendo.Mvc.FilterOperator.IsEqualTo, iInstallationId));
                    }
                    if (model.InsFilterNullable) oInsFilter.FilterDescriptors.Add(new Kendo.Mvc.FilterDescriptor("InstallationId", Kendo.Mvc.FilterOperator.IsEqualTo, null));
                }
                else
                    oInsFilter.FilterDescriptors.Add(new Kendo.Mvc.FilterDescriptor("InstallationId", Kendo.Mvc.FilterOperator.IsEqualTo, 0));

                if (request.Filters == null) request.Filters = new List<Kendo.Mvc.IFilterDescriptor>();
                request.Filters.Add(oInsFilter);
            }

            rows = value.ToDataSourceResult(request).Data;

            string[] arrColumns = columns.Split(',');

            MemoryStream output = new MemoryStream();
            string sContentType = "";
            string sFileName = "";

            ResourceBundle resBundle = ResourceBundle.GetInstance(model.Definition.ResourcesAssemblyName);
            
            switch (format)
            {
                case "xls":
                    ExportExcel(model, rows, arrColumns, output);
                    sContentType = "application/vnd.ms-excel";                    
                    sFileName = resBundle.GetString("Maintenance", String.Format("Fields.{0}.Export.XLSFilename", model.Name)) + ".xls";
                    break;
                case "pdf":
                    ExportPdf(model, rows, arrColumns, output);
                    sContentType = "application/pdf";
                    sFileName = resBundle.GetString("Maintenance", String.Format("Fields.{0}.Export.PDFFilename", model.Name)) + ".pdf";                                        
                    break;
            }

            //Return the result to the end user
            return File(output.ToArray(), sContentType, sFileName);
        }

        private void ExportExcel(MaintenanceDataModel model, IEnumerable rows, string[] columns, MemoryStream output)
        {
            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            List<string> oColumns = new List<string>();
            MaintenanceFieldDataModel oField = null;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    oField = model.Fields.Where(f => f.Mapping == columns[i]).FirstOrDefault();
                    if (oField.Type != MaintenanceFieldDataType.Template)
                    {
                        oColumns.Add(columns[i]);
                    }                    
                }
            }

            //Create new Excel sheet
            var sheet = ExportExcel_CreateSheet(workbook, model, oColumns.ToArray());

            var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;

            int rowNumber = 1;

            Dictionary<string, NPOI.SS.UserModel.ICellStyle> oCurrencyCellStyles = new Dictionary<string, NPOI.SS.UserModel.ICellStyle>();
            Dictionary<string, MaintenanceFieldDataModel> oFieldDataModels = new Dictionary<string, MaintenanceFieldDataModel>();
            bool bIsFK = false;

            //Populate the sheet with values from the grid data
            foreach (object row in rows)
            {
                if (rowNumber >= 0xFFFF)
                {
                    sheet = ExportExcel_CreateSheet(workbook, model, oColumns.ToArray());
                    rowNumber = 1;
                }
                //Create a new row
                var sheetRow = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                int i = 0;                
                foreach (string column in oColumns)
                {
                    if (column != "")
                    {
                        if (!oFieldDataModels.ContainsKey(column))
                        {
                            oFieldDataModels.Add(column, model.Fields.Where(f => f.Mapping == column && f.Type != MaintenanceFieldDataType.Template).FirstOrDefault());
                        }
                        oField = oFieldDataModels[column];
                        if (oField != null)
                        {

                            string value = "";
                            string value2 = "";
                            PropertyInfo propInfo = row.GetType().GetProperty(column + "_FK");
                            if (propInfo == null)
                            {
                                propInfo = row.GetType().GetProperty(column);
                                bIsFK = false;
                            }
                            else
                                bIsFK = true;
                            object obj = propInfo.GetValue(row, null);
                            if (obj != null)
                            {
                                //value = obj.ToString();
                                if (propInfo.PropertyType != typeof(DateTime) && propInfo.PropertyType != typeof(DateTime?))
                                    value = obj.ToString();
                                else
                                {
                                    value = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortDatePattern);
                                    value2 = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortTimePattern);
                                }
                            }

                            NPOI.SS.UserModel.ICell oCell = sheetRow.CreateCell(i);
                            //sheetRow.CreateCell(i).SetCellValue(value);

                            if (!bIsFK)
                            {                                
                                switch (oField.Definition.Type)
                                {
                                    case MaintenanceFieldDataType.Integer:
                                        {
                                            oCell.SetCellType(NPOI.SS.UserModel.CellType.NUMERIC);                                            
                                            if (obj != null)
                                            {
                                                int iValue = Convert.ToInt32(obj);
                                                CurrencyInfo oCurrencyInfo = oField.Definition.GetCurrencyInfo(row, MaintenanceDataObjectType.Entity);
                                                if (oCurrencyInfo != null)
                                                {                                                    
                                                    string sCellFormat = "#,###,##0.00";
                                                    if (oCurrencyInfo.Symbol.Trim().Length <= 2)
                                                        sCellFormat = oCurrencyInfo.ApplySymbol(sCellFormat);
                                                    if (!oCurrencyCellStyles.ContainsKey(sCellFormat))
                                                    {
                                                        var currencyCellStyle = workbook.CreateCellStyle();
                                                        currencyCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
                                                        var formatId = HSSFDataFormat.GetBuiltinFormat(sCellFormat);
                                                        if (formatId == -1)
                                                        {
                                                            var newDataFormat = workbook.CreateDataFormat();
                                                            currencyCellStyle.DataFormat = newDataFormat.GetFormat(sCellFormat);
                                                        }
                                                        else
                                                            currencyCellStyle.DataFormat = formatId;
                                                        oCurrencyCellStyles.Add(sCellFormat, currencyCellStyle);
                                                    }
                                                    oCell.CellStyle = oCurrencyCellStyles[sCellFormat];
                                                }
                                                oCell.SetCellValue(iValue);
                                            }
                                            break;
                                        }
                                    case MaintenanceFieldDataType.Float:
                                        {
                                            oCell.SetCellType(NPOI.SS.UserModel.CellType.NUMERIC);                                            
                                            if (obj != null)
                                            {
                                                decimal dValue = Convert.ToDecimal(obj);
                                                CurrencyInfo oCurrencyInfo = oField.Definition.GetCurrencyInfo(row, MaintenanceDataObjectType.Entity);
                                                if (oCurrencyInfo != null)
                                                {                                                    
                                                    string sCellFormat = "#,###,##0.00";
                                                    if (oCurrencyInfo.Symbol.Trim().Length <= 2)
                                                        sCellFormat = oCurrencyInfo.ApplySymbol(sCellFormat);
                                                    if (!oCurrencyCellStyles.ContainsKey(sCellFormat))
                                                    {
                                                        var currencyCellStyle = workbook.CreateCellStyle();
                                                        currencyCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
                                                        var formatId = HSSFDataFormat.GetBuiltinFormat(sCellFormat);
                                                        if (formatId == -1)
                                                        {
                                                            var newDataFormat = workbook.CreateDataFormat();
                                                            currencyCellStyle.DataFormat = newDataFormat.GetFormat(sCellFormat);
                                                        }
                                                        else
                                                            currencyCellStyle.DataFormat = formatId;
                                                        oCurrencyCellStyles.Add(sCellFormat, currencyCellStyle);
                                                    }
                                                    oCell.CellStyle = oCurrencyCellStyles[sCellFormat];
                                                }
                                                oCell.SetCellValue(Convert.ToDouble(dValue));
                                            }
                                            break;
                                        }
                                    case MaintenanceFieldDataType.Boolean:
                                        {
                                            oCell.SetCellType(NPOI.SS.UserModel.CellType.BOOLEAN);                                            
                                            if (obj != null)
                                                oCell.SetCellValue(Convert.ToBoolean(obj));
                                            break;
                                        }
                                    default:
                                        {                                            
                                            oCell.SetCellValue(value);
                                            break;
                                        }
                                }

                                if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?))
                                {
                                    i += 1;
                                    sheetRow.CreateCell(i).SetCellValue(value2);
                                }                                
                            }
                            else
                            {
                                oCell.SetCellValue(value);
                            }

                        }
                    }
                    i += 1;
                }
            }

            //Write the workbook to a memory stream            
            workbook.Write(output);

        }

        private NPOI.SS.UserModel.ISheet ExportExcel_CreateSheet(HSSFWorkbook workbook, MaintenanceDataModel model, string[] columns)
        {
            var sheet = workbook.CreateSheet();

            /*for (int i = 0; i < columns.Length; i++)
            {
                sheet.SetColumnWidth(i, 10 * 256);
            }*/

            var headerRow = sheet.CreateRow(0);

            ResourceBundle res = ResourceBundle.GetInstance(model.Definition.ResourcesAssemblyName);

            MaintenanceFieldDataModel oField = null;
            int j = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    oField = model.Fields.Where(f => f.Mapping == columns[i]).FirstOrDefault();
                    headerRow.CreateCell(i+j).SetCellValue(oField.LocalizedName);
                    if (oField.Type == MaintenanceFieldDataType.DateTime)
                    {                        
                        j += 1;
                        headerRow.CreateCell(i + j).SetCellValue(res.GetString("Maintenance", string.Format("Fields.{0}.{1}_Time", oField.Maintenance.Name, oField.Name), oField.Name));
                    }
                }
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            return sheet;
        }

        private void ExportPdf(MaintenanceDataModel model, IEnumerable rows, string[] columns, MemoryStream output)
        {
            Rectangle pageSize = PageSize.A4;
            if (columns.Length > 5) pageSize = pageSize.Rotate();

            var document = new Document(pageSize, 10, 10, 10, 10);

            PdfWriter.GetInstance(document, output);

            document.Open();

            var numOfColumns = columns.Count(e => e != "");
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;

            dataTable.DefaultCell.BorderWidth = 2;
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Font hFont = new Font(Font.FontFamily.COURIER, 8, Font.BOLD);
            Font rFont = new Font(Font.FontFamily.COURIER, 8, Font.NORMAL);

            List<string> oColumns = new List<string>();

            // Adding headers
            MaintenanceFieldDataModel oField = null;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    oField = model.Fields.Where(f => f.Mapping == columns[i]).FirstOrDefault();
                    if (oField.Type != MaintenanceFieldDataType.Template)
                    {
                        dataTable.AddCell(new PdfPCell(new Phrase(oField.LocalizedName, hFont)));
                        oColumns.Add(columns[i]);
                    }
                }
            }

            dataTable.HeaderRows = 1;
            dataTable.DefaultCell.BorderWidth = 1;

            long iCount = 0;
            foreach (object row in rows)
            {
                foreach (string column in oColumns)
                {
                    if (column != "")
                    {
                        string value = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(column + "_FK");
                        if (propInfo == null) propInfo = row.GetType().GetProperty(column);                        
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null) value = obj.ToString();
                        dataTable.AddCell(new PdfPCell(new Phrase(value, rFont)));
                    }
                }
                iCount++;
            }

            if (iCount == 0)
            {
                for (int i = 0; i < columns.Length; i++)
                    if (columns[i] != "") dataTable.AddCell("");
            }

            document.Add(dataTable);

            document.Close();

        }

    }
}
