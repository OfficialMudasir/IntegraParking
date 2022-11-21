using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using System.Web;
using System.IO;

namespace integraMobile.Infrastructure
{


    public class ContentTemplates
    {
        private class ContentTemplate
        {
            private class ContentVariable
            {
                public int FormatPosition {get; set;}
                public string Name { get; set; }
                public string Format { get; set; }

                public ContentVariable()
                {
                    FormatPosition = -1;
                    Name = "";
                    Format = "";
                }
            }

            private List<ContentVariable> m_oVariablesList;
            private StringBuilder m_strContent;
            private int m_iVarNumber;

            public ContentTemplate(string strPath)
            {
                m_oVariablesList = new List<ContentVariable>();
                m_strContent = new StringBuilder();
                m_iVarNumber = 0;

                string strText = System.IO.File.ReadAllText(strPath);

                string[] splittedValuesStarts = strText.Split(new char[] { '{' });

                int i = 0;
                foreach(string strCurrStartValue in splittedValuesStarts)
                {                    
                    if (strCurrStartValue[0] == '_')
                    {
                        string[] splittedValuesEnds = strCurrStartValue.Split(new char[] { '}' });

                        int j = 0;
                        foreach (string strCurrValue in splittedValuesEnds)
                        {
                            if ((strCurrValue[0] == '_') && (j==0)&&(strCurrValue.Length >= 3)/*{_X_}*/)
                            {
                                //see if exist format
                                string[] splittedTags = strCurrValue.Trim().Split(new string[] { "_:" }, StringSplitOptions.RemoveEmptyEntries);
                                if ((splittedTags.Length == 1) || (splittedTags.Length == 2))
                                {
                                    string strVarName = splittedTags[0].Remove(0,1);
                                    string strFormat = "";


                                    if (splittedTags.Length == 2)
                                        strFormat = splittedTags[1];
                                    else
                                        strVarName = strVarName.Remove(strVarName.Length - 1, 1);

                                    m_oVariablesList.Add(new ContentVariable()
                                    {
                                        Name = strVarName,
                                        FormatPosition = m_iVarNumber,
                                        Format = strFormat
                                    });


                                    if (!string.IsNullOrEmpty(strFormat))
                                    {
                                        m_strContent.Append(string.Format("{{{0}:{1}", m_iVarNumber, strFormat));
                                    }
                                    else
                                    {
                                        m_strContent.Append(string.Format("{{{0}", m_iVarNumber));
                                    }

                                    m_iVarNumber++;


                                }
                                else
                                {
                                    m_strContent.Append('{');
                                    m_strContent.Append(strCurrValue);
                                    if (splittedValuesEnds.Length == 1)
                                        m_strContent.Append('}');
                                }
                            }
                            else if (j==0)
                            {
                                m_strContent.Append('{');
                                m_strContent.Append(strCurrValue);
                                if (splittedValuesEnds.Length==1)                                
                                    m_strContent.Append('}');
                            }
                            else
                            {
                                m_strContent.Append('}');
                                m_strContent.Append(strCurrValue);                                
                            }
                            j++;
                        }

                    }
                    else if (i == 0)
                    {
                        m_strContent.Append(strCurrStartValue);
                        if (splittedValuesStarts.Length == 1)
                            m_strContent.Append('{');
                    }
                    else
                    {
                        m_strContent.Append('{');
                        m_strContent.Append(strCurrStartValue);
                    }
                    i++;
                }


            }

            public string SubstituteVariablesInTemplate(SortedList hashVariables)
            {

                object[] oVariables = new object[m_iVarNumber];
                int i = 0;
                foreach(ContentVariable oVariable in m_oVariablesList)
                {
                    oVariables[i++] = hashVariables[oVariable.Name];
                }

                return string.Format(m_strContent.ToString(), oVariables);
            }



        }

        private static SortedDictionary<string, ContentTemplate> m_oHashContentTemplates=null;

        public ContentTemplates ()
        {
           

        }

        public static string SubstituteVariablesInTemplate(string strCulture, string strFile,SortedList hashVariables)
        {
            string strRes="";
            try
            {
                if (m_oHashContentTemplates==null)
                {
                     m_oHashContentTemplates = new SortedDictionary<string, ContentTemplate>();
                }


                string strContentFolderRoot = "";

                if (ConfigurationManager.AppSettings["ContentTemplatesFolder"] == null)
                {
                    if (HttpContext.Current != null)
                    {
                        strContentFolderRoot = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Templates");
                    }
                    else
                    {
                        strContentFolderRoot = System.IO.Path.Combine((new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.FullName, "Templates");
                    }
                }
                else
                {
                    try
                    {
                        strContentFolderRoot = ConfigurationManager.AppSettings["ContentTemplatesFolder"].ToString();
                    }
                    catch
                    {
                    }

                }
               

                string strTemplateFile = System.IO.Path.Combine(strContentFolderRoot, strCulture, strFile);

                if (!m_oHashContentTemplates.ContainsKey(strTemplateFile))
                {
                    ContentTemplate oContentTemplate = new ContentTemplate(strTemplateFile);
                    m_oHashContentTemplates.Add(strTemplateFile, oContentTemplate);

                }

                if (m_oHashContentTemplates.ContainsKey(strTemplateFile))
                {

                    ContentTemplate oContentTemplate = m_oHashContentTemplates[strTemplateFile];
                    strRes = oContentTemplate.SubstituteVariablesInTemplate(hashVariables);                    
                }

            }
            catch
            {

                strRes = "";
            }


            return strRes;

        }

    }
}
