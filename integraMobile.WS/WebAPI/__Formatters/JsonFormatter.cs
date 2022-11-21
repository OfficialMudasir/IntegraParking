using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using fastJSON;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.WS.WebAPI
{
    /// <summary>Fast JSON Media type formatter.</summary>
    /// <seealso cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/>
    public class JsonFormatter : MediaTypeFormatter
    {
        private static readonly CLogWrapper Log = new CLogWrapper(typeof(JsonFormatter));

        /// <summary>Options for controlling the operation.</summary>
        private static JSONParameters _parameters;

        /// <summary>Default constructor.</summary>
        public JsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(false, true));

            if (_parameters != null)
                return;

            _parameters = new JSONParameters
            {
                InlineCircularReferences = true,
                EnableAnonymousTypes = true,
                UseFastGuid = true,
                UseExtensions = false,
                UsingGlobalTypes = false,
                SerializeNullValues = false,
                ShowReadOnlyProperties = true
            };
        }

        /// <summary>Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />
        ///  can deserializean object of the specified type.</summary>
        /// <param name="type">The type to deserialize.</param>
        /// <returns>true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can
        ///  deserialize the type; otherwise, false.</returns>
        /// <seealso cref="M:System.Net.Http.Formatting.MediaTypeFormatter.CanReadType(Type)"/>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />
        ///  can serializean object of the specified type.</summary>
        /// <param name="type">The type to serialize.</param>
        /// <returns>true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can
        ///  serialize the type; otherwise, false.</returns>
        /// <seealso cref="M:System.Net.Http.Formatting.MediaTypeFormatter.CanWriteType(Type)"/>
        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>Asynchronously deserializes an object of the specified type.</summary>
        /// <param name="type">           The type of the object to deserialize.</param>
        /// <param name="readStream">     The <see cref="T:System.IO.Stream" /> to read.</param>
        /// <param name="content">        The <see cref="T:System.Net.Http.HttpContent" />, if available.
        ///  It may be null.</param>
        /// <param name="formatterLogger">The
        ///  <see cref="T:System.Net.Http.Formatting.IFormatterLogger" />
        ///  to log events to.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> whose _result will be an object of the
        ///  given type.</returns>
        /// <seealso cref="M:System.Net.Http.Formatting.MediaTypeFormatter.ReadFromStreamAsync(Type,Stream,HttpContent,IFormatterLogger)"/>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            //string sContent = new StreamReader(readStream).ReadToEnd();
            //Log.LogMessage(LogLevels.logINFO, string.Format("ReadFromStreamAsync (type={0},content={1})", type?.FullName, sContent));
            var task = Task<object>.Factory.StartNew(() =>
            {
                string sContent = new StreamReader(readStream).ReadToEnd();
                Log.LogMessage(LogLevels.logINFO, string.Format("ReadFromStreamAsync (type={0},content={1})", type?.FullName, sContent));
                return JSON.ToObject(sContent, type);
            });
            return task;
        }

        /// <summary>Asynchronously writes an object of the specified type.</summary>
        /// <param name="type">            The type of the object to write.</param>
        /// <param name="value">           The object value to write.  It may be null.</param>
        /// <param name="writeStream">     The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">         The <see cref="T:System.Net.Http.HttpContent" /> if
        ///  available. It may be null.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if
        ///  available. It may be null.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.</returns>
        /// <seealso cref="M:System.Net.Http.Formatting.MediaTypeFormatter.WriteToStreamAsync(Type,object,Stream,HttpContent,TransportContext)"/>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var json = JSON.ToJSON(value, _parameters);
                Log.LogMessage(LogLevels.logINFO, string.Format("WriteToStreamAsync (type={0},content={1})", type?.FullName, json));
                using (var w = new StreamWriter(writeStream)) w.Write(json);
            });

            return task;
        }
    }
}