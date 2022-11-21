using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Entity
{
    [Serializable]
    public class ServicesPlateParameterInEntity: BaseParameterInEntity
    {
        #region Properties
	    public string IdServiceType { get; set;}
        public string TypeOfServiceType { get; set; }
        public string license { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string CardReducedMovility { get; set;}
        public string CompanyName { get; set;}
        public string CifNifCompany { get; set;}
        public string InstallationId { get; set; }
        public string ServiceTypeId { get; set; }
        public PhotoListEntity PhotoList { get; set;}
        #endregion    

        #region Constructor
        public ServicesPlateParameterInEntity()
        {
            PhotoList = new PhotoListEntity();
        }
        #endregion
    }
}