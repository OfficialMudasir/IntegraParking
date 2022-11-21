using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class PaymentSuscryptionTypeDataModel
    {
        [LocalizedDisplayName("PaymentSuscryptionTypeDataModel_PaymentSuscryptionTypeId", NameResourceType = typeof(Resources))]
        public int PaymentSuscryptionTypeId { get; set; }

        [LocalizedDisplayName("PaymentSuscryptionTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<PaymentSuscryptionTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentSuscryptionType)).Cast<PaymentSuscryptionType>()
                         select new PaymentSuscryptionTypeDataModel
                         {
                             PaymentSuscryptionTypeId = (int)d,
                             Description = Resources.ResourceManager.GetString("PaymentSuscryptionType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try {
                return Resources.ResourceManager.GetString("PaymentSuscryptionType_" + Enum.GetName(typeof(PaymentSuscryptionType), _type));
            }
            catch
            {
                return "";
            }
        }
        public static string GetTypeIdString(int? _type)
        {
            if (_type.HasValue)
                return GetTypeIdString(_type.Value);                
            else
                return "";
        }

    }
}