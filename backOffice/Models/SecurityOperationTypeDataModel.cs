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
    public class SecurityOperationTypeDataModel
    {
        [LocalizedDisplayName("SecurityOperationTypeDataModel_Id", NameResourceType = typeof(Resources))]
        public int Id { get; set; }

        [LocalizedDisplayName("SecurityOperationTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<SecurityOperationTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(SecurityOperationType)).Cast<SecurityOperationType>()
                         select new SecurityOperationTypeDataModel
                         {
                             Id = (int)d,
                             Description = Resources.ResourceManager.GetString("SecurityOperationType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return Resources.ResourceManager.GetString("SecurityOperationType_" + Enum.GetName(typeof(SecurityOperationType), _type));
        }

    }
}