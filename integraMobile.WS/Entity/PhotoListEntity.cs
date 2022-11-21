using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Entity
{
    [Serializable]
    public class PhotoListEntity 
    {
        #region Properties
        public List<PhotoEntity> Photo { get; set;}
        #endregion

        #region Constructor
        public PhotoListEntity()
        {
            Photo = new List<PhotoEntity>();
        }
        #endregion
    }
}