using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class ServiceChargeTypeDataModel
    {
        [LocalizedDisplayName("ServiceChargeTypeDataModel_ServiceChargeId", NameResourceType = typeof(Resources))]
        public int ServiceChargeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("ServiceChargeTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<ServiceChargeTypeDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var types = (from d in Enum.GetValues(typeof(ServiceChargeType)).Cast<ServiceChargeType>()
                         select new ServiceChargeTypeDataModel
                         {
                             ServiceChargeId = (int) d,
                             Description = Resources.ResourceManager.GetString("ServiceChargeType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return Resources.ResourceManager.GetString("ServiceChargeType_" + Enum.GetName(typeof(ServiceChargeType), _type));
        }
        public static string GetTypeIdString(int? _type)
        {
            if (_type.HasValue)
                return Resources.ResourceManager.GetString("ServiceChargeType_" + Enum.GetName(typeof(ServiceChargeType), _type));
            else
                return "";
        }

    }
}