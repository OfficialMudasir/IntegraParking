using CurrencyChanger.WS.Infrastructure.Logging.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.API
{
    public class CSFacturacionAPI
    {
        private static string PASSWORD = "SysConroe2015";

        public static bool UploadFile(byte[] m_fileData)
        {
            CSFacturacionWS.miservicioweb client = new CSFacturacionWS.miservicioweb();

            CLogWrapper log = new CLogWrapper(typeof(CSFacturacionAPI));

            try
            {                
                CSFacturacionWS.respuestaUpload response = client.Uploadarchivo(PASSWORD, m_fileData);

                if (!response.Resultado)
                {
                    log.LogMessage(LogLevels.logERROR, "File upload error: " + response.Msj);
                }

                return response.Resultado;
            }
            catch (Exception ex)
            {
                log.LogMessage(LogLevels.logFATAL, "File upload fatal error: " + ex.Message);
            }

            return false;
        }

        public static StampTicketResponse StampTicket(string m_refID, string m_receiverJsonData)
        {
            CSFacturacionWS.miservicioweb client = new CSFacturacionWS.miservicioweb();

            CLogWrapper log = new CLogWrapper(typeof(CSFacturacionAPI));

            try
            {
                CSFacturacionWS.respuestaTimbrado response = client.timbrarTicket(m_refID, PASSWORD, @m_receiverJsonData);

                if (String.IsNullOrEmpty(response.PDF64))
                {
                    log.LogMessage(LogLevels.logERROR, "Stamp ticket error: " + response.Msj);
                }
                else
                {
                    return new StampTicketResponse(response.NAMEFILE, response.PDF64);
                }                
            }
            catch (Exception ex)
            {
                log.LogMessage(LogLevels.logFATAL, "Stamp ticket fatal error: " + ex.Message);
            }

            return null;
        }

        public static VerifyTicketResponse VerifyTicket(double m_ticketTotal, string m_refID, string m_emissorRfc)
        {
            CSFacturacionWS.miservicioweb client = new CSFacturacionWS.miservicioweb();

            CLogWrapper log = new CLogWrapper(typeof(CSFacturacionAPI));

            try
            {
                CSFacturacionWS.respuestaValidarticket response = client.validarTicket(m_ticketTotal, m_refID, m_emissorRfc);
                
                if (!response.Resultado)
                {
                    log.LogMessage(LogLevels.logERROR, "Verify ticket error: " + response.Msj);
                }
                else
                {
                    return new VerifyTicketResponse(response.NAMEFILE, response.PDF64);
                }
            }
            catch (Exception ex)
            {
                log.LogMessage(LogLevels.logFATAL, "Verify ticket fatal error: " + ex.Message);
            }

            return null;
        }

        public static DataTicketResponse GetTicketData(string m_refID)
        {
            CSFacturacionWS.miservicioweb client = new CSFacturacionWS.miservicioweb();

            CLogWrapper log = new CLogWrapper(typeof(CSFacturacionAPI));

            try
            {
                CSFacturacionWS.respuestaObtenerdatosticket response = client.obtenerDatosticket(m_refID, PASSWORD);
                                
                if (!response.Resultado)
                {
                    log.LogMessage(LogLevels.logERROR, "Verify ticket error: " + response.Msj);
                }
                else
                {
                    return new DataTicketResponse(response.cDatosticket);
                }
            }
            catch (Exception ex)
            {
                log.LogMessage(LogLevels.logFATAL, "Verify ticket fatal error: " + ex.Message);
            }

            return null;
        }
    }
}
