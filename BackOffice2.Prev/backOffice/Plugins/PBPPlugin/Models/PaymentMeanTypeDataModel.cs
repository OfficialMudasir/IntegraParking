using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class PaymentMeanTypeDataModel
    {
        [LocalizedDisplayNameBundle("PaymentMeanTypeDataModel_PaymentMeanTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public int PaymentMeanTypeId { get; set; }

        [LocalizedDisplayNameBundle("PaymentMeanTypeDataModel_Description", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Description")]
        public string Description { get; set; }

        private static ResourceBundle resBundle = ResourceBundle.GetInstance();

        public static IQueryable<PaymentMeanTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentMeanType)).Cast<PaymentMeanType>()
                         select new PaymentMeanTypeDataModel
                         {
                             PaymentMeanTypeId = (int)d,
                             Description = resBundle.GetString("PBPPlugin", "PaymentMeanType_" + (d).ToString(), d.ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try
            {
                return resBundle.GetString("PBPPlugin", "PaymentMeanType_" + Enum.GetName(typeof(PaymentMeanType), _type), Enum.GetName(typeof(PaymentMeanType), _type));
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