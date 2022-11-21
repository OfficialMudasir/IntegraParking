using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace integraMobile.OxxoWS.WS.Data
{
    [Serializable]
    public class WSUserPlates
    {
        private List<string> m_oPlates;

        public WSUserPlates()
        {

        }

        public WSUserPlates(SortedList oParameters)
        {

            m_oPlates = new List<string>();

            if (oParameters.GetValueInt("userDATA_num") >= 1)
            {
                if (oParameters.GetValueInt("userDATA_0_userlp_num") >= 1)
                {
                    m_oPlates = oParameters.GetValueString("userDATA_0_userlp_0_lp").Split('~').ToList();
                }
            }
            else
            {
                /*var iCount = oParameters.GetOutputElementCount("userDATA/userlp");
                if (iCount == 1)
                    m_oPlates.Add(oParameters.GetOutputElement("userDATA/userlp")["lp"].ToString());
                else if (iCount > 1)
                {
                    m_oPlates = oParameters.GetOutputElementArray("userDATA/userlp/lp").ToArray().Select(plate => plate.ToString()).ToList();
                }*/
                var lp = oParameters.GetOutputElement("userDATA/userlp")["lp"];
                if (lp.GetType() == typeof(ArrayList))
                {
                    m_oPlates = oParameters.GetOutputElementArray("userDATA/userlp/lp").ToArray().Select(i => i.ToString()).ToList();
                }
                else
                    m_oPlates.Add(lp.ToString());
            }
        }

        public List<string> Plates
        {
            get { return m_oPlates; }
            set { m_oPlates = value; }
        }

    }
}