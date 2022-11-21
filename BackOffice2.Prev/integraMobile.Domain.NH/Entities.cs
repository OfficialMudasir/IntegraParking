using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PIC.Domain.Abstract;

namespace integraMobile.Domain.NH.Entities
{
    public partial class CustomerInscription
    {
        public virtual string CusinsUrl
        {
            get { return (ConfigurationManager.AppSettings["CustomerInscriptions_Url"] ?? "") + this.CusinsUrlParameter; }
        }
    }

    public partial class VwUser
    {

        public static IQueryable ProcessList(IQueryable<VwUser> oQuery, IList<Kendo.Mvc.IFilterDescriptor> oFilters, IBaseRepository oDataRepository)
        {
            ConvertFilters(oFilters, oDataRepository);
            return oQuery.AsQueryable();
        }

        public static void ConvertFilters(IList<Kendo.Mvc.IFilterDescriptor> oFilters, IBaseRepository oDataRepository)
        {            
            if (oFilters != null)
            {
                List<Kendo.Mvc.CompositeFilterDescriptor> addCompositeFilters = new List<Kendo.Mvc.CompositeFilterDescriptor>();
                List<Kendo.Mvc.FilterDescriptor> delFilters = new List<Kendo.Mvc.FilterDescriptor>();

                for (int iFilter = 0; iFilter < oFilters.Count; iFilter++)
                {
                    if (oFilters[iFilter].GetType() == typeof(Kendo.Mvc.FilterDescriptor))
                    {
                        Kendo.Mvc.FilterDescriptor oFilter = (Kendo.Mvc.FilterDescriptor)oFilters[iFilter];
                        Kendo.Mvc.CompositeFilterDescriptor oAddFilter = ConvertFilter(oFilter, oDataRepository);
                        if (oAddFilter != null)
                        {
                            addCompositeFilters.Add(oAddFilter);
                            delFilters.Add(oFilter);
                        }
                    }
                    else if (oFilters[iFilter].GetType() == typeof(Kendo.Mvc.CompositeFilterDescriptor))
                    {
                        Kendo.Mvc.CompositeFilterDescriptor oCompositeFilter = (Kendo.Mvc.CompositeFilterDescriptor)oFilters[iFilter];
                        ConvertFilters(oCompositeFilter, oDataRepository);
                    }
                }

                foreach (var oFilter in addCompositeFilters)
                {
                    oFilters.Add(oFilter);
                }
                foreach (var oFilter in delFilters)
                {
                    oFilters.Remove(oFilter);
                }        
            }            
        }
        private static void ConvertFilters(Kendo.Mvc.CompositeFilterDescriptor oCompositeFilter, IBaseRepository oDataRepository)
        {            
            List<Kendo.Mvc.CompositeFilterDescriptor> addCompositeFilters = new List<Kendo.Mvc.CompositeFilterDescriptor>();
            List<Kendo.Mvc.FilterDescriptor> delFilters = new List<Kendo.Mvc.FilterDescriptor>();

            for (int iFilter = 0; iFilter < oCompositeFilter.FilterDescriptors.Count; iFilter++)
            {
                if (oCompositeFilter.FilterDescriptors[iFilter].GetType() == typeof(Kendo.Mvc.FilterDescriptor))
                {
                    Kendo.Mvc.FilterDescriptor oFilter = (Kendo.Mvc.FilterDescriptor)oCompositeFilter.FilterDescriptors[iFilter];
                    Kendo.Mvc.CompositeFilterDescriptor oAddFilter = ConvertFilter(oFilter, oDataRepository);
                    if (oAddFilter != null)
                    {
                        addCompositeFilters.Add(oAddFilter);
                        delFilters.Add(oFilter);
                    }
                }
                else if (oCompositeFilter.FilterDescriptors[iFilter].GetType() == typeof(Kendo.Mvc.CompositeFilterDescriptor))
                {
                    Kendo.Mvc.CompositeFilterDescriptor oCompositeFilter2 = (Kendo.Mvc.CompositeFilterDescriptor)oCompositeFilter.FilterDescriptors[iFilter];
                    ConvertFilters(oCompositeFilter2, oDataRepository);
                }
            }
            foreach (var oFilter in addCompositeFilters)
            {
                oCompositeFilter.FilterDescriptors.Add(oFilter);
            }
            foreach (var oFilter in delFilters)
            {
                oCompositeFilter.FilterDescriptors.Remove(oFilter);
            }        
        }
        private static Kendo.Mvc.CompositeFilterDescriptor ConvertFilter(Kendo.Mvc.FilterDescriptor oFilter, IBaseRepository oDataRepository)
        {
            Kendo.Mvc.CompositeFilterDescriptor oRet = null;
            if (oFilter.Member == "Plates")
            {
                List<UserPlate> oUserPlates = null;
                switch (oFilter.Operator) {
                    case Kendo.Mvc.FilterOperator.IsEqualTo:
                            oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => uup.UsrpPlate == oFilter.Value.ToString() && uup.UsrpEnabled == 1).ToList();
                            break;

                    case Kendo.Mvc.FilterOperator.IsNotEqualTo:
                            oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => uup.UsrpPlate != oFilter.Value.ToString() && uup.UsrpEnabled == 1).ToList();
                            break;

                    case Kendo.Mvc.FilterOperator.Contains:
                            //oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => oFilter.Value.ToString().Contains(uup.UsrpPlate)).ToList();
                            oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => uup.UsrpPlate.Contains(oFilter.Value.ToString()) && uup.UsrpEnabled == 1).ToList();
                            break;

                    case Kendo.Mvc.FilterOperator.DoesNotContain:
                            //oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => !oFilter.Value.ToString().Contains(uup.UsrpPlate)).ToList();
                            oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => !uup.UsrpPlate.Contains(oFilter.Value.ToString()) && uup.UsrpEnabled == 1).ToList();
                            break;

                    case Kendo.Mvc.FilterOperator.StartsWith:
                            oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => uup.UsrpPlate.StartsWith(oFilter.Value.ToString()) && uup.UsrpEnabled == 1).ToList();
                            break;

                    case Kendo.Mvc.FilterOperator.EndsWith:
                            oUserPlates = oDataRepository.GetQuery(typeof(UserPlate)).Cast<UserPlate>().Where(uup => uup.UsrpPlate.EndsWith(oFilter.Value.ToString()) && uup.UsrpEnabled == 1).ToList();
                            break;
                }

                if (oUserPlates != null)
                {
                    oRet = new Kendo.Mvc.CompositeFilterDescriptor();
                    oRet.LogicalOperator = Kendo.Mvc.FilterCompositionLogicalOperator.Or;
                    if (oUserPlates.Count > 0)
                    {
                        foreach (var oPlate in oUserPlates)
                        {
                            oRet.FilterDescriptors.Add(new Kendo.Mvc.FilterDescriptor("Id", Kendo.Mvc.FilterOperator.IsEqualTo, oPlate.UsrpUsr.UsrId));
                        }
                    }
                    else
                        oRet.FilterDescriptors.Add(new Kendo.Mvc.FilterDescriptor("Id", Kendo.Mvc.FilterOperator.IsEqualTo, 0));
                }
                
            }
            return oRet;
        }

    }
}
