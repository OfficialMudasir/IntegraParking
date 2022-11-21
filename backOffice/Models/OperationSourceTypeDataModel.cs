using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class OperationSourceTypeDataModel
    {
        [LocalizedDisplayName("OperationSourceTypeDataModel_OperationSourceTypeId", NameResourceType = typeof(Resources))]
        public int OperationSourceTypeId { get; set; }

        [LocalizedDisplayName("OperationSourceTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<OperationSourceTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(OperationSourceType)).Cast<OperationSourceType>()
                         select new OperationSourceTypeDataModel
                         {
                             OperationSourceTypeId = (int)d,
                             Description = Resources.ResourceManager.GetString("OperationSourceType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return Resources.ResourceManager.GetString("OperationSourceType_" + Enum.GetName(typeof(OperationSourceType), _type));
        }

    }
}
