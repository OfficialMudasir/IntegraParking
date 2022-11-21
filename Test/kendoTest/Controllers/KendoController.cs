using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.IO;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using NPOI.HSSF.UserModel;
using iTextSharp.text;
using iTextSharp.text.pdf;

using kendoTest.Properties;
using kendoTest.Models;
using kendoTest.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace kendoTest.Controllers
{
    [HandleError]
    [NoCache]
    public class KendoController : Controller
    {

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;
        private SQLDataRepository dataRepository;

        public KendoController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, SQLDataRepository dataRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
            this.dataRepository = dataRepository;            
        }

        #region Operations

        #region Actions

        [Authorize]
        public ActionResult Operations()
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                PopulateCurrencies();
                return View(GetUserOperations(ref oUser, null, null, null, null));
            }
            else
                return RedirectToAction("LogOff", "Home");
        }
        [Authorize]
        public ActionResult UserOperations_Read([DataSourceRequest] DataSourceRequest request)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
                return Json(GetUserOperations(ref oUser, null, null, null, null).ToDataSourceResult(request));
            else
                return RedirectToAction("LogOff", "Home");
        }

        [Authorize]
        public ActionResult Filter_Types()
        {

            IEnumerable<SelectListItem> types = new SelectListItem[] 
            {
                new SelectListItem
                                   {
                                       Text = Resources.Account_Op_Type_Parking,
                                       Value = Convert.ToInt32(ChargeOperationsType.ParkingOperation).ToString()/*,
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ParkingOperation))*/
                                   },
                new SelectListItem
                                   {
                                       Text = Resources.Account_Op_Type_Extension,
                                       Value = Convert.ToInt32(ChargeOperationsType.ExtensionOperation).ToString()/*,
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ExtensionOperation))*/
                                   },
                new SelectListItem
                                   {
                                       Text = Resources.Account_Op_Type_Refund,
                                       Value = Convert.ToInt32(ChargeOperationsType.ParkingRefund).ToString()/*,
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ParkingRefund))*/
                                   },
                new SelectListItem
                                    {
                                        Text = Resources.Account_Op_Type_TicketPayment,
                                        Value = Convert.ToInt32(ChargeOperationsType.TicketPayment).ToString()/*,
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.TicketPayment))*/
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_Recharge,
                                        Value = Convert.ToInt32(ChargeOperationsType.BalanceRecharge).ToString()/*,
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.BalanceRecharge))*/
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_ServiceCharge,
                                        Value = Convert.ToInt32(ChargeOperationsType.ServiceCharge).ToString()/*,
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ServiceCharge))*/
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_Discount,
                                        Value = Convert.ToInt32(ChargeOperationsType.Discount).ToString()/*,
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.Discount))*/
                                    }
            
            };

            return Json(types, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public ActionResult Filter_Plates()
        {
            IEnumerable<SelectListItem> plates = new List<SelectListItem>();
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                plates = oUser.USER_PLATEs.Where(a => a.USRP_ENABLED == 1).
                                Select(a =>
                                   new SelectListItem
                                   {
                                       Text = a.USRP_PLATE,
                                       Value = a.USRP_ID.ToString()/*,
                                   Selected = (a.USRP_ID == selectedPlate)*/
                                   })
                                   .OrderBy(e => e.Text)
                                   .ToList();
            }
            return Json(plates, JsonRequestBehavior.AllowGet);
            /*var dataContext = new NorthwindDataContext();
            var categories = dataContext.Categories
                        .Select(c => new ClientCategoryViewModel
                        {
                            CategoryID = c.CategoryID,
                            CategoryName = c.CategoryName
                        })
                        .OrderBy(e => e.CategoryName);

            return Json(categories, JsonRequestBehavior.AllowGet);*/
        }

        [Authorize]
        public FileResult Export([DataSourceRequest]DataSourceRequest request, string columns, string format)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                //Get the data representing the current grid state - page, sort and filter
                IEnumerable operations = GetUserOperations(ref oUser, null, null, null, null).ToDataSourceResult(request).Data;

                string[] arrColumns = columns.Split(',');

                MemoryStream output = new MemoryStream();
                string sContentType = "";
                string sFileName = "";

                switch (format)
                {
                    case "xls":
                        ExportExcel(operations, arrColumns, output);
                        sContentType = "application/vnd.ms-excel";
                        sFileName = "Operations.xls";
                        break;
                    case "pdf":
                        ExportPdf(operations, arrColumns, output);
                        sContentType = "application/pdf";
                        sFileName = "Operations.pdf";
                        break;

                }

                //Return the result to the end user
                return File(output.ToArray(), sContentType, sFileName);
            }
            else
            {
                MemoryStream output = new MemoryStream();
                return File(output.ToArray(),   //The binary data of the XLS file
                    "application/vnd.ms-excel", //MIME type of Excel files
                    "GridExcelExport.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user                
            }

        }

        [Authorize]
        public ActionResult OperationsExt()
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                PopulateChargeOperationTypes();
                PopulateUsers(oUser);
                PopulateCurrencies();
                PopulateGroups();
                PopulateTariffs();
                PopulateerviceChargeTypes();
                return View(GetUserOperationsExt(ref oUser));
            }
            else
                return RedirectToAction("LogOff", "Home");
        }
        [Authorize]
        public ActionResult UserOperationsExt_Read([DataSourceRequest] DataSourceRequest request)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
                return Json(GetUserOperationsExt(ref oUser).ToDataSourceResult(request));
            else
                return RedirectToAction("LogOff", "Home");
        }

        #endregion

        #region Methods

        private static IEnumerable<OperationRowModel> GetUserOperations(ref USER user)
        {
            IEnumerable<OperationRowModel> res = null;

            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                decimal userId = user.USR_ID;
                res = dbContext.ALL_OPERATIONs.Where(r => r.OPE_USR_ID == userId).Select(domOp => new OperationRowModel
                {
                    TypeId = domOp.OPE_TYPE,
                    Type = GetOperationStringType(domOp.OPE_TYPE),
                    Installation = domOp.INS_DESCRIPTION,
                    Date = domOp.OPE_DATE,
                    DateIni = domOp.OPE_INIDATE,
                    DateEnd = domOp.OPE_ENDDATE,
                    Amount = Convert.ToDouble(domOp.OPE_AMOUNT),
                    CurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                    Time = domOp.OPE_TIME,
                    ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                    PlateId = Convert.ToInt32(domOp.USRP_ID),
                    Plate = domOp.USRP_PLATE,
                    TicketNumber = domOp.TIPA_TICKET_NUMBER,
                    TicketData = domOp.TIPA_TICKET_DATA
                });


            }
            catch (Exception e)
            {
                //m_Log.LogMessage(LogLevels.logERROR, "GetUserOperations: ", e);
            }

            return res;
        }

        private IQueryable<OperationModel> GetUserOperations(ref USER oUser,
                                                                int? Type,
                                                                DateTime? DateIni,
                                                                DateTime? DateEnd,
                                                                int? Plate)
        {

            var predicate = PredicateBuilder.True<ALL_OPERATION>();


            if (Type.HasValue)
            {
                predicate = predicate.And(a => a.OPE_TYPE == Type);
            }

            if (DateIni.HasValue)
            {
                predicate = predicate.And(a => a.OPE_DATE >= DateIni);
            }

            if (DateEnd.HasValue)
            {
                predicate = predicate.And(a => a.OPE_DATE <= DateEnd);
            }

            if (Plate.HasValue)
            {
                predicate = predicate.And(a => a.USRP_ID == Plate);
            }

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            var modelOps = from domOp in dataRepository.GetUserOperations(ref oUser, predicate)
                           select new OperationModel
                           {
                               TypeId = domOp.OPE_TYPE, 
                               Type = (ChargeOperationsType)domOp.OPE_TYPE /*GetOperationStringType(domOp.OPE_TYPE)*/,
                               Installation = domOp.INS_DESCRIPTION,
                               Date = domOp.OPE_DATE,
                               DateIni = domOp.OPE_INIDATE,
                               DateEnd = domOp.OPE_ENDDATE,
                               Amount = Convert.ToDouble(domOp.OPE_AMOUNT / 100.0),
                               AmountFormat = string.Format(provider, "{0:0.00} {1}", domOp.OPE_AMOUNT / 100.0, domOp.OPE_AMOUNT_CUR_ISO_CODE),
                               AmountCurrencyId = domOp.OPE_AMOUNT_CUR_ID,
                               CurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                               Time = domOp.OPE_TIME,
                               ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                               PlateId = Convert.ToInt32(domOp.USRP_ID),
                               Plate = domOp.USRP_PLATE,
                               TicketNumber = domOp.TIPA_TICKET_NUMBER,
                               TicketData = domOp.TIPA_TICKET_DATA,
                               Sector = domOp.GRP_DESCRIPTION,
                               Tariff = domOp.TAR_DESCRIPTION
                           };

            return modelOps;
        }

        private IQueryable<OperationExtModel> GetUserOperationsExt(ref USER oUser)
        {

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            var modelOps = from domOp in dataRepository.GetUserOperationsExt(ref oUser, predicate)
                           select new OperationExtModel
                           {
                               TypeId = domOp.OPE_TYPE,
                               Type = (ChargeOperationsType)domOp.OPE_TYPE /*GetOperationStringType(domOp.OPE_TYPE)*/,
                               UserId = domOp.OPE_USR_ID,
                               InstallationId = domOp.OPE_INS_ID,
                               Installation = domOp.INS_DESCRIPTION,
                               InstallationShortDesc = domOp.INS_SHORTDESC,                               
                               Date = domOp.OPE_DATE,
                               DateIni = domOp.OPE_INIDATE,
                               DateEnd = domOp.OPE_ENDDATE,
                               Amount = Convert.ToDouble(domOp.OPE_AMOUNT / 100.0),
                               AmountCurrencyId = domOp.OPE_AMOUNT_CUR_ID,
                               AmountCurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                               AmountFinal = Convert.ToDouble(domOp.OPE_FINAL_AMOUNT / 100.0),
                               Time = domOp.OPE_TIME,
                               BalanceBefore = Convert.ToDouble(domOp.CUSPMR_BALANCE_BEFORE / 100.0),
                               BalanceCurrencyId = domOp.OPE_BALANCE_CUR_ID,
                               BalanceCurrencyIsoCode = domOp.OPE_BALANCE_CUR_ISO_CODE,
                               ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                               PlateId = Convert.ToInt32(domOp.USRP_ID),
                               Plate = domOp.USRP_PLATE,
                               TicketNumber = domOp.TIPA_TICKET_NUMBER,
                               TicketData = domOp.TIPA_TICKET_DATA,
                               SectorId = domOp.GRP_ID,
                               TariffId = domOp.TAR_ID,
                               SuscriptionType = domOp.OPE_SUSCRIPTION_TYPE,                               
                               InsertionUTCDate = domOp.OPE_INSERTION_UTC_DATE,

                                RechargeId = domOp.OPE_CUSPMR_ID,
                                RechargeDate = domOp.CUSPMR_DATE,
                                RechargeAmount =  Convert.ToDouble(domOp.CUSPMR_AMOUNT / 100.0),
                                RechargeAmountCurrencyId = domOp.CUSPMR_CUR_ID,
                                RechargeAmountCurrencyIsoCode = domOp.CUSPMR_AMOUNT_ISO_CODE,
                                RechargeBalanceBefore =  Convert.ToDouble(domOp.CUSPMR_BALANCE_BEFORE / 100.0),
                                RechargeInsertionUTCDate = domOp.CUSPMR_INSERTION_UTC_DATE,

                                DiscountId = domOp.OPE_OPEDIS_ID,
                                DiscountDate = domOp.OPEDIS_DATE,
                                DiscountAmount =  Convert.ToDouble(domOp.OPEDIS_AMOUNT / 100.0),
                                DiscountAmountCurrencyId = domOp.OPEDIS_AMOUNT_CUR_ID,
                                DiscountAmountCurrencyIsoCode = domOp.OPEDIS_AMOUNT_CUR_ISO_CODE,
                                DiscountAmountFinal =  Convert.ToDouble(domOp.OPE_FINAL_AMOUNT / 100.0),
                                DiscountBalanceCurrencyId = domOp.OPEDIS_BALANCE_CUR_ID,
                                DiscountBalanceCurrencyIsoCode = domOp.OPEDIS_BALANCE_CUR_ISO_CODE,
                                DiscountBalanceBefore =  Convert.ToDouble(domOp.OPEDIS_BALANCE_BEFORE / 100.0),
                                DiscountChangeApplied = Convert.ToDouble(domOp.OPEDIS_CHANGE_APPLIED),
                                DiscountInsertionUTCDate = domOp.OPEDIS_INSERTION_UTC_DATE,

                                ServiceChargeTypeId = domOp.SECH_SECHT_ID
                           };

            return modelOps;
        }

        public static string GetOperationStringType(int opType)
        {
            string strRes = "";
            switch ((ChargeOperationsType)opType)
            {
                case ChargeOperationsType.ParkingOperation:
                    strRes = Resources.Account_Op_Type_Parking;
                    break;
                case ChargeOperationsType.ExtensionOperation:
                    strRes = Resources.Account_Op_Type_Extension;
                    break;
                case ChargeOperationsType.ParkingRefund:
                    strRes = Resources.Account_Op_Type_Refund;
                    break;
                case ChargeOperationsType.TicketPayment:
                    strRes = Resources.Account_Op_Type_TicketPayment;
                    break;
                case ChargeOperationsType.BalanceRecharge:
                    strRes = Resources.Account_Op_Type_Recharge;
                    break;
                case ChargeOperationsType.ServiceCharge:
                    strRes = Resources.Account_Op_Type_ServiceCharge;
                    break;
                case ChargeOperationsType.Discount:
                    strRes = Resources.Account_Op_Type_Discount;
                    break;
                default:
                    strRes = "";
                    break;


            }
            return strRes;

        }

        public static string GetTypesEnum()
        {
            //string[] names = Enum.GetNames(typeof(ChargeOperationsType));
            List<string> lstNames = new List<string>();
            lstNames.Add("");
            foreach (var type in Enum.GetValues(typeof(ChargeOperationsType)))
            {
                lstNames.Add(GetOperationStringType((int)type));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(lstNames.ToArray());

            /*string values = "";
            foreach (string name in names)
            {
                values += ",\"" + name + "\"";
            }
            if (values != "") values = values.Substring(1);            
            return System.Web.HttpUtility.JavaScriptStringEncode(values);*/
        }

        private void ExportExcel(IEnumerable operations, string[] columns, MemoryStream output)
        {

            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = ExportExcel_CreateSheet(workbook, columns);            

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (OperationModel operation in operations)
            {
                if (rowNumber >= 0xFFFF)
                {
                    sheet = ExportExcel_CreateSheet(workbook, columns);
                    rowNumber = 1;
                }
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                for (int i = 0; i < columns.Length; i++)
                {
                    string value = "";
                    object obj = operation.GetType().GetProperty(columns[i]).GetValue(operation, null);
                    if (obj != null) value = obj.ToString();
                    row.CreateCell(i).SetCellValue(value);
                }
            }

            //Write the workbook to a memory stream            
            workbook.Write(output);

        }

        private NPOI.SS.UserModel.ISheet ExportExcel_CreateSheet(HSSFWorkbook workbook, string[] columns)
        {            
            var sheet = workbook.CreateSheet();
            
            /*for (int i = 0; i < columns.Length; i++)
            {
                sheet.SetColumnWidth(i, 10 * 256);
            }*/
            
            var headerRow = sheet.CreateRow(0);

            for (int i = 0; i < columns.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(Resources.ResourceManager.GetString("Account_Op_" + columns[i]));
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            return sheet;
        }

        private void ExportPdf(IEnumerable operations, string[] columns, MemoryStream output)
        {            
            Rectangle pageSize = PageSize.A4; 
            if (columns.Length > 5) pageSize = pageSize.Rotate();
            
            var document = new Document(pageSize, 10, 10, 10, 10);
                        
            PdfWriter.GetInstance(document, output);
            
            document.Open();
            
            var numOfColumns = columns.Length;
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;

            dataTable.DefaultCell.BorderWidth = 2;
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;            
            Font hFont = new Font(Font.FontFamily.COURIER, 8, Font.BOLD);
            Font rFont = new Font(Font.FontFamily.COURIER, 8, Font.NORMAL);

            // Adding headers
            for (int i = 0; i < columns.Length; i++)
            {
                dataTable.AddCell(new PdfPCell(new Phrase(Resources.ResourceManager.GetString("Account_Op_" + columns[i]), hFont)));
                //dataTable.AddCell(Resources.ResourceManager.GetString("Account_Op_" + columns[i]));                
            }

            dataTable.HeaderRows = 1;
            dataTable.DefaultCell.BorderWidth = 1;

            long iCount = 0;
            foreach (OperationModel operation in operations)
            {
                foreach (string column in columns)
                {
                    string value = "";
                    object obj = operation.GetType().GetProperty(column).GetValue(operation, null);
                    if (obj != null) value = obj.ToString();
                    dataTable.AddCell(new PdfPCell(new Phrase(value, rFont)));
                }
                iCount++;
            }

            if (iCount == 0)
            {
                dataTable.AddCell("");
                dataTable.AddCell("");
                dataTable.AddCell("");
                dataTable.AddCell("");
            }
            
            document.Add(dataTable);
            
            document.Close();

        }

        private void PopulateChargeOperationTypes()
        {
            var types = (from d in Enum.GetValues(typeof(ChargeOperationsType)).Cast<ChargeOperationsType>()
                         select new ChargeOperationTypeModel
                         {
                             ChargeOperationTypeId = (int) d,
                             Description = GetOperationStringType((int) d)
                         })
                        .OrderBy(e => e.ToString());
            ViewData["chargeOperationTypes"] = types;
            ViewData["defaultChargeOperationTypes"] = types.First();
        }

        private void PopulateUsers(USER oUser)
        {
            var users = from dom in dataRepository.GetUsers(ref oUser)
                         select new UserAdminDataModel
                         {
                             UserID = dom.USR_ID,
                             Username = dom.USR_USERNAME
                         };
            ViewData["users"] = users;
            ViewData["defaultUser"] = users.First();
        }

        private void PopulateGroups()
        {
            var groups = from dom in dataRepository.GetGroups()
                             select new GroupDataModel
                             {
                                 GroupId = dom.GRP_ID,
                                 Description = dom.GRP_DESCRIPTION
                             };            
            ViewData["groups"] = groups;
            ViewData["defaultGroup"] = groups.First();
        }

        private void PopulateTariffs()
        {
            var tariffs = from dom in dataRepository.GetTariffs()
                         select new TariffDataModel
                         {
                             TariffId = dom.TAR_ID,
                             Description = dom.TAR_DESCRIPTION
                         };
            ViewData["tariffs"] = tariffs;
            ViewData["defaultTariff"] = tariffs.First();
        }

        private void PopulateerviceChargeTypes()
        {
            var types = from dom in dataRepository.GetServiceChargeTypes()
                          select new ServiceChargeTypeModel
                          {
                              ServiceChargeId = dom.SECHT_ID,
                              Description = dom.SECHT_DESCRIPCION
                          };
            ViewData["serviceChargeTypes"] = types;
            ViewData["defaultServiceChargeType"] = types.First();
        }

        #endregion

        #endregion

        #region Users
        
        #region Actions

        [Authorize]
        public ActionResult Users()
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                PopulateCountries();
                PopulateCurrencies();
                return View(GetUsers(ref oUser));
            }
            else
                return RedirectToAction("LogOff", "Home");
        }

        [Authorize]
        public ActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
                return Json(GetUsers(ref oUser).ToDataSourceResult(request));
            else
                return RedirectToAction("LogOff", "Home");
        }
        
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Create([DataSourceRequest] DataSourceRequest request, UserAdminDataModel user)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                if (user != null && ModelState.IsValid)
                {
                    if (!SetDomainCustomerAndUserFromUserDataAdminModel(ref oUser, user))
                    {
                        ModelState.AddModelError("customersDomainError", Resources.ErrorsMsg_ErrorAddindInformationToDB);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("customersDomainError", "Sessión inválida!!");
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Update([DataSourceRequest] DataSourceRequest request, UserAdminDataModel user)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                if (user != null && ModelState.IsValid)
                {
                    if (!SetDomainCustomerAndUserFromUserDataAdminModel(ref oUser, user))
                    {
                        ModelState.AddModelError("customersDomainError", Resources.ErrorsMsg_ErrorAddindInformationToDB);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("customersDomainError", "Sessión inválida!!");
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Destroy([DataSourceRequest] DataSourceRequest request, UserAdminDataModel user)
        {
            if (user != null)
            {
                //customersRepository.DeleteUser(user);
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult CountryFlagImage(decimal idCountry)
        {

            var predicate = PredicateBuilder.True<COUNTRy>();
            predicate = predicate.And(a => a.COU_ID == idCountry);

            var countries = from dom in dataRepository.GetCountries(predicate)
                            select new CountryDataModel
                            {
                                CountryID = dom.COU_ID,
                                Description = dom.COU_DESCRIPTION,
                                Code = dom.COU_CODE,
                                TelPrefix = dom.COU_TEL_PREFIX
                            };            
            CountryDataModel country = countries.First();

            string sFlagFilename = "";
            if (country != null) sFlagFilename = String.Format("{0}.gif", country.Code.ToString());
                
            return Json(new[] { sFlagFilename });
        }

        #endregion

        #region Methods

        private IQueryable<UserAdminDataModel> GetUsers(ref USER oUser)
        {

            var modelUsrs = from domUser in dataRepository.GetUsers(ref oUser)
                            select new UserAdminDataModel()
                            {
                                UserID = domUser.USR_ID,

                                CountryID = domUser.USR_COU_ID,
                                Country = new CountryDataModel() {
                                    CountryID = domUser.COUNTRy.COU_ID,
                                    Description = domUser.COUNTRy.COU_DESCRIPTION                                    
                                },

                                Email = domUser.USR_EMAIL,
                                Username = domUser.USR_USERNAME,

                                MainPhoneCountryID = domUser.USR_MAIN_TEL_COUNTRY,
                                MainPhoneCountry = new CountryDataModel() {
                                    CountryID = domUser.COUNTRy1.COU_ID,
                                    Description = domUser.COUNTRy1.COU_DESCRIPTION
                                },
                                MainPhoneNumber = domUser.USR_MAIN_TEL,

                                AlternativePhoneCountryID = domUser.USR_SECUND_TEL_COUNTRY,
                                AlternativePhoneCountry = new CountryDataModel() {
                                    CountryID = domUser.COUNTRy2.COU_ID,
                                    Description = domUser.COUNTRy2.COU_DESCRIPTION
                                },
                                AlternativePhoneNumber = domUser.USR_SECUND_TEL,

                                CurrencyID = domUser.USR_CUR_ID,
                                Currency = new CurrencyDataModel() {
                                    CurrencyID = domUser.CURRENCy.CUR_ID,
                                    Name = domUser.CURRENCy.CUR_NAME
                                },
                       
                                CurrentPassword = "",
                                NewPassword = "",
                                ConfirmNewPassword = ""

                            };

            return modelUsrs;
        }

        private bool SetDomainCustomerAndUserFromUserDataAdminModel(ref USER oUser,
                                                                    UserAdminDataModel model)
        {
            bool bRes = true;

            try
            {
                /*oUser.USR_USERNAME = model.Username;
                oUser.CUSTOMER.CUS_NAME = model.Name;
                oUser.CUSTOMER.CUS_SURNAME1 = model.Surname1;
                oUser.CUSTOMER.CUS_SURNAME2 = model.Surname2;
                oUser.CUSTOMER.CUS_DOC_ID = model.DocId;
                oUser.USR_SECUND_TEL_COUNTRY = Convert.ToDecimal(model.AlternativePhoneNumberPrefix);
                oUser.USR_SECUND_TEL = model.AlternativePhoneNumber;
                oUser.CUSTOMER.CUS_STREET = model.StreetName;
                oUser.CUSTOMER.CUS_STREE_NUMBER = Convert.ToInt32(model.StreetNumber);
                oUser.CUSTOMER.CUS_LEVEL_NUM = (model.LevelInStreetNumber != null &&
                                        model.LevelInStreetNumber.Length > 0) ? Convert.ToInt32(model.LevelInStreetNumber) : (int?)null;
                oUser.CUSTOMER.CUS_DOOR = model.DoorInStreetNumber;
                oUser.CUSTOMER.CUS_LETTER = model.LetterInStreetNumber;
                oUser.CUSTOMER.CUS_STAIR = model.StairInStreetNumber;
                oUser.CUSTOMER.CUS_COU_ID = Convert.ToInt32(model.Country);
                oUser.CUSTOMER.CUS_STATE = model.State;
                oUser.CUSTOMER.CUS_CITY = model.City;
                oUser.CUSTOMER.CUS_ZIPCODE = model.ZipCode;

                IList<string> Plates = new List<string>();

                foreach (SelectListItem item in model.Plates)
                {
                    Plates.Add(item.Text);
                }
                
                bRes = customersRepository.UpdateUser(ref oUser, Plates);*/

            }
            catch (Exception e)
            {
                bRes = false;

            }

            return bRes;
        }

        private void PopulateCountries()
        {
            var countries = from dom in dataRepository.GetCountries()
                             select new CountryDataModel
                             {
                                 CountryID = dom.COU_ID,
                                 Description = dom.COU_DESCRIPTION,
                                 Code = dom.COU_CODE,
                                 TelPrefix = dom.COU_TEL_PREFIX                                 
                             };
            ViewData["countries"] = countries;
            ViewData["defaultCountry"] = countries.First();
        }

        #endregion

        #endregion

        #region Countries

        #region Actions

        //[Authorize]
        public ActionResult Countries()
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null) {
                PopulateCurrencies(); // Si volem utilitzar ViewState per passar les dades a omplir al combo de 'currencies'
                return View(GetCountries());
            }
            else
                return RedirectToAction("LogOff", "Home");
        }

        /*public JsonResult GetCurrencies()
        {
            var currencies = from dom in dataRepository.GetCurrencies()
                             select new CountryCurrencyDataModel
                             {
                                 CurrencyID = dom.CUR_ID,
                                 CurrencyName = dom.CUR_NAME
                             };
            return Json(currencies, JsonRequestBehavior.AllowGet);
        }*/

        //[Authorize]
        public ActionResult Countries_Read([DataSourceRequest] DataSourceRequest request)
        {            
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
                return Json(GetCountries().ToDataSourceResult(request));
            else
                return RedirectToAction("LogOff", "Home");
        }

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Countries_Create([DataSourceRequest] DataSourceRequest request, CountryDataModel country)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                if (country != null && ModelState.IsValid)
                {
                    if (!country.SetDomain(dataRepository))
                    {
                        ModelState.AddModelError("customersDomainError", Resources.ErrorsMsg_ErrorAddindInformationToDB);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("customersDomainError", "Sessión inválida!!");
            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Countries_Update([DataSourceRequest] DataSourceRequest request, CountryDataModel country)
        {
            USER oUser = (USER)Session["USER_DATA"];
            if (oUser != null)
            {
                if (country != null && ModelState.IsValid)
                {
                    if (!country.SetDomain(dataRepository))
                    {
                        ModelState.AddModelError("customersDomainError", Resources.ErrorsMsg_ErrorAddindInformationToDB);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("customersDomainError", "Sessión inválida!!");
            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Countries_Destroy([DataSourceRequest] DataSourceRequest request, CountryDataModel country)
        {
            if (country != null)
            {
                if (!country.SetDomain(dataRepository, true))
                {
                    ModelState.AddModelError("customersDomainError", Resources.ErrorsMsg_ErrorAddindInformationToDB);
                }
            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region methods

        private IQueryable<CountryDataModel> GetCountries()
        {

            var model = from domCountry in dataRepository.GetCountries()
                        select new CountryDataModel()
                        {
                            CountryID = domCountry.COU_ID,
                            Description = domCountry.COU_DESCRIPTION,
                            Code = domCountry.COU_CODE,
                            TelPrefix = domCountry.COU_TEL_PREFIX,
                            Currency = new CurrencyDataModel()
                            {
                                CurrencyID = domCountry.CURRENCy.CUR_ID,
                                Name = domCountry.CURRENCy.CUR_NAME
                            }
                        };

            return model;
        }

        private void PopulateCurrencies()
        {
            var currencies = from dom in dataRepository.GetCurrencies()
                        select new CurrencyDataModel
                        {
                            CurrencyID = dom.CUR_ID,
                            Name = dom.CUR_NAME
                        };            
            ViewData["currencies"] = currencies;
            ViewData["defaultCurrency"] = currencies.First();
        }

        #endregion

        #endregion

        #region Products

        #region Actions

        public ActionResult Products()
        {
            PopulateCategories();
            return View();
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(SessionClientProductRepository.All().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Update([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<ClientProductViewModel> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    product.Category = GetCategory(product.CategoryID);
                    SessionClientProductRepository.Update(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Create([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<ClientProductViewModel> products)
        {
            var results = new List<ClientProductViewModel>();
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    product.Category = GetCategory(product.CategoryID);
                    SessionClientProductRepository.Insert(product);
                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<ClientProductViewModel> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    SessionClientProductRepository.Delete(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Methods

        private ClientCategoryViewModel GetCategory(int categoryID)
        {
            var dataContext = new NorthwindDataContext();
            var category = dataContext.Categories
                        .Where(c => c.CategoryID == categoryID)
                        .Select(c => new ClientCategoryViewModel
                        {
                            CategoryID = c.CategoryID,
                            CategoryName = c.CategoryName
                        }).FirstOrDefault();
            return category;
        }

        private void PopulateCategories()
        {
            var dataContext = new NorthwindDataContext();
            var categories = dataContext.Categories
                        .Select(c => new ClientCategoryViewModel
                        {
                            CategoryID = c.CategoryID,
                            CategoryName = c.CategoryName
                        })
                        .OrderBy(e => e.CategoryName);
            ViewData["categories"] = categories;
            ViewData["defaultCategory"] = categories.First();
        }

        #endregion

        #endregion

    }
}
