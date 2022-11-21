//using TariffComputer.WS.Common.Enums;

namespace integraMobile.WS.WebAPI.Actions
{
    public class ErrorResponse : BaseResponse<ErrorResponse>
    {
        #region Constructors

        public ErrorResponse()
            : base(0, null, null) { }

        /*public ErrorResponse(ResultCode result)
            : base(result, null) { }

        public ErrorResponse(ResultCode result, string message)
            : base(result, message, null) { }
        */
        #endregion
    }
}