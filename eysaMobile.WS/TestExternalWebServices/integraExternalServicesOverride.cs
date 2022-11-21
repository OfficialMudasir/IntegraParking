using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TestExternalWebServices
{
    class integraExternalServicesOverride:com.eysamobile.www.integraExternalServices
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);

            webRequest.KeepAlive = false;
            webRequest.ProtocolVersion = HttpVersion.Version10;
            return webRequest;
        }
    }
}
