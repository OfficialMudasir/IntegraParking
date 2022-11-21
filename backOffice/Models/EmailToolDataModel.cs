using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    [Serializable]
    public class EmailToolDataModel
    {
        [LocalizedDisplayName("EmailToolDataModel_Subject", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public string Subject { get; set; }

        [LocalizedDisplayName("EmailToolDataModel_Body", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public string Body { get; set; }

        private List<EmailDataModel> recipients = new List<EmailDataModel>();

        [LocalizedDisplayName("EmailToolDataModel_Recipients", NameResourceType = typeof(Resources))]
        public EmailDataModel[] Recipients
        {
            get { return recipients.ToArray(); }
            set { recipients = value.ToList(); }
        }

        private List<FileAttachmentInfo> attachments = new List<FileAttachmentInfo>();

        [LocalizedDisplayName("EmailToolDataModel_Attachments", NameResourceType = typeof(Resources))]
        public FileAttachmentInfo[] Attachments
        {
            get { return attachments.ToArray(); }
            set { attachments = value.ToList(); }
        }

        [LocalizedDisplayName("EmailToolDataModel_Password", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public string Password { get; set; }

        public void AddRecipient(string sRecipient)
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

    }

    [Serializable]
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