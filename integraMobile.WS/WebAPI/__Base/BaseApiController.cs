using System.Web.Http;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.WS.WebAPI
{
    public class BaseApiController<T> : ApiController
    {
        //protected CLogWrapper m_oLog = new CLogWrapper(typeof(T));
    }
}