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
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Properties;
using backOffice.Models;

namespace backOffice.Controllers
{
    public class ToolsController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;

        public ToolsController(IBackOfficeRepository _backOfficeRepository, IInfraestructureRepository _infrastructureRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;
            this.infrastructureRepository = _infrastructureRepository;
        }

        #region Actions

        public ActionResult Index()
        {
            return RedirectToAction("EmailTool");
        }

        public ActionResult EmailTool()
        {
            EmailToolDataModel model = new EmailToolDataModel();

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

            return View(model);
        }

        public ActionResult Recipients_Read([DataSourceRequest] DataSourceRequest request)
        {
            EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
            if (model == null) model = new EmailToolDataModel();

            return Json(model.Recipients.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<USER>();
            predicate = predicate.And(u => u.USR_ENABLED == 1);

            if (request.Filters != null)
            {
                decimal installationId = InstallationFilters(request.Filters);
                if (installationId > 0)
                {
                    predicate = predicate.And(u => u.OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
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
            return Json(UserDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Installations_Read()
        {
            var predicate = PredicateBuilder.True<INSTALLATION>();
            return Json(InstallationDataModel.List(backOfficeRepository, predicate), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRecipients(string recipients, [DataSourceRequest] DataSourceRequest request, int applyFilter)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                if (model == null) model = new EmailToolDataModel();


                string[] arrRecipients = (string[])Conversions.JsonDeserializeFromString(recipients, typeof(string[]));
                //IList<Kendo.Mvc.IFilterDescriptor> oFilters = (IList<Kendo.Mvc.IFilterDescriptor>)Conversions.JsonDeserializeFromString(filters, typeof(IList<Kendo.Mvc.IFilterDescriptor>));
                //object oFilters = Conversions.JsonDeserializeFromString(filters, typeof(object));

                if (applyFilter != 1)
                {
                    foreach (string sRecipient in arrRecipients)
                    {
                        model.AddRecipient(sRecipient);
                    }
                }
                else
                {
                    //DataSourceRequest request = new DataSourceRequest();
                    //request.Filters.Add((Kendo.Mvc.IFilterDescriptor) oFilters);
                    var predicate = PredicateBuilder.True<USER>();
                    predicate = predicate.And(u => u.USR_ENABLED == 1);
                    if (request.Filters != null)
                    {
                        decimal installationId = InstallationFilters(request.Filters);
                        if (installationId > 0)
                        {
                            predicate = predicate.And(u => u.OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
                        }
                    }
                    /*IQueryable<UserDataModel>*/ var users = UserDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request).Data;
                    foreach (UserDataModel oUser in users)
                    {
                        model.AddRecipient(oUser.Email);
                    }
                }

                Session["EMAILTOOL_MODEL"] = model;

                bRet = true;

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddRecipients(string recipients)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(recipients))
                {
                    EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                    if (model == null) model = new EmailToolDataModel();


                    //string[] arrRecipients = (string[])Conversions.JsonDeserializeFromString(recipients, typeof(string[]));
                    string sRecipients = (string)Conversions.JsonDeserializeFromString(recipients, typeof(string));
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

                    foreach (string sRecipient in arrRecipients)
                    {
                        if (ValidEmail(sRecipient.Trim()))
                            model.AddRecipient(sRecipient.Trim());
                    }

                    Session["EMAILTOOL_MODEL"] = model;

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
                if (model == null) model = new EmailToolDataModel();

                model.DeleteRecipient(recipient);

                Session["EMAILTOOL_MODEL"] = model;

                bRet = true;

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
                if (model == null) model = new EmailToolDataModel();

                model.DeleteAllRecipients();

                Session["EMAILTOOL_MODEL"] = model;

                bRet = true;

            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorInfo = "Error";
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendEmails(string subject, string body, string pwd)
        {
            bool bRet = false;
            string sErrorInfo = "";

            try
            {
                EmailToolDataModel model = (EmailToolDataModel)Session["EMAILTOOL_MODEL"];
                if (model == null) model = new EmailToolDataModel();

                string sConfigPwd = (ConfigurationManager.AppSettings["EmailTool_Password"] ?? "");

                if (sConfigPwd == pwd)
                {
                    if (model.Recipients.Count() > 0)
                    {
                        model.Subject = subject;
                        model.Body = Server.HtmlDecode(body);

                        //infrastructureRepository.SendEmailToMultiRecipients(model.GetRecipients(), model.Subject, model.Body, integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                        infrastructureRepository.SendEmailWithAttachmentsToMultiRecipients(model.GetRecipients(), model.Subject, model.Body, model.Attachments.ToList(), integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);

                        model.DeleteAllAttachments();

                        Session["EMAILTOOL_MODEL"] = model;

                        bRet = true;
                    }
                    else
                    {
                        bRet = false;
                        sErrorInfo = Resources.EmailTool_SendEmails_NoRecipients;
                    }
                }
                else
                {
                    bRet = false;
                    sErrorInfo = Resources.EmailTool_SendEmails_InvalidPwd;
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

    }
}
