using fastJSON;
using integraMobile.WS.WebAPI.Meypar;

namespace integraMobile.WS.WebAPI.Actions.Meypar
{
    public class SupportValidationResponse : BaseMeyparResponse
    {
        private string m_sExternalId;

        [JsonField("ExternalID")]
        public string ExternalId
        {
            get { return m_sExternalId; }
        }


        public SupportValidationResponse(string sExternalID, bool bAction, string sDescription, ResultTypeEnum eResultType)
            : base(ReasonType.None, bAction, true, eResultType)
        {
            m_sExternalId = sExternalID;
        }

    }
}