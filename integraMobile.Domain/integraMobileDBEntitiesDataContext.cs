using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using integraMobile.Infrastructure.Logging.Tools;
using System.Threading;
using System.Reflection;

namespace integraMobile.Domain
{
    public partial class integraMobileDBEntitiesDataContext
    {        
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(integraMobileDBEntitiesDataContext));

        private static int m_iConnectionCount = 0;
        private static int m_iHighWatermark = 0;
        private static object oLock = new object();
        private bool m_bDisposed=false;

        partial void OnCreated()
        {
            m_bDisposed = false;
            lock (oLock) 
            {
                m_iConnectionCount++;
                if (m_iConnectionCount > m_iHighWatermark)
                {
                    m_iHighWatermark = m_iConnectionCount;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("integraMobileDBEntitiesDataContext::OnCreated --> High Watermark Connections= {0} ", m_iHighWatermark));
                }
            }
        }

        public void Close()
        {
            if (!m_bDisposed)
            {
                lock (oLock)
                {
                    try
                    {
                        this.Connection.Close();
                        Dispose();
                    }
                    catch
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format(" ~integraMobileDBEntitiesDataContext: Error disposing connection", m_iConnectionCount));
                    }
                    finally
                    {
                        m_iConnectionCount--;
                        m_bDisposed = true;
                        //m_Log.LogMessage(LogLevels.logINFO, string.Format("Closing Connection. Current Connections: {0} ", m_iConnectionCount));
                    }
                }
            }

        }


        ~integraMobileDBEntitiesDataContext()
        {
            Close();
        }

        [System.Data.Linq.Mapping.Function(Name = "GetDate", IsComposable = true)]
        public DateTime GetDate()
        {
            MethodInfo mi = MethodBase.GetCurrentMethod() as MethodInfo;
            return (DateTime)this.ExecuteMethodCall(this, mi, new object[] { }).ReturnValue;
        }

        [System.Data.Linq.Mapping.Function(Name = "GetUTCDate", IsComposable = true)]
        public DateTime GetUTCDate()
        {
            MethodInfo mi = MethodBase.GetCurrentMethod() as MethodInfo;
            return (DateTime)this.ExecuteMethodCall(this, mi, new object[] { }).ReturnValue;
        }
    }
}
