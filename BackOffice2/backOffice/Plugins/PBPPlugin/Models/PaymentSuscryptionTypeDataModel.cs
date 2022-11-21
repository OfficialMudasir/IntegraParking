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
    public class PaymentSuscryptionTypeDataModel
    {
        [LocalizedDisplayNameBundle("PaymentSuscryptionTypeDataModel_PaymentSuscryptionTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public int PaymentSuscryptionTypeId { get; set; }

        [LocalizedDisplayNameBundle("PaymentSuscryptionTypeDataModel_Description", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Description")]
        public string Description { get; set; }

        private static ResourceBundle resBundle = ResourceBundle.GetInstance();

        public static IQueryable<PaymentSuscryptionTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentSuscryptionType)).Cast<PaymentSuscryptionType>()
                         select new PaymentSuscryptionTypeDataModel
                         {
                             PaymentSuscryptionTypeId = (int)d,
                             Description = resBundle.GetString("Maintenance", "TypeEnums.PaymentSuscryptionTypes." + (d).ToString(), d.ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try {
                return resBundle.GetString("Maintenance", "TypeEnums.PaymentSuscryptionTypes." + Enum.GetName(typeof(PaymentSuscryptionType), _type), Enum.GetName(typeof(PaymentSuscryptionType), _type));
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