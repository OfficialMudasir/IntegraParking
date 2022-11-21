using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZendeskSSO.Request
{
    public class TokenZendeskRequest
    {
        #region Properties
        public string strToken { get; set; }
        #endregion

        #region Methos Public
        public static TokenZendeskRequest getRequest(string sessionId)
        {
            TokenZendeskRequest request = new TokenZendeskRequest();
            request.strToken= sessionId;
            return request;
        }
        #endregion
        
    }
}