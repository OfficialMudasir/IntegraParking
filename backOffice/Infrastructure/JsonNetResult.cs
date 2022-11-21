using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace integraMobile.Infrastructure
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified
            };
        }

        public Newtonsoft.Json.JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var scriptSerializer = Newtonsoft.Json.JsonSerializer.Create(this.Settings);
            
            using (var sw = new System.IO.StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }

    public class JSONCustomDateConverter : Newtonsoft.Json.Converters.JavaScriptDateTimeConverter //.DateTimeConverterBase
    {
        public JSONCustomDateConverter()
        {
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
            writer.WriteValue(Convert.ToDateTime(value));
            writer.Flush();
        }
    }

}