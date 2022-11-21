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
    public class PaymentMeanTypeDataModel
    {
        [LocalizedDisplayName("PaymentMeanTypeDataModel_PaymentMeanTypeId", NameResourceType = typeof(Resources))]
        public int PaymentMeanTypeId { get; set; }

        [LocalizedDisplayName("PaymentMeanTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<PaymentMeanTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentMeanType)).Cast<PaymentMeanType>()
                         select new PaymentMeanTypeDataModel
                         {
                             PaymentMeanTypeId = (int)d,
                             Description = Resources.ResourceManager.GetString("PaymentMeanType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try
            {
                return Resources.ResourceManager.GetString("PaymentMeanType_" + Enum.GetName(typeof(PaymentMeanType), _type));
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