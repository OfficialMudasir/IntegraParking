using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Entity
{
    [Serializable]
    public class PhotoEntity
    {
        #region Properties
        public string Number { get; set;}
    	public string Image { get; set;}
        #endregion

        #region Constructor
        public PhotoEntity()
        { }
        #endregion
    }
}