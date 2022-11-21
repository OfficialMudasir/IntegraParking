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
    public class ExternalProviderDataModel
    {
        [LocalizedDisplayName("ExternalProviderDataModel_ExternalProviderId", NameResourceType = typeof(Resources))]
        public decimal ExternalProviderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("ExternalProviderDataModel_Name", NameResourceType = typeof(Resources))]
        public string Name { get; set; }

        public static IQueryable<ExternalProviderDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var groups = (from dom in backOfficeRepository.GetExternalProviders()
                          select new ExternalProviderDataModel
                         {
                             ExternalProviderId = dom.EXP_ID,
                             Name = dom.EXP_NAME
                         })
                         .OrderBy(e => e.Name)
                         .AsQueryable();
            return groups;
        }

        public static ExternalProviderDataModel Get(decimal externalProviderId, IBackOfficeRepository backOfficeRepository)
        {
            var externalProviders = List(backOfficeRepository).Where(a => a.ExternalProviderId == externalProviderId);
            if (externalProviders.Count() > 0)
                return externalProviders.First();
            else
                return new ExternalProviderDataModel();            
        }

        public static ExternalProviderDataModel Get(decimal? externalProviderId, IBackOfficeRepository backOfficeRepository)
        {
            decimal id = -1;
            if (externalProviderId.HasValue) id = externalProviderId.Value;
            return Get(id, backOfficeRepository);
        }

    }
}
