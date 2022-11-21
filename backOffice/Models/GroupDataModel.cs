using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class GroupDataModel
    {
        [LocalizedDisplayName("GroupDataModel_GroupId", NameResourceType = typeof(Resources))]
        public decimal? GroupId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("GroupDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public string InstallationShortDesc { get; set; }

        public string DescriptionWithInst
        {
            get { return InstallationShortDesc + " - " + Description; }
        }

        public static IQueryable<GroupDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<GROUP>();

            string sInfrastructureFilter = ConfigurationManager.AppSettings["InfrastructureFilter"] ?? "";
            if (!string.IsNullOrWhiteSpace(sInfrastructureFilter))
            {
                decimal dId = 0;
                string[] infraFilter = sInfrastructureFilter.Split(',').Select(id => (!decimal.TryParse(id, out dId)?id: "G" + id)).ToArray();
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, infraFilter);
            }

            var groups = (from dom in backOfficeRepository.GetGroups(predicate)
                         select new GroupDataModel
                         {
                             GroupId = dom.GRP_ID,
                             Description = (dom.GRP_DESCRIPTION2 ?? dom.GRP_DESCRIPTION),
                             InstallationShortDesc = dom.INSTALLATION.INS_SHORTDESC
                         })                         
                         .OrderBy(e => e.Description)
                         .AsQueryable();
            return groups;
        }

        public static GroupDataModel Get(decimal groupId, IBackOfficeRepository backOfficeRepository)
        {
            var groups = List(backOfficeRepository).Where(a => a.GroupId == groupId);
            if (groups.Count() > 0)
                return groups.First();
            else
                return new GroupDataModel();            
        }

        public static GroupDataModel Get(decimal? groupId, IBackOfficeRepository backOfficeRepository)
        {
            decimal id = -1;
            if (groupId.HasValue) id = groupId.Value;
            return Get(id, backOfficeRepository);
        }

        public static bool GetInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, string[] installations, out Dictionary<decimal, List<decimal>> insGrpIds)
        {
            bool bRet = false;
            insGrpIds = new Dictionary<decimal, List<decimal>>();

            if (installations != null)
            {
                // apply installations filter
                if (installations.Length > 0)
                {
                    decimal dIns = 0;
                    decimal dGrp = 0;
                    foreach (string sId in installations)
                    {
                        if (sId.StartsWith("I"))
                        {
                            dIns = decimal.Parse(sId.Substring(1));
                            if (dIns > 0 && !insGrpIds.ContainsKey(dIns))
                                insGrpIds.Add(dIns, new List<decimal>());
                        }
                        else if (sId.StartsWith("G"))
                        {
                            dGrp = decimal.Parse(sId.Substring(1));
                            //if (!grpIds.Contains(dGrp))
                            //{
                            var groups = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>().And(g => g.GRP_ID == dGrp));
                            if (groups != null && groups.Count() > 0)
                            {
                                GROUP oGroup = groups.First();
                                dIns = oGroup.GRP_INS_ID;

                                if (!insGrpIds.ContainsKey(dIns))
                                    insGrpIds.Add(dIns, new List<decimal>());

                                if (!insGrpIds[dIns].Contains(oGroup.GRP_ID))
                                    insGrpIds[dIns].Add(oGroup.GRP_ID);

                                var grpChilds = GetGroupChilds(oGroup);
                                foreach (decimal dGrpChild in grpChilds)
                                {
                                    if (!insGrpIds[dIns].Contains(dGrpChild))
                                        insGrpIds[dIns].Add(dGrpChild);
                                }
                            }
                            //}
                        }

                    }
                }
                bRet = true;
            }

            return bRet;
        }

        private static List<decimal> GetGroupChilds(GROUP oParent)
        {
            List<decimal> oRet = new List<decimal>();
            foreach (var oGrpH in oParent.GROUPS_HIERARCHies1)
            {
                oRet.Add(oGrpH.GRHI_GPR_ID_CHILD);
                oRet.AddRange(GetGroupChilds(oGrpH.GROUP));
            }
            return oRet;
        }

        public static Expression<Func<GROUP, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<GROUP, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;
            
            if (GroupDataModel.GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<GROUP>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<GROUP>();
                    predicateIns = predicateIns.And(g => g.GRP_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<GROUP>();
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(g => g.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);                
            }

            return predicate;
        }

    }
}