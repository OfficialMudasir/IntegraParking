using System;
using fastJSON;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.WS.WebAPI
{
    public abstract class BaseRequest<T> : BaseJson
    {
        protected static readonly CLogWrapper Log = new CLogWrapper(typeof(T));

    }
}
