using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Telerik.Reporting;
using Telerik.ReportViewer.Mvc;

//using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Models;
using backOffice.Infrastructure;
using backOffice.Infrastructure.Security;

namespace PBPPlugin.Controllers
{
    public class ToolsController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;
        private IGeograficAndTariffsRepository geograficAndTariffsRepository;
        private ICustomersRepository customersRepository;

        private ResourceBundle m_oResBundle = ResourceBundle.GetInstance();

        private Object m_oLock = new Object();

        public ToolsController()
        {

        }

        public ToolsController(IBackOfficeRepository _backOfficeRepository, IInfraestructureRepository _infrastructureRepository, IGeograficAndTariffsRepository _geograficAndTariffsRepository, ICustomersRepository _customersRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;
            this.infrastructureRepository = _infrastructureRepository;
            this.geograficAndTariffsRepository = _geograficAndTariffsRepository;
            this.customersRepository = _customersRepository;
        }

        #region Actions

        public ActionResult Index()
        {
            return RedirectToAction("EmailTool");
        }

        [Authorize]
        public ActionResult EmailTool()
        {
            if (FormAuthMemberShip.HelperService.FeatureReadAllowed("EmailTool"))
            {

                EmailToolDataModel model = new EmailToolDataModel(backOfficeRepository);

                var predicate = PredicateBuilder.True<INSTALLATION>();
                predicate = predicate.And(i => i.INS_ENABLED == 1);

                var oInstallations = InstallationDataModel.List(backOfficeRepository, predicate).ToList();
                if (oInstallations.FirstOrDefault() != null)
                    model.InstallationsCount = oInstallations.Count();
                else
                    model.InstallationsCount = 0;

                /*for (int i = 0; i < 2000; i++)
                {
                    model.AddRecipient(string.Format("email{0}@mail.com", i));                
                }*/

                Session["EMAILTOOL_MODEL"] = model;

                backOffice.Helper.Helper.PopulateCountries(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulatePaymentMeanTypes(ViewData);
                backOffice.Helper.Helper.PopulatePaymentMeanSubTypes(ViewData);
                backOffice.Helper.Helper.PopulatePaymentSuscryptionTypes(ViewData, true);
                backOffice.Helper.Helper.PopulateBooleans(ViewData);

                return View("EmailTool2", model);
            }
            else
            {                
                return Redirect(FormAuthMemberShip.HelperService.AccessDeniedUrl());
            }

        }

        [Authorize]
        public ActionResult BalanceTransfersTool()
        {
            if (FormAuthMemberShip.HelperService.FeatureWriteAllowed("BalanceTransfers"))
            {

                BalanceTransferDataModel model = new BalanceTransferDataModel(backOfficeRepository);

                var predicate = PredicateBuilder.True<INSTALLATION>();
                predicate = predicate.And(i => i.INS_ENABLED == 1);

                
                Session["TRANSFERTOOL_MODEL"] = model;

                backOffice.Helper.Helper.PopulateCountries(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulatePaymentMeanTypes(ViewData);
                backOffice.Helper.Helper.PopulatePaymentMeanSubTypes(ViewData);
                backOffice.Helper.Helper.PopulatePaymentSuscryptionTypes(ViewData, true);
                backOffice.Helper.Helper.PopulateBooleans(ViewData);

                return View(model);
            }
            else
            {
                return Redirect(FormAuthMemberShip.HelperService.AccessDeniedUrl());
            }

        }

        public ActionResult Recipients_Read([DataSourceRequest] DataSourceRequest request)
        {
            EmailToolDataModel oModel = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
            if (oModel == null)
                oModel = new EmailToolDataModel();
            else
                oModel.SetRepository(backOfficeRepository);

            return Json(oModel.GetRecipientsModel().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<USER>();
            predicate = predicate.And(u => u.USR_ENABLED == 1);

            if (request.Filters != null)
            {
                decimal installationId = InstallationFilters(request.Filters);
                /*if (installationId == 0)
                {
                    var oInsAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed("EMAILTOOL_READ");
                    if (oInsAllowed.Count == 1) installationId = oInsAllowed[0];
                    if (oInsAllowed.Count > 0)
                    {
                        var predicateIns = PredicateBuilder.True<INSTALLATION>();
                        predicateIns = predicateIns.And(i => oInsAllowed.Contains(Convert.ToInt32(i.INS_ID)));
                        installationId = InstallationDataModel.List(backOfficeRepository, predicateIns).First().InstallationId;
                    }
                }*/
                if (installationId > 0)
                {
                    //predicate = predicate.And(u => u.HIS_OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
                    predicate = predicate.And(u => u.USERS_OPERATIONS_IN_INSTALLATIONs.Select(o => o.UOII_OPE_INS_ID).Contains(installationId));
                }
                else
                {
                    /*var oInsAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed("EMAILTOOL_READ");
                    var predicateIns = PredicateBuilder.True<INSTALLATION>();
                    predicateIns = predicateIns.And(i => oInsAllowed.Contains(Convert.ToInt32(i.INS_ID)) && i.INS_ENABLED == 1);
                    var oInstallations = InstallationDataModel.List(backOfficeRepository, predicateIns).Select(i => i.InstallationId).ToList();

                    var predicateOr = PredicateBuilder.False<USER>();
                    foreach (decimal dInsId in oInstallations)
                    {
                        //predicateOr = predicateOr.Or(u => u.HIS_OPERATIONs.Select(o => o.OPE_INS_ID).Contains(dInsId));
                        predicateOr = predicateOr.Or(u => u.USERS_OPERATIONS_IN_INSTALLATIONs.Select(o => o.UOII_OPE_INS_ID).Contains(dInsId));
                    }
                    predicate = predicate.And(predicateOr);*/
                    predicate = predicate.And(u => u.USERS_OPERATIONS_IN_INSTALLATIONs.Any());

                }
            }

            /*if (request.Filters != null && request.Filters.Count == 1)
            {
                if (request.Filters[0].GetType() == typeof(Kendo.Mvc.FilterDescriptor))
                {
                    Kendo.Mvc.FilterDescriptor oFilter = (Kendo.Mvc.FilterDescriptor)request.Filters[0];
                    if (oFilter.Member == "undefined")
                    {
                        predicate = predicate.And(u => u.OPERATIONs.Select(o => o.OPE_INS_ID).Contains(Convert.ToDecimal(oFilter.Value)));
                        request.Filters.Clear();
                    }
                }
            }*/
            return Json(UserDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Installations_Read(string role = "EMAILTOOL_READ")
        {
            var predicate = PredicateBuilder.True<INSTALLATION>();
            var oInsAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed(role);
            predicate = predicate.And(i => oInsAllowed.Contains(Convert.ToInt32(i.INS_ID)) && i.INS_ENABLED == 1);
            return Json(InstallationDataModel.List(backOfficeRepository, predicate), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRecipients(string recipients, [DataSourceRequest] DataSourceRequest request, int applyFilter, string model = "EMAILTOOL_MODEL")
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                    EmailToolDataModel oModel = (EmailToolDataModel)Session[model];
                    if (oModel == null)
                        oModel = new EmailToolDataModel(backOfficeRepository);
                    else
                        oModel.SetRepository(backOfficeRepository);

                    int iRecipientsCount = 0;
                    EmailToolDataStatus oStatus = oModel.GetStatus(out iRecipientsCount);
                    if (oStatus == EmailToolDataStatus.Idle)
                    {

                        string[] arrRecipients = (string[])integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(recipients, typeof(string[]));
                        //IList<Kendo.Mvc.IFilterDescriptor> oFilters = (IList<Kendo.Mvc.IFilterDescriptor>)Conversions.JsonDeserializeFromString(filters, typeof(IList<Kendo.Mvc.IFilterDescriptor>));
                        //object oFilters = Conversions.JsonDeserializeFromString(filters, typeof(object));

                        /*HttpContext ctx = HttpContext.Current;
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            HttpContext.Current = ctx;
                            worker.DoWork();
                        }));
                        t.Start();
                        // [... do other job ...]
                        t.Join();*/

                        oModel.SetStatus(EmailToolDataStatus.Adding);

                        if (applyFilter != 1)
                        {
                            //model.AddRecipients(arrRecipients);

                            System.Threading.Tasks.Task.Factory.StartNew(() => AddRecipientsExecute(oModel, arrRecipients));

                            //foreach (string sRecipient in arrRecipients)
                            //{
                            //    model.AddRecipient(sRecipient);
                            //}
                        }
                        else
                        {
                            //DataSourceRequest request = new DataSourceRequest();
                            //request.Filters.Add((Kendo.Mvc.IFilterDescriptor) oFilters);
                            /*var predicate = PredicateBuilder.True<USER>();
                            predicate = predicate.And(u => u.USR_ENABLED == 1);
                            if (request.Filters != null)
                            {
                                decimal installationId = InstallationFilters(request.Filters);
                                if (installationId > 0)
                                {
                                    predicate = predicate.And(u => u.OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
                                }
                            }                        
                            var users = UserDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request).Data;
                            List<string> oRecipients = new List<string>();
                            foreach (UserDataModel oUser in users)
                            {
                                oRecipients.Add(oUser.Email);
                                //model.AddRecipient(oUser.Email);
                            }
                            model.AddRecipients(oRecipients.ToArray());*/

                            System.Threading.Tasks.Task.Factory.StartNew(() => AddRecipientsExecute(oModel, request));
                        }

                        Session[model] = oModel;

                        bRet = true;
                    }
                    else
                    {
                        sErrorInfo = "";
                    }

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = ex.StackTrace;
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRecipientsPaste(string recipients)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(recipients))
                {
                    EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                    if (model == null)
                        model = new EmailToolDataModel(backOfficeRepository);
                    else
                        model.SetRepository(backOfficeRepository);

                    int iRecipientsCount = 0;
                    EmailToolDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                    if (oStatus == EmailToolDataStatus.Idle)
                    {

                        //string[] arrRecipients = (string[])Conversions.JsonDeserializeFromString(recipients, typeof(string[]));
                        string sRecipients = (string)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(recipients, typeof(string));
                        string[] arrRecipients = sRecipients.Split(',');
                        if (arrRecipients.Length > 0)
                        {
                            if (!ValidEmail(arrRecipients[0].Trim(), true))
                            {
                                arrRecipients = sRecipients.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                            }
                        }
                        //IList<Kendo.Mvc.IFilterDescriptor> oFilters = (IList<Kendo.Mvc.IFilterDescriptor>)Conversions.JsonDeserializeFromString(filters, typeof(IList<Kendo.Mvc.IFilterDescriptor>));
                        //object oFilters = Conversions.JsonDeserializeFromString(filters, typeof(object));

                        /*foreach (string sRecipient in arrRecipients)
                        {
                            if (ValidEmail(sRecipient.Trim()))
                                model.AddRecipient(sRecipient.Trim());
                        }*/

                        model.SetStatus(EmailToolDataStatus.Adding);

                        System.Threading.Tasks.Task.Factory.StartNew(() => AddRecipientsExecute(model, arrRecipients/*.Where(email => ValidEmail(email.Trim()))*/.ToArray()));
                        //model.AddRecipients(arrRecipients.Where(email => ValidEmail(email.Trim())).ToArray());

                        Session["EMAILTOOL_MODEL"] = model;
                    }
                }
                bRet = true;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo },JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRecipient(string recipient)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                if (model == null)
                    model = new EmailToolDataModel(backOfficeRepository);
                else
                    model.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                EmailToolDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == EmailToolDataStatus.Idle)
                {

                    model.DeleteRecipient(recipient);

                    Session["EMAILTOOL_MODEL"] = model;

                    bRet = true;
                }
                else
                {

                }
                

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllRecipients(string recipient)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                if (model == null)
                    model = new EmailToolDataModel(backOfficeRepository);
                else
                    model.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                EmailToolDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == EmailToolDataStatus.Idle)
                {
                    model.DeleteAllRecipients();

                    Session["EMAILTOOL_MODEL"] = model;

                    bRet = true;
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult SendEmails(string subject, string body, string pwd)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {                
                EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                if (model == null)
                    model = new EmailToolDataModel(backOfficeRepository);
                else
                    model.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                EmailToolDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == EmailToolDataStatus.Idle)
                {
                    string sConfigPwd = (ConfigurationManager.AppSettings["EmailTool_Password"] ?? "");

                    if (sConfigPwd == pwd)
                    {
                        List<string> oRecipients = model.GetRecipients();
                        if (oRecipients.Count() > 0)
                        {
                            model.Subject = subject;
                            model.Body = Server.HtmlDecode(body);

                            /*var oUrlHelper = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
                            string sPath = oUrlHelper.RequestContext.HttpContext.Server.MapPath(RouteConfig.BasePath + "Content/");
                            var rd = new System.IO.StreamReader(System.IO.Path.Combine(sPath, "Eysa_newsletter2.htm"));
                            model.Body = rd.ReadToEnd();
                            rd.Close();*/

                            model.SetStatus(EmailToolDataStatus.Sending);
                            
                            //infrastructureRepository.SendEmailWithAttachmentsToMultiRecipients(oRecipients, model.Subject, model.Body, model.Attachments.ToList(), integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                            System.Threading.Tasks.Task.Factory.StartNew(() => SendEmailsExecute(model, oRecipients, model.Subject, model.Body, model.Attachments.ToList()));

                            //model.DeleteAllAttachments();

                            Session["EMAILTOOL_MODEL"] = model;

                            bRet = true;
                        }
                        else
                        {
                            bRet = false;
                            sErrorInfo = m_oResBundle.GetString("PBPPlugin", "EmailTool_SendEmails_NoRecipients");
                        }
                    }
                    else
                    {
                        bRet = false;
                        sErrorInfo = m_oResBundle.GetString("PBPPlugin", "EmailTool_SendEmails_InvalidPwd");
                    }
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        public ActionResult SaveAttachment(IEnumerable<HttpPostedFileBase> uploads)
        {
            EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
            if (model == null) model = new EmailToolDataModel();

            string sErrorInfo = "";

            try
            {
                if (uploads != null)
                {
                    foreach (var file in uploads)
                    {                        
                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        var fileName = Path.GetFileName(file.FileName);                        
                        var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), "#" + Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + "." + Path.GetExtension(file.FileName));

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);

                        model.AddAttachment(fileName, physicalPath, file.ContentType);
                    }
                    Session["EMAILTOOL_MODEL"] = model;
                }
            }
            catch (Exception ex)
            {
                sErrorInfo = ex.Message;
            }

            // Return an empty string to signify success
            return Content(sErrorInfo);
        }

        public ActionResult RemoveAttachment(string[] fileNames)
        {
            EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
            if (model == null) model = new EmailToolDataModel();

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    model.DeleteAttachment(fileName);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
                
        [Authorize]
        public ActionResult Reports()
        {


            if (FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.Deposits") ||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.LiquidationDetail") ||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.Bank")||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.Deposits2")||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.Deposits3")||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.GeneralData")||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.GeneralDataInstallation")||
                FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports.RegisteredUsers"))
            {
                /*var cultureInfo = new System.Globalization.CultureInfo("en-US");
                // Set the language for static text (i.e. column headings, titles)
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
                // Set the language for dynamic text (i.e. date, time, money)
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;*/

                /*var oRepository = new integraMobile.Domain.NH.Concrete.SQLBaseRepository(typeof(integraMobile.Domain.NH.Concrete.SQLBaseRepository), true);
                var predicate = PredicateBuilder.True<integraMobile.Domain.NH.Entities.AllOperationsExt>();
                predicate = predicate.And(o => o.UsrpPlate == "1111AAA");
                IQueryable<integraMobile.Domain.NH.Entities.AllOperationsExt> regs = oRepository.GetQuery(typeof(integraMobile.Domain.NH.Entities.AllOperationsExt)).Cast<integraMobile.Domain.NH.Entities.AllOperationsExt>().Where(o => o.UsrpPlate == "1111AAA").AsQueryable();
                var list = regs.ToList();*/

                integraMobile.Reports.ReportHelper.CurrentPlugin = "PBPPlugin";
            }
            else
                return RedirectToAction("AccessDenied", "Account", new { plugin = "SecurityPlugin" });

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAddRecipientsStatus(string uniqueId)
        {
            EmailToolDataStatus oStatus = EmailToolDataStatus.Idle;
            string sErrorInfo = "";
            int iRecipientsCount = 0;

            EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
            if (model != null && model.UniqueId == uniqueId)
            {
                model.SetRepository(backOfficeRepository);
                oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == EmailToolDataStatus.AddingSuccess || oStatus == EmailToolDataStatus.AddingFail)
                    model.SetStatus(EmailToolDataStatus.Idle);
            }

            return Json(new { Result = (oStatus == EmailToolDataStatus.AddingSuccess || oStatus == EmailToolDataStatus.AddingFail || oStatus == EmailToolDataStatus.Idle), 
                              AddingRecipientsResult = (oStatus != EmailToolDataStatus.AddingFail), 
                              RecipientsCount = iRecipientsCount,
                              ErrorInfo = sErrorInfo }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSendEmailsStatus(string uniqueId)
        {
            EmailToolDataStatus oStatus = EmailToolDataStatus.Idle;
            string sErrorInfo = "";
            int iRecipientsCount = 0;

            EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
            if (model != null && model.UniqueId == uniqueId)
            {
                model.SetRepository(backOfficeRepository);
                oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == EmailToolDataStatus.SendSuccess || oStatus == EmailToolDataStatus.SendFail)
                    model.SetStatus(EmailToolDataStatus.Idle);
            }

            return Json(new
            {
                Result = (oStatus == EmailToolDataStatus.SendSuccess || oStatus == EmailToolDataStatus.SendFail || oStatus == EmailToolDataStatus.Idle),
                SendEmailsResult = (oStatus != EmailToolDataStatus.SendFail),
                RecipientsCount = iRecipientsCount,
                ErrorInfo = sErrorInfo
            }, JsonRequestBehavior.AllowGet);
        }

        #region Balance Transfers Actions

        public ActionResult RecipientsTransfer_Read([DataSourceRequest] DataSourceRequest request)
        {
            BalanceTransferDataModel oModel = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
            if (oModel == null)
                oModel = new BalanceTransferDataModel();
            else
                oModel.SetRepository(backOfficeRepository);

            return Json(oModel.GetRecipientsModel().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRecipientsTransfer(string recipients, [DataSourceRequest] DataSourceRequest request, int applyFilter)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                BalanceTransferDataModel oModel = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
                if (oModel == null)
                    oModel = new BalanceTransferDataModel(backOfficeRepository);
                else
                    oModel.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                BalanceTransferDataStatus oStatus = oModel.GetStatus(out iRecipientsCount);
                if (oStatus == BalanceTransferDataStatus.Idle)
                {

                    string[] arrRecipients = (string[])integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(recipients, typeof(string[]));

                    oModel.SetStatus(BalanceTransferDataStatus.Adding);

                    if (applyFilter != 1)
                    {
                        //model.AddRecipients(arrRecipients);

                        System.Threading.Tasks.Task.Factory.StartNew(() => AddRecipientsTransferExecute(oModel, arrRecipients));

                        //foreach (string sRecipient in arrRecipients)
                        //{
                        //    model.AddRecipient(sRecipient);
                        //}
                    }
                    else
                    {
                        //DataSourceRequest request = new DataSourceRequest();
                        //request.Filters.Add((Kendo.Mvc.IFilterDescriptor) oFilters);
                        /*var predicate = PredicateBuilder.True<USER>();
                        predicate = predicate.And(u => u.USR_ENABLED == 1);
                        if (request.Filters != null)
                        {
                            decimal installationId = InstallationFilters(request.Filters);
                            if (installationId > 0)
                            {
                                predicate = predicate.And(u => u.OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
                            }
                        }                        
                        var users = UserDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request).Data;
                        List<string> oRecipients = new List<string>();
                        foreach (UserDataModel oUser in users)
                        {
                            oRecipients.Add(oUser.Email);
                            //model.AddRecipient(oUser.Email);
                        }
                        model.AddRecipients(oRecipients.ToArray());*/

                        System.Threading.Tasks.Task.Factory.StartNew(() => AddRecipientsTransferExecute(oModel, request));
                    }

                    Session["TRANSFERTOOL_MODEL"] = oModel;

                    bRet = true;
                }
                else
                {
                    sErrorInfo = "";
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = ex.StackTrace;
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRecipientsTransferPaste(string recipients)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(recipients))
                {
                    BalanceTransferDataModel model = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
                    if (model == null)
                        model = new BalanceTransferDataModel(backOfficeRepository);
                    else
                        model.SetRepository(backOfficeRepository);

                    int iRecipientsCount = 0;
                    BalanceTransferDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                    if (oStatus == BalanceTransferDataStatus.Idle)
                    {

                        //string[] arrRecipients = (string[])Conversions.JsonDeserializeFromString(recipients, typeof(string[]));
                        string sRecipients = (string)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(recipients, typeof(string));
                        string[] arrRecipients = sRecipients.Split(',');
                        if (arrRecipients.Length > 0)
                        {
                            if (!ValidEmail(arrRecipients[0].Trim(), true))
                            {
                                arrRecipients = sRecipients.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                            }
                        }
                        //IList<Kendo.Mvc.IFilterDescriptor> oFilters = (IList<Kendo.Mvc.IFilterDescriptor>)Conversions.JsonDeserializeFromString(filters, typeof(IList<Kendo.Mvc.IFilterDescriptor>));
                        //object oFilters = Conversions.JsonDeserializeFromString(filters, typeof(object));

                        /*foreach (string sRecipient in arrRecipients)
                        {
                            if (ValidEmail(sRecipient.Trim()))
                                model.AddRecipient(sRecipient.Trim());
                        }*/

                        model.SetStatus(BalanceTransferDataStatus.Adding);

                        System.Threading.Tasks.Task.Factory.StartNew(() => AddRecipientsTransferExecute(model, arrRecipients/*.Where(email => ValidEmail(email.Trim()))*/.ToArray()));
                        //model.AddRecipients(arrRecipients.Where(email => ValidEmail(email.Trim())).ToArray());

                        Session["TRANSFERTOOL_MODEL"] = model;
                    }
                }
                bRet = true;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRecipientTransfer(string recipient)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                BalanceTransferDataModel model = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
                if (model == null)
                    model = new BalanceTransferDataModel(backOfficeRepository);
                else
                    model.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                BalanceTransferDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == BalanceTransferDataStatus.Idle)
                {

                    model.DeleteRecipient(recipient);

                    Session["TRANSFERTOOL_MODEL"] = model;

                    bRet = true;
                }
                else
                {

                }


            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllRecipientsTransfer(string recipient)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                BalanceTransferDataModel model = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
                if (model == null)
                    model = new BalanceTransferDataModel(backOfficeRepository);
                else
                    model.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                BalanceTransferDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == BalanceTransferDataStatus.Idle)
                {
                    model.DeleteAllRecipients();

                    Session["TRANSFERTOOL_MODEL"] = model;

                    bRet = true;
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAddRecipientsTransferStatus(string uniqueId)
        {
            BalanceTransferDataStatus oStatus = BalanceTransferDataStatus.Idle;
            string sErrorInfo = "";
            int iRecipientsCount = 0;

            BalanceTransferDataModel model = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
            if (model != null && model.UniqueId == uniqueId)
            {
                model.SetRepository(backOfficeRepository);
                oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == BalanceTransferDataStatus.AddingSuccess || oStatus == BalanceTransferDataStatus.AddingFail)
                    model.SetStatus(BalanceTransferDataStatus.Idle);
            }

            return Json(new
            {
                Result = (oStatus == BalanceTransferDataStatus.AddingSuccess || oStatus == BalanceTransferDataStatus.AddingFail || oStatus == BalanceTransferDataStatus.Idle),
                AddingRecipientsResult = (oStatus != BalanceTransferDataStatus.AddingFail),
                RecipientsCount = iRecipientsCount,
                ErrorInfo = sErrorInfo
            }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTransferStatus(string uniqueId)
        {
            BalanceTransferDataStatus oStatus = BalanceTransferDataStatus.Idle;
            WS.ResultType oWSStatus = WS.ResultType.Result_OK;
            string sErrorInfo = "";
            int iRecipientsCount = 0;

            BalanceTransferDataModel model = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
            if (model != null && model.UniqueId == uniqueId)
            {
                model.SetRepository(backOfficeRepository);                
                oStatus = model.GetStatus(out iRecipientsCount, out oWSStatus);
                if (oStatus == BalanceTransferDataStatus.TransferSuccess || oStatus == BalanceTransferDataStatus.TransferFail)
                    model.SetStatus(BalanceTransferDataStatus.Idle);
                if (oWSStatus != WS.ResultType.Result_OK)
                {
                    sErrorInfo = m_oResBundle.GetString("PBPPlugin", string.Format("BalanceTransfer.TransferStatus.{0}", oWSStatus.ToString()), oWSStatus.ToString());
                }
            }

            return Json(new
            {
                Result = (oStatus == BalanceTransferDataStatus.TransferSuccess || oStatus == BalanceTransferDataStatus.TransferFail /*|| oStatus == BalanceTransferDataStatus.Idle*/),
                TransferResult = (oStatus != BalanceTransferDataStatus.TransferFail),
                RecipientsCount = iRecipientsCount,
                ErrorInfo = sErrorInfo
            }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Transfer(string sourceEmail, string password, decimal amount)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                BalanceTransferDataModel model = (BalanceTransferDataModel)Session["TRANSFERTOOL_MODEL"];
                if (model == null)
                    model = new BalanceTransferDataModel(backOfficeRepository);
                else
                    model.SetRepository(backOfficeRepository);

                int iRecipientsCount = 0;
                BalanceTransferDataStatus oStatus = model.GetStatus(out iRecipientsCount);
                if (oStatus == BalanceTransferDataStatus.Idle)
                {
                    List<string> oRecipients = model.GetRecipients();
                    if (oRecipients.Count() > 0)
                    {
                        model.SourceEmail = sourceEmail;
                        model.Password = password;
                        model.Amount = Convert.ToInt32(amount);


                        USER oSourceUser = null;

                        if (model.CheckBalanceTransferAmount(out oSourceUser))
                        {
                            model.SetStatus(BalanceTransferDataStatus.Transfering);

                            //infrastructureRepository.SendEmailWithAttachmentsToMultiRecipients(oRecipients, model.Subject, model.Body, model.Attachments.ToList(), integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                            System.Threading.Tasks.Task.Factory.StartNew(() => BalanceTransfersExecute(model, oRecipients));

                            //model.DeleteAllAttachments();

                            Session["TRANSFERTOOL_MODEL"] = model;

                            bRet = true;
                        }
                        else
                        {
                            bRet = false;
                            if (oSourceUser == null)
                                sErrorInfo = m_oResBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.InvalidSourceUser");
                            else
                                sErrorInfo = m_oResBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.NotEnoughBalance");
                        }
                    }
                    else
                    {
                        bRet = false;
                        sErrorInfo = m_oResBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.NoRecipients");
                    }
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        #endregion

        #region Cash Recharge Actions

        [Authorize]
        public ActionResult CashRechargeTool()
        {
            if (FormAuthMemberShip.HelperService.FeatureReadAllowed("CashRechargeTool"))
            {

                CashRechargeDataModel model = new CashRechargeDataModel(this.geograficAndTariffsRepository, this.backOfficeRepository, this.customersRepository);

                //Session["CASHRECHARGETOOL_MODEL"] = model;

                return View(model);
            }
            else
            {
                return Redirect(FormAuthMemberShip.HelperService.AccessDeniedUrl());
            }

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get), ValidateInput(false)]
        public ActionResult CashRechargeSummary(int installationId, string email, decimal totalAmount)
        {
            bool bRet = false;
            string sErrorInfo = "";

            string sAmount = "";
            string sFEE = "";
            string sVAT = "";
            string sTotalAmount = "";

            try
            {

                var oInstallationsAllowed = FormAuthMemberShip.HelperService.InstallationsFeatureAllowed("CashRechargeTool", AccessLevel.Read);
                if (oInstallationsAllowed.Contains(installationId))
                {

                    CashRechargeDataModel oModel = new CashRechargeDataModel(geograficAndTariffsRepository, backOfficeRepository, customersRepository)
                    {
                        InstallationId = installationId,
                        Email = email,
                        TotalAmount = Convert.ToInt32(totalAmount * 100)
                    };

                    if (oModel.OperatorId.HasValue)
                    {
                        USER oUser = oModel.GetUser();
                        if (oUser != null)
                        {
                            int iAmount = 0;
                            int iFEE = 0;
                            int iVAT = 0;

                            bRet = oModel.RechargeSummary(oUser, out iAmount, out iFEE, out iVAT);
                            if (bRet)
                            {
                                sAmount = string.Format((Convert.ToDouble(iAmount) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                                sFEE = string.Format((Convert.ToDouble(iFEE) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                                sVAT = string.Format((Convert.ToDouble(iVAT) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                                sTotalAmount = string.Format((Convert.ToDouble(oModel.TotalAmount) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                            }
                        }
                        else
                        {
                            sErrorInfo = m_oResBundle.GetString("PBPPlugin", "CashRecharge.Recharge.InvalidUser", "Invalid user");
                        }
                    }
                    else
                        sErrorInfo = m_oResBundle.GetString("PBPPlugin", "CashRecharge.Recharge.OperatorRequired", "Finantial operator required");
                }
                else
                    sErrorInfo = FormAuthMemberShip.HelperService.AccessDeniedMessage();

            }
            catch (Exception ex)
            {
                bRet = false;
                // ...
            }


            return Json(new { Result = bRet, ErrorInfo = sErrorInfo, Data = new { Email = email, Amount = sAmount, FEE = sFEE, VAT = sVAT, TotalAmount = sTotalAmount } }, 
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get), ValidateInput(false)]
        public ActionResult CashRechargeSave(int installationId, string email, decimal totalAmount)
        {
            bool bRet = false;
            string sErrorInfo = "";

            string sAmount = "";
            string sFEE = "";
            string sVAT = "";
            string sTotalAmount = "";

            try
            {

                var oInstallationsAllowed = FormAuthMemberShip.HelperService.InstallationsFeatureAllowed("CashRechargeTool", AccessLevel.Read);
                if (oInstallationsAllowed.Contains(installationId))
                {

                    CashRechargeDataModel oModel = new CashRechargeDataModel(geograficAndTariffsRepository, backOfficeRepository, customersRepository)
                    {
                        InstallationId = installationId,
                        Email = email,
                        TotalAmount = Convert.ToInt32(totalAmount * 100)
                    };

                    USER oUser = oModel.GetUser();
                    if (oUser != null)
                    {
                        int iAmount = 0;
                        int iFEE = 0;
                        int iVAT = 0;

                        var oRet = oModel.Recharge(oUser, out iAmount, out iFEE, out iVAT);
                        if (oRet == WS.ResultType.Result_OK)
                        {
                            sAmount = string.Format((Convert.ToDouble(iAmount) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                            sFEE = string.Format((Convert.ToDouble(iFEE) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                            sVAT = string.Format((Convert.ToDouble(iVAT) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                            sTotalAmount = string.Format((Convert.ToDouble(oModel.TotalAmount) / 100).ToString("##########0.00") + " {0}", oUser.CURRENCy.CUR_ISO_CODE);
                        }
                        else
                        {
                            switch (oRet)
                            {
                                case WS.ResultType.Result_Error_Invalid_User:
                                    sErrorInfo = m_oResBundle.GetString("PBPPlugin", "CashRecharge.Recharge.InvalidUser", "Invalid user");
                                    break;
                                default:
                                    sErrorInfo = m_oResBundle.GetString("PBPPlugin", string.Format("CashRecharge.Recharge.Error.{0}", oRet.ToString()), oRet.ToString());
                                    break;
                            }
                        }

                        bRet = (oRet == WS.ResultType.Result_OK);
                    }
                    else                    
                        sErrorInfo = m_oResBundle.GetString("PBPPlugin", "CashRecharge.Recharge.InvalidUser", "Invalid user");
                    

                }
                else
                    sErrorInfo = FormAuthMemberShip.HelperService.AccessDeniedMessage();
            }
            catch (Exception ex)
            {
                bRet = false;
                // ...
            }


            return Json(new { Result = bRet, ErrorInfo = sErrorInfo, Data = new { Email = email, Amount = sAmount, FEE = sFEE, VAT = sVAT, TotalAmount = sTotalAmount } },
                        JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InstallationsCashRecharge_Read()
        {
            var oModel = new CashRechargeDataModel(geograficAndTariffsRepository, backOfficeRepository, customersRepository);
            return Json(oModel.AllowedInstallations(), JsonRequestBehavior.AllowGet);

            /*var predicate = PredicateBuilder.True<INSTALLATION>();
            var oInsAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed("CASHRECHARGETOOL_READ");
            var oInsOperator = new List<int>();
            FINAN_DIST_OPERATOR oOperator = null;
            var oCurUser = FormAuthMemberShip.HelperService.GetCurrentUser();
            if (oCurUser != null && oCurUser.FdoId.HasValue)
            {
                if (geograficAndTariffsRepository.GetFinanDistOperator(oCurUser.FdoId.Value, ref oOperator))
                {
                    oInsOperator = oOperator.FINAN_DIST_OPERATORS_INSTALLATIONs.Select(i => Convert.ToInt32(i.FDOI_INS_ID)).ToList();
                }
            }
            predicate = predicate.And(i => oInsAllowed.Contains(Convert.ToInt32(i.INS_ID)) && i.INS_ENABLED == 1 &&
                                           oInsOperator.Contains(Convert.ToInt32(i.INS_ID)));
            return Json(InstallationDataModel.List(backOfficeRepository, predicate), JsonRequestBehavior.AllowGet);*/
        }

        #endregion

        #endregion

        private decimal InstallationFilters(IList<Kendo.Mvc.IFilterDescriptor> oFilters)
        {
            decimal installationId = 0;
            if (oFilters != null)
            {
                List<Kendo.Mvc.FilterDescriptor> delFilters = new List<Kendo.Mvc.FilterDescriptor>();
                List<Kendo.Mvc.CompositeFilterDescriptor> delCompositeFilters = new List<Kendo.Mvc.CompositeFilterDescriptor>();

                for (int iFilter = 0; iFilter < oFilters.Count; iFilter++)
                {
                    if (oFilters[iFilter].GetType() == typeof(Kendo.Mvc.FilterDescriptor))
                    {                        
                        Kendo.Mvc.FilterDescriptor oFilter = (Kendo.Mvc.FilterDescriptor)oFilters[iFilter];
                        if (oFilter.Member == "undefined" || oFilter.Member == "InstallationId")
                        {
                            installationId = Convert.ToDecimal(oFilter.Value);
                            delFilters.Add(oFilter);
                        }
                    }
                    else if (oFilters[iFilter].GetType() == typeof(Kendo.Mvc.CompositeFilterDescriptor))
                    {
                        Kendo.Mvc.CompositeFilterDescriptor oCompositeFilter = (Kendo.Mvc.CompositeFilterDescriptor)oFilters[iFilter];
                        installationId = InstallationFilters(oCompositeFilter.FilterDescriptors);
                        if (oCompositeFilter.FilterDescriptors.Count == 0)
                            delCompositeFilters.Add(oCompositeFilter);
                    }
                }
                foreach (var oDelFilter in delFilters)
                {
                    oFilters.Remove(oDelFilter);
                }
                foreach (var oDelCompositeFilter in delCompositeFilters)
                {
                    oFilters.Remove(oDelCompositeFilter);
                }

            }
            return installationId;
        }

        private bool ValidEmail(string emailaddress, bool bOnlyFormat = false)
        {
            bool bRet = false;

            string addressPattern =
                @"^(([\w-]+['\.])+[\w-]+|([\w-]+))@" +
                @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" +
                @"[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
                @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" +
                @"[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
                @"([a-zA-Z0-9]+[\w-]*\.)+[a-zA-Z]{2,9})$";

            bRet = System.Text.RegularExpressions.Regex.IsMatch(emailaddress, addressPattern);

            if (bRet && !bOnlyFormat)
            {
                var predicate = PredicateBuilder.True<USER>();
                predicate = predicate.And(r => r.USR_EMAIL.ToLower() == emailaddress.ToLower() && r.USR_ENABLED == 1); 
                bRet = (backOfficeRepository.GetUsers(predicate).Count() > 0);
            }

            return bRet;
        }

        private void AddRecipientsExecute(EmailToolDataModel model, string[] arrRecipients)
        {

            try
            {

                //model.SetStatus(EmailToolDataStatus.Adding);

                model.AddRecipients(arrRecipients);

                model.SetStatus(EmailToolDataStatus.AddingSuccess);
            }
            catch (Exception ex)
            {
                model.SetStatus(EmailToolDataStatus.AddingFail);
            }
        }
        private void AddRecipientsExecute(EmailToolDataModel model, DataSourceRequest request)
        {

            try
            {
                //model.SetStatus(EmailToolDataStatus.Adding);

                model.AddRecipients(request);

                model.SetStatus(EmailToolDataStatus.AddingSuccess);
            }
            catch (Exception ex)
            {
                model.SetStatus(EmailToolDataStatus.AddingFail);
            }
            
        }

        private void SendEmailsExecute(EmailToolDataModel model, List<string> oRecipients, string sSubject, string sBody, List<FileAttachmentInfo> oAttachments)
        {
            try
            {
                if (oAttachments != null && oAttachments.Count > 0)
                    infrastructureRepository.SendEmailWithAttachmentsToMultiRecipients(oRecipients, sSubject, sBody, oAttachments, integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                else
                    infrastructureRepository.SendEmailToMultiRecipientsTool(Convert.ToDecimal(model.UniqueId), sSubject, sBody, integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);

                model.DeleteAllAttachments();

                model.SetStatus(EmailToolDataStatus.SendSuccess);
            }
            catch (Exception ex)
            {
                model.SetStatus(EmailToolDataStatus.SendFail);
            }

        }

        private void AddRecipientsTransferExecute(BalanceTransferDataModel model, string[] arrRecipients)
        {
            try
            {

                model.AddRecipients(arrRecipients);

                model.SetStatus(BalanceTransferDataStatus.AddingSuccess);
            }
            catch (Exception ex)
            {
                model.SetStatus(BalanceTransferDataStatus.AddingFail);
            }
        }
        private void AddRecipientsTransferExecute(BalanceTransferDataModel model, DataSourceRequest request)
        {
            try
            {
                model.AddRecipients(request);

                model.SetStatus(BalanceTransferDataStatus.AddingSuccess);
            }
            catch (Exception ex)
            {
                model.SetStatus(BalanceTransferDataStatus.AddingFail);
            }
        }

        private void BalanceTransfersExecute(BalanceTransferDataModel model, List<string> oRecipients)
        {
            try
            {
                List<string> oTransfersSuccess;
                List<string> oTransfersFail;

                var oRet = model.Transfer(out oTransfersSuccess, out oTransfersFail);
                if (oRet == WS.ResultType.Result_OK)    
                    model.SetStatus(BalanceTransferDataStatus.TransferSuccess);
                else
                    model.SetStatus(BalanceTransferDataStatus.TransferFail, oRet);
            }
            catch (Exception ex)
            {
                model.SetStatus(BalanceTransferDataStatus.TransferFail);
            }

        }

    }
}
