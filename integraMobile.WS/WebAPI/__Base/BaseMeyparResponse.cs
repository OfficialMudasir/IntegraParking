using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastJSON;
using integraMobile.WS.WebAPI.Resources;

namespace integraMobile.WS.WebAPI.Meypar
{
    public enum ReasonType
    {
        None = 0,
        Autorizado = 1,
        NoAutorizado = 2
    }

    public enum ResultTypeEnum
    {
        Success,
        GenericError,
        InvalidParameter_ParkingId,
        InvalidParameter_Terminal,
        InvalidParameter_SupportType,
        InvalidParameter_OperationLocalDate,
        InvalidParameter_OperationUTCDate,
        InvalidParameter_ExternalID,
        ZoneNotExist,
        InvalidPlate,
        PlateNotExist,
        PlateWithMultipleUsers,
        OperationAlreadyClosed,
        OperationEntryAlreadyExists,
        EntryOperationNotExist,
        InvalidEntryOperation,
        RechargeFailed,
        RechargeNotPossible,
        InvalidPaymentMean,
        NotEnoughBalance,
        InvalidAuthenticationHash,
        InvalidInputParameter,
        MissingInputParameter,
        OperationNotAllowed_InvalidPaymentMethod,        
        OperationNotAllowed_UserWithDebt        
    }

    public class BaseMeyparResponse : BaseJson
    {
        protected ReasonType m_eReason;
        protected string m_sDescription;
        protected bool m_bAction;
        protected bool m_bResponseOK;

        [JsonField("Reason")]
        public int ReasonInt
        {
            get { return (int)m_eReason; }
        }
        [JsonField("Description")]
        public string Description
        {
            get { return m_sDescription; }
        }
        [JsonField("Action")]
        public bool Action
        {
            get { return m_bAction; }
        }
        [JsonField("ResponseOK")]
        public bool ResponseOK
        {
            get { return m_bResponseOK; }
        }

        public BaseMeyparResponse(ReasonType eReason, bool bAction, bool bResponseOK, ResultTypeEnum eResultType)
        {
            m_eReason = eReason;
            m_bAction = bAction;
            m_bResponseOK = bResponseOK;

            m_sDescription = LocalizedStrings.ResourceManager.GetString(string.Format("Result_{0}_{1}", this.GetType().Name.ToString(), eResultType.ToString()));
            if (m_sDescription == null)
                m_sDescription = LocalizedStrings.ResourceManager.GetString(string.Format("Result_{0}", eResultType.ToString()));

            if (m_sDescription == null)
                m_sDescription = eResultType.ToString();
        }

    }        
}
