using integraMobile.ExternalWS;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.WS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace integraMobile.WS.Tools
{
    public class Helpers
    {
        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(Helpers));
        static string _xmlTagName = "ipark";
        private const string IN_SUFIX = "_in";
        private const string OUT_SUFIX = "_out";
        #endregion

        #region Methods Public
        /// <summary>
        /// Método para validar un decimal cuando cuando el valor se encuentra en un SortedList (string)
        /// Ejemplo:    
        ///             parametersIn["CityID"]
        ///             ValidateInputParameterToDecimal(parametersIn,"CityId")
        /// En caso de que no lo encuentre o se produce un error devolverá null             
        /// </summary>
        /// <param name="parametersIn"></param>
        /// <param name="nameParameter"></param>
        /// <returns></returns>
        public static decimal? ValidateInputParameterToDecimal(SortedList parametersIn, string nameParameter)
        {
            decimal? dDecimal = null;
            if (parametersIn[nameParameter] != null)
            {
                try
                {
                    decimal dDecimalTry = Convert.ToDecimal(parametersIn[nameParameter].ToString());
                    dDecimal = dDecimalTry;
                }
                catch
                {
                    dDecimal = null;
                }
            }
            return dDecimal;
        }

        /// <summary>
        /// Método para validar un decimal de un string
        /// </summary>
        /// <param name="stringdecimal"></param>
        /// <returns></returns>
        public static decimal? ValidateStringToDecimal(string stringdecimal)
        {
            decimal? dDecimal = null;
            if (!String.IsNullOrEmpty(stringdecimal))
            {
                try
                {
                    decimal dDecimalTry = Convert.ToDecimal(stringdecimal);
                    dDecimal = dDecimalTry;
                }
                catch
                {
                    dDecimal = null;
                }
            }
            return dDecimal;
        }


        public static StringBuilder ValidateStringIsNullOrEmptyParameterInList(SortedList parametersIn, List<String> nameParameters)
        {
            StringBuilder listParameterInvalid = new StringBuilder();
            foreach (string sparameter in nameParameters)
            {
                string sInvalid = ValidateStringIsNullOrEmptyParameterIn(parametersIn, sparameter);
                if (!String.IsNullOrEmpty(sInvalid))
                {
                    listParameterInvalid.Append("- " + sInvalid + " is null ; ");
                }
            }
            return listParameterInvalid;
        }




        public static decimal? BonificationLogic(decimal? bonMlt, decimal? bonExtMlt)
        {
            decimal? dBonMlt = null;
            if (bonMlt.HasValue && bonExtMlt.HasValue)
            {
                dBonMlt = bonMlt / bonExtMlt;
                if (dBonMlt == 1)
                {
                    dBonMlt = null; ;
                }
            }
            else if (bonMlt.HasValue && !bonExtMlt.HasValue)
            {
                dBonMlt = bonMlt;
            }
            return dBonMlt;
        }

        public static int ApplyPercentageBonExtMlt(decimal? bonMlt, decimal? bonExtMlt, int totalAmount)
        {
            decimal? dBonMltTemp = BonificationLogic(bonMlt, bonExtMlt);
            if (dBonMltTemp.HasValue)
            {
                if (bonExtMlt.HasValue)
                {
                    return Convert.ToInt32(Math.Round((totalAmount * bonExtMlt.Value), MidpointRounding.AwayFromZero));
                }

            }
            return totalAmount;
        }



        private static String ValidateStringIsNullOrEmptyParameterIn(SortedList parametersIn, string nameParameter)
        {
            String errorNameParameter = nameParameter;
            if (parametersIn[nameParameter] != null)
            {
                try
                {
                    string sValue = parametersIn[nameParameter].ToString();
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        errorNameParameter = string.Empty;
                    }
                }
                catch
                {
                    
                }
            }
            return errorNameParameter;
        }


        public static String ValidateAppVersionParameterIn(SortedList parametersIn, string nameParameterOne, string nameParameterTwo, ref string xmlOut)
        {
            string sAppVersion = string.Empty;
            try
            {
                if (parametersIn[nameParameterOne] != null)
                    sAppVersion = parametersIn[nameParameterOne].ToString();
                else
                    sAppVersion = parametersIn[nameParameterTwo].ToString();
            }
            catch
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
            }
            return sAppVersion;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sXml"></param>
        /// <param name="isXmlIn"></param>
        /// <returns></returns>
        public static T StrinXmlToObject<T>(string sXml, bool isXmlIn)
        {
            T result = default(T);
            string strOut = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXml);

                string jsonString = JsonConvert.SerializeXmlNode(doc);
                if (isXmlIn)
                {
                    jsonString = jsonString.Replace(ConstantsEntity.TEXT_I_PARK_IN, string.Empty);
                }
                else
                {
                    jsonString = jsonString.Replace(ConstantsEntity.TEXT_I_PARK_OUT, string.Empty);
                }
                jsonString = jsonString.Remove(jsonString.Length - 1, 1);
                result = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, String.Format("Helpers::StrinXmlToObject::Error={0}", ex.Message));
            }
            return result;
        }


        /// <summary>
        /// Creamos un nombre
        /// </summary>
        /// <returns></returns>
        public static string CreateNameImage()
        {
            string fileNamefinal = string.Format("photo_{0}.jpg", Guid.NewGuid());
            return fileNamefinal;
        }
        

        /// <summary>
        /// Método para subir la imagen
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="base64Image">The base64 image.</param>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <param name="imagePath">The image path.</param>
        /// <param name="thumbPath">The thumb path.</param>
        /// <returns></returns>
        public static bool UploadImage(
            string rootPath,
            string base64Image,
            string path,
            ref string name,
            ref string imagePath,
            ref string thumbPath)
        {
            try
            {
                // Get image byte array
                var bytes = Convert.FromBase64String(base64Image);

                // Fixed image name
                if (String.IsNullOrEmpty(name))
                {
                    name = CreateNameImage();
                }
                name = name.Replace(".jpg", "");

                // Folder
                var imageFolder = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}", rootPath, path));

                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                // Image name
                imagePath = string.Format("{0}/{1}.jpg", path, name);
                var imageFilePath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}", rootPath, imagePath));

                // Save image
                using (var imageFile = new FileStream(imageFilePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                // Thumbnail name
                thumbPath = string.Format("{0}/{1}_thumb.jpg", path, name);
                var thumbFilePath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}", rootPath, thumbPath));

                
                // Create thumbnail
                using (var image = Image.FromFile(imageFilePath))
                {
                    // Compute thumbnail size
                    var thumbnailSize = GetThumbnailSize(image);

                    // Get thumbnail
                    using (
                        var thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null,
                            IntPtr.Zero))
                    {
                        // Save thumbnail
                        thumbnail.Save(thumbFilePath);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Helpers::UploadImage error: {0}", e));
                return false;
            }
        }

        /// <summary>
        /// Gets the download image.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetDownloadImage(string path)
        {
            try
            {
                var serverPath = HttpContext.Current.Server.MapPath(path);
                var image = File.ReadAllBytes(serverPath);

                return Convert.ToBase64String(image);
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Helpers::GetDownloadImage: ", e);
                return string.Empty;
            }
        }




        /// <summary>
        /// Gets the dictionary images.
        /// </summary>
        /// <param name="images">The images.</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetDictionaryImages(List<PhotoEntity> images)
        {
            
            var imagesDictionary = new Dictionary<string, int>();

            if (images != null)
            {
                imagesDictionary = images.Select(i => new
                {
                    i.Image,
                    i.Number
                }).ToDictionary(key => key.Image, value => Convert.ToInt32(value.Number));
            }

            return imagesDictionary;
        }

        public static string PathConstruction(decimal? insid, int? year, int? month, decimal? userid, decimal? id)
        {
            StringBuilder path = new StringBuilder();
            if (insid.HasValue)
            {
                path.AppendFormat("/{0}", insid.Value);
            }
            if (year.HasValue)
            {
                path.AppendFormat("/{0}", year.Value);
            }
            if (month.HasValue)
            {
                path.AppendFormat("/{0}", month.Value);
            }
            if (userid.HasValue)
            {
                path.AppendFormat("/{0}", userid.Value);
            }
            if (id.HasValue)
            {
                path.AppendFormat("/{0}", id.Value);
            }
            return path.ToString();
        }

        #endregion

        #region Private

        /// <summary>
        /// Gets the size of the thumbnail.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        private static Size GetThumbnailSize(Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 40;

            // Width and height.
            var originalWidth = original.Width;
            var originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;

            if (originalWidth > originalHeight)
                factor = (double)maxPixels / originalWidth;
            else
                factor = (double)maxPixels / originalHeight;

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        private static string GenerateXMLErrorResult(ResultType rt)
        {
            string strRes = "";
            try
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Error = {0}", rt.ToString()) );

                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + OUT_SUFIX);
                xmlOutDoc.AppendChild(root);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + OUT_SUFIX);
                XmlElement result = xmlOutDoc.CreateElement("r");
                result.InnerText = ((int)rt).ToString();
                rootNode.AppendChild(result);
                strRes = xmlOutDoc.OuterXml;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GenerateXMLErrorResult::Exception", e);
            }


            return strRes;
        }

        #endregion
    }
}