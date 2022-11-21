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
    public class BooleanDataModel
    {        
        public int BooleanId { get; set; }
        
        public string Description { get; set; }

        public static IQueryable<BooleanDataModel> List()
        {
            List<BooleanDataModel> booleans = new List<BooleanDataModel>();
            booleans.Add(new BooleanDataModel() { BooleanId = 0, Description = Resources.ResourceManager.GetString("Boolean_false") });
            booleans.Add(new BooleanDataModel() { BooleanId = 1, Description = Resources.ResourceManager.GetString("Boolean_true") });
                
            var types = (from d in booleans
                         select d
                         )
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return Resources.ResourceManager.GetString("Boolean_" + (_type==0?"false":"true"));
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