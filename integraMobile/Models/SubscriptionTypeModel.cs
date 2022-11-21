using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class SubscriptionTypeModel
    {
        #region Properties
        public List<SubscriptionTypeStModel> st { get; set; }
        #endregion

        #region Constructor
        public SubscriptionTypeModel()
        {
            st = new List<SubscriptionTypeStModel>();
        }
        #endregion

    }
}