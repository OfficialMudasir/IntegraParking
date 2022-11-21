using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class SubscriptionTypeStModel
    {
        #region Properties
        [JsonProperty("@id")]
        public int id { get; set; }
        [JsonProperty("#text")]
        public String enabled { get; set; }
        #endregion

        #region Constructor
        public SubscriptionTypeStModel()
        {
        }
        #endregion

        #region Methods Public
        public Boolean isEnabled()
        {
            return enabled.Equals("1");
        }
        #endregion
        
    }
}