using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.WS.WebAPI.Actions.Meypar;
using integraMobile.WS.WebAPI.Meypar;

namespace integraMobile.WS.WebAPI.Services.Interfaces
{
    public interface IMeyparService
    {

        bool SupportValidation(string sParkingId, int iTerminalId, TerminalDeviceTypeEnum eDeviceType, string sPlate, DateTime dtOperation, DateTime dtOperationUtc, out ResultTypeEnum eResult, out string sExternalId);

        bool StandardNotification(string sParkingId, int iTerminalId, decimal dOperationID, TerminalDeviceTypeEnum eDeviceType, string sPlate, DateTime dtOperation, DateTime dtOperationUtc, int iAmount, DateTime? dtEntry, DateTime? dtEntryUtc, DateTime? dtStayEnd, DateTime? dtStayEndUtc, string sTicketId, string sTicketNumber, out ResultTypeEnum eResult);
    }
}