using TariffComputer.WS.Common.Enums;
using PIC.Infrastructure.Logging;
//using iParkTicket.Common.Resources;
using integraMobile.WS.Actions;
using integraMobile.WS.Exceptions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace integraMobile.WS.WebAPI.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        #region Override

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var exception = actionExecutedContext.Exception;
            HttpResponseMessage response = null;

            // Invalid authentication hash
            if (exception is InvalidAuthenticationHashException)
            {
                response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new ErrorResponse(ResultCode.InvalidAuthenticationHash, exception.Message));
            }

            // Default error
            if (response == null)
            {
                response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(ResultCode.GenericError));
            }

            actionExecutedContext.Response = response;

            CLogWrapper log = new CLogWrapper(typeof(ExceptionFilter));
            log.LogMessage(LogLevels.logERROR, "Exception:", exception);

            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }

        #endregion
    }
}