using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public enum EmailToolDataStatus
    {
        Idle,
        Adding,
        AddingSuccess,
        AddingFail,
        Sending,
        SendSuccess,
        SendFail
    }

    [Serializable]
    public class EmailToolDataModel
    {
        [NonSerialized]
        private IBackOfficeRepository m_oBackOfficeRepository = null;

        private long m_dUniqueId;
        
        #region Properties

        [LocalizedDisplayNameBundle("EmailToolDataModel_Subject", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Subject")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public string Subject { get; set; }

        [LocalizedDisplayNameBundle("EmailToolDataModel_Body", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Body")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")] //***
        public string Body { get; set; }

        /*private List<EmailDataModel> recipients = new List<EmailDataModel>();        

        [LocalizedDisplayNameBundle("EmailToolDataModel_Recipients", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Recipients")]
        public EmailDataModel[] Recipients
        {
            get { return recipients.ToArray(); }
            set { recipients = value.ToList(); }
        }*/

        private List<FileAttachmentInfo> attachments = new List<FileAttachmentInfo>();

        [LocalizedDisplayNameBundle("EmailToolDataModel_Attachments", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Attachments")]
        public FileAttachmentInfo[] Attachments
        {
            get { return attachments.ToArray(); }
            set { attachments = value.ToList(); }
        }

        [LocalizedDisplayNameBundle("EmailToolDataModel_Password", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Password")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")] //***
        public string Password { get; set; }

        public int InstallationsCount { get; set; }

        public string UniqueId
        {
            get { return m_dUniqueId.ToString(); }
        }

        #endregion

        #region Constructor

        public EmailToolDataModel()
        {
        }

        public EmailToolDataModel(IBackOfficeRepository oBackOfficeRepository)
        {
            m_oBackOfficeRepository = oBackOfficeRepository;
            this.m_dUniqueId = DateTime.UtcNow.Ticks;

            // Delete old data from recipients temporary data            
            m_oBackOfficeRepository.DeleteEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID <= DateTime.UtcNow.AddDays(-1).Ticks));


        }

        #endregion

        #region Methods

        /*public void AddRecipient(string sRecipient)
        {
            if (recipients == null) recipients = new List<EmailDataModel>();
            EmailDataModel oRecipient = new EmailDataModel() { Email = sRecipient };
            EmailDataModelComparer comp = new EmailDataModelComparer();
            if (!recipients.Contains(oRecipient, comp))
                recipients.Add(oRecipient);
        }
        public void DeleteRecipient(string sRecipient)
        {
            if (recipients == null) recipients = new List<EmailDataModel>();
            EmailDataModel oRecipient = recipients.Find(r => r.Email == sRecipient);
            if (oRecipient != null)
            {
                recipients.Remove(oRecipient);
            }
        }
        public void DeleteAllRecipients()
        {
            recipients.Clear();
        }

        public List<string> GetRecipients()
        {
            return this.recipients.Select(r => r.Email).ToList<string>();
        }*/

        public void AddRecipient(string sRecipient)
        {            
            if (m_oBackOfficeRepository != null)
            {
                m_oBackOfficeRepository.AddEmailToolRecipient(m_dUniqueId, sRecipient);
            }            
        }
        public void AddRecipients(string[] arrRecipients)
        {            
            if (m_oBackOfficeRepository != null)
            {
                var oRecipients = arrRecipients.ToList();

                int iBlockSize = 3000;
                int iBlocks = oRecipients.Count / iBlockSize;
                for (int i = 0; i < iBlocks; i++)
                {
                    m_oBackOfficeRepository.AddEmailToolRecipients(m_dUniqueId, oRecipients.GetRange(i * iBlockSize, iBlockSize).ToArray());
                }
                if (oRecipients.Count > (iBlocks * iBlockSize))
                {
                    m_oBackOfficeRepository.AddEmailToolRecipients(m_dUniqueId, oRecipients.GetRange(iBlocks * iBlockSize, oRecipients.Count - (iBlocks * iBlockSize)).ToArray());
                }
                //m_oBackOfficeRepository.AddEmailToolRecipients(m_dUniqueId, arrRecipients);
            }            
        }
        public void AddRecipients(DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<USER>();
            predicate = predicate.And(u => u.USR_ENABLED == 1);
            if (request.Filters != null)
            {
                decimal installationId = InstallationFilters(request.Filters);
                if (installationId > 0)
                {
                    predicate = predicate.And(u => u.HIS_OPERATIONs.Select(o => o.OPE_INS_ID).Contains(installationId));
                }
            }
            /*IQueryable<UserDataModel>*/
            var users = UserDataModel.List(m_oBackOfficeRepository, predicate, false).ToDataSourceResult(request).Data;
            List<string> oRecipients = new List<string>();
            //for (int i = 0; i < 1000; i++)
            //{
                foreach (UserDataModel oUser in users)
                {
                    oRecipients.Add(oUser.Email);
                    //model.AddRecipient(oUser.Email);
                }
            //}
            this.AddRecipients(oRecipients.ToArray());
        }
        public void DeleteRecipient(string sRecipient)
        {
            if (m_oBackOfficeRepository != null)
            {
                m_oBackOfficeRepository.DeleteEmailToolRecipient(m_dUniqueId, sRecipient);
            }
        }
        public void DeleteAllRecipients()
        {
            if (m_oBackOfficeRepository != null)
            {
                m_oBackOfficeRepository.DeleteAllEmailToolRecipients(m_dUniqueId);
            }
        }

        public List<string> GetRecipients()
        {
            List<string> oRet = new List<string>();

            if (m_oBackOfficeRepository != null)
            {
                oRet = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId))
                                              .Select(e => e.ETR_EMAIL )
                                              .ToList();
            }

            return oRet;
        }

        public List<EmailDataModel> GetRecipientsModel()
        {
            List<EmailDataModel> oRet = new List<EmailDataModel>();

            if (m_oBackOfficeRepository != null)
            {
                oRet = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId))
                                              .Select(e => new EmailDataModel() { Email = e.ETR_EMAIL })
                                              .ToList();
            }

            return oRet;
        }

        public void AddAttachment(string sFileName, string sFullFileName, string sContentType)
        {
            if (attachments == null) attachments = new List<FileAttachmentInfo>();

            FileAttachmentInfo file = new FileAttachmentInfo();
            file.strName = sFileName;
            file.strMediaType = sContentType;
            file.filePath = sFullFileName;

            attachments.Add(file);

        }

        public void DeleteAttachment(string sFilename)
        {
            if (attachments == null) attachments = new List<FileAttachmentInfo>();

            FileAttachmentInfo file = attachments.Find(f => f.strName == sFilename);
            if (file.strName == sFilename)            
            {
                if (System.IO.File.Exists(file.filePath))
                {
                    System.IO.File.Delete(file.filePath);
                }
                attachments.Remove(file);
            }
        }

        public void DeleteAllAttachments()
        {
            if (attachments == null) attachments = new List<FileAttachmentInfo>();
            foreach (FileAttachmentInfo file in attachments)
            {
                System.IO.File.Delete(file.filePath);                
            }
            attachments.Clear();
        }

        public void SetRepository(IBackOfficeRepository oBackOfficeRepository)
        {
            m_oBackOfficeRepository = oBackOfficeRepository;
        }

        public bool SetStatus(EmailToolDataStatus oStatus)
        {
            bool bRet = false;
            bRet = m_oBackOfficeRepository.DeleteEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && e.ETR_EMAIL.StartsWith("##Status_")));
            if (bRet && oStatus != EmailToolDataStatus.Idle) bRet = m_oBackOfficeRepository.AddEmailToolRecipient(m_dUniqueId, "##Status_" + oStatus.ToString());
            return bRet;
        }

        public EmailToolDataStatus GetStatus(out int iRecipientsCount)
        {
            EmailToolDataStatus oRet = EmailToolDataStatus.Idle;
            iRecipientsCount = 0;

            var oItem = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && e.ETR_EMAIL.StartsWith("##Status_"))).FirstOrDefault();
            if (oItem != null)
            {
                if (!System.Enum.TryParse<EmailToolDataStatus>(oItem.ETR_EMAIL.Split('_')[1], out oRet))
                    oRet = EmailToolDataStatus.Idle;
            }
            iRecipientsCount = m_oBackOfficeRepository.GetEmailToolRecipients(PredicateBuilder.True<EMAILTOOL_RECIPIENT>().And(e => e.ETR_ID == m_dUniqueId && !e.ETR_EMAIL.StartsWith("##Status_"))).Count();            

            return oRet;
        }

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

        #endregion

    }

    public class EmailDataModel
    {
        public string Email { get; set; }
    }

    public class EmailDataModelComparer : IEqualityComparer<EmailDataModel>
    {        
        public bool Equals(EmailDataModel x, EmailDataModel y)
        {
            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return (x.Email == y.Email);
        }

        public int GetHashCode(EmailDataModel obj)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashEmail = obj.Email == null ? 0 : obj.Email.GetHashCode();
            
            return hashEmail;
        }
    }

}