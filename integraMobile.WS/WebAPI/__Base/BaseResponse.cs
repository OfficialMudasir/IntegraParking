using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastJSON;

namespace integraMobile.WS.WebAPI
{
    public abstract class BaseResponse<T> : BaseJson
    {
        private int m_iResult;
        private string m_sMessage;
        private T m_oData;

        [JsonField("res")]
        public int Result
        {
            get { return m_iResult; }
        }
        [JsonField("message")]
        public string Message
        {
            get { return m_sMessage; }
        }
        [JsonField("data")]
        public T Data
        {
            get { return m_oData; }
        }

        public BaseResponse(int iResult, string sMessage, T oData)
        {
            m_iResult = iResult;
            m_sMessage = sMessage;
            m_oData = oData;
        }

    }
}
