using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using integraMobile.WS.WebAPI;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.WS.WebAPI.Services.Interfaces;
using integraMobile.WS.WebAPI.Actions.Meypar;
using integraMobile.WS.WebAPI.Meypar;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.WS.WebAPI.Controllers
{
    //[RoutePrefix("meypar")]
    public class MeyparController : BaseApiController<MeyparController>
    {
        protected static readonly CLogWrapper Log = new CLogWrapper(typeof(MeyparController));

        private readonly IMeyparService m_oMeyparService;

        public MeyparController(IMeyparService oMeyparService)
        {
            m_oMeyparService = oMeyparService;
        }

        #region Actions

        [HttpGet]
        [Route("api/meypar")]
        public string Test()
        {
            return "API IS UP";
        }

        [HttpPost]
        [Route("api/meypar/SupportValidation")]
        public SupportValidationResponse Validation(SupportValidationBody oBody)
        {
            bool bAction = true;
            ResultTypeEnum eResult = ResultTypeEnum.Success;
            string sDescription = "";
            string sExternalId = null;

            Log.LogMessage(LogLevels.logINFO, string.Format("{0}: input={1})", "Validation", BodyToJson(oBody)));

            string sPlate = (oBody.LicensePlate ?? "").Trim().Replace(" ", "").ToUpper();

            if (string.IsNullOrEmpty(oBody.ParkingId))
            {
                eResult = ResultTypeEnum.InvalidParameter_ParkingId;                
            }
            else if (oBody.OperationTerminal <= 0)
            {
                eResult = ResultTypeEnum.InvalidParameter_Terminal;
            }
            else if (oBody.Support_Type != SupportTypeEnum.Plate || string.IsNullOrEmpty(sPlate))
            {
                eResult = ResultTypeEnum.InvalidParameter_SupportType; // "invalid support type or plate number";
            }
            else if (string.IsNullOrEmpty(sPlate))
            {
                eResult = ResultTypeEnum.InvalidPlate;
            }
            else if (!oBody.Operation_LocalDate.HasValue)
            {
                eResult = ResultTypeEnum.InvalidParameter_OperationLocalDate;
            }
            else if (!oBody.Operation_UTCDate.HasValue)
            {
                eResult = ResultTypeEnum.InvalidParameter_OperationUTCDate;
            }
            else                        
            {
                bAction = m_oMeyparService.SupportValidation(oBody.ParkingId, oBody.OperationTerminal, oBody.Device_Type, sPlate, oBody.Operation_LocalDate.Value, oBody.Operation_UTCDate.Value, out eResult, out sExternalId);
            }

            return new SupportValidationResponse(sExternalId, bAction, sDescription, eResult);
        }

        [HttpPost]
        [Route("api/meypar/StandardNotification")]
        public StandardNotificationResponse StandardNotification(StandardNotificationBody oBody)
        {
            bool bAction = true;
            ResultTypeEnum eResult = ResultTypeEnum.Success;
            string sDescription = "";

            Log.LogMessage(LogLevels.logINFO, string.Format("{0}: input={1})", "StandardNotification", BodyToJson(oBody)));

            decimal dOperationId;
            string sPlate = (oBody.LicensePlate ?? "").Trim().Replace(" ", "").ToUpper();

            if (string.IsNullOrEmpty(oBody.ParkingId))
            {
                eResult = ResultTypeEnum.InvalidParameter_ParkingId;
            }
            else if (oBody.OperationTerminal <= 0)
            {
                eResult = ResultTypeEnum.InvalidParameter_Terminal;
            }
            else if (string.IsNullOrEmpty(oBody.ExternalId) || !decimal.TryParse(oBody.ExternalId, out dOperationId))
            {
                eResult = ResultTypeEnum.InvalidParameter_ExternalID;
            }
            else if (string.IsNullOrEmpty(sPlate))
            {
                eResult = ResultTypeEnum.InvalidPlate;
            }
            else if (!oBody.Operation_LocalDate.HasValue)
            {
                eResult = ResultTypeEnum.InvalidParameter_OperationLocalDate;
            }
            else if (!oBody.Operation_UTCDate.HasValue)
            {
                eResult = ResultTypeEnum.InvalidParameter_OperationUTCDate;
            }
            else
            {
                int iAmount = Convert.ToInt32(oBody.TotalAmount * 100);


                bAction = m_oMeyparService.StandardNotification(oBody.ParkingId, oBody.OperationTerminal, dOperationId, oBody.Device_Type, sPlate, oBody.Operation_LocalDate.Value, oBody.Operation_UTCDate.Value, iAmount, 
                                                                oBody.StayStart_LocalDate,oBody.StayStart_UTCDate, oBody.StayEnd_LocalDate, oBody.StayEnd_UTCDate, oBody.TicketId, 
                                                                oBody.TicketNumber.ToString(), out eResult);
            }

            return new StandardNotificationResponse(oBody.ExternalId, bAction, sDescription, eResult);
        }

        #endregion

        #region Private Methods

        private string BodyToJson(object oBody)
        {

            var oJSONparameters = new fastJSON.JSONParameters
            {
                InlineCircularReferences = true,
                EnableAnonymousTypes = true,
                UseFastGuid = true,
                UseExtensions = false,
                UsingGlobalTypes = false,
                SerializeNullValues = false,
                ShowReadOnlyProperties = true
            };
            return fastJSON.JSON.Beautify(fastJSON.JSON.ToJSON(oBody, oJSONparameters));            
        }

        #endregion
    }
}