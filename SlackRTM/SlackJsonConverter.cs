using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SlackRTM.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SlackRTM
{
    class SlackJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType.Assembly == Assembly.GetExecutingAssembly())
                return true;
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Event))
                existingValue= GenerateEvent(ref reader);
            if (existingValue == null)
                existingValue = Activator.CreateInstance(objectType, true);
            serializer.ContractResolver = new TrueCamelCasePropertyNamesContractResolver();
            serializer.Populate(reader, existingValue);
            return existingValue;
        }

        private static object GenerateEvent(ref JsonReader reader)
        {
            JToken obj = JObject.ReadFrom(reader);
            reader = obj.CreateReader();
            if (obj["ok"] != null)
                return new Ack();
            switch (obj["type"].ToString())
            {
                case "hello":
                    return new Hello();
                case "message":
                    return new Message();
                default:
                    return new UnknownEvent(obj as JObject);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            JObject jo = new JObject();
            
            foreach (PropertyInfo prop in value.GetType().GetProperties())
            {
                if (prop.CanRead)
                {
                    object propValue = prop.GetValue(value, null);
                    if (propValue != null)
                    {
                        jo.Add(prop.Name.ToUnderscoreLower(), JToken.FromObject(propValue, serializer));
                    }
                }
            }
            jo.WriteTo(writer);
        }

        public class TrueCamelCasePropertyNamesContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.FromUnderscoreLower();
            }

        }

    }
}
